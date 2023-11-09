using EmrCloudApi.Constants;
using EmrCloudApi.Responses.DrugInfor.Dto;
using EmrCloudApi.Responses.DrugInfor;
using EmrCloudApi.Responses;
using UseCase.DrugInfor.GetContentDrugUsageHistory;

namespace EmrCloudApi.Presenters.DrugInfor;

public class GetContentDrugUsageHistoryPresenter : IGetContentDrugUsageHistoryOutputPort
{
    public Response<GetContentDrugUsageHistoryResponse> Result { get; private set; } = new();

    public void Complete(GetContentDrugUsageHistoryOutputData output)
    {
        Result.Data = new GetContentDrugUsageHistoryResponse(output.DrugUsageHistory.Select(item => new DrugUsageHistoryGroupDto(item)).ToList());
        Result.Message = GetMessage(output.Status);
        Result.Status = (int)output.Status;
    }

    private string GetMessage(GetContentDrugUsageHistoryStatus status) => status switch
    {
        GetContentDrugUsageHistoryStatus.Successed => ResponseMessage.Success,
        _ => string.Empty
    };
}
