using System.Windows;
using System.Windows.Media;

namespace PrettyDocComments.Model;

internal sealed class RectangleShape(Brush brush, Pen pen, Point origin, double width, double height)
    : Shape(origin, height)
{
    public override void Draw(DrawingContext dc, double commentWidth)
    {
        dc.DrawRectangle(brush, pen, new Rect(_origin.X, _origin.Y, width, _height));
        if (HasContinuationSymbol) {
            DrawContinuationSymbol(dc, commentWidth);
        }
    }
}
