using VyshKat.Domain;

namespace VyshKat.Services;

public sealed record InspectionResult(bool Accepted, string Reason);

public interface IServiceCenter
{
    InspectionResult Inspect(Vehicle vehicle);
}

public sealed class ServiceCenter(int minConditionPercent = 60) : IServiceCenter
{
    public InspectionResult Inspect(Vehicle vehicle) =>
        vehicle.ConditionPercent >= minConditionPercent
            ? new(true, $"техсостояние {vehicle.ConditionPercent}% ≥ {minConditionPercent}%")
            : new(false, $"техсостояние {vehicle.ConditionPercent}% < {minConditionPercent}%");
}

public interface IBeginnerPolicy
{
    bool IsSuitable(Vehicle vehicle);
}

public sealed class SimplicityBeginnerPolicy(int minSimplicity = 6) : IBeginnerPolicy
{
    public bool IsSuitable(Vehicle vehicle) => vehicle.Simplicity >= minSimplicity;
}
