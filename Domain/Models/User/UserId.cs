using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.User
{
    public class UserId : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private UserId(int value)
        {
            _value = value;
        }

        public static UserId From(int value)
        {
            return new UserId(value);
        }
    }
}
