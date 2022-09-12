using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.SetMst;
using UseCase.SupperSetDetail.GetSetByomeiList;

namespace EmrCloudApi.Tenant.Presenters.SetMst;

public class GetSetByomeiListPresenter : IGetSetByomeiListOutputPort
{
    public Response<GetSetByomeiListResponse> Result { get; private set; } = new();

    public void Complete(GetSetByomeiListOutputData output)
    {
        Result.Data = new GetSetByomeiListResponse(output.ListSetByomeiModel);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetSetByomeiListStatus status) => status switch
    {
        GetSetByomeiListStatus.Successed => ResponseMessage.Success,
        GetSetByomeiListStatus.Failed => ResponseMessage.Failed,
        GetSetByomeiListStatus.InvalidHpId => ResponseMessage.InvalidHpId,
        GetSetByomeiListStatus.InvalidSetCd => ResponseMessage.InvalidSetCd,
        _ => string.Empty
    };
}
