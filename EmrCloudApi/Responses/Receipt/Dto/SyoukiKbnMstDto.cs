using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt.Dto;

public class SyoukiKbnMstDto
{
    public SyoukiKbnMstDto(SyoukiKbnMstItem output)
    {
        SyoukiKbn = output.SyoukiKbn;
        Name = output.Name;
        StartYm = output.StartYm;
    }

    public int SyoukiKbn { get; private set; }

    public string Name { get; private set; }

    public int StartYm { get; private set; }

    public string SyoukiKbnDisplay
    {
        get => SyoukiKbn.ToString("d2") + " " + Name;
    }
}
