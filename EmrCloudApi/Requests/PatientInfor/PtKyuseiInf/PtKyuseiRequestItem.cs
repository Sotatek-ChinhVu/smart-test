namespace EmrCloudApi.Requests.PatientInfor.PtKyuseiInf;

public class PtKyuseiRequestItem
{
    public int HpId { get;  set; }

    public long PtId { get;  set; }

    public long SeqNo { get;  set; }

    public string KanaName { get; set; } = string.Empty;

    public string Name { get;  set; } = string.Empty;

    public int EndDate { get;  set; }
}
