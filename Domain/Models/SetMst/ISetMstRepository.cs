using Domain.Common;

namespace Domain.Models.SetMst;

public interface ISetMstRepository : IRepositoryBase
{
    List<SetMstModel> GetList(int hpId, int setKbn, int setKbnEdaNo, int generationId,  string textSearch);

    (bool status, List<SetMstModel> setMstModels) ReorderSetMst(int userId, int hpId, int setCdDragItem, int setCdDropItem);
    List<SetMstModel> PasteSetMst(int hpId, int userId, int generationId, int setCdCopyItem, int setCdPasteItem, bool pasteToOtherGroup, int copySetKbnEdaNo, int copySetKbn, int pasteSetKbnEdaNo, int pasteSetKbn);

    List<SetMstModel> SaveSetMstModel(int userId, int sinDate, SetMstModel setMstModel);

    bool CheckExistSetMstBySetCd(int setCd);

    bool CheckExistSetMstBySetCd(int hpId, List<int> setCdList);

    SetMstTooltipModel GetToolTip(int hpId, int setCd);
}
