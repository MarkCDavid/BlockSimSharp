using BlockSimSharp.Base;

namespace BlockSimSharp.Bitcoin;

public class BitcoinBlock : BaseBlock<BitcoinTransaction>
{
    public float UsedGas { get; set; }
}