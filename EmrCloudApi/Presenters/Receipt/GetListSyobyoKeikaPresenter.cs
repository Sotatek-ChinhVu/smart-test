using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetListSyobyoKeika;

namespace EmrCloudApi.Presenters.Receipt;

public class GetListSyobyoKeikaPresenter : IGetListSyobyoKeikaOutputPort
{
    public Response<GetListSyobyoKeikaResponse> Result { get; private set; } = new();

    public void Complete(GetListSyobyoKeikaOutputData outputData)
    {
        Result.Data = new GetListSyobyoKeikaResponse(outputData.SyobyoKeikaList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetListSyobyoKeikaStatus status) => status switch
    {
        GetListSyobyoKeikaStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
