namespace EmrCloudApi.Requests.SetMst;

public class GetOdrSetNameRequest
{
    public SetCheckBoxStatusRequest CheckBoxStatus { get; set; } = new();

    public int GenerationId { get; set; }

    public int TimeExpired { get; set; }

    public string ItemName { get; set; } = string.Empty;
}
