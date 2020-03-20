using System;
using System.Collections.Generic;
using System.Text;
using static LoxLanguage.Lox.Core.TokenKind;

namespace LoxLanguage.Lox.Core
{
    internal class Token
    {
        internal readonly Kind _kind;
        internal readonly string _lexeme;
        internal readonly object _literal;
        internal readonly int _line;

        public Token(Kind kind, string lexeme, object literal, int line)
        {
            _kind = kind;
            _lexeme = lexeme;
            _literal = literal;
            _line = line;
        }

        public override string ToString()
        {
            return $"{_kind} {_lexeme} {_literal}";
        }
    }
}
