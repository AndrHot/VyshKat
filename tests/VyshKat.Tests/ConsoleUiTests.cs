using VyshKat.Domain;
using VyshKat.Services;
using VyshKat.Ui;

namespace VyshKat.Tests;

public class ConsoleUiTests
{
    private readonly Fleet _fleet = new();

    private string Run(string input, bool demo = false)
    {
        var output = new StringWriter();
        var ui = new ConsoleUi(
            new FleetService(_fleet, new ServiceCenter()),
            new FleetReports(_fleet, new SimplicityBeginnerPolicy()),
            new StringReader(input),
            output);
        if (demo) ui.RunDemo(); else ui.Run();
        return output.ToString();
    }

    [Fact]
    public void Demo_RunsAllScenarios()
    {
        var output = Run("", demo: true);

        Assert.Contains("ОТКЛОНЁН", output);
        Assert.Contains("Всего единиц: 8 (транспорт: 4, вещи: 4)", output);
        Assert.Contains("Суточное энергопотребление: 10.95 кВт·ч", output);
        Assert.Contains("VK-0008", output);
    }

    [Fact]
    public void Menu_AddsVehicleAndEquipment_WithInputRetries()
    {
        var output = Run("1\n1\nG30\n42\n7\n90\n0,5\n2\n1\nШлем M\n3\n4\n5\n9\n0\n");

        Assert.Equal(2, _fleet.Assets.Count);
        Assert.Equal(0.5m, ((Scooter)_fleet.Assets[0]).DailyEnergyKwh);
        Assert.Contains("Введите целое число от 1 до 10.", output);
        Assert.Contains("VK-0002  Шлем M", output);
        Assert.Contains("Нет такого пункта.", output);
        Assert.Contains("До встречи!", output);
    }

    [Fact]
    public void Menu_EndOfInput_ExitsGracefully()
    {
        Assert.Contains("Ввод завершён.", Run("1\n1\n"));
        Assert.Empty(_fleet.Assets);
    }
}
