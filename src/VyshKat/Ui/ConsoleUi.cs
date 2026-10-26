using System.Globalization;
using VyshKat.Domain;
using VyshKat.Services;

namespace VyshKat.Ui;

public sealed class ConsoleUi(FleetService fleetService, FleetReports reports, TextReader input, TextWriter output)
{
    public void Run()
    {
        try
        {
            while (true)
            {
                output.WriteLine("""

                    1. Принять транспорт (с техосмотром)
                    2. Поставить вещь на баланс
                    3. Отчёт: количество и энергия
                    4. Транспорт для новичков
                    5. Инвентарный список
                    6. Демо-сценарий
                    0. Выход
                    """);
                switch (Read("Пункт: "))
                {
                    case "0": output.WriteLine("До встречи!"); return;
                    case "1": AddVehicle(); break;
                    case "2": AddEquipment(); break;
                    case "3": PrintSummary(); break;
                    case "4": PrintBeginners(); break;
                    case "5": PrintInventory(); break;
                    case "6": RunDemo(); break;
                    default: output.WriteLine("Нет такого пункта."); break;
                }
            }
        }
        catch (EndOfStreamException)
        {
            output.WriteLine("Ввод завершён.");
        }
    }

    public void RunDemo()
    {
        output.WriteLine("===== Демо =====");
        Accept(new Scooter("Ninebot Max G30", 7, 95, 0.55m));
        Accept(new Scooter("Kugoo Kirin M4", 4, 80, 0.80m));
        Accept(new Bicycle("Stels Navigator 300", 9, 70));
        Accept(new ElectricBicycle("Eltreco XT 750", 6, 88, 1.20m));
        Accept(new Scooter("Самокат из подвала корпуса", 8, 30, 0.50m));
        Register(new Helmet("Шлем ВышКат M"));
        Register(new Helmet("Шлем ВышКат L"));
        Register(new DockStation("Док-станция у Покровки", 2.40m));
        Register(new ChargingCabinet("Зарядный шкаф ФКН", 6.00m));
        PrintSummary();
        PrintBeginners();
        PrintInventory();
    }

    private void PrintSummary()
    {
        output.WriteLine($"Всего единиц: {reports.TotalUnits} (транспорт: {reports.VehicleCount}, вещи: {reports.EquipmentCount})");
        output.WriteLine($"Суточное энергопотребление: {reports.TotalDailyEnergyKwh.ToString(CultureInfo.InvariantCulture)} кВт·ч");
    }

    private void PrintBeginners()
    {
        output.WriteLine("Для новичков:");
        foreach (var vehicle in reports.BeginnerFriendlyVehicles())
            output.WriteLine($"  {vehicle}, простота {vehicle.Simplicity}");
    }

    private void PrintInventory()
    {
        output.WriteLine("Инвентарный список:");
        foreach (var item in reports.InventoryList())
            output.WriteLine($"  {item.InventoryNumber}  {item.Name}");
    }

    private void AddVehicle()
    {
        var kind = ReadInt("1 - электросамокат, 2 - велосипед, 3 - электровелосипед: ", 1, 3);
        var name = ReadName();
        var simplicity = ReadInt("Простота для новичка (1-10): ", 1, 10);
        var condition = ReadInt("Техсостояние, % (0-100): ", 0, 100);

        Vehicle vehicle = kind switch
        {
            1 => new Scooter(name, simplicity, condition, ReadEnergy()),
            2 => new Bicycle(name, simplicity, condition),
            _ => new ElectricBicycle(name, simplicity, condition, ReadEnergy()),
        };
        Accept(vehicle);
    }

    private void AddEquipment()
    {
        var kind = ReadInt("1 - шлем, 2 - док-станция, 3 - зарядный шкаф: ", 1, 3);
        var name = ReadName();

        Equipment equipment = kind switch
        {
            1 => new Helmet(name),
            2 => new DockStation(name, ReadEnergy()),
            _ => new ChargingCabinet(name, ReadEnergy()),
        };
        Register(equipment);
    }

    private void Accept(Vehicle vehicle)
    {
        var result = fleetService.AcceptVehicle(vehicle);
        output.WriteLine(result.Accepted
            ? $"ПРИНЯТ   {vehicle} ({result.Reason})"
            : $"ОТКЛОНЁН {vehicle.KindName} «{vehicle.Name}» ({result.Reason})");
    }

    private void Register(Equipment equipment)
    {
        fleetService.RegisterEquipment(equipment);
        output.WriteLine($"НА БАЛАНСЕ {equipment}");
    }

    private string ReadName()
    {
        while (true)
        {
            var name = Read("Наименование: ");
            if (name != "")
                return name;
            output.WriteLine("Наименование не может быть пустым.");
        }
    }

    private int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            if (int.TryParse(Read(prompt), out var value) && value >= min && value <= max)
                return value;
            output.WriteLine($"Введите целое число от {min} до {max}.");
        }
    }

    private decimal ReadEnergy()
    {
        while (true)
        {
            var text = Read("Расход в сутки, кВт·ч: ").Replace(',', '.');
            if (decimal.TryParse(text, CultureInfo.InvariantCulture, out var value) && value >= 0)
                return value;
            output.WriteLine("Введите неотрицательное число, например 0.55.");
        }
    }

    private string Read(string prompt)
    {
        output.Write(prompt);
        var line = input.ReadLine() ?? throw new EndOfStreamException();
        return line.Trim();
    }
}
