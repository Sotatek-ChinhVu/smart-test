namespace Domain.Models.KarteInf;

public class FileInfModel
{
    public FileInfModel(long raiinNo, long seqNo, string linkFile, bool isDelete)
    {
        RaiinNo = raiinNo;
        SeqNo = seqNo;
        LinkFile = linkFile;
        IsDelete = isDelete;
    }

    public long RaiinNo { get; private set; }

    public long SeqNo { get; private set; }

    public string LinkFile { get; private set; }

    public bool IsDelete { get; private set; }
}
