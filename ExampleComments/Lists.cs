namespace ExamplesCSharp;

class Lists
{
    /// <summary>Example of a bulleted list:
    /// <list type="bullet">
    ///   <item>
    ///     <description>Item 1.
    ///     Second line.</description>
    ///   </item>
    ///   <item>
    ///     <description>Item 2.</description>
    ///   </item>
    /// </list>
    /// </summary>
    public static void BulletedList()
    {
    }

    /// <summary>Example of a numered list:
    /// <list type="number">
    /// <listheader>
    ///    <description>description</description>
    /// </listheader>
    /// <item>
    /// <description>Item 1.</description>
    /// </item>
    /// <item>
    /// <description>Item 2.</description>
    /// </item>
    /// </list>
    /// </summary>
    public static void NumberedList()
    {
    }

    /// <summary>Example of a numbered definition list:
    /// <list type="number">
    ///   <listheader>
    ///     <term>Term</term>
    ///     <description>Description</description>
    ///   </listheader>
    ///   <item>
    ///     <term>Term 1</term>
    ///     <description>Item 1</description>
    ///   </item>
    ///   <item>
    ///     <term>Term 1</term>
    ///     <description>Item 2</description>
    ///   </item>
    /// </list>
    /// </summary>
    public static void NumberedDefinitionList()
    {
    }

    /// <dl>
    ///   <dt>Coffee</dt>
    ///   <dd>Black hot drink</dd>
    ///   <dt>Milk</dt>
    ///   <dd>White cold drink</dd>
    /// </dl>
    public static void HtmlDescriptionList()
    {
    }

    ///<summary>
    /// <b>Default bullet</b><br/>
    /// <ul>
    ///  <li>Beetroot</li>
    ///  <li>Ginger</li>
    ///  <li>Potato</li>
    ///  <li>Radish</li>
    /// </ul>
    /// <br/><b>Disc bullet</b><br/>
    /// <ul type="disc">
    ///  <li>Beetroot</li>
    ///  <li>Ginger</li>
    ///  <li>Potato</li>
    ///  <li>Radish</li>
    /// </ul>
    /// <br/><b>Square bullet</b><br/>
    /// <ul type="square">
    ///  <li>Beetroot</li>
    ///  <li>Ginger</li>
    ///  <li>Potato</li>
    ///  <li>Radish</li>
    /// </ul>
    /// <br/><b>Circle bullet</b><br/>
    /// <ul type="circle">
    ///  <li>Beetroot</li>
    ///  <li>Ginger</li>
    ///  <li>Potato</li>
    ///  <li>Radish</li>
    /// </ul>
    /// </summary>
    public static void UnorderedHtmlLists()
    {
    }

    ///<summary>
    /// <b>Numbered list "default"</b><br/>
    /// <ol>
    ///  <li>Beetroot</li>
    ///  <li>Ginger</li>
    ///  <li>Potato</li>
    ///  <li>Radish</li>
    /// </ol>
    /// <br/><b>Numbered list "1"</b><br/>
    /// <ol type="1">
    ///  <li>Beetroot</li>
    ///  <li>Ginger</li>
    ///  <li>Potato</li>
    ///  <li>Radish</li>
    /// </ol>
    /// <br/><b>Numbered list "A"</b><br/>
    /// <ol type="A">
    ///  <li>Beetroot</li>
    ///  <li>Ginger</li>
    ///  <li>Potato</li>
    ///  <li>Radish</li>
    /// </ol>
    /// <br/><b>Numbered list "a"</b><br/>
    /// <ol type="a">
    ///  <li>Beetroot</li>
    ///  <li>Ginger</li>
    ///  <li>Potato</li>
    ///  <li>Radish</li>
    /// </ol>
    /// <br/><b>Numbered list "I"</b><br/>
    /// <ol type="I">
    ///  <li>Beetroot</li>
    ///  <li>Ginger</li>
    ///  <li>Potato</li>
    ///  <li>Radish</li>
    /// </ol>
    /// <br/><b>Numbered list "i"</b><br/>
    /// <ol type="i">
    ///  <li>Beetroot</li>
    ///  <li>Ginger</li>
    ///  <li>Potato</li>
    ///  <li>Radish</li>
    /// </ol>
    /// </summary>
    public static void OrderedHtmlList()
    {
    }

    /// <summary>Example of a nested lists with default bullets:
    /// <list>
    /// <item>
    /// <description>Main Item 1.
    /// Second line.</description>
    /// <list>
    /// <item>
    /// <description>Sub-Item 1.
    /// Second line.</description>
    /// </item>
    /// <item>
    /// <description>Sub-Item 2.</description>
    /// <list>
    /// <item>
    /// <description>Sub-Sub-Item 1.
    /// Second line.
    /// <list>
    /// <item>
    /// <description>Sub-Sub-Sub-Item 1.
    /// Second line.</description>
    /// </item>
    /// <item>
    /// <description>Sub-Sub-Sub-Item 2.</description>
    /// </item>
    /// </list>
    /// </description>
    /// </item>
    /// <item>
    /// <description>Sub-Sub-Item 2.</description>
    /// <code>
    /// string s = "Code in a sub-sub-item";
    /// _accumulator.Add(s);
    /// </code>
    /// Still in the same item.
    /// </item>
    /// </list>
    /// </item>
    /// </list>
    /// </item>
    /// <item>
    /// <description>Main Item 2.</description>
    /// </item>
    /// </list>
    /// </summary>
    public static void NestedDefaultLists()
    {
    }
}
