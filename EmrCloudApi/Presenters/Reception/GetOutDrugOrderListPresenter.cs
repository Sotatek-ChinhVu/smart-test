using EmrCloudApi.Constants;
using EmrCloudApi.Responses;
using EmrCloudApi.Responses.Receipt.Dto;
using EmrCloudApi.Responses.Reception;
using UseCase.Reception.GetOutDrugOrderList;

namespace EmrCloudApi.Presenters.Reception;

public class GetOutDrugOrderListPresenter : IGetOutDrugOrderListOutputPort
{
    public Response<GetOutDrugOrderListResponse> Result { get; private set; } = new();

    public void Complete(GetOutDrugOrderListOutputData output)
    {
        Result.Data = new GetOutDrugOrderListResponse(output.RaiinInfToPrintList.Select(item => new RaiinInfToPrintDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetOutDrugOrderListStatus status) => status switch
    {
        GetOutDrugOrderListStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}

