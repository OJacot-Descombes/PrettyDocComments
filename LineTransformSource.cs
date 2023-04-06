using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace PrettyDocComments;

/// <summary>
/// A custom line transform source that transforms lines to fit doc comment adornments as needed.
/// </summary>
internal sealed class LineTransformSource : ILineTransformSource
{

    private readonly IWpfTextView _view;

    public LineTransformSource(IWpfTextView textView)
    {
        _view = textView;
    }

    public static bool AdornmentVisisble;//For tests. Simulates a detection of an aornment's state.

    public LineTransform GetLineTransform(ITextViewLine line, double yPosition, ViewRelativePosition placement)
    {
        if (line.Start.GetContainingLineNumber() is >= 38 and <= 41 && AdornmentVisisble) {
            return new LineTransform(0, 0, 0.6);
        }
        return new LineTransform(0, 0, 1);
    }
}