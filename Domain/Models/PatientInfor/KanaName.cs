using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.PatientInfor
{
    public class KanaName : ValueObject
    {
        private readonly string _value;

        public string Value => _value;

        private KanaName(string value)
        {
            _value = value;
        }

        public static KanaName From(string value)
        {
            return new KanaName(value);
        }
    }
}
