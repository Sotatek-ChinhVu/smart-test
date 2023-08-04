namespace EmrCloudApi.Requests.PatientInfor.PtKyuseiInf;

public class SavePtKyuseiRequestItem
{
    public long SeqNo { get; set; }

    public string KanaName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int EndDate { get; set; }

    public bool IsDeleted { get; set; }
}
