using BlockSimSharp.Core;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public class Network : BaseNetwork
{
    public override float BlockPropogationDelay(SimulationContext context)
    {
        return 0;
    }

    public override float TransactionPropogationDelay(SimulationContext context)
    {
        return 0;
    }
}