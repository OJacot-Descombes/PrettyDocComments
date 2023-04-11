using System.ComponentModel.Composition;
using System.Drawing;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Editor;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Text.Outlining;
using Microsoft.VisualStudio.TextManager.Interop;
using Microsoft.VisualStudio.Utilities;

namespace PrettyDocComments;

/// <summary>
/// Provides a <see cref="LineTransformSource"/> for each supported editor view and passes it a
/// <see cref="CommentAdornment"/>, so that doc comments can be properly sized and rendered.
/// </summary>
[Export(typeof(ILineTransformSourceProvider))]
[ContentType("code")]
[TextViewRole(PredefinedTextViewRoles.Document)]
internal sealed class LineTransformSourceProvider : ILineTransformSourceProvider
{
#pragma warning disable IDE0044, IDE0051 // Add readonly modifier, Remove unused private members.

    /// <summary>
    /// Defines the adornment layer for the adornment. This layer is ordered after the text layer in the Z-order.
    /// </summary>
    [Export(typeof(AdornmentLayerDefinition))]
    [Name("TextAdornment")]
    [Order(After = PredefinedAdornmentLayers.Text, Before = PredefinedAdornmentLayers.Caret)]
    private AdornmentLayerDefinition _editorAdornmentLayer;

    [Import]
    private IVsFontsAndColorsInformationService _fontsAndColorsInformationService;

#pragma warning restore IDE0044, IDE0051

    public ILineTransformSource Create(IWpfTextView textView)
    {
        if (AdornmentFactory.Create(textView, GetEditorFont()) is { } adornment) {
            return new LineTransformSource(textView, GetOutliningManager(textView), adornment);
        }
        return null;
    }

    private IOutliningManager GetOutliningManager(IWpfTextView textView) =>
        ((IComponentModel)ServiceProvider.GlobalProvider.GetService(typeof(SComponentModel)))
            ?.GetService<IOutliningManagerService>()?.GetOutliningManager(textView);

    private Font GetEditorFont()
    {
        IVsFontsAndColorsInformation fontInfo = TryGetFontAndColorInfo(_fontsAndColorsInformationService);
        if (fontInfo == null) {
            return new Font("Consolas", 15f);
        }
        FONTCOLORPREFERENCES2 preferences = fontInfo.GetFontAndColorPreferences();
        return Font.FromHfont(preferences.hRegularViewFont);
    }

    private static IVsFontsAndColorsInformation TryGetFontAndColorInfo(IVsFontsAndColorsInformationService service)
    {
        var guidTextFileType = new Guid(0x8239BEC4u, 0xEE87, 0x11D0, 0x8C, 0x98, 0x00, 0xC0, 0x4F, 0xC2, 0xAB, 0x22);
        var fonts = new FontsAndColorsCategory(
            guidTextFileType,
            Microsoft.VisualStudio.Editor.DefGuidList.guidTextEditorFontCategory,
            Microsoft.VisualStudio.Editor.DefGuidList.guidTextEditorFontCategory);

        return (service?.GetFontAndColorInformation(fonts));
    }
}