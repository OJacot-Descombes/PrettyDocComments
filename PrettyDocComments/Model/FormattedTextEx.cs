using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace PrettyDocComments.Model;

internal class FormattedTextEx(string textToFormat, CultureInfo culture, FlowDirection flowDirection, 
        Typeface typeface, double emSize, Brush foreground, double pixelsPerDip
    ) : FormattedText(textToFormat, culture, flowDirection, typeface, emSize, foreground, pixelsPerDip)
{
    private List<HighlightRun> _highlightRuns;

    public void SetHighlightBrush(Brush highlightBrush, int startIndex, int count)
    {
        if (highlightBrush == null) {
            return;
        }
        _highlightRuns ??= [];
        _highlightRuns.Add(new HighlightRun(highlightBrush, startIndex, count));
    }

    public void Draw(DrawingContext dc, Point origin)
    {
        if (_highlightRuns != null) {
            foreach (var run in _highlightRuns) {
                var gemometry = BuildHighlightGeometry(origin, run.StartIndex, run.Count);
                dc.DrawGeometry(run.HighlightBrush, null, gemometry);
            }
        }
        dc.DrawText(this, origin);
    }

    private readonly struct HighlightRun(Brush highlightBrush, int startIndex, int count)
    {
        public readonly Brush HighlightBrush = highlightBrush;
        public readonly int StartIndex = startIndex;
        public readonly int Count = count;
    }
}
