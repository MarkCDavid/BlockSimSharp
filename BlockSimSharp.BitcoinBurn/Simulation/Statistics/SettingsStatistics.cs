using BlockSimSharp.BitcoinBurn.Configuration;
using BlockSimSharp.Core.Configuration.Model;

namespace BlockSimSharp.BitcoinBurn.Simulation.Statistics;

public class SettingsStatistics
{
    public BlockSettings BlockSettings { get; init; } = null!;
    public TransactionSettings TransactionSettings { get; init; } = null!;
    public SimulationSettings SimulationSettings { get; init; } = null!;
    public RandomNumberGenerationSettings RandomNumberGenerationSettings { get; init; } = null!;
    public BurnSettings BurnSettings { get; init; } = null!;
}