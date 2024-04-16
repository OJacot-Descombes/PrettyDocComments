using System.Windows.Forms;
using System.Windows.Shapes;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;

namespace PrettyDocComments.Model;

internal readonly struct Comment<TData>(SnapshotSpan span, int commentLeftCharIndex, int firstLineNumber,
    int lastLineNumber, double width, TData data)
{
    public readonly SnapshotSpan Span = span;

    public readonly int CommentLeftCharIndex = commentLeftCharIndex;

    public readonly int FirstLineNumber = firstLineNumber;

    public readonly int LastLineNumber = lastLineNumber;

    public int NumberOfLines => LastLineNumber - FirstLineNumber + 1;

    public readonly double Width = width;

    public readonly TData Data = data;

    public bool ContainsLine(int lineNumber) => FirstLineNumber <= lineNumber && lineNumber <= LastLineNumber;

    public Comment<TNewData> ConvertTo<TNewData>(TNewData newData)
    {
        return new Comment<TNewData>(Span, CommentLeftCharIndex, FirstLineNumber, LastLineNumber, Width, newData);
    }

    public bool ContainsCaretOrSelStartOrEnd(IWpfTextView view)
    {
        int caretLineNumber = view.Caret?.Position.BufferPosition.GetContainingLineNumber() ?? -1;
        if (ContainsLine(caretLineNumber)) {
            return true;
        }
        if (view.Selection is { } selection) {
            return
                ContainsLine(selection.Start.Position.GetContainingLine().LineNumber) ||
                ContainsLine(selection.End.Position.GetContainingLine().LineNumber);
        }
        return false;
    }
}
