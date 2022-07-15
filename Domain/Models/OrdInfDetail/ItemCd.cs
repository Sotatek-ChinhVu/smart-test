using Domain.Core;

namespace Domain.Models.OrdInfDetails
{
    public class ItemCd : ValueObject
    {
        private readonly string? _value;

        public string? Value => _value;

        private ItemCd(string? value)
        {
            _value = value;
        }

        public static ItemCd From(string? value)
        {
            return new ItemCd(value);
        }
    }
}
