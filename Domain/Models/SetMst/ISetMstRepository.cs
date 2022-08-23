namespace Domain.Models.SetMst;

public interface ISetMstRepository
{
    IEnumerable<SetMstModel> GetList(int hpId, int setKbn, int setKbnEdaNo, string textSearch);

    bool ReorderSetMst(int userId, List<SetMstModel> setMstModels);

    SetMstModel? SaveSetMstModel(int userId, int sinDate, SetMstModel setMstModel);
}
