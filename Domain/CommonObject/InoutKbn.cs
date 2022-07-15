using Domain.Core;

namespace Domain.CommonObject
{
    public class InoutKbn : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private InoutKbn(int value)
        {
            _value = value;
        }

        public static InoutKbn From(int value)
        {
            return new InoutKbn(value);
        }
    }
}
