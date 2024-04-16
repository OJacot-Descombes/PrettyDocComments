using System.Windows.Media;
using System.Xml.Linq;

namespace PrettyDocComments.Model;

internal class XBrush(XName name, Brush brush, object content) : XElement(name, content)
{
    public Brush Brush { get; } = brush;
}
