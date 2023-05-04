using System.Windows.Media;
using System.Windows.Media.TextFormatting;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Utilities;

namespace PrettyDocComments.Helpers;

internal static class Factory
{
    public static FormattedText CreateFormattedText(string text, Typeface typeface, double indent, IWpfTextView view)
    {
        IFormattedLineSource formattedLineSource = view.FormattedLineSource;
        TextRunProperties defaultTextProperties = formattedLineSource.DefaultTextProperties;
        return new FormattedText(
            textToFormat: text,
            culture: defaultTextProperties.CultureInfo,
            flowDirection: view.VisualElement.FlowDirection,
            typeface: typeface,
            emSize: defaultTextProperties.FontRenderingEmSize * Options.FontScaling,
            foreground: Options.DefaultTextColor,
            pixelsPerDip: VisualTreeHelper.GetDpi(view.VisualElement).PixelsPerDip
        ) {
            MaxTextWidth = Math.Max(Options.CommentWidthInColumns * formattedLineSource.ColumnWidth - indent - Options.Padding.GetWidth(), 50)
        };
    }
}
