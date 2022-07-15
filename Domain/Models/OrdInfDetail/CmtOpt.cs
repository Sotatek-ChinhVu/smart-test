using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrdInfDetails
{
    public class CmtOpt : ValueObject
    {
        private readonly string? _value;

        public string? Value => _value;

        private CmtOpt(string? value)
        {
            _value = value;
        }

        public static CmtOpt From(string? value)
        {
            return new CmtOpt(value);
        }
    }
}
