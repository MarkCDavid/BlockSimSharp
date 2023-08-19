using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public sealed class Consensus
{
    public List<Block> GlobalBlockChain { get; private set; }
    
    private readonly Configuration _configuration;
    private readonly Randomness _randomness;
    private readonly Nodes _nodes;
    private readonly Difficulty _difficulty;

    public Consensus(
        Configuration configuration,
        Randomness randomness,
        Nodes nodes,
        Difficulty difficulty)
    {
        _configuration = configuration;
        _randomness = randomness;
        _nodes = nodes;
        _difficulty = difficulty;

        GlobalBlockChain = new List<Block>();
    }
    
    public double Protocol(Node miner)
    {
        var relativeHashPower = _difficulty.GetRelativeHashPower(miner);
        var blockRate = 1.0 / _configuration.Block.AverageIntervalInSeconds;
        return _randomness.Expovariate(relativeHashPower * blockRate);
    }
    
    public void ResolveForks()
    {
        var maximumBlockChainLength = _nodes.MaximumBlockChainLength;
    
        var minersWithBlockChainsOfMaximumLength = _nodes
                .Where(node => node.BlockChainLength == maximumBlockChainLength)
                .Select(node => node.NodeId)
                .ToList();
    
        if (minersWithBlockChainsOfMaximumLength.Count > 1)
        {
            var groupedByMiner = _nodes
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
    
        foreach (var node in _nodes)
        {
            if (node.BlockChainLength == maximumBlockChainLength &&
                node.LastBlock.Miner?.NodeId == minerWithBlockChainOfMaxLength)
            {
                GlobalBlockChain = node.BlockChain.ToList();
            }
        }
    
    }
}