using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FleetPro.Application.DTOs;
using FleetPro.Application.Interfaces;
using System.Collections.ObjectModel;

namespace FleetPro.WPF.ViewModels;

public partial class DriverFormViewModel : ObservableObject
{
    private readonly IDriverService _driverService;

    [ObservableProperty]
    private string employeeNumber = string.Empty;

    [ObservableProperty]
    private string fullName = string.Empty;

    [ObservableProperty]
    private string nic = string.Empty;

    [ObservableProperty]
    private string phone = string.Empty;

    [ObservableProperty]
    private string address = string.Empty;

    [ObservableProperty]
    private string licenseNumber = string.Empty;

    [ObservableProperty]
    private string licenseCategory = string.Empty;

    [ObservableProperty]
    private DateTime? licenseExpiryDate = DateTime.Today.AddYears(1);

    [ObservableProperty]
    private DateTime? joiningDate = DateTime.Today;

    [ObservableProperty]
    private LookupItemDto? selectedBranch;

    [ObservableProperty]
    private ObservableCollection<LookupItemDto> branches = new();

    [ObservableProperty]
    private bool isSaving;

    [ObservableProperty]
    private string formMessage = string.Empty;

    public string FormTitle => "Add Driver";
    public string SaveButtonText => "Save Driver";

    public event EventHandler? RequestClose;

    public DriverFormViewModel(IDriverService driverService)
    {
        _driverService = driverService;
    }

    public async Task LoadAsync()
{
    try
    {
        FormMessage = string.Empty;

        var branchItems = await _driverService.GetBranchesAsync();

        SetBranches(branchItems);
    }
    catch (Exception ex)
    {
        FormMessage = $"Failed to load branches: {ex.Message}";
    }
}

    public void SetBranches(IEnumerable<LookupItemDto> branchItems)
    {
        Branches = new ObservableCollection<LookupItemDto>(branchItems);

        if (Branches.Count > 0)
            SelectedBranch = Branches[0];
    }

    [RelayCommand]
    private async Task Save()
    {
        if (IsSaving)
            return;

        FormMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(EmployeeNumber))
        {
            FormMessage = "Employee number is required.";
            return;
        }

        if (string.IsNullOrWhiteSpace(FullName))
        {
            FormMessage = "Full name is required.";
            return;
        }

        if (string.IsNullOrWhiteSpace(LicenseNumber))
        {
            FormMessage = "License number is required.";
            return;
        }

        if (SelectedBranch is null)
        {
            FormMessage = "Please select a branch.";
            return;
        }

        if (LicenseExpiryDate.HasValue &&
            LicenseExpiryDate.Value.Date < DateTime.Today)
        {
            FormMessage = "Driver license has already expired.";
            return;
        }

        try
        {
            IsSaving = true;

            var request = new CreateDriverRequest
            {
                EmployeeNumber = EmployeeNumber.Trim(),
                FullName = FullName.Trim(),
                NIC = string.IsNullOrWhiteSpace(Nic) ? null : Nic.Trim(),
                Phone = string.IsNullOrWhiteSpace(Phone) ? null : Phone.Trim(),
                Address = string.IsNullOrWhiteSpace(Address) ? null : Address.Trim(),
                LicenseNumber = LicenseNumber.Trim(),
                LicenseCategory = string.IsNullOrWhiteSpace(LicenseCategory)
                    ? null
                    : LicenseCategory.Trim(),
                LicenseExpiryDate = LicenseExpiryDate,
                JoiningDate = JoiningDate,
                BranchId = SelectedBranch.Id
            };

            await _driverService.CreateAsync(request);

            FormMessage = "Driver created successfully.";

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