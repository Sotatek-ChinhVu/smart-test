using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetListSyoukiInf;

namespace EmrCloudApi.Presenters.Receipt;

public class GetListSyoukiInfPresenter : IGetListSyoukiInfOutputPort
{
    public Response<GetListSyoukiInfResponse> Result { get; private set; } = new();

    public void Complete(GetListSyoukiInfOutputData output)
    {
        Result.Data = new GetListSyoukiInfResponse(output.ListSyoukiInf, output.ListSyoukiKbnMst);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetListSyoukiInfStatus status) => status switch
    {
        GetListSyoukiInfStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
