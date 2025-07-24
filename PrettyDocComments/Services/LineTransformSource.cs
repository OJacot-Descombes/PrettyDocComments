using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Text.Outlining;
using PrettyDocComments.Helpers;
using PrettyDocComments.Model;

namespace PrettyDocComments.Services;

/// <summary>
/// <see cref="ILineTransformSource"/> implementation that scales lines of doc comments to make them fit the height doc
/// comment adornments.
/// </summary>
internal sealed class LineTransformSource : ILineTransformSource
{
    private readonly Renderer _renderer = new();

    private readonly IWpfTextView _view;
    private readonly IOutliningManager _outliningManager;
    private readonly Adornment _adornment;

    private readonly Locator _locator;
    private readonly ShapeParser _shapeParser;

    public LineTransformSource(IWpfTextView view, IOutliningManager outliningManager, Regex docCommentRegex)
    {
        _outliningManager = outliningManager;
        _view = view;
        _adornment = new Adornment(view, _renderer);

        _locator = new Locator(docCommentRegex, view);
        _shapeParser = new ShapeParser(view);

        Options.OptionsChanged += OnOptionChanged;
        _view.ViewportWidthChanged += View_ViewportWidthChanged;
        _view.Closed += OnClosed;
    }

    private void View_ViewportWidthChanged(object sender, EventArgs e)
    {
        OnOptionChanged();
    }

    private void OnOptionChanged()
    {
        if (_view is { IsClosed: false, InLayout: false }) {
            _renderer.RefreshLineHeightsBuffer();
            var firstLine = _view.TextViewLines.FirstVisibleLine;
            _view.DisplayTextLineContainingBufferPosition(firstLine.Start, firstLine.Top - _view.ViewportTop, ViewRelativePosition.Top);
        }
    }

    private void OnClosed(object sender, System.EventArgs e)
    {
        Options.OptionsChanged -= OnOptionChanged;
        _view.ViewportWidthChanged -= View_ViewportWidthChanged;
        _view.Closed -= OnClosed;
    }

    public LineTransform GetLineTransform(ITextViewLine line, double yPosition, ViewRelativePosition placement)
    {
        int index;
        if (_adornment.WasLayouted) {
            // This is the first call of GetLineTransform occurring before a new layout run.
            _adornment.RenderingInformation.Clear();
            _adornment.WasLayouted = false;
            index = -1;
        } else {
            int lineNumber = line.Start.GetContainingLineNumber();
            index = _adornment.RenderingInformation.FindIndex(c => c.ContainsLine(lineNumber));
        }
        Comment<RenderInfo> comment;
        if (index >= 0) {
            comment = _adornment.RenderingInformation[index];
        } else if (!TryAddRenderInfo(line, out comment)) {
            return new LineTransform(1.0);
        }
        if (!comment.ContainsCaretOrSelStartOrEnd(_view) && !IsLineCollapsed(line)) {
            if (Options.CompensateLineHeight && line.Start.GetContainingLineNumber() == comment.LastLineNumber) {
                return new LineTransform(comment.Data.LastLineVerticalScale);
            } else {
                return new LineTransform(comment.Data.VerticalScale);
            }
        }
        return new LineTransform(1.0);
    }

    private bool TryAddRenderInfo(ITextViewLine line, out Comment<RenderInfo> commentWithRenderInfo)
    {
        if (_locator.TryGetComment(_view.TextSnapshot, line, out Comment<string> commentWithXmlText) &&
            Xml.TryParseUnrootedNodes(commentWithXmlText.Data, out var nodes)) {

            if (Xml.LastXmlException is not null) {
                var errorInfo = new RenderInfo(
                    [GetCaretRectangle(commentWithXmlText), GetErrorText(commentWithXmlText)],
                    calculatedHeight: (commentWithXmlText.LastLineNumber - commentWithXmlText.FirstLineNumber + 1) *
                        _view.FormattedLineSource.LineHeight,
                    verticalScale: 1.0, lastLineVerticalScale: 1.0,
                    containsErrorHint: true
                );
                commentWithRenderInfo = commentWithXmlText.ConvertTo(errorInfo);
            } else {
                var commentWithShapes = _shapeParser.Parse(commentWithXmlText.ConvertTo(nodes));
                commentWithRenderInfo = _renderer.GetRenderInfo(commentWithShapes, _view);
            }
            _adornment.RenderingInformation.Add(commentWithRenderInfo);
            return true;
        }
        commentWithRenderInfo = default;
        return false;

        RectangleShape GetCaretRectangle(Comment<string> commentWithXmlText)
        {
            double columnWidth = _view.FormattedLineSource.ColumnWidth;
            var caretOrigin = Xml.LastXmlException.LineNumber > commentWithXmlText.NumberOfLines
                ? new Point(
                    x: commentWithXmlText.Width,
                    y: commentWithXmlText.NumberOfLines * _view.FormattedLineSource.LineHeight - 3)
                : new Point(
                    x: Math.Min((Xml.LastXmlException.LinePosition + 3) * columnWidth, commentWithXmlText.Width),
                    y: Xml.LastXmlException.LineNumber * _view.FormattedLineSource.LineHeight - 3
            );
            var caretRectangle = new RectangleShape(Brushes.Red, null, caretOrigin, width: columnWidth, height: 2);
            return caretRectangle;
        }

        TextShape GetErrorText(Comment<string> commentWithXmlText)
        {
            FormattedTextEx errorText = Xml.LastXmlException.Message.AsFormatted(
                Options.NormalTypeFace,
                    0.5 * Options.GetCommentWidthInPixels(_view, commentWithXmlText.CommentLeftCharIndex),
                _view);
            errorText.SetForegroundBrush(Options.ErrorOutline.Brush);
            var errorOrigin = new Point(2 * _view.FormattedLineSource.ColumnWidth + commentWithXmlText.Width, 0);
            var errorTextShape = new TextShape(errorText, errorOrigin);
            return errorTextShape;
        }
    }

    private bool IsLineCollapsed(ITextViewLine line)
    {
        return _outliningManager?.GetCollapsedRegions(line.GetTextElementSpan(line.End)).Any() ?? false;
    }
}