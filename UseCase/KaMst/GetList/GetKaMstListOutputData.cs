using Domain.Models.KaMst;
using UseCase.Core.Sync.Core;

namespace UseCase.KaMst.GetList;

public class GetKaMstListOutputData : IOutputData
{
    public GetKaMstListOutputData(GetKaMstListStatus status, List<KaMstModel> departments)
    {
        Status = status;
        Departments = departments;
    }

    public GetKaMstListStatus Status { get; private set; }
    public List<KaMstModel> Departments { get; private set; }
}
