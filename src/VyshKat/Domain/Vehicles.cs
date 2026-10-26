namespace VyshKat.Domain;

public abstract class Vehicle(string name, int simplicity, int conditionPercent) : Asset(name)
{
    public int Simplicity { get; } = CheckRange(simplicity, 1, 10);
    public int ConditionPercent { get; } = CheckRange(conditionPercent, 0, 100);
}

public sealed class Scooter(string name, int simplicity, int conditionPercent, decimal dailyEnergyKwh)
    : Vehicle(name, simplicity, conditionPercent), IEnergyConsumer
{
    public decimal DailyEnergyKwh { get; } = CheckEnergy(dailyEnergyKwh);
    public override string KindName => "Электросамокат";
}

public sealed class ElectricBicycle(string name, int simplicity, int conditionPercent, decimal dailyEnergyKwh)
    : Vehicle(name, simplicity, conditionPercent), IEnergyConsumer
{
    public decimal DailyEnergyKwh { get; } = CheckEnergy(dailyEnergyKwh);
    public override string KindName => "Электровелосипед";
}

public sealed class Bicycle(string name, int simplicity, int conditionPercent)
    : Vehicle(name, simplicity, conditionPercent)
{
    public override string KindName => "Велосипед";
}
