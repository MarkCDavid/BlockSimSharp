// See https://aka.ms/new-console-template for more information

using BlockSimSharp;
using BlockSimSharp.Base;
using BlockSimSharp.Bitcoin;
using BlockSimSharp.Bitcoin.Events;

var nodes = new List<BitcoinNode>
{
    new(0, 50),
    new(1, 20),
    new(2, 30)
};

var network = new Network();
var consensus = new BitcoinConsensus(nodes);

var context = new SimulationContext<BitcoinNode, BitcoinBlock>(nodes, consensus, network);
var eventQueue = new EventQueue<BitcoinNode, BitcoinBlock>();

foreach (var node in nodes)
{
    eventQueue.Enqueue(new MineBlockEvent(node, 0));
}

//
// var consensus = new BitcoinConsensus();
//
var clock = 0f;
while (clock < Configuration.Instance.SimulationLengthInSeconds && !eventQueue.IsEmpty())
{
    var @event = eventQueue.Dequeue();  

    var futureEvents = @event.Handle(context);
    
    foreach(var futureEvent in futureEvents)
        eventQueue.Enqueue(futureEvent);
    
    clock = @event.EventTime;
}

foreach (var node in nodes)
{
    Console.WriteLine($"Node {node.Id}");

    foreach (var block in node.BlockChain)
    {
        Console.WriteLine($"\tBlock {block.BlockId} ({block.Depth}) mined by {block.MinerId} at {block.Timestamp}");
    }
}



//  1. Create Transactions
//  2. Add Genesis Block to all Nodes
//  3. Create initial events (creation of block for all nodes)
//  4. Handle events until end of simulation
//      4.1. Handle Create Block Event
//      4.2. Handle Receive Block Event
//  5. Resolve forks
//  6. Distribute rewards
//  7. Calculate Statistics