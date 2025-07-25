using System.Collections.Immutable;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using PrettyDocComments.CustomOptions;
using PrettyDocComments.Helpers;
using PrettyDocComments.Model;

namespace PrettyDocComments.Services;

internal sealed class ShapeParser(IWpfTextView view)
{
    private const char NonBreakingSpace = '\u00A0';
    private const string SubtitleIdentSpaces = "      ";
    private const int MinSpace = 25;

    private static readonly ImmutableHashSet<string> _topLevelElements = new[] {
        "example", "exception", "param", "permission", "remarks", "returns",
        "seealso", "summary", "typeparam", "value"
    }.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);
    private double _emSize;
    private double _initialY;
    private double _width;

    private double _y;
    private List<Shape> _shapes;

    private void Initialize()
    {
        _initialY = Options.Padding.Top;
        _emSize = Options.GetNormalEmSize(view);
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
        _width = Options.GetCommentWidthInPixels(view, comment.CommentLeftCharIndex);
        _shapes = [];
        _y = _initialY;
        int blockNo = 0;
        string previousTagName = null;
        double minSpace = MinSpace;
        foreach (XElement el in WrapTopLevelText(comment.Data)) {
            if (previousTagName is "summary" && GeneralOptions.Instance.CollapseToSummary) {
                if (_shapes.LastOrDefault(s => s is not HorizontalLineShape) is { } lastShape) {
                    lastShape.HasContinuationSymbol = true;
                }
                return comment.ConvertTo(_shapes);
            }
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
                    AlignBlockWithInlineHeading(el, tagName, "Exceptions", GetExecptionCref);
                    break;
                case "param":
                    AlignBlockWithInlineHeading(el, tagName, "Parameters", GetName);
                    break;
                case "typeparam":
                    AlignBlockWithInlineHeading(el, tagName, "Type parameters", GetName);
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
        return comment.ConvertTo(_shapes);


        static bool IsBetweenFriendBlocks(XNode node) =>
            node.PreviousNode is XElement { Name.LocalName: var p } &&
            node is XElement { Name.LocalName: var e } &&
            p == e && p is "param" or "typeparam" or "exception" or "seealso";

        void GetTitleInfo(XElement el, string tagName, int indent, Func<XElement, string> getName,
            out string title, out double width)
        {
            string name = getName(el);
            title = SubtitleIdentSpaces + (name is { Length: > 0 } ? name : tagName) + ":";

            FormattedTextEx formattedCaption = CaptionText(title);
            double blockIndent = indent * _emSize;
            width = formattedCaption.Width - blockIndent;
        }

        static string GetName(XElement el) => el.Attributes("name").FirstOrDefault()?.Value;
        static string GetExecptionCref(XElement el)
        {
            string name = el.Attributes("cref").FirstOrDefault()?.Value;
            if (String.IsNullOrWhiteSpace(name)) {
                name = "Exception";
            }
            return name;
        }

        void AlignBlockWithInlineHeading(XElement el, string tagName, string mainTitle, Func<XElement, string> getName)
        {
            double width;
            if (previousTagName != tagName) {
                AddMainTitle(mainTitle);
                var allParams = el.ElementsAfterSelf()
                    .TakeWhile(e => e.Name.LocalName.ToLowerInvariant() == tagName);
                minSpace = MinSpace;
                foreach (XElement param in allParams) {
                    GetTitleInfo(param, tagName, 4, getName, out _, out width);
                    minSpace = Math.Max(minSpace, width);
                }
            }

            GetTitleInfo(el, tagName, 4, getName, out string title, out width);
            minSpace = Math.Max(minSpace, width);
            ParseBlockWithInlineHeading(el, title, minSpace: minSpace);
        }
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
        FormattedTextEx formattedText = CaptionText(title);
        _shapes.Add(new TextShape(formattedText, new Point(Options.Padding.Left, _y)));
        _y += formattedText.Height;
    }

    private void ParseBlockWithInlineHeading(XElement element, string caption, int indent = 4, double minSpace = 42.0)
    {
        FormattedTextEx formattedCaption = CaptionText(caption);
        _shapes.Add(new TextShape(formattedCaption, new Point(Options.Padding.Left, _y)));
        double blockIndent = indent * _emSize;
        int firstLineIndentChars = (int)(3.7 * Math.Max(formattedCaption.Width - blockIndent, minSpace) / _emSize) + 4;
        if (element.FirstNode is XText text) {
            text.Value = text.Value.TrimStart();
        }
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
        var parser = new FormatParser(indent, _emSize, _width, fontAspect, view);
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
            FormattedTextEx formattedText = text.AsFormatted(Options.NormalTypeFace, width, view);
            _shapes.Add(new TextShape(formattedText, new Point(indent, _y)));
            _y += formattedText.Height;
        } else {
            FormattedTextEx formattedText = "●".AsFormatted(Options.NormalTypeFace, width, view);
            _shapes.Add(new TextShape(formattedText, new Point(indent, _y)));
            ParseBlock(el, 3.0 * _emSize);
        }
    }

    private FormattedTextEx CaptionText(string text) => text.AsFormatted(Options.CaptionsTypeFace, _width, view);
}
