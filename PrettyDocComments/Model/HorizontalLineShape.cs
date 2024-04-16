using System.Windows;
using System.Windows.Media;

namespace PrettyDocComments.Model;

internal sealed class HorizontalLineShape(Pen pen, Point origin, double height) : Shape(origin, height)
{
    public override void Draw(DrawingContext dc, double commentWidth)
    {
        dc.DrawLine(pen, _origin, new Point(_origin.X + commentWidth, _origin.Y));
    }
}
