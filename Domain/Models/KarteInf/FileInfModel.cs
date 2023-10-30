namespace Domain.Models.KarteInf;

public class FileInfModel
{
    public FileInfModel(long raiinNo, long seqNo, bool isSchema, string linkFile, bool isDelete, DateTime createDate, DateTime updateDate, string createName, string updateName)
    {
        RaiinNo = raiinNo;
        SeqNo = seqNo;
        IsSchema = isSchema;
        LinkFile = linkFile;
        IsDelete = isDelete;
        CreateDate = createDate;
        UpdateDate = updateDate;
        CreateName = createName;
        UpdateName = updateName;
    }

    public FileInfModel(long raiinNo, long seqNo, bool isSchema, string linkFile)
    {
        RaiinNo = raiinNo;
        SeqNo = seqNo;
        IsSchema = isSchema;
        LinkFile = linkFile;
        IsDelete = false;
        CreateName = string.Empty;
        UpdateName = string.Empty;
    }

    public FileInfModel(bool isSchema, string linkFile)
    {
        RaiinNo = 0;
        SeqNo = 0;
        IsSchema = isSchema;
        LinkFile = linkFile;
        IsDelete = false;
        CreateName = string.Empty;
        UpdateName = string.Empty;
    }

    public long RaiinNo { get; private set; }

    public long SeqNo { get; private set; }

    public bool IsSchema { get; private set; }

    public string LinkFile { get; private set; }

    public bool IsDelete { get; private set; }

    public DateTime CreateDate { get; private set; }

    public DateTime UpdateDate { get; private set; }

    public string CreateName { get; private set; }

    public string UpdateName { get; private set; }
}
