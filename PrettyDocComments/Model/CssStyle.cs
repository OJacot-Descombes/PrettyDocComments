using System.Xml.Linq;
using Media = System.Windows.Media;

namespace PrettyDocComments.Model;

internal class CssStyle
{
    public Media.Brush Color { get; private set; }
    public Media.Brush BackgroundColor { get; private set; }

    public static CssStyle Get(XElement element)
    {
        CssStyle style = null;
        if (element.Attribute("style") is { Value.Length: > 0 } styleAttribute) {
            // Example: "height:2px;border-width:0;color:gray;background-color:gray"
            string[] parts = styleAttribute.Value.Split(';');
            foreach (string part in parts) {
                int index = part.IndexOf(':');
                if (index >= 0) {
                    string name = part.Substring(0, index);
                    string value = part.Substring(index + 1);
                    Media.Brush brush;
                    switch (name) {
                        case "color":
                            if (TryGetBrush(value, out brush)) {
                                style ??= new();
                                style.Color = brush;
                            }
                            break;
                        case "background-color" or "background":
                            if (TryGetBrush(value, out brush)) {
                                style ??= new();
                                style.BackgroundColor = brush;
                            }
                            break;
                    }
                }
            }
        }
        return style;
    }

    private static bool TryGetBrush(string s, out Media.Brush brush)
    {
        s = s.Replace(" ", "").Replace("!important", "");
        if (CssColor.TryFromHex(s, out CssColor color) || CssColor.TryFromRgb(s, out color) ||
            CssColor.TryFromRgba(s, out color) || CssColor.TryFromHsl(s, out color) ||
            CssColor.TryFromHsla(s, out color) || CssColors.TryGetByName(s, out color)) {

            brush = new Media.SolidColorBrush(color.ToMediaColor());
            return true;
        }
        brush = null;
        return false;
    }
}
