using UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

namespace EmrCloudApi.Responses.UserConf.MedicalExaminationConfigDto;

public class LayoutConfigurationDto
{
    public LayoutConfigurationDto(LayoutConfigurationOutputItem output)
    {
        HistoryCount = output.HistoryCount;
        HistoryVisible = output.HistoryVisible;
        ApprovalInfoVisible = output.ApprovalInfoVisible;
        BehaviorDoButton = output.BehaviorDoButton;
        DisplayAgeValue = output.DisplayAgeValue;
        LocationOfDisease = output.LocationOfDisease;
        LocationOfFlowSheet = output.LocationOfFlowSheet;
        LayoutOfScreen = output.LayoutOfScreen;
        ToolbarVisible = output.ToolbarVisible;
        LocationOfToolbar = output.LocationOfToolbar;
        TempCulVisible = output.TempCulVisible;
        DrgPrnVisible = output.DrgPrnVisible;
        TextVisible = output.TextVisible;
        RsvVisible = output.RsvVisible;
        KanFmlyVisible = output.KanFmlyVisible;
        DrgRekiVisible = output.DrgRekiVisible;
        KenResultVisible = output.KenResultVisible;
        FillVisible = output.FillVisible;
        SanteiVisible = output.SanteiVisible;
        KarteViewVisible = output.KarteViewVisible;
        TakeVisible = output.TakeVisible;
        ExpandWaitPtListVisible = output.ExpandWaitPtListVisible;
        User1Visible = output.User1Visible;
        PalletVisible = output.PalletVisible;
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
