using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.SaveConversion;

public class SaveConversionInputData : IInputData<SaveConversionOutputData>
{
    public SaveConversionInputData(int hpId, int userId, string sourceItemCd, string conversionItemCd, List<string> deleteConversionItemCdList)
    {
        HpId = hpId;
        UserId = userId;
        SourceItemCd = sourceItemCd;
        ConversionItemCd = conversionItemCd;
        DeleteConversionItemCdList = deleteConversionItemCdList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public string SourceItemCd { get; private set; }

    public string ConversionItemCd { get; private set; }

    public List<string> DeleteConversionItemCdList { get; private set; }
}
