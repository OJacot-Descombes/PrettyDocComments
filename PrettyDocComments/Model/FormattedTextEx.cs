using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace PrettyDocComments.Model;

internal class FormattedTextEx : FormattedText
{
    public FormattedTextEx(string textToFormat, CultureInfo culture, FlowDirection flowDirection, Typeface typeface, double emSize, Brush foreground, double pixelsPerDip)
        : base(textToFormat, culture, flowDirection, typeface, emSize, foreground, pixelsPerDip)
    {
    }

    private List<HighlightRun> _highlightRuns;
    public List<HighlightRun> HighlightRuns => _highlightRuns;

    public void SetHighlightBrush(Brush highlightBrush, int startIndex, int count)
    {
        if (highlightBrush == null) {
            return;
        }
        _highlightRuns ??= new();
        _highlightRuns.Add(new HighlightRun(highlightBrush, startIndex, count));
    }

    public readonly struct HighlightRun
    {
        public HighlightRun(Brush highlightBrush, int startIndex, int count)
        {
            HighlightBrush = highlightBrush;
            StartIndex = startIndex;
            Count = count;
        }

        public readonly Brush HighlightBrush;
        public readonly int StartIndex;
        public readonly int Count;
    }
}
