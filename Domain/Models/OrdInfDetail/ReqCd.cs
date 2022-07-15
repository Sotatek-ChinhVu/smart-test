using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrdInfDetails
{
    public class ReqCd : ValueObject
    {
        private readonly string? _value;

        public string? Value => _value;

        private ReqCd(string? value)
        {
            _value = value;
        }

        public static ReqCd From(string? value)
        {
            return new ReqCd(value);
        }
    }
}
