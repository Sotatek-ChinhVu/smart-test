using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrdInfDetails
{
    public class IpnCd : ValueObject
    {
        private readonly string? _value;

        public string? Value => _value;

        private IpnCd(string? value)
        {
            _value = value;
        }

        public static IpnCd From(string? value)
        {
            return new IpnCd(value);
        }
    }
}
