namespace VyshKat.Domain;

public interface IInventoried
{
    string Name { get; }
    string? InventoryNumber { get; }
}

public interface IEnergyConsumer
{
    decimal DailyEnergyKwh { get; }
}

public abstract class Asset : IInventoried
{
    protected Asset(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Наименование не может быть пустым.");
        Name = name.Trim();
    }

    public string Name { get; }
    public string? InventoryNumber { get; internal set; }
    public abstract string KindName { get; }

    public override string ToString() => $"{InventoryNumber}  {KindName} «{Name}»";

    protected static int CheckRange(int value, int min, int max)
    {
        if (value < min || value > max)
            throw new ArgumentOutOfRangeException(nameof(value), $"Допустимо от {min} до {max}.");
        return value;
    }

    protected static decimal CheckEnergy(decimal kwh)
    {
        if (kwh < 0)
            throw new ArgumentOutOfRangeException(nameof(kwh), "Расход не может быть отрицательным.");
        return kwh;
    }
}
