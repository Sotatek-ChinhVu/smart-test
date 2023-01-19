using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Family;
using UseCase.Family.GetListRaiinInf;

namespace EmrCloudApi.Presenters.Family;

public class GetListRaiinInfPresenter : IGetListRaiinInfOutputPort
{
    public Response<GetListRaiinInfResponse> Result { get; private set; } = new();

    public void Complete(GetListRaiinInfOutputData output)
    {
        Result.Data = new GetListRaiinInfResponse(output.ListRaiinInf.Select(item => new RaiinInfDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetListRaiinInfStatus status) => status switch
    {
        GetListRaiinInfStatus.Successed => ResponseMessage.Success,
        GetListRaiinInfStatus.InvalidPtId => ResponseMessage.PtInfNotFound,
        _ => string.Empty
    };
}
