using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.DeleteYousikiInf;

public class DeleteYousikiInfInputData : IInputData<DeleteYousikiInfOutputData>
{
    public DeleteYousikiInfInputData(int hpId, int userId, int sinYm, long ptId, int dataType)
    {
        HpId = hpId;
        UserId = userId;
        SinYm = sinYm;
        PtId = ptId;
        DataType = dataType;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public int SinYm { get; private set; }

    public long PtId { get; private set; }

    public int DataType { get; private set; }
}
