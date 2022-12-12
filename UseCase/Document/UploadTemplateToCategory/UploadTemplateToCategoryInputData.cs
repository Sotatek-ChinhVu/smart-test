using UseCase.Core.Sync.Core;

namespace UseCase.Document.UploadTemplateToCategory;

public class UploadTemplateToCategoryInputData : IInputData<UploadTemplateToCategoryOutputData>
{
    public UploadTemplateToCategoryInputData(int hpId, string fileName, int categoryCd, bool overWrite, Stream streamImage)
    {
        HpId = hpId;
        FileName = fileName;
        CategoryCd = categoryCd;
        OverWrite = overWrite;
        StreamImage = streamImage;
    }

    public int HpId { get; private set; }

    public string FileName { get; private set; }

    public int CategoryCd { get; private set; }

    public bool OverWrite { get; private set; }

    public Stream StreamImage { get; private set; }
}
