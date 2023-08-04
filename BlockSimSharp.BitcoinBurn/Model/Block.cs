using BlockSimSharp.Core.Model;

namespace BlockSimSharp.BitcoinBurn.Model;

public class Block : BaseBlock<Transaction, Block, Node>
{
    public float ScheduledTimestamp { get; set; }
    public float UsedGas { get; set; }
}