using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.LastDayInformation;
using EmrCloudApi.Responses.LastDayInformation.Dto;
using UseCase.LastDayInformation.GetLastDayInfoList;

namespace EmrCloudApi.Presenters.LastDayInformation;

public class GetLastDayInfoListPresenter : IGetLastDayInfoListOutputPort
{
    public Response<GetLastDayInfoListResponse> Result { get; private set; } = new();

    public void Complete(GetLastDayInfoListOutputData output)
    {
        Result.Data = new GetLastDayInfoListResponse(output.OdrDateInfList.Select(item => new OdrDateInfDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetLastDayInfoListStatus status) => status switch
    {
        GetLastDayInfoListStatus.Successed => ResponseMessage.Success,
        _ => throw new NotImplementedException(),
    };
}

