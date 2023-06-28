using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using PrettyDocComments.CustomOptions;

namespace PrettyDocComments.Helpers;

internal static class Options
{
    public static event Action OptionsChanged;

    public static readonly Typeface NormalTypeFace;
    public static readonly Typeface CaptionsTypeFace;
    public static readonly Typeface CodeTypeFace;

    private static Brush _commentBackground;
    public static Brush CommentBackground =>
        CreateBrush(ref _commentBackground, OptionsPage?.CommentBackColor) ?? Brushes.LightGoldenrodYellow;

    private static Brush _codeBackground;
    public static Brush CodeBackground =>
        CreateBrush(ref _codeBackground, OptionsPage?.CodeBlockBackColor) ?? Brushes.Gainsboro;

    private static Brush _defaultTextBrush;
    public static Brush DefaultTextBrush =>
        CreateBrush(ref _defaultTextBrush, OptionsPage?.TextColor) ?? Brushes.Black;

    private static Brush _specialTextBrush;
    public static Brush SpecialTextBrush =>
        CreateBrush(ref _specialTextBrush, OptionsPage?.SpecialTextColor) ?? Brushes.DarkSlateBlue;

    private static Brush _commentTextBrush;
    public static Brush CommentTextBrush =>
        CreateBrush(ref _commentTextBrush, OptionsPage?.HtmlCommentTextColor) ?? Brushes.ForestGreen;

    private static Pen _commentOutline;
    public static Pen CommentOutline =>
        CreatePen(ref _commentOutline, OptionsPage?.CommentLineColor, 2.0, Brushes.DarkKhaki);

    private static Pen _commentSeparator;
    public static Pen CommentSeparator =>
        CreatePen(ref _commentSeparator, OptionsPage?.CommentLineColor, 0.5, Brushes.DarkKhaki);

    public static Pen BoldCommentSeparator => CommentOutline;

    private static Pen _errorOutline;
    public static Pen ErrorOutline =>
        CreatePen(ref _errorOutline, OptionsPage?.ErrorTextColor, 2.0, Brushes.Red);

    private static Pen _frameStroke;
    public static Pen FrameStroke =>
        CreatePen(ref _frameStroke, OptionsPage?.TextColor, 0.8, Brushes.Black);

    private static int? _commentWidthInColumns;
    public static int CommentWidthInColumns =>
        CreateValue(ref _commentWidthInColumns, OptionsPage?.CommentWidthInColumns, 80);

    private static double? _fontScaling;
    /// <summary>
    /// By how much we multiply the editor font size to get the comment font size.
    /// </summary>
    public static double FontScaling => CreateValue(ref _fontScaling, OptionsPage?.FontScaling, 0.8);

    public static readonly Thickness Padding = new(left: 5.0, top: 3.0, right: 3.0, bottom: 3.0);

    public static double GetNormalEmSize(IWpfTextView view) =>
        view.FormattedLineSource.DefaultTextProperties.FontRenderingEmSize * FontScaling;

    private static OptionPageGrid _optionsPage;
    private static OptionPageGrid OptionsPage => _optionsPage ??= PrettyDocCommentsPackage.Instance?.GetOptionPage();

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
    }

    public static void Refresh()
    {
        _optionsPage = null;
        _commentBackground = null;
        _codeBackground = null;
        _defaultTextBrush = null;
        _specialTextBrush = null;
        _commentTextBrush = null;
        _commentOutline = null;
        _commentSeparator = null;
        _errorOutline = null;
        _frameStroke = null;
        _commentWidthInColumns = null;
        _fontScaling = null;

        OptionsChanged?.Invoke();
    }

    private static Brush CreateBrush(ref Brush backingField, DrawingColor? brushColor)
    {
        if (backingField is null && brushColor is { } color) {
            backingField = new SolidColorBrush(color.ToMediaColor());
            backingField.Freeze();
        }
        return backingField;
    }

    private static Pen CreatePen(ref Pen backingField, DrawingColor? penColor, double thickness, SolidColorBrush defaultBrush)
    {
        if (backingField is null && penColor is { } color) {
            backingField = new Pen(new SolidColorBrush(color.ToMediaColor()), thickness);
            backingField.Freeze();
        }
        return backingField ?? new Pen(defaultBrush, thickness);
    }

    private static T CreateValue<T>(ref T? backingField, T? value, T defaultValue)
        where T : struct
    {
        if (backingField is null && value is { } v) {
            backingField = v;
        }
        return backingField ?? defaultValue;
    }
}
