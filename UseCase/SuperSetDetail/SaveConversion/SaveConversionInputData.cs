using Domain.Models.SuperSetDetail;
using UseCase.Core.Sync.Core;

namespace UseCase.SuperSetDetail.SaveConversion;

public class SaveConversionInputData : IInputData<SaveConversionOutputData>
{
    public SaveConversionInputData(int hpId, int userId, ConversionItemModel conversionModel)
    {
        HpId = hpId;
        UserId = userId;
        ConversionModel = conversionModel;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public ConversionItemModel ConversionModel { get; private set; }
}
