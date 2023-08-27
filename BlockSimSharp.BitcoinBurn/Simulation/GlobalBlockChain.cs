using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public sealed class GlobalBlockChain
{
    public List<Block> BlockChain { get; private set; } = new();

    public void ResolveForks(Nodes nodes)
    {
        var maximumBlockChainLength = nodes.MaximumBlockChainLength;
    
        var minersWithBlockChainsOfMaximumLength = nodes
                .Where(node => node.BlockChainLength == maximumBlockChainLength)
                .Select(node => node.NodeId)
                .ToList();
    
        if (minersWithBlockChainsOfMaximumLength.Count > 1)
        {
            var groupedByMiner = nodes
                .Where(node => node.BlockChainLength == maximumBlockChainLength)
                .GroupBy(node => node.LastBlock.Miner!.NodeId)
                .Select(group => new { Miner = group.Key, Count = group.Count() })
                .ToList();
    
            var maxCount = groupedByMiner.Max(mc => mc.Count);
            
            minersWithBlockChainsOfMaximumLength = groupedByMiner
                .Where(mc => mc.Count == maxCount)
                .Select(mc => mc.Miner)
                .ToList();
        }
    
        var minerWithBlockChainOfMaxLength = minersWithBlockChainsOfMaximumLength.FirstOrDefault(-1);
    
        foreach (var node in nodes)
        {
            if (node.BlockChainLength == maximumBlockChainLength &&
                node.LastBlock.Miner?.NodeId == minerWithBlockChainOfMaxLength)
            {
                BlockChain = node.BlockChain.ToList();
            }
        }
    }
    
    public void DistributeRewards(Configuration configuration)
    {
        foreach (var block in BlockChain.Where(block => block.Miner is not null))
        {
            block.Miner!.Blocks += 1;
            block.Miner!.Balance += configuration.Block.Reward;
        }
    }
    
}