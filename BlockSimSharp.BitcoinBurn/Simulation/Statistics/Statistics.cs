using BlockSimSharp.BitcoinBurn.Configuration;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Contracts;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.BitcoinBurn.Simulation.Statistics;

public class Statistics : BaseStatistics
{
    public Statistics()
    {
        BlockStatistics = new List<BlockStatistics>();
        ProfitStatistics = new List<ProfitStatistics>();
    }

    public string Scenario { get; set; } = null!;
    public string Timestamp { get; set; } = null!;
    public Dictionary<string, ISettings> Configuration { get; set; } = null!;
    public List<Node> Nodes { get; set; } = null!;
    public Dictionary<int, float> HistoryOfDifficulty { get; set; } = null!;

    public int TotalBlocks { get; set; }
    public int MainBlocks { get; set; }
    public int StaleBlocks { get; set; }
    public float StaleRate { get; set; }
    public int TotalTransactions { get; set; }
    
    public float AverageBlockMiningTimeInSeconds { get; set; }
    public TotalStatistics Totals { get; set; } = null!;
    public List<ProfitStatistics> ProfitStatistics { get; set; }
    public List<BlockStatistics> BlockStatistics { get; set; }

    public override void Calculate(SimulationContext context)
    {
        Scenario = context.Get<Settings>().Get<ScenarioSettings>().ScenarioName;
        Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        Configuration = context.Get<Settings>().GetAll();
        Nodes = context.Get<Nodes>().Select(node => new Node(node)).ToList();
        HistoryOfDifficulty = context.Get<Difficulty>().DifficultyHistory;

        // CalculateTotals(context);
        CalculateSimulationStatistics(context);
        // CalculateBlockStatistics(context);
        // CalculateProfitStatistics(context);
    }

    private void CalculateTotals(SimulationContext context)
    {
        var nodes = context.Get<Nodes>();
        Totals = new TotalStatistics()
        {
            TotalBitcoinsEarned = nodes.Sum(node => node.Balance),
            // TotalEnergyCostInDollars = nodes.Sum(node => node.TotalPowerCost)
        };
    }


    private void CalculateSimulationStatistics(SimulationContext context)
    {
        var consensus = context.Get<Consensus>();

        MainBlocks = consensus.GlobalBlockChain.Count - 1;
        StaleBlocks = TotalBlocks - MainBlocks;
        StaleRate = MathF.Round((float)StaleBlocks / TotalBlocks, 2);
        TotalTransactions = consensus.GlobalBlockChain.Sum(block => block.Transactions.Count);

        
        for (var blockIndex = consensus.GlobalBlockChain.Count - 1; blockIndex > 0; blockIndex--)
        {
            var currentBlock = consensus.GlobalBlockChain[blockIndex];
            var previousBlock = consensus.GlobalBlockChain[blockIndex - 1];
            AverageBlockMiningTimeInSeconds += currentBlock.Timestamp - previousBlock.Timestamp;
        }

        AverageBlockMiningTimeInSeconds = AverageBlockMiningTimeInSeconds / (consensus.GlobalBlockChain.Count - 1);
    }

    private void CalculateBlockStatistics(SimulationContext context)
    {
        var consensus = context.Get<Consensus>();
        BlockStatistics = consensus.GlobalBlockChain.Select(block => new BlockStatistics
        {
            Depth = block.Depth,
            BlockId = block.BlockId,
            PreviousBlockId = block.PreviousBlock?.BlockId ?? -1,
            Timestamp = block.Timestamp,
            TransactionCount = block.Transactions.Count,
            SizeInMb = block.SizeInMb
        }).ToList();
    }

    private void CalculateProfitStatistics(SimulationContext context)
    {
        var settings = context.Get<Settings>();
        var simulationSettings = settings.Get<SimulationSettings>();
        var consensus = context.Get<Consensus>();
        var nodes = context.Get<Nodes>();
        var constants = context.Get<Constants>();
        ProfitStatistics = nodes.Select(node => new ProfitStatistics
        {
            NodeId = node.NodeId,
            HashPower = node.HashPower,
            // PercievedHashPower = consensus.PerceivedHashPower(context, node),
            PercentageOfAllHashPower = node.HashPower / constants.TotalHashPower, 
            Blocks = node.Blocks,
            PercentageOfAllBlocks = MathF.Round((float)node.Blocks / MainBlocks, 2),
            Balance = node.Balance,
            // The nodes should still be perceived as mining during the end of the simulation, although there might not
            // be any events. As such, we must calculate the power cost for the mining during the last seconds of the 
            // simulation.
            // TotalPowerCost = node.TotalPowerCost + consensus.PowerCost(context, node, simulationSettings.LengthInSeconds),
            // TotalDifficultyReductionCostInBitcoins = node.TotalDifficultyReductionCostInBitcoins
        }).ToList();
    }
}