using Domain.Core;

namespace Domain.CommonObject
{
    public class OdrKouiKbn : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private OdrKouiKbn(int value)
        {
            _value = value;
        }

        public static OdrKouiKbn From(int value)
        {
            return new OdrKouiKbn(value);
        }
    }
}
