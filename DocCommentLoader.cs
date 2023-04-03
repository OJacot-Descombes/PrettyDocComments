using System.Xml.Linq;

namespace PrettyDocComments;

internal sealed class DocCommentLoader
{
    public static readonly DocCommentLoader Instance = new DocCommentLoader();

    private DocCommentLoader() { }

    public bool TryLoad(string comment, out IEnumerable<XNode> nodes)
    {
        try {
            var doc = XDocument.Parse("<__root__>" + comment + "</__root__>");
            nodes = doc.Root.Nodes();
            return true;
        } catch {
            nodes = null;
            return false;
        }
    }
}
