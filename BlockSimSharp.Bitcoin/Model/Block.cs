using BlockSimSharp.Core.Model;

namespace BlockSimSharp.Bitcoin.Model;

public class Block : BaseBlock<Transaction, Block, Node>
{
    public float SizeInMb { get; set; }
    
    public float UsedGas { get; set; }
}