using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetMst;
using EmrCloudApi.Responses.SetMst.Dto;
using UseCase.MainMenu.GetOdrSetName;

namespace EmrCloudApi.Presenters.SetMst;

public class GetOdrSetNamePresenter : IGetOdrSetNameOutputPort
{
    public Response<GetOdrSetNameResponse> Result { get; private set; } = new();

    public void Complete(GetOdrSetNameOutputData output)
    {
        Result.Data = new GetOdrSetNameResponse(output.OdrSetNameList.Select(item => new OdrSetNameDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetOdrSetNameStatus status) => status switch
    {
        GetOdrSetNameStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
