using Domain.Core;

namespace Domain.CommonObject
{
    public class RpEdaNo : ValueObject
    {
        private readonly long _value;

        public long Value => _value;

        private RpEdaNo(long value)
        {
            _value = value;
        }

        public static RpEdaNo From(long value)
        {
            return new RpEdaNo(value);
        }
    }
}
