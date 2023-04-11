using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using PrettyDocComments.Model;

namespace PrettyDocComments;

internal sealed class DocCommentRenderer
{
    private readonly Composer _composer;

    public DocCommentRenderer(double pixelsPerDip, double editorFontEmSize)
    {
        _composer = new Composer(pixelsPerDip, editorFontEmSize, 2.0);
    }

    public void Render(DrawingContext dc, Rect bounds, IEnumerable<XNode> nodes)
    {
        var shapes = _composer.Parse(nodes, bounds.Width);
        foreach (var shape in shapes) {
            shape.Draw(dc);
        }
    }
}
