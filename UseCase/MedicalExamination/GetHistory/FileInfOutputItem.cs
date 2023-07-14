using Domain.Models.KarteInf;
using System.Text.Json.Serialization;

namespace UseCase.MedicalExamination.GetHistory;

public class FileInfOutputItem
{
    public FileInfOutputItem(FileInfModel model, string host)
    {
        SeqNo = model.SeqNo;
        IsSchema = model.IsSchema;
        LinkFile = model.LinkFile != string.Empty ? host + model.LinkFile : string.Empty;
        IsDeleted = model.IsDelete;
    }

    [JsonPropertyName("seqNo")]
    public long SeqNo { get; private set; }

    [JsonPropertyName("isSchema")]
    public bool IsSchema { get; private set; }

    [JsonPropertyName("linkFile")]
    public string LinkFile { get; private set; }

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; private set; }
}
