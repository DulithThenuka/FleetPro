namespace FleetPro.Domain.Entities;

public class VehicleType
{
    public int VehicleTypeId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public ICollection<Vehicle> Vehicles { get; set; }
        = new List<Vehicle>();
}