namespace EmrCloudApi.Requests.MainMenu;

public class GetKensaIraiRequest
{
    public long PtId { get; set; }

    public int StartDate { get; set; }

    public int EndDate { get; set; }

    public string KensaCenterMstCenterCd { get; set; } = string.Empty;

    public int KensaCenterMstPrimaryKbn { get; set; }
}
