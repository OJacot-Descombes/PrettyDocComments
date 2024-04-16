#pragma warning disable CA1822 // Mark members as static
#pragma warning disable IDE0060 // Remove unused parameter

namespace ExampleComments;

// Examples from https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments#d36-include

// D.2 Introduction
/// <summary>
/// Class <c>Point</c> models a point in a two-dimensional plane.
/// </summary>
public class Point
{
    /// <summary>
    /// Method <c>Draw</c> renders the point.
    /// </summary>
    public void Draw() { }
}

// D.3.2 <c>
/// <summary>
/// Class <c>Point</c> models a point in a two-dimensional plane.
/// </summary>
public class Point_c
{
}

// D.3.3 <code>
public class Point_code
{
    /// <summary>
    /// This method changes the point's location by the given x- and y-offsets.
    /// <example>
    /// For example:
    /// <code>
    /// Point p = new Point(3,5);
    /// p.Translate(-1,3);
    /// </code>
    /// results in <c>p</c>'s having the value (2,8).
    /// </example>
    /// </summary>
    public static void Translate(int dx, int dy)
    {
    }
}

// D.3.5 <exception>
public class DataBaseOperations
{
    /// <exception cref="MasterFileFormatCorruptException">
    /// Thrown when the master file is corrupted.
    /// </exception>
    /// <exception cref="MasterFileLockedOpenException">
    /// Thrown when the master file is already open.
    /// </exception>
    public static void ReadRecord(int flag)
    {
        if (flag == 1) {
            throw new MasterFileFormatCorruptException();
        } else if (flag == 2) {
            throw new MasterFileLockedOpenException();
        }
    }

    private class MasterFileFormatCorruptException : Exception { }
    private class MasterFileLockedOpenException : Exception { }
}

// D.3.6 <include>    (we don't resolve it)
/// <include file="docs.xml" path='extradoc/class[@name="IntList"]/*' />
public class IntList { }

// D.3.7 <list>
public class MyClass
{
    /// <summary>Here is an example of a bulleted list:
    /// <list type="bullet">
    /// <item>
    /// <description>Item 1.</description>
    /// </item>
    /// <item>
    /// <description>Item 2.</description>
    /// </item>
    /// </list>
    /// </summary>
    public static void Test()
    {
    }
}

// D.3.8 <para>
public class Point_para
{
    /// <summary>This is the entry point of the Point class testing program.
    /// <para>
    /// This program tests each method and operator, and
    /// is intended to be run after any non-trivial maintenance has
    /// been performed on the Point class.
    /// </para>
    /// </summary>
    public static void Main()
    {
    }
}

// D.3.9 <param>
public class Point_param
{
    /// <summary>
    /// This method changes the point's location to
    /// the given coordinates.
    /// </summary>
    /// <param name="xPosition">the new x-coordinate.</param>
    /// <param name="yPosition">the new y-coordinate.</param>
    public static void Move(int xPosition, int yPosition)
    {
    }
}

// D.3.10 <paramref>
public class Point_paramref
{
    /// <summary>This constructor initializes the new Point to
    /// (<paramref name="xPosition"/>,<paramref name="yPosition"/>).
    /// </summary>
    /// <param name="xPosition">the new Point's x-coordinate.</param>
    /// <param name="yPosition">the new Point's y-coordinate.</param>
    public Point_paramref(int xPosition, int yPosition)
    {
    }
}

// D.3.11 <permission>
public class MyClass_permission
{
    /// <permission cref="System.Security.PermissionSet">
    /// Everyone can access this method.
    /// </permission>
    public static void Test()
    {
    }
}

// D.3.12 <remarks>
/// <summary>
/// Class <c>Point</c> models a point in a two-dimensional plane.
/// </summary>
/// <remarks>
/// Uses polar coordinates
/// </remarks>
public class Point_remarks
{
}

// D.3.13 <returns>
public class Point_returns
{
    /// <summary>
    /// Report a point's location as a string.
    /// </summary>
    /// <returns>
    /// A string representing a point's location, in the form (x,y),
    /// without any leading, trailing, or embedded whitespace.
    /// </returns>
    public override string ToString() => $"({X},{Y})";
    public int X { get; set; }
    public int Y { get; set; }
}

// D.3.14 <see>
public class Point_see
{
    /// <summary>
    /// This method changes the point's location to
    /// the given coordinates. <see cref="Translate"/>
    /// </summary>
    public static void Move(int xPosition, int yPosition)
    {
    }
    /// <summary>This method changes the point's location by
    /// the given x- and y-offsets. <see cref="Move"/>
    /// </summary>
    public static void Translate(int dx, int dy)
    {
    }
}

// D.3.15 <seealso>
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
public class Point_also
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
{
    /// <summary>
    /// This method determines whether two Points have the same location.
    /// </summary>
    /// <seealso cref="operator=="/>
    /// <seealso cref="operator!="/>
    public override bool Equals(object? o)
    {
        return false;
    }
}

// D.3.16 <summary>
public class Point_summary
{

    /// <summary>
    /// This constructor initializes the new Point to
    /// (<paramref name="xPosition"/>,<paramref name="yPosition"/>).
    /// </summary>
    public Point_summary(int xPosition, int yPosition)
    {
    }

    /// <summary>This constructor initializes the new Point to (0,0).</summary>
    public Point_summary() : this(0, 0)
    {
    }
}

// D.3.17 <typeparam>
/// <summary>A generic list class.</summary>
/// <typeparam name="T">The type stored by the list.</typeparam>
public class MyList<T>
{
}

// D.3.18 <typeparamref>
public class MyClass_typeparamref
{
    /// <summary>
    /// This method fetches data and returns a list of
    /// <typeparamref name="T"/>.
    /// </summary>
    /// <param name="string">query to execute</param>
    public static List<T> FetchData<T>(string query)
    {
        return new List<T>();
    }
}

// D.3.19 <value>
public class Point_value
{
    /// <value>Property <c>X</c> represents the point's x-coordinate.</value>
    public int X { get; set; }
}

/// <summary>
/// Class <c>Point</c> models a point in a two-dimensional plane.
/// </summary>
public class Graphics_Point
{
    /// <value>
    /// Property <c>X</c> represents the point's x-coordinate.
    /// </value>
    public int X { get; set; }

    /// <value>
    /// Property <c>Y</c> represents the point's y-coordinate.
    /// </value>
    public int Y { get; set; }

    /// <summary>
    /// This constructor initializes the new Point to (0,0).
    /// </summary>
    public Graphics_Point() : this(0, 0) { }

    // Note: the <param> tag is obviously not used as specified: <param name="name">description</param>

    /// <summary>
    /// This constructor initializes the new Point to
    /// (<paramref name="xPosition"/>,<paramref name="yPosition"/>).
    /// </summary>
    /// <param><c>xPosition</c> is the new Point's x-coordinate.</param>
    /// <param><c>yPosition</c> is the new Point's y-coordinate.</param>
    public Graphics_Point(int xPosition, int yPosition)
    {
        X = xPosition;
        Y = yPosition;
    }

    /// <summary>
    /// This method changes the point's location to
    /// the given coordinates. <see cref="Translate"/>
    /// </summary>
    /// <param><c>xPosition</c> is the new x-coordinate.</param>
    /// <param><c>yPosition</c> is the new y-coordinate.</param>
    public void Move(int xPosition, int yPosition)
    {
        X = xPosition;
        Y = yPosition;
    }

    /// <summary>
    /// This method changes the point's location by
    /// the given x- and y-offsets.
    /// <example>For example:
    /// <code>
    /// Point p = new Point(3, 5);
    /// p.Translate(-1, 3);
    /// </code>
    /// results in <c>p</c>'s having the value (2, 8).
    /// <see cref="Move"/>
    /// </example>
    /// </summary>
    /// <param><c>dx</c> is the relative x-offset.</param>
    /// <param><c>dy</c> is the relative y-offset.</param>
    public void Translate(int dx, int dy)
    {
        X += dx;
        Y += dy;
    }

    /// <summary>
    /// This method determines whether two Points have the same location.
    /// </summary>
    /// <param>
    /// <c>o</c> is the object to be compared to the current object.
    /// </param>
    /// <returns>
    /// True if the Points have the same location and they have
    /// the exact same type; otherwise, false.
    /// </returns>
    /// <seealso cref="operator=="/>
    /// <seealso cref="operator!="/>
#pragma warning disable CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    public override bool Equals(object o)
#pragma warning restore CS8765 // Nullability of type of parameter doesn't match overridden member (possibly because of nullability attributes).
    {
        if (o == null) {
            return false;
        }
        if ((object)this == o) {
            return true;
        }
        if (GetType() == o.GetType()) {
            var p = (Graphics_Point)o;
            return (X == p.X) && (Y == p.Y);
        }
        return false;
    }

    /// <summary>
    /// This method returns a Point's hashcode.
    /// </summary>
    /// <returns>
    /// The int hashcode.
    /// </returns>
    public override int GetHashCode()
    {
        return X + (Y >> 4);    // a crude version
    }

    /// <summary>Report a point's location as a string.</summary>
    /// <returns>
    /// A string representing a point's location, in the form (x,y),
    /// without any leading, training, or embedded whitespace.
    /// </returns>
    public override string ToString() => $"({X},{Y})";

    /// <summary>
    /// This operator determines whether two Points have the same location.
    /// </summary>
    /// <param><c>p1</c> is the first Point to be compared.</param>
    /// <param><c>p2</c> is the second Point to be compared.</param>
    /// <returns>
    /// True if the Points have the same location and they have
    /// the exact same type; otherwise, false.
    /// </returns>
    /// <seealso cref="Equals"/>
    /// <seealso cref="operator!="/>
    public static bool operator ==(Graphics_Point p1, Graphics_Point p2)
    {
        if (p1 is null || p2 is null) {
            return false;
        }
        if (p1.GetType() == p2.GetType()) {
            return (p1.X == p2.X) && (p1.Y == p2.Y);
        }
        return false;
    }

    /// <summary>
    /// This operator determines whether two Points have the same location.
    /// </summary>
    /// <param><c>p1</c> is the first Point to be compared.</param>
    /// <param><c>p2</c> is the second Point to be compared.</param>
    /// <returns>
    /// True if the Points do not have the same location and the
    /// exact same type; otherwise, false.
    /// </returns>
    /// <seealso cref="Equals"/>
    /// <seealso cref="operator=="/>
    public static bool operator !=(Graphics_Point p1, Graphics_Point p2) => !(p1 == p2);
}