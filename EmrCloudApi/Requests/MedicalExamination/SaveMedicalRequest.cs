using EmrCloudApi.Requests.Family;
using UseCase.Diseases.Upsert;
using UseCase.FlowSheet.Upsert;
using UseCase.MedicalExamination.SaveMedical;
using UseCase.NextOrder;

namespace EmrCloudApi.Requests.MedicalExamination;

public class SaveMedicalRequest
{
    // Upsert medical
    public long PtId { get; set; }

    public int SyosaiKbn { get; set; }

    public int JikanKbn { get; set; }

    public int HokenPid { get; set; }

    public int SanteiKbn { get; set; }

    public int TantoId { get; set; }

    public int KaId { get; set; }

    public string UketukeTime { get; set; } = string.Empty;

    public string SinStartTime { get; set; } = string.Empty;

    public string SinEndTime { get; set; } = string.Empty;

    public byte Status { get; set; }

    public List<OdrInfItem> OdrInfs { get; set; } = new();

    public KarteItem KarteItem { get; set; } = new();

    public FileItemRequestItem FileItem { get; set; } = new();

    public List<FamilyRequestItem> FamilyList { get; set; } = new();

    public List<NextOrderItem> NextOrderItems { get; private set; } = new();

    public SpecialNoteItem SpecialNoteItem { get; private set; } = new();

    public List<UpsertPtDiseaseListInputItem> UpsertPtDiseaseListInputItems { get; private set; } = new();

    public List<UpsertFlowSheetItemInputData> FlowSheetItems { get; private set; } = new();
}
