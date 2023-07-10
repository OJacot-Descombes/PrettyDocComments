﻿using System;
using System.ComponentModel.Composition;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Microsoft;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Differencing;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Text.Outlining;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;
using PrettyDocComments.Model;
using PrettyDocComments.Services;

namespace PrettyDocComments;

/// <summary>
/// Provides a <see cref="LineTransformSource"/> for each supported editor view and passes it an
/// <see cref="Adornment"/>, so that doc comments can be properly sized and rendered.
/// </summary>
[Export(typeof(ILineTransformSourceProvider))]
[ContentType("code")]
[TextViewRole(PredefinedTextViewRoles.Document)]
internal sealed class LineTransformSourceProvider : ILineTransformSourceProvider
{
    private static readonly Regex _cSharpDocCommentRecoginzer = new(@"^\s*(///)([^/]|$)", RegexOptions.Compiled);
    private static readonly Regex _visualBasicDocCommentRecoginzer = new(@"^\s*(''')([^/]|$)", RegexOptions.Compiled);

    [SuppressMessage("Usage", "VSTHRD010:Invoke single-threaded types on Main thread", Justification = "<Pending>")]
    public LineTransformSourceProvider()
    {
#pragma warning disable VSSDK006 //  Check whether the result of GetService calls is null.
        var textManager = (IConnectionPointContainer)ServiceProvider.GlobalProvider.GetService(typeof(SVsTextManager));
#pragma warning restore VSSDK006
        Guid interfaceGuid = typeof(IVsTextManagerEvents).GUID;
        textManager.FindConnectionPoint(ref interfaceGuid, out var tmConnectionPoint);
        tmConnectionPoint.Advise(new TextManagerEvents(), out _);
    }

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
        // From https://github.com/microsoft/VS-PPT/blob/master/src/SyntacticFisheye/SyntacticFisheyeLineTransformSourceProvider.cs
        if (view.Roles.Contains(DifferenceViewerRoles.LeftViewTextViewRole) ||
            view.Roles.Contains(DifferenceViewerRoles.RightViewTextViewRole) ||
            view.Roles.Contains("VSMERGEDEFAULT" /*MergeViewerRoles.VSMergeDefaultRole from TFS*/)) {
            // Don't use line transforms for diff views since it will cause them to become misaligned.
            // Also, we don't want to prettyfy doc comments in diff views.
            return null;
        }

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