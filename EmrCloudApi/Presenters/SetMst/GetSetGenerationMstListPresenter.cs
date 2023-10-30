using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.SetMst;
using EmrCloudApi.Responses.SetMst.Dto;
using UseCase.SetMst.GetListSetGenerationMst;

namespace EmrCloudApi.Presenters.SetMst;

public class GetSetGenerationMstListPresenter : IGetSetGenerationMstListOutputPort
{
    public Response<GetSetGenerationMstListResponse> Result { get; private set; } = new();

    public void Complete(GetSetGenerationMstListOutputData output)
    {
        Result.Data = new GetSetGenerationMstListResponse(output.SetGenerationList.Select(item => new SetGenerationDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetSetGenerationMstListStatus status) => status switch
    {
        GetSetGenerationMstListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
