using BlockSimSharp.Core.Configuration;
using BlockSimSharp.Core.Configuration.Model;

namespace BlockSimSharp.Core.Simulation;

public abstract class BaseNetwork
{
    public virtual float BlockPropogationDelay(SimulationContext context)
    {
        return Utility.Expovariate(1.0f / context.Get<Settings>().Get<BlockSettings>().AveragePropogationDelay);
    }

    public virtual float TransactionPropogationDelay(SimulationContext context)
    {
        return Utility.Expovariate(1.0f / context.Get<Settings>().Get<TransactionSettings>().AveragePropogationDelay);
    }
}