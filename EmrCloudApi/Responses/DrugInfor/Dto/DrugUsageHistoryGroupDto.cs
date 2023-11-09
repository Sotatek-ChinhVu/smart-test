using UseCase.DrugInfor.Model;

namespace EmrCloudApi.Responses.DrugInfor.Dto;

public class DrugUsageHistoryGroupDto
{
    public DrugUsageHistoryGroupDto(DrugUsageHistoryGroupModel model)
    {
        KouiKbnId = model.KouiKbnId;
        KouiName = model.KouiName;
        DrugUsageHistoryContentList = model.DrugUsageHistoryContentList.Select(item => new DrugUsageHistoryContentDto(item)).ToList();
    }

    public int KouiKbnId { get; private set; }

    public string KouiName { get; private set; }

    public List<DrugUsageHistoryContentDto> DrugUsageHistoryContentList { get; private set; }

}
