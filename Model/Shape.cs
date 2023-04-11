using System.Windows;
using System.Windows.Media;

namespace PrettyDocComments.Model;

internal abstract class Shape
{
    public Shape(Point origin, double deltaY)
    {
        _origin = origin;
        DeltaY = deltaY;
    }

    protected Point _origin;

    public double DeltaY { get; }

    public abstract void Draw(DrawingContext dc);
}
