using Domain.Models.Reception;
using UseCase.Core.Sync.Core;
using UseCase.Diseases.Upsert;
using UseCase.Family;
using UseCase.FlowSheet.Upsert;
using UseCase.MedicalExamination.SaveKensaIrai;
using static Helper.Constants.KarteConst;
using static Helper.Constants.OrderInfConst;
using static Helper.Constants.RaiinInfConst;

namespace UseCase.MedicalExamination.SaveMedical;

public class SaveMedicalOutputData : IOutputData
{
    public SaveMedicalOutputData(
           SaveMedicalStatus status,
           RaiinInfTodayOdrValidationStatus validationRaiinInf,
           Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> validationOdrs,
           KarteValidationStatus validationKarte,
           ValidateFamilyListStatus validateFamily,
           UpsertFlowSheetStatus validationFlowSheetStatus,
           UpsertPtDiseaseListStatus validationPtDiseaseListStatus,
           int sinDate,
           long raiinNo,
           long ptId,
           List<ReceptionRowModel> receptionInfos,
           List<SameVisitModel> sameVisitList)
    {
        Status = status;
        ValidationRaiinInf = validationRaiinInf;
        ValidationOdrs = validationOdrs;
        ValidationKarte = validationKarte;
        ValidateFamily = validateFamily;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        PtId = ptId;
        ValidationFlowSheetStatus = validationFlowSheetStatus;
        ValidationPtDiseaseListStatus = validationPtDiseaseListStatus;
        ReceptionInfos = receptionInfos;
        SameVisitList = sameVisitList;
        KensaIraiOutputData = new SaveKensaIraiOutputData();
    }

    public SaveMedicalOutputData(
           SaveMedicalStatus status,
           RaiinInfTodayOdrValidationStatus validationRaiinInf,
           Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> validationOdrs,
           KarteValidationStatus validationKarte,
           ValidateFamilyListStatus validateFamily,
           UpsertFlowSheetStatus validationFlowSheetStatus,
           UpsertPtDiseaseListStatus validationPtDiseaseListStatus,
           int sinDate,
           long raiinNo,
           long ptId,
           List<ReceptionRowModel> receptionInfos,
           List<SameVisitModel> sameVisitList,
           SaveKensaIraiOutputData kensaIraiOutputData)
    {
        Status = status;
        ValidationRaiinInf = validationRaiinInf;
        ValidationOdrs = validationOdrs;
        ValidationKarte = validationKarte;
        ValidateFamily = validateFamily;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        PtId = ptId;
        ValidationFlowSheetStatus = validationFlowSheetStatus;
        ValidationPtDiseaseListStatus = validationPtDiseaseListStatus;
        ReceptionInfos = receptionInfos;
        SameVisitList = sameVisitList;
        KensaIraiOutputData = kensaIraiOutputData;
    }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public long PtId { get; private set; }

    public SaveMedicalStatus Status { get; private set; }

    public RaiinInfTodayOdrValidationStatus ValidationRaiinInf { get; private set; }

    public Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>> ValidationOdrs { get; private set; }

    public KarteValidationStatus ValidationKarte { get; private set; }

    public ValidateFamilyListStatus ValidateFamily { get; private set; }

    public UpsertFlowSheetStatus ValidationFlowSheetStatus { get; private set; }

    public UpsertPtDiseaseListStatus ValidationPtDiseaseListStatus { get; private set; }

    public List<ReceptionRowModel> ReceptionInfos { get; private set; }

    public List<SameVisitModel> SameVisitList { get; private set; }

    public SaveKensaIraiOutputData KensaIraiOutputData { get; private set; }
}
