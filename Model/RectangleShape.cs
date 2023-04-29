using System.Windows;
using System.Windows.Media;

namespace PrettyDocComments.Model;

internal sealed class RectangleShape : Shape
{
    private readonly Brush _brush;
    private readonly double _width, _height;

    public RectangleShape(Brush brush, Point origin, double width, double height, double deltaY)
        : base(origin, deltaY)
    {
        _brush = brush;
        _width = width;
        _height = height;
    }

    public override void Draw(DrawingContext dc)
    {
        dc.DrawRectangle(_brush, null, new Rect(_origin.X, _origin.Y, _width, _height));
    }
}
