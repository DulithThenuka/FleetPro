using CommunityToolkit.Mvvm.ComponentModel;

namespace FleetPro.WPF.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string appTitle = "FleetPro";

    [ObservableProperty]
    private string welcomeMessage =
        "Smart Fleet & Vehicle Service Management System";

    [ObservableProperty]
    private string systemStatus = "System is running";
}