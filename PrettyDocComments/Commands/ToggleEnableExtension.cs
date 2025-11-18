using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using PrettyDocComments.CustomOptions;
using PrettyDocComments.Helpers;
using Task = System.Threading.Tasks.Task;

namespace PrettyDocComments.Commands;

/// <summary>
/// Command handler
/// </summary>
internal sealed class ToggleEnableExtension : Command
{
    /// <summary>
    /// Command ID.
    /// </summary>
    override protected int CommandId => 0x0200;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToggleEnableExtension"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    private ToggleEnableExtension(AsyncPackage package, OleMenuCommandService commandService)
        : base(package, commandService)
    {
    }

    /// <summary>
    /// This function is the callback used to execute the command when the menu item is clicked.
    /// See the constructor to see how the menu item is associated with this function using
    /// OleMenuCommandService service and MenuCommand class.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    protected override void Execute(object sender, EventArgs e)
    {
        GeneralOptions.Instance.Enabled = !GeneralOptions.Instance.Enabled;
        Options.RefreshViews();
        GeneralOptions.Instance.Save();
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static ToggleEnableExtension Instance
    {
        get;
        private set;
    }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package)
    {
        // Switch to the main thread - the call to AddCommand in ToggleEnableExtension's constructor requires
        // the UI thread.
        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

        var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
        Instance = new ToggleEnableExtension(package, commandService);
    }
}
