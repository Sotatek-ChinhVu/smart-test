namespace Domain.Models.MainMenu;

public class StaConfModel
{
    public StaConfModel(int menuId, int confId, string val)
    {
        MenuId = menuId;
        ConfId = confId;
        Val = val;
    }

    public int MenuId { get; private set; }

    public int ConfId { get; private set; }

    public string Val { get; private set; }
}
