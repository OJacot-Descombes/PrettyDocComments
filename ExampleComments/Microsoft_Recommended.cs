namespace ExampleComments;

// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags

internal class Microsoft_Recommended
{
    /// <summary>
    /// This property always returns a value &lt; 1.
    /// </summary>
    /// <remarks>If you want angle brackets to appear in the text of a documentation comment, use the HTML encoding, which is &lt; and &gt; respectively.</remarks>
    class HtmlEncodingofLt { }

    ///<summary>description</summary>
    class Summary { }

    /// <remarks>
    /// description
    /// </remarks>
    class Remarks { }

    /// <returns>description</returns>
    class Returns { }

    /// <param name="name">description</param>
    class Param { }

    /// <paramref name="name"/>
    class Paramref { }

    /// <value>property-description</value>
    class Exception { }

    /// <exception cref="member">description</exception>
    class Value { }

    /// <remarks>
    ///     <para>
    ///         This is an introductory paragraph.
    ///     </para>
    ///     <para>
    ///         This paragraph contains more details.
    ///     </para>
    /// </remarks>
    class Para { }

    /// <example>Bullet:
    /// <list type="bullet">
    ///     <listheader>
    ///         <term>term</term>
    ///         <description>description</description>
    ///     </listheader>
    ///     <item>
    ///         <term>Assembly</term>
    ///         <description>The library or executable built from a compilation.</description>
    ///     </item>
    /// </list>
    /// </example>
    /// <example>Number:
    /// <list type="number">
    ///     <listheader>
    ///         <term>term</term>
    ///         <description>description</description>
    ///     </listheader>
    ///     <item>
    ///         <term>Assembly</term>
    ///         <description>The library or executable built from a compilation.</description>
    ///     </item>
    /// </list>
    /// </example>
    /// <example>Table:
    /// <list type="table">
    ///     <listheader>
    ///         <term>term</term>
    ///         <description>description</description>
    ///     </listheader>
    ///     <item>
    ///         <term>Assembly</term>
    ///         <description>The library or executable built from a compilation.</description>
    ///     </item>
    /// </list>
    /// </example>
    class List { }

    /// <c>text</c>
    class C { }

    /// <code>
    ///     var index = 5;
    ///     index++;
    /// </code>
    class Code { }

    /// <example>
    /// This shows how to increment an integer.
    /// <code>
    ///     var index = 5;
    ///     index++;
    /// </code>
    /// </example>
    class Example { }

    /// <para><inheritdoc/></para>
    /// <para><inheritdoc cref="Specify the member to inherit documentation from."/></para>
    /// <para><inheritdoc path="The XPath expression query that will result in a node set to show."/></para>
    /// <para><inheritdoc cref="Specify the member to inherit documentation from." path="The XPath expression query that will result in a node set to show."/></para>
    class Inheritdoc { }

    /// <include file='filename' path='tagpath[@name="id"]' />
    class Include { }

    /// <!-- <see cref="member"/> -->
    /// <see cref="member"/>
    /// <!-- or   <see cref="member">Link text</see> -->
    /// <see cref="member">Link text</see>
    /// <!-- or   <see href="link">Link Text</see> -->
    /// <see href="link">Link Text</see>
    /// <!-- or   <see langword="keyword"/> -->
    /// <see langword="keyword"/>
    class See { }

    /// <!-- <seealso cref="member"/> -->
    /// <seealso cref="member"/>
    /// <!-- or   <seealso href="link">Link Text</seealso> -->
    /// <seealso href="link">Link Text</seealso>
    class Seealso { }

    /// <typeparam name="TResult">The type returned from this method</typeparam>
    class Typeparam { }

    /// <typeparamref name="TKey"/>
    class Typeparamref { }
}
