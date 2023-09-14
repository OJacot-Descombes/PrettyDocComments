using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using PrettyDocComments.Helpers;
using PrettyDocComments.Model;

namespace PrettyDocComments.Services;

/// <summary>
/// Creates the adorners and adds them to the adorner layer.
/// </summary>
internal sealed class Adornment
{
    private readonly IAdornmentLayer _layer;
    private readonly IWpfTextView _view;
    private readonly Renderer _renderer;
    private readonly List<Comment<Image>> _renderedComments = new();

    private bool _delayVisibilityChange;

    /// <summary>
    /// Initializes a new instance of the <see cref="Adornment"/> class.
    /// </summary>
    /// <param name="view">Text view to create the adornment for</param>
    public Adornment(IWpfTextView view, Renderer renderer)
    {
        _renderer = renderer;
        if (view == null) {
            throw new ArgumentNullException("view");
        }
        _layer = view.GetAdornmentLayer("DocCommentImage");

        _view = view;
        _view.LayoutChanged += View_LayoutChanged;
        _view.Caret.PositionChanged += Caret_PositionChanged;
        _view.VisualElement.MouseLeftButtonUp += VisualElement_DelayedRefresh;
        _view.VisualElement.MouseLeave += VisualElement_DelayedRefresh;
    }

    public List<Comment<RenderInfo>> RenderingInformation { get; } = new();
    public bool WasLayouted { get; set; }

    private void VisualElement_DelayedRefresh(object sender, MouseEventArgs e)
    {
        if (_delayVisibilityChange) {
            _delayVisibilityChange = false;
            SetAdornmentVisibility();
        }
    }

    private void Caret_PositionChanged(object sender, CaretPositionChangedEventArgs e)
    {
        SetAdornmentVisibility();
    }

    private void SetAdornmentVisibility()
    {
        int caretlineNumber = _view.Caret?.Position.BufferPosition.GetContainingLineNumber() ?? -1;
        bool anyVisibilityChanged = false;
        bool isLeftMouseButtonPressed = Mouse.LeftButton == MouseButtonState.Pressed;
        foreach (var comment in _renderedComments) {
            Visibility newVisibility =
                comment.ContainsLine(caretlineNumber) ? Visibility.Hidden : Visibility.Visible;
            if (newVisibility != comment.Data.Visibility) {
                if (isLeftMouseButtonPressed) {
                    _delayVisibilityChange = true; // Otherwise changing line height might unintentionally select text.
                } else {
                    anyVisibilityChanged = true;
                    comment.Data.Visibility = newVisibility;
                }
            }
        }
        if (anyVisibilityChanged) { // Trigger layout change
            _view.ViewScroller.ScrollViewportVerticallyByPixels(0.001);
            _view.ViewScroller.ScrollViewportVerticallyByPixels(-0.001);
        }
    }

    private void SetAdornmentVisibility(Image image)
    {
        int i = _renderedComments.FindIndex(c => c.Data == image);
        if (i >= 0) {
            int caretlineNumber = _view.Caret?.Position.BufferPosition.GetContainingLineNumber() ?? -1;
            var comment = _renderedComments[i];
            comment.Data.Visibility = comment.ContainsLine(caretlineNumber) ? Visibility.Hidden : Visibility.Visible;
        }
    }

    /// <summary>
    /// Handles whenever the text displayed in the view changes by adding the adornment to any reformatted lines
    /// </summary>
    /// <remarks><para>This event is raised whenever the rendered text displayed in the <see cref="ITextView"/> changes.</para>
    /// <para>It is raised whenever the view does a layout (which happens when DisplayTextLineContainingBufferPosition is called or in response to text or classification changes).</para>
    /// <para>It is also raised whenever the view scrolls horizontally or when its size changes.</para>
    /// </remarks>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void View_LayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
    {
        try {
            _layer.RemoveAdornmentsByTag(this);

            IWpfTextViewLineCollection viewLines = _view.TextViewLines;
            int firstVisibleLineNumber = viewLines.FirstVisibleLine.Start.GetContainingLineNumber();
            int lastVisibleLineNumber = viewLines.LastVisibleLine.Start.GetContainingLineNumber();
            _renderedComments.Clear();
            foreach (Comment<RenderInfo> comment in RenderingInformation) {
                double top, topPadding, height;
                bool isFirstCommentLineVisible = comment.FirstLineNumber >= firstVisibleLineNumber;
                bool isLastCommentLineVisible = comment.LastLineNumber <= lastVisibleLineNumber;
                if (isFirstCommentLineVisible && isLastCommentLineVisible) {
                    top = SafeGetCharacterBounds(comment.Span.Start).Top;
                    height = SafeGetCharacterBounds(comment.Span.End).Bottom - top;
                    topPadding = Math.Max(0, height - comment.Data.CalculatedHeight);
                } else if (isFirstCommentLineVisible) {
                    top = SafeGetCharacterBounds(comment.Span.Start).Top;
                    height = Math.Max(comment.Data.CalculatedHeight, viewLines.LastVisibleLine.Bottom - top);
                    topPadding = 0.0;
                } else if (isLastCommentLineVisible) {
                    double bottom = SafeGetCharacterBounds(comment.Span.End).Bottom;
                    height = Math.Max(comment.Data.CalculatedHeight, bottom - viewLines.FirstVisibleLine.Top + 1);
                    top = bottom - height;
                    topPadding = Math.Max(0, height - comment.Data.CalculatedHeight);
                } else { // Neither first or last line of comment are visible, use heuristics.
                    // We anchor the adorner relative to the middle of the view port vertically.
                    double midViewport = 0.5 * (_view.ViewportTop + _view.ViewportBottom);
                    ITextViewLine textLine = _view.TextViewLines.GetTextViewLineContainingYCoordinate(midViewport);
                    double middleLineNumber = textLine.Start.GetContainingLineNumber() +
                        (midViewport - textLine.Top) / textLine.Height; // Add fraction of line for smoother scrolling.
                    height = Math.Max(_view.ViewportHeight, comment.Data.CalculatedHeight);
                    top = (midViewport - height * (middleLineNumber - comment.FirstLineNumber) / comment.NumberOfLines)
                        .Clamp(_view.ViewportBottom - height, _view.ViewportTop);
                    topPadding = 0.0;
                }

                var renderedComment = _renderer.Render(comment, height, topPadding, _view);
                Image image = renderedComment.Data;
                Canvas.SetTop(image, top);

                int lineNumber = Math.Max(comment.FirstLineNumber, firstVisibleLineNumber);
                SnapshotPoint lineStart = _view.TextSnapshot.GetLineFromLineNumber(lineNumber).Start;
                double left = SafeGetCharacterBounds(lineStart + comment.CommentLeftCharIndex).Left;
                Canvas.SetLeft(image, left);

                image.SizeChanged += Image_SizeChanged;
                _layer.AddAdornment(
                    behavior: AdornmentPositioningBehavior.TextRelative,
                    visualSpan: comment.Span,
                    tag: this,
                    adornment: image,
                    removedCallback: null);
                _renderedComments.Add(renderedComment);
            }
            WasLayouted = true; // Synchronizes the LineTransformSource.
        } catch (Exception ex) {
#if DEBUG
            MessageBox.Show(ex.ToString());
#endif
        }
    }

    private TextBounds SafeGetCharacterBounds(SnapshotPoint bufferPosition)
    {
        if (_view.TextViewLines is { IsValid: true } && _view.TextViewLines.ContainsBufferPosition(bufferPosition)) {
            return _view.TextViewLines.GetCharacterBounds(bufferPosition);
        } else {
            return new TextBounds(0, 0, 10, 10, 0, 5);
        }
    }

    // When an adornment is added, its RenderSize is not known yet. Therefore, its new visibility cannot be determined
    // and we defer this task until we get the SizeChanged event.
    private void Image_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var image = sender as Image;
        image.SizeChanged -= Image_SizeChanged;
        SetAdornmentVisibility(image);
    }
}

