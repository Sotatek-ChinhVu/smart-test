using Domain.Models.MstItem;

namespace EmrCloudApi.Responses.MainMenu.Dto;

public class KensaCenterMstDto
{
    public KensaCenterMstDto(KensaCenterMstModel model)
    {
        Id = model.Id;
        CenterCd = model.CenterCd;
        CenterName = model.CenterName;
        PrimaryKbn = model.PrimaryKbn;
        SortNo = model.SortNo;
    }

    public long Id { get; private set; }

    public string CenterCd { get; private set; }

    public string CenterName { get; private set; }

    public int PrimaryKbn { get; private set; }

    public int SortNo { get; private set; }
}
