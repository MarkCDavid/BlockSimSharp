using BlockSimSharp.BitcoinBurn.Model;

namespace BlockSimSharp.BitcoinBurn.Simulation.Events;

public sealed class BlockReceivedSimulationEvent: SimulationEvent
{
    public Block MinedBlock => Block;
    public Node Recipient => Node;
    
    public override void Handle(Simulator simulator)
    {
        var nextBlockInRecipientBlockChain = Recipient.LastBlock.Equals(MinedBlock.PreviousBlock);
        
        if (nextBlockInRecipientBlockChain)
        {
            // The last block in our block chain matches the last block that the
            // new mined block is based on. As such, we do not need to modify our
            // local blockchain and we can simply accept the block into our blockchain.
            Recipient.BlockChain.Add(Block);
        }
        else
        {  
            // Otherwise we have to check whether the miner of the received block
            // has a blockchain that is longer than ours. If they have a longer
            // blockchain, we discard our blockchain and accept their blockchain.
            if (Recipient.BlockChainLength < MinedBlock.Depth)
            {
                Recipient.UpdateLocalBlockChain(MinedBlock.Miner!, MinedBlock.Depth + 1);
            }
        }
        
        simulator.BlockReceivedIntegrationEvent.Invoke(this);
    }
}