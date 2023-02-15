using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetListSyobyoKeika;

public class GetListSyobyoKeikaOutputData : IOutputData
{
    public GetListSyobyoKeikaOutputData(List<SyobyoKeikaModel> listSyobyoKeika, GetListSyobyoKeikaStatus status)
    {
        ListSyobyoKeika = listSyobyoKeika.Select(item => new SyobyoKeikaItem(item)).ToList();
        Status = status;
    }

    public List<SyobyoKeikaItem> ListSyobyoKeika { get; private set; }

    public GetListSyobyoKeikaStatus Status { get; private set; }
}
