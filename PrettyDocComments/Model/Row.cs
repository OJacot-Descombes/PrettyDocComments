using System.Diagnostics;

namespace PrettyDocComments.Model;

[DebuggerDisplay("Row[IsHeader = {IsHeader}, Cells = {Cells.Count}]")]
internal class Row
{
    public bool IsHeader;
    public bool IsCaption;
    public List<Cell> Cells = [];
}
