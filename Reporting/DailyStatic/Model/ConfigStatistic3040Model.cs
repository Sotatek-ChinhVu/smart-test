using Entity.Tenant;
using Helper.Constants;
using Helper.Extension;

namespace Reporting.DailyStatic.Model;

public class ConfigStatistic3040Model : StatisticModelBase
{
    public ConfigStatistic3040Model(StaMenu staMenu, List<StaConf> listStaConf)
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

    public ConfigStatistic3040Model(int hpId, int groupId = 0, int reportId = 0, int sortNo = 0)
    {
        StaMenu = new StaMenu();
        StaMenu.HpId = hpId;
        StaMenu.GrpId = groupId;
        StaMenu.ReportId = reportId;
        StaMenu.SortNo = sortNo;

        ListStaConf = new List<StaConf>();
        ModelStatus = ModelStatus.Added;
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
    /// ソート順１
    /// </summary>
    public int SortOrder1
    {
        get
        {
            return GetValueConf(HpId, 31).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 31, value.AsString());
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
            return GetValueConf(HpId, 32).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 32, value.AsString());
        }
    }

    /// <summary>
    /// ソート順２
    /// </summary>
    public int SortOrder2
    {
        get
        {
            return GetValueConf(HpId, 33).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 33, value.AsString());
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
            return GetValueConf(HpId, 34).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 34, value.AsString());
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
            return GetValueConf(HpId, 101).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 101, value.AsString());
        }
    }

    /// <summary>
    /// 診療識別(44)
    /// </summary>
    public string MedicaIdentification
    {
        get
        {
            return GetValueConf(HpId, 102);
        }
        set
        {
            SettingConfig(HpId, 102, value.AsString());
        }
    }

    public List<int> SinryoSbt
    {
        get
        {
            if (string.IsNullOrEmpty(MedicaIdentification))
            {
                return new List<int>();
            }

            var arrSinrySbt = MedicaIdentification.Split(' ');
            return arrSinrySbt.Select(x => x.AsInteger()).ToList();
        }
    }

    public void CopyConfig(ConfigStatistic3040Model configSource)
    {
        this.MenuName = configSource.MenuName;
        this.ReportName = configSource.ReportName;
        this.FormReport = configSource.FormReport;
        this.OrderBy1 = configSource.OrderBy1;
        this.SortOrder1 = configSource.SortOrder1;
        this.OrderBy2 = configSource.OrderBy2;
        this.SortOrder2 = configSource.SortOrder2;
        this.TestPatient = configSource.TestPatient;
        this.MedicaIdentification = configSource.MedicaIdentification;
    }
}
