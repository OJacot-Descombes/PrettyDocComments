using System.Xml;
using System.Xml.Linq;

namespace PrettyDocComments.Helpers;

internal static class Xml
{
    public static bool TryParseUnrootedNodes(string unrootedXml, out IEnumerable<XNode> nodes)
    {
        try {
            var doc = XDocument.Parse("<root>" + unrootedXml + "</root>");
            nodes = doc.Root.Nodes();
            return true;
        } catch {
            nodes = null;
            return false;
        }
    }
}
