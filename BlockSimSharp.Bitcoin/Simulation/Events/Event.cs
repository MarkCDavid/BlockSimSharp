using BlockSimSharp.Bitcoin.Model;
using BlockSimSharp.Core.Simulation;

namespace BlockSimSharp.Bitcoin.Simulation.Events;

public abstract class Event: BaseEvent<Transaction, Block, Node, Network, Consensus, Event, Scheduler, Context>
{

}