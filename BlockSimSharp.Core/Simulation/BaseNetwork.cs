using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;

namespace BlockSimSharp.Core.Simulation;

public abstract class BaseNetwork
{
    public virtual float BlockPropogationDelay(SimulationContext context)
    {
        var averagePropogationDelay = context
            .Get<Settings>()
            .Get<BlockSettings>()
            .AveragePropogationDelay;

        if (averagePropogationDelay == 0)
        {
            return 0;
        }
        
        return context.Get<Randomness>().Expovariate(1.0f / averagePropogationDelay);
    }

    public virtual float TransactionPropogationDelay(SimulationContext context)
    {
        var averagePropogationDelay = context
            .Get<Settings>()
            .Get<TransactionSettings>()
            .AveragePropogationDelay;

        if (averagePropogationDelay == 0)
        {
            return 0;
        } 
        
        return context.Get<Randomness>().Expovariate(1.0f / averagePropogationDelay);
    }
}