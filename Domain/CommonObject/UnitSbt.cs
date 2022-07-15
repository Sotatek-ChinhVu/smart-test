using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonObject
{
    public class UnitSbt : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private UnitSbt(int value)
        {
            _value = value;
        }

        public static UnitSbt From(int value)
        {
            return new UnitSbt(value);
        }
    }
}
