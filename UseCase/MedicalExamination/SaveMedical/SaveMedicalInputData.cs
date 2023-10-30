using Domain.Models.MonshinInf;
using UseCase.Core.Sync.Core;
using UseCase.Diseases.Upsert;
using UseCase.Family;
using UseCase.FlowSheet.Upsert;
using UseCase.MedicalExamination.UpsertTodayOrd;
using UseCase.NextOrder;

namespace UseCase.MedicalExamination.SaveMedical;

public class SaveMedicalInputData : IInputData<SaveMedicalOutputData>
{
    public SaveMedicalInputData(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, byte status, List<OdrInfItemInputData> odrItems, KarteItemInputData karteInf, int userId, bool isSagaku, bool autoSaveKensaIrai, FileItemInputItem fileItem, List<FamilyItem> listFamily, List<NextOrderItem> nextOrderItems, SpecialNoteItem specialNoteItem, List<UpsertPtDiseaseListInputItem> upsertPtDiseaseListInputItems, List<UpsertFlowSheetItemInputData> flowSheetItems, MonshinInforModel monshins)
    {
        HpId = hpId;
        PtId = ptId;
        RaiinNo = raiinNo;
        SinDate = sinDate;
        SyosaiKbn = syosaiKbn;
        JikanKbn = jikanKbn;
        HokenPid = hokenPid;
        SanteiKbn = santeiKbn;
        TantoId = tantoId;
        KaId = kaId;
        OdrItems = odrItems;
        KarteInf = karteInf;
        UketukeTime = uketukeTime;
        SinStartTime = sinStartTime;
        SinEndTime = sinEndTime;
        Status = status;
        UserId = userId;
        IsSagaku = isSagaku;
        AutoSaveKensaIrai = autoSaveKensaIrai;
        FileItem = fileItem;
        FamilyList = listFamily;
        NextOrderItems = nextOrderItems;
        SpecialNoteItem = specialNoteItem;
        UpsertPtDiseaseListInputItems = upsertPtDiseaseListInputItems;
        FlowSheetItems = flowSheetItems;
        Monshins = monshins;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }

    public int SinDate { get; private set; }

    public int SyosaiKbn { get; private set; }

    public int JikanKbn { get; private set; }

    public int HokenPid { get; private set; }

    public int SanteiKbn { get; private set; }

    public int TantoId { get; private set; }

    public int KaId { get; private set; }

    public string UketukeTime { get; private set; }

    public string SinStartTime { get; private set; }

    public string SinEndTime { get; private set; }

    public byte Status { get; private set; }

    public int UserId { get; private set; }

    public bool IsSagaku { get; private set; }

    public bool AutoSaveKensaIrai { get; private set; }

    public List<OdrInfItemInputData> OdrItems { get; private set; }

    public KarteItemInputData KarteInf { get; private set; }

    public FileItemInputItem FileItem { get; private set; }

    public List<FamilyItem> FamilyList { get; private set; }

    public List<NextOrderItem> NextOrderItems { get; private set; }

    public SpecialNoteItem SpecialNoteItem { get; private set; }

    public List<UpsertPtDiseaseListInputItem> UpsertPtDiseaseListInputItems { get; private set; }

    public List<UpsertFlowSheetItemInputData> FlowSheetItems { get; private set; }

    public MonshinInforModel Monshins { get; private set; }
}
