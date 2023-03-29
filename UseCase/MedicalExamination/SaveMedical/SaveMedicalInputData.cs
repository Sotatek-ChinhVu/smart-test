using UseCase.Core.Sync.Core;
using UseCase.Family;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace UseCase.MedicalExamination.SaveMedical;

public class SaveMedicalInputData : IInputData<SaveMedicalOutputData>
{
    public SaveMedicalInputData(int hpId, long ptId, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, byte status, List<OdrInfItemInputData> odrItems, KarteItemInputData karteInf, int userId, FileItemInputItem fileItem, List<FamilyItem> listFamily)
    {
        HpId = hpId;
        PtId = ptId;
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
        FileItem = fileItem;
        FamilyList = listFamily;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

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

    public List<OdrInfItemInputData> OdrItems { get; private set; }

    public KarteItemInputData KarteInf { get; private set; }

    public FileItemInputItem FileItem { get; private set; }

    // Family input data
    public List<FamilyItem> FamilyList { get; private set; }
}
