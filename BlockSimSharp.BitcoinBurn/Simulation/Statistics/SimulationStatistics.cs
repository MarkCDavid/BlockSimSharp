using BlockSimSharp.Core;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.BitcoinBurn.Simulation.Statistics;

public class SimulationStatistics : BaseStatistics
{
    public SimulationStatistics()
    {
        BlockStatistics = new List<BlockStatistics>();
        ProfitStatistics = new List<ProfitStatistics>();
    }

    public int TotalBlocks { get; set; }
    public int MainBlocks { get; set; }
    public int StaleBlocks { get; set; }
    public float StaleRate { get; set; }
    public int TotalTransactions { get; set; }

    public List<BlockStatistics> BlockStatistics { get; set; }
    public List<ProfitStatistics> ProfitStatistics { get; set; }

    public override void Calculate(SimulationContext context)
    {
        CalculateSimulationStatistics(context);
        // CalculateBlockStatistics(context);
        CalculateProfitStatistics(context);
    }

    private void CalculateSimulationStatistics(SimulationContext context)
    {
        var consensus = context.Get<Consensus>();

        MainBlocks = consensus.GlobalBlockChain.Count - 1;
        StaleBlocks = TotalBlocks - MainBlocks;
        StaleRate = MathF.Round((float)StaleBlocks / TotalBlocks, 2);
        TotalTransactions = consensus.GlobalBlockChain.Sum(block => block.Transactions.Count);
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
        var nodes = context.Get<Nodes>();
        var consants = context.Get<Constants>();
        ProfitStatistics = nodes.Select(node => new ProfitStatistics
        {
            NodeId = node.NodeId,
            HashPower = node.HashPower,
            PercentageOfAllHashPower = node.HashPower / consants.TotalHashPower, 
            Blocks = node.Blocks,
            PercentageOfAllBlocks = MathF.Round((float)node.Blocks / MainBlocks, 2),
            Balance = node.Balance,
            TotalPowerCostInDollars = node.TotalPowerCostInDollars,
            TotalDifficultyReductionCostInDollars = node.TotalDifficultyReductionCostInBitcoins
        }).ToList();
    }
}