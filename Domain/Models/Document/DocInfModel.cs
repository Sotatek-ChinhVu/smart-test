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
}
