using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EnvDTE;
using Microsoft.VisualStudio.Text.Editor;
using PrettyDocComments.Helpers;
using PrettyDocComments.Model;

namespace PrettyDocComments.Services;

internal sealed class Renderer
{
    private readonly Dictionary<int, double> _lastLineScales = new Dictionary<int, double>();

    public Comment<RenderInfo> GetRenderInfo(Comment<List<Shape>> comment, IWpfTextView view)
    {
        List<Shape> shapes = comment.Data;
        double height = shapes.Count > 0
            ? Math.Max(shapes.Max(sh => sh.Bottom) + 5, view.LineHeight)
            : view.LineHeight;

        double verticalScale = height / (comment.NumberOfLines * view.LineHeight);
        double lastLineVerticalScale = _lastLineScales.TryGetValue(comment.FirstLineNumber, out double v)
            ? v
            : verticalScale;

        return comment.ConvertTo(new RenderInfo(shapes, height, verticalScale, lastLineVerticalScale));
    }

    public Comment<Image> Render(Comment<RenderInfo> comment, double height, double topPadding, IWpfTextView view)
    {
        const int CollpasedTextSurplusLength = 10;

        var drawingGroup = new DrawingGroup();
        using (DrawingContext dc = drawingGroup.Open()) {
            double rectWidth;
            if (comment.Data.ContainsErrorHint) {
                rectWidth = comment.Width + view.FormattedLineSource.ColumnWidth;
                dc.DrawRectangle(null, Options.ErrorOutline, new Rect(0, 0, rectWidth, height));
            } else {
                double originalCommentWidth = comment.Width + CollpasedTextSurplusLength * view.FormattedLineSource.ColumnWidth;
                rectWidth = Options.GetCommentWidthInPixels(view, comment.CommentLeftCharIndex);
                if (originalCommentWidth > rectWidth) {
                    var coverRect = new Rect(rectWidth, 0, originalCommentWidth - rectWidth, height);
                    dc.DrawRectangle(view.Background, null, coverRect);
                }

                var rect = new Rect(0, 0, rectWidth, height);
                dc.PushClip(new RectangleGeometry(rect));
                dc.DrawRectangle(Options.CommentBackground, Options.CommentOutline, rect);

                if (topPadding != 0.0) {
                    dc.PushTransform(new TranslateTransform(0, topPadding));
                }

                if (Options.CompensateLineHeight && comment.NumberOfLines > 1) {
                    double diff = comment.Data.CalculatedHeight - height;
                    if (diff is < -2.0 or > 1.0) {
                        const double Damping = 0.99;
                        double lastLineHeight = diff * Damping + view.LineHeight * comment.Data.LastLineVerticalScale;
                        double f = Math.Max(0.1, lastLineHeight / view.LineHeight);
                        _lastLineScales[comment.FirstLineNumber] = f;
                    }
                }
            }
            foreach (var shape in comment.Data.Shapes) {
                shape.Draw(dc, rectWidth);
            }
            dc.Close();
        }
        drawingGroup.Freeze();

        var drawingImage = new DrawingImage(drawingGroup);
        drawingImage.Freeze();

        var image = new Image {
            Source = drawingImage
        };
        return comment.ConvertTo(image);
    }

    public void RefreshLineHeightsBuffer()
    {
        _lastLineScales.Clear();
    }
}
