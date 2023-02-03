namespace UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

public class SaveConfirmationConfigurationOutputItem
{
    public SaveConfirmationConfigurationOutputItem(int claimSagakuComboboxConfig, int claimSagakuAtReceTimeConfig, int noteScreenDisplayComboboxConfig, ConfigCheckboxSubItem commentCheckConfig, ConfigCheckboxSubItem santeiCheckConfig, ConfigCheckboxSubItem inputCheckConfig, ConfigCheckboxSubItem kubunCheckConfig, ConfigCheckboxSubItem reportCheckConfig, ConfigCheckboxSubItem tenkeiByomeConfig)
    {
        ClaimSagakuComboboxConfig = claimSagakuComboboxConfig;
        ClaimSagakuAtReceTimeConfig = claimSagakuAtReceTimeConfig;
        NoteScreenDisplayComboboxConfig = noteScreenDisplayComboboxConfig;
        CommentCheckConfig = commentCheckConfig;
        SanteiCheckConfig = santeiCheckConfig;
        InputCheckConfig = inputCheckConfig;
        KubunCheckConfig = kubunCheckConfig;
        ReportCheckConfig = reportCheckConfig;
        TenkeiByomeConfig = tenkeiByomeConfig;
    }

    public SaveConfirmationConfigurationOutputItem()
    {
        ClaimSagakuComboboxConfig = 0;
        ClaimSagakuAtReceTimeConfig = 0;
        NoteScreenDisplayComboboxConfig = 0;
        CommentCheckConfig = new();
        SanteiCheckConfig = new();
        InputCheckConfig = new();
        KubunCheckConfig = new();
        ReportCheckConfig = new();
        TenkeiByomeConfig = new();
    }

    public int ClaimSagakuComboboxConfig { get; private set; }

    public int ClaimSagakuAtReceTimeConfig { get; private set; }

    public int NoteScreenDisplayComboboxConfig { get; private set; }

    public ConfigCheckboxSubItem CommentCheckConfig { get; private set; }

    public ConfigCheckboxSubItem SanteiCheckConfig { get; private set; }

    public ConfigCheckboxSubItem InputCheckConfig { get; private set; }

    public ConfigCheckboxSubItem KubunCheckConfig { get; private set; }

    public ConfigCheckboxSubItem ReportCheckConfig { get; private set; }

    public ConfigCheckboxSubItem TenkeiByomeConfig { get; private set; }
}


