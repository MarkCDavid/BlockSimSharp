using BlockSimSharp.Base;

namespace BlockSimSharp.Ethereum;

public class EthereumIncentives: BaseIncentives<EthereumNode, EthereumBlock, EthereumTransaction, EthereumScheduler>
{
    public override void DistributeRewards(SimulationContext<EthereumNode, EthereumBlock, EthereumTransaction, EthereumScheduler> context)
    {
        foreach (var block in context.Consensus.GlobalBlockChain)
        {
            foreach (var node in context.Nodes)
            {
                if (block.MinerId != node.Id)
                {
                    continue;
                }

                node.Blocks += 1;
                node.Balance += Configuration.Instance.BlockReward;
                node.Balance += TransactionFee(block);
                node.Balance += UncleInclusionRewards(block);
            }
            
            UncleRewards(context, block);
        }
    }


    private void UncleRewards(SimulationContext<EthereumNode, EthereumBlock, EthereumTransaction, EthereumScheduler> context, EthereumBlock block)
    {
        foreach (var uncle in block.Uncles)
        {
            foreach (var miner in context.Nodes)
            {
                if (uncle.MinerId != miner.Id)
                {
                    continue;
                }

                context.Statistics.TotalUncles += 1;

                var uncleHeight = uncle.Depth;
                var blockHeight = block.Depth;

                miner.Uncles += 1;
                miner.Balance += (uncleHeight - blockHeight + 8) * Configuration.Instance.BlockReward / 8;
            }
        }
    }

    private float UncleInclusionRewards(EthereumBlock block)
    {
        return block.Uncles.Sum(uncle => Configuration.Instance.UncleInclusionReward);
    }
    
    
}