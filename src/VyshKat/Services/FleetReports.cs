using VyshKat.Domain;

namespace VyshKat.Services;

public sealed class FleetReports(Fleet fleet, IBeginnerPolicy beginnerPolicy)
{
    public int TotalUnits => fleet.Assets.Count;

    public int VehicleCount => fleet.Assets.OfType<Vehicle>().Count();

    public int EquipmentCount => fleet.Assets.OfType<Equipment>().Count();

    public decimal TotalDailyEnergyKwh => fleet.Assets.OfType<IEnergyConsumer>().Sum(c => c.DailyEnergyKwh);

    public IEnumerable<Vehicle> BeginnerFriendlyVehicles() =>
        fleet.Assets.OfType<Vehicle>().Where(beginnerPolicy.IsSuitable);

    public IEnumerable<IInventoried> InventoryList() => fleet.Assets;
}
