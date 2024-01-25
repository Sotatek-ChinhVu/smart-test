using Domain.Models.Yousiki;
using UseCase.Yousiki.CommonOutputData.CommonOutputModel;

namespace UseCase.Yousiki.CommonOutputData;

public class LivingHabitModel
{
    public LivingHabitModel(List<Yousiki1InfDetailModel> yousiki1InfDetailList, List<OutpatientConsultationInfModel> outpatientConsultationInfList, List<StrokeHistoryModel> strokeHistoryList, List<AcuteCoronaryHistoryModel> acuteCoronaryHistoryList, List<AcuteAorticDissectionHistoryModel> acuteAorticHistoryList)
    {
        Yousiki1InfDetailList = yousiki1InfDetailList;
        OutpatientConsultationInfList = outpatientConsultationInfList;
        StrokeHistoryList = strokeHistoryList;
        AcuteCoronaryHistoryList = acuteCoronaryHistoryList;
        AcuteAorticHistoryList = acuteAorticHistoryList;
    }

    public List<Yousiki1InfDetailModel> Yousiki1InfDetailList { get; private set; }

    public List<OutpatientConsultationInfModel> OutpatientConsultationInfList { get; private set; }

    public List<StrokeHistoryModel> StrokeHistoryList { get; private set; }

    public List<AcuteCoronaryHistoryModel> AcuteCoronaryHistoryList { get; private set; }

    public List<AcuteAorticDissectionHistoryModel> AcuteAorticHistoryList { get; private set; }
}
