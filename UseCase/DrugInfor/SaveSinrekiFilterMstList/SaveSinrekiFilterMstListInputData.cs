using Domain.Models.DrugInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.DrugInfor.SaveSinrekiFilterMstList;

public class SaveSinrekiFilterMstListInputData : IInputData<SaveSinrekiFilterMstListOutputData>
{
    public SaveSinrekiFilterMstListInputData(int hpId, int userId, List<SinrekiFilterMstModel> sinrekiFilterMstList)
    {
        HpId = hpId;
        UserId = userId;
        SinrekiFilterMstList = sinrekiFilterMstList;
    }

    public int HpId { get; private set; }

    public int UserId { get; private set; }

    public List<SinrekiFilterMstModel> SinrekiFilterMstList { get; private set; }
}
