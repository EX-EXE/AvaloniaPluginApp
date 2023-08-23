using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaPluginSample;

internal partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private DateTimeOffset _create = DateTimeOffset.Now;
}
