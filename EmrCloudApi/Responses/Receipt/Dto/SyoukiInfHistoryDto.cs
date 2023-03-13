using UseCase.Receipt.SyoukiInfHistory;

namespace EmrCloudApi.Responses.Receipt.Dto;

public class SyoukiInfHistoryDto
{
    public SyoukiInfHistoryDto(SyoukiInfHistoryOutputItem output)
    {
        SinYm = output.SinYm;
        SinYmDisplay = output.SinYmDisplay;
        HokenId = output.HokenId;
        HokenName = output.HokenName;
        SyoukiInfList = output.SyoukiInfList.Select(item => new SyoukiInfDto(item)).ToList();
        SyoukiKbnMstList = output.SyoukiKbnList.Select(item => new SyoukiKbnMstDto(item)).ToList();
    }

    public int SinYm { get; private set; }

    public string SinYmDisplay { get; private set; }

    public int HokenId { get; private set; }

    public string HokenName { get; private set; }

    public List<SyoukiInfDto> SyoukiInfList { get; private set; }

    public List<SyoukiKbnMstDto> SyoukiKbnMstList { get; private set; }
}
