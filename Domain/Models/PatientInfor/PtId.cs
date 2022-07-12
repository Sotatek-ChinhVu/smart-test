using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.PatientInfor
{
    public class PtId : ValueObject
    {
        private readonly long _value;

        public long Value => _value;

        private PtId(long value)
        {
            _value = value;
        }

        public static PtId From(long value)
        {
            return new PtId(value);
        }
    }
}
