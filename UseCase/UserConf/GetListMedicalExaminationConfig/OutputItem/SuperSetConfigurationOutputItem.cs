namespace UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

public class SuperSetConfigurationOutputItem
{
    public SuperSetConfigurationOutputItem(int dropByomeiComboboxValue, int styleComboboxValue, int doubleClickComboBoxValue, int edaNoSelectedValue, int setKbnSelectedValue, int autoShowSelectedValue, int isTopMostSelectedValue, int closeSelectedValue, int positionSelectedValue, int columnNumber, int columnHeight, int columnMargin, int columnFontSize, bool doOrderChecked, bool doKarteChecked, bool doByomeiChecked, Dictionary<int, bool> kbnMstCheckBox)
    {
        DropByomeiComboboxValue = dropByomeiComboboxValue;
        StyleComboboxValue = styleComboboxValue;
        DoubleClickComboBoxValue = doubleClickComboBoxValue;
        EdaNoSelectedValue = edaNoSelectedValue;
        SetKbnSelectedValue = setKbnSelectedValue;
        AutoShowSelectedValue = autoShowSelectedValue;
        IsTopMostSelectedValue = isTopMostSelectedValue;
        CloseSelectedValue = closeSelectedValue;
        PositionSelectedValue = positionSelectedValue;
        ColumnNumber = columnNumber;
        ColumnHeight = columnHeight;
        ColumnMargin = columnMargin;
        ColumnFontSize = columnFontSize;
        DoOrderChecked = doOrderChecked;
        DoKarteChecked = doKarteChecked;
        DoByomeiChecked = doByomeiChecked;
        KbnMstCheckBox = kbnMstCheckBox;
    }

    public SuperSetConfigurationOutputItem()
    {
        DropByomeiComboboxValue = 0;
        StyleComboboxValue = 0;
        DoubleClickComboBoxValue = 0;
        EdaNoSelectedValue = 0;
        SetKbnSelectedValue = 0;
        AutoShowSelectedValue = 0;
        IsTopMostSelectedValue = 0;
        CloseSelectedValue = 0;
        PositionSelectedValue = 0;
        ColumnNumber = 0;
        ColumnHeight = 0;
        ColumnMargin = 0;
        ColumnFontSize = 0;
        DoOrderChecked = false;
        DoKarteChecked = false;
        DoByomeiChecked = false;
        KbnMstCheckBox = new();
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
