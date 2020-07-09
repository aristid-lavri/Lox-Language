using System;
using System.Collections.Generic;
namespace LoxLanguage.Lox.Core
{
public abstract class Stmt 
{

internal abstract T Accept<T>(Visitor<T> visitor);
internal interface Visitor<T>
{
    internal T VisitBlockStmt(Block stmt);
    internal T VisitExpressionStmt(Expression stmt);
    internal T VisitIfStmt(If stmt);
    internal T VisitPrintStmt(Print stmt);
    internal T VisitVarStmt(Var stmt);
}
 internal class Block : Stmt
{
 public Block ( List<Stmt> statements)
{
      this.statements = statements ;
}

internal override T Accept<T>(Visitor<T> visitor)
{
      return visitor.VisitBlockStmt(this);
    }

    internal List<Stmt> statements ;
  }
 internal class Expression : Stmt
{
 public Expression ( Expr expression)
{
      this.expression = expression ;
}

internal override T Accept<T>(Visitor<T> visitor)
{
      return visitor.VisitExpressionStmt(this);
    }

    internal Expr expression ;
  }
 internal class If : Stmt
{
 public If ( Expr condition, Stmt thenBranch, Stmt elseBranch)
{
      this.condition = condition ;
      this.thenBranch = thenBranch ;
      this.elseBranch = elseBranch ;
}

internal override T Accept<T>(Visitor<T> visitor)
{
      return visitor.VisitIfStmt(this);
    }

    internal Expr condition ;
    internal Stmt thenBranch ;
    internal Stmt elseBranch ;
  }
 internal class Print : Stmt
{
 public Print ( Expr expression)
{
      this.expression = expression ;
}

internal override T Accept<T>(Visitor<T> visitor)
{
      return visitor.VisitPrintStmt(this);
    }

    internal Expr expression ;
  }
 internal class Var : Stmt
{
 public Var ( Token name, Expr initializer)
{
      this.name = name ;
      this.initializer = initializer ;
}

internal override T Accept<T>(Visitor<T> visitor)
{
      return visitor.VisitVarStmt(this);
    }

    internal Token name ;
    internal Expr initializer ;
  }
}
}
