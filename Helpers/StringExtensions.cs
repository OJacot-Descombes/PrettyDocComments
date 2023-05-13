using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;

namespace PrettyDocComments.Helpers;

internal static class StringExtensions
{
    public static FormattedText AsFormatted(this string text, Typeface typeface, double width, IWpfTextView view)
    {
        IFormattedLineSource formattedLineSource = view.FormattedLineSource;
        TextRunProperties defaultTextProperties = formattedLineSource.DefaultTextProperties;
        double emSize = defaultTextProperties.FontRenderingEmSize * Options.FontScaling;
        return new FormattedText(
            textToFormat: text,
            culture: defaultTextProperties.CultureInfo,
            flowDirection: view.VisualElement.FlowDirection,
            typeface: typeface,
            emSize: emSize,
            foreground: Options.DefaultTextColor,
            pixelsPerDip: VisualTreeHelper.GetDpi(view.VisualElement).PixelsPerDip
        ) {
            MaxTextWidth = Math.Max(width, emSize)
        };
    }

    private static readonly Regex _normalizeWhiteSpacesRegex = new(@"[ \n\r\t\f]+", RegexOptions.Compiled);

    public static string NormalizeSpace(this string s, bool normalizeWS)
    => normalizeWS ? _normalizeWhiteSpacesRegex.Replace(s, " ") : s;

}
