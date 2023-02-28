namespace Domain.Models.NextOrder
{
    public class PrefixSuffixModel
    {
        public string Code { get; private set; }

        public string Name { get; private set; }

        public PrefixSuffixModel(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
