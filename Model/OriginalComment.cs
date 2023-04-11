using System.Windows;
using Microsoft.VisualStudio.Text;

namespace PrettyDocComments.Model;

internal readonly struct OriginalComment
{
    public OriginalComment(SnapshotSpan span, Rect bounds, string text)
    {
        Span = span;
        Bounds = bounds;
        Text = text;
    }

    public readonly SnapshotSpan Span;
    public readonly Rect Bounds;
    public readonly string Text;
}

