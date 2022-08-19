using Domain.Models.KarteFilterMst;
using UseCase.Core.Sync.Core;

namespace UseCase.KarteFilter.GetListKarteFilter;

public class GetKarteFilterOutputData : IOutputData
{
    public List<KarteFilterMstModel> ListData { get; private set; }

    public GetKarteFilterStatus Status { get; private set; }

    public GetKarteFilterOutputData(List<KarteFilterMstModel> listData, GetKarteFilterStatus status)
    {
        ListData = listData;
        Status = status;
    }
}
