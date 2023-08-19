
namespace BlockSimSharp.BitcoinBurn.SimulationConfiguration;

public class NodeConfiguration
{
    public int StartingNodeCount { get; set; }
    
    // TODO: Maybe change this to average frequency?
    public double NodeJoinChance { get; set; }
    public double NodeQuitChance { get; set; }
    
    // Aoki et al. (2019) SimBlock
    public double StandardDeviationOfHashRate { get; set; }
    public double AverageHashRate { get; set; } 
}