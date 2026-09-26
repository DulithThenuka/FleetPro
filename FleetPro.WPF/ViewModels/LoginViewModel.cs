using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FleetPro.WPF.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string loginMessage = string.Empty;

    [RelayCommand]
    private void Login()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            LoginMessage = "Please enter your username.";
            return;
        }

        LoginMessage = $"Login request received for {Username}.";
    }
}