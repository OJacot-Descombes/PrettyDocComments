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
            try { // If the XML contains a <!DOCTYPE html> it is probably rooted already and this may work:
                var doc = XDocument.Parse(unrootedXml);
                nodes = doc.Root.Nodes();
                return true;
            } catch {
            }
            LastXmlException = ex;
            nodes = Enumerable.Empty<XNode>();
            return true; // Return true to have error rendered.
        } catch {
            nodes = null;
            return false;
        }
    }

    public static XmlException LastXmlException { get; private set; }
}
