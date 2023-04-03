using System.Collections.Immutable;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;

namespace PrettyDocComments;

internal sealed class DocCommentRenderer
{
    private static readonly Typeface _boldTypeFace;
    private static readonly ImmutableHashSet<string> _topLevelElements = new[] {
        "example", "exception", "include", "param", "permission", "remarks", "returns",
        "seealso", "summary", "typeparam", "value"
    }.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);

    private readonly double _pixelsPerDip;
    private readonly double _editorFontEmSize;
    private readonly double _normalEmSize;
    private readonly Pen _separatorPen;

    // Render context 
    private double _x, _y, _indent;

    static DocCommentRenderer()
    {
        var textFontFamily = new FontFamily("Segoe UI");
        var fallbackTextFontFamily = new FontFamily("Arial");

        _boldTypeFace = new Typeface(textFontFamily, FontStyles.Normal, FontWeights.Bold, FontStretches.Normal,
            fallbackTextFontFamily);
    }

    public DocCommentRenderer(double pixelsPerDip, double editorFontEmSize, Pen separatorPen)
    {
        _separatorPen = separatorPen;
        _editorFontEmSize = editorFontEmSize;
        _pixelsPerDip = pixelsPerDip;
        _normalEmSize = 0.9 * _editorFontEmSize;
    }

    public void Render(DrawingContext dc, Rect bounds, IEnumerable<XNode> nodes)
    {
        _y = 2;
        int blockNo = 0;
        foreach (XElement el in WrapTopLevelText(nodes)) {
            blockNo++;
            _x = 3;
            _indent = 3;
            if (blockNo > 1 && !IsBetweenFriendBlocks(el)) {
                _y += 2;
                dc.DrawLine(_separatorPen, new(0, _y), new(bounds.Width, _y));
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
                        el.AddFirst(new XElement("b", "Exception: "));
                    } else {
                        el.AddFirst(new XElement("b", new XElement("c", exeptionName), ": "));
                    }
                    RenderBlock(dc, el);
                    break;
                default:
                    RenderBlock(dc, el);
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

    private void RenderBlockWithCaption(DrawingContext dc, XElement element, string caption)
    {
        _indent += _editorFontEmSize;
        FormattedText formattedText = BoldText(caption);
        dc.DrawText(formattedText, new Point(_x, _y));
        _x += formattedText.Width + _editorFontEmSize;
        double oldY = _y;
        RenderBlock(dc, element);
        if (_y == oldY) {
            _y += formattedText.Height;
        }
    }

    private void RenderBlockWithTitle(DrawingContext dc, XElement element, string title)
    {
        FormattedText formattedText = BoldText(title);
        dc.DrawText(formattedText, new Point(_x, _y));
        _y += formattedText.Height;
        RenderBlock(dc, element);
    }

    private void RenderBlock(DrawingContext dc, XElement element)
    {
        var children = element.Nodes().ToList();
        if (children.Count > 0) {
            if (children[0] is XText first) {
                first.Value = first.Value.TrimStart();
            }
            if (children[children.Count - 1] is XText last) {
                last.Value = last.Value.TrimEnd();
            }
            RenderFormatted(dc, element);
        }
    }

    private void RenderFormatted(DrawingContext dc, XElement element)
    {
        var accumulator = new FormatAccumulator(_normalEmSize, _pixelsPerDip);
        FormatParser.Instance.ParseElement(accumulator, element);
        FormattedText formattedText = accumulator.GetFormattedText();
        dc.DrawText(formattedText, new Point(Math.Max(_indent, _x), _y));
        _y += formattedText.Height;
    }

    private FormattedText BoldText(string text) =>
        new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
            _boldTypeFace, _normalEmSize, Brushes.Black, _pixelsPerDip);
}
