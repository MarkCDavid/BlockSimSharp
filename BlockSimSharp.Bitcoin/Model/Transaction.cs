using BlockSimSharp.Core.Model;

namespace BlockSimSharp.Bitcoin.Model;

public class Transaction: BaseTransaction<Transaction, Block, Node>
{
    public float UsedGas { get; set; }
    
    public override Transaction Clone()
    {
        return new Transaction
        {
             TransactionId = TransactionId,
             TransactionCreateTime = TransactionCreateTime,
             TransactionReceiveTime = TransactionReceiveTime,
             SenderNode = SenderNode,
             ReceiverNode = ReceiverNode,
             Value = Value,
             Fee = Fee,
             SizeInMb = SizeInMb,
             UsedGas = UsedGas,
        };
    }
}