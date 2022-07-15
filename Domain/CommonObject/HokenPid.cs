using Domain.Core;

namespace Domain.CommonObject
{
    public class HokenPid : ValueObject
    {
        private readonly int _value;

        public int Value => _value;

        private HokenPid(int value)
        {
            _value = value;
        }

        public static HokenPid From(int value)
        {
            return new HokenPid(value);
        }
    }
}
