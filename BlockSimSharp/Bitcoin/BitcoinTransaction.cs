using BlockSimSharp.Base;

namespace BlockSimSharp.Bitcoin;

public class BitcoinTransaction: BaseTransaction<BitcoinTransaction>
{
    public float UsedGas { get; set; }
    public override BitcoinTransaction DeepClone()
    {
        return new BitcoinTransaction
        {
             TransactionId = TransactionId,
             TransactionCreateTime = TransactionCreateTime,
             TransactionReceiveTime = TransactionReceiveTime,
             SenderNodeId = SenderNodeId,
             ReceiverNodeId = ReceiverNodeId,
             Value = Value,
             Fee = Fee,
             SizeInMb = SizeInMb,
             UsedGas = UsedGas,
        };
    }
}