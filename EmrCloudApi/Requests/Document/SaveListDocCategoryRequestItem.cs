namespace EmrCloudApi.Requests.Document;

public class SaveListDocCategoryRequestItem
{
    public int CategoryCd { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public int SortNo { get; set; }
}
