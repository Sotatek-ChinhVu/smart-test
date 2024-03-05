using Domain.Models.Yousiki;

namespace Domain.Models.Yousiki.CommonModel.CommonOutputModel;

public class StrokeHistoryModel
{
    public StrokeHistoryModel(Yousiki1InfDetailModel type, Yousiki1InfDetailModel onsetDate, bool isDeleted)
    {
        Type = type;
        OnsetDate = onsetDate;
        IsDeleted = isDeleted;
    }

    public StrokeHistoryModel(Yousiki1InfDetailModel type, Yousiki1InfDetailModel onsetDate)
    {
        Type = type;
        OnsetDate = onsetDate;
        IsDeleted = false;
    }

    public StrokeHistoryModel UpdateSortNo(int sortNo)
    {
        SortNo = sortNo;
        return this;
    }

    public Yousiki1InfDetailModel Type { get; private set; }

    public Yousiki1InfDetailModel OnsetDate { get; private set; }

    public bool IsDeleted { get; private set; }

    public int SortNo { get; private set; }

    public bool IsEnableOnsetDate { get => Type.Value == "1" || Type.Value == "2" || Type.Value == "3" || Type.Value == "4"; }
}
