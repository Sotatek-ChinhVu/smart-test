namespace EmrCloudApi.Responses.SetMst;

public class CopyPasteSetMstResponse
{
    public CopyPasteSetMstResponse(int newSetCd)
    {
        NewSetCd = newSetCd;
    }

    public int NewSetCd { get; private set; }
}
