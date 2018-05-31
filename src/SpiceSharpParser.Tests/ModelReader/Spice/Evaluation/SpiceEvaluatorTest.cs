using SpiceSharpParser.Common;
using SpiceSharpParser.ModelReader.Netlist.Spice.Evaluation;
using SpiceSharpParser.ModelReader.Netlist.Spice.Evaluation.CustomFunctions;
using System;
using System.Linq;
using Xunit;

namespace SpiceSharpevaluator.Tests.ModelReader.Spice.Evaluation
{
    public class SpiceEvaluatorTest
    {
        [Fact]
        public void GetParameterNames()
        {
            // arrange
            var p = new SpiceEvaluator();
            p.Init();
            p.SetParameter("a", 1);
            p.SetParameter("xyz", 13.0);

            Assert.Equal(3, p.GetParameterNames().Count()); // +1 for TEMP parameter
        }

        [Fact]
        public void ParentEvalautor()
        {
            // arrange
            var p = new SpiceEvaluator();
            p.SetParameter("a", 1);

            // act and assert
            var v = p.CreateChildEvaluator();

            v.SetParameter("xyz", 13.0);
            Assert.Equal(1, v.GetParameterValue("a"));

            v.SetParameter("a", 2);
            Assert.Equal(2, v.GetParameterValue("a"));
            Assert.Equal(1, p.GetParameterValue("a"));
        }

        [Fact]
        public void AddDynamicExpressionTest()
        {
            // arrange
            var v = new SpiceEvaluator();
            v.SetParameter("xyz", 13.0);

            double expressionValue = 0;

            // act
            v.AddDynamicExpression(new DoubleExpression("xyz +1", (double newValue) => { expressionValue = newValue; }), new string[] { "xyz" });
            v.SetParameter("xyz", 14);

            // assert
            Assert.Equal(15, expressionValue);
        }

        [Fact]
        public void EvaluateFailsWhenThereCurrlyBraces()
        {
            Evaluator v = new SpiceEvaluator();
            Assert.Throws<Exception>(() => v.EvaluateDouble("{1}"));
        }

        [Fact]
        public void EvaluateParameterTest()
        {
            Evaluator v = new SpiceEvaluator();
            v.SetParameter("xyz", 13.0);

            Assert.Equal(14, v.EvaluateDouble("xyz + 1"));
        }

        [Fact]
        public void GetVariablesTest()
        {
            // prepare
            Evaluator v = new SpiceEvaluator();
            v.SetParameter("xyz", 13.0);
            v.SetParameter("a", 1.0);

            // act
            var parameters = v.GetParametersFromExpression("xyz + 1 + a");

            // assert
            Assert.Contains("a", parameters);
            Assert.Contains("xyz", parameters);
        }

        [Fact]
        public void EvaluateSuffixTest()
        {
            Evaluator v = new SpiceEvaluator();
            Assert.Equal(2, v.EvaluateDouble("1V + 1"));
        }

        [Fact]
        public void TableTest()
        {
            SpiceEvaluator v = new SpiceEvaluator();
            v.Init();
            v.SetParameter("N", 1.0);
            Assert.Equal(10, v.EvaluateDouble("table(N, 1, pow(10, 1), 2 + 0, 20, 3, 30)"));
        }

        [Fact]
        public void EvaluateWithComaTest()
        {
            Evaluator v = new SpiceEvaluator();
            Assert.Equal(1.99666833293656, v.EvaluateDouble("1,99666833293656"));
        }

        [Fact]
        public void PowerInfixTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(8, evaluator.EvaluateDouble("2**3"));
        }

        [Fact]
        public void PowerInfixPrecedenceTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(7, evaluator.EvaluateDouble("2**3-1"));
        }

        [Fact]
        public void PowerInfixSecondPrecedenceTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(8, evaluator.EvaluateDouble("1+2**3-1"));
        }

        [Fact]
        public void PowerInfixThirdPrecedenceTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(17, evaluator.EvaluateDouble("1+2**3*2"));
        }

        [Fact]
        public void Round()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(1, evaluator.EvaluateDouble("round(1.2)"));
            Assert.Equal(2, evaluator.EvaluateDouble("round(1.9)"));
        }

        [Fact]
        public void PowMinusLtSpice()
        {
            // arrange
            var evaluator = new SpiceEvaluator(SpiceEvaluatorMode.LtSpice);
            evaluator.Init();

            // act and assert
            Assert.Equal(0, evaluator.EvaluateDouble("pow(-2,1.5)"));
        }

        [Fact]
        public void PwrLtSpice()
        {
            // arrange
            var evaluator = new SpiceEvaluator(SpiceEvaluatorMode.LtSpice);
            evaluator.Init();

            // act and assert
            Assert.Equal(8, evaluator.EvaluateDouble("pwr(-2,3)"));
        }

        [Fact]
        public void PwrHSpice()
        {
            // arrange
            var evaluator = new SpiceEvaluator(SpiceEvaluatorMode.HSpice);
            evaluator.Init();

            // act and assert
            Assert.Equal(-8, evaluator.EvaluateDouble("pwr(-2,3)"));
        }

        [Fact]
        public void PwrSmartSpice()
        {
            // arrange
            var evaluator = new SpiceEvaluator(SpiceEvaluatorMode.SmartSpice);
            evaluator.Init();

            // act and assert
            Assert.Equal(-8, evaluator.EvaluateDouble("pwr(-2,3)"));
        }

        [Fact]
        public void MinusPowerInfixLtSpice()
        {
            // arrange
            var evaluator = new SpiceEvaluator(SpiceEvaluatorMode.LtSpice);
            evaluator.Init();

            // act and assert
            Assert.Equal(0, evaluator.EvaluateDouble("-2**1.5"));
        }

        [Fact]
        public void PowMinusSmartSpice()
        {
            // arrange
            var evaluator = new SpiceEvaluator(SpiceEvaluatorMode.SmartSpice);
            evaluator.Init();

            // act and assert
            Assert.Equal(Math.Pow(2, (int)1.5), evaluator.EvaluateDouble("pow(-2,1.5)"));
        }

        [Fact]
        public void PowMinusHSpice()
        {
            // arrange
            var evaluator = new SpiceEvaluator(SpiceEvaluatorMode.HSpice);
            evaluator.Init();

            // act and assert
            Assert.Equal(Math.Pow(-2, (int)1.5), evaluator.EvaluateDouble("pow(-2,1.5)"));
        }

        [Fact]
        public void SgnTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator(SpiceEvaluatorMode.HSpice);
            evaluator.Init();

            // act and assert
            Assert.Equal(0, evaluator.EvaluateDouble("sgn(0)"));
            Assert.Equal(-1, evaluator.EvaluateDouble("sgn(-1)"));
            Assert.Equal(1, evaluator.EvaluateDouble("sgn(0.1)"));
        }

        [Fact]
        public void SqrtTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(2, evaluator.EvaluateDouble("sqrt(4)"));
        }

        [Fact]
        public void SqrtMinusHSpiceTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator(SpiceEvaluatorMode.HSpice);
            evaluator.Init();

            // act and assert
            Assert.Equal(-2, evaluator.EvaluateDouble("sqrt(-4)"));
        }

        [Fact]
        public void SqrtMinusSmartSpiceTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator(SpiceEvaluatorMode.SmartSpice);
            evaluator.Init();

            // act and assert
            Assert.Equal(2, evaluator.EvaluateDouble("sqrt(-4)"));
        }

        [Fact]
        public void SqrtMinusLtSpiceTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator(SpiceEvaluatorMode.LtSpice);
            evaluator.Init();

            // act and assert
            Assert.Equal(0, evaluator.EvaluateDouble("sqrt(-4)"));
        }

        [Fact]
        public void DefPositiveTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();
            evaluator.SetParameter("x1", 1);


            // act and assert
            Assert.Equal(1, evaluator.EvaluateDouble("def(x1)"));
        }

        [Fact]
        public void DefNegativeTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(0, evaluator.EvaluateDouble("def(x1)"));
        }

        [Fact]
        public void AbsTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(1, evaluator.EvaluateDouble("abs(-1)"));
        }

        [Fact]
        public void BufTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(1, evaluator.EvaluateDouble("buf(0.6)"));
            Assert.Equal(0, evaluator.EvaluateDouble("buf(0.3)"));
        }

        [Fact]
        public void CbrtTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(2, evaluator.EvaluateDouble("cbrt(8)"));
        }

        [Fact]
        public void CeilTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(3, evaluator.EvaluateDouble("ceil(2.9)"));
        }

        [Fact]
        public void DbSmartSpiceTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator(SpiceEvaluatorMode.SmartSpice);
            evaluator.Init();

            // act and assert
            Assert.Equal(20, evaluator.EvaluateDouble("db(-10)"));
        }

        [Fact]
        public void DbTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(-20, evaluator.EvaluateDouble("db(-10)"));
        }

        [Fact]
        public void ExpTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(Math.Exp(2), evaluator.EvaluateDouble("exp(2)"));
        }

        [Fact]
        public void FabsTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(3, evaluator.EvaluateDouble("fabs(-3)"));
        }

        [Fact]
        public void FlatTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act
            var res = evaluator.EvaluateDouble("flat(10)");

            // assert
            Assert.True(res >= -10 && res <= 10);
        }

        [Fact]
        public void FloorTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(2, evaluator.EvaluateDouble("floor(2.3)"));
        }

        [Fact]
        public void HypotTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(5, evaluator.EvaluateDouble("hypot(3,4)"));
        }

        [Fact]
        public void IfTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(3, evaluator.EvaluateDouble("if(0.5, 2, 3)"));
            Assert.Equal(2, evaluator.EvaluateDouble("if(0.6, 2, 3)"));
        }

        [Fact]
        public void IntTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(1, evaluator.EvaluateDouble("int(1.3)"));
        }

        [Fact]
        public void InvTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(0, evaluator.EvaluateDouble("inv(0.51)"));
            Assert.Equal(1, evaluator.EvaluateDouble("inv(0.5)"));
        }

        [Fact]
        public void LnTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(1, evaluator.EvaluateDouble("ln(e)"));
        }

        [Fact]
        public void LogTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(1, evaluator.EvaluateDouble("log(e)"));
        }

        [Fact]
        public void Log10Test()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(1, evaluator.EvaluateDouble("log10(10)"));
        }

        [Fact]
        public void MaxTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(100, evaluator.EvaluateDouble("max(10, -10, 1, 20, 100, 2)"));
        }

        [Fact]
        public void MinTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(-10, evaluator.EvaluateDouble("min(10, -10, 1, 20, 100, 2)"));
        }

        [Fact]
        public void NintTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(1, evaluator.EvaluateDouble("nint(1.2)"));
            Assert.Equal(2, evaluator.EvaluateDouble("nint(1.9)"));
        }

        [Fact]
        public void URampTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(1.2, evaluator.EvaluateDouble("uramp(1.2)"));
            Assert.Equal(0, evaluator.EvaluateDouble("uramp(-0.1)"));
        }

        [Fact]
        public void UTest()
        {
            // arrange
            var evaluator = new SpiceEvaluator();
            evaluator.Init();

            // act and assert
            Assert.Equal(1, evaluator.EvaluateDouble("u(1.2)"));
            Assert.Equal(0, evaluator.EvaluateDouble("u(-1)"));
        }
    }
}