using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.PatientInfor;
using UseCase.PatientInfor.GetTokiMstList;

namespace EmrCloudApi.Presenters.PatientInfor;

public class GetTokkiMstListPresenter : IGetTokkiMstListOutputPort
{
    public Response<GetTokkiMstListResponse> Result { get; private set; } = new();

    public void Complete(GetTokkiMstListOutputData outputData)
    {
        Result.Data = new GetTokkiMstListResponse(outputData.TokkiMstList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetTokkiMstListStatus status) => status switch
    {
        GetTokkiMstListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
