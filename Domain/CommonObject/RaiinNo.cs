using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonObject
{
    public class RaiinNo : ValueObject
    {
        private readonly long _value;

        public long Value => _value;

        private RaiinNo(long value)
        {
            _value = value;
        }

        public static RaiinNo From(long value)
        {
            return new RaiinNo(value);
        }
    }
}
