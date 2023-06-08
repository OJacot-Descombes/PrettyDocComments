using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;

namespace PrettyDocComments.Helpers;

internal static class Options
{
    public static readonly Typeface NormalTypeFace;
    public static readonly Typeface CaptionsTypeFace;
    public static readonly Typeface CodeTypeFace;

    public static readonly Brush CommentBackground = Brushes.LightGoldenrodYellow;
    public static readonly Brush CodeBackground = Brushes.Gainsboro;
    public static readonly Brush DefaultTextColor = Brushes.Black;
    public static readonly Brush SpecialTextColor = Brushes.DarkSlateBlue;
    public static readonly Brush CommentTextColor = Brushes.ForestGreen;

    public static readonly Pen CommentOutline;
    public static readonly Pen CommentSeparator;
    public static readonly Pen BoldCommentSeparator;
    public static readonly Pen ErrorOutline;
    public static readonly Pen FrameStroke;

    public static readonly int CommentWidthInColumns = 80;

    /// <summary>
    /// By how much we multiply the editor font size to get the comment font size.
    /// </summary>
    public static readonly double FontScaling = 0.8;

    public static readonly Thickness Padding = new(left: 5.0, top: 3.0, right: 3.0, bottom: 3.0);

    public static double GetNormalEmSize(IWpfTextView view) =>
        view.FormattedLineSource.DefaultTextProperties.FontRenderingEmSize * Options.FontScaling;

    static Options()
    {
        var textFontFamily = new FontFamily("Segoe UI");
        var fallbackTextFontFamily = new FontFamily("Arial");

        NormalTypeFace = new Typeface(textFontFamily,
            FontStyles.Normal, FontWeights.Normal, FontStretches.Normal, fallbackTextFontFamily);

        CaptionsTypeFace = new Typeface(textFontFamily,
            FontStyles.Normal, FontWeights.Bold, FontStretches.Normal, fallbackTextFontFamily);

        CodeTypeFace = new Typeface(new FontFamily("Cascadia Mono"),
            FontStyles.Normal, FontWeights.Normal, FontStretches.Normal, new FontFamily("Consolas"));

        CommentOutline = new Pen(Brushes.DarkKhaki, 2.0);
        CommentOutline.Freeze();

        CommentSeparator = new Pen(Brushes.DarkKhaki, 0.5);
        CommentSeparator.Freeze();

        ErrorOutline = new Pen(Brushes.Red, 2.0);
        ErrorOutline.Freeze();

        FrameStroke = new Pen(Brushes.Black, 0.8);
        FrameStroke.Freeze();

        BoldCommentSeparator = CommentOutline;
    }
}
