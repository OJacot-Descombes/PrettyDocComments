using System.Collections.Immutable;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace PrettyDocComments;

internal sealed class DocCommentRenderer
{
    private static readonly ImmutableHashSet<string> _topLevelElements = new[] {
        "example", "exception", "include", "param", "permission", "remarks", "returns",
        "seealso", "summary", "typeparam", "value"
    }.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

    private readonly double _pixelsPerDip;
    private readonly double _normalEmSize;

    // Render context 
    private const char NonBreakingSpace = '\u00A0';
    private const double BaseIndent = 3.0;
    private double _y;

    public DocCommentRenderer(double pixelsPerDip, double editorFontEmSize)
    {
        _pixelsPerDip = pixelsPerDip;
        _normalEmSize = 0.9 * editorFontEmSize;
    }

    public void Render(DrawingContext dc, Rect bounds, IEnumerable<XNode> nodes)
    {
        _y = 2;
        int blockNo = 0;
        foreach (XElement el in WrapTopLevelText(nodes)) {
            blockNo++;
            if (blockNo > 1 && !IsBetweenFriendBlocks(el)) {
                _y += 2;
                dc.DrawLine(Options.CommentSeparator, new(0, _y), new(bounds.Width, _y));
            }
            string tagName;
            tagName = el.Name.LocalName.ToLowerInvariant();
            switch (tagName) {
                case "param" or "typeparam":
                    string title = el.Attributes("name").FirstOrDefault()?.Value ?? tagName;
                    RenderBlockWithCaption(dc, el, title + ":");
                    break;
                case "returns":
                    RenderBlockWithCaption(dc, el, "Returns");
                    break;
                case "remarks":
                    RenderBlockWithTitle(dc, el, "Remarks");
                    break;
                case "example":
                    RenderBlockWithTitle(dc, el, "Example");
                    break;
                case "exception":
                    string exeptionName = el.Attributes("cref").FirstOrDefault()?.Value;
                    if (String.IsNullOrWhiteSpace(exeptionName)) {
                        RenderBlockWithCaption(dc, el, "Exception:");
                    } else {
                        RenderBlockWithCaption(dc, el, CodeCaptionText(exeptionName + ":"));
                    }
                    break;
                default:
                    RenderBlock(dc, el, BaseIndent);
                    break;
            }
        }

        static bool IsBetweenFriendBlocks(XNode node) =>
            node.PreviousNode is XElement { Name.LocalName: var p } &&
            node is XElement { Name.LocalName: var e } &&
            (
                p is "param" && e is "param" ||
                p is "typeparam" && e is "typeparam"
            );
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

    private void RenderBlockWithCaption(DrawingContext dc, XElement element, FormattedText formattedCaption)
    {
        dc.DrawText(formattedCaption, new Point(BaseIndent, _y));
        double blockIndent = 2 * _normalEmSize;
        int firstLineIndentChars = (int)(3.7 * Math.Max(formattedCaption.Width - blockIndent, 42) / _normalEmSize) + 4;
        element.AddFirst(new string(NonBreakingSpace, firstLineIndentChars));
        RenderBlock(dc, element, BaseIndent + blockIndent);
    }

    private void RenderBlockWithCaption(DrawingContext dc, XElement element, string caption)
    {
        RenderBlockWithCaption(dc, element, CaptionText(caption));
    }

    private void RenderBlockWithTitle(DrawingContext dc, XElement element, string title)
    {
        FormattedText formattedText = CaptionText(title);
        dc.DrawText(formattedText, new Point(BaseIndent, _y));
        _y += formattedText.Height;
        RenderBlock(dc, element, BaseIndent);
    }

    private void RenderBlock(DrawingContext dc, XElement element, double indent)
    {
        var children = element.Nodes().ToList();
        if (children.Count > 0) {
            if (children[0] is XText first) {
                first.Value = first.Value.TrimStart(' ', '\t', '\r', '\n', '\v'); // Exclude non-breaking space.
            }
            if (children[children.Count - 1] is XText last) {
                last.Value = last.Value.TrimEnd();
            }
            RenderFormatted(dc, element, indent);
        }
    }

    private void RenderFormatted(DrawingContext dc, XElement element, double indent)
    {
        var parser = new DocCommentParser(_normalEmSize, _pixelsPerDip, indent);
        foreach (TextBlock textBlock in parser.Parse(element)) {
            if (textBlock.Background != null) {
                const double Margin = 3.0;

                dc.DrawRectangle(textBlock.Background, null,
                    new Rect(textBlock.Left, _y, textBlock.Text.Width + 2 * Margin, textBlock.Text.Height + 2 * Margin));
                dc.DrawText(textBlock.Text, new Point(textBlock.Left + Margin, _y + Margin));
                _y += textBlock.Height + 2 * Margin;
            } else {
                dc.DrawText(textBlock.Text, new Point(textBlock.Left, _y));
                _y += textBlock.Height;
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
