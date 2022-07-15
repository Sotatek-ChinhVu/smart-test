using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrdInfDetails
{
    public class UnitName : ValueObject
    {
        private readonly string? _value;

        public string? Value => _value;

        private UnitName(string? value)
        {
            _value = value;
        }

        public static UnitName From(string? value)
        {
            return new UnitName(value);
        }
    }
}
