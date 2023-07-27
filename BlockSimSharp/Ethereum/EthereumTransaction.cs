using BlockSimSharp.Base;

namespace BlockSimSharp.Ethereum;

public class EthereumTransaction: BaseTransaction<EthereumTransaction>
{
    public float GasLimit { get; set; }
    public float UsedGas { get; set; }
    public float GasPrice { get; set; }

    public new float Fee => UsedGas * GasPrice;
    
    public override EthereumTransaction DeepClone()
    {
        return new EthereumTransaction
        {
            TransactionId = TransactionId,
            TransactionCreateTime = TransactionCreateTime,
            TransactionReceiveTime = TransactionReceiveTime,
            SenderNodeId = SenderNodeId,
            ReceiverNodeId = ReceiverNodeId,
            Value = Value,
            SizeInMb = SizeInMb,
            GasLimit = GasLimit,
            UsedGas = UsedGas,
            GasPrice = GasPrice,
        };
    }
}