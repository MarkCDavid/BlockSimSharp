using BlockSimSharp.Core.Model;

namespace BlockSimSharp.BitcoinBurn.Model;

public class Node: BaseNode<Transaction, Block, Node>
{
    public Block? CurrentlyMinedBlock { get; set; } = null;
    public float HashPower { get; }
    public int DifficultyReduction { get; }
    public float TotalPowerCostInDollars { get; set; }
    public float TotalDifficultyReductionCostInBitcoins { get; set; }

    public Node(int id, float hashPower, int difficultyReduction) : base(id)
    {
        HashPower = hashPower;
        DifficultyReduction = difficultyReduction;
        TotalPowerCostInDollars = 0;
        TotalDifficultyReductionCostInBitcoins = 0;
    }
}