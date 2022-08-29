namespace Domain.Models.SetMst;

public interface ISetMstRepository
{
    IEnumerable<SetMstModel> GetList(int hpId, int setKbn, int setKbnEdaNo, string textSearch);

    bool ReorderSetMst(int userId, int hpId, int setCdDragItem, int setCdDropItem);

    SetMstModel? SaveSetMstModel(int userId, int sinDate, SetMstModel setMstModel);
}
