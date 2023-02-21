using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetListSyoukiInf;

namespace EmrCloudApi.Presenters.Receipt;

public class GetSyoukiInfListPresenter : IGetSyoukiInfListOutputPort
{
    public Response<GetSyoukiInfListResponse> Result { get; private set; } = new();

    public void Complete(GetSyoukiInfListOutputData output)
    {
        Result.Data = new GetSyoukiInfListResponse(output.SyoukiInfList, output.SyoukiKbnMstList);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetSyoukiInfListStatus status) => status switch
    {
        GetSyoukiInfListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
