global using DrawingColor = System.Drawing.Color;
global using MediaColor = System.Windows.Media.Color;

namespace PrettyDocComments.Helpers;

internal static class ColorExtensions
{
    public static MediaColor ToMediaColor (this DrawingColor c)
        => MediaColor.FromArgb(c.A, c.R, c.G, c.B);
}
