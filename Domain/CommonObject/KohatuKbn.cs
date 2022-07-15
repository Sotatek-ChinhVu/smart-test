using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonObject
{
    public class KohatuKbn : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private KohatuKbn(int value)
        {
            _value = value;
        }

        public static KohatuKbn From(int value)
        {
            return new KohatuKbn(value);
        }
    }
}
