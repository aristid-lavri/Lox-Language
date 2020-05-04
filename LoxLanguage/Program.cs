using LoxLanguage.Lox.Core;
using LoxLanguage.Lox.Interpreter;
using LoxLanguage.Lox.Parser;
using LoxLanguage.Lox.Scanner;
using LoxLanguage.Lox.Seciruty;
using System;
using System.Collections.Generic;
using System.IO;

namespace LoxLanguage
{
    class Program
    {
        private static Interpreter interpreter = new Interpreter();

        static void Main(string[] args)
        {
            //if (args.Length == 1)
            //    RunFile(args[0]);
            //else
            //    RunPrompt();
            RunFile(@"E:\Personal\Projects\Learning\LoxLanguage\LoxLanguage\Language.Files\Statements.lox");
        }

        //static void Main(string[] args)
        //{
        //    Expr expression = new Expr.Binary(
        //        new Expr.Unary(
        //            new Token(TokenKind.Kind.MINUS, "-", null, 1),
        //            new Expr.Literal(123)),
        //        new Token(TokenKind.Kind.STAR, "*", null, 1),
        //        new Expr.Grouping(
        //            new Expr.Literal(45.67)));

        //    Console.WriteLine(new AstPrinter().Print(expression));

        //    Console.ReadKey();
        //}

        private static void RunFile(string path)
        {
            var fileContent = File.ReadAllText(path);
            Run(fileContent);

            if (Error.hadError) return;
            if (Error.hadRuntimeError) return;
        }

        private static void RunPrompt()
        {
            while (true)
            {
                var source = Console.ReadLine();

                Console.Write("\n lox > ");
                Run(source);

                Error.hadError = false;
            }
        }

        private static void Run(string source)
        {
            Scanner scranner = new Scanner(source);

            List<Token> tokens = scranner.ScanTokens();

            Parser parser = new Parser(tokens);
            List<Stmt> statements = parser.Parse();

            // Stop if there was a syntax error.                   
            if (Error.hadError) return;
            
            interpreter.Interpret(statements);
            //Console.WriteLine(new AstPrinter().Print(expression));
        }
    }
}
