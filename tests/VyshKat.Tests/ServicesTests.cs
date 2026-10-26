using Microsoft.Extensions.DependencyInjection;
using VyshKat.Domain;
using VyshKat.Services;
using VyshKat.Tests.Fakes;

namespace VyshKat.Tests;

public class ServicesTests
{
    private static ServiceProvider BuildProvider(IServiceCenter serviceCenter) =>
        new ServiceCollection().AddVyshKat().AddSingleton(serviceCenter).BuildServiceProvider();

    [Theory]
    [InlineData(60, true)]
    [InlineData(59, false)]
    public void ServiceCenter_AcceptsFromSixtyPercent(int condition, bool expected)
    {
        Assert.Equal(expected, new ServiceCenter().Inspect(new Bicycle("Велик", 5, condition)).Accepted);
    }

    [Theory]
    [InlineData(5, false)]
    [InlineData(6, true)]
    public void BeginnerPolicy_RequiresSimplicityAtLeastSix(int simplicity, bool expected)
    {
        Assert.Equal(expected, new SimplicityBeginnerPolicy().IsSuitable(new Bicycle("Велик", simplicity, 90)));
    }

    [Fact]
    public void AcceptVehicle_UsesServiceCenterFromContainer_WhenAccepted()
    {
        var fake = new FakeServiceCenter(accept: true);
        using var provider = BuildProvider(fake);
        var scooter = new Scooter("G30", 7, 10, 0.5m);

        var result = provider.GetRequiredService<FleetService>().AcceptVehicle(scooter);

        Assert.True(result.Accepted);
        Assert.Equal("VK-0001", scooter.InventoryNumber);
        Assert.Same(scooter, Assert.Single(fake.Inspected));
    }

    [Fact]
    public void AcceptVehicle_WhenRejected_VehicleIsNotInFleet()
    {
        var fake = new FakeServiceCenter(accept: false);
        using var provider = BuildProvider(fake);
        var scooter = new Scooter("G30", 7, 100, 0.5m);

        var result = provider.GetRequiredService<FleetService>().AcceptVehicle(scooter);

        Assert.False(result.Accepted);
        Assert.Null(scooter.InventoryNumber);
        Assert.Empty(provider.GetRequiredService<Fleet>().Assets);
    }

    [Fact]
    public void RegisterEquipment_SkipsInspection()
    {
        var fake = new FakeServiceCenter(accept: false);
        var fleet = new Fleet();

        new FleetService(fleet, fake).RegisterEquipment(new Helmet("Шлем"));

        Assert.Single(fleet.Assets);
        Assert.Empty(fake.Inspected);
    }

    [Fact]
    public void Reports_CountEnergyBeginnersAndInventory()
    {
        var fleet = new Fleet();
        var easyBike = new Bicycle("Велик", 9, 90);
        fleet.Register(new Scooter("Самокат", 4, 90, 0.5m));
        fleet.Register(easyBike);
        fleet.Register(new Helmet("Шлем"));
        fleet.Register(new ChargingCabinet("Шкаф", 6m));
        var reports = new FleetReports(fleet, new SimplicityBeginnerPolicy());

        Assert.Equal(4, reports.TotalUnits);
        Assert.Equal(2, reports.VehicleCount);
        Assert.Equal(2, reports.EquipmentCount);
        Assert.Equal(6.5m, reports.TotalDailyEnergyKwh);
        Assert.Same(easyBike, Assert.Single(reports.BeginnerFriendlyVehicles()));
        Assert.Equal("VK-0001", reports.InventoryList().First().InventoryNumber);
    }
}
