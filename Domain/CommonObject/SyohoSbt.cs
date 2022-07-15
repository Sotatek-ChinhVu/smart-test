using Domain.Core;

namespace Domain.CommonObject
{
    public class SyohoSbt : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private SyohoSbt(int value)
        {
            _value = value;
        }

        public static SyohoSbt From(int value)
        {
            return new SyohoSbt(value);
        }
    }
}
