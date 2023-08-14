using BlockSimSharp.Core.Model;

namespace BlockSimSharp.BitcoinBurn.Model;

public class Node: BaseNode<Transaction, Block, Node>
{
    public Block? CurrentlyMinedBlock { get; set; }
    public float HashPower { get; set; }

    public Node(int id, float hashPower) : base(id)
    {
        HashPower = hashPower;
    }
}