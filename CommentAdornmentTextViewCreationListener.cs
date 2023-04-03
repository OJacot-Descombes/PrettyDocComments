using System.ComponentModel.Composition;
using System.Drawing;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

namespace PrettyDocComments;

/// <summary>
/// Establishes an <see cref="IAdornmentLayer"/> to place the adornment on and exports the <see cref="IWpfTextViewCreationListener"/>
/// that instantiates the adornment on the event of a <see cref="IWpfTextView"/>'s creation
/// </summary>
[Export(typeof(IWpfTextViewCreationListener))]
[ContentType("code")]
[TextViewRole(PredefinedTextViewRoles.Document)]
internal sealed class CommentAdornmentTextViewCreationListener : IWpfTextViewCreationListener
{
    private static readonly Regex _cSharpDocCommentRecoginzer = new(@"^\s*(///)([^/]|$)", RegexOptions.Compiled);
    private static readonly Regex _visualBasicDocCommentRecoginzer = new(@"^\s*(''')([^/]|$)", RegexOptions.Compiled);

    // Disable "Field is never assigned to..." and "Field is never used" compiler's warnings. Justification: the field is used by MEF.
#pragma warning disable 649, 169

    /// <summary>
    /// Defines the adornment layer for the adornment. This layer is ordered
    /// after the selection layer in the Z-order
    /// </summary>
    [Export(typeof(AdornmentLayerDefinition))]
    [Name("TextAdornment")]
    [Order(After = PredefinedAdornmentLayers.Text, Before = PredefinedAdornmentLayers.Caret)]
    private AdornmentLayerDefinition _editorAdornmentLayer;

    [Import]
    private IVsFontsAndColorsInformationService _fontsAndColorsInformationService;

#pragma warning restore 649, 169

    private static IVsFontsAndColorsInformation TryGetFontAndColorInfo(IVsFontsAndColorsInformationService service)
    {
        var guidTextFileType = new Guid(2184822468u, 61063, 4560, 140, 152, 0, 192, 79, 194, 171, 34);
        var fonts = new Microsoft.VisualStudio.Editor.FontsAndColorsCategory(
            guidTextFileType,
            DefGuidList.guidTextEditorFontCategory,
            DefGuidList.guidTextEditorFontCategory);

        return (service?.GetFontAndColorInformation(fonts));
    }

    private Font GetEditorFont()
    {
        IVsFontsAndColorsInformation fontInfo = TryGetFontAndColorInfo(_fontsAndColorsInformationService);
        if (fontInfo == null) {
            return new System.Drawing.Font("Consolas", 15f);
        }
        FONTCOLORPREFERENCES2 preferences = fontInfo.GetFontAndColorPreferences();
        return System.Drawing.Font.FromHfont(preferences.hRegularViewFont);
    }

    #region IWpfTextViewCreationListener

    /// <summary>
    /// Called when a text view having matching roles is created over a text data model having a matching content type.
    /// Instantiates a TextAdornment manager when the textView is created.
    /// </summary>
    /// <param name="textView">The <see cref="IWpfTextView"/> upon which the adornment should be placed</param>
    public void TextViewCreated(IWpfTextView textView)
    {
        Font font = GetEditorFont();
        switch (textView.TextBuffer.ContentType.TypeName) {
            case "CSharp":
                new CommentAdornment(textView, _cSharpDocCommentRecoginzer, font);
                break;
            case "Basic":
                new CommentAdornment(textView, _visualBasicDocCommentRecoginzer, font);
                break;
        }
    }

    #endregion
}
