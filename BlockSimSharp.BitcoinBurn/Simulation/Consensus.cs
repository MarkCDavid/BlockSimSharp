using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public sealed class Consensus
{
    private readonly Configuration _configuration;
    private readonly Difficulty _difficulty;
    private readonly Randomness _randomness;
    private readonly List<Node> _nodes;

    public List<Block> GlobalBlockChain { get; private set; }

    public Consensus(
        Configuration configuration, 
        Difficulty difficulty, 
        Randomness randomness, 
        List<Node> nodes)
    {
        _configuration = configuration;
        _difficulty = difficulty;
        _randomness = randomness;
        _nodes = nodes;

        GlobalBlockChain = new List<Block>();
    }
    
    public double Protocol(Node miner)
    {
        var relativeHashPower = _difficulty.GetRelativeHashPower(miner);
        return _randomness.Expovariate(relativeHashPower * (1.0f / _configuration.Block.AverageIntervalInSeconds));
    }
    
    public void ResolveForks()
    {
        var maximumBlockChainLength = _nodes.MaxBy(node => node.BlockChainLength)?.BlockChainLength ?? 0;
    
        var minersWithBlockChainsOfMaximumLength = _nodes
                .Where(node => node.BlockChainLength == maximumBlockChainLength)
                .Select(node => node.NodeId)
                .ToList();
    
        if (minersWithBlockChainsOfMaximumLength.Count > 1)
        {
            var groupedByMiner = _nodes
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