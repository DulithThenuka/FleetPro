namespace FleetPro.WPF.ViewModels;

public class VehicleEditRequestedEventArgs : EventArgs
{
    public int VehicleId { get; }

    public VehicleEditRequestedEventArgs(
        int vehicleId)
    {
        VehicleId = vehicleId;
    }
}