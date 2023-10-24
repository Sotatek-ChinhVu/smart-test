using UseCase.Core.Sync.Core;

namespace UseCase.Document.SaveDocInf;

public class SaveDocInfInputData : IInputData<SaveDocInfOutputData>
{
    public SaveDocInfInputData(int hpId, int userId, long ptId, int getDate, long fileId, int categoryCd, string file, string displayFileName, Stream streamImage)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        GetDate = getDate;
        FileId = fileId;
        CategoryCd = categoryCd;
        FileName = file;
        DisplayFileName = displayFileName;
        StreamImage = streamImage;
    }

    public SaveDocInfInputData SetFileName(string fileName)
    {
        FileName = fileName;
        return this;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public long FileId { get; private set; }

    public int GetDate { get; private set; }

    public int CategoryCd { get; private set; }

    public string FileName { get; private set; }

    public string DisplayFileName { get; private set; }

    public Stream StreamImage { get; private set; }
}
