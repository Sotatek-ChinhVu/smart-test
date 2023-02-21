using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt;
using UseCase.Receipt.GetListSyobyoKeika;

namespace EmrCloudApi.Presenters.Receipt;

public class GetSyobyoKeikaListPresenter : IGetSyobyoKeikaListOutputPort
{
    public Response<GetSyobyoKeikaListResponse> Result { get; private set; } = new();

    public void Complete(GetSyobyoKeikaListOutputData outputData)
    {
        Result.Data = new GetSyobyoKeikaListResponse(outputData.SyobyoKeikaList);
        Result.Message = GetMessage(outputData.Status);
        Result.Status = (int)outputData.Status;
    }

    private string GetMessage(GetSyobyoKeikaListStatus status) => status switch
    {
        GetSyobyoKeikaListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
