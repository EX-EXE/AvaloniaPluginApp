using Avalonia.Controls;
using AvaloniaPluginApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaPluginApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public static readonly ContentControl defaultControl = new ContentControl();

    public PluginService? PluginService { get; set; } = null;

    public MainViewModel()
        : base()
    {
    }

    public MainViewModel(PluginService pluginService)
        : this()
    {
        this.PluginService = pluginService;
    }

    public string Greeting => "Welcome to Avalonia!";

    [ObservableProperty]
    private PluginInfo? selectedPlugin = null;

    [ObservableProperty]
    private ContentControl control = defaultControl;

    [RelayCommand]
    public async Task RunAsync(PluginInfo info, CancellationToken token)
    {
        if (PluginService != null)
        {
            if (!info.IsLoaded)
            {
                await PluginService.LoadPluginAsync(info, token).ConfigureAwait(false);
                SelectedPlugin = info;
            }
            else
            {
                if (info == SelectedPlugin)
                {
                    SelectedPlugin = null;
                }
                await PluginService.UnloadPluginAsync(info, token).ConfigureAwait(false);
            }
        }
    }

    partial void OnSelectedPluginChanged(PluginInfo? value)
    {
        if (value != null && value.Data != null && value.Data.ControlInfo.MainControl != null)
        {
            Control = value.Data.ControlInfo.MainControl;
        }
        else
        {
            Control = defaultControl;
        }
    }
}
