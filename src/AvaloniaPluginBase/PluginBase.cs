using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaPluginBase;

public interface IPluginBase
{
    ValueTask InitializeAsync(CancellationToken cancellationToken);
    ValueTask FinalizeAsync(CancellationToken cancellationToken);

    PluginControlInfo ControlInfo { get; }
}

public partial class PluginBase : ObservableObject, IPluginBase
{
    [ObservableProperty]
    public PluginControlInfo? _controlInfo;

    public virtual ValueTask InitializeAsync(CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }
    public virtual ValueTask FinalizeAsync(CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }
}

public partial class PluginControlInfo : ObservableObject
{
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private UserControl? _mainControl = null;

    [ObservableProperty]
    private UserControl? _settingControl = null;
}

