namespace BlockSimSharp.BitcoinBurn.Model;

public class Block
{
    public int BlockId { get; set; }
    public Block? PreviousBlock { get; set; }
    public Node? Miner { get; set; }
    public int Depth { get; set;  }
    public double SizeInMb { get; set; }

    public double ExecutedAt { get; set; }
    public double ScheduledAt { get; set; }
    public double UsedGas { get; set; }
    
    public List<Transaction> Transactions { get; set; } = new();
    
    public double TotalBlockFee => Transactions.Sum(transaction => transaction.Fee);
}