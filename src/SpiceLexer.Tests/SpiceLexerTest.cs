using SpiceGrammar;
using System.Linq;
using Xunit;

namespace SpiceLexer.Tests
{
    public class SpiceLexerTest
    {
        [Fact]
        public void VectorTest()
        {
            var tokensStr = "v(3,0)";
            SpiceLexer lexer = new SpiceLexer(new SpiceLexerOptions { HasTitle = false });
            var tokens = lexer.GetTokens(tokensStr).ToArray();

            Assert.Equal(7, tokens.Length);
        }

        [Fact]
        public void TitleTest()
        {
            var tokensStr = "Example of title";
            SpiceLexer lexer = new SpiceLexer(new SpiceLexerOptions { HasTitle = true });
            var tokens = lexer.GetTokens(tokensStr).ToArray();

            Assert.Equal(2, tokens.Length);

            Assert.True(tokens[0].SpiceTokenType == SpiceTokenType.TITLE);
            Assert.True(tokens[1].SpiceTokenType == SpiceTokenType.EOF);
        }

        [Fact]
        public void LineContinuationTest()
        {
            var tokensStr = "Example of title\nseq.part1\n+seq.part2\n+seq.part3\n";
            SpiceLexer lexer = new SpiceLexer(new SpiceLexerOptions { HasTitle = true });
            var tokens = lexer.GetTokens(tokensStr).ToArray();

            Assert.Equal(7, tokens.Length);
        }

        [Fact]
        public void Value1Text()
        {
            var tokensStr = "1picofarad";
            SpiceLexer lexer = new SpiceLexer(new SpiceLexerOptions { HasTitle = false });
            var tokens = lexer.GetTokens(tokensStr).ToArray();

            Assert.True(tokens.Length == 2);
            Assert.True(tokens[0].SpiceTokenType == SpiceTokenType.VALUE);
            Assert.True(tokens[1].SpiceTokenType == SpiceTokenType.EOF);
        }

        [Fact]
        public void Value2Text()
        {
            var tokensStr = "1pVolt";
            SpiceLexer lexer = new SpiceLexer(new SpiceLexerOptions { HasTitle = false });
            var tokens = lexer.GetTokens(tokensStr).ToArray();

            Assert.True(tokens.Length == 2);
            Assert.True(tokens[0].SpiceTokenType == SpiceTokenType.VALUE);
            Assert.True(tokens[1].SpiceTokenType == SpiceTokenType.EOF);
        }

        [Fact]
        public void Value3Text()
        {
            var tokensStr = "1e-12F";
            SpiceLexer lexer = new SpiceLexer(new SpiceLexerOptions { HasTitle = false });
            var tokens = lexer.GetTokens(tokensStr).ToArray();

            Assert.True(tokens.Length == 2);
            Assert.True(tokens[0].SpiceTokenType == SpiceTokenType.VALUE);
            Assert.True(tokens[1].SpiceTokenType == SpiceTokenType.EOF);
        }

        [Fact]
        public void Value4Text()
        {
            var tokensStr = "123.3k";
            SpiceLexer lexer = new SpiceLexer(new SpiceLexerOptions { HasTitle = false });
            var tokens = lexer.GetTokens(tokensStr).ToArray();

            Assert.True(tokens.Length == 2);
            Assert.True(tokens[0].SpiceTokenType == SpiceTokenType.VALUE);
            Assert.True(tokens[1].SpiceTokenType == SpiceTokenType.EOF);
        }

        [Fact]
        public void Value5Text()
        {
            var tokensStr = "1e-12";
            SpiceLexer lexer = new SpiceLexer(new SpiceLexerOptions { HasTitle = false });
            var tokens = lexer.GetTokens(tokensStr).ToArray();

            Assert.True(tokens.Length == 2);
            Assert.True(tokens[0].SpiceTokenType == SpiceTokenType.VALUE);
            Assert.True(tokens[1].SpiceTokenType == SpiceTokenType.EOF);
        }

        [Fact]
        public void Value6Text()
        {
            var tokensStr = "4.00000E+000";
            SpiceLexer lexer = new SpiceLexer(new SpiceLexerOptions { HasTitle = false });
            var tokens = lexer.GetTokens(tokensStr).ToArray();

            Assert.True(tokens.Length == 2);
            Assert.True(tokens[0].SpiceTokenType == SpiceTokenType.VALUE);
            Assert.True(tokens[1].SpiceTokenType == SpiceTokenType.EOF);
        }
    }
}
