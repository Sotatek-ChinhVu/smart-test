using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.PatientInfor
{
    public class PtNum : ValueObject
    {
        private readonly long _value;

        public long Value => _value;

        private PtNum(long value)
        {
            _value = value;
        }

        public static PtNum From(long value)
        {
            return new PtNum(value);
        }
    }
}
