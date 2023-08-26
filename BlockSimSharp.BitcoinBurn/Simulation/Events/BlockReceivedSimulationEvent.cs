namespace BlockSimSharp.BitcoinBurn.Simulation.Events;

public sealed class BlockReceivedSimulationEvent: SimulationEvent
{
    public override void Handle(Simulator simulator)
    {
        var nextBlockInRecipientBlockChain = Block.PreviousBlock?.BlockId == Node.LastBlock.BlockId;
        
        if (nextBlockInRecipientBlockChain)
        {
            // The last block in our block chain matches the last block that the new mined block
            // is based on. As such, we do not need to modify our local blockchain and we can simply
            // accept the block into our blockchain.
            Node.BlockChain.Add(Block);
        }
        else
        {  
            // Otherwise we have to check whether the miner of the received block has a blockchain that is longer
            // than ours. If they have a longer blockchain, we discard our blockchain and accept their blockchain.
            if (Node.BlockChainLength < Block.Depth)
            {
                Node.UpdateLocalBlockChain(Block.Miner!, Block.Depth + 1);
            }
        }
        
        simulator.BlockReceivedIntegrationEvent.Invoke(this);
    }
}