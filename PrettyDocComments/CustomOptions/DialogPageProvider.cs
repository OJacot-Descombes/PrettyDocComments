namespace PrettyDocComments.CustomOptions;

// From: https://github.com/madskristensen/OptionsSample

/// <summary>
/// A provider for custom <see cref="DialogPage" /> implementations.
/// </summary>
internal class DialogPageProvider
{
    public class General : BaseOptionPage<GeneralOptions> { }
}