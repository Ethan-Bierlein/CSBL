using System;
using System.Text.RegularExpressions;
using CSBL.Tokenization;

namespace CSBL
{
    /// <summary>
    /// This class is the main entry point for the CSBL language.
    /// </summary>
    public class EntryPoint
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Tokenizer tokenizer = new Tokenizer(
@"
(
    @X::<number> [attribute]
    @Y::<number> [attribute]
    @Z::<number> [attribute]
) <Vec3> [record]

(
    <Vec3> 
    @a @X [->] @b @X [->] [+]
    @a @Y [->] @b @Y [->] [+]
    @a @Z [->] @b @Z [->] [+] 
    [new]
) @Vec3_Add @a::<Vec3> @b::<Vec3> [fn]

(
    <Vec3> 
    @a @X [->] @b @X [->] [-]
    @a @Y [->] @b @Y [->] [-]
    @a @Z [->] @b @Z [->] [-] 
    [new]
) @Vec3_Add @a::<Vec3> @b::<Vec3> [fn]
",
                new TokenDefinition(TokenType.CodeBlockOpen, new Regex(@"\(")),
                new TokenDefinition(TokenType.CodeBlockClose, new Regex(@"\)")),

                new TokenDefinition(TokenType.Type, new Regex(@"<[a-zA-Z0-9_\-]+>")),
                new TokenDefinition(TokenType.Name, new Regex(@"@[a-zA-Z0-9_\-]+")),
                new TokenDefinition(TokenType.TypeNameSeparator, new Regex(@"::")),

                new TokenDefinition(TokenType.NumberLiteral, new Regex(@"\b-{0,1}[0-9]+(\.[0-9]*){0,1}\b")),
                new TokenDefinition(TokenType.StringLiteral, new Regex("(\"[^\"]+\")|('[^']+')")),
                new TokenDefinition(TokenType.ArrayOpenLiteral, new Regex(@"\[\|")),
                new TokenDefinition(TokenType.ArrayCloseLiteral, new Regex(@"\|\]")),

                new TokenDefinition(TokenType.CallOperator, new Regex(@"\[(\<\<|\<\=|\<|\>\>|\>\=|\>|\&\&|\|\||\||\^|\&|\~|\=|\-\>|\+|\-|\*|\/|)\]")),
                new TokenDefinition(TokenType.CallFunction, new Regex(@"\[[a-zA-Z0-9_\-]+\]")),
                new TokenDefinition(TokenType.CallCustomFunction, new Regex(@"{@[a-zA-Z0-9_\-]+}"))
            );

            Console.ReadLine();
            foreach(Token token in tokenizer.Tokenize())
            {
                Console.Write("{0} => {1}", token.Type, token.Value);
                Console.ReadLine();
            };
            Console.ReadLine();
        }
    }
}
