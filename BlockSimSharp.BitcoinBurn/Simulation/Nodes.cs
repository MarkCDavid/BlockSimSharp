using BlockSimSharp.BitcoinBurn.SimulationConfiguration;
using BlockSimSharp.Model;

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

    public double TotalHashPower => this.Sum(node => node.HashPower);

    private void BuildNodes()
    {
        for (var nodeId = 0; nodeId < _configuration.Node.StartingNodeCount; nodeId++)
        {
            // Aoki et al. (2019) SimBlock
            var hashPower = _randomness.NextGaussian() * _configuration.Node.StandardDeviationOfHashRate + _configuration.Node.AverageHashRate;
            hashPower = Math.Max(hashPower, 1);
            Add(new Node(nodeId, hashPower));
        }
    }
}