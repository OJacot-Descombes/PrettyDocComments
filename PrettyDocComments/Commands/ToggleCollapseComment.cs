using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using PrettyDocComments.CustomOptions;
using PrettyDocComments.Helpers;
using Task = System.Threading.Tasks.Task;

namespace PrettyDocComments.Commands;
/// <summary>
/// Command handler
/// </summary>
internal sealed class ToggleCollapseComment
{
    /// <summary>
    /// Command ID.
    /// </summary>
    public const int CommandId = 0x0100;

    /// <summary>
    /// Command menu group (command set GUID).
    /// </summary>
    public static readonly Guid CommandSet = new("54d1f8d3-3afd-431f-adc9-5e5236c31dad");

    /// <summary>
    /// VS Package that provides this command, not null.
    /// </summary>
    private readonly AsyncPackage _package;

    /// <summary>
    /// Initializes a new instance of the <see cref="ToggleCollapseComment"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    private ToggleCollapseComment(AsyncPackage package, OleMenuCommandService commandService)
    {
        _package = package ?? throw new ArgumentNullException(nameof(package));
        commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

        var menuCommandID = new CommandID(CommandSet, CommandId);
        var menuItem = new MenuCommand(Execute, menuCommandID);
        commandService.AddCommand(menuItem);
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static ToggleCollapseComment Instance
    {
        get;
        private set;
    }

    /// <summary>
    /// Gets the service provider from the owner package.
    /// </summary>
#pragma warning disable IDE0051 // Remove unused private members
    private IAsyncServiceProvider ServiceProvider
#pragma warning restore IDE0051 // Remove unused private members
    {
        get {
            return _package;
        }
    }

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package)
    {
        // Switch to the main thread - the call to AddCommand in ToggleCollapseComment's constructor requires
        // the UI thread.
        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

        var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
        Instance = new ToggleCollapseComment(package, commandService);
    }

    /// <summary>
    /// This function is the callback used to execute the command when the menu item is clicked.
    /// See the constructor to see how the menu item is associated with this function using
    /// OleMenuCommandService service and MenuCommand class.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    private void Execute(object sender, EventArgs e)
    {
        GeneralOptions.Instance.CollapseToSummary = !GeneralOptions.Instance.CollapseToSummary;
        Options.RefreshViews();
        GeneralOptions.Instance.Save();
    }
}
