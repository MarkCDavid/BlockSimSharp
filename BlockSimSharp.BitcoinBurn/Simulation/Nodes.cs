using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public sealed class Nodes: List<Node>
{
    private readonly Configuration _configuration;
    private readonly Randomness _randomness;

    public Nodes(Configuration configuration, Randomness randomness)
    {
        _configuration = configuration;
        _randomness = randomness;
        BuildNodes();
    }

    public IEnumerable<Node> Without(int nodeId) => this.Where(node => node.NodeId != nodeId);
    public double TotalHashPower => this.Sum(node => node.HashPower);
    public int MaximumBlockChainLength => this.Select(node => node.BlockChainLength).Max();

    private void BuildNodes()
    {
        for (var nodeId = 0; nodeId < _configuration.Node.StartingNodeCount; nodeId++)
        {
            // Aoki et al. (2019) SimBlock
            var hashPower = _randomness.NextGaussian() * _configuration.Node.StandardDeviationOfHashRate + _configuration.Node.AverageHashRate;
            hashPower = Math.Max(hashPower, 1);
            
            var node = new Node(nodeId, hashPower);
            Add(node);
        }
    }
}