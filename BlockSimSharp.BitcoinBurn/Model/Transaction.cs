namespace BlockSimSharp.BitcoinBurn.Model;

public class Transaction
{
    
    public int TransactionId { get; set; }
        
    public double TransactionCreateTime { get; set; }
    public double TransactionReceiveTime { get; set; }
        
    public Node SenderNode { get; set; }
    public Node ReceiverNode { get; set; }
        
    public double Value { get; set; }
    public double Fee { get; set; }
        
    public double SizeInMb { get; set; }

    public double UsedGas { get; set; }
    
    public Transaction Clone()
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