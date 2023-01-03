using Domain.Models.KarteFilterMst;
using UseCase.Core.Sync.Core;

namespace UseCase.KarteFilter.GetListKarteFilter;

public class GetKarteFilterOutputData : IOutputData
{
    public List<KarteFilterMstOutputItem> ListData { get; private set; }

    public GetKarteFilterStatus Status { get; private set; }

    public GetKarteFilterOutputData(List<KarteFilterMstOutputItem> listData, GetKarteFilterStatus status)
    {
        ListData = listData;
        Status = status;
    }

    public GetKarteFilterOutputData(GetKarteFilterStatus status)
    {
        ListData = new();
        Status = status;
    }
}
