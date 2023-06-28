using Microsoft.VisualStudio.TextManager.Interop;

namespace PrettyDocComments;

internal sealed class TextManagerEvents : IVsTextManagerEvents
{
    public static event Action UserPreferencesChanged;

    void IVsTextManagerEvents.OnRegisterMarkerType(int iMarkerType)
    {
    }

    void IVsTextManagerEvents.OnRegisterView(IVsTextView pView)
    {
    }

    void IVsTextManagerEvents.OnUnregisterView(IVsTextView pView)
    {
    }

    void IVsTextManagerEvents.OnUserPreferencesChanged(VIEWPREFERENCES[] pViewPrefs, FRAMEPREFERENCES[] pFramePrefs, LANGPREFERENCES[] pLangPrefs, FONTCOLORPREFERENCES[] pColorPrefs)
    {
        UserPreferencesChanged?.Invoke();
    }
}
