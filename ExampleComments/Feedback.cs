// https://marketplace.visualstudio.com/items?itemName=OlivierJacot-Descombes.PrettyDocComments&ssr=false#qna
using System.Text;

/// <h4>Custom Tags</h4>
/// <internalNotes note="Place comments to exclude from documentation here">
///  <para>
///   The <see langword="static"/> <see cref="M:Program.Main(string[])"/> method
///   for a Windows targeted app is located in the auto generated (App.g.i.cs)
///   file found in the \obj\Debug\net#.#-windows##.#\win###\Platforms\Windows
///   folder.
///  </para>
/// </internalNotes>
/// <initialAuthor>
///  Initial coding by John Doe
/// </initialAuthor>
/// <initialAuthoringDate>
///  7/8/2023
/// </initialAuthoringDate>
/// <br/>
/// <br/>
/// <h4>Mark Tag</h4>
/// <mark style="color: green;">Marked text 1</mark><br/>
/// <mark style="background-color: white;">Marked text 2</mark><br/>
/// <mark style="background: #00ced1!important">Marked text 3</mark><br/>
/// <mark style="color:red;background: #80FF80!important">Marked text 4</mark><br/>
/// <mark>Marked text 5</mark>
internal class CustomTagsBy_K_L_Carter_Sr { }

class WordWrapIssue
{
    // Michels, Lorenz: If you use wordwrap in your code, the wrapped part of the sentence will be shown like regular XML comment.
    // https://marketplace.visualstudio.com/items?itemName=OlivierJacot-Descombes.PrettyDocComments&ssr=false#review-details

    /// <summary>
    /// This is a long comment that will be wrapped in the code editor. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Cicero written in 45 BC: Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?
    /// </summary>
    /// <remarks> 
    /// The standard Lorem Ipsum passage, used since the 1500s and Section 1.10.32 of "de Finibus Bonorum et Malorum", written by Cicero in 45 BC
    /// </remarks>
    public void MethodName()
    {

    }
}

class ParameterAlignment
{
    // Athari: Example shows unaligned parameter descriptions in version 0.4.0. 
    // https://github.com/OJacot-Descombes/PrettyDocComments/issues/9

    /// <summary>
    /// Creates a handler to translate an interpolated string into a string.
    /// </summary>
    /// <param name="literalLength">The number of constant characters outside of interpolation expressions in the interpolated string.</param>
    /// <param name="formattedCount"> The number of interpolation expressions in the interpolated string.</param>
    /// <param name="stringBuilder">- The assiciated StringBuilder to which to append.</param>
    /// <param name="provider"> - An object that supplies culture-specific formatting information.</param>
    /// <remarks>This is intended to be called only by compiler-generated code. Arguments are not valid as they'd otherwise be for members intended to be used directly.</remarks>
    public ParameterAlignment(int literalLength, int formattedCount, StringBuilder stringBuilder, IFormatProvider provider)
    {
    }

    /// <summary>
    /// Example for type parameters.
    /// </summary>
    /// <typeparam name="T1">Type parameter 1</typeparam>
    /// <typeparam name="Txxxxxxxxxxxxx">Type parameter 2</typeparam>
    /// <typeparam name="Tyyyyyyyyyyyyyyyy">Type parameter 3</typeparam>
    public void TypeParameterAligment<T1, Txxxxxxxxxxxxx, Tyyyyyyyyyyyyyyyy>()
    {
    }

    /// <summary>
    /// Example for exceptions.
    /// </summary>
    /// <exception cref="IndexOutOfRangeException">The index is out of range.</exception>
    /// <exception cref="ArgumentNullException">The argument is null.</exception>
    /// <exception cref="AggregateException">Aggregate exception.</exception>
    public void ExceptionAlignment()
    {
    }
}
