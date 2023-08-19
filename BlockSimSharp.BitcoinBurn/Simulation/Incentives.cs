using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public class Incentives
{
    private readonly Configuration _configuration;
    private readonly Consensus _consensus;
    
    public Incentives(Configuration configuration, Consensus consensus)
    {
        _configuration = configuration;
        _consensus = consensus;
    }
    
    public void DistributeRewards()
    {
        foreach (var block in _consensus.GlobalBlockChain)
        {
            var miner = block.Miner;
            if (miner is null)
                continue;
            
            miner.Blocks += 1;
            miner.Balance += _configuration.Block.Reward;
        }
    }

}