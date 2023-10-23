using CommonChecker.Models;
using CommonChecker.Models.OrdInf;
using CommonCheckers.OrderRealtimeChecker.Models;
using Domain.Common;
using Domain.Models.Diseases;
using UseCase.Family;
using UseCase.MedicalExamination.SaveMedical;

namespace Interactor.CommonChecker.CommonMedicalCheck;

public interface ICommonMedicalCheck
{
    List<UnitCheckInfoModel> CheckListOrder(int hpId, long ptId, int sinday, List<OrdInfoModel> currentListOdr, List<OrdInfoModel> listCheckingOrder, SpecialNoteItem specialNoteItem, List<PtDiseaseModel> ptDiseaseModels, List<FamilyItem> familyItems, bool isDataOfDb, RealTimeCheckerCondition realTimeCheckerCondition);

    List<UnitCheckInfoModel> CheckListOrder(int hpId, long ptId, int sinday, List<OrdInfoModel> listCheckingOrder, RealTimeCheckerCondition checkerCondition, SpecialNoteItem specialNoteItem, List<PtDiseaseModel> ptDiseaseModels, List<FamilyItem> familyItems, bool isDataOfDb);

    List<ErrorInfoModel> GetErrorDetails(int hpId, long ptId, int sinday, List<UnitCheckInfoModel> listErrorInfo);

    List<DayLimitResultModel> CheckOnlyDayLimit(OrdInfoModel checkingOrder);

    void ReleaseResource();
}
