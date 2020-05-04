using System;
using System.Collections.Generic;
namespace LoxLanguage.Lox.Core
{
    public abstract class Stmt
    {
        internal abstract T Accept<T>(Visitor<T> visitor);
        internal interface Visitor<T>
        {
            internal T VisitExpressionStmt(Expression stmt);
            internal T VisitPrintStmt(Print stmt);
            internal T VisitVarStmt(Var stmt);
        }
        internal class Expression : Stmt
        {
            public Expression(Expr expression)
            {
                this.expression = expression;
            }

            internal override T Accept<T>(Visitor<T> visitor)
            {
                return visitor.VisitExpressionStmt(this);
            }

            internal Expr expression;
        }
        internal class Print : Stmt
        {
            public Print(Expr expression)
            {
                this.expression = expression;
            }

            internal override T Accept<T>(Visitor<T> visitor)
            {
                return visitor.VisitPrintStmt(this);
            }

            internal Expr expression;
        }
        internal class Var : Stmt
        {
            public Var(Token name, Expr initializer)
            {
                this.name = name;
                this.initializer = initializer;
            }

            internal override T Accept<T>(Visitor<T> visitor)
            {
                return visitor.VisitVarStmt(this);
            }

            internal Token name;
            internal Expr initializer;
        }
    }
}
