using System.Windows.Media;
using System.Xml.Linq;

namespace PrettyDocComments.Model;

internal class XBrush : XElement
{
    public Brush Brush { get; }

    public XBrush(XName name, Brush brush, object content)
        : base(name, content)
    {
        Brush = brush;
    }
}
