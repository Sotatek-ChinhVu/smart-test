using Domain.Core;

namespace Domain.CommonObject
{
    public class KohatuKbn : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private KohatuKbn(int value)
        {
            _value = value;
        }

        public static KohatuKbn From(int value)
        {
            return new KohatuKbn(value);
        }
    }
}
