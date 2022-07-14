using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonObject
{
    public class HpId : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private HpId(int value)
        {
            _value = value;
        }

        public static HpId From(int value)
        {
            return new HpId(value);
        }
    }
}
