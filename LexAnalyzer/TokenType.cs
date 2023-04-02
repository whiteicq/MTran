using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LexAnalyzer
{
    public enum TokenType
    {
        Integer,
        Float,
        Bool,
        String,
        Operator,   
        KeyWord,
        Identificator
    }
}
