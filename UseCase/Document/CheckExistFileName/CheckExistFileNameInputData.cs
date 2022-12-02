using UseCase.Core.Sync.Core;

namespace UseCase.Document.CheckExistFileName;

public class CheckExistFileNameInputData : IInputData<CheckExistFileNameOutputData>
{
    public CheckExistFileNameInputData(int hpId, string fileName, int categoryCd, int ptId, bool isCheckDocInf)
    {
        HpId = hpId;
        FileName = fileName;
        CategoryCd = categoryCd;
        PtId = ptId;
        IsCheckDocInf = isCheckDocInf;
    }

    public int HpId { get; private set; }

    public string FileName { get; private set; }

    public int CategoryCd { get; private set; }

    public int PtId { get; private set; }

    public bool IsCheckDocInf { get; private set; }
}
