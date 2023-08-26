
using BlockSimSharp.BitcoinBurn.Simulation;
using BlockSimSharp.BitcoinBurn.Simulation.Events;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.SimulationStatistics;

public class Statistics
{
    private readonly Nodes _nodes;
    private readonly Consensus _consensus;

    public Statistics(Configuration configuration, Nodes nodes, Difficulty difficulty, Consensus consensus)
    {
        _nodes = nodes;
        _consensus = consensus;
        Configuration = configuration;
        Difficulty = difficulty;
        BlockStatistics = new List<BlockStatistics>();
        ProfitStatistics = new List<ProfitStatistics>();
    }

    public void OnBlockMined(SimulationEvent simulationEvent)
    {
        TotalBlocks += 1;
    }

    public string Timestamp { get; set; } = null!;
    public Configuration Configuration { get; init; }
    public Difficulty Difficulty { get; init; }

    // public Dictionary<string, ISettings> Configuration { get; set; } = null!;
    public List<SimulationNode> Nodes { get; set; } = null!;

    public int TotalBlocks { get; set; }
    public int MainBlocks { get; set; }
    public int StaleBlocks { get; set; }
    public double StaleRate { get; set; }
    
    public double AverageBlockMiningTimeInSeconds { get; set; }
    public TotalStatistics Totals { get; set; } = null!;
    public List<ProfitStatistics> ProfitStatistics { get; set; }
    public List<BlockStatistics> BlockStatistics { get; set; }

    public void Calculate(Nodes nodes)
    {
        Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        Nodes = nodes.Select(node => new SimulationNode(node)).ToList();
        
        CalculateTotals();
        CalculateSimulationStatistics();
        // CalculateBlockStatistics(context);
        // CalculateProfitStatistics(context);
    }

    private void CalculateTotals()
    {
        Totals = new TotalStatistics()
        {
            TotalBitcoinsEarned = _nodes.Sum(node => node.Balance),
            TotalEnergyCost = _nodes.Sum(node => node.PowerUsed),
            TotalEnergyCostRatioWithHashPower = _nodes.Sum(node => node.PowerUsed) /  _nodes.Sum(node => node.HashPower)
        };
    }


    private void CalculateSimulationStatistics()
    {
        MainBlocks = _consensus.GlobalBlockChain.Count - 1;
        StaleBlocks = TotalBlocks - MainBlocks;
        StaleRate = Math.Round((double)StaleBlocks / TotalBlocks, 2);
        
        
        for (var blockIndex = _consensus.GlobalBlockChain.Count - 1; blockIndex > 0; blockIndex--)
        {
            var currentBlock = _consensus.GlobalBlockChain[blockIndex];
            var previousBlock = _consensus.GlobalBlockChain[blockIndex - 1];
            AverageBlockMiningTimeInSeconds += currentBlock.MinedAt - previousBlock.MinedAt;
        }
        
        AverageBlockMiningTimeInSeconds /= (_consensus.GlobalBlockChain.Count - 1);
    }

    private void CalculateBlockStatistics()
    {
        // var consensus = contextOld.Get<Consensus>();
        // BlockStatistics = consensus.GlobalBlockChain.Select(block => new BlockStatistics
        // {
        //     Depth = block.Depth,
        //     BlockId = block.BlockId,
        //     PreviousBlockId = block.PreviousBlock?.BlockId ?? -1,
        //     Timestamp = block.ExecutedAt,
        //     TransactionCount = block.Transactions.Count,
        //     SizeInMb = block.SizeInMb
        // }).ToList();
    }

    private void CalculateProfitStatistics()
    {
        // var settings = contextOld.Get<Settings>();
        // var simulationSettings = settings.Get<SimulationConfiguration.SimulationConfiguration>();
        // var consensus = contextOld.Get<Consensus>();
        // var nodes = contextOld.Get<Nodes>();
        // var constants = contextOld.Get<Constants>();
        // ProfitStatistics = nodes.Select(node => new ProfitStatistics
        // {
        //     NodeId = node.NodeId,
        //     HashPower = node.HashPower,
        //     // PercievedHashPower = consensus.PerceivedHashPower(context, node),
        //     PercentageOfAllHashPower = node.HashPower / constants.TotalHashPower, 
        //     Blocks = node.Blocks,
        //     PercentageOfAllBlocks = MathF.Round((double)node.Blocks / MainBlocks, 2),
        //     Balance = node.Balance,
        //     // The nodes should still be perceived as mining during the end of the simulation, although there might not
        //     // be any events. As such, we must calculate the power cost for the mining during the last seconds of the 
        //     // simulation.
        //     // TotalPowerCost = node.TotalPowerCost + consensus.PowerCost(context, node, simulationSettings.LengthInSeconds),
        //     // TotalDifficultyReductionCostInBitcoins = node.TotalDifficultyReductionCostInBitcoins
        // }).ToList();
    }
}