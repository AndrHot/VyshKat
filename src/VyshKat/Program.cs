using System.Text;
using Microsoft.Extensions.DependencyInjection;
using VyshKat;
using VyshKat.Services;
using VyshKat.Ui;

Console.OutputEncoding = Console.InputEncoding = Encoding.UTF8;

using var provider = new ServiceCollection().AddVyshKat().BuildServiceProvider();
var ui = new ConsoleUi(
    provider.GetRequiredService<FleetService>(),
    provider.GetRequiredService<FleetReports>(),
    Console.In,
    Console.Out);

if (args.Contains("--demo"))
    ui.RunDemo();
else
    ui.Run();
