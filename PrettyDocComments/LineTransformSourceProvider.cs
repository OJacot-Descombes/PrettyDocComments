using Microsoft;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Differencing;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Text.Outlining;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using PrettyDocComments.CustomOptions;
using PrettyDocComments.Services;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace PrettyDocComments;

/// <summary>
/// Provides a <see cref="LineTransformSource"/> for each supported editor view and passes it an
/// <see cref="Adornment"/>, so that doc comments can be properly sized and rendered.
/// </summary>
[Export(typeof(ILineTransformSourceProvider))]
[ContentType("text")]
[TextViewRole(PredefinedTextViewRoles.Document)]
[TextViewRole(PredefinedTextViewRoles.EmbeddedPeekTextView)]
[TextViewRole(PredefinedTextViewRoles.PreviewTextView)]
[Name("PrettyDocCommentsLineTransformSourceProvider")]
internal sealed class LineTransformSourceProvider : ILineTransformSourceProvider
{
    private static readonly Regex _cSharpDocCommentRecognizer = new(@"^\s*(///)([^/]|$)", RegexOptions.Compiled);
    private static readonly Regex _visualBasicDocCommentRecognizer = new(@"^\s*(''')([^/]|$)", RegexOptions.Compiled);

    [SuppressMessage("Usage", "VSTHRD010:Invoke single-threaded types on Main thread", Justification = "<Pending>")]
    public LineTransformSourceProvider()
    {
        var textManager = (IConnectionPointContainer)ServiceProvider.GlobalProvider.GetService(typeof(SVsTextManager));
        Assumes.Present(textManager);
        Guid interfaceGuid = typeof(IVsTextManagerEvents).GUID;
        textManager.FindConnectionPoint(ref interfaceGuid, out var tmConnectionPoint);
        tmConnectionPoint.Advise(new TextManagerEvents(), out _);
    }

#pragma warning disable IDE0044, IDE0051, CS0649, CS0169 // Add readonly modifier, Remove unused private members.

    /// <summary>
    /// Defines the adornment layer for the adornment. This layer is ordered after the text layer in the Z-order.
    /// </summary>
    [Export(typeof(AdornmentLayerDefinition))]
    [Name("DocCommentImage")]
    [Order(After = PredefinedAdornmentLayers.Caret)]
    private AdornmentLayerDefinition _editorAdornmentLayer;

    [Import]
    private IVsFontsAndColorsInformationService _fontsAndColorsInformationService;

#pragma warning restore  IDE0044, IDE0051, CS0649, CS0169

    public ILineTransformSource Create(IWpfTextView view)
    {
        // From https://github.com/microsoft/VS-PPT/blob/master/src/SyntacticFisheye/SyntacticFisheyeLineTransformSourceProvider.cs
        if (view.Roles.Contains(DifferenceViewerRoles.LeftViewTextViewRole) ||
            view.Roles.Contains(DifferenceViewerRoles.RightViewTextViewRole) ||
            view.Roles.Contains("VSMERGEDEFAULT" /*MergeViewerRoles.VSMergeDefaultRole from TFS*/)) {
            // Don't use line transforms for diff views since it will cause them to become misaligned.
            // Also, we don't want to prettyfy doc comments in diff views.
            return null;
        }

        Regex docCommentRegex = view.TextBuffer.ContentType.TypeName switch {
            "CSharp" or "F#" or "FSharp" or "C" or "C/C++" => _cSharpDocCommentRecognizer,
            "Basic" or "VisualBasic" => _visualBasicDocCommentRecognizer,
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