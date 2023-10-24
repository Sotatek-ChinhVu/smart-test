using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetTenMstList;

namespace EmrCloudApi.Presenters.MstItem;

public class GetTenMstListPresenter : IGetTenMstListOutputPort
{
    public Response<GetTenMstListResponse> Result { get; private set; } = new();

    public void Complete(GetTenMstListOutputData output)
    {
        Result.Data = new GetTenMstListResponse(output.TenMstList.Select(item => new TenItemDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetTenMstListStatus status) => status switch
    {
        GetTenMstListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

