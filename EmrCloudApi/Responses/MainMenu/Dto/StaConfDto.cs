using UseCase.MainMenu;

namespace EmrCloudApi.Responses.MainMenu.Dto;

public class StaConfDto
{
    public StaConfDto(StaConfItem item)
    {
        MenuId = item.MenuId;
        ConfId = item.ConfId;
        Val = item.Val;
    }

    public int MenuId { get; private set; }

    public int ConfId { get; private set; }

    public string Val { get; private set; }
}
