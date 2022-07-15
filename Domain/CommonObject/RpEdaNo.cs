using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonObject
{
    public class RpEdaNo : ValueObject
    {
        private readonly long _value;

        public long Value => _value;

        private RpEdaNo(long value)
        {
            _value = value;
        }

        public static RpEdaNo From(long value)
        {
            return new RpEdaNo(value);
        }
    }
}
