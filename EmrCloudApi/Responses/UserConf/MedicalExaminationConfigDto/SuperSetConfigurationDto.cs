using UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

namespace EmrCloudApi.Responses.UserConf.MedicalExaminationConfigDto;

public class SuperSetConfigurationDto
{
    public SuperSetConfigurationDto(SuperSetConfigurationOutputItem output)
    {
        DropByomeiComboboxValue = output.DropByomeiComboboxValue;
        StyleComboboxValue = output.StyleComboboxValue;
        DoubleClickComboBoxValue = output.DoubleClickComboBoxValue;
        EdaNoSelectedValue = output.EdaNoSelectedValue;
        SetKbnSelectedValue = output.SetKbnSelectedValue;
        AutoShowSelectedValue = output.AutoShowSelectedValue;
        IsTopMostSelectedValue = output.IsTopMostSelectedValue;
        CloseSelectedValue = output.CloseSelectedValue;
        PositionSelectedValue = output.PositionSelectedValue;
        ColumnNumber = output.ColumnNumber;
        ColumnHeight = output.ColumnHeight;
        ColumnMargin = output.ColumnMargin;
        ColumnFontSize = output.ColumnFontSize;
        DoOrderChecked = output.DoOrderChecked;
        DoKarteChecked = output.DoKarteChecked;
        DoByomeiChecked = output.DoByomeiChecked;
        KbnMstCheckBox = output.KbnMstCheckBox;
    }

    public int DropByomeiComboboxValue { get; private set; }

    public int StyleComboboxValue { get; private set; }

    public int DoubleClickComboBoxValue { get; private set; }

    public int EdaNoSelectedValue { get; private set; }

    public int SetKbnSelectedValue { get; private set; }

    public int AutoShowSelectedValue { get; private set; }

    public int IsTopMostSelectedValue { get; private set; }

    public int CloseSelectedValue { get; private set; }

    public int PositionSelectedValue { get; private set; }

    public int ColumnNumber { get; private set; }

    public int ColumnHeight { get; private set; }

    public int ColumnMargin { get; private set; }

    public int ColumnFontSize { get; private set; }

    public bool DoOrderChecked { get; private set; }

    public bool DoKarteChecked { get; private set; }

    public bool DoByomeiChecked { get; private set; }

    public Dictionary<int, bool> KbnMstCheckBox { get; private set; }
}
