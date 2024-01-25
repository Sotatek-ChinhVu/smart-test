using Domain.Models.Yousiki;

namespace UseCase.Yousiki.CommonOutputData.CommonOutputModel;

public class AcuteCoronaryHistoryModel
{
    public AcuteCoronaryHistoryModel(Yousiki1InfDetailModel type, Yousiki1InfDetailModel onsetDate, bool isDeleted)
    {
        Type = type;
        OnsetDate = onsetDate;
        IsDeleted = isDeleted;
    }

    public AcuteCoronaryHistoryModel(Yousiki1InfDetailModel type, Yousiki1InfDetailModel onsetDate)
    {
        Type = type;
        OnsetDate = onsetDate;
        IsDeleted = false;
    }

    public AcuteCoronaryHistoryModel UpdateSortNo(int sorNo)
    {
        SortNo = sorNo;
        return this;
    }

    public Yousiki1InfDetailModel Type { get; private set; }

    public Yousiki1InfDetailModel OnsetDate { get; private set; }

    public int SortNo { get; private set; }

    public bool IsDeleted { get; private set; }

    public bool IsEnableOnsetDate { get => Type.Value == "1" || Type.Value == "2"; }
}
