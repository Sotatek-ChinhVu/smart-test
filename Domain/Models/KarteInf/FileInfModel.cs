namespace Domain.Models.KarteInf;

public class FileInfModel
{
    public FileInfModel(long raiinNo, long seqNo, bool isSchema, string linkFile, bool isDelete)
    {
        RaiinNo = raiinNo;
        SeqNo = seqNo;
        IsSchema = isSchema;
        LinkFile = linkFile;
        IsDelete = isDelete;
    }

    public long RaiinNo { get; private set; }

    public long SeqNo { get; private set; }

    public bool IsSchema { get; private set; }

    public string LinkFile { get; private set; }

    public bool IsDelete { get; private set; }
}
