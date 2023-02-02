using Domain.Models.JsonSetting;
using Domain.Models.SetKbnMst;
using Domain.Models.SystemConf;
using Domain.Models.UserConf;
using Helper.Extension;
using UseCase.UserConf.GetListMedicalExaminationConfig;
using UseCase.UserConf.GetListMedicalExaminationConfig.OutputItem;

namespace Interactor.UserConf;

public class GetListMedicalExaminationConfigInteractor : IGetListMedicalExaminationConfigInputPort
{
    private readonly IUserConfRepository _userConfRepository;
    private readonly IJsonSettingRepository _jsonSettingRepository;
    private readonly ISystemConfRepository _systemConfRepository;

    public GetListMedicalExaminationConfigInteractor(IUserConfRepository userConfRepository, IJsonSettingRepository jsonSettingRepository, ISystemConfRepository systemConfRepository)
    {
        _userConfRepository = userConfRepository;
        _jsonSettingRepository = jsonSettingRepository;
        _systemConfRepository = systemConfRepository;
    }

    public GetListMedicalExaminationConfigOutputData Handle(GetListMedicalExaminationConfigInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetListMedicalExaminationConfigOutputData(GetListMedicalExaminationConfigStatus.InvalidHpId);
            }
            else if (inputData.UserId <= 0)
            {
                return new GetListMedicalExaminationConfigOutputData(GetListMedicalExaminationConfigStatus.InvalidUserId);
            }
            List<int> grpCodes = new();
            grpCodes.AddRange(new List<int>() { 2, 4, 5, 6, 7, 8, 11, 12, 16, 18, 209 }); //layoutConfiguration
            grpCodes.AddRange(new List<int>() { 202, 201, 203, 205, 204, 206, 207, 208 }); //orderConfiguration
            grpCodes.AddRange(new List<int>() { 99, 101, 104, 103, 908, 105 }); //karteConfiguration
            grpCodes.AddRange(new List<int>() { 910, 911, 912 }); //headerConfiguration
            grpCodes.AddRange(new List<int>() { 913, 901, 914, 924, 927 }); //summaryConfiguration
            grpCodes.AddRange(new List<int>() { 10, 3, 902, 922, 923, 906, 907, 14, 919, 903, 9, 905, 15, 921, 928, 19 }); //functionConfiguration
            grpCodes.AddRange(new List<int>() { 921, 922, 919, 923 }); //saveConfirmationConfiguration
            grpCodes.AddRange(new List<int>() { 301, 302, 303, 304, 305, 306, 307, 308, 309, 310 }); //superSetConfiguration
            grpCodes.AddRange(new List<int>() { 1, 13, 909, 100001, 100002, 100004, 100005, 100006, 100011, 100012 }); //otherConfiguration

            var listUserConfig = _userConfRepository.GetList(inputData.HpId, inputData.UserId, grpCodes);

            var layoutConfiguration = ConvertToLayoutConfiguration(listUserConfig);
            var orderConfiguration = ConvertToOrderConfiguration(listUserConfig);
            var karteConfiguration = ConvertToKarteConfiguration(listUserConfig);
            var headerConfiguration = ConvertToHeaderConfiguration(listUserConfig);
            var summaryConfiguration = ConvertToSummaryConfiguration(listUserConfig);
            var functionConfiguration = ConvertToFunctionConfiguration(inputData.HpId, inputData.UserId, listUserConfig);
            var saveConfirmationConfiguration = ConvertToSaveConfirmationConfiguration(listUserConfig);
            var superSetConfiguration = ConvertToSuperSetConfiguration(listUserConfig);
            var otherConfiguration = ConvertToOtherConfiguration(listUserConfig);

            return new GetListMedicalExaminationConfigOutputData(
                                                                    GetListMedicalExaminationConfigStatus.Successed,
                                                                    layoutConfiguration,
                                                                    orderConfiguration,
                                                                    karteConfiguration,
                                                                    headerConfiguration,
                                                                    summaryConfiguration,
                                                                    functionConfiguration,
                                                                    saveConfirmationConfiguration,
                                                                    superSetConfiguration,
                                                                    otherConfiguration
                                                                );
        }
        finally
        {
            _userConfRepository.ReleaseResource();
            _jsonSettingRepository.ReleaseResource();
            _systemConfRepository.ReleaseResource();
        }
    }

    private LayoutConfigurationOutputItem ConvertToLayoutConfiguration(List<UserConfModel> listUserConfig)
    {
        int historyCount = GetConfigModel(listUserConfig, 5);
        int historyVisible = GetConfigModel(listUserConfig, 4);
        int approvalInfoVisible = GetConfigModel(listUserConfig, 209);
        int behaviorDoButton = GetConfigModel(listUserConfig, 16);
        int displayAgeValue = GetConfigModel(listUserConfig, 18);
        int locationOfDisease = GetConfigModel(listUserConfig, 11);
        int locationOfFlowSheet = GetConfigModel(listUserConfig, 12);
        int layoutOfScreen = GetConfigModel(listUserConfig, 2);
        int toolbarVisible = GetConfigModel(listUserConfig, 7);
        int locationOfToolbar = GetConfigModel(listUserConfig, 6);
        int tempCulVisible = GetConfigModel(listUserConfig, 8, 1);
        int drgPrnVisible = GetConfigModel(listUserConfig, 8, 2);
        int textVisible = GetConfigModel(listUserConfig, 8, 3);
        int rsvVisible = GetConfigModel(listUserConfig, 8, 4);
        int kanFmlyVisible = GetConfigModel(listUserConfig, 8, 5);
        int drgRekiVisible = GetConfigModel(listUserConfig, 8, 6);
        int kenResultVisible = GetConfigModel(listUserConfig, 8, 7);
        int fillVisible = GetConfigModel(listUserConfig, 8, 8);
        int santeiVisible = GetConfigModel(listUserConfig, 8, 9);
        int karteViewVisible = GetConfigModel(listUserConfig, 8, 10);
        int takeVisible = GetConfigModel(listUserConfig, 8, 11);
        int expandWaitPtListVisible = GetConfigModel(listUserConfig, 8, 12);
        int user1Visible = GetConfigModel(listUserConfig, 8, 99);
        int palletVisible = GetConfigModel(listUserConfig, 8, 13);

        return new LayoutConfigurationOutputItem(
                                                    historyCount,
                                                    historyVisible,
                                                    approvalInfoVisible,
                                                    behaviorDoButton,
                                                    displayAgeValue,
                                                    locationOfDisease,
                                                    locationOfFlowSheet,
                                                    layoutOfScreen,
                                                    toolbarVisible,
                                                    locationOfToolbar,
                                                    tempCulVisible,
                                                    drgPrnVisible,
                                                    textVisible,
                                                    rsvVisible,
                                                    kanFmlyVisible,
                                                    drgRekiVisible,
                                                    kenResultVisible,
                                                    fillVisible,
                                                    santeiVisible,
                                                    karteViewVisible,
                                                    takeVisible,
                                                    expandWaitPtListVisible,
                                                    user1Visible,
                                                    palletVisible
                                                 );
    }

    private OrderConfigurationOutputItem ConvertToOrderConfiguration(List<UserConfModel> listUserConfig)
    {
        #region Hard Code
        int _attributeDisplayInformation = 202;
        int _setNameCheckbox = 2;
        int _entererCheckbox = 3;
        int _inputDateCheckbox = 4;
        int _drugPriceCheckbox = 5;

        int _fontNameandSize = 201;
        int _stringCopy = 203;

        int _detailDisplayHistory = 205;
        int _detailDisplayToday = 204;
        int _all = 1;
        int _setNameOnly = 2;
        int _teaching = 3;
        int _stayHome = 4;
        int _prescription = 5;
        int _injection = 6;
        int _treatment = 7;
        int _surgery = 8;
        int _inspection = 9;
        int _image = 10;
        int _other = 11;
        int _ownCost = 12;

        int _orderSheet = 206;
        int _orderSheetTeaching = 1;//医学管理
        int _orderSheetStayHome = 2;//在宅
        int _orderSheetPrescription = 3;//処方
        int _orderSheetInternalMedicine = 4;//（処方）内服
        int _orderSheetClothing = 5;//（処方）頓服
        int _orderSheetExternalUse = 6;//（処方）外用
        int _orderSheetOther1 = 7;//（処方）その他
        int _orderSheetInjection = 8;//注射
        int _orderSheetMuscleInjection = 9;//（注射）筋注
        int _orderSheetIntravenousInjection = 10;//（注射）静注
        int _infusionInjection = 11;//（注射）点滴
        int _otherNotesInjection = 12;//（注射）他注
        int _orderSheetTreatment = 13;//処置
        int _orderSheetSurgery = 14;//手術
        int _orderSheetInspection = 15;//検査
        int _orderSheetSample = 16;//（検査）検体
        int _orderSheetLivingBody = 17;//（検査）生体
        int _orderSheetOther3 = 18;//（検査）その他
        int _orderSheetImage = 19;//画像
        int _orderSheetOther4 = 20;//その他
        int _orderSheetOwnCost = 21;//自費

        int _initialSelection = 207;
        int _displayDays = 208;
        #endregion

        int setNameCheckboxValue = GetConfigModel(listUserConfig, _attributeDisplayInformation, _setNameCheckbox);
        int inputDateCheckboxValue = GetConfigModel(listUserConfig, _attributeDisplayInformation, _inputDateCheckbox);
        int entererCheckboxValue = GetConfigModel(listUserConfig, _attributeDisplayInformation, _entererCheckbox);
        int drugPriceCheckboxValue = GetConfigModel(listUserConfig, _attributeDisplayInformation, _drugPriceCheckbox);

        var configFontNameCombobox = listUserConfig.FirstOrDefault(item => item.GrpCd == 201);
        string fontNameComboboxValue = configFontNameCombobox != null ? configFontNameCombobox.Param : "Meiryo UI";

        int fontSizeValue = GetConfigModel(listUserConfig, _fontNameandSize, 0);
        int stringCopyComboboxValue = GetConfigModel(listUserConfig, _stringCopy);
        int historyAllCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayHistory, _all);
        int historySetNameOnlyCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayHistory, _setNameOnly);
        int historyTeachingCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayHistory, _teaching);
        int historyStayHomeCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayHistory, _stayHome);
        int historyPrescriptionCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayHistory, _prescription);
        int historyInjectionCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayHistory, _injection);
        int historyTreatmentCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayHistory, _treatment);
        int historySurgeryCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayHistory, _surgery);
        int historyInspectionCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayHistory, _inspection);
        int historyImageCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayHistory, _image);
        int historyOtherCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayHistory, _other);
        int historyOwnCostCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayHistory, _ownCost);
        int todayAllCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayToday, _all);
        int todaySetNameOnlyCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayToday, _setNameOnly);
        int todayTeachingCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayToday, _teaching);
        int todayStayHomeCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayToday, _stayHome);
        int todayPrescriptionCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayToday, _prescription);
        int todayInjectionCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayToday, _injection);
        int todayTreatmentCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayToday, _treatment);
        int todaySurgeryCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayToday, _surgery);
        int todayInspectionCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayToday, _inspection);
        int todayImageCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayToday, _image);
        int todayOtherCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayToday, _other);
        int todayOwnCostCheckboxValue = GetConfigModel(listUserConfig, _detailDisplayToday, _ownCost);
        int orderSheetTeachingCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetTeaching);
        int orderSheetStayHomeCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetStayHome);
        int orderSheetPrescriptionCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetPrescription);
        int orderSheetInternalMedicineCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetInternalMedicine);
        int orderSheetClothingCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetClothing);
        int orderSheetExternalUseCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetExternalUse);
        int orderSheetOther1CheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetOther1);
        int orderSheetInjectionCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetInjection);
        int orderSheetMuscleInjectionCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetMuscleInjection);
        int orderSheetIntravenousInjectionCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetIntravenousInjection);
        int infusionInjectionCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _infusionInjection);
        int otherNotesInjectionCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _otherNotesInjection);
        int orderSheetTreatmentCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetTreatment);
        int orderSheetSurgeryCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetSurgery);
        int orderSheetInspectionCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetInspection);
        int orderSheetSampleCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetSample);
        int orderSheetLivingBodyCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetLivingBody);
        int orderSheetOther3CheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetOther3);
        int orderSheetImageCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetImage);
        int orderSheetOther4CheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetOther4);
        int orderSheetOwnCostCheckboxValue = GetConfigModel(listUserConfig, _orderSheet, _orderSheetOwnCost);
        int initialSelectionComboboxValue = GetConfigModel(listUserConfig, _initialSelection);
        int displayDaysValue = GetConfigModel(listUserConfig, _displayDays, 0);

        return new OrderConfigurationOutputItem(
                                                    setNameCheckboxValue,
                                                    inputDateCheckboxValue,
                                                    entererCheckboxValue,
                                                    drugPriceCheckboxValue,
                                                    fontNameComboboxValue,
                                                    fontSizeValue,
                                                    stringCopyComboboxValue,
                                                    historyAllCheckboxValue,
                                                    historySetNameOnlyCheckboxValue,
                                                    historyTeachingCheckboxValue,
                                                    historyStayHomeCheckboxValue,
                                                    historyPrescriptionCheckboxValue,
                                                    historyInjectionCheckboxValue,
                                                    historyTreatmentCheckboxValue,
                                                    historySurgeryCheckboxValue,
                                                    historyInspectionCheckboxValue,
                                                    historyImageCheckboxValue,
                                                    historyOtherCheckboxValue,
                                                    historyOwnCostCheckboxValue,
                                                    todayAllCheckboxValue,
                                                    todaySetNameOnlyCheckboxValue,
                                                    todayTeachingCheckboxValue,
                                                    todayStayHomeCheckboxValue,
                                                    todayPrescriptionCheckboxValue,
                                                    todayInjectionCheckboxValue,
                                                    todayTreatmentCheckboxValue,
                                                    todaySurgeryCheckboxValue,
                                                    todayInspectionCheckboxValue,
                                                    todayImageCheckboxValue,
                                                    todayOtherCheckboxValue,
                                                    todayOwnCostCheckboxValue,
                                                    orderSheetTeachingCheckboxValue,
                                                    orderSheetStayHomeCheckboxValue,
                                                    orderSheetPrescriptionCheckboxValue,
                                                    orderSheetInternalMedicineCheckboxValue,
                                                    orderSheetClothingCheckboxValue,
                                                    orderSheetExternalUseCheckboxValue,
                                                    orderSheetOther1CheckboxValue,
                                                    orderSheetInjectionCheckboxValue,
                                                    orderSheetMuscleInjectionCheckboxValue,
                                                    orderSheetIntravenousInjectionCheckboxValue,
                                                    infusionInjectionCheckboxValue,
                                                    otherNotesInjectionCheckboxValue,
                                                    orderSheetTreatmentCheckboxValue,
                                                    orderSheetSurgeryCheckboxValue,
                                                    orderSheetInspectionCheckboxValue,
                                                    orderSheetSampleCheckboxValue,
                                                    orderSheetLivingBodyCheckboxValue,
                                                    orderSheetOther3CheckboxValue,
                                                    orderSheetImageCheckboxValue,
                                                    orderSheetOther4CheckboxValue,
                                                    orderSheetOwnCostCheckboxValue,
                                                    initialSelectionComboboxValue,
                                                    displayDaysValue
                                               );
    }

    private KarteConfigurationOutputItem ConvertToKarteConfiguration(List<UserConfModel> listUserConfig)
    {
        var configFontStyle = listUserConfig.FirstOrDefault(item => item.GrpCd == 101);
        string fontStyle = configFontStyle != null ? configFontStyle.Param : "Meiryo UI";

        int fontSize = GetConfigModel(listUserConfig, 101);
        int chartAutoWrapTextSetting = GetConfigModel(listUserConfig, 103);
        int imageSize = GetConfigModel(listUserConfig, 104, 2);
        int destinationOfCopy = GetConfigModel(listUserConfig, 908);
        int iMESetting = GetConfigModel(listUserConfig, 105);
        int isAllowResize = GetConfigModel(listUserConfig, 104, 1);
        return new KarteConfigurationOutputItem(
                                                    fontStyle,
                                                    fontSize,
                                                    chartAutoWrapTextSetting,
                                                    imageSize,
                                                    destinationOfCopy,
                                                    iMESetting,
                                                    isAllowResize
                                                );
    }

    private HeaderConfigurationOutputItem ConvertToHeaderConfiguration(List<UserConfModel> listUserConfig)
    {
        var header1Param = listUserConfig.FirstOrDefault(item => item.GrpCd == 910)?.Param ?? string.Empty;
        var header2Param = listUserConfig.FirstOrDefault(item => item.GrpCd == 911)?.Param ?? string.Empty;
        var listColor = listUserConfig.Where(item => item.GrpCd == 912).OrderBy(item => item.GrpItemCd).ToList();

        Dictionary<int, string> colorCode = new();
        foreach (var item in listColor)
        {
            colorCode.Add(item.GrpItemCd, item.Param);
        }

        List<int> listHeader1 = new();
        List<int> listHeader2 = new();
        foreach (var item in header1Param.ToCharArray())
        {
            listHeader1.Add(ConvertStringToIntHeaderConfiguration(item.ToString()));
        }
        foreach (var item in header2Param.ToCharArray())
        {
            listHeader2.Add(ConvertStringToIntHeaderConfiguration(item.ToString()));
        }

        return new HeaderConfigurationOutputItem(
                                                    colorCode,
                                                    listHeader1,
                                                    listHeader2
                                                );
    }

    private SummaryConfigurationOutputItem ConvertToSummaryConfiguration(List<UserConfModel> listUserConfig)
    {
        int popupVisible = GetConfigModel(listUserConfig, 924);
        int fontSize = GetConfigModel(listUserConfig, 901);

        var configFontStyle = listUserConfig.FirstOrDefault(item => item.GrpCd == 901);
        string fontStyle = configFontStyle != null ? configFontStyle.Param : "Yu Gothic UI";

        int buttonPositionValue = GetConfigModel(listUserConfig, 927, 2);
        bool pinummaryOnSet = GetConfigModel(listUserConfig, 927, 1) == 1;
        Dictionary<int, string> colorCode = new();
        List<int> isSelectedProperties = new();

        var isSelectedPropertieParam = listUserConfig.FirstOrDefault(item => item.GrpCd == 913)?.Param ?? string.Empty;
        var listColor = listUserConfig.Where(item => item.GrpCd == 914).OrderBy(item => item.GrpItemCd).ToList();

        foreach (var item in listColor)
        {
            colorCode.Add(item.GrpItemCd, item.Param);
        }

        foreach (var item in isSelectedPropertieParam.ToCharArray())
        {
            isSelectedProperties.Add(ConvertStringToIntSummaryConfiguration(item.ToString()));
        }

        return new SummaryConfigurationOutputItem(
                                                    popupVisible,
                                                    fontSize,
                                                    fontStyle,
                                                    buttonPositionValue,
                                                    pinummaryOnSet,
                                                    colorCode,
                                                    isSelectedProperties
                                                );
    }

    private FunctionConfigurationOutputItem ConvertToFunctionConfiguration(int hpId, int userId, List<UserConfModel> listUserConfig)
    {
        string _mouseRightClickKey = "MouseRightClick";

        int nextOrderDisplayComboboxValue = GetConfigModel(listUserConfig, 10, 1);
        int nextOrderUnexecutedExistComboboxValue = GetConfigModel(listUserConfig, 10, 2);
        int patientInforComboboxValue = GetConfigModel(listUserConfig, 3);
        int watingListComboboxValue = GetConfigModel(listUserConfig, 902);
        int setEditButtonComboboxValue = GetConfigModel(listUserConfig, 906);
        int summaryEditLockComboboxValue = GetConfigModel(listUserConfig, 907);
        int actionWhenClosingSchemaValue = GetConfigModel(listUserConfig, 14);
        int rightClickComboboxValue = int.Parse(_jsonSettingRepository.Get(userId, _mouseRightClickKey)?.Value ?? "0");
        int pendingMedicalRecordsComboboxValue = GetConfigModel(listUserConfig, 903);
        int startKarteViewerValue = GetConfigModel(listUserConfig, 9);
        int isShowDrugUsageHistoryValue = GetConfigModel(listUserConfig, 19);
        bool isShowReservationsSetting = _systemConfRepository.GetSettingValue(100014, 0, hpId) == 1;
        int puchiExternalReservationListValue = GetConfigModel(listUserConfig, 905);
        int waitingListUpdateInterval = GetConfigModel(listUserConfig, 15);
        int lastDayVisibleInPatientInfo = GetConfigModel(listUserConfig, 928, 0, 1);
        int lastDayVisibleInMedical = GetConfigModel(listUserConfig, 928, 0, 2);
        int lastDayVisibleInAccounting = GetConfigModel(listUserConfig, 928, 0, 3);

        return new FunctionConfigurationOutputItem(
                                                        nextOrderDisplayComboboxValue,
                                                        nextOrderUnexecutedExistComboboxValue,
                                                        patientInforComboboxValue,
                                                        watingListComboboxValue,
                                                        setEditButtonComboboxValue,
                                                        summaryEditLockComboboxValue,
                                                        actionWhenClosingSchemaValue,
                                                        rightClickComboboxValue,
                                                        pendingMedicalRecordsComboboxValue,
                                                        startKarteViewerValue,
                                                        isShowDrugUsageHistoryValue,
                                                        isShowReservationsSetting,
                                                        puchiExternalReservationListValue,
                                                        waitingListUpdateInterval,
                                                        lastDayVisibleInPatientInfo,
                                                        lastDayVisibleInMedical,
                                                        lastDayVisibleInAccounting
                                                  );
    }

    private SaveConfirmationConfigurationOutputItem ConvertToSaveConfirmationConfiguration(List<UserConfModel> listUserConfig)
    {
        int claimSagakuComboboxConfig = GetConfigModel(listUserConfig, 922);
        int claimSagakuAtReceTimeConfig = GetConfigModel(listUserConfig, 923);
        int noteScreenDisplayComboboxConfig = GetConfigModel(listUserConfig, 919);
        ConfigCheckboxSubItem commentCheckConfig = ConvertToConfigCheckboxModel(listUserConfig, 921, 0, "00000");
        ConfigCheckboxSubItem santeiCheckConfig = ConvertToConfigCheckboxModel(listUserConfig, 921, 1, "10100");
        ConfigCheckboxSubItem inputCheckConfig = ConvertToConfigCheckboxModel(listUserConfig, 921, 2, "10100");
        ConfigCheckboxSubItem kubunCheckConfig = ConvertToConfigCheckboxModel(listUserConfig, 921, 3, "10100");
        ConfigCheckboxSubItem reportCheckConfig = ConvertToConfigCheckboxModel(listUserConfig, 921, 4, "10101");
        ConfigCheckboxSubItem tenkeiByomeConfig = ConvertToConfigCheckboxModel(listUserConfig, 921, 5, "11111");
        return new SaveConfirmationConfigurationOutputItem(
                                                            claimSagakuComboboxConfig,
                                                            claimSagakuAtReceTimeConfig,
                                                            noteScreenDisplayComboboxConfig,
                                                            commentCheckConfig,
                                                            santeiCheckConfig,
                                                            inputCheckConfig,
                                                            kubunCheckConfig,
                                                            reportCheckConfig,
                                                            tenkeiByomeConfig
                                                       );
    }

    private SuperSetConfigurationOutputItem ConvertToSuperSetConfiguration(List<UserConfModel> listUserConfig)
    {
        List<int> listSuperSetButtonCodeItem = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        int dropByomeiComboboxValue = GetConfigModel(listUserConfig, 310);
        int styleComboboxValue = GetConfigModel(listUserConfig, 301);
        int doubleClickComboBoxValue = GetConfigModel(listUserConfig, 302);
        int edaNoSelectedValue = GetConfigModel(listUserConfig, 304, 1);
        int setKbnSelectedValue = GetConfigModel(listUserConfig, 304);
        int autoShowSelectedValue = GetConfigModel(listUserConfig, 305);
        int isTopMostSelectedValue = GetConfigModel(listUserConfig, 306);
        int closeSelectedValue = GetConfigModel(listUserConfig, 307);
        int positionSelectedValue = GetConfigModel(listUserConfig, 309);
        int columnNumber = GetConfigModel(listUserConfig, 309, 1);
        int columnHeight = GetConfigModel(listUserConfig, 309, 2);
        int columnMargin = GetConfigModel(listUserConfig, 309, 3);
        int columnFontSize = GetConfigModel(listUserConfig, 309, 4);
        bool doOrderChecked = GetConfigModel(listUserConfig, 308) == 1;
        bool doKarteChecked = GetConfigModel(listUserConfig, 308, 1) == 1;
        bool doByomeiChecked = GetConfigModel(listUserConfig, 308, 2) == 1;

        var listSetKbnConfig = listUserConfig.Where(item => item.GrpCd == 303
                                                            && listSuperSetButtonCodeItem.Contains(item.GrpItemCd))
                                             .OrderBy(x => x.Val)
                                             .ToList();
        Dictionary<int, bool> kbnMstCheckBox = new();
        foreach (var item in listSetKbnConfig)
        {
            kbnMstCheckBox.Add(item.GrpItemCd, int.Parse(item.Param) == 1);
        }
        return new SuperSetConfigurationOutputItem(
                                                        dropByomeiComboboxValue,
                                                        styleComboboxValue,
                                                        doubleClickComboBoxValue,
                                                        edaNoSelectedValue,
                                                        setKbnSelectedValue,
                                                        autoShowSelectedValue,
                                                        isTopMostSelectedValue,
                                                        closeSelectedValue,
                                                        positionSelectedValue,
                                                        columnNumber,
                                                        columnHeight,
                                                        columnMargin,
                                                        columnFontSize,
                                                        doOrderChecked,
                                                        doKarteChecked,
                                                        doByomeiChecked,
                                                        kbnMstCheckBox
                                                    );
    }

    private OtherConfigurationOutputItem ConvertToOtherConfiguration(List<UserConfModel> listUserConfig)
    {
        int dateFormatComboBox1Value = GetConfigModel(listUserConfig, 100001);
        int mininumDiseaseComboboxValue = GetConfigModel(listUserConfig, 13);
        int dateFormatComboBox2Value = GetConfigModel(listUserConfig, 100002);
        int toolbarLocationComboboxValue = GetConfigModel(listUserConfig, 909);
        int confirmChangeCombobox1Value = GetConfigModel(listUserConfig, 100004);
        int confirmChangeCombobox2Value = GetConfigModel(listUserConfig, 100005);
        int warningWhenEditComboboxValue = GetConfigModel(listUserConfig, 100006);
        int birthDayFormartSetting = GetConfigModel(listUserConfig, 100011);
        int visitingDateFormartSetting = GetConfigModel(listUserConfig, 100012);

        var configColorBackground = listUserConfig.FirstOrDefault(item => item.GrpCd == 1);
        string sColorBackground = configColorBackground != null ? configColorBackground.Param : "FFFFFFFF";

        return new OtherConfigurationOutputItem(
                                                    dateFormatComboBox1Value,
                                                    mininumDiseaseComboboxValue,
                                                    dateFormatComboBox2Value,
                                                    toolbarLocationComboboxValue,
                                                    confirmChangeCombobox1Value,
                                                    confirmChangeCombobox2Value,
                                                    warningWhenEditComboboxValue,
                                                    birthDayFormartSetting,
                                                    visitingDateFormartSetting,
                                                    sColorBackground
                                                );
    }

    private int GetConfigModel(List<UserConfModel> listUserConfig, int grpCd, int grpItemCd = 0, int grpItemEdaNo = 0)
    {
        var tempModel = listUserConfig.FirstOrDefault(item => item.GrpCd == grpCd && item.GrpItemCd == grpItemCd && item.GrpItemEdaNo == grpItemEdaNo);
        if (tempModel == null)
        {
            return _userConfRepository.GetDefaultValue(grpCd, grpItemCd);
        }
        return tempModel.Val;
    }

    private int ConvertStringToIntHeaderConfiguration(string value)
    {
        string intValue = "123456789";
        string srtValue = value.ToUpper();
        if (value == string.Empty)
        {
            return 0;
        }
        else if (intValue.Contains(srtValue))
        {
            return int.Parse(srtValue);
        }
        switch (srtValue)
        {
            case "A":
                return 10;
            case "B":
                return 11;
            case "C":
                return 12;
            case "D":
                return 13;
            default:
                return 14;
        }
    }

    private int ConvertStringToIntSummaryConfiguration(string value)
    {
        string intValue = "123456789";
        string srtValue = value.ToUpper();
        if (value == string.Empty)
        {
            return 0;
        }
        else if (intValue.Contains(srtValue))
        {
            return int.Parse(srtValue);
        }
        switch (srtValue)
        {
            case "A":
                return 10;
            case "B":
                return 11;
            case "C":
                return 12;
            case "D":
                return 13;
            case "E":
                return 14;
            default:
                return 15;
        }
    }

    private ConfigCheckboxSubItem ConvertToConfigCheckboxModel(List<UserConfModel> listUserConfig, int grpCd, int grpItemCd, string defaultParam)
    {
        var tempModel = listUserConfig.FirstOrDefault(item => item.GrpCd == grpCd && item.GrpItemCd == grpItemCd);
        var param = tempModel != null ? tempModel.Param : defaultParam;
        bool isCheckedFirstCharParam = !string.IsNullOrEmpty(param) && param[0].AsInteger() == 1;
        bool isCheckedSecondCharParam = !string.IsNullOrEmpty(param) && param[1].AsInteger() == 1;
        bool isCheckedThirdCharParam = !string.IsNullOrEmpty(param) && param[2].AsInteger() == 1;
        bool isCheckedFourthCharParam = !string.IsNullOrEmpty(param) && param[3].AsInteger() == 1;
        bool isCheckedFifthCharParam = !string.IsNullOrEmpty(param) && param[4].AsInteger() == 1;
        return new ConfigCheckboxSubItem(
                                            isCheckedFirstCharParam,
                                            isCheckedSecondCharParam,
                                            isCheckedThirdCharParam,
                                            isCheckedFourthCharParam,
                                            isCheckedFifthCharParam
                                       );
    }
}
