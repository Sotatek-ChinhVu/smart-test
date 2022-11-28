using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.UketukeSbt;
using UseCase.UketukeSbtMst.GetNext;

namespace EmrCloudApi.Presenters.UketukeSbt;

public class GetNextUketukeSbtMstPresenter : IGetNextUketukeSbtMstOutputPort
{
    public Response<GetNextUketukeSbtMstResponse> Result { get; private set; } = new Response<GetNextUketukeSbtMstResponse>();
    
    public void Complete(GetNextUketukeSbtMstOutputData output)
    {
        Result.Data = new GetNextUketukeSbtMstResponse(output.ReceptionType);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetNextUketukeSbtMstStatus status) => status switch
    {
        GetNextUketukeSbtMstStatus.Success => ResponseMessage.Success,
        GetNextUketukeSbtMstStatus.NotFound => ResponseMessage.NotFound,
        _ => string.Empty
    };
}
