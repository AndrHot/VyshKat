using VyshKat.Domain;

namespace VyshKat.Tests;

public class DomainTests
{
    [Theory]
    [InlineData(0, 50)]
    [InlineData(11, 50)]
    [InlineData(5, -1)]
    [InlineData(5, 101)]
    public void Vehicle_WithInvalidCharacteristics_Throws(int simplicity, int condition)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Bicycle("Велик", simplicity, condition));
    }

    [Fact]
    public void InvalidNameOrEnergy_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Helmet("  "));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Scooter("Самокат", 5, 90, -0.1m));
        Assert.Throws<ArgumentOutOfRangeException>(() => new DockStation("Док", -1m));
    }

    [Fact]
    public void Fleet_AssignsSequentialNumbers_AndRejectsDuplicates()
    {
        var fleet = new Fleet();
        var helmet = new Helmet("Шлем");
        var bike = new Bicycle("Велик", 9, 90);
        fleet.Register(helmet);
        fleet.Register(bike);

        Assert.Equal("VK-0001", helmet.InventoryNumber);
        Assert.Equal("VK-0002", bike.InventoryNumber);
        Assert.Throws<InvalidOperationException>(() => fleet.Register(helmet));
        Assert.Equal(2, fleet.Assets.Count);
        Assert.Equal("VK-0001  Шлем «Шлем»", helmet.ToString());
    }
}
