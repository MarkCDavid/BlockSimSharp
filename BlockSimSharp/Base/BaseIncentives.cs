namespace BlockSimSharp.Base;

public abstract class BaseIncentives<TNode, TBlock, TTransaction, TScheduler>
    where TNode: BaseNode<TBlock, TTransaction>
    where TBlock: BaseBlock<TTransaction>, new()
    where TTransaction: BaseTransaction<TTransaction>
    where TScheduler: BaseScheduler<TBlock, TNode, TTransaction, TScheduler>
{

    public virtual void DistributeRewards(SimulationContext<TNode, TBlock, TTransaction, TScheduler> context)
    {
        foreach (var block in context.Consensus.GlobalBlockChain)
        {
            foreach (var node in context.Nodes)
            {
                if (block.MinerId != node.Id)
                {
                    continue;
                }

                node.Blocks += 1;
                node.Balance += Configuration.Instance.BlockReward;
                node.Balance += TransactionFee(block);
            }
        }
    }

    protected float TransactionFee(TBlock block)
    {
        return block.Transactions.Sum(transaction => transaction.Fee);
    }
}