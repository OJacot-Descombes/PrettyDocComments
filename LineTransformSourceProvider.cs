using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Text.Outlining;
using Microsoft.VisualStudio.Utilities;
using PrettyDocComments.Services;

namespace PrettyDocComments;

/// <summary>
/// Provides a <see cref="LineTransformSource"/> for each supported editor view and passes it an i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i i
/// <see cref="Adornment"/>, so that doc comments can be properly sized and rendered.
/// </summary>
[Export(typeof(ILineTransformSourceProvider))]
[ContentType("code")]
[TextViewRole(PredefinedTextViewRoles.Document)]
internal sealed class LineTransformSourceProvider : ILineTransformSourceProvider
{
    private static readonly Regex _cSharpDocCommentRecoginzer = new(@"^\s*(///)([^/]|$)", RegexOptions.Compiled);
    private static readonly Regex _visualBasicDocCommentRecoginzer = new(@"^\s*(''')([^/]|$)", RegexOptions.Compiled);

#pragma warning disable IDE0044, IDE0051, CS0649, CS0169 // Add readonly modifier, Remove unused private members.

    /// <summary>
    /// Defines the adornment layer for the adornment. This layer is ordered after the text layer in the Z-order.
    /// </summary>
    [Export(typeof(AdornmentLayerDefinition))]
    [Name("TextAdornment")]
    [Order(After = PredefinedAdornmentLayers.Text, Before = PredefinedAdornmentLayers.Caret)]
    private AdornmentLayerDefinition _editorAdornmentLayer;

    [Import]
    private IVsFontsAndColorsInformationService _fontsAndColorsInformationService;

#pragma warning restore  IDE0044, IDE0051, CS0649, CS0169

    public ILineTransformSource Create(IWpfTextView view)
    {
        Regex docCommentRegex = view.TextBuffer.ContentType.TypeName switch {
            "CSharp" or "F#" or "C/C++" => _cSharpDocCommentRecoginzer,
            "Basic" => _visualBasicDocCommentRecoginzer,
            _ => null
        };

        if (docCommentRegex != null) {
            return new LineTransformSource(view, GetOutliningManager(view), docCommentRegex);
        }
        return null;
    }

    [SuppressMessage("Usage", "VSTHRD010:Invoke single-threaded types on Main thread", Justification = "<Pending>")]
    private IOutliningManager GetOutliningManager(IWpfTextView textView) =>
        ((IComponentModel)ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel)))
            ?.GetService<IOutliningManagerService>()?.GetOutliningManager(textView);
}