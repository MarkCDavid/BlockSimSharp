namespace BlockSimSharp.BitcoinBurn.Model;

public class Block: IEquatable<Block>
{
    public int BlockId { get; init; }
    public Block? PreviousBlock { get; init; }
    public Node? Miner { get; init; }
    public int Depth { get; init;  }
    public double MinedAt { get; init; }
    public double ScheduledAt { get; init; }

    public bool Equals(Block? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return BlockId == other.BlockId;
    }

    public override bool Equals(object? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return other.GetType() == GetType() && Equals((Block)other);
    }

    public override int GetHashCode()
    {
        return BlockId;
    }
}