﻿using SpiceNetlist.SpiceObjects;

namespace SpiceNetlist.Connectors.SpiceSharpConnector
{
    public abstract class StatementProcessor
    {
        public abstract void Process(Statement statement, NetList netlist);

        public abstract void Init();
    }
}
