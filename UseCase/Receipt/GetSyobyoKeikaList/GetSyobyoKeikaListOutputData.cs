using Domain.Models.Receipt;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetListSyobyoKeika;

public class GetSyobyoKeikaListOutputData : IOutputData
{
    public GetSyobyoKeikaListOutputData(List<SyobyoKeikaModel> syobyoKeikaList, GetSyobyoKeikaListStatus status)
    {
        SyobyoKeikaList = syobyoKeikaList.Select(item => new SyobyoKeikaItem(item)).ToList();
        Status = status;
    }

    public List<SyobyoKeikaItem> SyobyoKeikaList { get; private set; }

    public GetSyobyoKeikaListStatus Status { get; private set; }
}
