using UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

namespace EmrCloudApi.Responses.UserConf.MedicalExaminationConfigDto;

public class ConfigCheckboxSubDto
{
    public ConfigCheckboxSubDto(ConfigCheckboxSubItem output)
    {
        IsCheckedFirstCharParam = output.IsCheckedFirstCharParam;
        IsCheckedSecondCharParam = output.IsCheckedSecondCharParam;
        IsCheckedThirdCharParam = output.IsCheckedThirdCharParam;
        IsCheckedFourthCharParam = output.IsCheckedFourthCharParam;
        IsCheckedFifthCharParam = output.IsCheckedFifthCharParam;
    }

    public bool IsCheckedFirstCharParam { get; private set; }

    public bool IsCheckedSecondCharParam { get; private set; }

    public bool IsCheckedThirdCharParam { get; private set; }

    public bool IsCheckedFourthCharParam { get; private set; }

    public bool IsCheckedFifthCharParam { get; private set; }
}
