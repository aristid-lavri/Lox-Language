using LoxLanguage.Lox.Core;
using System;
using System.Collections.Generic;
using System.Text;
using static LoxLanguage.Lox.Core.Expr;

namespace LoxLanguage.Lox.Scanner
{
    internal class AstPrinter : Visitor<string>
    {
        internal String Print(Expr expr)
        {
            return expr.Accept(this);
        }

        string Visitor<string>.VisitBinaryExpr(Binary expr)
        {
            return Parenthesize(expr.loxOperator._lexeme, expr.left, expr.right);
        }

        string Visitor<string>.VisitGroupingExpr(Grouping expr)
        {
            return Parenthesize("group", expr.expression);
        }

        string Visitor<string>.VisitLiteralExpr(Literal expr)
        {
            if (expr.value == null) return "nil";
            return expr.value.ToString();
        }

        string Visitor<string>.VisitUnaryExpr(Unary expr)
        {
            return Parenthesize(expr.loxOperator._lexeme, expr.right);
        }

        private string Parenthesize(string name , params Expr[] exprs)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(").Append(name);
            foreach (Expr expr in exprs)
            {
                builder.Append(" ");
                builder.Append(expr.Accept(this));
            }
            builder.Append(")");

            return builder.ToString();
        }

        string Visitor<string>.VisitVariableExpr(Variable expr)
        {
            throw new NotImplementedException();
        }

        string Visitor<string>.VisitAssignExpr(Assign expr)
        {
            throw new NotImplementedException();
        }
    }
}
