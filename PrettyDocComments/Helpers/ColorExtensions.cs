global using DrawingColor = System.Drawing.Color;
global using MediaColor = System.Windows.Media.Color;

namespace PrettyDocComments.Helpers;

internal static class ColorExtensions
{
    public static MediaColor ToMediaColor (this DrawingColor c)
        => MediaColor.FromArgb(c.A, c.R, c.G, c.B);

    /// <summary>
    /// Converts an RGB <see cref="System.Drawing.Color"/> to HSV (Hue, Saturation, Value) color space.
    /// </summary>
    /// <param name="color">A <see cref="System.Drawing.Color"/> to be converted.</param>
    /// <returns>A tuple containing hue, saturation and brightness.</returns>
    /// <remarks>Adapted from: https://github.com/forReason/ColorHelper.Net/blob/master/ColorConverter.cs</remarks>
    public static (double hue, double saturation, double value) ToHSV(this DrawingColor color)
    {
        int max = Math.Max(color.R, Math.Max(color.G, color.B));
        int min = Math.Min(color.R, Math.Min(color.G, color.B));

        double hue = color.GetHue();
        double saturation = (max == 0) ? 0 : 1d - (1d * min / max);
        double value = max / 255d;

        return (hue, saturation, value);
    }

    /// <summary>
    /// Converts an HSV (hue, saturation, value) color representation to an RGB <see cref="System.Drawing.Color"/>.
    /// </summary>
    /// <remarks>This method converts the HSV color model to the RGB color model and returns the corresponding
    /// <see cref="System.Drawing.Color"/>. The alpha channel of the resulting color is always set to 255 (fully
    /// opaque).</remarks>
    /// <param name="hue">The hue of the color, in degrees, ranging from 0 to 360.</param>
    /// <param name="saturation">The saturation of the color, as a value between 0.0 and 1.0, where 0.0 is grayscale and 1.0 is fully saturated.</param>
    /// <param name="brightnessValue">The brightness (value) of the color, as a value between 0.0 and 1.0, where 0.0 is black and 1.0 is the brightest
    /// color.</param>
    /// <returns>A <see cref="System.Drawing.Color"/> object representing the color in the RGB color space.</returns>
    /// <remarks>Adapted from: https://github.com/forReason/ColorHelper.Net/blob/master/ColorConverter.cs</remarks>
    public static DrawingColor ColorFromHSV(double hue, double saturation, double brightnessValue)
    {
        // Adapted from: https://github.com/forReason/ColorHelper.Net/blob/master/ColorConverter.cs
        int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
        double f = hue / 60 - Math.Floor(hue / 60);

        brightnessValue *= 255;
        int v = Convert.ToInt32(brightnessValue);
        int p = Convert.ToInt32(brightnessValue * (1 - saturation));
        int q = Convert.ToInt32(brightnessValue * (1 - f * saturation));
        int t = Convert.ToInt32(brightnessValue * (1 - (1 - f) * saturation));

        if (hi == 0)
            return DrawingColor.FromArgb(255, v, t, p);
        else if (hi == 1)
            return DrawingColor.FromArgb(255, q, v, p);
        else if (hi == 2)
            return DrawingColor.FromArgb(255, p, v, t);
        else if (hi == 3)
            return DrawingColor.FromArgb(255, p, q, v);
        else if (hi == 4)
            return DrawingColor.FromArgb(255, t, p, v);
        else
            return DrawingColor.FromArgb(255, v, p, q);
    }
}
