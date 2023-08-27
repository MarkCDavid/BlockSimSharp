using BlockSimSharp.BitcoinBurn.Model;
using BlockSimSharp.BitcoinBurn.SimulationConfiguration;
using BlockSimSharp.BitcoinBurn.Utility;

namespace BlockSimSharp.BitcoinBurn.Simulation.Data;

public class DifficultyHistory
{
    public DifficultyHistory(Configuration configuration)
    {
        _configuration = configuration;
    }
    
    private readonly List<double> _difficultyChangeHistory = new();
    private readonly DefaultDictionary<Node, List<DifficultyReductionParticipation>> _difficultyReductionParticipationHistory 
        = new(() => new List<DifficultyReductionParticipation>());

    public IReadOnlyList<double> DifficultyChangeHistory => _difficultyChangeHistory;
    public List<DifficultyReductionParticipation> DifficultyReductionParticipationHistory(Node node) => _difficultyReductionParticipationHistory[node];
    
    public void OnDifficultyChange(double difficulty)
    {
        _difficultyChangeHistory.Add(difficulty);
    }

    public void OnDifficultyParticipation(Node node)
    {
        var difficultyParticipation = new DifficultyReductionParticipation()
        {
            Epoch = node.BlockChainLength / _configuration.Difficulty.AdjustmentFrequencyInBlocks,
            DifficultyReduction = node.DifficultyReduction,
        };
        
        _difficultyReductionParticipationHistory[node].Add(difficultyParticipation);
    }
    
    private readonly Configuration _configuration;
}

public class DifficultyReductionParticipation
{
    public int Epoch { get; init; }
    public double DifficultyReduction { get; init; }
}