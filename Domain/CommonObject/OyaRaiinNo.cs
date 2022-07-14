using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonObject
{
    public class OyaRaiinNo : ValueObject
    {
        private readonly long _value;

        public long Value => _value;

        private OyaRaiinNo(long value)
        {
            _value = value;
        }

        public static OyaRaiinNo From(long value)
        {
            return new OyaRaiinNo(value);
        }
    }
}
