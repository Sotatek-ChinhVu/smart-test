using Domain.Models.MonshinInf;
using EmrCloudApi.Requests.Diseases;
using EmrCloudApi.Requests.Family;
using UseCase.FlowSheet.Upsert;
using UseCase.MedicalExamination.SaveMedical;
using UseCase.NextOrder;

namespace EmrCloudApi.Requests.MedicalExamination;

public class SaveMedicalRequest
{
    // Upsert medical
    public long PtId { get; set; }

    public long RaiinNo { get; set; }

    public int SinDate { get; set; }

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

    public bool IsSagaku { get; set; }

    public bool AutoSaveKensaIrai { get; set; } = true;

    public List<OdrInfItem> OdrInfs { get; set; } = new();

    public KarteItem KarteItem { get; set; } = new();

    public FileItemRequestItem FileItem { get; set; } = new();

    public List<FamilyRequestItem> FamilyList { get; set; } = new();

    public List<NextOrderItem> NextOrderItems { get; set; } = new();

    public SpecialNoteItem SpecialNoteItem { get; set; } = new();

    public List<UpsertPtDiseaseListItem> DiseaseListItems { get; set; } = new();

    public List<UpsertFlowSheetItemInputData> FlowSheetItems { get; set; } = new();

    public MonshinInforModel Monshin { get; set; } = new();

    public MedicalStateChanged StateChanged { get; set; } = new();
}
