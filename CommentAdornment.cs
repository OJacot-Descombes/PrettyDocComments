using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using PrettyDocComments.Helpers;
using PrettyDocComments.Model;
using PrettyDocComments.Services;

namespace PrettyDocComments;

/// <summary>
/// Places yellow boxes behind all the XML doc comments in the editor window
/// </summary>
internal sealed class CommentAdornment
{

    private readonly IAdornmentLayer _layer;
    private readonly IWpfTextView _view;
    private readonly DocCommentRenderer _docCommentRenderer;
    private readonly Detector _detector;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentAdornment"/> class.
    /// </summary>
    /// <param name="view">Text view to create the adornment for</param>
    public CommentAdornment(IWpfTextView view, Regex docCommentRegex, System.Drawing.Font editorFont)
    {
        if (view == null) {
            throw new ArgumentNullException("view");
        }
        _layer = view.GetAdornmentLayer("TextAdornment");

        _view = view;
        _detector = new Detector(view, docCommentRegex, editorFont);
        _view.LayoutChanged += View_LayoutChanged;
        _view.Caret.PositionChanged += Caret_PositionChanged;

        _docCommentRenderer = new(VisualTreeHelper.GetDpi(_view.VisualElement).PixelsPerDip, editorFont.Size);
    }

    private void Caret_PositionChanged(object sender, CaretPositionChangedEventArgs e)
    {
        if (GetCaretRect() is Rect caretRect) {
            foreach (IAdornmentLayerElement element in _layer.Elements) {
                if (element.Tag == this) {
                    SetAdornmentVisibility(caretRect, element.Adornment, true);
                }
            }
        }
    }

    private void SetAdornmentVisibility(Rect caretRect, UIElement element, bool updateViewOnchange = false)
    {
        if (LogicalTreeHelper.GetParent(element) is Visual parent) {
            GeneralTransform gt = element.TransformToAncestor(parent);
            Rect elementRect = gt.TransformBounds(new Rect(element.RenderSize));
            Visibility newVisibility = caretRect.Top >= elementRect.Top && caretRect.Bottom <= elementRect.Bottom
                            ? Visibility.Hidden
                            : Visibility.Visible;
            if (newVisibility != element.Visibility) {
                element.Visibility = newVisibility;

                /*Test*/
                LineTransformSource.AdornmentVisisble = newVisibility == Visibility.Visible;
                if (updateViewOnchange) { // Trigger layout
                    _view.ViewScroller.ScrollViewportVerticallyByPixels(0.001);
                    _view.ViewScroller.ScrollViewportVerticallyByPixels(-0.001);
                }
            }
        }
    }

    private Rect? GetCaretRect()
    {
        SnapshotPoint caretPos = _view.Caret.Position.BufferPosition;
        IWpfTextViewLineCollection textViewLines = _view.TextViewLines;
        if (caretPos >= textViewLines.FirstVisibleLine.Start && caretPos <= textViewLines.LastVisibleLine.End) {
            TextBounds caretBounds = textViewLines.GetCharacterBounds(caretPos);
            return new Rect(caretBounds.Left, caretBounds.Top + 1, caretBounds.Width, caretBounds.Height - 2);
        }
        return null;
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
        if (_view.InLayout) {
            return;
        }

        try {
            _layer.RemoveAdornmentsByTag(this);

            foreach (OriginalComment docComment in _detector.GetVisibleComments(_view.TextSnapshot)) {
                if (Xml.TryParseUnrootedNodes(docComment.Text, out IEnumerable<XNode> nodes)) {
                    CreateVisuals(docComment, nodes);
                }
            }
        } catch (Exception ex) {
#if DEBUG
            MessageBox.Show(ex.ToString());
#endif
        }
    }

    /// <summary>
    /// Adds the scarlet box behind the 'a' characters within the given line
    /// </summary>
    /// <param name="line">Line to add the adornments</param>
    private void CreateVisuals(OriginalComment docComment, IEnumerable<XNode> nodes)
    {

        var drawingGroup = new DrawingGroup();
        using (DrawingContext dc = drawingGroup.Open()) {
            RenderDocComment(dc, docComment, nodes);
            dc.Close();
        }
        drawingGroup.Freeze();

        var drawingImage = new DrawingImage(drawingGroup);
        drawingImage.Freeze();

        var image = new Image {
            Source = drawingImage,
        };


        // Align the image with the top of the bounds of the text geometry
        Canvas.SetLeft(image, docComment.Bounds.Left);
        Canvas.SetTop(image, docComment.Bounds.Top);
        image.SizeChanged += Image_SizeChanged;
        _layer.AddAdornment(
            behavior: AdornmentPositioningBehavior.TextRelative,
            visualSpan: docComment.Span,
            tag: this,
            adornment: image,
            removedCallback: null);
    }

    // When an adornment is added, its RenderSize is not known yet. Therefore, its new visibility cannot be determined
    // and we defer this task until we get the SizeChanged event.
    private void Image_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var image = sender as Image;
        image.SizeChanged -= Image_SizeChanged;

        if (GetCaretRect() is Rect caretRect) {
            SetAdornmentVisibility(caretRect, image);
        }
    }

    private void RenderDocComment(DrawingContext dc, OriginalComment docComment, IEnumerable<XNode> nodes)
    {
        dc.PushClip(new RectangleGeometry(docComment.Bounds));
        dc.DrawRectangle(Options.CommentBackground, Options.CommentOutline, docComment.Bounds);
        dc.PushTransform(new TranslateTransform(docComment.Bounds.X, docComment.Bounds.Y));

        _docCommentRenderer.Render(dc, docComment.Bounds, nodes);
    }
}

