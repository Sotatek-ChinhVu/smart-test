using Domain.Models.KarteInf;

namespace UseCase.MedicalExamination.GetHistory;

public class FileInfOutputItem
{
    public FileInfOutputItem(FileInfModel model, string host)
    {
        SeqNo = model.SeqNo;
        LinkFile = model.LinkFile != string.Empty ? host + model.LinkFile : string.Empty;
        IsDeleted = model.IsDelete;
    }

    public long SeqNo { get; private set; }

    public string LinkFile { get; private set; }

    public bool IsDeleted { get; private set; }
}
