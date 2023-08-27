using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;
using BlockSimSharp.BitcoinBurn.Utility;

namespace BlockSimSharp.BitcoinBurn.Simulation.Data;

public class Incentives
{
    private readonly DefaultDictionary<Node, double> _balance = new(0.0);
    private readonly DefaultDictionary<Node, int> _blocksMined = new(0);

    public double GetBalance(Node node) => _balance[node];
    public int GetBlocksMined(Node node) => _blocksMined[node];
    
    public void DistributeRewards(Configuration configuration, GlobalBlockChain globalBlockChain)
    {
        foreach (var block in globalBlockChain)
        {
            if (block.Miner is null)
            {
                continue;
            }

            _blocksMined[block.Miner] += 1;
            _balance[block.Miner]  += configuration.Block.Reward;
        }
    }
}