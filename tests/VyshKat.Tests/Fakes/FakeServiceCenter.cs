using VyshKat.Domain;
using VyshKat.Services;

namespace VyshKat.Tests.Fakes;

public sealed class FakeServiceCenter(bool accept) : IServiceCenter
{
    public List<Vehicle> Inspected { get; } = new();

    public InspectionResult Inspect(Vehicle vehicle)
    {
        Inspected.Add(vehicle);
        return new InspectionResult(accept, accept ? "fake: ok" : "fake: отказ");
    }
}
