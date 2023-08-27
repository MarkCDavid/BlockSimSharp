using BlockSimSharp.BitcoinBurn.SimulationConfiguration.Model;

namespace BlockSimSharp.BitcoinBurn.SimulationConfiguration;

public sealed class Configuration
{
    public string ScenarioName { get; init; } = null!;
    public string DataStoragePath { get; init; } = null!;
    public BlockSettings Block { get; } = new();
    public RandomNumberGeneratorSettings RandomNumberGenerator { get; } = new();
    public DifficultySettings Difficulty { get; } = new();
    public SimulationSettings Simulation { get; } = new();
    public NodeSettings Node { get; } = new();
    public StatisticsSettings Statistics { get; } = new();
}