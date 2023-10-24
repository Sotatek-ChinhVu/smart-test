using UseCase.Receipt;

namespace EmrCloudApi.Responses.Receipt.Dto;

public class RecePreviewDto
{
    public RecePreviewDto(ReceInfItem output)
    {
        SeikyuYm = output.SeikyuYm;
        HokenId = output.HokenId;
        HokenKbn = output.HokenKbn;
        SinYm = output.SinYm;
        SeikyuYmDisplay = output.SeikyuYmDisplay;
        SinYmDisplay = output.SinYmDisplay;
        HokenPatternName = output.HokenPatternName;
    }

    public int SeikyuYm { get; private set; }

    public int SinYm { get; private set; }

    public int HokenId { get; private set; }

    public int HokenKbn { get; private set; }

    public string SeikyuYmDisplay { get; private set; }

    public string SinYmDisplay { get; private set; }

    public string HokenPatternName { get; private set; }
}
