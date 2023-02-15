using UseCase.Core.Sync.Core;

namespace UseCase.Receipt.SaveListSyoukiInf;

public class SaveSyoukiInfListInputData : IInputData<SaveSyoukiInfListOutputData>
{
    public SaveSyoukiInfListInputData(int hpId, int userId, long ptId, int sinYm, int hokenId, List<SyoukiInfItem> syoukiInfList)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        SinYm = sinYm;
        HokenId = hokenId;
        SyoukiInfList = syoukiInfList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public List<SyoukiInfItem> SyoukiInfList { get; private set; }
}
