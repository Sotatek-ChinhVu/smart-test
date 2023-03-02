using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientInfor;
using UseCase.PatientInfor.GetPatientInfoBetweenTimesList;

namespace EmrCloudApi.Presenters.PatientInfor;

public class GetPatientInfoBetweenTimesListPresenter : IGetPatientInfoBetweenTimesListOutputPort
{
    public Response<GetPatientInfoBetweenTimesListResponse> Result { get; private set; } = new();

    public void Complete(GetPatientInfoBetweenTimesListOutputData outputData)
    {
        Result.Data = new GetPatientInfoBetweenTimesListResponse(outputData.PatientInfoList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetPatientInfoBetweenTimesListStatus status) => status switch
    {
        GetPatientInfoBetweenTimesListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}