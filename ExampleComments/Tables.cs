namespace ExampleComments;

/// <summary>
/// <para>
/// Here is an example of a table:
/// <list type="table">
///   <listheader>
///      <term>Column 1</term>
///      <term>Column 2</term>
///   </listheader>
///   <item>
///       <term>Row 1, Column 1</term>
///   </item>
///   <item>
///     <term>Row 2, Column 1</term>
///     <description>Row 2, Column 2</description>
///     <term>Row 2, Column 3</term>
///   </item>
/// </list>
/// </para>
/// Inside list:
/// <list type="bullet">
///   <item>
///     <description>
///       <list type="table">
///         <listheader>
///            <term>Column 1</term>
///            <term>Column 2</term>
///         </listheader>
///         <item>
///             <term>Row 1, Column 1</term>
///         </item>
///         <item>
///           <term>Row 2, Column 1</term>
///           <description>Row 2, Column 2: i i i i i i <b>bold</b> i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i j i
///               <code>
/// int i = 42;
/// int j = 7;
///               </code>
///           </description>
///         </item>
///       </list>
///     </description>
///   </item>
/// </list>
/// Text after table.
/// </summary>
internal class Tables
{
}

/// <list type="table">
///   <listheader>
///      <term>Column 1</term>
///      <term>Column 2</term>
///      <term/>
///   </listheader>
///   <item>
///       <term>Row 1, Column 1</term>
///       <term>Row 1<br/>Column 2</term>
///   </item>
///   <item>
///       <term>Row 2<br/>Column 1</term>
///       <term>Row 2, Column 2</term>
///   </item>
/// </list>
internal class SimpleTable
{
}

/// <list type="table">
///   <listheader>
///      <term>Column 1</term>
///      <term>Column 2</term>
///   </listheader>
///   <item>
///       <term>Row 1, Column 1</term>
///   </item>
///   <item>
///     <term>Row 2, Column 1</term>
///     <description>
///        <list type="table">
///            <listheader>
///                <term>C1</term>
///                <term>C2</term>
///            </listheader>
///            <item>
///                <term>R1, C1</term>
///                <term>R1, C2</term>
///            </item>       
///            <item>        
///                <term>R2, C1</term>
///                <term>R2, C2</term>
///            </item>
///        </list>
///     </description>
///     <term>Row 2, Column 3</term>
///   </item>
/// </list>
internal class NestedTables
{
}

/// <summary>
/// <list type="table">
/// <listheader><term>1</term><term>2</term></listheader>
/// <item><term>a</term><term>12345678901234567890</term></item>
/// </list>
/// </summary>
internal class AnotherTable
{
}

/// <table>
///   <caption>Monthly savings</caption>
///   <tr>
///     <th>Month</th>
///     <th>Savings</th>
///   </tr>
///   <tr>
///     <td>January</td>
///     <td>$100</td>
///   </tr>
///   <tr>
///     <td>Nested table</td>
///     <td>
///     <table>
///       <caption>Monthly savings</caption>
///       <tr>
///         <th>Month</th>
///         <th>Savings</th>
///       </tr>
///       <tr>
///         <td>January</td>
///         <td>$100</td>
///       </tr>
///     </table>
///     </td>
///   </tr>
/// </table>
class WithCaption { }

