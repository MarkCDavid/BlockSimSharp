using BlockSimSharp.BitcoinBurn.Simulation.Data;

namespace BlockSimSharp.BitcoinBurn.SimulationStatistics.Data;

public class Nodes: List<Model.Node>
{
    public void Update(
        Simulation.Nodes nodes, 
        Incentives incentives, 
        PowerUsage powerUsage,
        DifficultyHistory difficultyHistory)
    {
        foreach (var node in nodes)
        {
            var nodeData = new Model.Node
            {
                NodeId = node.NodeId,
                HashPower = node.HashPower,
                BlocksMined = incentives.GetBlocksMined(node),
                Balance = incentives.GetBalance(node),
                PowerUsed = powerUsage.PowerUsed(node),
                DifficultyReductionParticipationHistory = difficultyHistory
                    .DifficultyReductionParticipationHistory(node)
                    .Select(participation => new Model.DifficultyReductionParticipation(participation))
                    .ToList()
            };
            
            Add(nodeData);
        }
    }
}