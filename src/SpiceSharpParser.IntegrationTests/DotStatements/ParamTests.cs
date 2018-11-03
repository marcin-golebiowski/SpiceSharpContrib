using System;
using Xunit;

namespace SpiceSharpParser.IntegrationTests.DotStatements
{
    public class ParamTests : BaseTests
    {
        [Fact]
        public void ParamFunctionAdvanced()
        {
            var netlist = ParseNetlist(
                "PARAM custom function test",
                "V1 IN 0 10.0",
                "R1 IN OUT 10e3",
                "C1 OUT 0 10e-6",
                ".OP",
                ".SAVE VOUT_db V(OUT)",
                ".PARAM decibels_plus_param(value,x)={log10(value)*2+x} add(x,y)={x+y}",
                ".LET VOUT_db {add(decibels_plus_param(V(OUT),1), -0.5)}",
                ".END");

            double[] export = RunOpSimulation(netlist, new string[] { "VOUT_db", "V(OUT)" });

            Assert.Equal(2.5, export[0]);
            Assert.Equal(10, export[1]);
        }

        [Fact]
        public void ParamVoltage()
        {
            var netlist = ParseNetlist(
                "PARAM voltage test",
                "V0 1  0 10.0",
                "V1 IN 0 {X}",
                "R1 IN OUT 10e3",
                "C1 OUT 0 10e-6",
                ".OP",
                ".SAVE V(OUT)",
                ".PARAM X = { V(1) }",
                ".END");

            double[] export = RunOpSimulation(netlist, new string[] { "V(OUT)" });

            Assert.Equal(10, export[0]);
        }

        [Fact]
        public void ParamFunctionManyArguments()
        {
            var netlist = ParseNetlist(
                "PARAM custom function test",
                "V1 IN 0 10.0",
                "R1 IN OUT 10e3",
                "C1 OUT 0 10e-6",
                ".OP",
                ".SAVE some_output_vector",
                ".PARAM somemagicfunction(x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13) = { x1 + x2 + x3  + x4  + x5  + x6  + x7  + x8  + x9 + x10 + x11 + x12 + x13 }",
                ".LET some_output_vector {somemagicfunction(1,2,3,4,5,6,7,8,9,10,11,12,13)}",
                ".END");

            double[] export = RunOpSimulation(netlist, new string[] { "some_output_vector" });

            Assert.Equal(13 * (13+1)/2, export[0]);
        }

        [Fact]
        public void ParamFunctionWithoutArguments()
        {
            var netlist = ParseNetlist(
                "PARAM custom function test",
                "V1 OUT 0 10.0",
                "R1 OUT 0 {somefunction()}",
                ".OP",
                ".SAVE V(OUT) @R1[i]",
                ".PARAM somefunction() = {17}",
                ".END");

            double[] export = RunOpSimulation(netlist, new string[] { "V(OUT)", "@R1[i]" });

            Assert.Equal(10.0, export[0]);
            Assert.Equal(10.0 / 17.0, export[1]);
        }

        //[Fact]
        public void ParamFunctionFunctionFactRecursiveFunction()
        {
            var netlist = ParseNetlist(
                "PARAM recurisve custom function test",
                "V1 OUT 0 60.0",
                "R1 OUT 0 {fact(3)}",
                ".OP",
                ".SAVE V(OUT) @R1[i]",
                ".PARAM fact(x) = {x == 0 ? 1: x * lazy(#fact(x -1)#)}",
                ".END");

            double[] export = RunOpSimulation(netlist, new string[] { "V(OUT)", "@R1[i]" });

            Assert.Equal(60.0, export[0]);
            Assert.Equal(60.0 / 6, export[1]);
        }

        [Fact]
        public void ParamInSubckt()
        {
            var netlist = ParseNetlist(
               "Subcircuit with PARAM",
               "V1 IN 0 4.0",
               "X1 IN OUT twoResistorsInSeries R1=1 R2=2",
               "RX OUT 0 1",
               ".SUBCKT twoResistorsInSeries input output params: R1=10 R2=100",
               "R2 input output {RES}",
               ".PARAM RES = {R1 + R2}",
               ".ENDS twoResistorsInSeries",
               ".OP",
               ".SAVE V(OUT)",
               ".END");

            double export = RunOpSimulation(netlist, "V(OUT)");

            // Get references
            double[] references = { 1.0 };

            EqualsWithTol(new double[] { export }, references);
        }

        [Fact]
        public void ParamFunctionFactRecursiveFunctionCleanSyntax()
        {
            var netlist = ParseNetlist(
                "PARAM recurisve custom function test",
                "V1 OUT 0 60.0",
                "R1 OUT 0 {fact(3)}",
                ".OP",
                ".SAVE V(OUT) @R1[i]",
                ".PARAM fact(x) = {x == 0 ? 1: x * fact(x -1)}",
                ".END");

            double[] export = RunOpSimulation(netlist, new string[] { "V(OUT)", "@R1[i]" });

            Assert.Equal(60.0, export[0]);
            Assert.Equal(60.0 / 6, export[1]);
        }
    }
}