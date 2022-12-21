namespace EmrCloudApi.Requests.MedicalExamination;

public class FileItemRequestItem
{
    public bool IsUpdateFile { get; set; }

    public List<string> ListFileItems { get; set; } = new();
}
