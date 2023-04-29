using System.Windows;
using System.Windows.Media;

namespace PrettyDocComments.Model;

internal sealed class LineShape : Shape
{
    private readonly Pen _pen;
    private readonly Point _endpoint;

    public LineShape(Pen pen, Point origin, Point endpoint, double deltaY)
        : base(origin, deltaY)
    {
        _endpoint = endpoint;
        _pen = pen;
    }

    public override void Draw(DrawingContext dc)
    {
        dc.DrawLine(_pen, _origin, _endpoint);
    }
}
