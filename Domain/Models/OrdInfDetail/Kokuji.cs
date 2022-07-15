using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrdInfDetails
{
    public class Kokuji : ValueObject
    {
        private readonly string? _value;

        public string? Value => _value;

        private Kokuji(string? value)
        {
            _value = value;
        }

        public static Kokuji From(string? value)
        {
            return new Kokuji(value);
        }
    }
}
