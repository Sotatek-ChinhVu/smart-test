namespace EmrCloudApi.Requests.Document;

public class DeleteDocCategoryRequest
{
    public int CategoryCd { get; set; }

    public long PtId { get; set; }

    public int MoveToCategoryCd { get; set; }
}
