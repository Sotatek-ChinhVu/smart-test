using Entity.Tenant;
using Helper.Constants;
using Helper.Extension;
using static Reporting.Statistics.Sta3060.Models.CoSta3060PrintConf;

namespace Reporting.DailyStatic.Model;

public class ConfigStatistic3060Model : StatisticModelBase
{
    public ConfigStatistic3060Model()
    {
    }

    public ConfigStatistic3060Model(StaMenu staMenu, List<StaConf> listStaConf)
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

    public ConfigStatistic3060Model(int groupId = 0, int reportId = 0, int sortNo = 0)
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
            return GetValueConf(1);
        }
        set
        {
            SettingConfig(1, value);
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
            SettingConfig(2, value);
        }
    }

    /// <summary>
    /// 集計区分
    ///     0:診療日別 1:診療年月別 2:診療科別 3:担当医別 4:保険種別 5:年齢区分別
    /// </summary>
    public int ReportKbn
    {
        get
        {
            return GetValueConf(9).AsInteger();
        }
        set
        {
            SettingConfig(9, value.AsString());
        }
    }

    /// <summary>
    /// 改ページ１
    /// </summary>
    public int PageBreak1
    {
        get
        {
            return GetValueConf(10).AsInteger();
        }
        set
        {
            SettingConfig(10, value.AsString());
        }
    }

    /// <summary>
    /// 改ページ２
    /// </summary>
    public int PageBreak2
    {
        get
        {
            return GetValueConf(11).AsInteger();
        }
        set
        {
            SettingConfig(11, value.AsString());
        }
    }

    /// <summary>
    /// 改ページ３
    /// </summary>
    public int PageBreak3
    {
        get
        {
            return GetValueConf(12).AsInteger();
        }
        set
        {
            SettingConfig(12, value.AsString());
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
            SettingConfig(30, value.AsString());
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
            return GetValueConf(31);
        }
        set
        {
            SettingConfig(31, value);
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
            return GetValueConf(32);
        }
        set
        {
            SettingConfig(32, value);
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

    /// <summary>
    /// 保険種別
    /// </summary>
    public string HokenSbt
    {
        get
        {
            return GetValueConf(33);
        }
        set
        {
            SettingConfig(33, value);
        }
    }

    /// <summary>
    /// 担当医
    /// </summary>
    public List<int> ListHokenSbt
    {
        get
        {
            if (string.IsNullOrEmpty(HokenSbt))
            {
                return new List<int>();
            }

            var arrTemp = HokenSbt.Split(' ');
            return arrTemp.Select(x => x.AsInteger()).ToList();
        }
    }

    /// <summary>
    /// 条件
    ///    グループ
    /// </summary>
    public string GroupIds
    {
        get
        {
            return GetValueConf(34);
        }
        set
        {
            SettingConfig(34, value);
        }
    }

    public List<PtGrp> ListPtGrps
    {
        get
        {
            List<PtGrp> listPtGrps = new List<PtGrp>();
            if (!string.IsNullOrEmpty(GroupIds))
            {
                PtGrp ptGrp = new PtGrp();
                List<string> groupDatas = GroupIds.Split('/').ToList();
                if (groupDatas.Count > 0)
                {
                    foreach (var group in groupDatas)
                    {
                        ptGrp.GrpId = group.Substring(0, group.IndexOf(' ')).AsInteger();
                        ptGrp.GrpCode = group.Substring(group.IndexOf(' ') + 1);
                        if (ptGrp.GrpId <= 0 || string.IsNullOrEmpty(ptGrp.GrpCode))
                        {
                            continue;
                        }
                        listPtGrps.Add(ptGrp);
                    }
                }
            }
            return listPtGrps;
        }
    }

    public void CopyConfig(ConfigStatistic3060Model configSource)
    {
        this.MenuName = configSource.MenuName;
        this.ReportName = configSource.ReportName;
        this.FormReport = configSource.FormReport;
        this.ReportKbn = configSource.ReportKbn;
        this.PageBreak1 = configSource.PageBreak1;
        this.PageBreak2 = configSource.PageBreak2;
        this.PageBreak3 = configSource.PageBreak3;
        this.TestPatient = configSource.TestPatient;
        this.KaId = configSource.KaId;
        this.TantoId = configSource.TantoId;
        this.HokenSbt = configSource.HokenSbt;
        this.GroupIds = configSource.GroupIds;
    }
}
