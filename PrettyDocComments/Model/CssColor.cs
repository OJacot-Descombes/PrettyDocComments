using Microsoft.VisualStudio.OLE.Interop;

namespace PrettyDocComments.Model;

// Adapted from: https://github.com/TylerBrinks/ExCSS/blob/master/src/ExCSS

public struct CssColor
{
    private readonly byte _red;
    private readonly byte _green;
    private readonly byte _blue;
    private readonly byte _alpha;

    public CssColor(int red, int green, int blue, int alpha = 255)
    {
        _red = (byte)red;
        _green = (byte)green;
        _blue = (byte)blue;
        _alpha = (byte)alpha;
    }

    /// <summary>
    /// Tries to convert from a string like <c>"#faf"</c> or <c>"#ffaaff"</c> to a <c>CssColor</c> if the string
    /// represents a valid CSS hexadecimal RGB or RGBA value.
    /// </summary>
    /// <param name="hex">A hexadecimal string starting with <c>'#'</c></param>
    /// <param name="color">The resulting color if the conversion succeeds.</param>
    /// <returns><c>true</c> if <paramref name="hex"/> is a valid CSS hexadecimal RGB or RGBA value and <c>false</c>
    /// otherwise.</returns>
    public static bool TryFromHex(string hex, out CssColor color)
    {
        if (hex is not { Length:>0 } || hex[0] != '#') {
            color = default; return false;
        }

        hex = hex.ToLowerInvariant();
        for (int i = 1; i < hex.Length; i++) {
            if (hex[i] is not (>= '0' and <= '9' or >= 'a' and <= 'f')) {
                color = default; return false;
            }
        }

        int r, g, b, a = 255;
        switch (hex.Length) {
            case 1 + 4:
                a = 17 * FromHex(hex[4]);
                goto case 1 + 3;
            case 1 + 3:
                r = 17 * FromHex(hex[1]);
                g = 17 * FromHex(hex[2]);
                b = 17 * FromHex(hex[3]);
                break;
            case 1 + 8:
                a = 16 * FromHex(hex[7]) + FromHex(hex[8]);
                goto case 1 + 6;
            case 1 + 6:
                r = 16 * FromHex(hex[1]) + FromHex(hex[2]);
                g = 16 * FromHex(hex[3]) + FromHex(hex[4]);
                b = 16 * FromHex(hex[5]) + FromHex(hex[6]);
                break;
            default:
                color = default;
                return false;
        }
        color = new CssColor(r, g, b, a);
        return true;


        static int FromHex(char c) =>
            c is >= '0' and <= '9'
                ? c - '0'
                : c - 'W';
    }

    /// <summary>
    /// Tries to convert from a string like <c>"rgb(255, 160, 255)"</c> or <c>"rgb(100%, 62.5%, 100%)"</c> to a 
    /// <c>CssColor</c> if the string represents a valid CSS rgb() value.
    /// </summary>
    /// <param name="rgb">A string supposed to represent a rgb() value.</param>
    /// <param name="color">The resulting color if the conversion succeeds.</param>
    /// <returns><c>true</c> if <paramref name="rgb"/> is a valid CSS rgb() value and <c>false</c> otherwise.</returns>
    public static bool TryFromRgb(string rgb, out CssColor color)
    {
        if (rgb.Length < 10 || !rgb.StartsWith("rgb(", StringComparison.Ordinal) || rgb[rgb.Length - 1] != ')') {
            color = default; return false;
        }
        Value[] values = GetValues(rgb, 4, rgb.Length - 5);
        if (values is not { Length: 3 }) {
            color = default; return false;
        }
        color = new(values[0].To255Range(), values[1].To255Range(), values[2].To255Range());
        return true;
    }

    /// <summary>
    /// Tries to convert from a string like <c>"rgba(255, 160, 255, 1)"</c> or <c>"rgba(100%, 62.5%, 100%, 1)"</c> to a 
    /// <c>CssColor</c> if the string represents a valid CSS rgba() value.
    /// </summary>
    /// <param name="rgba">A string supposed to represent a rgba() value.</param>
    /// <param name="color">The resulting color if the conversion succeeds.</param>
    /// <returns><c>true</c> if <paramref name="rgba"/> is a valid CSS rgba() value and <c>false</c> otherwise.</returns>
    public static bool TryFromRgba(string rgba, out CssColor color)
    {
        if (rgba.Length < 13 || !rgba.StartsWith("rgba(", StringComparison.Ordinal) || rgba[rgba.Length - 1] != ')') {
            color = default; return false;
        }
        Value[] values = GetValues(rgba, 5, rgba.Length - 6);
        if (values is not { Length: 4 }) {
            color = default; return false;
        }
        color = new(values[0].To255Range(), values[1].To255Range(), values[2].To255Range(), values[3].From1To255Range());
        return true;
    }

    /// <summary>
    /// Tries to convert from a string like <c>"hsl(0, 100%, 50%)"</c> to a <c>CssColor</c> if the string represents a 
    /// valid CSS hsl() value.
    /// </summary>
    /// <param name="hsl">A string supposed to represent a hsl() value.</param>
    /// <param name="color">The resulting color if the conversion succeeds.</param>
    /// <returns><c>true</c> if <paramref name="hsl"/> is a valid CSS hsl() value and <c>false</c> otherwise.</returns>
    public static bool TryFromHsl(string hsl, out CssColor color)
    {
        if (hsl.Length < 10 || !hsl.StartsWith("hsl(", StringComparison.Ordinal) || hsl[hsl.Length - 1] != ')') {
            color = default; return false;
        }
        Value[] values = GetValues(hsl, 4, hsl.Length - 5);
        if (values is not { Length: 3 }) {
            color = default; return false;
        }
        float h = values[0].ToDecimal() / 360f;
        float s = values[1].ToDecimal();
        float l = values[2].ToDecimal();
        color = FromHsla(h, s, l, 1f);
        return true;
    }

    /// <summary>
    /// Tries to convert from a string like <c>"hsla(0, 100%, 50%, 1)"</c> to a <c>CssColor</c> if the string represents
    /// a valid CSS hsla() value.
    /// </summary>
    /// <param name="hsla">A string supposed to represent a hsla() value.</param>
    /// <param name="color">The resulting color if the conversion succeeds.</param>
    /// <returns><c>true</c> if <paramref name="hsla"/> is a valid CSS hsla() value and <c>false</c> otherwise.</returns>
    public static bool TryFromHsla(string hsla, out CssColor color)
    {
        if (hsla.Length < 13 || !hsla.StartsWith("hsla(", StringComparison.Ordinal) || hsla[hsla.Length - 1] != ')') {
            color = default; return false;
        }
        Value[] values = GetValues(hsla, 5, hsla.Length - 6);
        if (values is not { Length: 4 }) {
            color = default; return false;
        }
        float h = values[0].ToDecimal() / 360f;
        float s = values[1].ToDecimal();
        float l = values[2].ToDecimal();
        float a = values[3].ToDecimal();
        color = FromHsla(h, s, l, a);
        return true;
    }

    private static CssColor FromHsla(float hue, float saturation, float luminosity, float alpha)
    {
        const float Third = 1f / 3f;

        float m2 = luminosity <= 0.5f
            ? luminosity * (saturation + 1f)
            : luminosity + saturation - luminosity * saturation;

        float m1 = 2f * luminosity - m2;
        int r = (int)Math.Round(255f * HueToRgb(m1, m2, hue + Third));
        int g = (int)Math.Round(255f * HueToRgb(m1, m2, hue));
        int b = (int)Math.Round(255f * HueToRgb(m1, m2, hue - Third));

        int a = (int)Math.Round(255f * alpha);
        return new CssColor(r, g, b, a < 0 ? 0 : a > 255 ? 255 : a);


        static float HueToRgb(float m1, float m2, float h)
        {
            const float OneSixth = 1f / 6f;
            const float TwoThird = 2f / 3f;

            if (h < 0f) {
                h += 1f;
            } else if (h > 1f) {
                h -= 1f;
            }

            if (h < OneSixth) {
                return m1 + (m2 - m1) * h * 6f;
            }
            if (h < 0.5) {
                return m2;
            }

            if (h < TwoThird) {
                return m1 + (m2 - m1) * (TwoThird - h) * 6f;
            }

            return m1;
        }
    }

    public readonly MediaColor ToMediaColor() => MediaColor.FromArgb(_alpha, _red, _green, _blue);

    private readonly struct Value
    {
        public Value(float number, bool isPercent)
        {
            Number = number;
            IsPercent = isPercent;
        }

        public readonly float Number;
        public readonly bool IsPercent;

        public readonly int To255Range()
        {
            int n = IsPercent ? (int)Math.Round(2.55f * Number) : (int)Number;
            return n < 0 ? 0 : n > 255 ? 255 : n;
        }

        public readonly int From1To255Range()
        {
            int n = (int)Math.Round((IsPercent ? 2.55f : 255f) * Number);
            return n < 0 ? 0 : n > 255 ? 255 : n;
        }

        public readonly float ToDecimal() => IsPercent ? 0.01f * Number : Number;

        public override string ToString() => $"{Number}{(IsPercent ? "%" : null)}";
    }

    private static Value[] GetValues(string s, int start, int count)
    {
        string[] parts = s.Substring(start, count).Split(',');
        var values = new Value[parts.Length];
        for (int i = 0; i < parts.Length; i++) {
            string part = parts[i];
            bool isPercent = part[part.Length - 1] == '%';
            if (isPercent) {
                part = part.Substring(0, part.Length - 1);
            }
            if (!Single.TryParse(part, out float number)) {
                return null;
            }
            values[i] = new(number, isPercent);
        }
        return values;
    }
}
