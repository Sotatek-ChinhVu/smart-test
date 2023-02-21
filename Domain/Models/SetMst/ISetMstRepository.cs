using Domain.Common;

namespace Domain.Models.SetMst;

public interface ISetMstRepository : IRepositoryBase
{
    IEnumerable<SetMstModel> GetList(int hpId, int setKbn, int setKbnEdaNo, string textSearch);

    bool ReorderSetMst(int userId, int hpId, int setCdDragItem, int setCdDropItem);

    int PasteSetMst(int hpId, int userId, int setCdCopyItem, int setCdPasteItem, bool pasteToOtherGroup, int copyGenerationId, int copySetKbnEdaNo, int copySetKbn, int pasteGenerationId, int pasteSetKbnEdaNo, int pasteSetKbn);

    SetMstModel SaveSetMstModel(int userId, int sinDate, SetMstModel setMstModel);

    bool CheckExistSetMstBySetCd(int setCd);

    SetMstTooltipModel GetToolTip(int hpId, int setCd);
}
