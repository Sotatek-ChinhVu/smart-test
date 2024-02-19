using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;

namespace Reporting.DailyStatic.Model;

public class ConfigStatistic3030Model : StatisticModelBase
{
    public ConfigStatistic3030Model(StaMenu staMenu, List<StaConf> listStaConf)
    {
        StaMenu = staMenu;
        ListStaConf = listStaConf;

        if (StaMenu == null)
        {
            StaMenu = new StaMenu();
        }

        if (ListStaConf == null)
        {
            ListStaConf = new List<StaConf>();
        }

        if (MenuId > 0)
        {
            ModelStatus = ModelStatus.None;
        }
        else
        {
            ModelStatus = ModelStatus.Added;
        }

        SetTimeDefault();
    }

    public ConfigStatistic3030Model(int hpId, int groupId = 0, int reportId = 0, int sortNo = 0)
    {
        StaMenu = new StaMenu();
        StaMenu.HpId = hpId;
        StaMenu.GrpId = groupId;
        StaMenu.ReportId = reportId;
        StaMenu.SortNo = sortNo;

        ListStaConf = new List<StaConf>();
        ModelStatus = ModelStatus.Added;
        SetTimeDefault();
    }

    private void SetTimeDefault()
    {
        DateTime dateSystem = CIUtil.GetJapanDateTimeNow().Date;
        int dateSystemInt = CIUtil.DateTimeToInt(dateSystem);

        StartDateFrom = (dateSystem.Year * 100 + dateSystem.Month) * 100 + 1;
        StartDateTo = dateSystemInt;

        EnableRangeFrom = CIUtil.DateTimeToInt(dateSystem.AddYears(-1));
        EnableRangeTo = dateSystemInt;
    }

    /// <summary>
    /// Hospital ID
    /// </summary>
    public int HpId
    {
        get => StaMenu.HpId;
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
            if (ModelStatus == ModelStatus.None)
            {
                ModelStatus = ModelStatus.Modified;
            }
        }
    }

    /// <summary>
    /// 帳票タイトル
    /// </summary>
    public string ReportName
    {
        get
        {
            return GetValueConf(HpId, 1);
        }
        set
        {
            SettingConfig(HpId, 1, value.AsString());
        }
    }

    /// <summary>
    /// フォームファイル
    /// </summary>
    public string FormReport
    {
        get
        {
            return GetValueConf(HpId, 2);
        }
        set
        {
            SettingConfig(HpId, 2, value.AsString());
        }
    }

    /// <summary>
    /// 改ページ１
    /// </summary>
    public int PageBreak1
    {
        get
        {
            return GetValueConf(HpId, 10).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 10, value.AsString());
        }
    }

    /// <summary>
    /// ソート順１
    /// </summary>
    public int SortOrder1
    {
        get
        {
            return GetValueConf(HpId, 20).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 20, value.AsString());
        }
    }

    /// <summary>
    /// ソート順１
    ///    0:昇順 1:降順
    /// </summary>
    public int SortOpt1
    {
        get
        {
            return GetValueConf(HpId, 21).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 21, value.AsString());
        }
    }

    /// <summary>
    /// ソート順２
    /// </summary>
    public int SortOrder2
    {
        get
        {
            return GetValueConf(HpId, 22).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 22, value.AsString());
        }
    }

    /// <summary>
    /// ソート順２
    ///    0:昇順 1:降順
    /// </summary>
    public int SortOpt2
    {
        get
        {
            return GetValueConf(HpId, 23).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 23, value.AsString());
        }
    }

    /// <summary>
    /// ソート順３
    /// </summary>
    public int SortOrder3
    {
        get
        {
            return GetValueConf(HpId, 24).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 24, value.AsString());
        }
    }

    /// <summary>
    /// ソート順３
    ///    0:昇順 1:降順
    /// </summary>
    public int SortOpt3
    {
        get
        {
            return GetValueConf(HpId, 25).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 25, value.AsString());
        }
    }

    public bool IsIncludeTestPt
    {
        get
        {
            return GetValueConf(HpId, 30).AsBool();
        }
        set
        {
            SettingConfig(HpId, 30, value.AsString());
        }
    }

    /// <summary>
    /// 開始日From
    /// </summary>
    public int StartDateFrom { get; set; }

    /// <summary>
    /// 開始日To
    /// </summary>
    public int StartDateTo { get; set; }

    /// <summary>
    /// 転帰日From
    /// </summary>
    public int TenkiDateFrom { get; set; }

    /// <summary>
    /// 転帰日To
    /// </summary>
    public int TenkiDateTo { get; set; }

    /// <summary>
    /// 有効期間From
    /// </summary>
    public int EnableRangeFrom { get; set; }

    /// <summary>
    /// 有効期間To
    /// </summary>
    public int EnableRangeTo { get; set; }

    /// <summary>
    /// 転帰区分
    /// </summary>
    public string TenkiKbn
    {
        get
        {
            return GetValueConf(HpId, 40);
        }
        set
        {
            SettingConfig(HpId, 40, value);
        }
    }


    /// <summary>
    /// 転帰区分
    ///     0:継続 1:治ゆ 2:中止 3:死亡 9:その他
    /// </summary>
    public List<int> ListTenkiKbn
    {
        get
        {
            if (string.IsNullOrEmpty(TenkiKbn))
            {
                return new List<int>();
            }

            var arrTemp = TenkiKbn.Split(' ');
            return arrTemp.Select(x => x.AsInteger()).ToList();
        }
    }

    /// <summary>
    /// 主病名
    /// </summary>
    public string SyubyoKbn
    {
        get
        {
            return GetValueConf(HpId, 41);
        }
        set
        {
            SettingConfig(HpId, 41, value);
        }
    }

    /// <summary>
    /// 主病名
    ///     0:主病名以外 1:主病名
    /// </summary>
    public List<int> ListSyubyo
    {
        get
        {
            if (string.IsNullOrEmpty(SyubyoKbn))
            {
                return new List<int>();
            }

            var arrTemp = SyubyoKbn.Split(' ');
            return arrTemp.Select(x => x.AsInteger()).ToList();
        }
    }

    /// <summary>
    /// 疑い
    ///    0:疑い以外 1:疑い
    /// </summary>
    public string DoubtKbn
    {
        get
        {
            return GetValueConf(HpId, 42);
        }
        set
        {
            SettingConfig(HpId, 42, value);
        }
    }

    /// <summary>
    /// 疑い
    ///     0:疑い以外 1:疑い
    /// </summary>
    public List<int> ListDoubt
    {
        get
        {
            if (string.IsNullOrEmpty(DoubtKbn))
            {
                return new List<int>();
            }

            var arrTemp = DoubtKbn.Split(' ');
            return arrTemp.Select(x => x.AsInteger()).ToList();
        }
    }

    /// <summary>
    /// 患者選択
    ///    患者番号
    /// </summary>
    public string PtId
    {
        get
        {
            return GetValueConf(HpId, 50);
        }
        set
        {
            SettingConfig(HpId, 50, value);
        }
    }

    /// <summary>
    /// 患者番号
    /// </summary>
    public List<long> ListPtId
    {
        get
        {
            if (string.IsNullOrEmpty(PtId))
            {
                return new List<long>();
            }

            var arrTemp = PtId.Split(' ');
            return arrTemp.Select(x => x.AsLong()).ToList();
        }
    }

    /// <summary>
    /// 来院日From
    /// </summary>
    public int SinDateFrom { get; set; }

    public ConfigStatistic3030Model(int sinDateFrom)
    {
        SinDateFrom = sinDateFrom;
    }

    /// <summary>
    /// 来院日To
    /// </summary>
    public int SinDateTo { get; set; }

    /// <summary>
    /// 病名検索ワード 
    /// </summary>
    public string ByomeiWords
    {
        get
        {
            return GetValueConf(HpId, 60);
        }
        set
        {
            SettingConfig(HpId, 60, value);
        }
    }

    /// <summary>
    /// 病名検索ワード の検索オプション 
    ///     0:or 
    ///     1:and
    /// </summary>
    public int ByomeiWordOpt
    {
        get
        {
            return GetValueConf(HpId, 61).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 61, value.AsString());
        }
    }

    /// <summary>
    /// 検索病名 
    /// </summary>
    public string ByomeiCd
    {
        get
        {
            return GetValueConf(HpId, 62);
        }
        set
        {
            SettingConfig(HpId, 62, value);
        }
    }

    /// <summary>
    /// Get 検索病名
    /// </summary>
    /// <returns></returns>
    public List<string> ListByomeiCdAll
    {
        get
        {
            if (string.IsNullOrEmpty(ByomeiCd))
            {
                return new List<string>();
            }

            var arrTemp = ByomeiCd.Split(',');
            return arrTemp.Where(x => !string.IsNullOrEmpty(x)).ToList();
        }
    }

    public List<string> ListByomeiCd
    {
        get
        {
            if (string.IsNullOrEmpty(ByomeiCd))
            {
                return new List<string>();
            }

            var arrTemp = ByomeiCd.Split(',');
            return arrTemp.Where(x => !string.IsNullOrEmpty(x) && !x.Equals(ByomeiConstant.FreeWordCode)).ToList();
        }
    }

    /// <summary>
    /// 検索病名内の未コード化傷病名 
    /// </summary>
    public string FreeByomei
    {
        get
        {
            return GetValueConf(HpId, 63);
        }
        set
        {
            SettingConfig(HpId, 63, value);
        }
    }

    /// <summary>
    /// Get 未コード化傷病名 
    /// </summary>
    /// <returns></returns>
    public List<string> ListFreeByomei
    {
        get
        {
            List<string> listFree = new List<string>();
            string freeStr = FreeByomei;

            while (!string.IsNullOrEmpty(freeStr))
            {
                int length = 0;
                if (freeStr.StartsWith("/"))
                {
                    int indexSpace = freeStr.IndexOf(" ");
                    length = freeStr.Substring(1, indexSpace - 1).AsInteger();
                    freeStr = freeStr.Remove(0, indexSpace + 1);
                }

                if (length <= 0)
                {
                    length = freeStr.Length;
                }

                listFree.Add(freeStr.Substring(0, length));
                freeStr = freeStr.Remove(0, length);
            }

            return listFree;
        }
    }

    /// <summary>
    /// 検索病名の検索オプション（患者毎）
    ///     0:or 1:and
    /// </summary>
    public int ByomeiCdOpt
    {
        get
        {
            return GetValueConf(HpId, 64).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 64, value.AsString());
        }
    }

    /// <summary>
    /// 算定/オーダー
    ///     0:算定 1:オーダー
    /// </summary>
    public int SanteiOrder
    {
        get
        {
            return GetValueConf(HpId, 70).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 70, value.AsString());
        }
    }

    /// <summary>
    /// 項目検索ワード 
    /// </summary>
    public string ItemWords
    {
        get
        {
            return GetValueConf(HpId, 71);
        }
        set
        {
            SettingConfig(HpId, 71, value);
        }
    }

    /// <summary>
    /// 項目検索ワードの検索オプション（項目毎）
    ///     0:or 
    ///     1:and
    /// </summary>
    public int ItemWordOpt
    {
        get
        {
            return GetValueConf(HpId, 72).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 72, value.AsString());
        }
    }

    /// <summary>
    /// 検索項目 
    /// </summary>
    public string ItemCd
    {
        get
        {
            return GetValueConf(HpId, 73);
        }
        set
        {
            SettingConfig(HpId, 73, value);
        }
    }

    /// <summary>
    /// Get 検索項目
    /// </summary>
    /// <returns></returns>
    public List<string> ListItemCd
    {
        get
        {
            if (string.IsNullOrEmpty(ItemCd))
            {
                return new List<string>();
            }

            return ItemCd.Split(' ').ToList();
        }
    }

    /// <summary>
    /// 検索項目の検索オプション（患者毎）
    ///     0:or 
    ///     1:and
    /// </summary>
    public int ItemCdOpt
    {
        get
        {
            return GetValueConf(HpId, 74).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 74, value.AsString());
        }
    }

    public void CopyConfig(ConfigStatistic3030Model configSource)
    {
        this.MenuName = configSource.MenuName;
        this.ReportName = configSource.ReportName;
        this.FormReport = configSource.FormReport;
        this.PageBreak1 = configSource.PageBreak1;
        this.SortOrder1 = configSource.SortOrder1;
        this.SortOrder2 = configSource.SortOrder2;
        this.SortOrder3 = configSource.SortOrder3;
        this.SortOpt1 = configSource.SortOpt1;
        this.SortOpt2 = configSource.SortOpt2;
        this.SortOpt3 = configSource.SortOpt3;
        this.IsIncludeTestPt = configSource.IsIncludeTestPt;
        this.StartDateFrom = configSource.StartDateFrom;
        this.StartDateTo = configSource.StartDateTo;
        this.TenkiDateFrom = configSource.TenkiDateFrom;
        this.TenkiDateTo = configSource.TenkiDateTo;
        this.EnableRangeFrom = configSource.EnableRangeFrom;
        this.EnableRangeTo = configSource.EnableRangeTo;
        this.TenkiKbn = configSource.TenkiKbn;
        this.SyubyoKbn = configSource.SyubyoKbn;
        this.DoubtKbn = configSource.DoubtKbn;
        this.PtId = configSource.PtId;
        this.SinDateFrom = configSource.SinDateFrom;
        this.SinDateTo = configSource.SinDateTo;
        this.ByomeiWords = configSource.ByomeiWords;
        this.ByomeiWordOpt = configSource.ByomeiWordOpt;
        this.ByomeiCd = configSource.ByomeiCd;
        this.FreeByomei = configSource.FreeByomei;
        this.ByomeiCdOpt = configSource.ByomeiCdOpt;
        this.SanteiOrder = configSource.SanteiOrder;
        this.ItemWords = configSource.ItemWords;
        this.ItemWordOpt = configSource.ItemWordOpt;
        this.ItemCd = configSource.ItemCd;
        this.ItemCdOpt = configSource.ItemCdOpt;
    }
}
