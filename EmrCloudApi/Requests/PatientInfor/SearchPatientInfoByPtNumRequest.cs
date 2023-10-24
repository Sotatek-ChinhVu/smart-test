namespace EmrCloudApi.Requests.PatientInfor;

public class SearchPatientInfoByPtNumRequest
{
    public long PtNum { get; set; }

    public int SinDate { get; set; } = 0;
}
