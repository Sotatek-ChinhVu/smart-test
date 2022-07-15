using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonObject
{
    public class ByomeiCd : ValueObject
    {
        private readonly string _value;

        public string Value => _value;

        private ByomeiCd(string value)
        {
            _value = value;
        }

        public static ByomeiCd From(string value)
        {
            return new ByomeiCd(value);
        }
    }
}
