using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetRecePreviewList;

public class GetRecePreviewListOutputData : IOutputData
{
    public GetRecePreviewListOutputData(List<ReceInfModel> receInfList, GetRecePreviewListStatus status)
    {
        ReceInfList = receInfList.Select(item => new ReceInfItem(item)).ToList();
        Status = status;
    }

    public List<ReceInfItem> ReceInfList { get; private set; }

    public GetRecePreviewListStatus Status { get; private set; }
}
