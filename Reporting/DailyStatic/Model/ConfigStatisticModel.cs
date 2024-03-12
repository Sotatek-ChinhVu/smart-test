using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Reporting.DailyStatic.Enum;

namespace Reporting.DailyStatic.Model;

public class ConfigStatisticModel
{
    private StaMenu _staMenu;
    public StaMenu StaMenu
    {
        get
        {
            switch (_reportId)
            {
                case (int)StatisticReportType.Sta2021:
                    return ConfigStatistic2021.StaMenu;
                case (int)StatisticReportType.Sta3001:
                    return ConfigStatistic3001.StaMenu;
                case (int)StatisticReportType.Sta3010:
                    return ConfigStatistic3010.StaMenu;
                case (int)StatisticReportType.Sta3020:
                    return ConfigStatistic3020.StaMenu;
                case (int)StatisticReportType.Sta3030:
                    return ConfigStatistic3030.StaMenu;
                case (int)StatisticReportType.Sta3040:
                    return ConfigStatistic3040.StaMenu;
                case (int)StatisticReportType.Sta3041:
                    return ConfigStatistic3041.StaMenu;
                case (int)StatisticReportType.Sta3050:
                    return ConfigStatistic3050.StaMenu;
                case (int)StatisticReportType.Sta3060:
                    return ConfigStatistic3060.StaMenu;
                case (int)StatisticReportType.Sta3061:
                    return ConfigStatistic3061.StaMenu;
                case (int)StatisticReportType.Sta3070:
                    return ConfigStatistic3070.StaMenu;
                case (int)StatisticReportType.Sta3071:
                    return ConfigStatistic3071.StaMenu;
                case (int)StatisticReportType.Sta3080:
                    return ConfigStatistic3080.StaMenu;
                default:
                    return _staMenu;
            }
        }
    }

    private List<StaConf> _listStaConf;
    public List<StaConf> ListStaConf
    {
        get
        {
            switch (_reportId)
            {
                case (int)StatisticReportType.Sta2021:
                    return ConfigStatistic2021.ListStaConf;
                case (int)StatisticReportType.Sta3001:
                    return ConfigStatistic3001.ListStaConf;
                case (int)StatisticReportType.Sta3010:
                    return ConfigStatistic3010.ListStaConf;
                case (int)StatisticReportType.Sta3020:
                    return ConfigStatistic3020.ListStaConf;
                case (int)StatisticReportType.Sta3030:
                    return ConfigStatistic3030.ListStaConf;
                case (int)StatisticReportType.Sta3040:
                    return ConfigStatistic3040.ListStaConf;
                case (int)StatisticReportType.Sta3041:
                    return ConfigStatistic3041.ListStaConf;
                case (int)StatisticReportType.Sta3050:
                    return ConfigStatistic3050.ListStaConf;
                case (int)StatisticReportType.Sta3060:
                    return ConfigStatistic3060.ListStaConf;
                case (int)StatisticReportType.Sta3061:
                    return ConfigStatistic3061.ListStaConf;
                case (int)StatisticReportType.Sta3070:
                    return ConfigStatistic3070.ListStaConf;
                case (int)StatisticReportType.Sta3071:
                    return ConfigStatistic3071.ListStaConf;
                case (int)StatisticReportType.Sta3080:
                    return ConfigStatistic3080.ListStaConf;
                default:
                    return _listStaConf;
            }
        }
    }

    public ConfigStatistic2021Model ConfigStatistic2021 { get; set; }
    public ConfigStatistic3001Model ConfigStatistic3001 { get; set; }
    public ConfigStatistic3010Model ConfigStatistic3010 { get; set; }
    public ConfigStatistic3020Model ConfigStatistic3020 { get; set; }
    public ConfigStatistic3030Model ConfigStatistic3030 { get; set; }

    public ConfigStatistic3040Model ConfigStatistic3040 { get; set; }
    public ConfigStatistic3041Model ConfigStatistic3041 { get; set; }
    public ConfigStatistic3050Model ConfigStatistic3050 { get; set; }
    public ConfigStatistic3060Model ConfigStatistic3060 { get; set; }
    public ConfigStatistic3061Model ConfigStatistic3061 { get; set; }
    public ConfigStatistic3070Model ConfigStatistic3070 { get; set; }
    public ConfigStatistic3071Model ConfigStatistic3071 { get; set; }
    public ConfigStatistic3080Model ConfigStatistic3080 { get; set; }

    private int _reportId = -1;

    public ConfigStatisticModel(ConfigStatistic3080Model configStatistic3080)
    {
        _reportId = configStatistic3080.ReportId;
        ConfigStatistic3080 = configStatistic3080;
    }

    public ConfigStatisticModel(ConfigStatistic3071Model configStatistic3071)
    {
        _reportId = configStatistic3071.ReportId;
        ConfigStatistic3071 = configStatistic3071;
    }

    public ConfigStatisticModel(ConfigStatistic3070Model configStatistic3070)
    {
        _reportId = configStatistic3070.ReportId;
        ConfigStatistic3070 = configStatistic3070;
    }

    public ConfigStatisticModel(ConfigStatistic3060Model configStatistic3060)
    {
        _reportId = configStatistic3060.ReportId;
        ConfigStatistic3060 = configStatistic3060;
    }

    public ConfigStatisticModel(ConfigStatistic3061Model configStatistic3061)
    {
        _reportId = configStatistic3061.ReportId;
        ConfigStatistic3061 = configStatistic3061;
    }

    public ConfigStatisticModel(ConfigStatistic2021Model configStatistic2021)
    {
        _reportId = configStatistic2021.ReportId;
        ConfigStatistic2021 = configStatistic2021;
    }
    public ConfigStatisticModel(ConfigStatistic3020Model configStatistic3020)
    {
        _reportId = configStatistic3020.ReportId;
        ConfigStatistic3020 = configStatistic3020;
    }

    public ConfigStatisticModel(ConfigStatistic3030Model configStatistic3030)
    {
        _reportId = configStatistic3030.ReportId;
        ConfigStatistic3030 = configStatistic3030;
    }

    public ConfigStatisticModel(ConfigStatistic3050Model configStatistic3050)
    {
        _reportId = configStatistic3050.ReportId;
        ConfigStatistic3050 = configStatistic3050;
    }

    public ConfigStatisticModel(ConfigStatistic3041Model configStatistic3041)
    {
        _reportId = configStatistic3041.ReportId;
        ConfigStatistic3041 = configStatistic3041;
    }

    public ConfigStatisticModel(ConfigStatistic3040Model configStatistic3040)
    {
        _reportId = configStatistic3040.ReportId;
        ConfigStatistic3040 = configStatistic3040;
    }

    public ConfigStatisticModel(int hpId, int groupId = 0, int reportId = 0, int sortNo = 0)
    {
        _reportId = reportId;
        _staMenu = new StaMenu();
        _staMenu.HpId = hpId;
        _staMenu.GrpId = groupId;
        _staMenu.ReportId = reportId;
        _staMenu.SortNo = sortNo;

        _listStaConf = new List<StaConf>();

        switch ((StatisticReportType)reportId)
        {
            case StatisticReportType.Sta3001:
                ConfigStatistic3001 = new ConfigStatistic3001Model(_staMenu, _listStaConf);
                break;
            case StatisticReportType.Sta3010:
                ConfigStatistic3010 = new ConfigStatistic3010Model(_staMenu, _listStaConf);
                break;
            default:

                break;
        }
    }

    public ConfigStatisticModel(StaMenu staMenu, List<StaConf> listStaConf)
    {
        _reportId = staMenu.ReportId;
        _staMenu = staMenu;
        _listStaConf = listStaConf;

        if (_staMenu == null)
        {
            _staMenu = new StaMenu();
        }

        if (_listStaConf == null)
        {
            _listStaConf = new List<StaConf>();
        }

        switch ((StatisticReportType)staMenu.ReportId)
        {
            case StatisticReportType.Sta3001:
                ConfigStatistic3001 = new ConfigStatistic3001Model(_staMenu, _listStaConf);
                break;
            case StatisticReportType.Sta3010:
                ConfigStatistic3010 = new ConfigStatistic3010Model(_staMenu, _listStaConf);
                break;

        }
    }

    /// <summary>
    /// メニューID
    /// </summary>
    public int MenuId
    {
        get => StaMenu.MenuId;
    }

    /// <summary>
    /// グループID
    /// </summary>
    public int GrpId
    {
        get => StaMenu.GrpId;
        set => StaMenu.GrpId = value;
    }

    /// <summary>
    /// 帳票ID
    /// </summary>
    public int ReportId
    {
        get => StaMenu.ReportId;
        set => StaMenu.ReportId = value;
    }

    /// <summary>
    /// 並び順
    /// </summary>
    /// 
    public int SortNo
    {
        get => StaMenu.SortNo;
        set => StaMenu.SortNo = value;
    }

    /// <summary>
    /// メニュー名称	
    /// </summary>
    public string MenuName
    {
        get => StaMenu.MenuName ?? string.Empty;
        set
        {
            StaMenu.MenuName = value;
        }
    }

    /// <summary>
    /// 帳票タイトル
    /// </summary>
    public string ReportName
    {
        get
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                return ConfigStatistic3001.ReportName;
            }

            if (ReportId == (int)StatisticReportType.Sta3010)
            {
                return ConfigStatistic3010.ReportName;
            }

            return GetValueConf(1);
        }
        set
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                ConfigStatistic3001.ReportName = value;
                return;
            }

            if (ReportId == (int)StatisticReportType.Sta3010)
            {
                ConfigStatistic3010.ReportName = value;
                return;
            }

            var conf = CheckExisConfig(1);
            conf.Val = value;
        }
    }

    /// <summary>
    /// フォームファイル
    /// </summary>
    public string FormReport
    {
        get
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                return ConfigStatistic3001.FormReport;
            }

            if (ReportId == (int)StatisticReportType.Sta3010)
            {
                return ConfigStatistic3010.FormReport;
            }

            return GetValueConf(2);
        }
        set
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                ConfigStatistic3001.FormReport = value;
                return;
            }

            if (ReportId == (int)StatisticReportType.Sta3010)
            {
                ConfigStatistic3010.FormReport = value;
                return;
            }

            var conf = CheckExisConfig(2);
            conf.Val = value;
        }
    }

    /// <summary>
    /// 対象データ(9)
    /// </summary>
    public int TargetData
    {
        get
        {
            return GetValueConf(9).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(9);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 改ページ１
    /// </summary>
    public int BreakPage1
    {
        get
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                return ConfigStatistic3001.BreakPage1;
            }

            if (ReportId == (int)StatisticReportType.Sta3010)
            {
                return ConfigStatistic3010.BreakPage1;
            }

            var result = GetValueConf(10);
            return string.IsNullOrEmpty(result) ? -1 : result.AsInteger();
        }
        set
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                ConfigStatistic3001.BreakPage1 = value;
                return;
            }

            if (ReportId == (int)StatisticReportType.Sta3010)
            {
                ConfigStatistic3010.BreakPage1 = value;
                return;
            }

            var conf = CheckExisConfig(10);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 改ページ２
    /// </summary>
    public int BreakPage2
    {
        get
        {
            return GetValueConf(11).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(11);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 改ページ３
    /// </summary>
    public int BreakPage3
    {
        get
        {
            return GetValueConf(12).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(12);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// ソート順１
    /// </summary>
    public int SortOrder1
    {
        get
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                return ConfigStatistic3001.SortOrder1;
            }

            return GetValueConf(20).AsInteger();
        }
        set
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                ConfigStatistic3001.SortOrder1 = value;
                return;
            }

            var conf = CheckExisConfig(20);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// ソート順１
    ///    0:昇順 1:降順
    /// </summary>
    public int OrderBy1
    {
        get
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                return ConfigStatistic3001.OrderBy1;
            }

            return GetValueConf(21).AsInteger();
        }
        set
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                ConfigStatistic3001.OrderBy1 = value;
                return;
            }

            var conf = CheckExisConfig(21);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// ソート順２
    /// </summary>
    public int SortOrder2
    {
        get
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                return ConfigStatistic3001.SortOrder2;
            }

            return GetValueConf(22).AsInteger();
        }
        set
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                ConfigStatistic3001.SortOrder2 = value;
                return;
            }

            var conf = CheckExisConfig(22);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// ソート順２
    ///    0:昇順 1:降順
    /// </summary>
    public int OrderBy2
    {
        get
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                return ConfigStatistic3001.OrderBy2;
            }

            return GetValueConf(23).AsInteger();
        }
        set
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                ConfigStatistic3001.OrderBy2 = value;
                return;
            }

            var conf = CheckExisConfig(23);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// ソート順３
    /// </summary>
    public int SortOrder3
    {
        get
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                return ConfigStatistic3001.SortOrder3;
            }

            return GetValueConf(24).AsInteger();
        }
        set
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                ConfigStatistic3001.SortOrder3 = value;
                return;
            }

            var conf = CheckExisConfig(24);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// ソート順３
    ///    0:昇順 1:降順
    /// </summary>
    public int OrderBy3
    {
        get
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                return ConfigStatistic3001.OrderBy3;
            }

            return GetValueConf(25).AsInteger();
        }
        set
        {
            if (ReportId == (int)StatisticReportType.Sta3001)
            {
                ConfigStatistic3001.OrderBy3 = value;
                return;
            }

            var conf = CheckExisConfig(25);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 条件
    ///    テスト患者 0:false 1:true
    /// </summary>
    public int TestPatient
    {
        get
        {
            return GetValueConf(30).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(30);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 条件
    ///    受付種別
    /// </summary>
    public string UketukeKbnId
    {
        get
        {
            return GetValueConf(31);
        }
        set
        {
            var conf = CheckExisConfig(31);
            conf.Val = value;
        }
    }

    /// <summary>
    /// 条件
    ///    診療科
    /// </summary>
    public string KaId
    {
        get
        {
            return GetValueConf(32);
        }
        set
        {
            var conf = CheckExisConfig(32);
            conf.Val = value;
        }
    }

    /// <summary>
    /// 条件
    ///    担当医
    /// </summary>
    public string UserId
    {
        get
        {
            return GetValueConf(33);
        }
        set
        {
            var conf = CheckExisConfig(33);
            conf.Val = value;
        }
    }

    /// <summary>
    /// 条件
    ///    支払区分
    /// </summary>
    public string PaymentKbn
    {
        get
        {
            return GetValueConf(34);
        }
        set
        {
            var conf = CheckExisConfig(34);
            conf.Val = value;
        }
    }

    /// <summary>
    /// 請求区分
    /// </summary>
    public int InvoiceKbn
    {
        get
        {
            return GetValueConf(35).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(35);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 請求金額に変更がある患者のみ
    /// </summary>
    public int OnlyPatientInvoiceChange
    {
        get
        {
            return GetValueConf(36).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(36);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 期間外に入金がある場合は未収としない
    /// </summary>
    public int IncludeOutRangeNyukin
    {
        get
        {
            return GetValueConf(39).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(39);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 未収区分
    ///     1:未収
    ///     2:免除
    ///     3:調整
    /// </summary>
    public string MisyuKbns
    {
        get
        {
            return GetValueConf(40);
        }
        set
        {
            var conf = CheckExisConfig(40);
            conf.Val = value.AsString();
        }
    }

    public void AddMisyuKbn(int misyuKbn)
    {
        if (misyuKbn <= 0)
        {
            return;
        }

        string misyuStr = MisyuKbns.Trim(',');
        if (!misyuStr.Contains(misyuKbn.AsString()))
        {
            if (!string.IsNullOrEmpty(misyuStr))
            {
                misyuStr += ",";
            }
            misyuStr += $"{misyuKbn}";
            MisyuKbns = misyuStr;
        }
    }

    public void RemoveMisyuKbn(int misyuKbn)
    {
        if (misyuKbn <= 0)
        {
            return;
        }

        string misyuStr = MisyuKbns.Trim(',');
        if (misyuStr.Contains($",{misyuKbn}"))
        {
            MisyuKbns = misyuStr.Replace($",{misyuKbn}", string.Empty).Trim(',');
        }
        else if (misyuStr.Contains($"{misyuKbn}"))
        {
            MisyuKbns = misyuStr.Replace($"{misyuKbn}", string.Empty).Trim(',');
        }
    }

    public List<int> GetListMisyuKbn()
    {
        List<int> listMisyuKbn = new List<int>();
        if (string.IsNullOrEmpty(MisyuKbns))
        {
            return listMisyuKbn;
        }

        listMisyuKbn = MisyuKbns.Split(',')
                                .Select(x => x.AsInteger())
                                .Where(x => x > 0)
                                .OrderBy(x => x).ToList();
        return listMisyuKbn;
    }

    /// <summary>
    /// 期間From
    /// </summary>
    public int TimeDailyFrom
    {
        get
        {
            return GetValueConf(100).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(100);
            conf.Val = value.AsString();
        }
    }


    /// <summary>
    /// 期間To
    /// </summary>
    public int TimeDailyTo
    {
        get
        {
            return GetValueConf(101).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(101);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 保険種別(37)
    /// </summary>
    public string InsuranceType
    {
        get
        {
            return GetValueConf(37);
        }
        set
        {
            var conf = CheckExisConfig(37);
            conf.Val = value;
        }
    }

    /// <summary>
    /// 診療点数(38)
    /// </summary>
    public int MedicalTreatment
    {
        get
        {
            return GetValueConf(38).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(38);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 保険外金額(39)
    /// </summary>
    public int NonInsuranceAmount
    {
        get
        {
            return GetValueConf(39).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(39);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 対象レセプト(40)
    /// </summary>
    public string TargetReceipt
    {
        get
        {
            return GetValueConf(40);
        }
        set
        {
            var conf = CheckExisConfig(40);
            conf.Val = value;
        }
    }

    /// <summary>
    /// 政令指定都市(41)
    /// </summary>
    public int Designated
    {
        get
        {
            return GetValueConf(41).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(41);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 未精算の来院を未収とする
    /// </summary>
    public int UnPaidVisit
    {
        get
        {
            return GetValueConf(41).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(41);
            conf.Val = value.AsString();
        }
    }


    /// <summary>
    /// 在宅患者(42)
    /// </summary>
    public int HomePatient
    {
        get
        {
            return GetValueConf(42).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(42);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 項目入力欄(43)
    /// </summary>
    public string ItemInput
    {
        get
        {
            return GetValueConf(43);
        }
        set
        {
            var conf = CheckExisConfig(43);
            conf.Val = value;
        }
    }

    /// <summary>
    /// 内訳を表示(44)
    /// </summary>
    public int ShowBreakdown
    {
        get
        {
            return GetValueConf(44).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(44);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 診療識別(44)
    /// </summary>
    public string MedicaIdentification
    {
        get
        {
            return GetValueConf(44);
        }
        set
        {
            var conf = CheckExisConfig(44);
            conf.Val = value;
        }
    }

    /// <summary>
    /// 診療行為区分(45)
    /// </summary>
    public string DiagnosisTreatment
    {
        get
        {
            return GetValueConf(45);
        }
        set
        {
            var conf = CheckExisConfig(45);
            conf.Val = value;
        }
    }

    /// <summary>
    /// 麻毒区分(46)
    /// </summary>
    public string Leprosy
    {
        get
        {
            return GetValueConf(46);
        }
        set
        {
            var conf = CheckExisConfig(46);
            conf.Val = value;
        }
    }

    /// <summary>
    /// 向精神薬区分(47)
    /// </summary>
    public string PsychotropiDrug
    {
        get
        {
            return GetValueConf(47);
        }
        set
        {
            var conf = CheckExisConfig(47);
            conf.Val = value;
        }
    }

    /// <summary>
    /// 検索ワード(48)
    /// </summary>
    public string KeySearch
    {
        get
        {
            return GetValueConf(48);
        }
        set
        {
            var conf = CheckExisConfig(48);
            conf.Val = value;
        }
    }

    /// <summary>
    /// 検索演算子(49)
    /// </summary>
    public int SearchOperator
    {
        get
        {
            return GetValueConf(49).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(49);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 未精算を除く(50)
    /// </summary>
    public int ExcludingUnpaid
    {
        get
        {
            return GetValueConf(50).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(50);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 検索項目の検索オプション 
    /// </summary>
    public int ItemCdOpt
    {
        get
        {
            return GetValueConf(51).AsInteger();
        }
        set
        {
            var conf = CheckExisConfig(51);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 院内院外区分
    /// </summary>
    public string InoutKbn
    {
        get
        {
            return GetValueConf(52);
        }
        set
        {
            var conf = CheckExisConfig(52);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 院内院外区分
    /// </summary>
    public List<int> ListInoutKbn
    {
        get
        {
            if (string.IsNullOrEmpty(InoutKbn))
            {
                return new List<int>();
            }

            var arrTemp = InoutKbn.Split(' ');
            return arrTemp.Select(x => x.AsInteger()).ToList();
        }
    }

    /// <summary>
    /// 後発医薬品区分
    /// </summary>
    public string KohatuKbn
    {
        get
        {
            return GetValueConf(53);
        }
        set
        {
            var conf = CheckExisConfig(53);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 後発医薬品区分
    /// </summary>
    public List<int> ListKohatuKbn
    {
        get
        {
            if (string.IsNullOrEmpty(KohatuKbn))
            {
                return new List<int>();
            }

            var arrTemp = KohatuKbn.Split(' ');
            return arrTemp.Select(x => x.AsInteger()).ToList();
        }
    }

    /// <summary>
    /// 採用区分
    /// </summary>
    public string IsAdopted
    {
        get
        {
            return GetValueConf(54);
        }
        set
        {
            var conf = CheckExisConfig(54);
            conf.Val = value.AsString();
        }
    }

    /// <summary>
    /// 採用区分
    /// </summary>
    public List<int> ListIsAdopted
    {
        get
        {
            if (string.IsNullOrEmpty(IsAdopted))
            {
                return new List<int>();
            }

            var arrTemp = IsAdopted.Split(' ');
            return arrTemp.Select(x => x.AsInteger()).ToList();
        }
    }

    public ModelStatus ModelStatus
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.ModelStatus;
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.ModelStatus;
                default:
                    return ModelStatus.None;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.ModelStatus = value;
                    break;
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.ModelStatus = value;
                    break;
            }
        }
    }

    #region Config Report 3001
    /// <summary>
    /// 薬剤区分－内用薬
    /// </summary>
    public bool IsDrugCategoryInternal
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.DrugCategoryInternal == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.DrugCategoryInternal = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 薬剤区分－外用薬
    /// </summary>
    public bool IsDrugCategoryTopical
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.DrugCategoryTopical == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.DrugCategoryTopical = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 薬剤区分－注射薬
    /// </summary>
    public bool IsDrugCategoryInjection
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.DrugCategoryInjection == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.DrugCategoryInjection = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 薬剤区分－歯科用薬剤
    /// </summary>
    public bool IsDrugCategoryDental
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.DrugCategoryDental == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.DrugCategoryDental = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 薬剤区分－その他
    /// </summary>
    public bool IsDrugCategoryOther
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.DrugCategoryOther == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.DrugCategoryOther = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 麻毒区分－麻毒等以外
    /// </summary>
    public bool IsMalignantCategoryOtherThanNarcotic
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.MalignantCategoryOtherThanNarcotic == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.MalignantCategoryOtherThanNarcotic = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 麻毒区分－麻薬
    /// </summary>
    public bool IsMalignantCategoryNarcotic
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.MalignantCategoryNarcotic == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.MalignantCategoryNarcotic = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 麻毒区分－毒薬
    /// </summary>
    public bool IsMalignantCategoryPoisonousDrug
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.MalignantCategorynPoisonousDrug == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.MalignantCategorynPoisonousDrug = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 麻毒区分－覚せい剤
    /// </summary>
    public bool IsMalignantCategoryAntipsychotics
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.MalignantCategoryAntipsychotics == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.MalignantCategoryAntipsychotics = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 麻毒区分－向精神薬
    ///    0:false 1:true
    /// </summary>
    public bool IsMalignantCategoryPsychotropicDrug
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.MalignantCategoryPsychotropicDrug == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.MalignantCategoryPsychotropicDrug = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 向精神薬区分－向精神薬以外
    ///    0:false 1:true
    /// </summary>
    public bool IsPsychotropicDrugCategoryOtherThanPsychotropicDrugs
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.PsychotropicDrugCategoryOtherThanPsychotropicDrugs == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.PsychotropicDrugCategoryOtherThanPsychotropicDrugs = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 向精神薬区分－抗不安薬
    /// </summary>
    public bool IsPsychotropicDrugCategoryAnxiolytics
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.PsychotropicDrugCategoryAnxiolytics == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.PsychotropicDrugCategoryAnxiolytics = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 向精神薬区分－睡眠薬
    /// </summary>
    public bool IsPsychotropicDrugCategorySleepingPills
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.PsychotropicDrugCategorySleepingPills == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.PsychotropicDrugCategorySleepingPills = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 向精神薬区分－抗うつ薬
    /// </summary>
    public bool IsPsychotropicDrugCategoryAntidepressants
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.PsychotropicDrugCategoryAntidepressants == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.PsychotropicDrugCategoryAntidepressants = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 向精神薬区分－抗精神病薬
    /// </summary>
    public bool IsPsychotropicDrugCategoryAntipsychoticDrugs
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.PsychotropicDrugCategoryAntipsychoticDrugs == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.PsychotropicDrugCategoryAntipsychoticDrugs = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 後発フラグ－先発品（後発品なし）
    /// </summary>
    public bool IsGeneralFlagOriginalProductNoGeneric
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.GeneralFlagOriginalProductNoGeneric == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.GeneralFlagOriginalProductNoGeneric = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 後発フラグ－先発品（後発品あり）
    /// </summary>
    public bool IsGeneralFlagOriginalProductWithGeneric
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.GeneralFlagOriginalProductWithGeneric == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.GeneralFlagOriginalProductWithGeneric = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 後発フラグ－後発品
    /// </summary>
    public bool IsGeneralFlagGeneralProcduct
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.GeneralFlagGeneralProcduct == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.GeneralFlagGeneralProcduct = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 開始日－From
    /// </summary>
    public int StartDateFrom
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.StartDateFrom;
                default:
                    return 0;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.StartDateFrom = value;
                    break;
            }

            if (value > 0 && StartDateTo == 0)
            {
                StartDateTo = value;
            }
        }
    }

    public string StartDateFromStr
    {
        get
        {
            return CIUtil.SDateToShowSDate(StartDateFrom);
        }
        set
        {
            int dateInt = CIUtil.ShowWDateToSDate(value.AsString());
            if (dateInt == 0)
            {
                dateInt = CIUtil.ShowSDateToSDate(value.AsString());
            }

            StartDateFrom = dateInt;
        }
    }

    public DateTime? StartDateFromDateTime
    {
        get
        {
            if (StartDateFrom <= 0)
            {
                return null;
            }

            return CIUtil.IntToDate(StartDateFrom);
        }
        set
        {
            StartDateFrom = CIUtil.DateTimeToInt((DateTime)value);
        }
    }

    /// <summary>
    /// 開始日－To
    /// </summary>
    public int StartDateTo
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.StartDateTo;
                default:
                    return 0;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.StartDateTo = value;
                    break;
            }
        }
    }

    public string StartDateToStr
    {
        get
        {
            return CIUtil.SDateToShowSDate(StartDateTo);
        }
        set
        {
            int dateInt = CIUtil.ShowWDateToSDate(value.AsString());
            if (dateInt == 0)
            {
                dateInt = CIUtil.ShowSDateToSDate(value.AsString());
            }

            StartDateTo = dateInt;
        }
    }

    public DateTime? StartDateToDateTime
    {
        get
        {
            if (StartDateTo <= 0)
            {
                return null;
            }

            return CIUtil.IntToDate(StartDateTo);
        }
        set
        {
            StartDateTo = CIUtil.DateTimeToInt((DateTime)value);
        }
    }

    /// <summary>
    /// 終了日－From
    /// </summary>
    public int EndDateFrom
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.EndDateFrom;
                default:
                    return 0;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.EndDateFrom = value;
                    break;
            }

            if (value > 0 && EndDateTo == 0)
            {
                EndDateTo = value;
            }
        }
    }

    public string EndDateFromStr
    {
        get
        {
            return CIUtil.SDateToShowSDate(EndDateFrom);
        }
        set
        {
            int dateInt = CIUtil.ShowWDateToSDate(value.AsString());
            if (dateInt == 0)
            {
                dateInt = CIUtil.ShowSDateToSDate(value.AsString());
            }

            EndDateFrom = dateInt;
        }
    }

    public DateTime? EndDateFromDateTime
    {
        get
        {
            if (EndDateFrom <= 0)
            {
                return null;
            }

            return CIUtil.IntToDate(EndDateFrom);
        }
        set
        {
            EndDateFrom = CIUtil.DateTimeToInt((DateTime)value);
        }
    }

    /// <summary>
    /// 終了日－To
    /// </summary>
    public int EndDateTo
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.EndDateTo;
                default:
                    return 0;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.EndDateTo = value;
                    break;
            }
        }
    }

    public string EndDateToStr
    {
        get
        {
            return CIUtil.SDateToShowSDate(EndDateTo);
        }
        set
        {
            int dateInt = CIUtil.ShowWDateToSDate(value.AsString());
            if (dateInt == 0)
            {
                dateInt = CIUtil.ShowSDateToSDate(value.AsString());
            }

            EndDateTo = dateInt;
        }
    }

    public DateTime? EndDateToDateTime
    {
        get
        {
            if (EndDateTo <= 0)
            {
                return null;
            }

            return CIUtil.IntToDate(EndDateTo);
        }
        set
        {
            EndDateTo = CIUtil.DateTimeToInt((DateTime)value);
        }
    }

    /// <summary>
    /// オプション－一般名
    /// </summary>
    public bool IsOptionCommonName
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.OptionCommonName == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.OptionCommonName = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// オプション－レセプト名称
    /// </summary>
    public bool IsOptionReceiptName
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    return ConfigStatistic3001.OptionReceiptName == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3001:
                    ConfigStatistic3001.OptionReceiptName = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 薬剤区分
    /// </summary>
    public bool IsDrugCategory
    {
        get
        {
            return !IsDrugCategoryDental &&
                   !IsDrugCategoryInternal &&
                   !IsDrugCategoryOther &&
                   !IsDrugCategoryTopical &&
                   !IsDrugCategoryInjection;
        }
        set
        {
            if (value)
            {
                IsDrugCategoryDental = false;
                IsDrugCategoryInternal = false;
                IsDrugCategoryOther = false;
                IsDrugCategoryTopical = false;
                IsDrugCategoryInjection = false;
            }
        }
    }

    /// <summary>
    /// 薬剤区分
    /// </summary>
    public bool IsMalignantCategory
    {
        get
        {
            return !IsMalignantCategoryAntipsychotics &&
                   !IsMalignantCategoryNarcotic &&
                   !IsMalignantCategoryOtherThanNarcotic &&
                   !IsMalignantCategoryPsychotropicDrug &&
                   !IsMalignantCategoryPoisonousDrug;
        }
        set
        {
            if (value)
            {
                IsMalignantCategoryAntipsychotics = false;
                IsMalignantCategoryNarcotic = false;
                IsMalignantCategoryOtherThanNarcotic = false;
                IsMalignantCategoryPoisonousDrug = false;
                IsMalignantCategoryPsychotropicDrug = false;
            }
        }
    }

    /// <summary>
    /// 向精神薬区分
    /// </summary>
    public bool IsPsychotropicDrugCategory
    {
        get
        {
            return !IsPsychotropicDrugCategoryAntidepressants &&
                   !IsPsychotropicDrugCategoryAntipsychoticDrugs &&
                   !IsPsychotropicDrugCategoryAnxiolytics &&
                   !IsPsychotropicDrugCategoryOtherThanPsychotropicDrugs &&
                   !IsPsychotropicDrugCategorySleepingPills;
        }
        set
        {
            if (value)
            {
                IsPsychotropicDrugCategoryAntidepressants = false;
                IsPsychotropicDrugCategoryAntipsychoticDrugs = false;
                IsPsychotropicDrugCategoryAnxiolytics = false;
                IsPsychotropicDrugCategoryOtherThanPsychotropicDrugs = false;
                IsPsychotropicDrugCategorySleepingPills = false;
            }
        }
    }

    /// <summary>
    /// 後発フラグ
    /// </summary>
    public bool IsGeneralFlag
    {
        get
        {
            return !IsGeneralFlagGeneralProcduct &&
                   !IsGeneralFlagOriginalProductNoGeneric &&
                   !IsGeneralFlagOriginalProductWithGeneric;
        }
        set
        {
            if (value)
            {
                IsGeneralFlagGeneralProcduct = false;
                IsGeneralFlagOriginalProductNoGeneric = false;
                IsGeneralFlagOriginalProductWithGeneric = false;
            }
        }
    }
    #endregion

    #region Properties Report 3010
    /// <summary>
    /// セット1-1
    /// </summary>
    public bool IsCheckSet1_1
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set1_1 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set1_1 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set1 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット1-2
    /// </summary>
    public bool IsCheckSet1_2
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set1_2 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set1_2 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set1 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット1-3
    /// </summary>
    public bool IsCheckSet1_3
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set1_3 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set1_3 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set1 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット1-4
    /// </summary>
    public bool IsCheckSet1_4
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set1_4 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set1_4 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set1 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット1-5
    /// </summary>
    public bool IsCheckSet1_5
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set1_5 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set1_5 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set1 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット1-6
    /// </summary>
    public bool IsCheckSet1_6
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set1_6 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set1_6 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set1 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット1
    /// </summary>
    public bool IsCheckSet1
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set1 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set1 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set1_1 = 0;
                        ConfigStatistic3010.Set1_2 = 0;
                        ConfigStatistic3010.Set1_3 = 0;
                        ConfigStatistic3010.Set1_4 = 0;
                        ConfigStatistic3010.Set1_5 = 0;
                        ConfigStatistic3010.Set1_6 = 0;
                    }

                    break;
            }
        }
    }

    /// <summary>
    /// セット2-1
    /// </summary>
    public bool IsCheckSet2_1
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set2_1 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set2_1 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set2 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット2-2
    /// </summary>
    public bool IsCheckSet2_2
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set2_2 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set2_2 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set2 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット2-3
    /// </summary>
    public bool IsCheckSet2_3
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set2_3 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set2_3 = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// セット2-4
    /// </summary>
    public bool IsCheckSet2_4
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set2_4 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set2_4 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set2 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット2-5
    /// </summary>
    public bool IsCheckSet2_5
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set2_5 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set2_5 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set2 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット2-6
    /// </summary>
    public bool IsCheckSet2_6
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set2_6 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set2_6 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set2 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット2
    /// </summary>
    public bool IsCheckSet2
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set2 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set2 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set2_1 = 0;
                        ConfigStatistic3010.Set2_2 = 0;
                        ConfigStatistic3010.Set2_3 = 0;
                        ConfigStatistic3010.Set2_4 = 0;
                        ConfigStatistic3010.Set2_5 = 0;
                        ConfigStatistic3010.Set2_6 = 0;
                    }

                    break;
            }
        }
    }

    /// <summary>
    /// セット3-1
    /// </summary>
    public bool IsCheckSet3_1
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set3_1 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set3_1 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set3 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット3-2
    /// </summary>
    public bool IsCheckSet3_2
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set3_2 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set3_2 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set3 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット3-3
    /// </summary>
    public bool IsCheckSet3_3
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set3_3 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set3_3 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set3 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット3-4
    /// </summary>
    public bool IsCheckSet3_4
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set3_4 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set3_4 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set3 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット3-5
    /// </summary>
    public bool IsCheckSet3_5
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set3_5 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set3_5 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set3 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット3-6
    /// </summary>
    public bool IsCheckSet3_6
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set3_6 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set3_6 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set3 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット3
    /// </summary>
    public bool IsCheckSet3
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set3 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set3 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set3_1 = 0;
                        ConfigStatistic3010.Set3_2 = 0;
                        ConfigStatistic3010.Set3_3 = 0;
                        ConfigStatistic3010.Set3_4 = 0;
                        ConfigStatistic3010.Set3_5 = 0;
                        ConfigStatistic3010.Set3_6 = 0;
                    }

                    break;
            }
        }
    }


    /// <summary>
    /// セット4-1
    /// </summary>
    public bool IsCheckSet4_1
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set4_1 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set4_1 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set4 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット4-2
    /// </summary>
    public bool IsCheckSet4_2
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set4_2 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set4_2 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set4 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット4-3
    /// </summary>
    public bool IsCheckSet4_3
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set4_3 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set4_3 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set4 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット4-4
    /// </summary>
    public bool IsCheckSet4_4
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set4_4 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set4_4 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set4 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット4-5
    /// </summary>
    public bool IsCheckSet4_5
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set4_5 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set4_5 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set4 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット4-6
    /// </summary>
    public bool IsCheckSet4_6
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set4_6 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set4_6 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set4 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット4
    /// </summary>
    public bool IsCheckSet4
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set4 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set4 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set4_1 = 0;
                        ConfigStatistic3010.Set4_2 = 0;
                        ConfigStatistic3010.Set4_3 = 0;
                        ConfigStatistic3010.Set4_4 = 0;
                        ConfigStatistic3010.Set4_5 = 0;
                        ConfigStatistic3010.Set4_6 = 0;
                    }

                    break;
            }
        }
    }


    /// <summary>
    /// セット5-1
    /// </summary>
    public bool IsCheckSet5_1
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set5_1 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set5_1 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set5 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット5-2
    /// </summary>
    public bool IsCheckSet5_2
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set5_2 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set5_2 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set5 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット5-3
    /// </summary>
    public bool IsCheckSet5_3
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set5_3 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set5_3 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set5 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット5-4
    /// </summary>
    public bool IsCheckSet5_4
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set5_4 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set5_4 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set5 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット5-5
    /// </summary>
    public bool IsCheckSet5_5
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set5_5 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set5_5 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set5 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット5-6
    /// </summary>
    public bool IsCheckSet5_6
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set5_6 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set5_6 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set5 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット5
    /// </summary>
    public bool IsCheckSet5
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set5 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set5 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set5_1 = 0;
                        ConfigStatistic3010.Set5_2 = 0;
                        ConfigStatistic3010.Set5_3 = 0;
                        ConfigStatistic3010.Set5_4 = 0;
                        ConfigStatistic3010.Set5_5 = 0;
                        ConfigStatistic3010.Set5_6 = 0;
                    }

                    break;
            }
        }
    }

    /// <summary>
    /// セット6-1
    /// </summary>
    public bool IsCheckSet6_1
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set6_1 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set6_1 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set6 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット6-2
    /// </summary>
    public bool IsCheckSet6_2
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set6_2 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set6_2 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set6 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット6-3
    /// </summary>
    public bool IsCheckSet6_3
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set6_3 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set6_3 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set6 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット6-4
    /// </summary>
    public bool IsCheckSet6_4
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set6_4 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set6_4 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set6 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット6-5
    /// </summary>
    public bool IsCheckSet6_5
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set6_5 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set6_5 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set6 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット6-6
    /// </summary>
    public bool IsCheckSet6_6
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set6_6 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set6_6 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set6 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット6
    /// </summary>
    public bool IsCheckSet6
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set6 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set6 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set6_1 = 0;
                        ConfigStatistic3010.Set6_2 = 0;
                        ConfigStatistic3010.Set6_3 = 0;
                        ConfigStatistic3010.Set6_4 = 0;
                        ConfigStatistic3010.Set6_5 = 0;
                        ConfigStatistic3010.Set6_6 = 0;
                    }

                    break;
            }
        }
    }

    /// <summary>
    /// セット7-1
    /// </summary>
    public bool IsCheckSet7_1
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set7_1 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set7_1 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set7 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット7-2
    /// </summary>
    public bool IsCheckSet7_2
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set7_2 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set7_2 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set7 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット7-3
    /// </summary>
    public bool IsCheckSet7_3
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set7_3 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set7_3 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set7 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット7-4
    /// </summary>
    public bool IsCheckSet7_4
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set7_4 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set7_4 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set7 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット7-5
    /// </summary>
    public bool IsCheckSet7_5
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set7_5 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set7_5 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set7 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット7-6
    /// </summary>
    public bool IsCheckSet7_6
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set7_6 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set7_6 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set7 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット7
    /// </summary>
    public bool IsCheckSet7
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set7 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set7 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set7_1 = 0;
                        ConfigStatistic3010.Set7_2 = 0;
                        ConfigStatistic3010.Set7_3 = 0;
                        ConfigStatistic3010.Set7_4 = 0;
                        ConfigStatistic3010.Set7_5 = 0;
                        ConfigStatistic3010.Set7_6 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット8-1
    /// </summary>
    public bool IsCheckSet8_1
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set8_1 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set8_1 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set8 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット8-2
    /// </summary>
    public bool IsCheckSet8_2
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set8_2 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set8_2 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set8 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット8-3
    /// </summary>
    public bool IsCheckSet8_3
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set8_3 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set8_3 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set8 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット8-4
    /// </summary>
    public bool IsCheckSet8_4
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set8_4 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set8_4 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set8 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット8-5
    /// </summary>
    public bool IsCheckSet8_5
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set8_5 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set8_5 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set8 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット8-6
    /// </summary>
    public bool IsCheckSet8_6
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set8_6 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set8_6 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set8 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット8
    /// </summary>
    public bool IsCheckSet8
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set8 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set8 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set8_1 = 0;
                        ConfigStatistic3010.Set8_2 = 0;
                        ConfigStatistic3010.Set8_3 = 0;
                        ConfigStatistic3010.Set8_4 = 0;
                        ConfigStatistic3010.Set8_5 = 0;
                        ConfigStatistic3010.Set8_6 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット9-1
    /// </summary>
    public bool IsCheckSet9_1
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set9_1 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set9_1 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set9 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット9-2
    /// </summary>
    public bool IsCheckSet9_2
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set9_2 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set9_2 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set9 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット9-3
    /// </summary>
    public bool IsCheckSet9_3
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set9_3 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set9_3 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set9 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット9-4
    /// </summary>
    public bool IsCheckSet9_4
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set9_4 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set9_4 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set9 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット9-5
    /// </summary>
    public bool IsCheckSet9_5
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set9_5 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set9_5 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set9 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット9-6
    /// </summary>
    public bool IsCheckSet9_6
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set9_6 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set9_6 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set9 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット9
    /// </summary>
    public bool IsCheckSet9
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set9 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set9 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set9_1 = 0;
                        ConfigStatistic3010.Set9_2 = 0;
                        ConfigStatistic3010.Set9_3 = 0;
                        ConfigStatistic3010.Set9_4 = 0;
                        ConfigStatistic3010.Set9_5 = 0;
                        ConfigStatistic3010.Set9_6 = 0;
                    }

                    break;
            }
        }
    }

    /// <summary>
    /// セット10-1
    /// </summary>
    public bool IsCheckSet10_1
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set10_1 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set10_1 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set10 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット10-2
    /// </summary>
    public bool IsCheckSet10_2
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set10_2 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set10_2 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set10 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット10-3
    /// </summary>
    public bool IsCheckSet10_3
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set10_3 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set10_3 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set10 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット10-4
    /// </summary>
    public bool IsCheckSet10_4
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set10_4 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set10_4 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set10 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット10-5
    /// </summary>
    public bool IsCheckSet10_5
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set10_5 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set10_5 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set10 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット10-6
    /// </summary>
    public bool IsCheckSet10_6
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set10_6 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set10_6 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set10 = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット10
    /// </summary>
    public bool IsCheckSet10
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.Set10 == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.Set10 = value ? 1 : 0;
                    if (value)
                    {
                        ConfigStatistic3010.Set10_1 = 0;
                        ConfigStatistic3010.Set10_2 = 0;
                        ConfigStatistic3010.Set10_3 = 0;
                        ConfigStatistic3010.Set10_4 = 0;
                        ConfigStatistic3010.Set10_5 = 0;
                        ConfigStatistic3010.Set10_6 = 0;
                    }

                    break;
            }
        }
    }

    /// <summary>
    /// Check any set is checked
    /// </summary>
    /// <returns></returns>
    public bool IsCheckAllSet()
    {
        return IsCheckSet1 || IsCheckSet1_1 || IsCheckSet1_2 || IsCheckSet1_3 || IsCheckSet1_4 || IsCheckSet1_5 || IsCheckSet1_6 ||
               IsCheckSet2 || IsCheckSet2_1 || IsCheckSet2_2 || IsCheckSet2_3 || IsCheckSet2_4 || IsCheckSet2_5 || IsCheckSet2_6 ||
               IsCheckSet3 || IsCheckSet3_1 || IsCheckSet3_2 || IsCheckSet3_3 || IsCheckSet3_4 || IsCheckSet3_5 || IsCheckSet3_6 ||
               IsCheckSet4 || IsCheckSet4_1 || IsCheckSet4_2 || IsCheckSet4_3 || IsCheckSet4_4 || IsCheckSet4_5 || IsCheckSet4_6 ||
               IsCheckSet5 || IsCheckSet5_1 || IsCheckSet5_2 || IsCheckSet5_3 || IsCheckSet5_4 || IsCheckSet5_5 || IsCheckSet5_6 ||
               IsCheckSet6 || IsCheckSet6_1 || IsCheckSet6_2 || IsCheckSet6_3 || IsCheckSet6_4 || IsCheckSet6_5 || IsCheckSet6_6 ||
               IsCheckSet7 || IsCheckSet7_1 || IsCheckSet7_2 || IsCheckSet7_3 || IsCheckSet7_4 || IsCheckSet7_5 || IsCheckSet7_6 ||
               IsCheckSet8 || IsCheckSet8_1 || IsCheckSet8_2 || IsCheckSet8_3 || IsCheckSet8_4 || IsCheckSet8_5 || IsCheckSet8_6 ||
               IsCheckSet9 || IsCheckSet9_1 || IsCheckSet9_2 || IsCheckSet9_3 || IsCheckSet9_4 || IsCheckSet9_5 || IsCheckSet9_6 ||
               IsCheckSet10 || IsCheckSet10_1 || IsCheckSet10_2 || IsCheckSet10_3 || IsCheckSet10_4 || IsCheckSet10_5 || IsCheckSet10_6;
    }

    /// <summary>
    /// 対象データ-すべて
    /// </summary>
    public bool IsTargetAllData
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.TargetData == 0;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    if (value)
                    {
                        ConfigStatistic3010.TargetData = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 対象データ-期限切れ項目
    /// </summary>
    public bool IsTargetExpiredItem
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.TargetData == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    if (value)
                    {
                        ConfigStatistic3010.TargetData = 1;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 対象データ-項目選択
    /// </summary>
    public bool IsTargetItemSelection
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.TargetData == 2;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    if (value)
                    {
                        ConfigStatistic3010.TargetData = 2;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// セット内の他の項目
    /// </summary>
    public bool IsOtherItemOpt
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.OtherItemOpt == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.OtherItemOpt = value ? 1 : 0;
                    break;
            }
        }
    }

    /// <summary>
    /// 対象データ-フリーコメント
    /// </summary>
    public bool IsTargetFreeComment
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.TargetData == 3;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    if (value)
                    {
                        ConfigStatistic3010.TargetData = 3;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 対象データ-部位
    /// </summary>
    public bool IsTargetPart
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.TargetData == 4;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    if (value)
                    {
                        ConfigStatistic3010.TargetData = 4;
                    }
                    break;
            }
        }
    }

    public bool IsOtherItemOptEnable
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.TargetData > 0;
            }

            return false;
        }
    }

    /// <summary>
    /// 検索ワード
    /// </summary>
    public string SearchWord
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.SearchWord;
                default:
                    return string.Empty;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    ConfigStatistic3010.SearchWord = value;
                    break;
            }
        }
    }

    /// <summary>
    /// 検索ワードの検索オプション -Or
    /// </summary>
    public bool IsSearchOr
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.SearchOpt == 0;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    if (value)
                    {
                        ConfigStatistic3010.SearchOpt = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 検索ワードの検索オプション-And
    /// </summary>
    public bool IsSearchAnd
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.SearchOpt == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    if (value)
                    {
                        ConfigStatistic3010.SearchOpt = 1;
                    }
                    break;
            }
        }
    }

    //public InputItemTenMstViewModel ListTenMstInput
    //{
    //    get
    //    {
    //        switch ((StatisticReportType)ReportId)
    //        {
    //            case StatisticReportType.Sta3010:
    //                return ConfigStatistic3010.ListTenMstInput;
    //            default:
    //                return new InputItemTenMstViewModel();
    //        }
    //    }
    //    set
    //    {
    //        switch ((StatisticReportType)ReportId)
    //        {
    //            case StatisticReportType.Sta3010:
    //                ConfigStatistic3010.ListTenMstInput = value;
    //                break;
    //        }
    //    }
    //}

    /// <summary>
    /// 検索項目の検索オプション -Or
    /// </summary>
    public bool IsSearchItemCdOr
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.ItemCdOpt == 0;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    if (value)
                    {
                        ConfigStatistic3010.ItemCdOpt = 0;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 検索項目の検索オプション-And
    /// </summary>
    public bool IsSearchItemCdAnd
    {
        get
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    return ConfigStatistic3010.ItemCdOpt == 1;
                default:
                    return false;
            }
        }
        set
        {
            switch ((StatisticReportType)ReportId)
            {
                case StatisticReportType.Sta3010:
                    if (value)
                    {
                        ConfigStatistic3010.ItemCdOpt = 1;
                    }
                    break;
            }
        }
    }

    public List<string> ListSetKbnName { get; set; }

    /// <summary>
    /// セット区分名称1
    /// </summary>
    public string SetName1
    {
        get
        {
            if (ListSetKbnName?.Count() > 0)
            {
                return ListSetKbnName[0];
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// セット区分名称2
    /// </summary>
    public string SetName2
    {
        get
        {
            if (ListSetKbnName?.Count() > 1)
            {
                return ListSetKbnName[1];
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// セット区分名称3
    /// </summary>
    public string SetName3
    {
        get
        {
            if (ListSetKbnName?.Count() > 2)
            {
                return ListSetKbnName[2];
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// セット区分名称4
    /// </summary>
    public string SetName4
    {
        get
        {
            if (ListSetKbnName?.Count() > 3)
            {
                return ListSetKbnName[3];
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// セット区分名称5
    /// </summary>
    public string SetName5
    {
        get
        {
            if (ListSetKbnName?.Count() > 4)
            {
                return ListSetKbnName[4];
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// セット区分名称6
    /// </summary>
    public string SetName6
    {
        get
        {
            if (ListSetKbnName?.Count() > 5)
            {
                return ListSetKbnName[5];
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// セット区分名称7
    /// </summary>
    public string SetName7
    {
        get
        {
            if (ListSetKbnName?.Count() > 6)
            {
                return ListSetKbnName[6];
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// セット区分名称8
    /// </summary>
    public string SetName8
    {
        get
        {
            if (ListSetKbnName?.Count() > 7)
            {
                return ListSetKbnName[7];
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// セット区分名称9
    /// </summary>
    public string SetName9
    {
        get
        {
            if (ListSetKbnName?.Count() > 8)
            {
                return ListSetKbnName[8];
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// セット区分名称10
    /// </summary>
    public string SetName10
    {
        get
        {
            if (ListSetKbnName?.Count() > 9)
            {
                return ListSetKbnName[9];
            }

            return string.Empty;
        }
    }

    public int TabSelectedIndex { get; set; }
    #endregion

    public ConfigStatisticModel CopyConfig()
    {
        var confCopy = new ConfigStatisticModel(this.GrpId, this.ReportId, this.SortNo);
        if (this.ReportId == (int)StatisticReportType.Sta3001)
        {
            confCopy.ConfigStatistic3001.CopyConfig(this.ConfigStatistic3001);
        }
        else if (this.ReportId == (int)StatisticReportType.Sta3010)
        {
            confCopy.ConfigStatistic3010.CopyConfig(this.ConfigStatistic3010);
        }
        else
        {
            confCopy.MenuName = this.MenuName;
            confCopy.ReportName = this.ReportName;
            confCopy.FormReport = this.FormReport;
            confCopy.TestPatient = this.TestPatient;
            confCopy.KaId = this.KaId;
            confCopy.UserId = this.UserId;
            switch (this.ReportId)
            {
                case 1001:
                    CopyReport1001(confCopy);
                    break;
                case 1002:
                    CopyReport1002(confCopy);
                    break;
                case 1010:
                    CopyReport1010(confCopy);
                    break;
                case 2001:
                case 2002:
                    CopyReport2001(confCopy);
                    break;
                case 2003:
                    CopyReport2003(confCopy);
                    break;
                case 2010:
                    CopyReport2010(confCopy);
                    break;
                case 2011:
                    CopyReport2011(confCopy);
                    break;
                case 2020:
                    CopyReport2020(confCopy);
                    break;
            }
        }

        return confCopy;
    }

    private void CopyReport2020(ConfigStatisticModel confCopy)
    {
        confCopy.TargetData = this.TargetData;
        confCopy.BreakPage1 = this.BreakPage1;
        confCopy.BreakPage2 = this.BreakPage2;
        confCopy.BreakPage3 = this.BreakPage3;

        confCopy.SortOrder1 = this.SortOrder1;
        confCopy.OrderBy1 = this.OrderBy1;
        confCopy.SortOrder2 = this.SortOrder2;
        confCopy.OrderBy2 = this.OrderBy2;
        confCopy.SortOrder3 = this.SortOrder3;
        confCopy.OrderBy3 = this.OrderBy3;

        confCopy.MedicaIdentification = this.MedicaIdentification;
        confCopy.DiagnosisTreatment = this.DiagnosisTreatment;
        confCopy.Leprosy = this.Leprosy;
        confCopy.PsychotropiDrug = this.PsychotropiDrug;
        confCopy.KeySearch = this.KeySearch;
        confCopy.SearchOperator = this.SearchOperator;
        confCopy.ItemInput = this.ItemInput;
        confCopy.ItemCdOpt = this.ItemCdOpt;
        confCopy.InoutKbn = this.InoutKbn;
        confCopy.KohatuKbn = this.KohatuKbn;
        confCopy.IsAdopted = this.IsAdopted;
    }

    private void CopyReport2011(ConfigStatisticModel confCopy)
    {
        confCopy.BreakPage1 = this.BreakPage1;
        confCopy.BreakPage2 = this.BreakPage2;
        confCopy.BreakPage3 = this.BreakPage3;
        confCopy.TargetReceipt = this.TargetReceipt;
        confCopy.HomePatient = this.HomePatient;
        confCopy.ShowBreakdown = this.ShowBreakdown;
        confCopy.ItemInput = this.ItemInput;
    }

    private void CopyReport2010(ConfigStatisticModel confCopy)
    {
        confCopy.BreakPage1 = this.BreakPage1;
        confCopy.BreakPage2 = this.BreakPage2;
        confCopy.BreakPage3 = this.BreakPage3;
        confCopy.Designated = this.Designated;
        confCopy.InsuranceType = this.InsuranceType;
        confCopy.TargetReceipt = this.TargetReceipt;
    }

    private void CopyReport2003(ConfigStatisticModel confCopy)
    {
        confCopy.BreakPage1 = this.BreakPage1;
        confCopy.BreakPage2 = this.BreakPage2;
        confCopy.SortOrder1 = this.SortOrder1;
        confCopy.OrderBy1 = this.OrderBy1;
        confCopy.ExcludingUnpaid = this.ExcludingUnpaid;
        confCopy.InsuranceType = this.InsuranceType;
        confCopy.MedicalTreatment = this.MedicalTreatment;
        confCopy.NonInsuranceAmount = this.NonInsuranceAmount;
    }

    private void CopyReport2001(ConfigStatisticModel confCopy)
    {
        confCopy.ExcludingUnpaid = this.ExcludingUnpaid;
        confCopy.BreakPage1 = this.BreakPage1;
        confCopy.BreakPage2 = this.BreakPage2;
        if (confCopy.ReportId == (int)StatisticReportType.Sta2001)
        {
            confCopy.PaymentKbn = this.PaymentKbn;
        }
    }

    private void CopyReport1010(ConfigStatisticModel confCopy)
    {
        confCopy.BreakPage1 = this.BreakPage1;
        confCopy.BreakPage2 = this.BreakPage2;

        confCopy.SortOrder1 = this.SortOrder1;
        confCopy.OrderBy1 = this.OrderBy1;
        confCopy.SortOrder2 = this.SortOrder2;
        confCopy.OrderBy2 = this.OrderBy2;
        confCopy.SortOrder3 = this.SortOrder3;
        confCopy.OrderBy3 = this.OrderBy3;

        confCopy.InvoiceKbn = this.InvoiceKbn;
        confCopy.OnlyPatientInvoiceChange = this.OnlyPatientInvoiceChange;
        confCopy.IncludeOutRangeNyukin = this.IncludeOutRangeNyukin;
        confCopy.MisyuKbns = this.MisyuKbns;
        confCopy.UnPaidVisit = this.UnPaidVisit;
    }

    private void CopyReport1002(ConfigStatisticModel confCopy)
    {
        confCopy.TimeDailyFrom = this.TimeDailyFrom;
        confCopy.TimeDailyTo = this.TimeDailyTo;
        confCopy.BreakPage1 = this.BreakPage1;
        confCopy.BreakPage2 = this.BreakPage2;
        confCopy.BreakPage3 = this.BreakPage3;
        confCopy.ExcludingUnpaid = this.ExcludingUnpaid;
        confCopy.UketukeKbnId = this.UketukeKbnId;
        confCopy.PaymentKbn = this.PaymentKbn;
    }

    private void CopyReport1001(ConfigStatisticModel confCopy)
    {
        confCopy.TimeDailyFrom = this.TimeDailyFrom;
        confCopy.TimeDailyTo = this.TimeDailyTo;
        confCopy.BreakPage1 = this.BreakPage1;
        confCopy.BreakPage2 = this.BreakPage2;
        confCopy.BreakPage3 = this.BreakPage3;

        confCopy.SortOrder1 = this.SortOrder1;
        confCopy.OrderBy1 = this.OrderBy1;
        confCopy.SortOrder2 = this.SortOrder2;
        confCopy.OrderBy2 = this.OrderBy2;
        confCopy.SortOrder3 = this.SortOrder3;
        confCopy.OrderBy3 = this.OrderBy3;

        confCopy.ExcludingUnpaid = this.ExcludingUnpaid;
        confCopy.UketukeKbnId = this.UketukeKbnId;
        confCopy.PaymentKbn = this.PaymentKbn;
    }

    public void SetMenuId(int menuId)
    {
        if (_staMenu.MenuId != menuId)
        {
            _staMenu.MenuId = menuId;
        }

        foreach (var item in ListStaConf)
        {
            if (item.MenuId != menuId)
            {
                item.MenuId = menuId;
            }
        }
    }

    #region Private Method

    private StaConf CheckExisConfig(int config)
    {
        var conf = ListStaConf.Find(x => x.ConfId == config);

        if (conf == null)
        {
            conf = new StaConf();
            conf.MenuId = StaMenu.MenuId;
            conf.ConfId = config;

            ListStaConf.Add(conf);
        }

        return conf;
    }

    private string GetValueConf(int config)
    {
        var conf = ListStaConf.Find(x => x.ConfId == config);

        if (conf == null)
        {
            return string.Empty;
        }

        return conf.Val ?? string.Empty;
    }
    #endregion
}
