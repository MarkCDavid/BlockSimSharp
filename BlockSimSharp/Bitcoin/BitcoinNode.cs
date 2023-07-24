using BlockSimSharp.Base;

namespace BlockSimSharp.Bitcoin;

public class BitcoinNode: BaseNode<BitcoinBlock>
{
    public float HashPower { get; }

    public BitcoinNode(int id, float hashPower) : base(id)
    {
        HashPower = hashPower;
    }
}