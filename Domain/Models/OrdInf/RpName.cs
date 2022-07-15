using Domain.Core;

namespace Domain.Models.OrdInfs
{
    public class RpName : ValueObject
    {
        private readonly string? _value;

        public string? Value => _value;

        private RpName(string? value)
        {
            _value = value;
        }

        public static RpName From(string? value)
        {
            return new RpName(value);
        }
    }
}
