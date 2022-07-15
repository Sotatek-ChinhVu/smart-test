using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonObject
{
    public class HokenPid : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private HokenPid(int value)
        {
            _value = value;
        }

        public static HokenPid From(int value)
        {
            return new HokenPid(value);
        }
    }
}
