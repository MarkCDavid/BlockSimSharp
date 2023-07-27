using BlockSimSharp.Core.Interface;

namespace BlockSimSharp.Core.Model;

public abstract class BaseTransaction<TSelf>: IDeepCloneable<TSelf>
        where TSelf: BaseTransaction<TSelf>
{
        public int TransactionId { get; set; }
        
        public float TransactionCreateTime { get; set; }
        public float TransactionReceiveTime { get; set; }
        
        public int SenderNodeId { get; set; }
        public int ReceiverNodeId { get; set; }
        
        public float Value { get; set; }
        public float Fee { get; set; }

        public abstract TSelf DeepClone();
}