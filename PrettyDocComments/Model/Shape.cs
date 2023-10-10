using System.Windows;
using System.Windows.Media;
using PrettyDocComments.Helpers;

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

    public bool HasContinuationSymbol { get; set; }

    public abstract void Draw(DrawingContext dc, double commentWidth);

    protected void DrawContinuationSymbol(DrawingContext dc, double commentWidth)
    {
        const double Width = 6, Height = 6, RightMargin = 3;

        var point1 = new Point(commentWidth - RightMargin - Width, Bottom - Height);
        var point2 = new Point(commentWidth - RightMargin, Bottom - Height / 2);
        var point3 = new Point(commentWidth - RightMargin - Width, Bottom);
        dc.DrawLine(Options.CommentOutline, point1, point2);
        dc.DrawLine(Options.CommentOutline, point2, point3);
    }
}
