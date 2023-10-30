namespace EmrCloudApi.Requests.SetMst;

public class SaveOdrSetRequest
{
    public int SinDate { get; set; }

    public List<SaveOdrSetRequestItem> SetNameModelList { get; set; } = new();

    public List<SaveSetNameRequestItem> UpdateSetNameList { get; set; } = new();
}
