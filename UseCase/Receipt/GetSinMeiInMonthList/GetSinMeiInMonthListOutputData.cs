using Domain.Models.Accounting;
using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.GetSinMeiInMonthList;

public class GetSinMeiInMonthListOutputData : IOutputData
{
    public GetSinMeiInMonthListOutputData(List<SinMeiModel> sinMeiModels, Dictionary<int, string> holidays, GetSinMeiInMonthListStatus status)
    {
        SinMeiModels = sinMeiModels;
        Holidays = holidays;
        Status = status;
    }

    public List<SinMeiModel> SinMeiModels { get; private set; }
    public Dictionary<int, string> Holidays { get; private set; }
    public GetSinMeiInMonthListStatus Status { get; private set; }
}
