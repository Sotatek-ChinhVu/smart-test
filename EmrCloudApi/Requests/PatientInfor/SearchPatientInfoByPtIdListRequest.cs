namespace EmrCloudApi.Requests.PatientInfor;

public class SearchPatientInfoByPtIdListRequest
{
    public List<long> PtIdList { get; set; } = new();
}
