using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml.Linq;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace PrettyDocComments;

/// <summary>
/// Places yellow boxes behind all the XML doc comments in the editor window
/// </summary>
internal sealed class CommentAdornment
{
    private readonly struct DocComment
    {
        public DocComment(SnapshotSpan span, Rect bounds, string text)
        {
            Span = span;
            Bounds = bounds;
            Text = text;
        }

        public readonly SnapshotSpan Span;
        public readonly Rect Bounds;
        public readonly string Text;
    }

    private readonly IAdornmentLayer _layer;
    private readonly IWpfTextView _view;
    private readonly Regex _docCommentRecognizer;
    private readonly DocCommentRenderer _docCommentRenderer;
    private readonly double _averageCharWidth;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommentAdornment"/> class.
    /// </summary>
    /// <param name="view">Text view to create the adornment for</param>
    public CommentAdornment(IWpfTextView view, Regex docCommentRecognizer, System.Drawing.Font editorFont)
    {
        if (view == null) {
            throw new ArgumentNullException("view");
        }
        _layer = view.GetAdornmentLayer("TextAdornment");

        _view = view;
        _docCommentRecognizer = docCommentRecognizer;
        _view.LayoutChanged += View_LayoutChanged;
        _view.Caret.PositionChanged += Caret_PositionChanged;

        _docCommentRenderer = new(VisualTreeHelper.GetDpi(_view.VisualElement).PixelsPerDip, editorFont.Size, Options.CommentOutline);
        _averageCharWidth = GetAverageCharWidth(editorFont);
    }

    private void Caret_PositionChanged(object sender, CaretPositionChangedEventArgs e)
    {
        if (GetCaretRect() is Rect caretRect) {
            foreach (IAdornmentLayerElement element in _layer.Elements) {
                if (element.Tag == this) {
                    SetAdornmentVisibility(caretRect, element.Adornment);
                }
            }
        }
    }

    private static void SetAdornmentVisibility(Rect caretRect, UIElement element)
    {
        if (LogicalTreeHelper.GetParent(element) is Visual parent) {
            GeneralTransform gt = element.TransformToAncestor(parent);
            Rect elementRect = gt.TransformBounds(new Rect(element.RenderSize));
            element.Visibility = caretRect.Top >= elementRect.Top && caretRect.Bottom <= elementRect.Bottom
                ? Visibility.Hidden
                : Visibility.Visible;
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

            foreach (DocComment docComment in GetDocComments(_view.TextSnapshot)) {
                if (DocCommentLoader.Instance.TryLoad(docComment.Text, out IEnumerable<XNode> nodes)) {
                    CreateVisuals(docComment, nodes);
                }
            }
        } catch (Exception ex) {
#if DEBUG
            MessageBox.Show(ex.ToString());
#endif
        }
    }

    private (int commentStart, int visibleStart, int visibleEnd) GetLineIndexRange(ITextSnapshot textSnapshot)
    {
        int visibleStart = _view.TextViewLines.FirstVisibleLine.Start.GetContainingLineNumber();
        int visibleEnd = _view.TextViewLines.LastVisibleLine.Start.GetContainingLineNumber();

        int commentStart = visibleStart;
        if (commentStart < textSnapshot.LineCount) {
            while (commentStart > 0 &&
                _docCommentRecognizer.IsMatch(textSnapshot.GetLineFromLineNumber(commentStart).GetText())) {

                commentStart--;
            }
        }

        return (commentStart, visibleStart, visibleEnd);
    }

    private IEnumerable<DocComment> GetDocComments(ITextSnapshot textSnapshot)
    {
        int commentStartLineIndex = 0;
        int maxTextLength = 0;
        double left = -1;
        bool isInComment = false;

        var (commentStart, visibleStart, visibleEnd) = GetLineIndexRange(textSnapshot);
        int lineIndex = commentStart;


        SnapshotPoint visibleStartPoint = textSnapshot.GetLineFromLineNumber(visibleStart).Start;
        TextBounds visibleStartBounds = _view.TextViewLines.GetCharacterBounds(visibleStartPoint);

        SnapshotPoint visibleEndPoint = textSnapshot.GetLineFromLineNumber(visibleEnd).Start;
        double visibleBottom = _view.TextViewLines.GetCharacterBounds(visibleEndPoint).Bottom;

        StringBuilder sb = new();
        while (lineIndex <= (isInComment ? textSnapshot.LineCount : Math.Min(visibleEnd, textSnapshot.LineCount))) {
            ITextSnapshotLine textLine = textSnapshot.GetLineFromLineNumber(lineIndex);
            string text = textLine.GetText();
            Match match = _docCommentRecognizer.Match(text);
            if (match.Success) {
                if (!isInComment) { // Start of new doc comment.
                    commentStartLineIndex = lineIndex;
                    isInComment = true;
                    maxTextLength = 0;
                    left = -1;
                }

                string commentText = text.Substring(match.Groups[2].Index);
                sb.AppendLine(commentText.Trim());
                maxTextLength = Math.Max(maxTextLength, commentText.Length);
                if (left == -1 && lineIndex >= visibleStart && lineIndex <= visibleEnd) {
                    left = GetLeftBound(textSnapshot, lineIndex, match);
                }
            } else if (isInComment) { // Reached end of comment, return DocComment structure.
                double top, bottom;
                SnapshotPoint startPoint = textSnapshot.GetLineFromLineNumber(commentStartLineIndex).Start;
                if (commentStartLineIndex >= visibleStart) {
                    top = _view.TextViewLines.GetCharacterBounds(startPoint).Top;
                } else { // Line is not in TextViewLines buffer. 
                    top = visibleStartBounds.Top - _view.LineHeight * (visibleStart - commentStartLineIndex);
                }

                int previousIndex = lineIndex - 1;
                SnapshotPoint endPoint = textSnapshot.GetLineFromLineNumber(previousIndex).Start;
                if (previousIndex <= visibleEnd) {
                    bottom = _view.TextViewLines.GetCharacterBounds(endPoint).Bottom;
                } else {
                    bottom = visibleBottom + _view.LineHeight * (previousIndex - visibleEnd);
                }
                var rect = new Rect(left, top, _averageCharWidth * (maxTextLength + 3), bottom - top);
                var docComment = new DocComment(new SnapshotSpan(startPoint, endPoint), rect, sb.ToString());
                yield return docComment;
                isInComment = false;
                sb.Clear();
            }
            lineIndex++;
        }
    }

    private double GetLeftBound(ITextSnapshot textSnapshot, int lineIndex, Match match)
    {
        double left;
        SnapshotPoint lineStart = textSnapshot.GetLineFromLineNumber(lineIndex).Start;
        int commentLeftCharIndex = match.Groups[1].Index;
        left = _view.TextViewLines.GetCharacterBounds(lineStart + commentLeftCharIndex).Left;
        return left;
    }

    /// <summary>
    /// Adds the scarlet box behind the 'a' characters within the given line
    /// </summary>
    /// <param name="line">Line to add the adornments</param>
    private void CreateVisuals(DocComment docComment, IEnumerable<XNode> nodes)
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

    // When an adornment is added, its RenderSize is not known yet. Therefore, its visibility cannot be determined
    // and we defer this task until we get the SizeChanged event.
    private void Image_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        var image = sender as Image;
        image.SizeChanged -= Image_SizeChanged;

        if (GetCaretRect() is Rect caretRect) {
            SetAdornmentVisibility(caretRect, image);
        }
    }

    private void RenderDocComment(DrawingContext dc, DocComment docComment, IEnumerable<XNode> nodes)
    {
        dc.PushClip(new RectangleGeometry(docComment.Bounds));
        dc.DrawRectangle(Options.CommentBackground, Options.CommentOutline, docComment.Bounds);
        dc.PushTransform(new TranslateTransform(docComment.Bounds.X, docComment.Bounds.Y));

        _docCommentRenderer.Render(dc, docComment.Bounds, nodes);
    }

    private double GetAverageCharWidth(System.Drawing.Font font)
    {
        const string ExampleText = "The Quick Brown Fox Jumps Over The Lazy Dog";

        using var bmp = new System.Drawing.Bitmap(1, 1);
        using var g = System.Drawing.Graphics.FromImage(bmp);
        System.Drawing.SizeF size = g.MeasureString(ExampleText, font);
        return size.Width / ExampleText.Length;
    }
}

