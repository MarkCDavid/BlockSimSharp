namespace BlockSimSharp.BitcoinBurn.SimulationStatistics;

public class Node
{
    public int Id { get; set; }
    public double HashPower { get; set; }
    
    public Node(Model.Node node)
    {
        Id = node.NodeId;
        HashPower = node.HashPower;
    }

}