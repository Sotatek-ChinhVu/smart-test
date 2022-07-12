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
        private readonly long _value;

        public long Value => _value;

        private UserId(long value)
        {
            _value = value;
        }

        public static UserId From(long value)
        {
            return new UserId(value);
        }
    }
}
