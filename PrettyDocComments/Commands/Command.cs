using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;

namespace PrettyDocComments.Commands;

/// <summary>
/// Command handler base class
/// </summary>
internal abstract class Command
{
    /// <summary>
    /// Command menu group (command set GUID).
    /// </summary>
    protected static readonly Guid CommandSet = new("54d1f8d3-3afd-431f-adc9-5e5236c31dad");

    /// <summary>
    /// VS Package that provides this command, not null.
    /// </summary>
    private readonly AsyncPackage _package;

    /// <summary>
    /// Represents the menu command associated with this instance.
    /// </summary>
    /// <remarks>This field is intended to store a reference to a <see cref="OleMenuCommand"/> object,  which can
    /// be used to define or execute specific actions within a menu system.</remarks>
    private readonly OleMenuCommand _menuItem;

    /// <summary>
    /// Command ID.
    /// </summary>
    protected abstract int CommandId { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ToggleEnableExtension"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    protected Command(AsyncPackage package, OleMenuCommandService commandService)
    {
        _package = package ?? throw new ArgumentNullException(nameof(package));
        commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

        var menuCommandID = new CommandID(CommandSet, CommandId);
        _menuItem = new OleMenuCommand(Execute, menuCommandID);
        commandService.AddCommand(_menuItem);
    }

    /// <summary>
    /// This function is the callback used to execute the command when the menu item is clicked.
    /// See the constructor to see how the menu item is associated with this function using
    /// OleMenuCommandService service and MenuCommand class.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    protected abstract void Execute(object sender, EventArgs e);
}
