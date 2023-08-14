using BlockSimSharp.BitcoinBurn.Configuration;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public class Constants
{
    public float TotalHashPower { get; }
    public float AveragePowerCostHashPower { get; }

    public Constants(SimulationContext context)
    {
        var nodes = context.Get<Nodes>();
        var settings = context.Get<Settings>();
        var burnSettings = settings.Get<BurnSettings>();
        
        TotalHashPower = 
            (nodes.Sum(node => node.HashPower)) * 2;
        
        AveragePowerCostHashPower =
            burnSettings.AveragePowerCostPerSecond / TotalHashPower;
    }
}