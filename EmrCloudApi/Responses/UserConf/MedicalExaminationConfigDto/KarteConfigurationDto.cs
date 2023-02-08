using UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

namespace EmrCloudApi.Responses.UserConf.MedicalExaminationConfigDto;

public class KarteConfigurationDto
{
    public KarteConfigurationDto(KarteConfigurationOutputItem output)
    {
        FontStyle = output.FontStyle;
        FontSize = output.FontSize;
        ChartAutoWrapTextSetting = output.ChartAutoWrapTextSetting;
        ImageSize = output.ImageSize;
        DestinationOfCopy = output.DestinationOfCopy;
        IMESetting = output.IMESetting;
        IsAllowResize = output.IsAllowResize;
    }

    public string FontStyle { get; private set; }

    public int FontSize { get; private set; }

    public int ChartAutoWrapTextSetting { get; private set; }

    public int ImageSize { get; private set; }

    public int DestinationOfCopy { get; private set; }

    public int IMESetting { get; private set; }

    public int IsAllowResize { get; private set; }
}
