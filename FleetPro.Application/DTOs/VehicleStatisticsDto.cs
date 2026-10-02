namespace FleetPro.Application.DTOs;

public class VehicleStatisticsDto
{
    public int TotalVehicles { get; set; }

    public int AvailableVehicles { get; set; }

    public int AssignedVehicles { get; set; }

    public int VehiclesUnderMaintenance { get; set; }

    public int AccidentVehicles { get; set; }

    public int InactiveVehicles { get; set; }

    public int RetiredVehicles { get; set; }

    public int SoldVehicles { get; set; }
}