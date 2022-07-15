using Domain.Core;

namespace Domain.CommonObject
{
    public class RaiinNo : ValueObject
    {
        private readonly long _value;

        public long Value => _value;

        private RaiinNo(long value)
        {
            _value = value;
        }

        public static RaiinNo From(long value)
        {
            return new RaiinNo(value);
        }
    }
}
