using EmrCloudApi.Responses.DrugInfor.Dto;

namespace EmrCloudApi.Responses.DrugInfor;

public class GetContentDrugUsageHistoryResponse
{
    public GetContentDrugUsageHistoryResponse(List<DrugUsageHistoryGroupDto> drugUsageHistory)
    {
        DrugUsageHistory = drugUsageHistory;
    }

    public List<DrugUsageHistoryGroupDto> DrugUsageHistory { get; private set; }
}
