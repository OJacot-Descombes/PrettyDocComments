using System.Windows;
using System.Windows.Media;
using PrettyDocComments.Helpers;

namespace PrettyDocComments.Model;

internal abstract class Shape(Point origin, double height)
{
    protected readonly Point _origin = origin;
    protected readonly double _height = height;

    public double Bottom => _height + _origin.Y;

    public bool HasContinuationSymbol { get; set; }

    public abstract void Draw(DrawingContext dc, double commentWidth);

    protected void DrawContinuationSymbol(DrawingContext dc, double commentWidth)
    {
        const double CWidth = 6, CHeight = 6, RightMargin = 3;

        double bottom = Bottom;
        var point1 = new Point(commentWidth - RightMargin - CWidth, bottom - CHeight);
        var point2 = new Point(commentWidth - RightMargin, bottom - CHeight / 2);
        var point3 = new Point(commentWidth - RightMargin - CWidth, bottom);
        dc.DrawLine(Options.CommentOutline, point1, point2);
        dc.DrawLine(Options.CommentOutline, point2, point3);
    }
}
