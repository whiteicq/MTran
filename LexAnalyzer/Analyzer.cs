using System.Text.RegularExpressions;

namespace LexAnalyzer
{
    public class Analyzer
    {
        private Regex _identificatorRegex = new Regex(@"^[a-zA-Z_][a-zA-Z0-9_]*$");
        private Regex _integerRegex = new Regex(@"^-?\d+$");
        private Regex _stringRegex = new Regex(@"^"".*""$"); ///
        private Regex _floatRegex = new Regex(@"^[-\+]?[\d]*[\.][\d]+(?:[eE][-\+]?[\d]+)?$");
        private Regex _boolRegex = new Regex(@"^(true|false)$");

        private string? currentLine = string.Empty;

        private static string[] _boolOperators = new string[] { "==", ">=", "<=", "!=", ">", "<", };
        private static string[] _mathOperators = new string[]
        {
            "--", "++", "+=",
            "-=", "/=", "*=",
            "%=", "+", "-",
            "*", "/", "%",
        };


        private string[] _keyWords = new string[]
        {   /*"cout",*/ "cin",
            "int", "float", "bool",
            "string", "main", "return"
        };


        private readonly string _delimitersRegexPattern = $@"\s+|(\=|\(|\)|\;|{string.Join('|',
        _mathOperators
            .Concat(_boolOperators)
            .Select(
                o => $"\\{string.Join('\\', o.ToArray())}"
            )
    )})";
        private StringReader reader;
        private Queue<Token> tokens = new Queue<Token>();
        private string _text;
        private uint _line = 0;
        string lastSymbol = "";
        public string Text
        {
            set
            {
                if (string.IsNullOrWhiteSpace(value) || string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException();
                }
                else _text = value;
            }
            get
            {
                return _text;
            }
        }

        public Analyzer(string text)
        {
            Text = text;
            reader = new StringReader(Text);
            GetNextLine();
        }

        private void GetNextLine()
        {
            currentLine = reader.ReadLine();
            ++_line;
        }

        public void Analyze()
        {

            while (currentLine != null)
            {
                string[] words = Regex.Split(currentLine, _delimitersRegexPattern);
                words = words.Where(w => !string.IsNullOrWhiteSpace(w)).ToArray();
                for (int n = 0; n < words.Length; n++)
                {
                    try
                    {
                        tokens.Enqueue(new Token(GetTokenType(words[n]), words[n]));
                    }
                    catch (LexException exception)
                    {
                        throw new LexException($"Unexpected token - Line {_line}: ");
                    }
                }
                GetNextLine();
            }
            PrintTokens();
        }

        private TokenType GetTokenType(string line)
        {

            if (_keyWords.Contains(line))
            {
                return TokenType.KeyWord;
            }

            if (_mathOperators.Contains(line))
                return TokenType.MathOperator;

            if (_boolOperators.Contains(line))
                return TokenType.BoolOperator;

            if (line == ";")
            {
                return TokenType.SemicolonOperator;
            }

            if (line == "=")
            {
                return TokenType.AssignOperator;
            }

            if (line == "if" || line == "else")
            {
                return TokenType.IfStatement;
            }

            if (line == "while")
            {
                return TokenType.WhileStatement;
            }

            if (line == "for")
            {
                return TokenType.ForStatement;
            }

            if (line == "break" || line == "continue")
            {
                return TokenType.ControlStatement;
            }

            /*if(line == "\r", "    ")*/

            if (line == "cout")
            {
                return TokenType.Log;
            }

            if (line == "(" || line == "{")
            {
                return TokenType.LPar;
            }

            if (line == ")" || line == "}")
            {
                return TokenType.RPar;
            }

            if (Regex.IsMatch(line, _integerRegex.ToString()))
                return TokenType.Integer;

            if (Regex.IsMatch(line, _floatRegex.ToString()))
                return TokenType.Float;

            if (Regex.IsMatch(line, _stringRegex.ToString()))
                return TokenType.String;

            if (Regex.IsMatch(line, _boolRegex.ToString()))
                return TokenType.Bool;

            if (Regex.IsMatch(line, _identificatorRegex.ToString()))
                return TokenType.Identificator;

            throw new LexException("Null token");
        }

        private void PrintTokens()
        {
            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
        }

        public Token GetToken()
        {
            return tokens.Dequeue();
        }

    }
}
