using UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

namespace EmrCloudApi.Responses.UserConf.MedicalExaminationConfigDto;

public class FunctionConfigurationDto
{
    public FunctionConfigurationDto(FunctionConfigurationOutputItem output)
    {
        NextOrderDisplayComboboxValue = output.NextOrderDisplayComboboxValue;
        NextOrderUnexecutedExistComboboxValue = output.NextOrderUnexecutedExistComboboxValue;
        PatientInforComboboxValue = output.PatientInforComboboxValue;
        WatingListComboboxValue = output.WatingListComboboxValue;
        SetEditButtonComboboxValue = output.SetEditButtonComboboxValue;
        SummaryEditLockComboboxValue = output.SummaryEditLockComboboxValue;
        ActionWhenClosingSchemaValue = output.ActionWhenClosingSchemaValue;
        RightClickComboboxValue = output.RightClickComboboxValue;
        PendingMedicalRecordsComboboxValue = output.PendingMedicalRecordsComboboxValue;
        StartKarteViewerValue = output.StartKarteViewerValue;
        IsShowDrugUsageHistoryValue = output.IsShowDrugUsageHistoryValue;
        IsShowReservationsSetting = output.IsShowReservationsSetting;
        PuchiExternalReservationListValue = output.PuchiExternalReservationListValue;
        WaitingListUpdateInterval = output.WaitingListUpdateInterval;
        LastDayVisibleInPatientInfo = output.LastDayVisibleInPatientInfo;
        LastDayVisibleInMedical = output.LastDayVisibleInMedical;
        LastDayVisibleInAccounting = output.LastDayVisibleInAccounting;
    }

    public int NextOrderDisplayComboboxValue { get; private set; }

    public int NextOrderUnexecutedExistComboboxValue { get; private set; }

    public int PatientInforComboboxValue { get; private set; }

    public int WatingListComboboxValue { get; private set; }

    public int SetEditButtonComboboxValue { get; private set; }

    public int SummaryEditLockComboboxValue { get; private set; }

    public int ActionWhenClosingSchemaValue { get; private set; }

    public int RightClickComboboxValue { get; private set; }

    public int PendingMedicalRecordsComboboxValue { get; private set; }

    public int StartKarteViewerValue { get; private set; }

    public int IsShowDrugUsageHistoryValue { get; private set; }

    public bool IsShowReservationsSetting { get; private set; }

    public int PuchiExternalReservationListValue { get; private set; }

    public int WaitingListUpdateInterval { get; private set; }

    public int LastDayVisibleInPatientInfo { get; private set; }

    public int LastDayVisibleInMedical { get; private set; }

    public int LastDayVisibleInAccounting { get; private set; }
}
