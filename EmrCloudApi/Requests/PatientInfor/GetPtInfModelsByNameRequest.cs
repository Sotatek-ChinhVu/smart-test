namespace EmrCloudApi.Requests.PatientInfor;

public class GetPtInfModelsByNameRequest
{
    public string KanaName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public int BirthDate { get; set; }

    public int Sex1 { get; set; }

    public int Sex2 { get; set; }
}
