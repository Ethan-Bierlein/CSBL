using System;
using System.Collections.Generic;
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
                [+] fugg xD
                [-] [*] [/] [<=] 
                [<] [>=] [>] [==] [!=] 
                [|] [^] 
                [&] [~] [<<] [>>] [&&] 
                [||]",
                new TokenDefinition(TokenType.CodeBlockOpen, new Regex(@"\(")),
                new TokenDefinition(TokenType.CodeBlockClose, new Regex(@"\)")),

                new TokenDefinition(TokenType.Type, new Regex(@"<[a-zA-Z0-9_\-]+>")),
                new TokenDefinition(TokenType.Name, new Regex(@"@[a-zA-Z0-9_\-]+")),
                new TokenDefinition(TokenType.TypeNameSeparator, new Regex(@"::")),

                new TokenDefinition(TokenType.NumberLiteral, new Regex(@"\b-{0,1}[0-9]+(\.[0-9]*){0,1}\b")),
                new TokenDefinition(TokenType.StringLiteral, new Regex("(\"[^\"]+\")|('[^']+')")),
                new TokenDefinition(TokenType.ArrayOpenLiteral, new Regex(@"\[\[")),
                new TokenDefinition(TokenType.ArrayCloseLiteral, new Regex(@"\]\]")),

                new TokenDefinition(TokenType.CallOperator, new Regex(@"\[(\<\<|\<\=|\<|\>\>|\>\=|\>|\=\=|\!\=|\&\&|\|\||\||\^|\&|\~|\-\>|\+|\-|\*|\/|)\]")),
                new TokenDefinition(TokenType.CallFunction, new Regex(@"\[[a-zA-Z0-9_\-]+\]")),
                new TokenDefinition(TokenType.CallCustomFunction, new Regex(@"{@[a-zA-Z0-9_\-]+}"))
            );

            Console.ReadLine();
            List<Token> tokens = tokenizer.Tokenize();
            if(tokens != null)
            {
                foreach(Token token in tokenizer.Tokenize())
                {
                    Console.WriteLine("[TOKEN ({0},{1}) -> {2}] = '{3}'", token.Position.Line, token.Position.Column, token.Type, token.Value);
                }
            }
            Console.ReadLine();
        }
    }
}
