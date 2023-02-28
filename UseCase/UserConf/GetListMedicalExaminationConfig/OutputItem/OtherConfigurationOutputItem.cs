namespace UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

public class OtherConfigurationOutputItem
{
    public OtherConfigurationOutputItem(int dateFormatComboBox1Value, int mininumDiseaseComboboxValue, int dateFormatComboBox2Value, int toolbarLocationComboboxValue, int confirmChangeCombobox1Value, int confirmChangeCombobox2Value, int warningWhenEditComboboxValue, int birthDayFormartSetting, int visitingDateFormartSetting, string sColorBackground)
    {
        DateFormatComboBox1Value = dateFormatComboBox1Value;
        MininumDiseaseComboboxValue = mininumDiseaseComboboxValue;
        DateFormatComboBox2Value = dateFormatComboBox2Value;
        ToolbarLocationComboboxValue = toolbarLocationComboboxValue;
        ConfirmChangeCombobox1Value = confirmChangeCombobox1Value;
        ConfirmChangeCombobox2Value = confirmChangeCombobox2Value;
        WarningWhenEditComboboxValue = warningWhenEditComboboxValue;
        BirthDayFormartSetting = birthDayFormartSetting;
        VisitingDateFormartSetting = visitingDateFormartSetting;
        SColorBackground = sColorBackground;
    }

    public OtherConfigurationOutputItem()
    {
        DateFormatComboBox1Value = 0;
        MininumDiseaseComboboxValue = 0;
        DateFormatComboBox2Value = 0;
        ToolbarLocationComboboxValue = 0;
        ConfirmChangeCombobox1Value = 0;
        ConfirmChangeCombobox2Value = 0;
        WarningWhenEditComboboxValue = 0;
        BirthDayFormartSetting = 0;
        VisitingDateFormartSetting = 0;
        SColorBackground = string.Empty;
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
