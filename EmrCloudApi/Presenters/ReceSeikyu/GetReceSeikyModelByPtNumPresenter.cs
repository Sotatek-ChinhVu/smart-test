using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.ReceSeikyu;
using UseCase.ReceSeikyu.GetReceSeikyModelByPtNum;

namespace EmrCloudApi.Presenters.ReceSeikyu;

public class GetReceSeikyModelByPtNumPresenter : IGetReceSeikyModelByPtNumOutputPort
{
    public Response<GetReceSeikyModelByPtNumResponse> Result { get; private set; } = new();

    public void Complete(GetReceSeikyModelByPtNumOutputData output)
    {
        Result.Data = new GetReceSeikyModelByPtNumResponse(output.ReceSeikyuModel);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetReceSeikyModelByPtNumStatus status) => status switch
    {
        GetReceSeikyModelByPtNumStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
