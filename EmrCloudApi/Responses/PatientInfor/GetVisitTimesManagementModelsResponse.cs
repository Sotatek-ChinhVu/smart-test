using EmrCloudApi.Responses.PatientInfor.Dto;

namespace EmrCloudApi.Responses.PatientInfor;

public class GetVisitTimesManagementModelsResponse
{
    public GetVisitTimesManagementModelsResponse(List<VisitTimesManagementDto> visitTimesManagementList)
    {
        VisitTimesManagementList = visitTimesManagementList;
    }

    public List<VisitTimesManagementDto> VisitTimesManagementList { get; private set; }
}
