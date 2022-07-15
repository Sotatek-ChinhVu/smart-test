using Domain.Core;

namespace Domain.CommonObject
{
    public class TosekiKbn : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private TosekiKbn(int value)
        {
            _value = value;
        }

        public static TosekiKbn From(int value)
        {
            return new TosekiKbn(value);
        }
    }
}
