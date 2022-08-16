using UseCase.Core.Sync.Core;

namespace UseCase.KarteFilter.GetListKarteFilter;

public class GetKarteFilterOutputData : IOutputData
{
    public List<GetKarteFilterMstModelOutputItem> ListData { get; private set; }

    public GetKarteFilterStatus Status { get; private set; }

    public GetKarteFilterOutputData(List<GetKarteFilterMstModelOutputItem> listData, GetKarteFilterStatus status)
    {
        ListData = listData;
        Status = status;
    }
}
