using Domain.Models.Yousiki;

namespace Domain.Models.Yousiki.CommonModel.CommonOutputModel;

public class AcuteAorticDissectionHistoryModel
{
    public AcuteAorticDissectionHistoryModel(Yousiki1InfDetailModel onsetDate, bool isDeleted)
    {
        OnsetDate = onsetDate;
        IsDeleted = isDeleted;
    }

    public AcuteAorticDissectionHistoryModel(Yousiki1InfDetailModel onsetDate)
    {
        OnsetDate = onsetDate;
        IsDeleted = false;
    }

    public Yousiki1InfDetailModel OnsetDate { get; private set; }

    public bool IsDeleted { get; private set; }
}
