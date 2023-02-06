namespace UseCase.Receipt;

public class SearchByoMstInputItem
{
    public SearchByoMstInputItem(string byomeiCd, string inputName, bool isComment)
    {
        ByomeiCd = byomeiCd;
        InputName = inputName;
        IsComment = isComment;
    }

    public string ByomeiCd { get; private set; }

    public string InputName { get; private set; }

    public bool IsComment { get; private set; }
}
