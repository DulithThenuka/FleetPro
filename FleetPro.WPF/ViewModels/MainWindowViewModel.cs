using CommunityToolkit.Mvvm.ComponentModel;

namespace FleetPro.WPF.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableObject? currentViewModel;

    public void ShowLogin(
        LoginViewModel loginViewModel)
    {
        CurrentViewModel = loginViewModel;
    }

    public void ShowShell(
        MainShellViewModel shellViewModel)
    {
        CurrentViewModel = shellViewModel;
    }
}