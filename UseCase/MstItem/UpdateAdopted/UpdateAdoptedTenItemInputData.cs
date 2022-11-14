using UseCase.Core.Sync.Core;

namespace UseCase.MstItem.UpdateAdopted;

public class UpdateAdoptedTenItemInputData : IInputData<UpdateAdoptedTenItemOutputData>
{
    public UpdateAdoptedTenItemInputData(int valueAdopted, string itemCdInputItem, int startDateInputItem, int hpId, int userId)
    {
        ValueAdopted = valueAdopted;
        ItemCdInputItem = itemCdInputItem;
        StartDateInputItem = startDateInputItem;
        HpId = hpId;
        UserId = userId;
    }

    public int ValueAdopted { get; private set; }

    public string ItemCdInputItem { get; private set; }

    public int StartDateInputItem { get; private set; }

    public int HpId { get; private set; }

    public int UserId { get; private set; }
}
