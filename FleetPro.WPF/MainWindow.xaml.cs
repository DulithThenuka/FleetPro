using System.Windows;
using FleetPro.WPF.ViewModels;

namespace FleetPro.WPF;

public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();

        DataContext = viewModel;
    }
}