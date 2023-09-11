using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Ka;
using UseCase.Ka.GetKacodeYousikiMst;

namespace EmrCloudApi.Presenters.Ka;

public class GetKaCodeYousikiMstPresenter : IGetKaCodeYousikiMstOutputPort
{
    public Response<GetKaCodeYousikiMstResponse> Result { get; private set; } = new();

    public void Complete(GetKaCodeYousikiMstOutputData output)
    {
        Result.Data = new GetKaCodeYousikiMstResponse(output.KaCodeMstModels);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetKaCodeYousikiMstStatus status) => status switch
    {
        GetKaCodeYousikiMstStatus.Success => ResponseMessage.Success,
        GetKaCodeYousikiMstStatus.NoData => ResponseMessage.NoData,
        GetKaCodeYousikiMstStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
