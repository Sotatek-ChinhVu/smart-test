using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.MstItem;
using UseCase.MstItem.GetTeikyoByomei;

namespace EmrCloudApi.Presenters.MstItem;

public class GetTeikyoByomeiPresenter : IGetTeikyoByomeiOutputPort
{
    public Response<GetTeikyoByomeiResponse> Result { get; private set; } = default!;

    public void Complete(GetTeikyoByomeiOutputData outputData)
    {
        Result = new Response<GetTeikyoByomeiResponse>()
        {
            Data = new GetTeikyoByomeiResponse(outputData.TeikyoByomeis),
            Status = (int)outputData.Status
        };
        switch (outputData.Status)
        {
            case GetTeikyoByomeiStatus.Successful:
                Result.Message = ResponseMessage.Success;
                break;
        }
    }
}
