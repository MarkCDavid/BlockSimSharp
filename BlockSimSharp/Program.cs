// See https://aka.ms/new-console-template for more information

using BlockSimSharp;
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

var transactionContext = new LightTransactionContext<BitcoinNode, BitcoinBlock, BitcoinTransaction>();


var context = new SimulationContext<BitcoinNode, BitcoinBlock, BitcoinTransaction>(nodes, consensus, network, transactionContext);

//  1. Create Transactions
transactionContext.CreateTransactions(context);

var eventQueue = new EventQueue<BitcoinNode, BitcoinBlock, BitcoinTransaction>();

//  3. Create initial events (creation of block for all nodes)
foreach (var node in nodes)
{
    eventQueue.Enqueue(new MineBlockEvent(node, 0));
}

//  4. Handle events until end of simulation
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

//  5. Resolve forks
consensus.ForkResolution();

Console.WriteLine("\nGlobal consensus");
foreach (var block in consensus.GlobalBlockChain)
{
    Console.WriteLine($"\tBlock {block.BlockId} ({block.Depth}) mined by {block.MinerId} at {block.Timestamp}");
}


//  6. Distribute rewards
var incentives = new BitcoinIncentives();
incentives.DistributeRewards(context);

//  7. Calculate Statistics






