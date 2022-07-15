using Domain.Core;

namespace Domain.Models.OrdInfDetails
{
    public class Bunkatu : ValueObject
    {
        private readonly string? _value;

        public string? Value => _value;

        private Bunkatu(string? value)
        {
            _value = value;
        }

        public static Bunkatu From(string? value)
        {
            return new Bunkatu(value);
        }
    }
}
