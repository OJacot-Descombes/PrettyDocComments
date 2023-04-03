using System.Text;

namespace PrettyDocComments.Helpers;

public static class Number
{
    // See also: https://en.wikipedia.org/wiki/Roman_numerals
    private static readonly (int number, string roman)[] Numbers = {
        (100_000, "ↈ"  ),
        ( 90_000, "ↂↈ"),
        ( 50_000, "ↇ"   ),
        ( 40_000, "ↂↇ" ),
        ( 10_000, "ↂ"  ),
        (  9_000, "Mↂ" ),
        (  5_000, "ↁ"   ),
        (  4_000, "Mↁ"  ),
        (  1_000, "M"   ),
        (    900, "CM"  ),
        (    500, "D"   ),
        (    400, "CD"  ),
        (    100, "C"   ),
        (     90, "XC"  ),
        (     50, "L"   ),
        (     40, "XL"  ),
        (     10, "X"   ),
        (      9, "IX"  ),
        (      5, "V"   ),
        (      4, "IV"  ),
        (      1, "I"   )
    };


    // Improved version of https://www.c-sharpcorner.com/article/convert-numbers-to-roman-characters-in-c-sharp/
    public static string ToRoman(int n, bool lowerCase = false)
    {
        var sb = new StringBuilder();
        int i = 0;
        while (n != 0) {
            (int number, string roman) = Numbers[i];
            if (n >= number) {
                n -= number;
                sb.Append(roman);
            } else {
                i++;
            }
        }
        if (lowerCase) {
            return sb.ToString().ToLowerInvariant();
        }
        return sb.ToString();
    }

    public static string ToAlphabet(int num, bool lowerCase = false)
    {
        if (lowerCase) {
            return ((char)('a' + num - 1)).ToString();
        } else {
            return ((char)('A' + num - 1)).ToString();
        }
    }
}
