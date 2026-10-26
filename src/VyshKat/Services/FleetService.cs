using VyshKat.Domain;

namespace VyshKat.Services;

public sealed class FleetService(Fleet fleet, IServiceCenter serviceCenter)
{
    public InspectionResult AcceptVehicle(Vehicle vehicle)
    {
        var result = serviceCenter.Inspect(vehicle);
        if (result.Accepted)
            fleet.Register(vehicle);
        return result;
    }

    public void RegisterEquipment(Equipment equipment) => fleet.Register(equipment);
}
