using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public class Incentives: BaseIncentives
{
    public override void DistributeRewards(SimulationContext context)
    {
        var consensus = context.Get<Consensus>();
        
        var settings = context.Get<Settings>();
        var blockSettings = settings.Get<BlockSettings>();
        
        foreach (var block in consensus.GlobalBlockChain)
        {
            if (block.Miner is null)
                continue;
            
            block.Miner.Blocks += 1;
            block.Miner.Balance += blockSettings.Reward;
            block.Miner.Balance += TransactionFee(block);
        }
    }

    private static float TransactionFee(Block block)
    {
        return block.Transactions.Sum(transaction => transaction.Fee);
    }
}