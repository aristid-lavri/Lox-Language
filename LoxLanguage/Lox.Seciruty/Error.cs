using LoxLanguage.Lox.Core;
using System;
using System.Collections.Generic;
using System.Text;
using static LoxLanguage.Lox.Core.TokenKind;

namespace LoxLanguage.Lox.Seciruty
{
    public static class Error
    {
        public static bool hadError = false;

        internal static void Emit(Token token, string message)
        {
            if (token._kind == Kind.EOF)
            {
                Report(token._line, " at end", message);
            }
            else
            {
                Report(token._line, " at '" + token._lexeme + "'", message);
            }
        }

        internal static void Emit(int line, string message)
        {
            Report(line, "", message);
        }

        internal static void Report(int line, string where, string message)
        {
            Console.WriteLine($"[line {line} ] Error {where} : {message}");
            hadError = true;
        }
    }
}
