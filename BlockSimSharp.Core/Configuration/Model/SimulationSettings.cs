using BlockSimSharp.Core.Configuration.Contracts;

namespace BlockSimSharp.Core.Configuration.Model;

public sealed class SimulationSettings: ISettings
{
    public int LengthInSeconds { get; init; }
}