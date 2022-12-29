using UseCase.Core.Sync.Core;

namespace UseCase.Santei.SaveListSanteiInf;

public class SaveListSanteiInfInputData : IInputData<SaveListSanteiInfOutputData>
{
    public SaveListSanteiInfInputData(int hpId, int userId, long ptId, List<SanteiInfInputItem> listSanteiInfs)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        ListSanteiInfs = listSanteiInfs;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public List<SanteiInfInputItem> ListSanteiInfs { get; set; }
}
