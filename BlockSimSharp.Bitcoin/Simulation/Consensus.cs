using BlockSimSharp.Bitcoin.Model;
using BlockSimSharp.Bitcoin.Simulation.Events;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.Bitcoin.Simulation;

public class Consensus: BaseConsensus<Transaction, Block, Node>
{
    public override float Protocol(SimulationContext context, Node miner)
    {
        var settings = context.Get<Settings>();
        var blockSettings = settings.Get<BlockSettings>();
        var nodes = context.Get<Nodes>();
        var randomness = context.Get<Randomness>();

        var totalHashPower = nodes.Sum(node => node.HashPower);
        var hashPower = miner.HashPower / totalHashPower;
        return randomness.Expovariate(hashPower * (1.0f / blockSettings.AverageIntervalInSeconds));
    }
    
    public override void ResolveForks(SimulationContext context)
    {
        var nodes = context.Get<Nodes>();
        
        var maximumBlockChainLength = nodes.MaxBy(node => node.BlockChainLength)!.BlockChainLength;
    
        var minersWithBlockChainsOfMaximumLength = nodes
                .Where(node => node.BlockChainLength == maximumBlockChainLength)
                .Select(node => node.NodeId)
                .ToList();
    
        if (minersWithBlockChainsOfMaximumLength.Count > 1)
        {
            var groupedByMiner = nodes
                .Where(node => node.BlockChainLength == maximumBlockChainLength)
                .GroupBy(node => node.LastBlock.Miner.NodeId)
                .Select(group => new { Miner = group.Key, Count = group.Count() })
                .ToList();
    
            var maxCount = groupedByMiner.Max(mc => mc.Count);
            
            minersWithBlockChainsOfMaximumLength = groupedByMiner
                .Where(mc => mc.Count == maxCount)
                .Select(mc => mc.Miner)
                .ToList();
        }
    
        var minerWithBlockChainOfMaxLength = minersWithBlockChainsOfMaximumLength.First();
    
        foreach (var node in nodes)
        {
            if (node.BlockChainLength == maximumBlockChainLength &&
                node.LastBlock.Miner.NodeId == minerWithBlockChainOfMaxLength)
            {
                GlobalBlockChain = node.BlockChain.ToList();
            }
        }
    
    }
}