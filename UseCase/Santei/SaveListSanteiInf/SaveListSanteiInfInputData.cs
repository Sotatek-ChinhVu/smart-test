using UseCase.Core.Sync.Core;

namespace UseCase.Santei.SaveListSanteiInf;

public class SaveListSanteiInfInputData : IInputData<SaveListSanteiInfOutputData>
{
    public SaveListSanteiInfInputData(int hpId, int userId, long ptId, int sinDate, int hokenPid, List<SanteiInfInputItem> listSanteiInfs)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        SinDate = sinDate;
        HokenPid = hokenPid;
        ListSanteiInfs = listSanteiInfs;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public int HokenPid { get; private set; }

    public List<SanteiInfInputItem> ListSanteiInfs { get; private set; }
}
