using System.Windows;
using System.Windows.Media;

namespace PrettyDocComments.Model;

internal sealed class TextShape(FormattedTextEx text, Point origin) : Shape(origin, text.Height)
{
    public override void Draw(DrawingContext dc, double commentWidth)
    {
        text.Draw(dc, _origin);
        if (HasContinuationSymbol) {
            DrawContinuationSymbol(dc, commentWidth);
        }
    }
}
