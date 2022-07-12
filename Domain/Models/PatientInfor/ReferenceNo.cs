using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.PatientInfor
{
    public class ReferenceNo : ValueObject
    {
        private readonly long _value;

        public long Value => _value;

        private ReferenceNo(long value)
        {
            _value = value;
        }

        public static ReferenceNo From(long value)
        {
            return new ReferenceNo(value);
        }
    }
}
