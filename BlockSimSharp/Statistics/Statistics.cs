using BlockSimSharp.Base;

namespace BlockSimSharp.Statistics;

public class Statistics<TNode, TBlock, TTransaction>
    where TNode: BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction
{
    public int TotalBlocks { get; set; } = 0;
    public int MainBlocks { get; set; } = 0;
    public int StaleBlocks { get; set; } = 0;
    public float StaleRate { get; set; } = 0;

    public int TotalTransactions { get; set; } = 0;
    
    public List<StatisticsBlock> BlockChain { get; set; }
    public List<ProfitResult> ProfitResults { get; set; }

    public Statistics()
    {
        BlockChain = new List<StatisticsBlock>();
        ProfitResults = new List<ProfitResult>();
    }

    public void Calculate(SimulationContext<TNode, TBlock, TTransaction> context)
    {
        CalculateGlobalChain(context);
        CalculateBlockResults(context);
        CalculateProfitResults(context);
    }


    private void CalculateGlobalChain(SimulationContext<TNode, TBlock, TTransaction> context)
    {
        foreach (var block in context.Consensus.GlobalBlockChain)
        {
            BlockChain.Add(new StatisticsBlock()
            {
                Depth = block.Depth,
                BlockId = block.BlockId,
                PreviousBlockId = block.PreviousBlockId,
                Timestamp = block.Timestamp,
                TransactionCount = block.Transactions.Count,
                SizeInMb = block.SizeInMb,
            });
        }
    }

    private void CalculateBlockResults(SimulationContext<TNode, TBlock, TTransaction> context)
    {
        MainBlocks = context.Consensus.GlobalBlockChain.Count - 1;
        StaleBlocks = TotalBlocks - MainBlocks;
        StaleRate = MathF.Round((float)StaleBlocks / TotalBlocks, 2);
        TotalTransactions = context.Consensus.GlobalBlockChain.Sum(block => block.Transactions.Count);
    }

    private void CalculateProfitResults(SimulationContext<TNode, TBlock, TTransaction> context)
    {
        foreach (var node in context.Nodes)
        {
            ProfitResults.Add(new ProfitResult()
            {
                NodeId = node.Id,
                Blocks = node.Blocks,
                PercentageOfAllBlocks = MathF.Round((float)node.Blocks / MainBlocks, 2),
                Balance = node.Balance
            });   
        }
    }


}

public class StatisticsBlock
{
    public int Depth { get; set; }
    public int BlockId { get; set; }
    public int? PreviousBlockId { get; set; }
    public float Timestamp { get; set; }
    public int TransactionCount { get; set; }
    public float SizeInMb { get; set; }
}

public class ProfitResult
{
    public int NodeId { get; set; }
    public int Blocks { get; set; }
    public float PercentageOfAllBlocks { get; set; }
    public float Balance { get; set; }
}
