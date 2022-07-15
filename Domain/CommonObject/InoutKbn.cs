using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonObject
{
    public class InoutKbn : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private InoutKbn(int value)
        {
            _value = value;
        }

        public static InoutKbn From(int value)
        {
            return new InoutKbn(value);
        }
    }
}
