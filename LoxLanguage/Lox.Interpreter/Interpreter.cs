using LoxLanguage.Lox.Core;
using LoxLanguage.Lox.Environment;
using LoxLanguage.Lox.Seciruty;
using System;
using System.Collections.Generic;
using static LoxLanguage.Lox.Core.TokenKind;

namespace LoxLanguage.Lox.Interpreter
{
    class Interpreter : Expr.Visitor<Object>, Stmt.Visitor<object>
    {
        private Environment.Environment environment = new Environment.Environment();
        object Expr.Visitor<object>.VisitBinaryExpr(Expr.Binary expr)
        {
            Object left = Evaluate(expr.left);
            Object right = Evaluate(expr.right);

            switch (expr.loxOperator._kind) 
            {
                case Kind.PLUS:
                {
                    if (left is double && right is double)
                    {
                        return (double)left + (double)right;
                    }

                    if (left is string && right is string)
                    {
                        return (string)left + (string)right;
                    }
                        throw new LoxRunTimeError(expr.loxOperator,
                        "Operands must be two numbers or two strings.");
                }
                case Kind.GREATER:
                {
                    CheckNumberOperands(expr.loxOperator, left, right);
                    return (double)left > (double)right;
                }
                case Kind.GREATER_EQUAL:
                {
                    CheckNumberOperands(expr.loxOperator, left, right);
                    return (double)left >= (double)right;
                }
                case Kind.LESS:
                    CheckNumberOperands(expr.loxOperator, left, right);
                    return (double)left < (double)right;
                case Kind.LESS_EQUAL:
                    CheckNumberOperands(expr.loxOperator, left, right);
                    return (double)left <= (double)right;
                case Kind.BANG_EQUAL: return !IsEqual(left, right);
                case Kind.EQUAL_EQUAL: return IsEqual(left, right);
                case Kind.MINUS:
                {
                    CheckNumberOperands(expr.loxOperator, left, right);
                    return (double)left - (double)right;
                }
                case Kind.SLASH:
                {
                    CheckNumberOperands(expr.loxOperator, left, right);
                    return (double)left / (double)right;
                }
                case Kind.STAR:
                {
                    CheckNumberOperands(expr.loxOperator, left, right);
                    return (double)left * (double)right;
                }
            }

            return null;
        }

        private void CheckNumberOperands(Token loxOperator, object left, object right)
        {
            if (left is Double && right is double) return;
            throw new LoxRunTimeError(loxOperator, "Operand must be a number.");
        }

        private bool IsEqual(object a, object b)
        {
            if (a == null && b == null) return true;
            if (a == null) return false;

            return a.Equals(b);
        }

        object Expr.Visitor<object>.VisitGroupingExpr(Expr.Grouping expr)
        {
            return Evaluate(expr.expression);
        }

        object Expr.Visitor<object>.VisitLiteralExpr(Expr.Literal expr)
        {
            return expr.value;
        }

        object Expr.Visitor<object>.VisitUnaryExpr(Expr.Unary expr)
        {
            Object right = Evaluate(expr.right);

            switch (expr.loxOperator._kind) 
            {                
                case Kind.BANG:
                    return !IsTruthy(right);
                case Kind.MINUS:
                    return -(double)right;
            }

            return null;
        }

        private bool IsTruthy(object expr)
        {
            if (expr == null) return false;
            if (expr is bool) return (bool)expr;

            return true;
        }

        private object Evaluate(Expr expr)
        {
            return expr.Accept(this);
        }

        internal void Interpret(List<Stmt> statements)
        {
            try
            {
                foreach(Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            catch(LoxRunTimeError error)
            {
                Lox.Seciruty.Error.RuntimeError(error);
            }
        }

        private void Execute(Stmt stmt)
        {
            stmt.Accept(this);
        }

        private string Stringify(object obj)
        {
            if (obj == null) return "nil";

            // Hack. Work around Java adding ".0" to integer-valued doubles.
            if (obj is double) {
                string text = obj.ToString();
                if (text.EndsWith(".0"))
                {
                    text = text.Substring(0, text.Length - 2);
                }
                return text;
            }

            return obj.ToString();
        }

        object Stmt.Visitor<object>.VisitExpressionStmt(Stmt.Expression stmt)
        {
            Evaluate(stmt.expression);
            return null;
        }

        object Stmt.Visitor<object>.VisitPrintStmt(Stmt.Print stmt)
        {
            Object value = Evaluate(stmt.expression);
            Console.WriteLine(Stringify(value));
            return null;
        }

        object Stmt.Visitor<object>.VisitVarStmt(Stmt.Var stmt)
        {
            Object value = null;
            if (stmt.initializer != null)
            {
                value = Evaluate(stmt.initializer);
            }

            environment.Define(stmt.name._lexeme, value);
            return null;
        }

        object Expr.Visitor<object>.VisitVariableExpr(Expr.Variable expr)
        {
            return environment.Get(expr.name);
        }

        object Expr.Visitor<object>.VisitAssignExpr(Expr.Assign expr)
        {
            Object value = Evaluate(expr.value);

            environment.Assign(expr.name, value);
            return value;
        }

        object Stmt.Visitor<object>.VisitBlockStmt(Stmt.Block stmt)
        {
            ExecuteBlock(stmt.statements, new Environment.Environment(environment));
            return null;
        }

        void ExecuteBlock(List<Stmt> statements, Environment.Environment environment)
        {
            Environment.Environment previous = this.environment;
            try
            {
                this.environment = environment;

                foreach (Stmt statement in statements)
                {
                    Execute(statement);
                }
            }
            finally
            {
                this.environment = previous;
            }
        }

        object Stmt.Visitor<object>.VisitIfStmt(Stmt.If stmt)
        {
            if (IsTruthy(Evaluate(stmt.condition)))
            {
                Execute(stmt.thenBranch);
            }
            else if (stmt.elseBranch != null)
            {
                Execute(stmt.elseBranch);
            }
            return null;
        }
    }
}
