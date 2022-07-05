using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.User
{
    public class Name : ValueObject
    {
        private readonly string _value;

        public string Value => _value;

        private Name(string value)
        {
            _value = value;
        }

        public static Name From(string value)
        {
            return new Name(value);
        }
    }
}
