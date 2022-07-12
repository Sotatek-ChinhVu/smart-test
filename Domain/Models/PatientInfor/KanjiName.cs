using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.PatientInfor
{
    public class KanjiName : ValueObject
    {
        private readonly string _value;

        public string Value => _value;

        private KanjiName(string value)
        {
            _value = value;
        }

        public static KanjiName From(string value)
        {
            return new KanjiName(value);
        }
    }
}
