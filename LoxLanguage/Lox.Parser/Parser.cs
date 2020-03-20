using LoxLanguage.Lox.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static LoxLanguage.Lox.Core.TokenKind;

namespace LoxLanguage.Lox.Parser
{
    public class Parser
    {
        private sealed class ParseError : SystemException { }

        private List<Token> _tokens;
        private int _current = 0;

        internal Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        internal Expr Parse()
        {
            try
            {
                return Expression();
            }
            catch (ParseError error)
            {
                return null;
            }
        }

        private Expr Expression()
        {
            return Equality();
        }

        private Expr Equality()
        {
            Expr left = LoxComparison();

            while (Match(Kind.BANG_EQUAL, Kind.EQUAL_EQUAL))
            {
                Token loxOperator = Previous();
                Expr right = LoxComparison();
                left = new Expr.Binary(left, loxOperator, right);
            }

            return left;
        }

        private Token Previous()
        {
            return _tokens.ElementAt(_current - 1);
        }

        private bool Match(params Kind[] types)
        {
            foreach (var kind in types)
            {
                if(Check(kind))
                {
                    Advance();
                    return true;
                }
            }

            return false;
        }

        private bool Check(Kind kind)
        {
            if (IsAtEnd()) return false;
            return Peek()._kind == kind;
        }

        private bool IsAtEnd()
        {
            return Peek()._kind == Kind.EOF;
        }

        private Token Peek()
        {
            return _tokens.ElementAt(_current);
        }

        private Token Advance()
        {
            if (!IsAtEnd()) _current++;
            return Previous();
        }

        private Expr LoxComparison()
        {
            Expr expr = Addition();

            while (Match(Kind.GREATER, Kind.GREATER_EQUAL, Kind.LESS, Kind.LESS_EQUAL))
            {
                Token loxOperator = Previous();
                Expr right = Addition();
                expr = new Expr.Binary(expr, loxOperator, right);
            }

            return expr;
        }

        private Expr Addition()
        {
            Expr expr = Multiplication();

            while (Match(Kind.MINUS, Kind.PLUS))
            {
                Token loxOperator = Previous();
                Expr right = Multiplication();
                expr = new Expr.Binary(expr, loxOperator, right);
            }

            return expr;
        }

        private Expr Multiplication()
        {
            Expr expr = Unary();

            while (Match(Kind.SLASH, Kind.STAR))
            {
                Token loxOperator = Previous();
                Expr right = Unary();
                expr = new Expr.Binary(expr, loxOperator, right);
            }

            return expr;
        }

        private Expr Unary()
        {
            if (Match(Kind.BANG, Kind.MINUS))
            {
                Token loxOperator = Previous();
                Expr right = Unary();
                return new Expr.Unary(loxOperator, right);
            }

            return Primary();
        }

        private Expr Primary()
        {
            if (Match(Kind.FALSE)) return new Expr.Literal(false);
            if (Match(Kind.TRUE)) return new Expr.Literal(true);
            if (Match(Kind.NIL)) return new Expr.Literal(null);

            if (Match(Kind.NUMBER, Kind.STRING))
            {
                return new Expr.Literal(Previous()._literal);
            }
            
            if (Match(Kind.LEFT_PARENTH))
            {
                Expr expr = Expression();
                Consume(Kind.RIGHT_PARENTH, "Expect ')' after expression.");
                return new Expr.Grouping(expr);
            }

            throw Error(Peek(), "Expect Expression");
        }

        private Token Consume(Kind kind, string message)
        {
            if (Check(kind)) return Advance();

            throw Error(Peek(), message);
        }

        private Exception Error(Token token, string message)
        {
            Lox.Seciruty.Error.Emit(token, message);
            return new ParseError();
        }

        private void Synchronize()
        {
            Advance();

            while (!IsAtEnd())
            {
                if (Previous()._kind == Kind.SEMICOLON) return;

                switch (Peek()._kind)
                {
                    case Kind.CLASS:
                    case Kind.FUN:
                    case Kind.VAR:
                    case Kind.FOR:
                    case Kind.IF:
                    case Kind.WHILE:
                    case Kind.PRINT:
                    case Kind.RETURN:
                        return;
                }

                Advance();
            }
        }
    }
}
