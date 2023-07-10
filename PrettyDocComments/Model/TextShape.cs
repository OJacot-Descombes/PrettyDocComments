using System.Windows;
using System.Windows.Media;

namespace PrettyDocComments.Model;

internal sealed class TextShape : Shape
{
    public TextShape(FormattedTextEx text, Point origin)
        : base(origin, text.Height)
    {
        _text = text;
    }

    private readonly FormattedTextEx _text;

    public override void Draw(DrawingContext dc, double commentWidth)
    {
        if (_text.HighlightRuns != null) {
            foreach (var run in _text.HighlightRuns) {
                var gemometry = _text.BuildHighlightGeometry(_origin, run.StartIndex, run.Count);
                dc.DrawGeometry(run.HighlightBrush, null, gemometry);
            }
        }
        dc.DrawText(_text, _origin);
    }
}
