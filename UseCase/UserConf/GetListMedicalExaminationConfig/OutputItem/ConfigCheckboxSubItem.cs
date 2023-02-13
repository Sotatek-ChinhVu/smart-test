namespace UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

public class ConfigCheckboxSubItem
{
    public ConfigCheckboxSubItem(bool isCheckedFirstCharParam, bool isCheckedSecondCharParam, bool isCheckedThirdCharParam, bool isCheckedFourthCharParam, bool isCheckedFifthCharParam)
    {
        IsCheckedFirstCharParam = isCheckedFirstCharParam;
        IsCheckedSecondCharParam = isCheckedSecondCharParam;
        IsCheckedThirdCharParam = isCheckedThirdCharParam;
        IsCheckedFourthCharParam = isCheckedFourthCharParam;
        IsCheckedFifthCharParam = isCheckedFifthCharParam;
    }

    public ConfigCheckboxSubItem()
    {
        IsCheckedFirstCharParam = false;
        IsCheckedSecondCharParam = false;
        IsCheckedThirdCharParam = false;
        IsCheckedFourthCharParam = false;
        IsCheckedFifthCharParam = false;
    }

    public bool IsCheckedFirstCharParam { get; private set; }

    public bool IsCheckedSecondCharParam { get; private set; }

    public bool IsCheckedThirdCharParam { get; private set; }

    public bool IsCheckedFourthCharParam { get; private set; }

    public bool IsCheckedFifthCharParam { get; private set; }
}
