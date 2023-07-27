using BlockSimSharp.Core.Model;

namespace BlockSimSharp.Bitcoin.Model;

public class Transaction: BaseTransaction<Transaction>
{
    public float SizeInMb { get; set; }
    public float UsedGas { get; set; }
    
    public override Transaction DeepClone()
    {
        return new Transaction
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