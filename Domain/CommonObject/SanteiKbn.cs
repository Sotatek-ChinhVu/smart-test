using Domain.Core;

namespace Domain.CommonObject
{
    public class SanteiKbn : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private SanteiKbn(int value)
        {
            _value = value;
        }

        public static SanteiKbn From(int value)
        {
            return new SanteiKbn(value);
        }
    }
}
