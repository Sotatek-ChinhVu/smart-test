using Domain.Core;

namespace Domain.CommonObject
{
    public class UnitSbt : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private UnitSbt(int value)
        {
            _value = value;
        }

        public static UnitSbt From(int value)
        {
            return new UnitSbt(value);
        }
    }
}
