using Domain.Models.SuperSetDetail;
using UseCase.Core.Sync.Core;

namespace UseCase.MainMenu.GetOdrSetName;

public class GetOdrSetNameOutputData : IOutputData
{
    public GetOdrSetNameOutputData(List<OdrSetNameModel> odrSetNameList, GetOdrSetNameStatus status)
    {
        OdrSetNameList = odrSetNameList;
        Status = status;
    }

    public List<OdrSetNameModel> OdrSetNameList { get; private set; }

    public GetOdrSetNameStatus Status { get; private set; }
}
