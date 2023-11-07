using EmrCloudApi.Responses.DrugInfor.Dto;

namespace EmrCloudApi.Responses.DrugInfor;

public class GetContentDrugUsageHistoryResponse
{
    public GetContentDrugUsageHistoryResponse(List<DrugUsageHistoryContentDto> drugUsageHistory)
    {
        DrugUsageHistory = drugUsageHistory;
    }

    public List<DrugUsageHistoryContentDto> DrugUsageHistory { get; private set; }
}
