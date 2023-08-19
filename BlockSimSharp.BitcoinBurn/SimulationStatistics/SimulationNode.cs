namespace BlockSimSharp.BitcoinBurn.SimulationStatistics;

public class SimulationNode
{
    public int Id { get; set; }
    public double HashPower { get; set; }
    
    public SimulationNode(Model.Node node)
    {
        Id = node.NodeId;
        HashPower = node.HashPower;
    }

}