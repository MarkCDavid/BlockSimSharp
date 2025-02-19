
namespace BlockSimSharp.BitcoinBurn.SimulationConfiguration.Model;

public class NodeSettings
{
    public int StartingNodeCount { get; set; }

    // Aoki et al. (2019) SimBlock
    public double StandardDeviationOfHashRate { get; set; }
    public double AverageHashRate { get; set; } 
}