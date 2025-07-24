using System.Numerics;

namespace ExampleComments;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "empty example methods")]

/// This is a <u>top-level</u> text.
/// <summary>
/// This class contains example doc comments.
/// </summary>
/// <typeparam name="T">Dummy type parameter 1</typeparam>
/// <typeparam name="U">Dummy type parameter 2</typeparam>
/// This is yet another <u>top-level</u> text.
internal class MiscExamples<T, U>
{
    /// <summary>
    /// This is a <b>bold</b> word.
    /// </summary>
    public int Bold { get; set; }

    /// <summary>
    /// This is an <i>italic</i> word.
    /// </summary>
    public int Italic { get; set; }

    /// <summary>
    /// This is an <u>underlined</u> word.
    /// </summary>
    public int Underline { get; set; }

    /// <summary>
    /// This is <c>The Code</c> format.
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// This word has the <strike>strikethrough</strike> style.
    /// </summary>
    public int Strikethrough { get; set; }

    /// Text <u>preceeding</u> the summary.
    /// <summary>
    /// This is a <b><i>bold and italic</i></b> word. Sequence of <b>Bold</b> <i>italic</i> with whitespace between.
    /// </summary>
    public int BoldItalic { get; set; }

    /// <summary>
    /// This <b>bold, <u><strike>bold and underlined and strikethrough</strike>, bold and underlined</u>, bold</b>.
    /// </summary>
    public int BoldUnderlineStrikethrough { get; set; }

    /// <summary>
    /// Performs an <b>integer</b> division
    /// </summary>
    /// <param name="dividend"><c>int</c> dividend</param>
    /// <param name="divisor"><c>int</c> divisor</param>
    /// <returns>The integer part of the division</returns>
    /// <example>
    /// C#:<code>
    /// 
    ///      int result =
    ///          Divide(40, 7);
    ///          
    ///      if (bounds.Top &gt;= viewportTop &amp;&amp; bounds.Bottom &lt;= viewportBottom) {
    ///          // Element is fully visible.
    ///      }
    ///   </code>
    /// </example>
    /// 
    /// <example>No indent but leading and trailing empty lines:<code>
    ///    
    /// xxxxxx
    /// 
    /// </code></example>
    /// 
    /// <example>Empty code tag:<code></code></example>
    /// <example>Code tag with syle foreground/background color:<code style="background-color:black;color:limegreen">
    /// console> input
    /// console> output
    /// </code></example>
    public static int Divide(int dividend, int divisor)
    {
        return dividend / divisor;
    }

    /// <summary>
    /// Performs a generic division.
    /// </summary>
    /// <typeparam name="TDividend">The type of the number that will be divided.
    /// Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat,
    /// sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.
    /// Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.
    /// </typeparam>
    /// <typeparam name="TDivisor">The type of the number by which will be divided.
    /// Next line.
    /// Last line</typeparam>
    /// <typeparam name="TResult">The type of the result.</typeparam>
    /// <param name="dividend">The number that will be divided.</param>
    /// <param name="divisor">The number by which will be divided.</param>
    /// <returns>The quotient of both numbers.</returns>
    public static TResult Divide<TDividend, TDivisor, TResult>(TDividend dividend, TDivisor divisor)
        where TDividend : IDivisionOperators<TDividend, TDivisor, TResult>
    {
        return dividend / divisor;
    }

    /// Example-tag embedded
    ///  in summary:
    /// <summary>
    /// This method changes the point's location by the given x- and y-offsets.
    /// <example>
    /// For example:
    /// <code>
    /// Point p = <b>new</b> Point(3, 5);
    /// p.Translate(-1, 3);
    /// </code>
    /// results in <c>p</c>'s having the value (2,8).
    /// </example>
    /// </summary>
    /// <param name="dx">x-offset</param>
    /// <param name="dy">y-offset</param>
    public static void Translate(int dx, int dy)
    {
    }

    /// <exception cref="MasterFileFormatCorruptException">
    /// Thrown when the master file is corrupted.
    /// </exception>
    /// <exception cref="MasterFileLockedOpenException">Thrown when the master file is already open.
    /// Try to avoid.</exception>
    /// <exception>Exception with &lt;cref&gt; tag missing.</exception>
    public static void ReadRecord(int flag)
    {
    }

    /// <summary>
    /// The ampersand (&) is not escaped.
    /// </summary>
    public static void XmlError()
    {
    }

    /// <summary>
    /// The ampersand (&amp;) is escaped.
    /// </summary
    public static void XmlError_AfterLastLine()
    {
    }

    /// <summary>
    /// The <c>ampersand (&amp;) is escaped xxxxxxxxxxxxx yyyyy zzzzzzzzzzzzzzz.
    /// </summary>
    public static void XmlError_NonClosedTag()
    {
    }
}

