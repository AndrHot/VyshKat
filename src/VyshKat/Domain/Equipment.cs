namespace VyshKat.Domain;

public abstract class Equipment(string name) : Asset(name);

public sealed class Helmet(string name) : Equipment(name)
{
    public override string KindName => "Шлем";
}

public sealed class DockStation(string name, decimal dailyEnergyKwh) : Equipment(name), IEnergyConsumer
{
    public decimal DailyEnergyKwh { get; } = CheckEnergy(dailyEnergyKwh);
    public override string KindName => "Док-станция";
}

public sealed class ChargingCabinet(string name, decimal dailyEnergyKwh) : Equipment(name), IEnergyConsumer
{
    public decimal DailyEnergyKwh { get; } = CheckEnergy(dailyEnergyKwh);
    public override string KindName => "Зарядный шкаф";
}
