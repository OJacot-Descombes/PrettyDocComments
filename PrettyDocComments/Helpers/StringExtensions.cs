using System.Text.RegularExpressions;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;

namespace PrettyDocComments.Helpers;

internal static class StringExtensions
{
    public static FormattedText AsFormatted(this string text, Typeface typeface, double width, IWpfTextView view)
    {
        double emSize = Options.GetNormalEmSize(view);
        return new FormattedText(
            textToFormat: text,
            culture: view.FormattedLineSource.DefaultTextProperties.CultureInfo,
            flowDirection: view.VisualElement.FlowDirection,
            typeface: typeface,
            emSize: emSize,
            foreground: Options.DefaultTextBrush,
            pixelsPerDip: VisualTreeHelper.GetDpi(view.VisualElement).PixelsPerDip
        ) {
            MaxTextWidth = Math.Max(width, emSize)
        };
    }

    private static readonly Regex _normalizeWhiteSpacesRegex = new(@"[ \n\r\t\f]+", RegexOptions.Compiled);

    public static string NormalizeSpace(this string s, bool normalizeWS)
    => normalizeWS ? _normalizeWhiteSpacesRegex.Replace(s, " ") : s;

    public static string FirstCap(this string s)
    {
        if (s is { Length: > 0 }) {
            return s.Substring(0, 1).ToUpper() + s.Substring(1, s.Length - 1);
        }
        return s;
    }
}
