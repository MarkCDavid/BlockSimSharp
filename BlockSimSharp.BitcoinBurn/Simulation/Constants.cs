using BlockSimSharp.BitcoinBurn.Configuration;
using BlockSimSharp.Core;
using BlockSimSharp.Core.Configuration;

namespace BlockSimSharp.BitcoinBurn.Simulation;

public class Constants
{
    public float TotalHashPower { get; init; }
    public float AveragePowerCostInDollarsPerSecondPerHashPower { get; init; }

    public Constants(SimulationContext context)
    {
        var nodes = context.Get<Nodes>();
        var settings = context.Get<Settings>();
        var burnSettings = settings.Get<BurnSettings>();
        
        TotalHashPower = nodes.Sum(node => node.HashPower);
        AveragePowerCostInDollarsPerSecondPerHashPower =
            burnSettings.AveragePowerCostInDollarsPerSecond / TotalHashPower;
    }
}