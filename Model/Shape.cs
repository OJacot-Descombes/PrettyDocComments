using System.Windows;
using System.Windows.Media;

namespace PrettyDocComments.Model;

internal abstract class Shape
{
    public Shape(Point origin, double height)
    {
        _origin = origin;
        Bottom = height + _origin.Y;
    }

    protected Point _origin;

    public double Bottom { get; }

    public abstract void Draw(DrawingContext dc, double commentWidth);
}
