using BlockSimSharp.BitcoinBurn.Configuration;
using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public class Consensus: BaseConsensus<Transaction, Block, Node>
{
    public override float Protocol(SimulationContext context, Node miner)
    {
        var difficulty = context.Get<Difficulty>();
        var randomness = context.Get<Randomness>();
        var settings = context.Get<Settings>();
        var blockSettings = settings.Get<BlockSettings>();

        var relativeHashPower = difficulty.GetRelativeHashPower(miner);
        return randomness.Expovariate(relativeHashPower * (1.0f / blockSettings.AverageIntervalInSeconds));
    }
    
    public override void ResolveForks(SimulationContext context)
    {
        var nodes = context.Get<Nodes>();
        
        var maximumBlockChainLength = nodes.MaxBy(node => node.BlockChainLength)?.BlockChainLength ?? 0;
    
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
    
        var minerWithBlockChainOfMaxLength = minersWithBlockChainsOfMaximumLength.FirstOrDefault(-1);
    
        foreach (var node in nodes)
        {
            if (node.BlockChainLength == maximumBlockChainLength &&
                node.LastBlock.Miner?.NodeId == minerWithBlockChainOfMaxLength)
            {
                GlobalBlockChain = node.BlockChain.ToList();
            }
        }
    
    }

    public float PowerCost(SimulationContext context, Node miner, float eventTime)
    {
        if (miner.CurrentlyMinedBlock is null)
        {
            return 0;
        }

        var nodePowerCostInDollarsPerSecond 
            = context.Get<Constants>().AveragePowerCostHashPower * miner.HashPower;
        
        var totalTimeSpentMiningABlockInSeconds 
            = eventTime - miner.CurrentlyMinedBlock.ScheduledTimestamp;
        
        return totalTimeSpentMiningABlockInSeconds * nodePowerCostInDollarsPerSecond;
    }
}