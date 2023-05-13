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
    private static readonly Renderer _renderer = new();

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

        _locator = new Locator(docCommentRegex, view.FormattedLineSource.ColumnWidth);
        _shapeParser = new ShapeParser(view);
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
        int caretLineNumber = _view.Caret?.Position.BufferPosition.GetContainingLineNumber() ?? -1;
        if (!comment.ContainsLine(caretLineNumber) && !IsLineCollapsed(line)) {
            return new LineTransform(comment.Data.VerticalScale);
        }
        return new LineTransform(1.0);
    }

    private bool TryAddRenderInfo(ITextViewLine line, out Comment<RenderInfo> commentWithRenderInfo)
    {
        if (_locator.TryGetComment(_view.TextSnapshot, line, out Comment<string> commentWithXmlText) &&
            Xml.TryParseUnrootedNodes(commentWithXmlText.Data, out var nodes)) {

            if (Xml.LastXmlException is not null) {
                double columnWidth = _view.FormattedLineSource.ColumnWidth;
                var origin = new Point(
                    x: (commentWithXmlText.CommentLeftCharIndex + Xml.LastXmlException.LinePosition + 3) * columnWidth,
                    y: Xml.LastXmlException.LineNumber * _view.FormattedLineSource.LineHeight - 3
                );
                var errorInfo = new RenderInfo(
                    new List<Shape> { // Caret
                        new RectangleShape(Brushes.Red, null, origin, width: columnWidth, height: 2, deltaY: 2)
                    },
                    calculatedHeight: (commentWithXmlText.LastLineNumber - commentWithXmlText.FirstLineNumber + 1) * _view.FormattedLineSource.LineHeight,
                    verticalScale: 1.0,
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
    }

    private bool IsLineCollapsed(ITextViewLine line)
    {
        return _outliningManager?.GetCollapsedRegions(line.GetTextElementSpan(line.End)).Any() ?? false;
    }
}