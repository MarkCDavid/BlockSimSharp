using BlockSimSharp.Core.Model;

namespace BlockSimSharp.BitcoinBurn.Model;

public class Node: BaseNode<Transaction, Block, Node>
{
    public Block? CurrentlyMinedBlock { get; set; } = null;
    public float HashPower { get; }

    public Node(int id, float hashPower) : base(id)
    {
        HashPower = hashPower;
    }
}