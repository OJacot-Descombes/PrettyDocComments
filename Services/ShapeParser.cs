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
    private readonly double _emSize;
    private readonly double _initialY;
    private readonly double _width;

    private double _y;
    private List<Shape> _shapes;

    public ShapeParser(IWpfTextView view)
    {
        _view = view;
        _initialY = Options.Padding.Top;
        _emSize = view.FormattedLineSource.DefaultTextProperties.FontRenderingEmSize * Options.FontScaling;
        _width = Options.CommentWidthInColumns * view.FormattedLineSource.ColumnWidth;
    }

    /// <summary>
    /// Parses the XML nodes and returns the shapes the adorner is made of.
    /// </summary>
    /// <param name="comment">Comment with the XML nodes of the doc comment.</param>
    /// <returns>Comment with list of shapes forming the content of the adorner.</returns>
    /// <remarks>The background rectangle of the adorner is not returned.</remarks>
    public Comment<List<Shape>> Parse(Comment<IEnumerable<XNode>> comment)
    {
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
                    string title = SubtitleIdentSpaces + el.Attributes("name").FirstOrDefault()?.Value ?? tagName;
                    ParseBlockWithInlineHeading(el, title + ":");
                    break;
                case "typeparam":
                    if (previousTagName != tagName) {
                        AddMainTitle("Type parameters");
                    }
                    title = SubtitleIdentSpaces + el.Attributes("name").FirstOrDefault()?.Value ?? tagName;
                    ParseBlockWithInlineHeading(el, title + ":");
                    break;
                case "returns":
                    ParseBlockWithInlineHeading(el, "Returns");
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
                default:
                    ParseBlock(el, Options.Padding.Left);
                    break;
            }
            previousTagName = el.Name.LocalName;
        }

        static bool IsBetweenFriendBlocks(XNode node) =>
            node.PreviousNode is XElement { Name.LocalName: var p } &&
            node is XElement { Name.LocalName: var e } &&
            (
                p is "param" && e is "param" ||
                p is "typeparam" && e is "typeparam" ||
                p is "seealso" && e is "seealso"
            );


        return comment.ConvertTo(_shapes);
    }

    private static IEnumerable<XElement> WrapTopLevelText(IEnumerable<XNode> nodes)
    {
        XElement topLevelText = null;
        foreach (XNode node in nodes) {
            if (node is XElement el && _topLevelElements.Contains(el.Name.LocalName)) {
                if (topLevelText is not null) {
                    yield return topLevelText;
                    topLevelText = null;
                }
                yield return el;
            } else {
                topLevelText ??= new XElement("_text_");
                topLevelText.Add(node);
            }
        }
        if (topLevelText is not null) {
            yield return topLevelText;
        }
    }

    private void AddSeparator(string previousTagName, string tagName)
    {
        Pen commentSeparator = tagName is "summary" or "value" || previousTagName is "summary" or "value"
            ? Options.BoldCommentSeparator
            : Options.CommentSeparator;
        _y += 4.0;
        _shapes.Add(new LineShape(commentSeparator, new(0, _y), new(_width, _y), 4.0));
        _y += 2.0;
    }

    private void AddMainTitle(string title)
    {
        FormattedText formattedText = CaptionText(title);
        double deltaY = formattedText.Height;
        _shapes.Add(new TextShape(formattedText, new Point(Options.Padding.Left, _y), deltaY));
        _y += deltaY;
    }

    private void ParseBlockWithInlineHeading(XElement element, string caption)
    {
        FormattedText formattedCaption = CaptionText(caption);
        _shapes.Add(new TextShape(formattedCaption, new Point(Options.Padding.Left, _y), deltaY: 0));
        double blockIndent = 4 * _emSize;
        int firstLineIndentChars = (int)(3.7 * Math.Max(formattedCaption.Width - blockIndent, 42) / _emSize) + 4;
        element.AddFirst(new string(NonBreakingSpace, firstLineIndentChars));
        ParseBlock(element, Options.Padding.Left + blockIndent);
    }

    private void ParseBlockWithTitle(XElement element, string title, double blockIndent = 0.0)
    {
        AddMainTitle(title);
        ParseBlock(element, Options.Padding.Left + blockIndent);
    }

    private void ParseBlock(XElement element, double indent)
    {
        var parser = new DocCommentParser(indent, _view);
        foreach (TextBlock textBlock in parser.Parse(element)) {
            if (textBlock.Background != null) {
                double deltaY = textBlock.Height + Options.Padding.GetHeight();
                _shapes.Add(new RectangleShape(
                    textBlock.Background,
                    new Point(textBlock.Left, _y),
                    textBlock.Text.MaxTextWidth + Options.Padding.Right,
                    textBlock.Text.Height + Options.Padding.GetHeight(),
                    deltaY));
                _shapes.Add(
                    new TextShape(
                        textBlock.Text,
                        new Point(textBlock.Left + Options.Padding.Left, _y + Options.Padding.Top),
                        deltaY: 0)
                );
                _y += deltaY;
            } else {
                double deltaY = textBlock.Height;
                _shapes.Add(new TextShape(textBlock.Text, new Point(textBlock.Left, _y), deltaY));
                _y += deltaY;
            }
        }
    }

    private void ParseSeealsoBlock(XElement el)
    {
        if (String.IsNullOrWhiteSpace(el.Value)) { // There is no text, just add the attribute values(s).
            string text = "● " + String.Join(" ", el.Attributes().Select(a => a.Value));
            FormattedText formattedText = text.AsFormatted(Options.NormalTypeFace, 0.0, _view);
            _shapes.Add(new TextShape(
                formattedText, new Point(Options.Padding.Left, _y),
                formattedText.Height)
            );
            _y += formattedText.Height;
        } else {
            FormattedText formattedText = "●".AsFormatted(Options.NormalTypeFace, 0.0, _view);
            _shapes.Add(new TextShape(formattedText, new Point(Options.Padding.Left, _y), 0.0));
            ParseBlock(el, 1.3 * _emSize);
        }
    }

    private FormattedText CaptionText(string text) => text.AsFormatted(Options.CaptionsTypeFace, 0.0, _view);
}
