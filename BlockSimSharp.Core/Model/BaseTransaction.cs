using BlockSimSharp.Core.Interface;

namespace BlockSimSharp.Core.Model;

public abstract class BaseTransaction<TTransaction, TBlock, TNode>: ICloneable<TTransaction>
        where TTransaction: BaseTransaction<TTransaction, TBlock, TNode>
        where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
        where TNode: BaseNode<TTransaction, TBlock, TNode>
{
        public int TransactionId { get; set; }
        
        public float TransactionCreateTime { get; set; }
        public float TransactionReceiveTime { get; set; }
        
        public TNode SenderNode { get; set; }
        public TNode ReceiverNode { get; set; }
        
        public float Value { get; set; }
        public float Fee { get; set; }
        
        public float SizeInMb { get; set; }

        public abstract TTransaction Clone();
}