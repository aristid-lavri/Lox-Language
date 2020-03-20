using System;
using System.Collections.Generic;
using System.Text;
using static LoxLanguage.Lox.Core.TokenKind;

namespace LoxLanguage.Lox.Scanner
{
    public static class Reserved
    {
        public static IDictionary<String, Kind> Keywords;

        static Reserved()
        {
            Keywords = new Dictionary<string, Kind>();                        
            Keywords.Add("and",    Kind.AND);                       
            Keywords.Add("class",  Kind.CLASS);                     
            Keywords.Add("else",   Kind.ELSE);                      
            Keywords.Add("false",  Kind.FALSE);                     
            Keywords.Add("for",    Kind.FOR);                       
            Keywords.Add("fun",    Kind.FUN);                       
            Keywords.Add("if",     Kind.IF);                        
            Keywords.Add("nil",    Kind.NIL);                       
            Keywords.Add("or",     Kind.OR);                        
            Keywords.Add("print",  Kind.PRINT);                     
            Keywords.Add("return", Kind.RETURN);                    
            Keywords.Add("super",  Kind.SUPER);                     
            Keywords.Add("this",   Kind.THIS);                      
            Keywords.Add("true",   Kind.TRUE);                      
            Keywords.Add("var",    Kind.VAR);                       
            Keywords.Add("while",  Kind.WHILE);                     

        }
        
    }
}
