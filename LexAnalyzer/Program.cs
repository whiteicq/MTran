
namespace LexAnalyzer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using StreamReader reader = new StreamReader("D:\\MTran\\LexAnalyzer\\SourceCode.txt");
            string code = reader.ReadToEnd();
            Analyzer lexAnalyzer = new Analyzer(code);
            lexAnalyzer.Analyze();
        }
    }
}