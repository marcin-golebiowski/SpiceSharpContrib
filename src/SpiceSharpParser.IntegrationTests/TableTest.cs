using Xunit;

namespace SpiceSharpParser.IntegrationTests
{
    public class TableTest : BaseTests
    {
        [Fact]
        public void ParsingFirstFormat()
        {
            var netlist = ParseNetlist(
                "TABLE circuit",
                "V1 1 0 1.5m",
                "R1 1 0 10",
                "E12 2 1 TABLE {V(1,0)} = (0,1) (1m,2) (2m,3)",
                "R2 2 0 10",
                ".SAVE V(2,1)",
                ".OP",
                ".END");

            var export = RunOpSimulation(netlist, "V(2,1)");
            Assert.NotNull(netlist);
            Assert.Equal(2.5, export);
        }

        [Fact]
        public void ParsingSecondFormat()
        {
            var netlist = ParseNetlist(
                "TABLE circuit",
                "V1 1 0 1.5m",
                "R1 1 0 10",
                "E12 2 1 TABLE {V(1,0)} (0,1) (1m,2) (2m,3)",
                "R2 2 0 10",
                ".SAVE V(2,1)",
                ".OP",
                ".END");
            var export = RunOpSimulation(netlist, "V(2,1)");
            Assert.NotNull(netlist);
            Assert.Equal(2.5, export);
        }

        [Fact]
        public void ParsingThirdFormat()
        {
            var netlist = ParseNetlist(
                "TABLE circuit",
                "V1 1 0 1.5m",
                "R1 1 0 10",
                "E12 2 1 TABLE {V(1,0)} ((0,1) (1m,2) (2m,3))",
                "R2 2 0 10",
                ".SAVE V(2,1)",
                ".OP",
                ".END");
            var export = RunOpSimulation(netlist, "V(2,1)");
            Assert.NotNull(netlist);
            Assert.Equal(2.5, export);
        }

        [Fact]
        public void ParsingAdvancedExpression()
        {
            var netlist = ParseNetlist(
                "TABLE circuit",
                "V1 1 0 1.5",
                "V2 2 1 2.5",
                "R1 3 2 10",
                "R2 3 0 10",
                "E12 3 2 TABLE {9 + (V(1,0) + V(2,0))} ((-10,-10) (10, 10))",
                ".SAVE V(3,2)",
                ".OP",
                ".END");
            Assert.NotNull(netlist);
            var export = RunOpSimulation(netlist, "V(3,2)");
            Assert.NotNull(netlist);
            Assert.Equal(10, export);
        }
    }
}