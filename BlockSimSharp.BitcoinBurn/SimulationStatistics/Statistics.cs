using BlockSimSharp.BitcoinBurn.Simulation.Data;
using BlockSimSharp.BitcoinBurn.Simulation.Events;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;
using BlockSimSharp.BitcoinBurn.SimulationStatistics.Data;

namespace BlockSimSharp.BitcoinBurn.SimulationStatistics;

public class Statistics
{
    public string Timestamp { get; private set; } = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    public Configuration Configuration { get; private set; }
    public Average Average { get; private set; } = new();
    public Power Power { get; private set; } = new();
    public Blocks Blocks { get; private set; } = new();
    public Nodes Nodes { get; private set; } = new();
    public List<double> DifficultyChangeHistory { get; private set; } = new();
    
    public Statistics(
        Configuration configuration,
        Simulation.Nodes nodes,
        GlobalBlockChain globalBlockChain,
        Incentives incentives,
        PowerUsage powerUsage, 
        DifficultyHistory difficultyHistory)
    {
        Configuration = configuration;
        _nodes = nodes;
        _globalBlockChain = globalBlockChain;
        _incentives = incentives;
        _powerUsage = powerUsage;
        _difficultyHistory = difficultyHistory;
    }

    public void OnBlockMined(SimulationEvent simulationEvent)
    {
        Blocks.Total += 1;
    }

    
    public void Calculate()
    {
        Blocks.Update(_globalBlockChain, Configuration);
        Average.Update(_globalBlockChain);
        Power.Update(_nodes, _powerUsage, Blocks, Configuration);
        Nodes.Update(_nodes, _incentives, _powerUsage, _difficultyHistory);
        DifficultyChangeHistory.AddRange(_difficultyHistory.DifficultyChangeHistory);
    }

    private readonly Simulation.Nodes _nodes;
    private readonly GlobalBlockChain _globalBlockChain;
    private readonly Incentives _incentives;
    private readonly PowerUsage _powerUsage;
    private readonly DifficultyHistory _difficultyHistory;
}