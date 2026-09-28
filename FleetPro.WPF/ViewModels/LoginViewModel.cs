using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FleetPro.Application.DTOs;
using FleetPro.Application.Interfaces;

namespace FleetPro.WPF.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthenticationService
        _authenticationService;

    private readonly MainWindowViewModel
        _mainWindowViewModel;

    private readonly MainShellViewModel
        _mainShellViewModel;


    public LoginViewModel(
        IAuthenticationService authenticationService,
        MainWindowViewModel mainWindowViewModel,
        MainShellViewModel mainShellViewModel)
    {
        _authenticationService =
            authenticationService;

        _mainWindowViewModel =
            mainWindowViewModel;

        _mainShellViewModel =
            mainShellViewModel;
    }


    [ObservableProperty]
    private string username = string.Empty;


    [ObservableProperty]
    private string password = string.Empty;


    [ObservableProperty]
    private string loginMessage = string.Empty;


    [ObservableProperty]
    private bool isLoading;


    [RelayCommand]
    private async Task Login()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            LoginMessage =
                "Please enter your username.";

            return;
        }


        if (string.IsNullOrWhiteSpace(Password))
        {
            LoginMessage =
                "Please enter your password.";

            return;
        }


        try
        {
            IsLoading = true;

            LoginMessage =
                "Checking credentials...";


            var request = new LoginRequest
            {
                Username = Username,
                Password = Password
            };


            var result =
                await _authenticationService
                .LoginAsync(request);


            if (!result.Success)
            {
                LoginMessage =
                    result.Message;

                return;
            }


            _mainShellViewModel.SetUser(
                result.Username!,
                result.Role ?? "Unknown");


            _mainWindowViewModel.ShowShell(
                _mainShellViewModel);
        }
        catch
        {
            LoginMessage =
                "An unexpected error occurred. Please try again.";
        }
        finally
        {
            IsLoading = false;
        }
    }
}