using BlockSimSharp.Base;

namespace BlockSimSharp.Bitcoin;

public class BitcoinConsensus: BaseConsensus<BitcoinBlock, BitcoinNode, BitcoinTransaction>
{
    private readonly IList<BitcoinNode> _nodes;

    public BitcoinConsensus(IList<BitcoinNode> nodes)
    {
        _nodes = nodes;
    }

    public override float Protocol(BitcoinNode miner)
    {
        var totalHashPower = _nodes.Sum(node => node.HashPower);
        var hashPower = miner.HashPower / totalHashPower;
        return Utility.Expovariate(hashPower * (1.0f / Configuration.Instance.AverageBlockIntervalInSeconds));
    }

    public override void ForkResolution()
    {
        var maximumBlockChainLength = _nodes.MaxBy(node => node.BlockChainLength).BlockChainLength;

        var minersWithBlockChainsOfMaximumLength = _nodes
                .Where(node => node.BlockChainLength == maximumBlockChainLength)
                .Select(node => node.Id)
                .ToList();

        if (minersWithBlockChainsOfMaximumLength.Count > 1)
        {
            var groupedByMiner = _nodes
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

        foreach (var node in _nodes)
        {
            if (node.BlockChainLength == maximumBlockChainLength &&
                node.LastBlock.MinerId == minerWithBlockChainOfMaxLength)
            {
                GlobalBlockChain = node.BlockChain.ToList();
            }
        }

    }
}