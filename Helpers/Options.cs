using System.Windows;
using System.Windows.Media;

namespace PrettyDocComments.Helpers;

internal static class Options
{
    public static readonly Typeface NormalTypeFace;
    public static readonly Typeface CaptionsTypeFace;
    public static readonly Typeface CodeTypeFace;
    public static readonly Typeface CodeCaptionTypeFace;

    public static readonly Brush CommentBackground = Brushes.LightGoldenrodYellow;
    public static readonly Brush CodeBackground = Brushes.Gainsboro;
    public static readonly Brush DefaultTextColor = Brushes.Black;
    public static readonly Brush CRefTextColor = Brushes.DarkSlateBlue;

    public static readonly Pen CommentOutline;
    public static readonly Pen CommentSeparator;
    public static readonly Pen BoldCommentSeparator;

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

        CodeCaptionTypeFace = new Typeface(new FontFamily("Cascadia Mono"),
            FontStyles.Normal, FontWeights.Bold, FontStretches.Normal, new FontFamily("Consolas"));

        CommentOutline = new Pen(Brushes.DarkKhaki, 2.0);
        CommentOutline.Freeze();

        CommentSeparator = new Pen(Brushes.DarkKhaki, 0.5);
        CommentSeparator.Freeze();

        BoldCommentSeparator = CommentOutline;
    }
}
