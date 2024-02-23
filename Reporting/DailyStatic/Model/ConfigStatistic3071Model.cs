using Entity.Tenant;
using Helper.Constants;
using Helper.Extension;

namespace Reporting.DailyStatic.Model;

public class ConfigStatistic3071Model : StatisticModelBase
{
    public ConfigStatistic3071Model(StaMenu staMenu, List<StaConf> listStaConf)
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

    public ConfigStatistic3071Model(int hpId, int groupId = 0, int reportId = 0, int sortNo = 0)
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
            SettingConfig(HpId, 1, value);
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
            SettingConfig(HpId, 2, value);
        }
    }

    /// <summary>
    /// 集計項目（縦）
    ///     1:診療科別 2:担当医別 3:保険種別 4:日別 5:月別 6:時間区分別 7:年齢区分別 8:性別 2X:患者グループX
    /// </summary>
    public int ReportKbnV
    {
        get
        {
            return GetValueConf(HpId, 3).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 3, value.AsString());
        }
    }

    /// <summary>
    /// 集計項目（横）
    ///     1:診療科別 2:担当医別 3:保険種別 4:日別 5:月別 6:時間区分別 7:年齢区分別 8:性別 2X:患者グループX
    /// </summary>
    public int ReportKbnH
    {
        get
        {
            return GetValueConf(HpId, 4).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 4, value.AsString());
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
            return GetValueConf(HpId, 20).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 20, value.AsString());
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
            return GetValueConf(HpId, 21);
        }
        set
        {
            SettingConfig(HpId, 21, value);
        }
    }

    /// <summary>
    /// 診療科
    /// </summary>
    public List<int> ListKaId
    {
        get
        {
            if (string.IsNullOrEmpty(KaId))
            {
                return new List<int>();
            }

            var arrTemp = KaId.Split(' ');
            return arrTemp.Select(x => x.AsInteger()).ToList();
        }
    }

    /// <summary>
    /// 条件
    ///    担当医
    /// </summary>
    public string TantoId
    {
        get
        {
            return GetValueConf(HpId, 22);
        }
        set
        {
            SettingConfig(HpId, 22, value);
        }
    }

    /// <summary>
    /// 担当医
    /// </summary>
    public List<int> ListTantoId
    {
        get
        {
            if (string.IsNullOrEmpty(TantoId))
            {
                return new List<int>();
            }

            var arrTemp = TantoId.Split(' ');
            return arrTemp.Select(x => x.AsInteger()).ToList();
        }
    }

    public void CopyConfig(ConfigStatistic3071Model configSource)
    {
        this.MenuName = configSource.MenuName;
        this.ReportName = configSource.ReportName;
        this.FormReport = configSource.FormReport;
        this.ReportKbnV = configSource.ReportKbnV;
        this.ReportKbnH = configSource.ReportKbnH;
        this.TestPatient = configSource.TestPatient;
        this.KaId = configSource.KaId;
        this.TantoId = configSource.TantoId;
    }
}
