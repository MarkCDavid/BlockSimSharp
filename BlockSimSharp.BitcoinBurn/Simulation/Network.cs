using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public class Network
{
    private readonly Configuration _configuration;
    private readonly Randomness _randomness;

    public Network(Configuration configuration, Randomness randomness)
    {
        _configuration = configuration;
        _randomness = randomness;
    }
    
    public virtual double BlockPropogationDelay()
    {
        if (_configuration.Block.AveragePropogationDelay == 0)
        {
            return 0;
        }
        
        return _randomness.Expovariate(1.0f / _configuration.Block.AveragePropogationDelay);
    }
}