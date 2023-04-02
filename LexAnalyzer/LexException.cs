using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexAnalyzer
{
    public class LexException : Exception
    {
        public LexException(string message = "")
            : base(message)
        { }
    }
}
