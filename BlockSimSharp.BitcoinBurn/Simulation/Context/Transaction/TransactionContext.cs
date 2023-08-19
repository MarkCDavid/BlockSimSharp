using BlockSimSharp.BitcoinBurn.Model;

namespace BlockSimSharp.BitcoinBurn.Simulation.Context.Transaction;

public abstract class TransactionContext
{
    public abstract void CreateTransactions();
    public abstract (List<Model.Transaction>, double) CollectTransactions(Node node, double currentTime);
}