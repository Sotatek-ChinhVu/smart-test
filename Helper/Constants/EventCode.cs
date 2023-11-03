namespace Helper.Constants;

public class EventCode
{
    public static string Madoguchi_Seisan { get; } = "03000000001";
    public static string Syuno_Seisan { get; } = "99004000001";
    /// <summary>
    /// 属性編集画面ー受付
    /// </summary>
    public static string Zokusei_Uketuke { get; } = "01000000001";

    public static string SystemStart { get; } = "00000000000";
    public static string SystemEnd { get; } = "00000000999";
    public static string Login { get; } = "00001000000";
    public static string Logout { get; } = "00001000999";
    public static string PatientInfoOpenReceExclude { get; } = "01000000000";
    public static string PatientInfoReception { get; } = "01000000001";
    public static string PatientInfoUpdate { get; } = "01000000002";
    public static string PatientInfoOpen { get; } = "01000000003";
    public static string PatientInfoUpdateClick { get; } = "01000000004";
    public static string PatientInfoReceptionClick { get; } = "01000000005";
    public static string PatientInfoClose { get; } = "01000000999";
    public static string PatientInfSwitchHokenPattern { get; } = "01001000001";


    public static string MedicalOpen { get; } = "02000000000";
    public static string OrderUpdate { get; } = "02000000001";
    public static string KarteUpdate { get; } = "02000000002";
    public static string MedicalOpenReceExclude { get; } = "02000000003";
    public static string OrderUpdateReceExclude { get; } = "02000000004";
    public static string KarteUpdateReceExclude { get; } = "02000000005";
    public static string NextOdrUpdate { get; } = "02000000006";
    public static string PeriodicOdrUpdate { get; } = "02000000007";
    public static string OtherPatientMedicalOpen { get; } = "02000000008";
    public static string OtherPatientMedicalOpenReceExclude { get; } = "02000000009";
    public static string SamePatientMedicalOpen { get; } = "02000000010";
    public static string SamePatientMedicalOpenReceExclude { get; } = "02000000011";
    public static string OdrDrugInUpdate { get; } = "02000000012";
    public static string OdrDrugInUpdateReceExclude { get; } = "02000000013";
    public static string MedicalCloseTempExclude { get; } = "02000000996";
    public static string MedicalCloseTemp { get; } = "02000000997";
    public static string MedicalCloseReceExclude { get; } = "02000000998";
    public static string MedicalClose { get; } = "02000000999";

    public static string SavePress { get; } = "02000001001";
    public static string SavePressOrderChanged { get; } = "02000001002";
    public static string SavePressKarteChanged { get; } = "02000001003";
    public static string SavePressReceExclude { get; } = "02000001004";
    public static string SavePressOrderChangedReceExclude { get; } = "02000001005";
    public static string SavePressKarteChangedReceExclude { get; } = "02000001006";
    public static string SavePressNextOdrChanged { get; } = "02000001007";
    public static string SavePressPeriodicOdrChanged { get; } = "02000001008";

    public static string KeisanSavePress { get; } = "02000002001";
    public static string KeisanSavePressOrderChanged { get; } = "02000002002";
    public static string KeisanSavePressKarteChanged { get; } = "02000002003";
    public static string KeisanSavePressReceExclude { get; } = "02000002004";
    public static string KeisanSavePressOrderChangedReceExclude { get; } = "02000002005";
    public static string KeisanSavePressKarteChangedReceExclude { get; } = "02000002006";
    public static string KeisanSavePressNextOdrChanged { get; } = "02000002007";
    public static string KeisanSavePressPeriodicOdrChanged { get; } = "02000002008";

    public static string ShisanSavePress { get; } = "02000003001";
    public static string ShisanSavePressOrderChanged { get; } = "02000003002";
    public static string ShisanSavePressKarteChanged { get; } = "02000003003";
    public static string ShisanSavePressReceExclude { get; } = "02000003004";
    public static string ShisanSavePressOrderChangedReceExclude { get; } = "02000003005";
    public static string ShisanSavePressKarteChangedReceExclude { get; } = "02000003006";

    public static string TempSavePress { get; } = "02000004001";
    public static string TempSavePressOrderChanged { get; } = "02000004002";
    public static string TempSavePressKarteChanged { get; } = "02000004003";
    public static string TempSavePressReceExclude { get; } = "02000004004";
    public static string TempSavePressOrderChangedReceExclude { get; } = "02000004005";
    public static string TempSavePressKarteChangedReceExclude { get; } = "02000004006";
    public static string TempSavePressNextOdrChanged { get; } = "02000004007";
    public static string TempSavePressPeriodicOdrChanged { get; } = "02000004008";

    public static string PrintPress { get; } = "02000005001";
    public static string PrintPressOrderChanged { get; } = "02000005002";
    public static string PrintPressKarteChanged { get; } = "02000005003";
    public static string PrintPressReceExclude { get; } = "02000005004";
    public static string PrintPressOrderChangedReceExclude { get; } = "02000005005";
    public static string PrintPressKarteChangedReceExclude { get; } = "02000005006";
    public static string PrintPressNextOrderChanged { get; } = "02000005007";
    public static string PrintPressPeriodicOrderChanged { get; } = "02000005008";

    public static string SupetSetDetailChanged { get; } = "02001000001";

    public static string AccountingOpen { get; } = "03000000000";
    public static string AccountingExecute { get; } = "03000000001";
    public static string DisCharged { get; } = "03000000002";
    public static string AccountingClose { get; } = "03000000999";

    public static string DeleteRaiinInf { get; } = "04000000001";

    public static string ReceListOpen { get; } = "05000000000";
    public static string ReceListCSVOutput { get; } = "05000000001";
    public static string ReceListClose { get; } = "05000000999";
    public static string ReceCheckOpen { get; } = "05001000000";
    public static string ReceCheckClose { get; } = "05001000999";

    public static string UketukeInfoReception { get; } = "99003000002";

    public static string ListSetMasterOpen { get; } = "99206000000";
    public static string ListSetMasterClose { get; } = "99206000999";

    public static string KensaMasterOpen { get; } = "99205000000";
    public static string KensaMasterClose { get; } = "99205000999";

    public static string HokenMasterOpen { get; } = "99204000000";
    public static string HokenMasterClose { get; } = "99204000999";

    public static string TenMasterOpen { get; } = "99201000000";
    public static string TenMasterClose { get; } = "99201000999";
    public static string TenMstBasic { get; } = "99201002000";
    public static string TenMstIji { get; } = "99201003000";
    public static string TenMstPrecription { get; } = "99201004000";
    public static string TenMstUsage { get; } = "99201005000";
    public static string TenMstSpecialMaterial { get; } = "99201006000";
    public static string TenMstDrugInfomation { get; } = "99201007000";
    public static string TenMstTeikyoByomei { get; } = "99201008000";
    public static string TenMstSanteiKaishu { get; } = "99201009000";
    public static string TenMstHaihan { get; } = "99201010000";
    public static string TenMstHoukatsu { get; } = "99201011000";
    public static string TenMstCombinedContraindication { get; } = "99201012000";
    public static string TenMstRenkei { get; } = "99201013000";

    public static string TimeZoneMasterOpen { get; } = "99203000000";
    public static string TimeZoneMasterClose { get; } = "99203000999";

    public static string HolidayMasterOpen { get; } = "99202000000";
    public static string HolidayMasterClose { get; } = "99202000999";

    public static string MonshinOpen { get; } = "99009000000";
    public static string MonshinClose { get; } = "99009000999";

    public static string KarteApprovalOpen { get; } = "99008000000";
    public static string KarteApprovalPreview { get; } = "99008000001";
    public static string KarteApprovalClose { get; } = "99008000999";

    public static string FillingOpen { get; } = "99007000000";
    public static string FillingClose { get; } = "99007000999";
    public static string PreviewOpen { get; } = "99007000000";
    public static string PreviewPrint { get; } = "99007000000";
    public static string PreviewClose { get; } = "99007000999";

    public static string DocumentManagementOpen { get; } = "99006000000";
    public static string DocumentManagementClose { get; } = "99006000999";
    public static string EditDocumentOpen { get; } = "99006001000";
    public static string EditDocumentSave { get; } = "99006001001";
    public static string EditDocumentPrint { get; } = "99006001002";
    public static string EditDocumentClose { get; } = "99006001999";

    public static string SpecialNoteOpen { get; } = "99002000000";
    public static string SummaryUpdated { get; } = "99005000001";
    public static string SpecialNoteClose { get; } = "99002000999";

    public static string AccountDueListOpen { get; } = "99004000000";
    public static string AccountDueListUpdate { get; } = "99004000001";
    public static string AccountDueListClose { get; } = "99004000999";

    public static string OpenReceptionInCreateMode { get; } = "99003000000";
    public static string OpenReceptionInUpdateMode { get; } = "99003000001";
    public static string ReceptionClose { get; } = "99003000999";
    public static string ReceptionCloseInUpdateMode { get; } = "99003000998";
    public static string KarteHistoryOpen { get; } = "99002001000";
    public static string KarteHistoryClose { get; } = "99002001999";

    //Click button 来院追加
    public static string ReceptionFromVisitingList { get; } = "04000000002";

    //Order without reception
    public static string ReceptionFromMedical { get; } = "02999000000";

    public static string DiseaseRegOpen { get; } = "99001000000";
    public static string DiseaseRegClose { get; } = "99001000999";
    public static string DiseaseRegUpdate { get; } = "99001000001";

    public static string UpdateToReception { get; } = "98000000001";
    public static string UpdateToTempSave { get; } = "98000000003";
    public static string UpdateToCalculate { get; } = "98000000005";
    public static string UpdateToWaiting { get; } = "98000000007";
    public static string UpdateToSettled { get; } = "98000000009";

    public static string TeamKarteOrderUpdate { get; } = "99901000001";
    public static string TeamKarteKarteUpdate { get; } = "99901000002";

    public static string UnlockInf { get; } = "99999000001";

    /// <summary>
    /// 診察券発行時
    /// </summary>
    public static string ShinsatuKen { get; } = "97006000000";
    /// <summary>
    /// 診察券発行指示
    /// </summary>
    public static string PrintShinsatuKenShiji { get; } = "97006000001";

    /// <summary>
    /// カルテビューア・開くとき
    /// </summary>
    public static string KarteViewOpen { get; } = "99010000000";
    /// <summary>
    /// カルテビューア・閉じるとき
    /// </summary>
    public static string KarteViewClose { get; } = "99010000999";

    public static string StickyNoteUpdate { get; } = "99010000001";

    public static string RecalculationViewOpen { get; } = "05002000000";
    public static string Recalculation { get; } = "05002000001";
    public static string RecalculationViewClose { get; } = "05002000999";

    #region 帳票印刷
    /// <summary>
    /// カルテ１号紙印刷
    /// </summary>
    public const string ReportKarte1 = "97001000000";
    /// <summary>
    /// カルテ２号紙印刷
    /// </summary>
    public const string ReportKarte2 = "97002000000";
    /// <summary>
    /// 病名一覧印刷
    /// </summary>
    public const string ReportByomei = "97003000000";
    /// <summary>
    /// カルテ３号紙印刷
    /// </summary>
    public const string ReportKarte3 = "97004000000";
    /// <summary>
    /// 受診票
    /// </summary>
    public const string ReportJyusinHyo = "97005000000";
    /// <summary>
    /// オーダーラベル
    /// </summary>
    public const string ReportOrderLabel = "97021000000";
    /// <summary>
    /// 薬袋ラベル
    /// </summary>
    public const string ReportYakutai = "97022000000";
    /// <summary>
    /// 院内処方箋印刷
    /// </summary>
    public const string ReportInDrug = "97023000000";
    /// <summary>
    /// 院外処方箋印刷
    /// </summary>
    public const string ReportOutDrug = "97024000000";
    /// <summary>
    /// 指示箋印刷
    /// </summary>
    public const string ReportSijisen = "97025000000";
    /// <summary>
    /// 薬情印刷
    /// </summary>
    public const string ReportDrugInf = "97026000000";
    /// <summary>
    /// お薬手帳シール印刷
    /// </summary>
    public const string ReportDrugNoteSeal = "97027000000";
    /// <summary>
    /// 領収証印刷
    /// </summary>
    public const string ReportAccounting = "97041000000";
    /// <summary>
    /// 領収証明細印刷
    /// </summary>
    public const string ReportAccountingDetail = "97042000000";
    /// <summary>
    /// 会計カード印刷
    /// </summary>
    public const string ReportAccountingCard = "97043000000";
    /// <summary>
    /// 会計カード一覧印刷
    /// </summary>
    public const string ReportAccountingCardList = "97044000000";

    /// <summary>
    /// レセプト印刷
    /// </summary>
    public const string ReportReceipt = "97061000000";
    /// <summary>
    /// レセプト電算出力
    /// </summary>
    public const string ReportReceden = "97061000001";
    /// <summary>
    /// レセ対象患者一覧印刷
    /// </summary>
    public const string ReportReceTarget = "97062000000";
    /// <summary>
    /// 症状詳記印刷
    /// </summary>
    public const string ReportSyojyoSyoki = "97063000000";
    /// <summary>
    /// レセチェック一覧印刷
    /// </summary>
    public const string ReportReceiptList = "97064000000";
    /// <summary>
    /// レセチェックリスト印刷
    /// </summary>
    public const string ReportReceiptCheckList = "97065000000";
    /// <summary>
    /// 予約票印刷
    /// </summary>
    public const string ReportYoyakuHyo = "97070000000";
    /// <summary>
    /// 予約一覧印刷
    /// </summary>
    public const string ReportYoyakuList = "97071000000";
    /// <summary>
    /// 検査依頼書
    /// </summary>
    public const string ReportKensaIraiList = "97072000000";
    /// <summary>
    /// 検査依頼ファイル出力
    /// </summary>
    public const string ReportKensaIraiFile = "97072000001";
    /// <summary>
    /// ネームラベル
    /// </summary>
    public const string ReportNameLabel = "97073000000";
    /// <summary>
    /// JunNaviオプション印刷
    /// </summary>
    public const string JunNaviOption = "97080000000";
    /// <summary>
    /// JunNavi番号カード印刷
    /// </summary>
    public const string JunNaviCard = "97081000000";

    /// <summary>
    /// 日計表（印刷）
    /// </summary>
    public const string DailyReportPrint = "97101000000";
    /// <summary>
    /// 日計表（ファイル出力）
    /// </summary>
    public const string DailyReportFileOutput = "97101000001";
    /// <summary>
    /// 日計表[保険別]（印刷）
    /// </summary>
    public const string DailyHokenReportPrint = "97102000000";
    /// <summary>
    /// 日計表[保険別]（ファイル出力）
    /// </summary>
    public const string DailyHokenReportFileOutput = "97102000001";
    /// <summary>
    /// 未収金一覧表（印刷）
    /// </summary>
    public const string MisyuReportPrint = "97103000000";
    /// <summary>
    /// 未収金一覧表（ファイル出力）
    /// </summary>
    public const string MisyuReportFileOutput = "97103000001";
    /// <summary>
    /// 月計表（印刷）
    /// </summary>
    public const string MonthlyReportPrint = "97201000000";
    /// <summary>
    /// 月計表（ファイル出力）
    /// </summary>
    public const string MonthlyReportFileOutput = "97201000001";
    /// <summary>
    /// 月計表[保険別]（印刷）
    /// </summary>
    public const string MonthlyHokenReportPrint = "97202000000";
    /// <summary>
    /// 月計表[保険別]（ファイル出力）
    /// </summary>
    public const string MonthlyHokenReportFileOutput = "97202000001";
    /// <summary>
    /// 月計表[明細]（印刷）
    /// </summary>
    public const string MonthlyDetailReportPrint = "97203000000";
    /// <summary>
    /// 月計表[明細]（ファイル出力）
    /// </summary>
    public const string MonthlyDetailReportFileOutput = "97203000001";
    /// <summary>
    /// 保険種別総括表（印刷）
    /// </summary>
    public const string SokatuReportPrint = "97204000000";
    /// <summary>
    /// 保険種別総括表（ファイル出力）
    /// </summary>
    public const string SokatuReportFileOutput = "97204000001";
    /// <summary>
    /// 保険種別総括表合計表（印刷）
    /// </summary>
    public const string SokatuGokeiReportPrint = "97205000000";
    /// <summary>
    /// 保険種別総括表合計表（ファイル出力）
    /// </summary>
    public const string SokatuGokeiReportFileOutput = "97205000001";
    /// <summary>
    /// 診療項目別集計表（印刷）
    /// </summary>
    public const string SinItemReportPrint = "97206000000";
    /// <summary>
    /// 診療項目別集計表（ファイル出力）
    /// </summary>
    public const string SinItemReportFileOutput = "97206000001";
    /// <summary>
    /// 診療項目月別集計一覧表（印刷）
    /// </summary>
    public const string SinItemMonthlyReportPrint = "97207000000";
    /// <summary>
    /// 診療項目月別集計一覧表（ファイル出力）
    /// </summary>
    public const string SinItemMonthlyReportFileOutput = "97207000001";
    /// <summary>
    /// 行為項目別使用患者一覧
    /// </summary>
    //public const string ReportPtListByActionItem = "97301000000";
    /// <summary>
    /// 採用薬一覧（印刷）
    /// </summary>
    public const string AdoptedDrugListPrint = "97302000000";
    /// <summary>
    /// 採用薬一覧（ファイル出力）
    /// </summary>
    public const string AdoptedDrugListFileOutput = "97302000001";
    /// <summary>
    /// セット一覧（印刷）
    /// </summary>
    public const string SetListPrint = "97303000000";
    /// <summary>
    /// セット一覧（ファイル出力）
    /// </summary>
    public const string SetListFileOutput = "97303000001";
    /// <summary>
    /// 医薬品使用実績一覧（印刷）
    /// </summary>
    public const string UsedDrugListPrint = "97304000000";
    /// <summary>
    /// 医薬品使用実績一覧（ファイル出力）
    /// </summary>
    public const string UsedDrugListFileOutput = "97304000001";
    /// <summary>
    /// 向精神薬投与患者一覧表（印刷）
    /// </summary>
    public const string KouseisinListPrint = "97305000000";
    /// <summary>
    /// 向精神薬投与患者一覧表（ファイル出力）
    /// </summary>
    public const string KouseisinListFileOutput = "97305000001";
    /// <summary>
    /// 病名一覧（印刷）
    /// </summary>
    public const string PtByomeiListPrint = "97306000000";
    /// <summary>
    /// 病名一覧（ファイル出力）
    /// </summary>
    public const string PtByomeiListFileOutput = "97306000001";
    /// <summary>
    /// 来院状況実績表（印刷）
    /// </summary>
    public const string RaiinReportPrint = "97307000000";
    /// <summary>
    /// 来院状況実績表（ファイル出力）
    /// </summary>
    public const string RaiinReportFileOutput = "97307000001";
    /// <summary>
    /// 患者数集計表（印刷）
    /// </summary>
    public const string PtCntReportPrint = "97309000000";
    /// <summary>
    /// 患者数集計表（ファイル出力）
    /// </summary>
    public const string PtCntReportFileOutput = "97309000001";
    /// <summary>
    /// 診療項目別患者一覧表（印刷）
    /// </summary>
    public const string SinItemPtListPrint = "97308000000";
    /// <summary>
    /// 診療項目別患者一覧表（ファイル出力）
    /// </summary>
    public const string SinItemPtListFileOutput = "97308000001";
    /// <summary>
    /// 行為別診療費集計表（印刷）
    /// </summary>
    public const string SinKouiReportPrint = "97310000000";
    /// <summary>
    /// 行為別診療費集計表（ファイル出力）
    /// </summary>
    public const string SinKouiReportFileOutput = "97310000001";
    /// <summary>
    /// 精神科デイ・ケア等実施患者一覧表（印刷）
    /// </summary>
    public const string SeisinDayCarePrint = "97311000000";
    /// <summary>
    /// 精神科デイ・ケア等実施患者一覧表（ファイル出力）
    /// </summary>
    public const string SeisinDayCareFileOutput = "97311000001";
    /// <summary>
    /// リストセット一覧（印刷）
    /// </summary>
    public const string ListSetPrint = "97312000000";
    /// <summary>
    /// リストセット一覧（ファイル出力）
    /// </summary>
    public const string ListSetFileOutput = "97312000001";
    /// <summary>
    /// 連携患者追加
    /// </summary>
    public const string RenkeiPtInfAdd = "99901001001";
    /// <summary>
    /// 連携患者更新
    /// </summary>
    public const string RenkeiPtInfUpdate = "99901001002";
    /// <summary>
    /// 連携予約追加
    /// </summary>
    public const string RenkeiYoyakuAdd = "99901002001";
    /// <summary>
    /// 連携予約更新
    /// </summary>
    public const string RenkeiYoyakuUpdate = "99901002002";
    /// <summary>
    /// 連携受付時
    /// </summary>
    public const string RenkeiUketuke = "99901002003";
    /// <summary>
    /// Web登録用ID用紙（印刷）
    /// </summary>
    public const string WebId = "97074000000";

    public const string KensaLabel = "97028000000";
    #endregion

    public static string SwitchKarute { get; } = "02000006001";

    public static string SwitchKaruteOrderChanged { get; } = "02000006002";

    public static string SwitchKaruteKarteChanged { get; } = "02000006003";

    public static string SwitchKaruteNotRece { get; } = "02000006004";

    public static string SwitchKaruteNotReceOrderChanged { get; } = "02000006005";

    public static string SwitchKaruteNotReceKarteChanged { get; } = "02000006006";

    public static string SwitchKaruteNextOrderChanged { get; } = "02000006007";

    public static string SwitchKarutePeriodicOrderChanged { get; } = "02000006008";

    public static string ByomeiTenkiToolOpen { get; } = "99015000000";
    public static string ByomeiTenkiToolExecute { get; } = "99015000001";
    public static string ByomeiTenkiToolClose { get; } = "99015000999";
}
