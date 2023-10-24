using Domain.Models.Insurance;
using UseCase.Core.Sync.Core;

namespace UseCase.Insurance.FindHokenInfByPtId;

public class FindHokenInfByPtIdOutputData : IOutputData
{
    public FindHokenInfByPtIdOutputData(List<HokenInfModel> hokenInfList, FindHokenInfByPtIdStatus status)
    {
        HokenInfList = hokenInfList;
        Status = status;
    }

    public List<HokenInfModel> HokenInfList { get; private set; }

    public FindHokenInfByPtIdStatus Status { get; private set; }
}
