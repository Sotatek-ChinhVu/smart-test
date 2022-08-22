using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.RaiinFilter;
using UseCase.RaiinFilterMst.SaveList;

namespace EmrCloudApi.Tenant.Presenters.RaiinFilter;

public class SaveRaiinFilterMstListPresenter : ISaveRaiinFilterMstListOutputPort
{
    public Response<SaveRaiinFilterMstListResponse> Result { get; private set; } = new Response<SaveRaiinFilterMstListResponse>();

    public void Complete(SaveRaiinFilterMstListOutputData output)
    {
        Result.Data = new SaveRaiinFilterMstListResponse(output.Status == SaveRaiinFilterMstListStatus.Success);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(SaveRaiinFilterMstListStatus status) => status switch
    {
        SaveRaiinFilterMstListStatus.Success => ResponseMessage.Success,
        _ => string.Empty
    };
}
