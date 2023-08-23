using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using AvaloniaPluginApp.Services;
using AvaloniaPluginApp.ViewModels;
using AvaloniaPluginApp.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Runtime.InteropServices.JavaScript;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaPluginApp;

public partial class App : Application, IDisposable
{
    public string[] Args { get; set; } = Array.Empty<string>();

    private IHost? host = null;
    private Task? hostTask = null;
    private CancellationTokenSource hostTaskCancellationTokenSource = new CancellationTokenSource();

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public void Dispose()
    {
        if (hostTask != null)
        {
            hostTaskCancellationTokenSource.Cancel();
            hostTask.GetAwaiter().GetResult();
            hostTask = null;
        }
        if (host != null)
        {
            host.Dispose();
        }
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var args = Array.Empty<string>();
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            args = desktopLifetime.Args;
        }
        host = Host.CreateDefaultBuilder(args)
        .ConfigureServices((_, services) =>
        {
            services.AddTransient<MainWindow>();
            services.AddTransient<MainView>();
            services.AddTransient<MainViewModel>();

            services.AddSingleton<PluginService>();
            services.AddHostedService<PluginService>(x => x.GetRequiredService<PluginService>());
        })
        .Build();

        // Line below is needed to remove Avalonia data validation.
        // Without this line you will get duplicate validations from both Avalonia and CT
        BindingPlugins.DataValidators.RemoveAt(0);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var window = host.Services.GetRequiredService<MainWindow>();
            window.DataContext = host.Services.GetRequiredService<MainViewModel>();
            desktop.MainWindow = window;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            var view = host.Services.GetRequiredService<MainView>();
            view.DataContext = host.Services.GetRequiredService<MainViewModel>();
            singleViewPlatform.MainView = view;
        }

        var cancellationToken = hostTaskCancellationTokenSource.Token;
        hostTask = Task.Run(() => host.RunAsync(cancellationToken), cancellationToken);
        base.OnFrameworkInitializationCompleted();
    }
}
