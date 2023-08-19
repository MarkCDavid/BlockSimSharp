namespace BlockSimSharp.Core.Model;

public abstract class BaseNode<TTransaction, TBlock, TNode>
    where TTransaction: BaseTransaction<TTransaction, TBlock, TNode>
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
{
   

    

 
}