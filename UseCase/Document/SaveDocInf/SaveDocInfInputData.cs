using UseCase.Core.Sync.Core;

namespace UseCase.Document.SaveDocInf;

public class SaveDocInfInputData : IInputData<SaveDocInfOutputData>
{
    public SaveDocInfInputData(int hpId, int userId, long ptId, int sinDate, long raiinNo, int seqNo, int categoryCd, string file, string displayFileName, Stream streamImage)
    {
        HpId = hpId;
        UserId = userId;
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        SeqNo = seqNo;
        CategoryCd = categoryCd;
        FileName = file;
        DisplayFileName = displayFileName;
        StreamImage = streamImage;
    }

    public SaveDocInfInputData SetFileName(string file)
    {
        FileName = file;
        return this;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public int SeqNo { get; private set; }

    public int CategoryCd { get; private set; }

    public string FileName { get; private set; }

    public string DisplayFileName { get; private set; }

    public Stream StreamImage { get; private set; }
}
