namespace BlockSimSharp.BitcoinBurn.SimulationStatistics;

public class SimulationNode
{
    public int Id { get; set; }
    public double HashPower { get; set; }
    public double PowerUsed { get; set; }
    public List<NodeDifficultyDecreaseParticipationHistory> NodeDifficultyDecreaseParticipationHistory { get; set; }
    
    public SimulationNode(Model.Node node)
    {
        Id = node.NodeId;
        HashPower = node.HashPower;
        PowerUsed = node.PowerUsed;
        NodeDifficultyDecreaseParticipationHistory = node.NodeDifficultyDecreaseParticipationHistory
            .Select(history =>new NodeDifficultyDecreaseParticipationHistory()
            {
                DifficultyDecreaseDuringCurrentEpoch = history.DifficultyDecreaseDuringCurrentEpoch,
                DifficultyDecrease = history.DifficultyDecrease
            })
            .ToList();
    }
}

public class NodeDifficultyDecreaseParticipationHistory
{
    public bool DifficultyDecreaseDuringCurrentEpoch { get; set; }
    public double DifficultyDecrease { get; set; }
}