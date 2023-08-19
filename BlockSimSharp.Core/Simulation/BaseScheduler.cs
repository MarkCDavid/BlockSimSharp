using BlockSimSharp.Core.Model;

namespace BlockSimSharp.Core.Simulation;

public abstract class BaseScheduler<TTransaction, TBlock, TNode>
    where TTransaction: BaseTransaction<TTransaction, TBlock, TNode>, new()
    where TBlock: BaseBlock<TTransaction, TBlock, TNode>, new()
    where TNode: BaseNode<TTransaction, TBlock, TNode>
{


}