using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonObject
{
    public class SanteiKbn : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private SanteiKbn(int value)
        {
            _value = value;
        }

        public static SanteiKbn From(int value)
        {
            return new SanteiKbn(value);
        }
    }
}
