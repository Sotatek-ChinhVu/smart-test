using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.UketukeSbt;
using UseCase.UketukeSbtMst.GetList;

namespace EmrCloudApi.Tenant.Presenters.UketukeSbt;

public class GetUketukeSbtMstListPresenter : IGetUketukeSbtMstListOutputPort
{
    public Response<GetUketukeSbtMstListResponse> Result { get; private set; } = new Response<GetUketukeSbtMstListResponse>();
    
    public void Complete(GetUketukeSbtMstListOutputData output)
    {
        Result.Data = new GetUketukeSbtMstListResponse(output.ReceptionTypes);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetUketukeSbtMstListStatus status) => status switch
    {
        GetUketukeSbtMstListStatus.Success => ResponseMessage.Success,
        GetUketukeSbtMstListStatus.NoData => ResponseMessage.NoData,
        _ => string.Empty
    };
}
