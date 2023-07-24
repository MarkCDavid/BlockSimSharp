using BlockSimSharp.Base;

namespace BlockSimSharp.Bitcoin;

public class BitcoinTransaction: BaseTransaction
{
    public float UsedGas { get; set; }
}