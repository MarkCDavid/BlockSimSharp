using BlockSimSharp.Core.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.Core.Interface;

public interface ITransactionContext<TNode, TBlock, TTransaction, TScheduler>
    where TNode: BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction<TTransaction>
    where TScheduler: BaseScheduler<TBlock, TNode, TTransaction, TScheduler>
{
    void CreateTransactions(BaseContext<TNode, TBlock, TTransaction, TScheduler> baseContext);
    (List<TTransaction>, float) CollectTransactions(TNode node, float currentTime);

}