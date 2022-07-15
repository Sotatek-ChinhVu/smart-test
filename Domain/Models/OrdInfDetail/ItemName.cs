using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrdInfDetails
{
    public class ItemName : ValueObject
    {
        private readonly string? _value;

        public string? Value => _value;

        private ItemName(string? value)
        {
            _value = value;
        }

        public static ItemName From(string? value)
        {
            return new ItemName(value);
        }
    }
}
