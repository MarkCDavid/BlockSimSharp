namespace BlockSimSharp.BitcoinBurn.SimulationStatistics.Model;

public class Block
{
    public double MinedAt { get; }
    public int MinedBy { get; }

    public Block(BitcoinBurn.Model.Block block)
    {
        MinedAt = block.MinedAt;
        MinedBy = block.Miner?.NodeId ?? -1;
    }
}