using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FleetPro.Application.Interfaces;

namespace FleetPro.WPF.ViewModels;

public partial class CompleteAssignmentViewModel : ObservableObject
{
    private readonly IDriverService _driverService;

    private int _assignmentId;

    [ObservableProperty]
    private string driverName = string.Empty;

    [ObservableProperty]
    private string vehicleName = string.Empty;

    [ObservableProperty]
    private DateTime endDate = DateTime.Today;

    [ObservableProperty]
    private string endMileage = string.Empty;

    [ObservableProperty]
    private string notes = string.Empty;

    [ObservableProperty]
    private string formMessage = string.Empty;

    [ObservableProperty]
    private bool isSaving;

    public event EventHandler? RequestClose;

    public CompleteAssignmentViewModel(IDriverService driverService)
    {
        _driverService = driverService;
    }

    public void SetAssignment(
        int assignmentId,
        string driver,
        string vehicle)
    {
        _assignmentId = assignmentId;

        DriverName = driver;
        VehicleName = vehicle;

        EndDate = DateTime.Today;
        EndMileage = string.Empty;
        Notes = string.Empty;
        FormMessage = string.Empty;
    }

    [RelayCommand]
    private async Task Save()
    {
        if (IsSaving)
            return;

        FormMessage = string.Empty;

        if (EndDate.Date > DateTime.Today)
        {
            FormMessage = "End date cannot be in the future.";
            return;
        }

        decimal? mileage = null;

        if (!string.IsNullOrWhiteSpace(EndMileage))
        {
            if (!decimal.TryParse(EndMileage, out var parsedMileage))
            {
                FormMessage = "Enter a valid end mileage.";
                return;
            }

            mileage = parsedMileage;
        }

        try
        {
            IsSaving = true;

            await _driverService.CompleteAssignmentAsync(
                _assignmentId,
                EndDate,
                mileage,
                string.IsNullOrWhiteSpace(Notes)
                    ? null
                    : Notes.Trim());

            RequestClose?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            FormMessage = ex.Message;
        }
        finally
        {
            IsSaving = false;
        }
    }

    [RelayCommand]
    private void Cancel()
    {
        RequestClose?.Invoke(this, EventArgs.Empty);
    }
}