namespace BlockSimSharp.BitcoinBurn.Model;

public class Node
{

    public IntegrationEvent<Node> DifficultyDecreaseParticipationIntegrationEvent { get; } = new();
    
    public int NodeId { get; }
    public List<Block> BlockChain { get; }
    public Block? CurrentlyMinedBlock { get; set; }
    public double HashPower { get; set; }
    public double DifficultyReduction { get; set; }
    
    public Node(int nodeId, double hashPower)
    {
        NodeId = nodeId;
        BlockChain = new List<Block> { new() };
        HashPower = hashPower;
    }

    public void UpdateDifficultyDecreaseParticipation(double difficultyReduction, bool participatesInReduction)
    {
        DifficultyReduction = difficultyReduction;

        if (participatesInReduction)
        {
            DifficultyDecreaseParticipationIntegrationEvent.Invoke(this);
        }
    }
    
    public int BlockChainLength => BlockChain.Count - 1;
    public Block LastBlock => BlockChain[BlockChainLength];

    public void UpdateLocalBlockChain(Node sourceNode, int depth)
    {
        // Say NodeA has a BlockChain of [b0, b1, b2] and then they mine Block b3.
        // A receive event for NodeB is scheduled some time in the future.
        // Before the receive event is handled, NodeA mines Block b4. Their
        // copy of the BlockChain is [b0, b1, b2, b3, b4].
        // Eventually, the receive event for NodeB is handled. Say their copy
        // of the BlockChain is [b0] only. Our expectation is, that after the
        // update of local BlockChain, NodeB would have [b0, b1, b2, b3] and not
        // b4, as it was mined after propogation of the b3 happened.
        for (var index = 0; index < depth; index++)
        {
            if (index < BlockChain.Count)
            {
                BlockChain[index] = sourceNode.BlockChain[index];
            }
            else
            {
                BlockChain.Add(sourceNode.BlockChain[index]);
            }
        }
    }
}