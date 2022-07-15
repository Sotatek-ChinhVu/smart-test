using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonObject
{
    public class SinKouiKbn : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private SinKouiKbn(int value)
        {
            _value = value;
        }

        public static SinKouiKbn From(int value)
        {
            return new SinKouiKbn(value);
        }
    }
}
