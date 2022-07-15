using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrdInfs
{
    public class RpName : ValueObject
    {
        private readonly string? _value;

        public string? Value => _value;

        private RpName(string? value)
        {
            _value = value;
        }

        public static RpName From(string? value)
        {
            return new RpName(value);
        }
    }
}
