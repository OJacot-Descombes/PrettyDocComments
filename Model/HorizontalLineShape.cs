using System.Windows;
using System.Windows.Media;

namespace PrettyDocComments.Model;

internal sealed class HorizontalLineShape : Shape
{
    private readonly Pen _pen;

    public HorizontalLineShape(Pen pen, Point origin, double height)
        : base(origin, height)
    {
        _pen = pen;
    }

    public override void Draw(DrawingContext dc, double commentWidth)
    {
        dc.DrawLine(_pen, _origin, new Point(_origin.X + commentWidth, _origin.Y));
    }
}
