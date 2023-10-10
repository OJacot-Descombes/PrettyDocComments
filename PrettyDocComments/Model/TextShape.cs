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
        _text.Draw(dc, _origin);
        if (HasContinuationSymbol) {
            DrawContinuationSymbol(dc, commentWidth);
        }
    }
}
