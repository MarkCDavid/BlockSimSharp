using BlockSimSharp.Core.Configuration.Contracts;

namespace BlockSimSharp.BitcoinBurn.Configuration;

public class NodeSettings: ISettings
{
    public int StartingNodeCount { get; set; }
    
    // TODO: Maybe change this to average frequency?
    public float NodeJoinChance { get; set; }
    public float NodeQuitChance { get; set; }
    
    // Aoki et al. (2019) SimBlock
    public float StandardDeviationOfHashRate { get; set; }
    public float AverageHashRate { get; set; } 
}