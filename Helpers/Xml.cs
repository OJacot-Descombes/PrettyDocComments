using System.Xml.Linq;

namespace PrettyDocComments.Helpers;

internal static class Xml
{
    public static bool TryParseUnrootedNodes(string unrootedXml, out IEnumerable<XNode> nodes)
    {
        try {
            var doc = XDocument.Parse("<__root__>" + unrootedXml + "</__root__>");
            nodes = doc.Root.Nodes();
            return true;
        } catch {
            nodes = null;
            return false;
        }
    }
}
