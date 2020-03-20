using LoxLanguage.Lox.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace LoxLanguage.Lox.Seciruty
{
    internal class LoxRunTimeError : SystemException
    {
        internal Token _token ;
        private string _message;

        internal LoxRunTimeError(Token token, String message): base(message) {
            _token = token;
        }
    }
}
