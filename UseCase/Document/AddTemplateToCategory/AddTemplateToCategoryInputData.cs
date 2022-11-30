using UseCase.Core.Sync.Core;

namespace UseCase.Document.AddTemplateToCategory;

public class AddTemplateToCategoryInputData : IInputData<AddTemplateToCategoryOutputData>
{
    public AddTemplateToCategoryInputData(int hpId, string fileName, int categoryCd, Stream streamImage)
    {
        HpId = hpId;
        FileName = fileName;
        CategoryCd = categoryCd;
        StreamImage = streamImage;
    }

    public int HpId { get; private set; }

    public string FileName { get; private set; }

    public int CategoryCd { get; private set; }

    public Stream StreamImage { get; private set; }
}
