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
        CreateDate = model.CreateDate;
        UpdateDate = model.UpdateDate;
        CreateName = model.CreateName;
        UpdateName = model.UpdateName;
    }

    [JsonPropertyName("seqNo")]
    public long SeqNo { get; private set; }

    [JsonPropertyName("isSchema")]
    public bool IsSchema { get; private set; }

    [JsonPropertyName("linkFile")]
    public string LinkFile { get; private set; }

    [JsonPropertyName("isDeleted")]
    public bool IsDeleted { get; private set; }

    [JsonPropertyName("createDate")]
    public DateTime CreateDate { get; private set; }

    [JsonPropertyName("updateDate")]
    public DateTime UpdateDate { get; private set; }

    [JsonPropertyName("createName")]
    public string CreateName { get; private set; }

    [JsonPropertyName("updateName")]
    public string UpdateName { get; private set; }

    [JsonPropertyName("updateDateDisplay")]
    public string UpdateDateDisplay
    {
        get => UpdateDate.ToString("yyyy/MM/dd HH:mm");
    }

    [JsonPropertyName("createDateDisplay")]
    public string CreateDateDisplay
    {
        get => CreateDate.ToString("yyyy/MM/dd HH:mm");
    }
}
