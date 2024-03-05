namespace EmrCloudApi.Requests.Online;

public class OnlQuaConfirmationType
{
    public int ConfirmationType { get; set; }

    public string InfConsFlg { get; set; } = string.Empty;

    public int prescriptionIssueType { get; set; }
}
