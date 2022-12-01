namespace Domain.Models.Document;

public class DocInfModel
{
    public DocInfModel(int hpId, long ptId, int sinDate, long raiinNo, int seqNo, int categoryCd, string categoryName, string fileName, string displayFileName, DateTime updateDate)
    {
        HpId = hpId;
        PtId = ptId;
        SinDate = sinDate;
        RaiinNo = raiinNo;
        SeqNo = seqNo;
        CategoryCd = categoryCd;
        CategoryName = categoryName;
        File = fileName;
        DisplayFileName = displayFileName;
        UpdateDate = updateDate;
        FileLink = string.Empty;
    }

    public DocInfModel SetFileLinkForDocInf(string fileLink)
    {
        FileLink = fileLink;
        return this;
    }

    public int HpId { get; private set; }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public int SeqNo { get; private set; }

    public int CategoryCd { get; private set; }

    public string CategoryName { get; private set; }

    public string File { get; private set; }

    public string DisplayFileName { get; private set; }

    public DateTime UpdateDate { get; private set; }

    public string FileLink { get; private set; }
}
