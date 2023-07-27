using BlockSimSharp.Core.Model;

namespace BlockSimSharp.Bitcoin.Model;

public class Node: BaseNode<Transaction, Block, Node>
{
    public float HashPower { get; }

    public Node(int id, float hashPower) : base(id)
    {
        HashPower = hashPower;
    }
}