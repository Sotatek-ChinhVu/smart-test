using UseCase.Core.Sync.Core;

namespace UseCase.Yousiki.CreateYuIchiFile;

public class CreateYuIchiFileInputData : IInputData<CreateYuIchiFileOutputData>
{
    public CreateYuIchiFileInputData(int hpId, int sinYm, bool isCreateForm1File, bool isCreateEFFile, bool isCreateEFile, bool isCreateFFile, bool isCreateKData, ReactCreateYuIchiFile reactCreateYuIchiFile)
    {
        HpId = hpId;
        SinYm = sinYm;
        IsCreateForm1File = isCreateForm1File;
        IsCreateEFFile = isCreateEFFile;
        IsCreateEFile = isCreateEFile;
        IsCreateFFile = isCreateFFile;
        IsCreateKData = isCreateKData;
        ReactCreateYuIchiFile = reactCreateYuIchiFile;
    }

    public int HpId { get; private set; }

    public int SinYm { get; private set; }

    public bool IsCreateForm1File { get; private set; }

    public bool IsCreateEFFile { get; private set; }

    public bool IsCreateEFile { get; private set; }

    public bool IsCreateFFile { get; private set; }

    public bool IsCreateKData { get; private set; }

    public ReactCreateYuIchiFile ReactCreateYuIchiFile { get; private set; }
}
