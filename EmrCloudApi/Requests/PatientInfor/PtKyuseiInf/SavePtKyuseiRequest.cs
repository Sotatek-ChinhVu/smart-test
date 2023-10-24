namespace EmrCloudApi.Requests.PatientInfor.PtKyuseiInf;

public class SavePtKyuseiRequest
{
    public long PtId { get; set; }

    public List<SavePtKyuseiRequestItem> PtKyuseiList { get; set; } = new();
}
