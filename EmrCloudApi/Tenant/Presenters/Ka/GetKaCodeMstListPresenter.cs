using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.Ka;
using UseCase.KaMst.GetKaCodeList;

namespace EmrCloudApi.Tenant.Presenters.Ka;

public class GetKaCodeMstListPresenter : IgetKaCodeMstListOutputPort
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
        _ => string.Empty
    };
}
