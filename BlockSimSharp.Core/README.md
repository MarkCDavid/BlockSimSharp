## Suggested Class Creation Order

- Transaction
- Block (Depends on Transaction)
- Node (Depends on Transaction and Block)
- Scheduler (Depends on Transaction, Block and Node)