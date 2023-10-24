using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Ka;
using UseCase.Ka.GetKacodeMstYossi;

namespace EmrCloudApi.Presenters.Ka;

public class GetKaCodeMstYossiPresenter : IGetKacodeMstYossiOutputPort
{
    public Response<GetKaCodeMstListResponse> Result { get; private set; } = new();

    public void Complete(GetKacodeMstYossiOutputData output)
    {
        Result.Data = new GetKaCodeMstListResponse(output.KaCodeMstModels);
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetKacodeMstYossiStatus status) => status switch
    {
        GetKacodeMstYossiStatus.Success => ResponseMessage.Success,
        GetKacodeMstYossiStatus.NoData => ResponseMessage.NoData,
        GetKacodeMstYossiStatus.Failed => ResponseMessage.Failed,
        _ => string.Empty
    };
}
