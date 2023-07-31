using BlockSimSharp.Bitcoin.Model;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.Bitcoin.Simulation;

public class Incentives: BaseIncentives
{
    public override void DistributeRewards(SimulationContext context)
    {
        var nodes = context.Get<Nodes>();
        var consensus = context.Get<Consensus>();
        
        var settings = context.Get<Settings>();
        var blockSettings = settings.Get<BlockSettings>();
        
        foreach (var block in consensus.GlobalBlockChain)
        {
            foreach (var node in nodes.Where(node => block.MinerId == node.NodeId))
            {
                node.Blocks += 1;
                node.Balance += blockSettings.Reward;
                node.Balance += TransactionFee(block);
            }
        }
    }

    private static float TransactionFee(Block block)
    {
        return block.Transactions.Sum(transaction => transaction.Fee);
    }
}