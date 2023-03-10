using UseCase.Receipt;
using UseCase.Receipt.SyobyoKeikaHistory;

namespace EmrCloudApi.Responses.Receipt.Dto;

public class SyobyoKeikaHistoryDto
{
    public SyobyoKeikaHistoryDto(SyobyoKeikaHistoryOutputItem output)
    {
        SinYm = output.SinYm;
        SinYmDisplay = output.SinYmDisplay;
        HokenId = output.HokenId;
        HokenName = output.HokenName;
        SyobyoKeikaList = output.SyobyoKeikaList.Select(item => new SyobyoKeikaDto(item)).ToList();
    }

    public int SinYm { get; private set; }

    public string SinYmDisplay { get; private set; }

    public int HokenId { get; private set; }

    public string HokenName { get; private set; }

    public List<SyobyoKeikaDto> SyobyoKeikaList { get; private set; }
}
