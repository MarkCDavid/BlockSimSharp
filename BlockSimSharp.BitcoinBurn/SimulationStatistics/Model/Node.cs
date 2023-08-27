namespace BlockSimSharp.BitcoinBurn.SimulationStatistics.Model;

public class Node
{
    public int NodeId { get; set; }
    public double HashPower { get; set; }
    public int BlocksMined { get; set; }
    public double Balance { get; set; }
    public double PowerUsed { get; set; }
    public List<DifficultyReductionParticipation> DifficultyReductionParticipationHistory { get; set; }
}