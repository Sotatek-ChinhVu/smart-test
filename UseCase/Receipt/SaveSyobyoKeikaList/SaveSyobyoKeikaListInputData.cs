using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveListSyobyoKeika;

public class SaveSyobyoKeikaListInputData : IInputData<SaveSyobyoKeikaListOutputData>
{
    public SaveSyobyoKeikaListInputData(int hpId, int userId, long ptId, int sinYm, int hokenId, List<SyobyoKeikaItem> syobyoKeikaList)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        SinYm = sinYm;
        HokenId = hokenId;
        SyobyoKeikaList = syobyoKeikaList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public List<SyobyoKeikaItem> SyobyoKeikaList { get; private set; }
}
