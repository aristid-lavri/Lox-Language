using LoxLanguage.Lox.Core;
using System;
using System.Collections.Generic;
using System.Text;
using static LoxLanguage.Lox.Core.TokenKind;

namespace LoxLanguage.Lox.Scanner
{
    internal class Scanner
    {
        private readonly string _source;
        private int _start = 0;
        private int _current = 0;
        private int _line = 1;

        private readonly List<Token> _tokens = new List<Token>();
        public Scanner(string source)
        {
            _source = source;
        }

        // Scan all tokens in source code
        internal List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                _start = _current;

                ScanToken();
            }

            _tokens.Add(new Token(TokenKind.Kind.EOF, "", null, _line));

            return _tokens;
        }

        // Scan token pieces by pieces
        private void ScanToken()
        {
            char c = Advance();
            switch (c)
            {
                case '(': AddToken(TokenKind.Kind.LEFT_PARENTH); break;
                case ')': AddToken(TokenKind.Kind.RIGHT_PARENTH); break;
                case '{': AddToken(TokenKind.Kind.LEFT_BRACE); break;
                case '}': AddToken(TokenKind.Kind.RIGHT_BRACE); break;
                case ',': AddToken(TokenKind.Kind.COMMA); break;
                case '.': AddToken(TokenKind.Kind.DOT); break;
                case '-': AddToken(TokenKind.Kind.MINUS); break;
                case '+': AddToken(TokenKind.Kind.PLUS); break;
                case ';': AddToken(TokenKind.Kind.SEMICOLON); break;
                case '*': AddToken(TokenKind.Kind.STAR); break;
                case '!': AddToken(Match('=') ? TokenKind.Kind.BANG_EQUAL : TokenKind.Kind.BANG); break;
                case '=': AddToken(Match('=') ? TokenKind.Kind.EQUAL_EQUAL : TokenKind.Kind.EQUAL); break;
                case '<': AddToken(Match('=') ? TokenKind.Kind.LESS_EQUAL : TokenKind.Kind.LESS); break;
                case '>': AddToken(Match('=') ? TokenKind.Kind.GREATER_EQUAL : TokenKind.Kind.GREATER); break;
                case '/':
                    if (Match('/'))
                    {
                        // A comment goes until the end of the _line.                
                        while (Peek() != '\n' && !IsAtEnd()) Advance();
                    }
                    else
                    {
                        AddToken(TokenKind.Kind.SLASH);
                    }
                    break;
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace.                      
                    break;

                case '\n':
                    _line++;
                    break;
                case '"': GetStringLiteral(); break;
                case 'o':
                    if (Peek() == 'r')
                    {
                        AddToken(TokenKind.Kind.OR);
                    }
                    break;

                default:
                    if (IsDigit(c))
                    {
                        GetNumberLiteral();
                    
                    } 
                    else if (IsAlpha(c))
                    {
                        GetIdentifier();
                    }
                    else
                    {
                        Lox.Seciruty.Error.Emit(_line, "Unexpected character.");
                    }
                    break;
            }
        }

        private void GetIdentifier()
        {
            while (IsAlphaNumeric(Peek())) Advance();
            // See if the identifier is a reserved word.   
            string text = _source.Substring(_start, (((_current -1) - _start) + 1));

            var IsKeyExist = Reserved.Keywords.TryGetValue(text, out TokenKind.Kind kind);
            if (!IsKeyExist) kind = kind = Kind.IDENTIFIER;
            AddToken(kind);
        }

        private bool IsAlphaNumeric(char c)
        {
            return IsAlpha(c) || IsDigit(c);
        }

        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_';
        }

        private void GetNumberLiteral()
        {
            while (IsDigit(Peek())) Advance();

            // Look for a fractional part.                            
            if (Peek() == '.' && IsDigit(PeekNext()))
            {
                // Consume the "."                                      
                Advance();

                while (IsDigit(Peek())) Advance();
            }

            AddToken(TokenKind.Kind.NUMBER,
                Double.Parse(_source.Substring(_start, ((_current - 1) - _start) + 1)));
        }

        private char PeekNext()
        {
            if (_current + 1 >= _source.Length) return '\0';
            return _source[_current + 1];
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private void GetStringLiteral()
        {
            while (Peek() != '\"' && !IsAtEnd())
            {
                if (Peek() == '\n') _line++;
                Advance();
            }

            // Unterminated string.                                 
            if (IsAtEnd())
            {
                Lox.Seciruty.Error.Emit(_line, "Unterminated string.");
                return;
            }

            // The closing ".                                       
            Advance();

            // Trim the surrounding quotes.                         
            string value = _source.Substring(_start + 1, (((_current - 1) - (_start + 1))));
            AddToken(TokenKind.Kind.STRING, value);
        }

        private char Peek()
        {
            if (IsAtEnd()) return '\0';
            return _source[_current];
        }

        // For composite caracteres
        private bool Match(char excpected)
        {
            if (IsAtEnd()) return false;
            if (_source[_current] != excpected) return false;

            _current++;
            return true;
        }

        private void AddToken(Kind kind)
        {
            AddToken(kind, null);
        }

        private void AddToken(Kind kind, object literal)
        {
            String text = _source.Substring(_start, (((_current - 1) - _start) + 1));
            _tokens.Add(new Token(kind, text, literal, _line));
        }

        // Go throu the caracteres
        private char Advance()
        {
            _current++;
            return _source[_current - 1];
        }

        // Check if we are at the end of the source code
        private bool IsAtEnd()
        {
            return _current >= _source.Length;
        }
    }
}
