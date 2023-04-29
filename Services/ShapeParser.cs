using PrettyDocComments.Helpers;
using PrettyDocComments.Model;
using System.Xml.Linq;
using System.Collections.Immutable;
using System.Globalization;
using System.Windows.Media;
using System.Windows;

namespace PrettyDocComments.Services;

internal sealed class ShapeParser
{
    private const double BaseIndent = 3.0;
    private const char NonBreakingSpace = '\u00A0';
    private const string SubtitleIdentSpaces = "    ";

    private static readonly ImmutableHashSet<string> _topLevelElements = new[] {
        "example", "exception", "include", "param", "permission", "remarks", "returns",
        "seealso", "summary", "typeparam", "value"
    }.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

    private readonly double _pixelsPerDip;
    private readonly double _normalEmSize;
    private readonly double _initialY;

    private double _y;
    private List<Shape> _shapes;

    public ShapeParser(double pixelsPerDip, double editorFontEmSize, double initialY = 2)
    {
        _initialY = initialY;
        _pixelsPerDip = pixelsPerDip;
        _normalEmSize = 0.8 * editorFontEmSize;
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
                AddSeparator(comment, previousTagName, tagName);
            }
            switch (tagName) {
                case "param":
                    if (previousTagName != tagName) {
                        AddMainTitle("Parameters");
                    }
                    string title = SubtitleIdentSpaces + el.Attributes("name").FirstOrDefault()?.Value ?? tagName;
                    ParseBlockWithCaption(el, title + ":");
                    break;
                case "typeparam":
                    if (previousTagName != tagName) {
                        AddMainTitle("Type parameters");
                    }
                    title = SubtitleIdentSpaces + el.Attributes("name").FirstOrDefault()?.Value ?? tagName;
                    ParseBlockWithCaption(el, title + ":");
                    break;
                case "returns":
                    ParseBlockWithCaption(el, "Returns");
                    break;
                case "remarks":
                    ParseBlockWithTitle(el, "Remarks");
                    break;
                case "example":
                    ParseBlockWithTitle(el, "Example", _normalEmSize);
                    break;
                case "exception":
                    if (previousTagName != tagName) {
                        AddMainTitle("Exceptions");
                    }
                    string exeptionName = el.Attributes("cref").FirstOrDefault()?.Value;
                    if (String.IsNullOrWhiteSpace(exeptionName)) {
                        exeptionName = "Exception";
                    }
                    ParseBlockWithCaption(el, SubtitleIdentSpaces + exeptionName + ":");
                    break;
                default:
                    ParseBlock(el, BaseIndent);
                    break;
            }
            previousTagName = el.Name.LocalName;
        }

        static bool IsBetweenFriendBlocks(XNode node) =>
            node.PreviousNode is XElement { Name.LocalName: var p } &&
            node is XElement { Name.LocalName: var e } &&
            (
                p is "param" && e is "param" ||
                p is "typeparam" && e is "typeparam"
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

    private void AddSeparator(Comment<IEnumerable<XNode>> comment, string previousTagName, string tagName)
    {
        Pen commentSeparator = tagName == "summary" || previousTagName == "summary"
            ? Options.BoldCommentSeparator
            : Options.CommentSeparator;
        _y += 4.0;
        _shapes.Add(new LineShape(commentSeparator, new(0, _y), new(comment.Width, _y), 4.0));
        _y += 2.0;
    }

    private void AddMainTitle(string title)
    {
        FormattedText formattedText = CaptionText(title);
        double deltaY = formattedText.Height;
        _shapes.Add(new TextShape(formattedText, new Point(BaseIndent, _y), deltaY));
        _y += deltaY;
    }

    private void ParseBlockWithCaption(XElement element, FormattedText formattedCaption)
    {
        _shapes.Add(new TextShape(formattedCaption, new Point(BaseIndent, _y), deltaY: 0));
        double blockIndent = 3 * _normalEmSize;
        int firstLineIndentChars = (int)(3.7 * Math.Max(formattedCaption.Width - blockIndent, 42) / _normalEmSize) + 4;
        element.AddFirst(new string(NonBreakingSpace, firstLineIndentChars));
        ParseBlock(element, BaseIndent + blockIndent);
    }

    private void ParseBlockWithCaption(XElement element, string caption)
    {
        ParseBlockWithCaption(element, CaptionText(caption));
    }

    private void ParseBlockWithTitle(XElement element, string title, double blockIndent = 0.0)
    {
        FormattedText formattedText = CaptionText(title);
        double deltaY = formattedText.Height;
        _shapes.Add(new TextShape(formattedText, new Point(BaseIndent, _y), deltaY));
        _y += deltaY;
        ParseBlock(element, BaseIndent + blockIndent);
    }

    private void ParseBlock(XElement element, double indent)
    {
        var children = element.Nodes().ToList();
        if (children.Count > 0) {
            if (children[0] is XText first) {
                first.Value = first.Value.TrimStart(' ', '\t', '\r', '\n', '\v'); // Exclude non-breaking space.
            }
            if (children[children.Count - 1] is XText last) {
                last.Value = last.Value.TrimEnd();
            }
            ParseFormatted(element, indent);
        }
    }

    private void ParseFormatted(XElement element, double indent)
    {
        var parser = new DocCommentParser(_normalEmSize, _pixelsPerDip, indent);
        foreach (TextBlock textBlock in parser.Parse(element)) {
            if (textBlock.Background != null) {
                const double Margin = 3.0;

                double deltaY = textBlock.Height + 2 * Margin;
                _shapes.Add(new RectangleShape(
                    textBlock.Background,
                    new Point(textBlock.Left, _y),
                    textBlock.Text.Width + 2 * Margin,
                    textBlock.Text.Height + 2 * Margin,
                    deltaY));
                _shapes.Add(new TextShape(textBlock.Text, new Point(textBlock.Left + Margin, _y + Margin), deltaY: 0));
                _y += deltaY;
            } else {
                double deltaY = textBlock.Height;
                _shapes.Add(new TextShape(textBlock.Text, new Point(textBlock.Left, _y), deltaY));
                _y += deltaY;
            }
        }
    }

    private FormattedText CaptionText(string text) =>
        new(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
            Options.CaptionsTypeFace, _normalEmSize, Brushes.Black, _pixelsPerDip);

    private FormattedText CodeCaptionText(string text) =>
        new(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
            Options.CodeCaptionTypeFace, _normalEmSize, Brushes.Black, _pixelsPerDip);

}
