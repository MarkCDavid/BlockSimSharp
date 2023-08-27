using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public class EventTimeDeltas
{
    public EventTimeDeltas(Configuration configuration, Randomness randomness, Difficulty difficulty)
    {
        _configuration = configuration;
        _randomness = randomness;
        _difficulty = difficulty;
    }
    
    public double BlockMiningTimeDelta(Node miner)
    {
        var relativeHashPower = _difficulty.GetRelativeHashPower(miner);
        var blockRate = 1.0 / _configuration.Block.AverageIntervalInSeconds;
        return _randomness.Expovariate(relativeHashPower * blockRate);
    }
    
    public double BlockPropogationTimeDelta()
    {
        if (_configuration.Block.AveragePropogationDelay == 0)
        {
            return 0;
        }
        
        return _randomness.Expovariate(1.0f / _configuration.Block.AveragePropogationDelay);
    }
    
    private readonly Configuration _configuration;
    private readonly Randomness _randomness;
    private readonly Difficulty _difficulty;
}