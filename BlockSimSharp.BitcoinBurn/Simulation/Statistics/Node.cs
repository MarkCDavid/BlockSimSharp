namespace BlockSimSharp.BitcoinBurn.Simulation.Statistics;

public class Node
{
    public int Id { get; set; }
    public float HashPower { get; set; }
    
    public Node(Model.Node node)
    {
        Id = node.NodeId;
        HashPower = node.HashPower;
    }

}