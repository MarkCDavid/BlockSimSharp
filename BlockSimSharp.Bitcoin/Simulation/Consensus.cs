using BlockSimSharp.Bitcoin.Model;
using BlockSimSharp.Bitcoin.Simulation.Events;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.Bitcoin.Simulation;

public class Consensus: BaseConsensus<Transaction, Block, Node, Network, Consensus, Event, Scheduler, Context>
{
    public override float Protocol(Context context, Node miner)
    {
        var blockSettings = context.Settings.Get<BlockSettings>();
        var totalHashPower = context.Nodes.Sum(node => node.HashPower);
        var hashPower = miner.HashPower / totalHashPower;
        return Utility.Expovariate(hashPower * (1.0f / blockSettings.AverageIntervalInSeconds));
    }
    
    public override void ForkResolution(Context context)
    {
        var maximumBlockChainLength = context.Nodes.MaxBy(node => node.BlockChainLength).BlockChainLength;
    
        var minersWithBlockChainsOfMaximumLength = context.Nodes
                .Where(node => node.BlockChainLength == maximumBlockChainLength)
                .Select(node => node.NodeId)
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