﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using PrettyDocComments.Helpers;
using PrettyDocComments.Model;

namespace PrettyDocComments.Services;

internal sealed class Renderer
{
    public Comment<RenderInfo> GetRenderInfo(Comment<List<Shape>> comment, IWpfTextView view)
    {
        List<Shape> shapes = comment.Data;
        double height = shapes.Count > 0
            ? Math.Max(shapes.Max(sh => sh.Bottom) + 5, view.LineHeight)
            : view.LineHeight;

        double verticalScale = height / (comment.NumberOfLines * view.LineHeight);
        return comment.ConvertTo(new RenderInfo(shapes, height, verticalScale));
    }

    public Comment<Image> Render(Comment<RenderInfo> comment, double height, double topPadding)
    {
        var drawingGroup = new DrawingGroup();
        using (DrawingContext dc = drawingGroup.Open()) {
            var rect = new Rect(0, 0, comment.Width, height);
            dc.PushClip(new RectangleGeometry(rect));
            dc.DrawRectangle(Options.CommentBackground, Options.CommentOutline, rect);
            if (topPadding != 0.0) {
                dc.PushTransform(new TranslateTransform(0, topPadding));
            }

            foreach (var shape in comment.Data.Shapes) {
                shape.Draw(dc);
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
}
