namespace Domain.Models.SuperSetDetail;
public class PrefixSuffixModel
{
    public PrefixSuffixModel(string code, string name)
    {
        Code = code;
        Name = name;
    }
    public string Code { get; private set; }

    public string Name { get; private set; }

}
