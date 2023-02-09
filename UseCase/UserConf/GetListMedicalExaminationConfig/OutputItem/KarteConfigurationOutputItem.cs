namespace UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

public class KarteConfigurationOutputItem
{
    public KarteConfigurationOutputItem(string fontStyle, int fontSize, int chartAutoWrapTextSetting, int imageSize, int destinationOfCopy, int iMESetting, int isAllowResize)
    {
        FontStyle = fontStyle;
        FontSize = fontSize;
        ChartAutoWrapTextSetting = chartAutoWrapTextSetting;
        ImageSize = imageSize;
        DestinationOfCopy = destinationOfCopy;
        IMESetting = iMESetting;
        IsAllowResize = isAllowResize;
    }

    public KarteConfigurationOutputItem()
    {
        FontStyle = string.Empty;
        FontSize = 0;
        ChartAutoWrapTextSetting = 0;
        ImageSize = 0;
        DestinationOfCopy = 0;
        IMESetting = 0;
        IsAllowResize = 0;
    }

    public string FontStyle { get; private set; }

    public int FontSize { get; private set; }

    public int ChartAutoWrapTextSetting { get; private set; }

    public int ImageSize { get; private set; }

    public int DestinationOfCopy { get; private set; }

    public int IMESetting { get; private set; }

    public int IsAllowResize { get; private set; }
}
