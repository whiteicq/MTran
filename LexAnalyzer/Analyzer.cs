using System.Text.RegularExpressions;

namespace LexAnalyzer
{
    internal class Analyzer
    {
        private Regex _identificatorRegex = new Regex(@"^[a-zA-Z_][a-zA-Z0-9_]*$");
        private Regex _integerRegex = new Regex(@"^-?\d+$");
        private Regex _stringRegex = new Regex(@"^"".*""$"); /// 
        private Regex _floatRegex = new Regex(@"^[-\+]?[\d]*[\.][\d]+(?:[eE][-\+]?[\d]+)?$");
        private Regex _boolRegex = new Regex(@"^(true|false)$");

        private string? currentLine = string.Empty;

        private string[] _keyWords = new string[]
        {   "cout", "cin", "while",
            "for", "continue", "break",
            "if", "else", "if else",
            "int", "float", "bool",
            "string", "main", "return"
        };
        private static string[] _operators = new string[]
        {   ">>", "<<", "++", "--", "!=",
            ">=", "==", "/=", "-=", "^=",
            "+=", "*=", "%=", "<=",
            ",", "+", "-",
            "/", "*", "%",
             "=",
            ">", ";",
            "<", "(", ")",
            "{", "}"
        };

        private readonly string _delimitersRegexPattern = $@"\s+|({string.Join('|',
        _operators
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

                        if (n == 0)
                        {

                            tokens.Enqueue(new Token(GetTokenType(words[n], lastSymbol), words[n]));
                        }
                        else
                        {
                            lastSymbol = words[n - 1];
                            tokens.Enqueue(new Token(GetTokenType(words[n], lastSymbol), words[n]));
                        }
                        if (n == words.Length) lastSymbol = words[n];


                    }
                    catch (LexException exception)
                    {
                        if (exception.Message == "Wrong token")
                        {
                            throw new LexException($"Wrong token - Line {_line}: {words[n]}");
                        }
                        else
                        {
                            throw new LexException($"Unexpected token - Line {_line}: {lastSymbol}");
                        }

                    }
                }

                GetNextLine();
            }

            PrintTokens();
            lastSymbol = "";
        }

        private TokenType GetTokenType(string line, string preline)
        {

            if (_keyWords.Contains(line))
                return TokenType.KeyWord;

            if (_operators.Contains(line))
                return TokenType.Operator;

            if (Regex.IsMatch(line, _integerRegex.ToString()))
                return TokenType.Integer;

            if (Regex.IsMatch(line, _floatRegex.ToString()))
                return TokenType.Float;

            if (Regex.IsMatch(line, _stringRegex.ToString()))
                return TokenType.String;

            if (Regex.IsMatch(line, _boolRegex.ToString()))
                return TokenType.Bool;

            if (_keyWords.Contains(preline) || _operators.Contains(preline))
                if (Regex.IsMatch(line, _identificatorRegex.ToString()))
                    return TokenType.Identificator;
                else
                {
                    lastSymbol = line;
                    throw new LexException("Wrong token");

                }

            lastSymbol = line;
            throw new LexException("Null token");
        }

        private void PrintTokens()
        {
            List<Token> integers = new List<Token>();
            List<Token> booleans = new List<Token>();
            List<Token> strings = new List<Token>();
            List<Token> floats = new List<Token>();
            List<Token> operators = new List<Token>();
            List<Token> keywords = new List<Token>();
            List<Token> identificators = new List<Token>();

            foreach (var token in tokens)
            {
                if (token.Type is TokenType.Integer) integers.Add(token);
                if (token.Type is TokenType.Bool) booleans.Add(token);
                if (token.Type is TokenType.String) strings.Add(token);
                if (token.Type is TokenType.Float) floats.Add(token);
                if (token.Type is TokenType.Operator) operators.Add(token);
                if (token.Type is TokenType.KeyWord) keywords.Add(token);
                if (token.Type is TokenType.Identificator) identificators.Add(token);
            }

            integers = integers.Distinct().ToList();
            booleans = booleans.Distinct().ToList();
            strings = strings.Distinct().ToList();
            floats = floats.Distinct().ToList();
            operators = operators.Distinct().ToList();
            keywords = keywords.Distinct().ToList();
            identificators = identificators.Distinct().ToList();

            foreach (var integer in integers) Console.WriteLine(integer);
            Console.WriteLine("-----------------");
            foreach(var boolean in booleans) Console.WriteLine(boolean);
            Console.WriteLine("-----------------");
            foreach (var str in strings) Console.WriteLine(str);
            Console.WriteLine("-----------------");
            foreach (var fl in floats) Console.WriteLine(fl);
            Console.WriteLine("-----------------");
            foreach (var op in operators) Console.WriteLine(op);
            Console.WriteLine("-----------------");
            foreach (var kw in keywords) Console.WriteLine(kw);
            Console.WriteLine("-----------------");
            foreach (var id in identificators) Console.WriteLine(id);

        }

        public Token GetToken()
        {
            return tokens.Dequeue();
        }

    }
}
