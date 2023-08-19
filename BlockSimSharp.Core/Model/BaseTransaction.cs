using BlockSimSharp.Core.Interface;

namespace BlockSimSharp.Core.Model;

public abstract class BaseTransaction<TTransaction, TBlock, TNode>: ICloneable<TTransaction>
        where TTransaction: BaseTransaction<TTransaction, TBlock, TNode>
        where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
        where TNode: BaseNode<TTransaction, TBlock, TNode>
{
       
}