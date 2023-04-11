using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Text.Outlining;

namespace PrettyDocComments;

/// <summary>
/// A custom line transform source that transforms lines to fit doc comment adornments as needed.
/// </summary>
internal sealed class LineTransformSource : ILineTransformSource
{

    private readonly IWpfTextView _view;
    private readonly IOutliningManager _outliningManager;
    private readonly CommentAdornment _adornment;

    public LineTransformSource(IWpfTextView textView, IOutliningManager outliningManager, CommentAdornment adornment)
    {
        _outliningManager = outliningManager;
        _view = textView;
        _adornment = adornment;
    }

    public static bool AdornmentVisisble;//For tests. Simulates a detection of an aornment's state.



    public LineTransform GetLineTransform(ITextViewLine line, double yPosition, ViewRelativePosition placement)
    {
        int lineNumber = line.Start.GetContainingLineNumber();
        if (lineNumber is >= 38 and <= 41 && AdornmentVisisble && !IsLineCollapsed(line)) {
            return new LineTransform(0, 0, 0.6);
        }
        return new LineTransform(0, 0, 1);
    }

    private bool IsLineCollapsed(ITextViewLine line)
    {
        return _outliningManager?.GetCollapsedRegions(line.GetTextElementSpan(line.End)).Any() ?? false;
    }
}