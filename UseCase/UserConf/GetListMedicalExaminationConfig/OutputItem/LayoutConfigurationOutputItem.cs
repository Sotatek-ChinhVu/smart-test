namespace UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

public class LayoutConfigurationOutputItem
{
    public LayoutConfigurationOutputItem(int historyCount, int historyVisible, int approvalInfoVisible, int behaviorDoButton, int displayAgeValue, int locationOfDisease, int locationOfFlowSheet, int layoutOfScreen, int toolbarVisible, int locationOfToolbar, int tempCulVisible, int drgPrnVisible, int textVisible, int rsvVisible, int kanFmlyVisible, int drgRekiVisible, int kenResultVisible, int fillVisible, int santeiVisible, int karteViewVisible, int takeVisible, int expandWaitPtListVisible, int user1Visible, int palletVisible)
    {
        HistoryCount = historyCount;
        HistoryVisible = historyVisible;
        ApprovalInfoVisible = approvalInfoVisible;
        BehaviorDoButton = behaviorDoButton;
        DisplayAgeValue = displayAgeValue;
        LocationOfDisease = locationOfDisease;
        LocationOfFlowSheet = locationOfFlowSheet;
        LayoutOfScreen = layoutOfScreen;
        ToolbarVisible = toolbarVisible;
        LocationOfToolbar = locationOfToolbar;
        TempCulVisible = tempCulVisible;
        DrgPrnVisible = drgPrnVisible;
        TextVisible = textVisible;
        RsvVisible = rsvVisible;
        KanFmlyVisible = kanFmlyVisible;
        DrgRekiVisible = drgRekiVisible;
        KenResultVisible = kenResultVisible;
        FillVisible = fillVisible;
        SanteiVisible = santeiVisible;
        KarteViewVisible = karteViewVisible;
        TakeVisible = takeVisible;
        ExpandWaitPtListVisible = expandWaitPtListVisible;
        User1Visible = user1Visible;
        PalletVisible = palletVisible;
    }

    public LayoutConfigurationOutputItem()
    {
        HistoryCount = 0;
        HistoryVisible = 0;
        ApprovalInfoVisible = 0;
        BehaviorDoButton = 0;
        DisplayAgeValue = 0;
        LocationOfDisease = 0;
        LocationOfFlowSheet = 0;
        LayoutOfScreen = 0;
        ToolbarVisible = 0;
        LocationOfToolbar = 0;
        TempCulVisible = 0;
        DrgPrnVisible = 0;
        TextVisible = 0;
        RsvVisible = 0;
        KanFmlyVisible = 0;
        DrgRekiVisible = 0;
        KenResultVisible = 0;
        FillVisible = 0;
        SanteiVisible = 0;
        KarteViewVisible = 0;
        TakeVisible = 0;
        ExpandWaitPtListVisible = 0;
        User1Visible = 0;
        PalletVisible = 0;
    }

    public int HistoryCount { get; private set; }

    public int HistoryVisible { get; private set; }

    public int ApprovalInfoVisible { get; private set; }

    public int BehaviorDoButton { get; private set; }

    public int DisplayAgeValue { get; private set; }

    public int LocationOfDisease { get; private set; }

    public int LocationOfFlowSheet { get; private set; }

    public int LayoutOfScreen { get; private set; }

    public int ToolbarVisible { get; private set; }

    public int LocationOfToolbar { get; private set; }

    public int TempCulVisible { get; private set; }

    public int DrgPrnVisible { get; private set; }

    public int TextVisible { get; private set; }

    public int RsvVisible { get; private set; }

    public int KanFmlyVisible { get; private set; }

    public int DrgRekiVisible { get; private set; }

    public int KenResultVisible { get; private set; }

    public int FillVisible { get; private set; }

    public int SanteiVisible { get; private set; }

    public int KarteViewVisible { get; private set; }

    public int TakeVisible { get; private set; }

    public int ExpandWaitPtListVisible { get; private set; }

    public int User1Visible { get; private set; }

    public int PalletVisible { get; private set; }
}
