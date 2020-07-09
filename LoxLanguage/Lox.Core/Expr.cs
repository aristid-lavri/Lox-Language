using System;
using System.Collections.Generic;
namespace LoxLanguage.Lox.Core
{
public abstract class Expr 
{

internal abstract T Accept<T>(Visitor<T> visitor);
internal interface Visitor<T>
{
    internal T VisitAssignExpr(Assign expr);
    internal T VisitBinaryExpr(Binary expr);
    internal T VisitGroupingExpr(Grouping expr);
    internal T VisitLiteralExpr(Literal expr);
    internal T VisitUnaryExpr(Unary expr);
    internal T VisitVariableExpr(Variable expr);
}
 internal class Assign : Expr
{
 public Assign ( Token name, Expr value)
{
      this.name = name ;
      this.value = value ;
}

internal override T Accept<T>(Visitor<T> visitor)
{
      return visitor.VisitAssignExpr(this);
    }

    internal Token name ;
    internal Expr value ;
  }
 internal class Binary : Expr
{
 public Binary ( Expr left, Token loxOperator, Expr right)
{
      this.left = left ;
      this.loxOperator = loxOperator ;
      this.right = right ;
}

internal override T Accept<T>(Visitor<T> visitor)
{
      return visitor.VisitBinaryExpr(this);
    }

    internal Expr left ;
    internal Token loxOperator ;
    internal Expr right ;
  }
 internal class Grouping : Expr
{
 public Grouping ( Expr expression)
{
      this.expression = expression ;
}

internal override T Accept<T>(Visitor<T> visitor)
{
      return visitor.VisitGroupingExpr(this);
    }

    internal Expr expression ;
  }
 internal class Literal : Expr
{
 public Literal ( Object value)
{
      this.value = value ;
}

internal override T Accept<T>(Visitor<T> visitor)
{
      return visitor.VisitLiteralExpr(this);
    }

    internal Object value ;
  }
 internal class Unary : Expr
{
 public Unary ( Token loxOperator, Expr right)
{
      this.loxOperator = loxOperator ;
      this.right = right ;
}

internal override T Accept<T>(Visitor<T> visitor)
{
      return visitor.VisitUnaryExpr(this);
    }

    internal Token loxOperator ;
    internal Expr right ;
  }
 internal class Variable : Expr
{
 public Variable ( Token name)
{
      this.name = name ;
}

internal override T Accept<T>(Visitor<T> visitor)
{
      return visitor.VisitVariableExpr(this);
    }

    internal Token name ;
  }
}
}
