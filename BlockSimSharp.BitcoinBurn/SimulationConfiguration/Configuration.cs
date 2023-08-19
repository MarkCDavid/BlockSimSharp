using BlockSimSharp.BitcoinBurn.SimulationConfiguration.Model;

namespace BlockSimSharp.BitcoinBurn.SimulationConfiguration;

public sealed class Configuration
{
    public string ScenarioName { get; init; } = null!;
    public BlockConfiguration Block { get; } = new();
    public TransactionConfiguration Transaction { get; } = new();
    public RandomNumberGeneratorConfiguration RandomNumberGenerator { get; } = new();
    public DifficultyConfiguration Difficulty { get; } = new();
    public Model.SimulationConfiguration Simulation { get; } = new();
    public NodeConfiguration Node { get; } = new();
}