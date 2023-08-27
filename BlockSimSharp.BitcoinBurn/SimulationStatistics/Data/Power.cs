using BlockSimSharp.BitcoinBurn.Simulation.Data;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;

namespace BlockSimSharp.BitcoinBurn.SimulationStatistics.Data;

public class Power
{
    public double TotalPowerUsed { get; set; }
    public double AveragePowerUsePerHashPower { get; set; }
    public double AveragePowerUsePerHashPowerPerBlockMined { get; set; }
    public double AveragePowerUsePerHashPowerPerBlockMinedPerSecond { get; set; }


    public void Update(Simulation.Nodes nodes, PowerUsage powerUsage, Blocks blocks, Configuration configuration)
    {
        TotalPowerUsed 
            = powerUsage.TotalPowerUsed();
        
        AveragePowerUsePerHashPower 
            = TotalPowerUsed / nodes.TotalHashPower;
        
        AveragePowerUsePerHashPowerPerBlockMined 
            = AveragePowerUsePerHashPower / blocks.Total;
        
        AveragePowerUsePerHashPowerPerBlockMinedPerSecond 
            = AveragePowerUsePerHashPowerPerBlockMined / configuration.Block.AverageIntervalInSeconds;
    }
}