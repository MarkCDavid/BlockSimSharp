using BlockSimSharp.Base;

namespace BlockSimSharp;

public interface ITransactionContext
{
    void CreateTransactions();
    void ExecuteTransactions();

}