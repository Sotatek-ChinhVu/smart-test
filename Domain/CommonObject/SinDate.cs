using Domain.Core;

namespace Domain.CommonObject
{
    public class SinDate : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private SinDate(int value)
        {
            _value = value;
        }

        public static SinDate From(int value)
        {
            return new SinDate(value);
        }
    }
}
