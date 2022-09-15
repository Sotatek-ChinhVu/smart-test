namespace UseCase.SupperSetDetail.SaveSuperSetDetail.SaveSetByomeiInput;

public class PrefixSuffixInputItem
{
    public PrefixSuffixInputItem(string code, string name)
    {
        Code = code;
        Name = name;
    }

    public string Code { get; private set; }

    public string Name { get; private set; }
}
