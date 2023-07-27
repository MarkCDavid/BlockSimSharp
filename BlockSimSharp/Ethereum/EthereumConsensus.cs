using BlockSimSharp.Base;

namespace BlockSimSharp.Ethereum;

public class EthereumConsensus: BaseConsensus<EthereumBlock, EthereumNode, EthereumTransaction, EthereumScheduler>
{
    public override float Protocol(SimulationContext<EthereumNode, EthereumBlock, EthereumTransaction, EthereumScheduler> context, EthereumNode miner)
    {
        var totalHashPower = context.Nodes.Sum(node => node.HashPower);
        var hashPower = miner.HashPower / totalHashPower;
        return Utility.Expovariate(hashPower * (1.0f / Configuration.Instance.AverageBlockIntervalInSeconds));
    }

    public override void ForkResolution(SimulationContext<EthereumNode, EthereumBlock, EthereumTransaction, EthereumScheduler> context)
    {
        var maximumBlockChainLength = context.Nodes.MaxBy(node => node.BlockChainLength).BlockChainLength;

        var minersWithBlockChainsOfMaximumLength = context.Nodes
            .Where(node => node.BlockChainLength == maximumBlockChainLength)
            .Select(node => node.Id)
            .ToList();

        if (minersWithBlockChainsOfMaximumLength.Count > 1)
        {
            var groupedByMiner = context.Nodes
                .Where(node => node.BlockChainLength == maximumBlockChainLength)
                .GroupBy(node => node.LastBlock.MinerId)
                .Select(group => new { Miner = group.Key, Count = group.Count() })
                .ToList();

            var maxCount = groupedByMiner.Max(mc => mc.Count);
            
            minersWithBlockChainsOfMaximumLength = groupedByMiner
                .Where(mc => mc.Count == maxCount)
                .Select(mc => mc.Miner)
                .ToList();
        }

        var minerWithBlockChainOfMaxLength = minersWithBlockChainsOfMaximumLength.First();

        foreach (var node in context.Nodes)
        {
            if (node.BlockChainLength == maximumBlockChainLength &&
                node.LastBlock.MinerId == minerWithBlockChainOfMaxLength)
            {
                GlobalBlockChain = node.BlockChain.ToList();
            }
        }
    }
}