namespace UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

public class FunctionConfigurationOutputItem
{
    public FunctionConfigurationOutputItem(int nextOrderDisplayComboboxValue, int nextOrderUnexecutedExistComboboxValue, int patientInforComboboxValue, int watingListComboboxValue, int setEditButtonComboboxValue, int summaryEditLockComboboxValue, int actionWhenClosingSchemaValue, int rightClickComboboxValue, int pendingMedicalRecordsComboboxValue, int startKarteViewerValue, int isShowDrugUsageHistoryValue, bool isShowReservationsSetting, int puchiExternalReservationListValue, int waitingListUpdateInterval, int lastDayVisibleInPatientInfo, int lastDayVisibleInMedical, int lastDayVisibleInAccounting)
    {
        NextOrderDisplayComboboxValue = nextOrderDisplayComboboxValue;
        NextOrderUnexecutedExistComboboxValue = nextOrderUnexecutedExistComboboxValue;
        PatientInforComboboxValue = patientInforComboboxValue;
        WatingListComboboxValue = watingListComboboxValue;
        SetEditButtonComboboxValue = setEditButtonComboboxValue;
        SummaryEditLockComboboxValue = summaryEditLockComboboxValue;
        ActionWhenClosingSchemaValue = actionWhenClosingSchemaValue;
        RightClickComboboxValue = rightClickComboboxValue;
        PendingMedicalRecordsComboboxValue = pendingMedicalRecordsComboboxValue;
        StartKarteViewerValue = startKarteViewerValue;
        IsShowDrugUsageHistoryValue = isShowDrugUsageHistoryValue;
        IsShowReservationsSetting = isShowReservationsSetting;
        PuchiExternalReservationListValue = puchiExternalReservationListValue;
        WaitingListUpdateInterval = waitingListUpdateInterval;
        LastDayVisibleInPatientInfo = lastDayVisibleInPatientInfo;
        LastDayVisibleInMedical = lastDayVisibleInMedical;
        LastDayVisibleInAccounting = lastDayVisibleInAccounting;
    }

    public FunctionConfigurationOutputItem()
    {
        NextOrderDisplayComboboxValue = 0;
        NextOrderUnexecutedExistComboboxValue = 0;
        PatientInforComboboxValue = 0;
        WatingListComboboxValue = 0;
        SetEditButtonComboboxValue = 0;
        SummaryEditLockComboboxValue = 0;
        ActionWhenClosingSchemaValue = 0;
        RightClickComboboxValue = 0;
        PendingMedicalRecordsComboboxValue = 0;
        StartKarteViewerValue = 0;
        IsShowDrugUsageHistoryValue = 0;
        IsShowReservationsSetting = false;
        PuchiExternalReservationListValue = 0;
        WaitingListUpdateInterval = 0;
        LastDayVisibleInPatientInfo = 0;
        LastDayVisibleInMedical = 0;
        LastDayVisibleInAccounting = 0;
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
