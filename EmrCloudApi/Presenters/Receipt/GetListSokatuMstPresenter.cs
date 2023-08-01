using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetListSokatuMst;

namespace EmrCloudApi.Presenters.Receipt;

public class GetListSokatuMstPresenter : IGetListSokatuMstOutputPort
{
    public Response<GetListSokatuMstResponse> Result { get; private set; } = new Response<GetListSokatuMstResponse>();

    public void Complete(GetListSokatuMstOutputData output)
    {
        Result.Data = new GetListSokatuMstResponse(output.SokatuMstModels);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetListSokatuMstStatus status) => status switch
    {
        GetListSokatuMstStatus.Success => ResponseMessage.Success,
        GetListSokatuMstStatus.InvalidSeikyuYm => ResponseMessage.InvalidSeikyuYm,
        _ => string.Empty
    };
}
