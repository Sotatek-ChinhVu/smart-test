﻿using Entity.Tenant;
using Helper.Constants;
using Helper.Extension;

namespace Reporting.DailyStatic.Model;

public class ConfigStatistic2021Model : StatisticModelBase
{
    public ConfigStatistic2021Model()
    {
    }

    public ConfigStatistic2021Model(StaMenu staMenu, List<StaConf> listStaConf)
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

    public ConfigStatistic2021Model(int hpId, int groupId = 0, int reportId = 0, int sortNo = 0)
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
    /// 対象データ
    /// </summary>
    public int DataKind
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
    /// ソート順１
    /// </summary>
    public int SortOrder1
    {
        get
        {
            return GetValueConf(20).AsInteger();
        }
        set
        {
            SettingConfig(20, value.AsString());
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
            return GetValueConf(21).AsInteger();
        }
        set
        {
            SettingConfig(21, value.AsString());
        }
    }

    /// <summary>
    /// ソート順２
    /// </summary>
    public int SortOrder2
    {
        get
        {
            return GetValueConf(22).AsInteger();
        }
        set
        {
            SettingConfig(22, value.AsString());
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
            return GetValueConf(23).AsInteger();
        }
        set
        {
            SettingConfig(23, value.AsString());
        }
    }

    /// <summary>
    /// ソート順３
    /// </summary>
    public int SortOrder3
    {
        get
        {
            return GetValueConf(24).AsInteger();
        }
        set
        {
            SettingConfig(24, value.AsString());
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
            return GetValueConf(25).AsInteger();
        }
        set
        {
            SettingConfig(25, value.AsString());
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
    /// 診療識別
    /// </summary>
    public string SinId
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

    /// <summary>
    /// 診療識別
    /// </summary>
    public List<string> ListSinId
    {
        get
        {
            if (string.IsNullOrEmpty(SinId))
            {
                return new List<string>();
            }

            var arrTemp = SinId.Split(' ');
            return arrTemp?.ToList() ?? new();
        }
    }

    /// <summary>
    /// 診療行為区分
    /// </summary>
    public string SinKouiKbn
    {
        get
        {
            return GetValueConf(35);
        }
        set
        {
            SettingConfig(35, value);
        }
    }

    /// <summary>
    /// 診療行為区分
    /// </summary>
    public List<string> ListSinKouiKbn
    {
        get
        {
            if (string.IsNullOrEmpty(SinKouiKbn))
            {
                return new List<string>();
            }

            var arrTemp = SinKouiKbn.Split(' ');
            return arrTemp.ToList();
        }
    }

    /// <summary>
    /// 麻毒区分
    /// </summary>
    public string MadokuKbn
    {
        get
        {
            return GetValueConf(36);
        }
        set
        {
            SettingConfig(36, value);
        }
    }

    /// <summary>
    /// 麻毒区分
    /// </summary>
    public List<int> ListMadokuKbn
    {
        get
        {
            if (string.IsNullOrEmpty(MadokuKbn))
            {
                return new List<int>();
            }

            var arrTemp = MadokuKbn.Split(' ');
            return arrTemp.Select(x => x.AsInteger()).ToList();
        }
    }

    /// <summary>
    /// 向精神薬区分
    /// </summary>
    public string KouseisinKbn
    {
        get
        {
            return GetValueConf(37);
        }
        set
        {
            SettingConfig(37, value);
        }
    }

    /// <summary>
    /// 向精神薬区分
    /// </summary>
    public List<int> ListKouseisinKbn
    {
        get
        {
            if (string.IsNullOrEmpty(KouseisinKbn))
            {
                return new List<int>();
            }

            var arrTemp = KouseisinKbn.Split(' ');
            return arrTemp.Select(x => x.AsInteger()).ToList();
        }
    }

    /// <summary>
    /// 検索ワード 
    /// </summary>
    public string SearchWord
    {
        get
        {
            return GetValueConf(50);
        }
        set
        {
            SettingConfig(50, value);
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
            return GetValueConf(51).AsInteger();
        }
        set
        {
            SettingConfig(51, value.AsString());
        }
    }

    /// <summary>
    /// 検索項目 
    /// </summary>
    public string ItemCd
    {
        get
        {
            return GetValueConf(52);
        }
        set
        {
            SettingConfig(52, value);
        }
    }

    /// <summary>
    /// 検索項目の検索オプション 
    /// </summary>
    public int ItemCdOpt
    {
        get
        {
            return GetValueConf(53).AsInteger();
        }
        set
        {
            SettingConfig(53, value.AsString());
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
    /// 院内院外区分
    /// </summary>
    public string InoutKbn
    {
        get
        {
            return GetValueConf(54);
        }
        set
        {
            SettingConfig(54, value);
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
            return GetValueConf(55);
        }
        set
        {
            SettingConfig(55, value);
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
            return GetValueConf(56);
        }
        set
        {
            SettingConfig(56, value);
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

    public void CopyConfig(ConfigStatistic2021Model configSource)
    {
        this.MenuName = configSource.MenuName;
        this.ReportName = configSource.ReportName;
        this.FormReport = configSource.FormReport;
        this.DataKind = configSource.DataKind;
        this.PageBreak1 = configSource.PageBreak1;
        this.PageBreak2 = configSource.PageBreak2;
        this.SortOrder1 = configSource.SortOrder1;
        this.SortOrder2 = configSource.SortOrder2;
        this.SortOrder3 = configSource.SortOrder3;
        this.SortOpt1 = configSource.SortOpt1;
        this.SortOpt2 = configSource.SortOpt2;
        this.SortOpt3 = configSource.SortOpt3;
        this.TestPatient = configSource.TestPatient;
        this.KaId = configSource.KaId;
        this.TantoId = configSource.TantoId;
        this.HokenSbt = configSource.HokenSbt;
        this.SinId = configSource.SinId;
        this.SinKouiKbn = configSource.SinKouiKbn;
        this.MadokuKbn = configSource.MadokuKbn;
        this.KouseisinKbn = configSource.KouseisinKbn;
        this.SearchWord = configSource.SearchWord;
        this.SearchOpt = configSource.SearchOpt;
        this.ItemCd = configSource.ItemCd;
        this.ItemCdOpt = configSource.ItemCdOpt;
        this.InoutKbn = configSource.InoutKbn;
        this.KohatuKbn = configSource.KohatuKbn;
        this.IsAdopted = configSource.IsAdopted;
    }
}
