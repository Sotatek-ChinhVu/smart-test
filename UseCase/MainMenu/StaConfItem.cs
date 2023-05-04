using Domain.Models.MainMenu;

namespace UseCase.MainMenu;

public class StaConfItem
{
    public StaConfItem(StaConfModel model)
    {
        MenuId = model.MenuId;
        ConfId = model.ConfId;
        Val = model.Val;
    }

    public StaConfItem(int menuId, int confId, string val)
    {
        MenuId = menuId;
        ConfId = confId;
        Val = val;
    }

    public int MenuId { get; private set; }

    public int ConfId { get; private set; }

    public string Val { get; private set; }
}
