using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LexAnalyzer
{
    internal class Token : IEquatable<Token>
    {
        public readonly TokenType Type;
        public readonly string Value;
        
        public bool Equals(Token? sec)
        {
            if (object.ReferenceEquals(sec, null)) return false;
            if (Object.ReferenceEquals(this, sec)) return true;

            //Check whether the products' properties are equal.
            return Value.Equals(sec.Value) && Type.Equals(sec.Type);
        }

        public override int GetHashCode()
        {

            //Get hash code for the Name field if it is not null.
            int hashProductName = Type == null ? 0 : Type.GetHashCode();

            //Get hash code for the Code field.
            int hashProductCode = Value.GetHashCode();

            //Calculate the hash code for the product.
            return hashProductName ^ hashProductCode;
        }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return $"< {Value} >, < {Type} >";
        }
    }
}
