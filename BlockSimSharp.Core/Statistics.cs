// using BlockSimSharp.Core;
// using BlockSimSharp.Core.Model;
// using BlockSimSharp.Core.Simulation;
//
// namespace BlockSimSharp.Core;
//
// public class Statistics<TNode, TBlock, TTransaction, TScheduler>
//     where TNode: BaseNode<TBlock, TTransaction>
//     where TBlock: BaseBlock<TTransaction>, new()
//     where TTransaction: BaseTransaction<TTransaction>
//     where TScheduler: BaseScheduler<TBlock, TNode, TTransaction, TScheduler>
// {
//     public int TotalBlocks { get; set; } = 0;
//     public int TotalUncles { get; set; } = 0;
//     public int UncleBlocks { get; set; } = 0;
//     public int MainBlocks { get; set; } = 0;
//     public int StaleBlocks { get; set; } = 0;
//     public float StaleRate { get; set; } = 0;
//
//     public float UncleRate { get; set; } = 0;
//
//     public int TotalTransactions { get; set; } = 0;
//     
//     public List<StatisticsBlock> BlockChain { get; set; }
//     public List<ProfitResult> ProfitResults { get; set; }
//
//     public Statistics()
//     {
//         BlockChain = new List<StatisticsBlock>();
//         ProfitResults = new List<ProfitResult>();
//     }
//
//     public void Calculate(BaseContext<TNode, TBlock, TTransaction, TScheduler> baseContext)
//     {
//         // CalculateGlobalChain(baseContext);
//         // CalculateBlockResults(baseContext);
//         // CalculateProfitResults(baseContext);
//     }
//
//
//     // private void CalculateGlobalChain(BaseContext<TNode, TBlock, TTransaction, TScheduler> baseContext)
//     // {
//     //     foreach (var block in baseContext.Consensus.GlobalBlockChain)
//     //     {
//     //         BlockChain.Add(new StatisticsBlock()
//     //         {
//     //             Depth = block.Depth,
//     //             BlockId = block.BlockId,
//     //             PreviousBlockId = block.PreviousBlockId,
//     //             Timestamp = block.Timestamp,
//     //             TransactionCount = block.Transactions.Count,
//     //             // SizeInMb = block.SizeInMb,
//     //         });
//     //     }
//     // }
//     //
//     // private void CalculateBlockResults(BaseContext<TNode, TBlock, TTransaction, TScheduler> baseContext)
//     // {
//     //     MainBlocks = baseContext.Consensus.GlobalBlockChain.Count - 1;
//     //     StaleBlocks = TotalBlocks - MainBlocks;
//     //     StaleRate = MathF.Round((float)StaleBlocks / TotalBlocks, 2);
//     //     TotalTransactions = baseContext.Consensus.GlobalBlockChain.Sum(block => block.Transactions.Count);
//     // }
//     //
//     // private void CalculateProfitResults(BaseContext<TNode, TBlock, TTransaction, TScheduler> baseContext)
//     // {
//     //     foreach (var node in baseContext.Nodes)
//     //     {
//     //         
//     //         ProfitResults.Add(new ProfitResult()
//     //         {
//     //             NodeId = node.NodeId,
//     //             Blocks = node.Blocks,
//     //             PercentageOfAllBlocks = MathF.Round((float)node.Blocks / MainBlocks, 2),
//     //             Balance = node.Balance
//     //         });   
//     //     }
//     // }
//
//
// }
//
// public class StatisticsBlock
// {
//     public int Depth { get; set; }
//     public int BlockId { get; set; }
//     public int? PreviousBlockId { get; set; }
//     public float Timestamp { get; set; }
//     public int TransactionCount { get; set; }
//     public float SizeInMb { get; set; }
//     public float UsedGas { get; set; }
//     public int UncleCount { get; set; }
// }
//
// public class ProfitResult
// {
//     public int NodeId { get; set; }
//     public int Blocks { get; set; }
//     public int Uncles { get; set; }
//     
//     public float PercentageOfAllBlocks { get; set; }
//     public float PercentageOfAllBlocksIncludingUncles { get; set; }
//     public float Balance { get; set; }
// }
