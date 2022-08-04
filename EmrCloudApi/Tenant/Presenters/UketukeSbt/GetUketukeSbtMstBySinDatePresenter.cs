using EmrCloudApi.Tenant.Constants;
using EmrCloudApi.Tenant.Responses;
using EmrCloudApi.Tenant.Responses.UketukeSbt;
using UseCase.UketukeSbtMst.GetBySinDate;

namespace EmrCloudApi.Tenant.Presenters.UketukeSbt;

public class GetUketukeSbtMstBySinDatePresenter : IGetUketukeSbtMstBySinDateOutputPort
{
    public Response<GetUketukeSbtMstBySinDateResponse> Result { get; private set; } = new Response<GetUketukeSbtMstBySinDateResponse>();
    
    public void Complete(GetUketukeSbtMstBySinDateOutputData output)
    {
        Result.Data = new GetUketukeSbtMstBySinDateResponse(output.ReceptionType);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetUketukeSbtMstBySinDateStatus status) => status switch
    {
        GetUketukeSbtMstBySinDateStatus.Success => ResponseMessage.Success,
        GetUketukeSbtMstBySinDateStatus.NotFound => ResponseMessage.NotFound,
        GetUketukeSbtMstBySinDateStatus.InvalidSinDate => ResponseMessage.InvalidSinDate,
        _ => string.Empty
    };
}
