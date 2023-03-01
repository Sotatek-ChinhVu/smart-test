using Domain.Models.SystemConf;
using UseCase.Core.Sync.Core;

namespace UseCase.SystemConf.GetSystemConfList;

public class GetSystemConfListOutputData : IOutputData
{
    public GetSystemConfListOutputData(List<SystemConfModel> systemConfList, GetSystemConfListStatus status)
    {
        SystemConfList = systemConfList;
        Status = status;
    }

    public List<SystemConfModel> SystemConfList { get; private set; }

    public GetSystemConfListStatus Status { get; private set; }
}
