using Entity.Tenant;
using Helper.Constants;
using Helper.Extension;

namespace Reporting.DailyStatic.Model;

public class ConfigStatistic3020Model : StatisticModelBase
{
    public ConfigStatistic3020Model()
    {
    }

    public ConfigStatistic3020Model(StaMenu staMenu, List<StaConf> listStaConf)
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
    }

    public ConfigStatistic3020Model(int groupId = 0, int reportId = 0, int sortNo = 0)
    {
        StaMenu = new StaMenu();
        StaMenu.HpId = Session.HospitalID;
        StaMenu.GrpId = groupId;
        StaMenu.ReportId = reportId;
        StaMenu.SortNo = sortNo;

        ListStaConf = new List<StaConf>();
        ModelStatus = ModelStatus.Added;
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
        get => StaMenu.MenuName ?? String.Empty;
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
            return GetValueConf(1);
        }
        set
        {
            SettingConfig(1, value.AsString());
        }
    }

    /// <summary>
    /// フォームファイル
    /// </summary>
    public string FormReport
    {
        get
        {
            return GetValueConf(2);
        }
        set
        {
            SettingConfig(2, value.AsString());
        }
    }

    /// <summary>
    /// 改ページ１
    /// </summary>
    public int PageBreak1
    {
        get
        {
            var result = GetValueConf(10);
            return string.IsNullOrEmpty(result) ? -1 : result.AsInteger();
        }
        set
        {
            SettingConfig(10, value.AsString());
        }
    }

    /// <summary>
    /// セット区分-医学管理
    ///     0:false 1:true
    /// </summary>
    public int SetKbnKanri
    {
        get
        {
            return GetValueConf(30).AsInteger();
        }
        set
        {
            SettingConfig(30, value.AsString());
        }
    }

    /// <summary>
    /// セット区分-在宅
    ///     0:false 1:true
    /// </summary>
    public int SetKbnZaitaku
    {
        get
        {
            return GetValueConf(31).AsInteger();
        }
        set
        {
            SettingConfig(31, value.AsString());
        }
    }

    /// <summary>
    /// セット区分-処方
    ///     0:false 1:true
    /// </summary>
    public int SetKbnSyoho
    {
        get
        {
            return GetValueConf(32).AsInteger();
        }
        set
        {
            SettingConfig(32, value.AsString());
        }
    }

    /// <summary>
    /// セット区分-用法
    ///     0:false 1:true
    /// </summary>
    public int SetKbnYoho
    {
        get
        {
            return GetValueConf(33).AsInteger();
        }
        set
        {
            SettingConfig(33, value.AsString());
        }
    }

    /// <summary>
    /// セット区分-注射手技
    ///     0:false 1:true
    /// </summary>
    public int SetKbnChusyaSyugi
    {
        get
        {
            return GetValueConf(34).AsInteger();
        }
        set
        {
            SettingConfig(34, value.AsString());
        }
    }


    /// <summary>
    /// セット区分-注射
    ///     0:false 1:true
    /// </summary>
    public int SetKbnChusya
    {
        get
        {
            return GetValueConf(35).AsInteger();
        }
        set
        {
            SettingConfig(35, value.AsString());
        }
    }

    /// <summary>
    /// セット区分-処置
    ///     0:false 1:true
    /// </summary>
    public int SetKbnSyochi
    {
        get
        {
            return GetValueConf(36).AsInteger();
        }
        set
        {
            SettingConfig(36, value.AsString());
        }
    }

    /// <summary>
    /// セット区分-検査
    ///     0:false 1:true
    /// </summary>
    public int SetKbnKensa
    {
        get
        {
            return GetValueConf(37).AsInteger();
        }
        set
        {
            SettingConfig(37, value.AsString());
        }
    }

    /// <summary>
    /// セット区分-手術
    ///     0:false 1:true
    /// </summary>
    public int SetKbnSyujutsu
    {
        get
        {
            return GetValueConf(38).AsInteger();
        }
        set
        {
            SettingConfig(38, value.AsString());
        }
    }

    /// <summary>
    /// セット区分-画像
    ///     0:false 1:true
    /// </summary>
    public int SetKbnGazo
    {
        get
        {
            return GetValueConf(39).AsInteger();
        }
        set
        {
            SettingConfig(39, value.AsString());
        }
    }

    /// <summary>
    /// セット区分-その他
    ///     0:false 1:true
    /// </summary>
    public int SetKbnSonota
    {
        get
        {
            return GetValueConf(40).AsInteger();
        }
        set
        {
            SettingConfig(40, value.AsString());
        }
    }

    /// <summary>
    /// セット区分-自費
    ///     0:false 1:true
    /// </summary>
    public int SetKbnJihi
    {
        get
        {
            return GetValueConf(41).AsInteger();
        }
        set
        {
            SettingConfig(41, value.AsString());
        }
    }

    /// <summary>
    /// セット区分-病名
    ///     0:false 1:true
    /// </summary>
    public int SetKbnByomei
    {
        get
        {
            return GetValueConf(42).AsInteger();
        }
        set
        {
            SettingConfig(42, value.AsString());
        }
    }

    /// <summary>
    /// 対象データ 
    ///    0: すべて
    ///    1: 期限切れ項目
    ///    2: 項目選択
    /// </summary>
    public int TargetData
    {
        get
        {
            return GetValueConf(43).AsInteger();
        }
        set
        {
            SettingConfig(43, value.AsString());
        }
    }

    /// <summary>
    /// 検索ワード 
    /// </summary>
    public string SearchWord
    {
        get
        {
            return GetValueConf(44);
        }
        set
        {
            SettingConfig(44, value);
        }
    }

    /// <summary>
    /// 検索ワードの検索オプション 
    ///     0:or 
    ///     1:and
    /// </summary>
    public int SearchOpt
    {
        get
        {
            return GetValueConf(45).AsInteger();
        }
        set
        {
            SettingConfig(45, value.AsString());
        }
    }

    /// <summary>
    /// 検索項目の検索オプション 
    ///     0:項目 
    ///     1:病名
    /// </summary>
    public int ItemSearchOpt
    {
        get
        {
            return GetValueConf(46).AsInteger();
        }
        set
        {
            SettingConfig(46, value.AsString());
        }
    }


    /// <summary>
    /// 検索項目 
    /// </summary>
    public string ItemCds
    {
        get
        {
            return GetValueConf(47);
        }
        set
        {
            SettingConfig(47, value);
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
            if (string.IsNullOrEmpty(ItemCds))
            {
                return new List<string>();
            }

            return ItemCds.Split(' ').ToList();
        }
    }

    /// <summary>
    /// Get 検索病名
    /// </summary>
    /// <returns></returns>
    public List<string> ListByomeiCd
    {
        get
        {
            if (string.IsNullOrEmpty(ItemCds))
            {
                return new List<string>();
            }

            var arrTemp = ItemCds.Split(',');
            return arrTemp.Where(x => !string.IsNullOrEmpty(x)).ToList();
        }
    }

    public void CopyConfig(ConfigStatistic3020Model configSource)
    {
        this.MenuName = configSource.MenuName;
        this.ReportName = configSource.ReportName;
        this.FormReport = configSource.FormReport;
        this.PageBreak1 = configSource.PageBreak1;
        this.SetKbnKanri = configSource.SetKbnKanri;
        this.SetKbnZaitaku = configSource.SetKbnZaitaku;
        this.SetKbnSyoho = configSource.SetKbnSyoho;
        this.SetKbnYoho = configSource.SetKbnYoho;
        this.SetKbnChusyaSyugi = configSource.SetKbnChusyaSyugi;
        this.SetKbnChusya = configSource.SetKbnChusya;
        this.SetKbnSyochi = configSource.SetKbnSyochi;
        this.SetKbnKensa = configSource.SetKbnKensa;
        this.SetKbnSyujutsu = configSource.SetKbnSyujutsu;
        this.SetKbnGazo = configSource.SetKbnGazo;
        this.SetKbnSonota = configSource.SetKbnSonota;
        this.SetKbnJihi = configSource.SetKbnJihi;
        this.SetKbnByomei = configSource.SetKbnByomei;
        this.TargetData = configSource.TargetData;
        this.SearchWord = configSource.SearchWord;
        this.SearchOpt = configSource.SearchOpt;
        this.ItemCds = configSource.ItemCds;
        this.ItemSearchOpt = configSource.ItemSearchOpt;
    }
}
