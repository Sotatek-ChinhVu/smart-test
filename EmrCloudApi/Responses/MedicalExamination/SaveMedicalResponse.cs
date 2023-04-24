using EmrCloudApi.Responses.Diseases;
using EmrCloudApi.Responses.FlowSheet;
using EmrCloudApi.Responses.KarteInf;
using UseCase.MedicalExamination.SaveMedical;

namespace EmrCloudApi.Responses.MedicalExamination;

public class SaveMedicalResponse
{
    public SaveMedicalResponse(SaveMedicalStatus status, RaiinInfItemResponse validationRaiinInf, List<ValidationTodayOrdItemResponse> validationOdrInfs, ValidationKarteInfResponse validationKarte, ValidateFamilyListResponse validateFamily, UpsertFlowSheetMedicalResponse validationFlowSheet, UpsertPtDiseaseListMedicalResponse validationDisease)
    {
        Status = status;
        ValidationRaiinInf = validationRaiinInf;
        ValidationOdrInfs = validationOdrInfs;
        ValidationKarte = validationKarte;
        ValidateFamily = validateFamily;
        ValidationFlowSheet = validationFlowSheet;
        ValidationDisease = validationDisease;
    }
    public SaveMedicalStatus Status { get; private set; }

    public RaiinInfItemResponse ValidationRaiinInf { get; private set; }

    public List<ValidationTodayOrdItemResponse> ValidationOdrInfs { get; private set; }

    public ValidationKarteInfResponse ValidationKarte { get; private set; }

    public ValidateFamilyListResponse ValidateFamily { get; private set; }

    public UpsertFlowSheetMedicalResponse ValidationFlowSheet { get; private set; }

    public UpsertPtDiseaseListMedicalResponse ValidationDisease { get; private set; }
}
