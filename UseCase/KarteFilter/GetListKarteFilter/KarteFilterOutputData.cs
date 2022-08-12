using UseCase.Core.Sync.Core;

namespace UseCase.KarteFilter.GetListKarteFilter;

public class KarteFilterOutputData : IOutputData
{
    public List<KarteFilterMstModelOutputItem> ListData { get; private set; }

    public KarteFilterStatus Status { get; private set; }

    public KarteFilterOutputData(List<KarteFilterMstModelOutputItem> listData, KarteFilterStatus status)
    {
        ListData = listData;
        Status = status;
    }
}
