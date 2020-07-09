using System;
using System.Collections.Generic;
using System.IO;

namespace LoxLanguage.Tool
{
    class Program
    {
        const string _path = @"E:\Personal\Projects\Learning\LoxLanguage\LoxLanguage\Lox.Core\";
        static void Main(string[] args)
        {
            DefineAst(_path, "Expr", new List<string> {
                "Assign   : Token name, Expr value",
                "Binary   : Expr left, Token loxOperator, Expr right",
                "Grouping : Expr expression",
                "Literal  : Object value",
                "Unary    : Token loxOperator, Expr right",
                "Variable : Token name"
            });

            DefineAst(_path, "Stmt", new List<string> {
                "Block      : List<Stmt> statements",
                "Expression : Expr expression",
                "If         : Expr condition, Stmt thenBranch, Stmt elseBranch",
                "Print      : Expr expression",
                "Var        : Token name, Expr initializer"
            });
        }

        private static void DefineAst(string path, string className, List<string> types)
        {
            string classPath = $@"{path}\{className}.cs";
            var file = File.Create(classPath);
            using(var writer = new StreamWriter(file,System.Text.Encoding.UTF8))
            {
                writer.WriteLine("using System;");
                writer.WriteLine("using System.Collections.Generic;");

                writer.WriteLine("namespace LoxLanguage.Lox.Core");
                writer.WriteLine("{");

                writer.WriteLine($"public abstract class {className} ");
                writer.WriteLine("{");
                // The base accept() method.                                   
                writer.WriteLine();
                writer.WriteLine("internal abstract T Accept<T>(Visitor<T> visitor);");
                DefineVisitor(writer, className, types);

                foreach (var type in types)
                {
                    string subClassName = type.Split(":")[0].Trim();
                    string fields = type.Split(":")[1].Trim();
                    DefineType(writer, className, subClassName, fields);
                }
                
                writer.WriteLine("}");
                writer.WriteLine("}");
            }
        }

        private static void DefineVisitor(StreamWriter writer, string className, List<string> types)
        {
            writer.WriteLine("internal interface Visitor<T>");
            writer.WriteLine("{");

            foreach (var type in types)
            {
                var subClassName = type.Split(":")[0].Trim();
                writer.WriteLine($@"    internal T Visit{subClassName}{className}({subClassName} { className.ToLower()});");
            }

            writer.WriteLine("}");
        }

        private static void DefineType(StreamWriter writer, string className, string subClassName, string fields)
        {
            writer.WriteLine($" internal class { subClassName} : {className}");
            writer.WriteLine("{");

            // Constructor.                                              
            writer.WriteLine($" public {subClassName} ( {fields})");
            writer.WriteLine("{");

            // Store parameters in fields.                               
            string[] fieldArrays = fields.Split(", ");
            foreach (string field in fieldArrays)
            {
                string name = field.Split(" ")[1];
                writer.WriteLine($"      this.{name} = {name} ;");
            }

            writer.WriteLine("}");

            // Visitor pattern.                                      
            writer.WriteLine();
            writer.WriteLine("internal override T Accept<T>(Visitor<T> visitor)");
            writer.WriteLine("{");
            writer.WriteLine($@"      return visitor.Visit{subClassName}{ className}(this);");
            writer.WriteLine("    }");

            // Fields.                                                   
            writer.WriteLine();
            foreach (string field in fieldArrays)
            {
                writer.WriteLine($"    internal {field} ;");
            }

            writer.WriteLine("  }");
        }
    }
}
