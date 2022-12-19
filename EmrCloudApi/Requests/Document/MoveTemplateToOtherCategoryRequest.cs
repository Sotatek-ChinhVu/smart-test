namespace EmrCloudApi.Requests.Document;

public class MoveTemplateToOtherCategoryRequest
{
    public int OldCategoryCd { get; set; }

    public int NewCategoryCd { get; set; }

    public string FileName { get; set; } = string.Empty;
}
