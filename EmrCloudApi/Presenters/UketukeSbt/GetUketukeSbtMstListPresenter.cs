using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.UketukeSbt;
using UseCase.UketukeSbtMst.GetList;

namespace EmrCloudApi.Presenters.UketukeSbt;

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
