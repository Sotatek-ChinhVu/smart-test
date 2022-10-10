using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Ka;
using UseCase.Ka.GetKaCodeList;

namespace EmrCloudApi.Tenant.Presenters.Ka;

public class GetKaCodeMstListPresenter : IGetKaCodeMstListOutputPort
{
    public Response<GetKaCodeMstListResponse> Result { get; private set; } = new();

    public void Complete(GetKaCodeMstListOutputData output)
    {
        Result.Data = new GetKaCodeMstListResponse(output.KaCodeMstModels);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetKaCodeMstListStatus status) => status switch
    {
        GetKaCodeMstListStatus.Success => ResponseMessage.Success,
        GetKaCodeMstListStatus.NoData => ResponseMessage.NoData,
        GetKaCodeMstListStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
