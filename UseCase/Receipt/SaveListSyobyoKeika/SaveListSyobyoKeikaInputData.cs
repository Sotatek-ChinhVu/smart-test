using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveListSyobyoKeika;

public class SaveListSyobyoKeikaInputData : IInputData<SaveListSyobyoKeikaOutputData>
{
    public SaveListSyobyoKeikaInputData(int hpId, int userId, long ptId, int sinYm, int hokenId, List<SyobyoKeikaItem> listSyoukiInf)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        SinYm = sinYm;
        HokenId = hokenId;
        ListSyoukiInf = listSyoukiInf;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public List<SyobyoKeikaItem> ListSyoukiInf { get; private set; }
}
