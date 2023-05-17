using System.Diagnostics;
using System.Xml.Linq;
using Microsoft.VisualStudio.Utilities;
using PrettyDocComments.Helpers;

namespace PrettyDocComments.Model;

[DebuggerDisplay("Cell[TextBlocks = {TextBlocks?.Count}, _height = {_height}, Element = {Element}]")]
internal class Cell
{
    public XElement Element;
    public List<TextBlock> TextBlocks;

    private double? _height;
    public double Height => _height ??= TextBlocks.Sum(t => t.DeltaY) + Options.Padding.GetHeight();
}
