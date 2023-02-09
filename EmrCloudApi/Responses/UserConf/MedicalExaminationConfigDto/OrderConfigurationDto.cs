using UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

namespace EmrCloudApi.Responses.UserConf.MedicalExaminationConfigDto;

public class OrderConfigurationDto
{
    public OrderConfigurationDto(OrderConfigurationOutputItem output)
    {
        SetNameCheckboxValue = output.SetNameCheckboxValue;
        InputDateCheckboxValue = output.InputDateCheckboxValue;
        EntererCheckboxValue = output.EntererCheckboxValue;
        DrugPriceCheckboxValue = output.DrugPriceCheckboxValue;
        FontNameComboboxValue = output.FontNameComboboxValue;
        FontSizeValue = output.FontSizeValue;
        StringCopyComboboxValue = output.StringCopyComboboxValue;
        HistoryAllCheckboxValue = output.HistoryAllCheckboxValue;
        HistorySetNameOnlyCheckboxValue = output.HistorySetNameOnlyCheckboxValue;
        HistoryTeachingCheckboxValue = output.HistoryTeachingCheckboxValue;
        HistoryStayHomeCheckboxValue = output.HistoryStayHomeCheckboxValue;
        HistoryPrescriptionCheckboxValue = output.HistoryPrescriptionCheckboxValue;
        HistoryInjectionCheckboxValue = output.HistoryInjectionCheckboxValue;
        HistoryTreatmentCheckboxValue = output.HistoryTreatmentCheckboxValue;
        HistorySurgeryCheckboxValue = output.HistorySurgeryCheckboxValue;
        HistoryInspectionCheckboxValue = output.HistoryInspectionCheckboxValue;
        HistoryImageCheckboxValue = output.HistoryImageCheckboxValue;
        HistoryOtherCheckboxValue = output.HistoryOtherCheckboxValue;
        HistoryOwnCostCheckboxValue = output.HistoryOwnCostCheckboxValue;
        TodayAllCheckboxValue = output.TodayAllCheckboxValue;
        TodaySetNameOnlyCheckboxValue = output.TodaySetNameOnlyCheckboxValue;
        TodayTeachingCheckboxValue = output.TodayTeachingCheckboxValue;
        TodayStayHomeCheckboxValue = output.TodayStayHomeCheckboxValue;
        TodayPrescriptionCheckboxValue = output.TodayPrescriptionCheckboxValue;
        TodayInjectionCheckboxValue = output.TodayInjectionCheckboxValue;
        TodayTreatmentCheckboxValue = output.TodayTreatmentCheckboxValue;
        TodaySurgeryCheckboxValue = output.TodaySurgeryCheckboxValue;
        TodayInspectionCheckboxValue = output.TodayInspectionCheckboxValue;
        TodayImageCheckboxValue = output.TodayImageCheckboxValue;
        TodayOtherCheckboxValue = output.TodayOtherCheckboxValue;
        TodayOwnCostCheckboxValue = output.TodayOwnCostCheckboxValue;
        OrderSheetTeachingCheckboxValue = output.OrderSheetTeachingCheckboxValue;
        OrderSheetStayHomeCheckboxValue = output.OrderSheetStayHomeCheckboxValue;
        OrderSheetPrescriptionCheckboxValue = output.OrderSheetPrescriptionCheckboxValue;
        OrderSheetInternalMedicineCheckboxValue = output.OrderSheetInternalMedicineCheckboxValue;
        OrderSheetClothingCheckboxValue = output.OrderSheetClothingCheckboxValue;
        OrderSheetExternalUseCheckboxValue = output.OrderSheetExternalUseCheckboxValue;
        OrderSheetOther1CheckboxValue = output.OrderSheetOther1CheckboxValue;
        OrderSheetInjectionCheckboxValue = output.OrderSheetInjectionCheckboxValue;
        OrderSheetMuscleInjectionCheckboxValue = output.OrderSheetMuscleInjectionCheckboxValue;
        OrderSheetIntravenousInjectionCheckboxValue = output.OrderSheetIntravenousInjectionCheckboxValue;
        InfusionInjectionCheckboxValue = output.InfusionInjectionCheckboxValue;
        OtherNotesInjectionCheckboxValue = output.OtherNotesInjectionCheckboxValue;
        OrderSheetTreatmentCheckboxValue = output.OrderSheetTreatmentCheckboxValue;
        OrderSheetSurgeryCheckboxValue = output.OrderSheetSurgeryCheckboxValue;
        OrderSheetInspectionCheckboxValue = output.OrderSheetInspectionCheckboxValue;
        OrderSheetSampleCheckboxValue = output.OrderSheetSampleCheckboxValue;
        OrderSheetLivingBodyCheckboxValue = output.OrderSheetLivingBodyCheckboxValue;
        OrderSheetOther3CheckboxValue = output.OrderSheetOther3CheckboxValue;
        OrderSheetImageCheckboxValue = output.OrderSheetImageCheckboxValue;
        OrderSheetOther4CheckboxValue = output.OrderSheetOther4CheckboxValue;
        OrderSheetOwnCostCheckboxValue = output.OrderSheetOwnCostCheckboxValue;
        InitialSelectionComboboxValue = output.InitialSelectionComboboxValue;
        DisplayDaysValue = output.DisplayDaysValue;
    }

    public int SetNameCheckboxValue { get; private set; }

    public int InputDateCheckboxValue { get; private set; }

    public int EntererCheckboxValue { get; private set; }

    public int DrugPriceCheckboxValue { get; private set; }

    public string FontNameComboboxValue { get; private set; }

    public int FontSizeValue { get; private set; }

    public int StringCopyComboboxValue { get; private set; }

    public int HistoryAllCheckboxValue { get; private set; }

    public int HistorySetNameOnlyCheckboxValue { get; private set; }

    public int HistoryTeachingCheckboxValue { get; private set; }

    public int HistoryStayHomeCheckboxValue { get; private set; }

    public int HistoryPrescriptionCheckboxValue { get; private set; }

    public int HistoryInjectionCheckboxValue { get; private set; }

    public int HistoryTreatmentCheckboxValue { get; private set; }

    public int HistorySurgeryCheckboxValue { get; private set; }

    public int HistoryInspectionCheckboxValue { get; private set; }

    public int HistoryImageCheckboxValue { get; private set; }

    public int HistoryOtherCheckboxValue { get; private set; }

    public int HistoryOwnCostCheckboxValue { get; private set; }

    public int TodayAllCheckboxValue { get; private set; }

    public int TodaySetNameOnlyCheckboxValue { get; private set; }

    public int TodayTeachingCheckboxValue { get; private set; }

    public int TodayStayHomeCheckboxValue { get; private set; }

    public int TodayPrescriptionCheckboxValue { get; private set; }

    public int TodayInjectionCheckboxValue { get; private set; }

    public int TodayTreatmentCheckboxValue { get; private set; }

    public int TodaySurgeryCheckboxValue { get; private set; }

    public int TodayInspectionCheckboxValue { get; private set; }

    public int TodayImageCheckboxValue { get; private set; }

    public int TodayOtherCheckboxValue { get; private set; }

    public int TodayOwnCostCheckboxValue { get; private set; }

    public int OrderSheetTeachingCheckboxValue { get; private set; }

    public int OrderSheetStayHomeCheckboxValue { get; private set; }

    public int OrderSheetPrescriptionCheckboxValue { get; private set; }

    public int OrderSheetInternalMedicineCheckboxValue { get; private set; }

    public int OrderSheetClothingCheckboxValue { get; private set; }

    public int OrderSheetExternalUseCheckboxValue { get; private set; }

    public int OrderSheetOther1CheckboxValue { get; private set; }

    public int OrderSheetInjectionCheckboxValue { get; private set; }

    public int OrderSheetMuscleInjectionCheckboxValue { get; private set; }

    public int OrderSheetIntravenousInjectionCheckboxValue { get; private set; }

    public int InfusionInjectionCheckboxValue { get; private set; }

    public int OtherNotesInjectionCheckboxValue { get; private set; }

    public int OrderSheetTreatmentCheckboxValue { get; private set; }

    public int OrderSheetSurgeryCheckboxValue { get; private set; }

    public int OrderSheetInspectionCheckboxValue { get; private set; }

    public int OrderSheetSampleCheckboxValue { get; private set; }

    public int OrderSheetLivingBodyCheckboxValue { get; private set; }

    public int OrderSheetOther3CheckboxValue { get; private set; }

    public int OrderSheetImageCheckboxValue { get; private set; }

    public int OrderSheetOther4CheckboxValue { get; private set; }

    public int OrderSheetOwnCostCheckboxValue { get; private set; }

    public int InitialSelectionComboboxValue { get; private set; }

    public int DisplayDaysValue { get; private set; }
}
