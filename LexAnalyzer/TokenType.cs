using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LexAnalyzer
{
    public enum TokenType
    {
        KeyWord,
        Integer,
        Float,
        Bool,
        String,
        BoolOperator, // >, <, ==, >=, <=, !=
        MathOperator, // +, -, *, /, %
        AssignOperator, // =
        SemicolonOperator,
        IfStatement, // if, else
        WhileStatement, // while
        ForStatement, // for
        ControlStatement, // break, continue
        Identificator,
        Space, // \t, \r, \n
        Log, // cout
        /*Plus, 
        Minus,*/
        LPar, // (, {
        RPar // ), }
    }
}
