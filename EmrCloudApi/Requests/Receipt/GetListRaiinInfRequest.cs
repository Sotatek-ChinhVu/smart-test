namespace EmrCloudApi.Requests.Receipt;

public class GetListRaiinInfRequest
{
    public long PtId { get; set; }

    public int SinYm { get; set; }

    public int DayInMonth { get; set; }

    public int RpNo { get; set; }

    public int SeqNo { get; set; }
}
