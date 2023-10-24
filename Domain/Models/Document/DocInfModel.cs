using System;

namespace Domain.Models.Document;

public class DocInfModel
{
    public DocInfModel()
    {
        CategoryName = string.Empty;
        FileName = string.Empty;
        DisplayFileName = string.Empty;
        UpdateDate = new DateTime();
        FileLink = string.Empty;
    }

    public DocInfModel(int hpId, long fileId, long ptId, int getDate, int categoryCd, string categoryName, string fileName, string displayFileName, DateTime updateDate)
    {
        HpId = hpId;
        FileId = fileId;
        PtId = ptId;
        GetDate = getDate;
        CategoryCd = categoryCd;
        CategoryName = categoryName;
        FileName = fileName;
        DisplayFileName = displayFileName;
        UpdateDate = updateDate;
        FileLink = string.Empty;
    }

    public DocInfModel SetFileLinkForDocInf(string fileLink)
    {
        FileLink = fileLink;
        return this;
    }

    public DocInfModel(long ptId, long ptNum, int sinDate, long raiinNo, int seqNo, int categoryCd, string fileName, string dspFileName, int isLocked, DateTime lockDate, int lockId, string lockMachine, int isDeleted)
    {
        PtId = ptId;
        PtNum = ptNum;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        SeqNo = seqNo;
        CategoryCd = categoryCd;
        FileName = fileName;
        DspFileName = dspFileName;
        IsLocked = isLocked;
        LockDate = lockDate;
        LockId = lockId;
        LockMachine = lockMachine;
        IsDeleted = isDeleted;
        CategoryName = string.Empty;
        DisplayFileName = string.Empty;
        FileLink = string.Empty;
    }

    public int HpId { get; private set; }

    public long FileId { get; private set; }

    public long PtId { get; private set; }

    public int GetDate { get; private set; }

    public int CategoryCd { get; private set; }

    public string CategoryName { get; private set; }

    public string FileName { get; private set; }

    public string DisplayFileName { get; private set; }

    public DateTime UpdateDate { get; private set; }

    public string FileLink { get; private set; }

    public long PtNum { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public int SeqNo { get; private set; }

    public string DspFileName { get; private set; }

    public int IsLocked { get; private set; }

    public DateTime LockDate { get; private set; }

    public int LockId { get; private set; }

    public string LockMachine { get; private set; }

    public int IsDeleted { get; private set; }
}
