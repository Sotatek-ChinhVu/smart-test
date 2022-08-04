using Domain.Models.UketukeSbtMst;
using UseCase.Core.Sync.Core;

namespace UseCase.UketukeSbtDayInf.Upsert;

public class UpsertUketukeSbtDayInfOutputData : IOutputData
{
    public UpsertUketukeSbtDayInfOutputData(UpsertUketukeSbtDayInfStatus status)
    {
        Status = status;
    }

    public UpsertUketukeSbtDayInfStatus Status { get; private set; }
}
