using System.Xml.Linq;
using Microsoft.VisualStudio.Utilities;
using PrettyDocComments.Helpers;

namespace PrettyDocComments.Model;

internal class Cell
{
    public XElement Element;
    public List<TextBlock> TextBlocks;

    private double? _height;
    public double Height => _height ??= TextBlocks.Sum(t => t.DeltaY) +
        (TextBlocks.LastOrDefault().BackgroundType == BackgroundType.Shaded ? Options.Padding.GetHeight() : 0);
}
