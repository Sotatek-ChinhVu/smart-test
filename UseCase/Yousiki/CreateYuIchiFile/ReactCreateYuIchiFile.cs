using Newtonsoft.Json;

namespace UseCase.Yousiki.CreateYuIchiFile;

public class ReactCreateYuIchiFile
{
    [JsonConstructor]
    public ReactCreateYuIchiFile(bool confirmPatientList)
    {
        ConfirmPatientList = confirmPatientList;
    }

    public ReactCreateYuIchiFile()
    {
    }

    public bool ConfirmPatientList { get; private set; }
}
