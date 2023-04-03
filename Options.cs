using System.Windows;
using System.Windows.Media;

namespace PrettyDocComments;

internal static class Options
{
    public static readonly Typeface NormalTypeFace;
    public static readonly Typeface CaptionsTypeFace;
    public static readonly Typeface CodeTypeFace;
    public static readonly Typeface CodeCaptionTypeFace;

    public static readonly Brush CommentBackground = Brushes.LightGoldenrodYellow;
    public static readonly Brush CodeBackground = Brushes.Gainsboro;

    public static readonly Pen CommentOutline;

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

        CommentOutline = new Pen(Brushes.DarkKhaki, 0.8);
        CommentOutline.Freeze();
    }
}
