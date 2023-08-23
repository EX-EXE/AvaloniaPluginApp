using AvaloniaPluginBase;
using System.Collections.ObjectModel;

namespace AvaloniaPluginSample;

public class SamplePlugin : PluginBase
{

    public override ValueTask InitializeAsync(CancellationToken cancellationToken)
    {
        ControlInfo = new PluginControlInfo()
        {
            Name = "Name",
            MainControl = new MainControl()
            {
                DataContext = new MainViewModel()
            }
        };

        return ValueTask.CompletedTask;
    }
    public override ValueTask FinalizeAsync(CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

}