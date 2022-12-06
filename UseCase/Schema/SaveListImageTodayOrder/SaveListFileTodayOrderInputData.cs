using UseCase.Core.Sync.Core;

namespace UseCase.Schema.SaveListImageTodayOrder;

public class SaveListFileTodayOrderInputData : IInputData<SaveListFileTodayOrderOutputData>
{
    public SaveListFileTodayOrderInputData(int hpId, long ptId, long raiinNo, List<FileItem> listImages, List<long> listFileDeletes)
    {
        HpId = hpId;
        PtId = ptId;
        RaiinNo = raiinNo;
        ListImages = listImages;
        ListFileIdDeletes = listFileDeletes;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public long RaiinNo { get; private set; }

    public List<FileItem> ListImages { get; private set; }

    public List<long> ListFileIdDeletes { get; private set; }
}
