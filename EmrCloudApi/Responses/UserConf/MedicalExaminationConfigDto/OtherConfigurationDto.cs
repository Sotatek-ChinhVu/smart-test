using UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

namespace EmrCloudApi.Responses.UserConf.MedicalExaminationConfigDto;

public class OtherConfigurationDto
{
    public OtherConfigurationDto(OtherConfigurationOutputItem output)
    {
        DateFormatComboBox1Value = output.DateFormatComboBox1Value;
        MininumDiseaseComboboxValue = output.MininumDiseaseComboboxValue;
        DateFormatComboBox2Value = output.DateFormatComboBox2Value;
        ToolbarLocationComboboxValue = output.ToolbarLocationComboboxValue;
        ConfirmChangeCombobox1Value = output.ConfirmChangeCombobox1Value;
        ConfirmChangeCombobox2Value = output.ConfirmChangeCombobox2Value;
        WarningWhenEditComboboxValue = output.WarningWhenEditComboboxValue;
        BirthDayFormartSetting = output.BirthDayFormartSetting;
        VisitingDateFormartSetting = output.VisitingDateFormartSetting;
        SColorBackground = output.SColorBackground;
    }

    public int DateFormatComboBox1Value { get; private set; }

    public int MininumDiseaseComboboxValue { get; private set; }

    public int DateFormatComboBox2Value { get; private set; }

    public int ToolbarLocationComboboxValue { get; private set; }

    public int ConfirmChangeCombobox1Value { get; private set; }

    public int ConfirmChangeCombobox2Value { get; private set; }

    public int WarningWhenEditComboboxValue { get; private set; }

    public int BirthDayFormartSetting { get; private set; }

    public int VisitingDateFormartSetting { get; private set; }

    public string SColorBackground { get; private set; }
}
