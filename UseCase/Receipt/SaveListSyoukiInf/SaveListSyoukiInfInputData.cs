using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveListSyoukiInf;

public class SaveListSyoukiInfInputData : IInputData<SaveListSyoukiInfOutputData>
{
    public SaveListSyoukiInfInputData(int hpId, int userId, long ptId, int sinYm, int hokenId, List<SyoukiInfItem> listSyoukiInf)
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

    public List<SyoukiInfItem> ListSyoukiInf { get; private set; }
}
