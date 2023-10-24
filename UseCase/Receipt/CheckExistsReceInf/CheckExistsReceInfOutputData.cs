using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.CheckExistsReceInf;

public class CheckExistsReceInfOutputData : IOutputData
{
    public CheckExistsReceInfOutputData(bool isExisted, CheckExistsReceInfStatus status)
    {
        Status = status;
        IsExisted = isExisted;
    }

    public bool IsExisted { get; private set; }

    public CheckExistsReceInfStatus Status { get; private set; }
}
