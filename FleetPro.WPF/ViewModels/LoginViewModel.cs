using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FleetPro.Application.DTOs;
using FleetPro.Application.Interfaces;

namespace FleetPro.WPF.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthenticationService _authenticationService;


    public LoginViewModel(
        IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
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
        try
        {
            IsLoading = true;

            LoginMessage = "Checking credentials...";


            var request = new LoginRequest
            {
                Username = Username,
                Password = Password
            };


            var result =
                await _authenticationService
                .LoginAsync(request);


            if (result.Success)
            {
                LoginMessage =
                    $"Welcome {result.Username}\nRole: {result.Role}";
            }
            else
            {
                LoginMessage = result.Message;
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}