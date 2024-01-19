using UseCase.Yousiki.CreateYuIchiFile;

namespace EmrCloudApi.Requests.Yousiki;

public class CreateYuIchiFileRequest
{
    public int SinYm { get; set; }

    public bool IsCreateForm1File { get; set; }

    public bool IsCreateEFFile { get; set; }

    public bool IsCreateEFile { get; set; }

    public bool IsCreateFFile { get; set; }

    public bool IsCreateKData { get; set; }

    public ReactCreateYuIchiFile ReactCreateYuIchiFile { get; set; } = new();
}
