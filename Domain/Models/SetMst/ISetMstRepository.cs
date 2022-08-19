namespace Domain.Models.SetMst;

public interface ISetMstRepository
{
    IEnumerable<SetMstModel> GetList(int hpId, int setKbn, int setKbnEdaNo, string textSearch);

    bool SaveSetMstModel(SetMstModel model, int userId, int sinDate);
}
