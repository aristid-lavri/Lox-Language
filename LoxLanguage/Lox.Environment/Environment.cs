using LoxLanguage.Lox.Core;
using LoxLanguage.Lox.Seciruty;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxLanguage.Lox.Environment
{
    public class Environment
    {
        private Environment _enclosing;
        private IDictionary<string, object> values = new Dictionary<string, object>();

        public Environment()
        {
            _enclosing = null;
        }

        public Environment(Environment enclosing)
        {
            _enclosing = enclosing;
        }

        internal object Get(Token token)
        {
            if (values.ContainsKey(token._lexeme))
            {
                values.TryGetValue(token._lexeme, out var keyValue);
                return keyValue;
            }

            if (_enclosing != null) return _enclosing.Get(token);

            throw new LoxRunTimeError(token,
                "Undefined variable '" + token._lexeme + "'.");
        }

        internal void Assign(Token token, Object value)
        {
            if (values.ContainsKey(token._lexeme))
            {
                values.Add(token._lexeme, value);
                return;
            }

            if (_enclosing != null)
            {
                _enclosing.Assign(token, value);
                return;
            }

            throw new LoxRunTimeError(token,
                "Undefined variable '" + token._lexeme + "'.");
        }

        public void Define(string token, object value)
        {
            values.Add(token, value);
        }
    }
}
