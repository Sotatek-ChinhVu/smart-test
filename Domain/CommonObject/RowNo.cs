using Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommonObject
{
    public class RowNo : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private RowNo(int value)
        {
            _value = value;
        }

        public static RowNo From(int value)
        {
            return new RowNo(value);
        }
    }
}
