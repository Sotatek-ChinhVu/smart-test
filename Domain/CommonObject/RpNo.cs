using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonObject
{
    public class RpNo : ValueObject
    {
        private readonly long _value;

        public long Value => _value;

        private RpNo(long value)
        {
            _value = value;
        }

        public static RpNo From(long value)
        {
            return new RpNo(value);
        }
    }
}
