// See https://aka.ms/new-console-template for more information

using BlockSimSharp.Bitcoin.Model;
using BlockSimSharp.Bitcoin.Simulation;
using BlockSimSharp.Bitcoin.Simulation.Events;
using BlockSimSharp.Core;

var simulator = new Simulator<Transaction, Block, Node, Network, Consensus, Event, Scheduler, Context, ContextFactory>();

var contextFactory = new ContextFactory();

simulator.Simulate(contextFactory);