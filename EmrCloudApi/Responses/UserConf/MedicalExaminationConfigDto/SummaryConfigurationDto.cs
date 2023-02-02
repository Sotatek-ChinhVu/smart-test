using UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

namespace EmrCloudApi.Responses.UserConf.MedicalExaminationConfigDto;

public class SummaryConfigurationDto
{
    public SummaryConfigurationDto(SummaryConfigurationOutputItem output)
    {
        PopupVisible = output.PopupVisible;
        FontSize = output.FontSize;
        FontStyle = output.FontStyle;
        ButtonPositionValue = output.ButtonPositionValue;
        PinummaryOnSet = output.PinummaryOnSet;
        ColorCode = output.ColorCode;
        IsSelectedProperties = output.IsSelectedProperties;
    }

    public int PopupVisible { get; private set; }

    public int FontSize { get; private set; }

    public string FontStyle { get; private set; }

    public int ButtonPositionValue { get; private set; }

    public bool PinummaryOnSet { get; private set; }

    public Dictionary<int, string> ColorCode { get; private set; }

    public List<int> IsSelectedProperties { get; private set; }
}
