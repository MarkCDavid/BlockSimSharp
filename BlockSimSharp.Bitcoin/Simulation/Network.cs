using BlockSimSharp.Bitcoin.Model;
using BlockSimSharp.Bitcoin.Simulation.Events;
using BlockSimSharp.Core.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.Bitcoin.Simulation;

public class Network: BaseNetwork<Transaction, Block, Node, Network, Consensus, Event, Scheduler, Context>
{

}