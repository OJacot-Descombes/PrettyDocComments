using System.Collections.Immutable;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using PrettyDocComments.Helpers;
using PrettyDocComments.Model;

namespace PrettyDocComments.Services;

internal sealed class ShapeParser
{
    private const char NonBreakingSpace = '\u00A0';
    private const string SubtitleIdentSpaces = "      ";

    private static readonly ImmutableHashSet<string> _topLevelElements = new[] {
        "example", "exception", "param", "permission", "remarks", "returns",
        "seealso", "summary", "typeparam", "value"
    }.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

    private readonly IWpfTextView _view;
    private double _emSize;
    private double _initialY;
    private double _width;

    private double _y;
    private List<Shape> _shapes;

    public ShapeParser(IWpfTextView view)
    {
        _view = view;
    }

    private void Initialize()
    {
        _initialY = Options.Padding.Top;
        _emSize = Options.GetNormalEmSize(_view);
        _width = Options.CommentWidthInColumns * _view.FormattedLineSource.ColumnWidth;
    }

    /// <summary>
    /// Parses the XML nodes and returns the shapes the adorner is made of.
    /// </summary>
    /// <param name="comment">Comment with the XML nodes of the doc comment.</param>
    /// <returns>Comment with list of shapes forming the content of the adorner.</returns>
    /// <remarks>The background rectangle of the adorner is not returned.</remarks>
    public Comment<List<Shape>> Parse(Comment<IEnumerable<XNode>> comment)
    {
        Initialize();
        _shapes = new List<Shape>();
        _y = _initialY;
        int blockNo = 0;
        string previousTagName = null;
        foreach (XElement el in WrapTopLevelText(comment.Data)) {
            blockNo++;
            string tagName = el.Name.LocalName.ToLowerInvariant();
            if (blockNo > 1 && !IsBetweenFriendBlocks(el)) {
                AddSeparator(previousTagName, tagName);
            }
            switch (tagName) {
                case "example":
                    ParseBlockWithTitle(el, "Example", 1.7 * _emSize);
                    break;
                case "exception":
                    if (previousTagName != tagName) {
                        AddMainTitle("Exceptions");
                    }
                    string exeptionName = el.Attributes("cref").FirstOrDefault()?.Value;
                    if (String.IsNullOrWhiteSpace(exeptionName)) {
                        exeptionName = "Exception";
                    }
                    ParseBlockWithInlineHeading(el, SubtitleIdentSpaces + exeptionName + ":");
                    break;
                case "param":
                    if (previousTagName != tagName) {
                        AddMainTitle("Parameters");
                    }

                    string name = el.Attributes("name").FirstOrDefault()?.Value;
                    string title = SubtitleIdentSpaces + (name is { Length: > 0 } ? name : tagName);
                    ParseBlockWithInlineHeading(el, title + ":");
                    break;
                case "typeparam":
                    if (previousTagName != tagName) {
                        AddMainTitle("Type parameters");
                    }
                    name = el.Attributes("name").FirstOrDefault()?.Value;
                    title = SubtitleIdentSpaces + (name is { Length: > 0 } ? name : tagName);
                    ParseBlockWithInlineHeading(el, title + ":");
                    break;
                case "returns":
                    ParseBlockWithInlineHeading(el, "Returns", 1, 0.0);
                    break;
                case "remarks":
                    ParseBlockWithTitle(el, "Remarks");
                    break;
                case "seealso":
                    if (previousTagName != tagName) {
                        AddMainTitle("See also");
                    }
                    ParseSeealsoBlock(el);
                    break;
                case "summary":
                    ParseBlock(el, Options.Padding.Left, 1.1);
                    break;
                default:
                    ParseBlock(el, Options.Padding.Left);
                    break;
            }
            previousTagName = el.Name.LocalName;
        }

        static bool IsBetweenFriendBlocks(XNode node) =>
            node.PreviousNode is XElement { Name.LocalName: var p } &&
            node is XElement { Name.LocalName: var e } &&
            p == e && p is "param" or "typeparam" or "exception" or "seealso";

        return comment.ConvertTo(_shapes);
    }

    private static IEnumerable<XElement> WrapTopLevelText(IEnumerable<XNode> nodes)
    {
        XElement topLevelText = null;
        bool hasText = false;
        foreach (XNode node in nodes) {
            if (node is XElement el && _topLevelElements.Contains(el.Name.LocalName)) {
                if (topLevelText is not null) {
                    if (hasText) {
                        yield return topLevelText;
                    }
                    topLevelText = null;
                    hasText = false;
                }
                yield return el;
            } else {
                topLevelText ??= new XElement("_text_");
                topLevelText.Add(node);
                hasText |= node is not XText txt || !String.IsNullOrWhiteSpace(txt.Value);
            }
        }
        if (hasText) {
            yield return topLevelText;
        }
    }

    private void AddSeparator(string previousTagName, string tagName)
    {
        Pen commentSeparator = tagName is "summary" or "value" || previousTagName is "summary" or "value"
            ? Options.BoldCommentSeparator
            : Options.CommentSeparator;
        _y += 4.0;
        _shapes.Add(new HorizontalLineShape(commentSeparator, new(0, _y), 4.0));
        _y += 2.0;
    }

    private void AddMainTitle(string title)
    {
        FormattedText formattedText = CaptionText(title);
        _shapes.Add(new TextShape(formattedText, new Point(Options.Padding.Left, _y)));
        _y += formattedText.Height;
    }

    private void ParseBlockWithInlineHeading(XElement element, string caption, int indent = 4, double minSpace = 42.0)
    {
        FormattedText formattedCaption = CaptionText(caption);
        _shapes.Add(new TextShape(formattedCaption, new Point(Options.Padding.Left, _y)));
        double blockIndent = indent * _emSize;
        int firstLineIndentChars = (int)(3.7 * Math.Max(formattedCaption.Width - blockIndent, minSpace) / _emSize) + 4;
        element.AddFirst(new string(NonBreakingSpace, firstLineIndentChars));
        ParseBlock(element, Options.Padding.Left + blockIndent);
    }

    private void ParseBlockWithTitle(XElement element, string title, double blockIndent = 0.0)
    {
        AddMainTitle(title);
        ParseBlock(element, Options.Padding.Left + blockIndent);
    }

    private void ParseBlock(XElement element, double indent, double fontAspect = 1.0)
    {
        var parser = new FormatParser(indent, _emSize, _width, fontAspect, _view);
        foreach (TextBlock textBlock in parser.Parse(element)) {
            if (textBlock.BackgroundType is BackgroundType.Default) {
                _shapes.Add(new TextShape(textBlock.Text, new Point(textBlock.Left, _y)));
            } else {
                bool hasText = textBlock.Text.Text is { Length: > 0 };
                _shapes.Add(new RectangleShape(
                    textBlock.Fill, textBlock.Stroke,
                    origin: new Point(textBlock.Left, _y),
                    width: textBlock.Text.MaxTextWidth + Options.Padding.GetWidth(),
                    height: textBlock.Height));
                if (hasText) {
                    _shapes.Add(
                        new TextShape(
                            textBlock.Text,
                            new Point(textBlock.Left + Options.Padding.Left, _y + Options.Padding.Top))
                    );
                }
            }
            _y += textBlock.DeltaY;
        }
    }

    private void ParseSeealsoBlock(XElement el)
    {
        double indent = Options.Padding.Left + 1.7 * _emSize;
        double width = _width - indent - Options.Padding.Right;
        if (String.IsNullOrWhiteSpace(el.Value)) { // There is no text, just add the attribute values(s).
            string text = "● " + String.Join(" ", el.Attributes().Select(a => a.Value));
            FormattedText formattedText = text.AsFormatted(Options.NormalTypeFace, width, _view);
            _shapes.Add(new TextShape(formattedText, new Point(indent, _y)));
            _y += formattedText.Height;
        } else {
            FormattedText formattedText = "●".AsFormatted(Options.NormalTypeFace, width, _view);
            _shapes.Add(new TextShape(formattedText, new Point(indent, _y)));
            ParseBlock(el, 1.3 * _emSize);
        }
    }

    private FormattedText CaptionText(string text) => text.AsFormatted(Options.CaptionsTypeFace, _width, _view);
}
