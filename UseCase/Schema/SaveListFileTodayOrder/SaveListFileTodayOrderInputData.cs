using UseCase.Core.Sync.Core;

namespace UseCase.Schema.SaveListFileTodayOrder;

public class SaveListFileTodayOrderInputData : IInputData<SaveListFileTodayOrderOutputData>
{
    public SaveListFileTodayOrderInputData(int hpId, int userId, long ptId, int setCd, int typeUpload, List<FileItem> listImages)
    {
        HpId = hpId;
        PtId = ptId;
        SetCd = setCd;
        TypeUpload = typeUpload;
        ListImages = listImages;
        UserId = userId;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public int SetCd { get; private set; }

    public int TypeUpload { get; private set; }

    public List<FileItem> ListImages { get; private set; }
}
