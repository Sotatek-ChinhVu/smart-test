using Domain.Core;

namespace Domain.Models.OrdInfs
{
    public class OrderId : ValueObject
    {
        private readonly long _value;

        public long Value => _value;

        private OrderId(long value)
        {
            _value = value;
        }

        public static OrderId From(long value)
        {
            return new OrderId(value);
        }
    }
}
