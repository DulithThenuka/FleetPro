using System.Windows.Controls;
using FleetPro.WPF.ViewModels;

namespace FleetPro.WPF.Views;

public partial class LoginView : UserControl
{
    public LoginView(LoginViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;
    }
}