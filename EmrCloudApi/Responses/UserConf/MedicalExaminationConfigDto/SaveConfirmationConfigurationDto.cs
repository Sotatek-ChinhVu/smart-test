using UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

namespace EmrCloudApi.Responses.UserConf.MedicalExaminationConfigDto;

public class SaveConfirmationConfigurationDto
{
    public SaveConfirmationConfigurationDto(SaveConfirmationConfigurationOutputItem output)
    {
        ClaimSagakuComboboxConfig = output.ClaimSagakuComboboxConfig;
        ClaimSagakuAtReceTimeConfig = output.ClaimSagakuAtReceTimeConfig;
        NoteScreenDisplayComboboxConfig = output.NoteScreenDisplayComboboxConfig;
        CommentCheckConfig = new ConfigCheckboxSubDto(output.CommentCheckConfig);
        SanteiCheckConfig = new ConfigCheckboxSubDto(output.SanteiCheckConfig);
        InputCheckConfig = new ConfigCheckboxSubDto(output.InputCheckConfig);
        KubunCheckConfig = new ConfigCheckboxSubDto(output.KubunCheckConfig);
        ReportCheckConfig = new ConfigCheckboxSubDto(output.ReportCheckConfig);
        TenkeiByomeConfig = new ConfigCheckboxSubDto(output.TenkeiByomeConfig);
    }

    public int ClaimSagakuComboboxConfig { get; private set; }

    public int ClaimSagakuAtReceTimeConfig { get; private set; }

    public int NoteScreenDisplayComboboxConfig { get; private set; }

    public ConfigCheckboxSubDto CommentCheckConfig { get; private set; }

    public ConfigCheckboxSubDto SanteiCheckConfig { get; private set; }

    public ConfigCheckboxSubDto InputCheckConfig { get; private set; }

    public ConfigCheckboxSubDto KubunCheckConfig { get; private set; }

    public ConfigCheckboxSubDto ReportCheckConfig { get; private set; }

    public ConfigCheckboxSubDto TenkeiByomeConfig { get; private set; }
}
