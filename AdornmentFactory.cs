using System.Drawing;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text.Editor;

namespace PrettyDocComments;

/// <summary>
/// Creates and configures <see cref="CommentAdornment"/>s.
/// </summary>
internal static class AdornmentFactory
{
    private static readonly Regex _cSharpDocCommentRecoginzer = new(@"^\s*(///)([^/]|$)", RegexOptions.Compiled);
    private static readonly Regex _visualBasicDocCommentRecoginzer = new(@"^\s*(''')([^/]|$)", RegexOptions.Compiled);

    /// <summary>
    /// Provides a <see cref="CommentAdornment"/> for supported code languages for an editor text view.
    /// </summary>
    /// <param name="textView">The editor text view.</param>
    /// <param name="editorFont">The editor font which is used to determine the metrics of the rendered doc comment.</param>
    /// <returns>An adornment if the code language is supported and <c>null</c> otherwise.</returns>
    public static CommentAdornment Create(IWpfTextView textView, Font editorFont)
    {
        return textView.TextBuffer.ContentType.TypeName switch {
            "CSharp" or "F#" or "C/C++" => new CommentAdornment(textView, _cSharpDocCommentRecoginzer, editorFont),
            "Basic" => new CommentAdornment(textView, _visualBasicDocCommentRecoginzer, editorFont),
            _ => null,
        };
    }
}
