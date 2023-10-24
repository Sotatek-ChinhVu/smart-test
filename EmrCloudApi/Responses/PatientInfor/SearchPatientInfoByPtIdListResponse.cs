namespace EmrCloudApi.Responses.PatientInfor;

public class SearchPatientInfoByPtIdListResponse
{
    public SearchPatientInfoByPtIdListResponse(List<PatientInfoDto> ptInfList)
    {
        PtInfList = ptInfList;
    }

    public List<PatientInfoDto> PtInfList { get; private set; }
}
