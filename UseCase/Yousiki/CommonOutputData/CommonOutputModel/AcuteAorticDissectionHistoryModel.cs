using Domain.Models.Yousiki;

namespace UseCase.Yousiki.CommonOutputData.CommonOutputModel;

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
