using System.Text.RegularExpressions;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using PrettyDocComments.Model;

namespace PrettyDocComments.Helpers;

internal static class StringExtensions
{
    public static FormattedTextEx AsFormatted(this string text, Typeface typeface, double width, IWpfTextView view)
    {
        double emSize = Options.GetNormalEmSize(view);
        return new FormattedTextEx(
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

    // See: comment of Aviad P. in: https://stackoverflow.com/a/5796427/880990
    private static readonly Regex _splitCamelCaseRegex = new(@"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", RegexOptions.Compiled);

    public static string SplitCamelCase(this string s)
    {
        if (s is { Length: > 1 }) {
            return _splitCamelCaseRegex.Replace(s, " $1");
        }
        return s;
    }
}