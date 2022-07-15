using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonObject
{
    public class SikyuKbn : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private SikyuKbn(int value)
        {
            _value = value;
        }

        public static SikyuKbn From(int value)
        {
            return new SikyuKbn(value);
        }
    }
}
