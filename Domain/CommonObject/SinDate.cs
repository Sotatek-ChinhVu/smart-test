using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonObject
{
    public class SinDate : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private SinDate(int value)
        {
            _value = value;
        }

        public static SinDate From(int value)
        {
            return new SinDate(value);
        }
    }
}
