using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonObject
{
    public class SeqNo: ValueObject
    {
        private readonly long _value;

        public long Value => _value;

        private SeqNo(long value)
        {
            _value = value;
        }

        public static SeqNo From(long value)
        {
            return new SeqNo(value);
        }
    }
}
