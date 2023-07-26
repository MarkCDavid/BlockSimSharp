
using System.Diagnostics;
using System.Text.Json;
using BlockSimSharp;
using BlockSimSharp.Bitcoin;
using BlockSimSharp.Bitcoin.Events;
using BlockSimSharp.Statistics;

Stopwatch stopwatch = new Stopwatch();

stopwatch.Start();

var nodes = new List<BitcoinNode>
{
    new(0, 50),
    new(1, 20),
    new(2, 30)
};

var network = new Network();
var consensus = new BitcoinConsensus(nodes);

var statistics = new Statistics<BitcoinNode, BitcoinBlock, BitcoinTransaction>();
ITransactionContext<BitcoinNode, BitcoinBlock, BitcoinTransaction> transactionContext 
    = Configuration.Instance.TransactionContextType == "light"
    ? new LightTransactionContext<BitcoinNode, BitcoinBlock, BitcoinTransaction>()
    : new FullTransactionContext<BitcoinNode, BitcoinBlock, BitcoinTransaction>();


var context = new SimulationContext<BitcoinNode, BitcoinBlock, BitcoinTransaction>(nodes, consensus, network, transactionContext, statistics);

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

// foreach (var node in nodes)
// {
//     Console.WriteLine($"Node {node.Id}");
//
//     foreach (var block in node.BlockChain)
//     {
//         Console.WriteLine($"\tBlock {block.BlockId} ({block.Depth}) mined by {block.MinerId} at {block.Timestamp}");
//     }
// }

//  5. Resolve forks
consensus.ForkResolution();
//
// Console.WriteLine("\nGlobal consensus");
// foreach (var block in consensus.GlobalBlockChain)
// {
//     Console.WriteLine($"\tBlock {block.BlockId} ({block.Depth}) mined by {block.MinerId} at {block.Timestamp}");
// }
//

//  6. Distribute rewards
var incentives = new BitcoinIncentives();
incentives.DistributeRewards(context);

//  7. Calculate Statistics
statistics.Calculate(context);


var options = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = null
};

var filename =((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToString();
File.WriteAllText($"{filename}.json", JsonSerializer.Serialize(statistics, options));

stopwatch.Stop();

TimeSpan ts = stopwatch.Elapsed;

// Format and display the TimeSpan value.
string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
    ts.Hours, ts.Minutes, ts.Seconds,
    ts.Milliseconds / 10);

Console.WriteLine("RunTime " + elapsedTime);


