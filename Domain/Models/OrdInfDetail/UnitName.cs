using Domain.Core;

namespace Domain.Models.OrdInfDetails
{
    public class UnitName : ValueObject
    {
        private readonly string? _value;

        public string? Value => _value;

        private UnitName(string? value)
        {
            _value = value;
        }

        public static UnitName From(string? value)
        {
            return new UnitName(value);
        }
    }
}
