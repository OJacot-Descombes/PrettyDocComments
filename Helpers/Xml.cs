using System.Xml;
using System.Xml.Linq;

namespace PrettyDocComments.Helpers;

internal static class Xml
{
    public static bool TryParseUnrootedNodes(string unrootedXml, out IEnumerable<XNode> nodes)
    {
        LastXmlException = null;
        try {
            var doc = XDocument.Parse("<root>" + unrootedXml + "</root>");
            nodes = doc.Root.Nodes();
            return true;
        } catch (XmlException ex) {
            nodes = Enumerable.Empty<XNode>();
            LastXmlException = ex;
            return true;
        } catch {
            nodes = null;
            return false;
        }
    }

    public static XmlException LastXmlException { get; private set; }
}
