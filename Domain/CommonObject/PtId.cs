using Domain.Core;

namespace Domain.CommonObject
{
    public class PtId : ValueObject
    {
        private readonly long _value;

        public long Value => _value;

        private PtId(long value)
        {
            _value = value;
        }

        public static PtId From(long value)
        {
            return new PtId(value);
        }
    }
}
