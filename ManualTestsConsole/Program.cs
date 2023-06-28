using System.Xml.Linq;

string input = """
    <root>
    <code>
    AAAA
        BBBB
    CCCC
    </code>
    </root>
    """;

var doc = XDocument.Parse(input);
if (doc?.Root is { } root) {
    foreach (var node in root.Nodes()) {
        Write(node, 0);
    }
}
Console.ReadKey();

static void Write(XNode node, int indent)
{
    switch (node) {
        case XElement el:
            Console.WriteLine(new string(' ', indent) + node.ToString());
            foreach (var subnode in el.Nodes()) {
                Write(subnode, indent + 4);
            }
            break;
        case XText text:
            Console.WriteLine(new string(' ', indent) + "\"" + text.Value + "\"");
            break;
    }
}