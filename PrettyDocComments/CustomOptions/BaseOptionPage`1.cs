﻿using Microsoft.VisualStudio.Shell;
using PrettyDocComments.Helpers;

namespace PrettyDocComments.CustomOptions;

// From: https://github.com/madskristensen/OptionsSample, applied some code fixes.

/// <summary>
/// A base class for a DialogPage to show in Tools -> Options.
/// </summary>
internal class BaseOptionPage<T> : DialogPage where T : BaseOptionModel<T>, new()
{
    private readonly BaseOptionModel<T> _model;

    public BaseOptionPage()
    {
#pragma warning disable VSTHRD102 // Implement internal logic asynchronously
        _model = ThreadHelper.JoinableTaskFactory.Run(BaseOptionModel<T>.CreateAsync);
#pragma warning restore VSTHRD102 // Implement internal logic asynchronously
    }

    public override object AutomationObject => _model;

    public override void LoadSettingsFromStorage()
    {
        _model.Load();
        Options.Refresh();
    }

    public override void SaveSettingsToStorage()
    {
        _model.Save();
        Options.Refresh();
    }
}
