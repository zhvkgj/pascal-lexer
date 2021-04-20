using System.Linq;
using Antlr4.Runtime;
using NUnit.Framework;

namespace pascal_lexer.Test
{
    [TestFixture]
    public class LexerTest
    {
        [Test]
        [Description("Simple test")]
        public void Test1()
        {
            const string input = "program HelloWorld begin write read  end";
            var inputStream = new AntlrInputStream(input);
            var lexer = new PascalLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            tokens.Fill();
            var tokenList = tokens.GetTokens(); 
            // 12 + 1 -- EOF
            Assert.AreEqual(13, tokenList.Count);
            Assert.AreEqual(PascalLexer.IDENT, tokenList[0].Type);
            Assert.AreEqual(PascalLexer.IDENT, tokenList[2].Type);
            Assert.AreEqual(PascalLexer.IDENT, tokenList[4].Type);
            Assert.AreEqual(PascalLexer.IDENT, tokenList[6].Type);
            Assert.AreEqual(PascalLexer.IDENT, tokenList[8].Type);
            Assert.AreEqual(PascalLexer.IDENT, tokenList[11].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[1].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[3].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[5].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[9].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[10].Type);
            Assert.AreEqual(PascalLexer.Eof,tokenList.Last().Type);
            Assert.Pass();
        }

        [Test]
        [Description("Test with mixture of numbers")]
        public void Test2()
        {
            const string input = " 2131 $331AFfcdb $031477701  -%01010111011 ";
            var inputStream = new AntlrInputStream(input);
            var lexer = new PascalLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            tokens.Fill();
            var tokenList = tokens.GetTokens(); 
            // 10 + 1 -- EOF
            Assert.AreEqual(11, tokenList.Count);
            Assert.AreEqual(PascalLexer.WS, tokenList[0].Type);
            Assert.AreEqual(PascalLexer.UnsignedNumber, tokenList[1].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[2].Type);
            Assert.AreEqual(PascalLexer.UnsignedNumber, tokenList[3].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[4].Type);
            Assert.AreEqual(PascalLexer.UnsignedNumber, tokenList[5].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[6].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[7].Type);
            Assert.AreEqual(PascalLexer.SignedNumber, tokenList[8].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[9].Type); ;
            Assert.AreEqual(PascalLexer.Eof,tokenList.Last().Type);
            Assert.Pass();
        }
        
        [Test]
        [Description("Comments test")]
        public void Test3()
        {
            const string input1 = "// 2131 +faf324v __??.f \n//dsaffa ";
            var inputStream = new AntlrInputStream(input1);
            var lexer = new PascalLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            tokens.Fill();
            var tokenList = tokens.GetTokens(); 
            // 3 + 1 -- EOF
            Assert.AreEqual(4, tokenList.Count);
            Assert.AreEqual(PascalLexer.SINGLE_COMMENT, tokenList[0].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[1].Type);
            Assert.AreEqual(PascalLexer.SINGLE_COMMENT, tokenList[2].Type);
            Assert.AreEqual(PascalLexer.Eof,tokenList.Last().Type);
            
            
            const string input2 = "{ My beautiful function returns an interesting result }  ";
            inputStream = new AntlrInputStream(input2);
            lexer = new PascalLexer(inputStream);
            tokens = new CommonTokenStream(lexer);
            tokens.Fill();
            tokenList = tokens.GetTokens();
            Assert.AreEqual(4, tokenList.Count);
            Assert.AreEqual(PascalLexer.MultiComment2, tokenList[0].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[1].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[2].Type);
            Assert.AreEqual(PascalLexer.Eof,tokenList.Last().Type);

            const string input3 = "(* This is an old style comment *)\n{  This is a Turbo Pascal comment }\n// This is a Delphi comment. All is ignored till the end of the line. ";
            inputStream = new AntlrInputStream(input3);
            lexer = new PascalLexer(inputStream);
            tokens = new CommonTokenStream(lexer);
            tokens.Fill();
            tokenList = tokens.GetTokens();
            Assert.AreEqual(6, tokenList.Count);
            Assert.AreEqual(PascalLexer.MultiComment1, tokenList[0].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[1].Type);
            Assert.AreEqual(PascalLexer.MultiComment2, tokenList[2].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[3].Type);
            Assert.AreEqual(PascalLexer.SINGLE_COMMENT, tokenList[4].Type);
            Assert.AreEqual(PascalLexer.Eof,tokenList.Last().Type);
            
            Assert.Pass();
        }
        
        [Test]
        [Description("Super test with mixture of comments")]
        public void Test4()
        {
            const string input1 = "{ comment 1 // Comment 2 }";
            var inputStream = new AntlrInputStream(input1);
            var lexer = new PascalLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            tokens.Fill();
            var tokenList = tokens.GetTokens(); 
            Assert.AreEqual(2, tokenList.Count);
            Assert.AreEqual(PascalLexer.MultiComment2, tokenList[0].Type);
            Assert.AreEqual(PascalLexer.Eof,tokenList.Last().Type);
            
            const string input2 = "// comment 1 { comment 2 }  ";
            inputStream = new AntlrInputStream(input2);
            lexer = new PascalLexer(inputStream);
            tokens = new CommonTokenStream(lexer);
            tokens.Fill();
            tokenList = tokens.GetTokens(); 
            Assert.AreEqual(2, tokenList.Count);
            Assert.AreEqual(PascalLexer.SINGLE_COMMENT, tokenList[0].Type);
            Assert.AreEqual(PascalLexer.Eof,tokenList.Last().Type);

            const string input3 = " (* Comment 1 { comment 2 } *) ";
            inputStream = new AntlrInputStream(input3);
            lexer = new PascalLexer(inputStream);
            tokens = new CommonTokenStream(lexer);
            tokens.Fill();
            tokenList = tokens.GetTokens();

            Assert.AreEqual(4, tokenList.Count);
            Assert.AreEqual(PascalLexer.WS, tokenList[0].Type);
            Assert.AreEqual(PascalLexer.MultiComment1, tokenList[1].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[2].Type);
            Assert.AreEqual(PascalLexer.Eof,tokenList.Last().Type);
            
            const string input4 = "{ Comment 1 (* comment 2 *) } ";
            inputStream = new AntlrInputStream(input4);
            lexer = new PascalLexer(inputStream);
            tokens = new CommonTokenStream(lexer);
            tokens.Fill();
            tokenList = tokens.GetTokens(); 
            Assert.AreEqual(3, tokenList.Count);
            Assert.AreEqual(PascalLexer.MultiComment2, tokenList[0].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[1].Type);
            Assert.AreEqual(PascalLexer.Eof,tokenList.Last().Type);
            
            Assert.Pass();
        }
        
        [Test]
        [Description("Test with mixture of identifiers, comments and numbers")]
        public void Test5()
        {
            const string input = "// afklakfj { ffsf }\n ZEvar313_1_  343ff! -- +23111";
            var inputStream = new AntlrInputStream(input);
            var lexer = new PascalLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            tokens.Fill();
            var tokenList = tokens.GetTokens(); 
            Assert.AreEqual(15, tokenList.Count);
            Assert.AreEqual(PascalLexer.SINGLE_COMMENT, tokenList[0].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[1].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[2].Type);
            Assert.AreEqual(PascalLexer.IDENT, tokenList[3].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[4].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[5].Type);
            Assert.AreEqual(PascalLexer.UnsignedNumber, tokenList[6].Type);
            Assert.AreEqual(PascalLexer.IDENT, tokenList[7].Type);
            Assert.AreEqual(PascalLexer.BAD_CHARACTER, tokenList[8].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[9].Type);
            Assert.AreEqual(PascalLexer.BAD_CHARACTER, tokenList[10].Type);
            Assert.AreEqual(PascalLexer.BAD_CHARACTER, tokenList[11].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[12].Type);
            Assert.AreEqual(PascalLexer.SignedNumber, tokenList[13].Type);
            Assert.AreEqual(PascalLexer.Eof,tokenList.Last().Type);
            Assert.Pass();
        }
        
        [Test]
        [Description("Test with wrong comments")]
        public void Test6()
        {
            const string input1 = "// Valid comment { No longer valid comment !!\n} ";
            var inputStream = new AntlrInputStream(input1);
            var lexer = new PascalLexer(inputStream);
            var tokens = new CommonTokenStream(lexer);
            tokens.Fill();
            var tokenList = tokens.GetTokens(); 
            Assert.AreEqual(5, tokenList.Count);
            Assert.AreEqual(PascalLexer.SINGLE_COMMENT, tokenList[0].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[1].Type);
            Assert.AreEqual(PascalLexer.BAD_CHARACTER, tokenList[2].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[3].Type);
            Assert.AreEqual(PascalLexer.Eof,tokenList.Last().Type);
            
            const string input2 = "// Valid comment (* No longer valid comment !!\n *) ";
            inputStream = new AntlrInputStream(input2);
            lexer = new PascalLexer(inputStream);
            tokens = new CommonTokenStream(lexer);
            tokens.Fill();
            tokenList = tokens.GetTokens(); 
            Assert.AreEqual(7, tokenList.Count);
            Assert.AreEqual(PascalLexer.SINGLE_COMMENT, tokenList[0].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[1].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[2].Type);
            Assert.AreEqual(PascalLexer.BAD_CHARACTER, tokenList[3].Type);
            Assert.AreEqual(PascalLexer.BAD_CHARACTER, tokenList[4].Type);
            Assert.AreEqual(PascalLexer.WS, tokenList[5].Type);
            Assert.AreEqual(PascalLexer.Eof,tokenList.Last().Type);
            Assert.Pass();
        }
    }
}