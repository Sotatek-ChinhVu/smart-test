namespace EmrCloudApi.Requests.Yousiki;

public class CreateYuIchiFileRequest
{
    public int SinYm { get; set; }

    public bool IsCreateForm1File { get; set; }

    public bool IsCreateEFFile { get; set; }

    public bool IsCreateEFile { get; set; }

    public bool IsCreateFFile { get; set; }

    public bool IsCreateKData { get; set; }

    public bool IsCheckedTestPatient { get; set; }

    public ReactCreateYuIchiFileRequestItem ReactCreateYuIchiFile { get; set; } = new();
}

public class ReactCreateYuIchiFileRequestItem
{
    public bool ConfirmPatientList { get; set; }
}