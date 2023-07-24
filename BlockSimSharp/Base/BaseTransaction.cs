namespace BlockSimSharp.Base;

public abstract class BaseTransaction
{
        public int TransactionId { get; set; }
        public float TransactionCreateTime { get; set; }
        public float TransactionReceiveTime { get; set; }
        
        public int SenderNodeId { get; set; }
        public int ReceiverNodeId { get; set; }
        
        public float Value { get; set; }
        public float Fee { get; set; }
        
        public float SizeInMb { get; set; }
}