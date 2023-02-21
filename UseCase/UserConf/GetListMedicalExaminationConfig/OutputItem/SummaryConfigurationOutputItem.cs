namespace UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

public class SummaryConfigurationOutputItem
{
    public SummaryConfigurationOutputItem(int popupVisible, int fontSize, string fontStyle, int buttonPositionValue, bool pinummaryOnSet, Dictionary<int, string> colorCode, List<int> isSelectedProperties)
    {
        PopupVisible = popupVisible;
        FontSize = fontSize;
        FontStyle = fontStyle;
        ButtonPositionValue = buttonPositionValue;
        PinummaryOnSet = pinummaryOnSet;
        ColorCode = colorCode;
        IsSelectedProperties = isSelectedProperties;
    }

    public SummaryConfigurationOutputItem()
    {
        PopupVisible = 0;
        FontSize = 0;
        FontStyle = string.Empty;
        ButtonPositionValue = 0;
        PinummaryOnSet = false;
        ColorCode = new();
        IsSelectedProperties = new();
    }

    public int PopupVisible { get; private set; }

    public int FontSize { get; private set; }

    public string FontStyle { get; private set; }

    public int ButtonPositionValue { get; private set; }

    public bool PinummaryOnSet { get; private set; }

    public Dictionary<int, string> ColorCode { get; private set; }

    public List<int> IsSelectedProperties { get; private set; }
}
