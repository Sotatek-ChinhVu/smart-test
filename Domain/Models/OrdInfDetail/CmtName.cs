using Domain.Core;

namespace Domain.Models.OrdInfDetails
{
    public class CmtName : ValueObject
    {
        private readonly string? _value;

        public string? Value => _value;

        private CmtName(string? value)
        {
            _value = value;
        }

        public static CmtName From(string? value)
        {
            return new CmtName(value);
        }
    }
}
