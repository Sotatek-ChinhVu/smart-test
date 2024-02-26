using Entity.Tenant;
using Helper.Constants;
using Helper.Extension;

namespace Reporting.DailyStatic.Model;

public class ConfigStatistic3010Model : StatisticModelBase
{
    public ConfigStatistic3010Model(StaMenu staMenu, List<StaConf> listStaConf)
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
            SetValDefault();
            ModelStatus = ModelStatus.Added;
        }
    }

    public ConfigStatistic3010Model(int hpId, int groupId = 0, int reportId = 0, int sortNo = 0)
    {
        StaMenu = new StaMenu();
        StaMenu.HpId = hpId;
        StaMenu.GrpId = groupId;
        StaMenu.ReportId = reportId;
        StaMenu.SortNo = sortNo;

        ListStaConf = new List<StaConf>();
        ModelStatus = ModelStatus.Added;
        SetValDefault();
    }

    public void SetValDefault()
    {
        this.Set1 = 1;
        this.Set2 = 1;
        this.Set3 = 1;
        this.Set4 = 1;
        this.Set5 = 1;
        this.Set6 = 1;
        this.Set7 = 1;
        this.Set8 = 1;
        this.Set9 = 1;
        this.Set10 = 1;
    }

    /// <summary>
    /// Hospital Id
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
        get => StaMenu.MenuName;
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
    public int BreakPage1
    {
        get
        {
            var result = GetValueConf(HpId, 10);
            return string.IsNullOrEmpty(result) ? -1 : result.AsInteger();
        }
        set
        {
            SettingConfig(HpId, 10, value.AsString());
        }
    }

    /// <summary>
    /// セット1
    /// </summary>
    public int Set1
    {
        get
        {
            return GetValueConf(HpId, 100).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 100, value.AsString());
        }
    }

    /// <summary>
    /// セット1-1
    /// </summary>
    public int Set1_1
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
    /// セット1-2
    /// </summary>
    public int Set1_2
    {
        get
        {
            return GetValueConf(HpId, 102).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 102, value.AsString());
        }
    }

    /// <summary>
    /// セット1-3
    /// </summary>
    public int Set1_3
    {
        get
        {
            return GetValueConf(HpId, 103).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 103, value.AsString());
        }
    }

    /// <summary>
    /// セット1-4
    /// </summary>
    public int Set1_4
    {
        get
        {
            return GetValueConf(HpId, 104).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 104, value.AsString());
        }
    }

    /// <summary>
    /// セット1-5
    /// </summary>
    public int Set1_5
    {
        get
        {
            return GetValueConf(HpId, 105).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 105, value.AsString());
        }
    }

    /// <summary>
    /// セット1-6
    /// </summary>
    public int Set1_6
    {
        get
        {
            return GetValueConf(HpId, 106).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 106, value.AsString());
        }
    }

    /// <summary>
    /// セット2
    /// </summary>
    public int Set2
    {
        get
        {
            return GetValueConf(HpId, 200).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 200, value.AsString());
        }
    }

    /// <summary>
    /// セット2-1
    /// </summary>
    public int Set2_1
    {
        get
        {
            return GetValueConf(HpId, 201).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 201, value.AsString());
        }
    }

    /// <summary>
    /// セット2-2
    /// </summary>
    public int Set2_2
    {
        get
        {
            return GetValueConf(HpId, 202).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 202, value.AsString());
        }
    }

    /// <summary>
    /// セット2-3
    /// </summary>
    public int Set2_3
    {
        get
        {
            return GetValueConf(HpId, 203).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 203, value.AsString());
        }
    }

    /// <summary>
    /// セット2-4
    /// </summary>
    public int Set2_4
    {
        get
        {
            return GetValueConf(HpId, 204).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 204, value.AsString());
        }
    }

    /// <summary>
    /// セット2-5
    /// </summary>
    public int Set2_5
    {
        get
        {
            return GetValueConf(HpId, 205).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 205, value.AsString());
        }
    }

    /// <summary>
    /// セット2-6
    /// </summary>
    public int Set2_6
    {
        get
        {
            return GetValueConf(HpId, 206).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 206, value.AsString());
        }
    }

    /// <summary>
    /// セット3
    /// </summary>
    public int Set3
    {
        get
        {
            return GetValueConf(HpId, 300).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 300, value.AsString());
        }
    }

    /// <summary>
    /// セット3-1
    /// </summary>
    public int Set3_1
    {
        get
        {
            return GetValueConf(HpId, 301).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 301, value.AsString());
        }
    }

    /// <summary>
    /// セット3-2
    /// </summary>
    public int Set3_2
    {
        get
        {
            return GetValueConf(HpId, 302).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 302, value.AsString());
        }
    }

    /// <summary>
    /// セット3-3
    /// </summary>
    public int Set3_3
    {
        get
        {
            return GetValueConf(HpId, 303).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 303, value.AsString());
        }
    }

    /// <summary>
    /// セット3-4
    /// </summary>
    public int Set3_4
    {
        get
        {
            return GetValueConf(HpId, 304).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 304, value.AsString());
        }
    }

    /// <summary>
    /// セット3-5
    /// </summary>
    public int Set3_5
    {
        get
        {
            return GetValueConf(HpId, 305).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 305, value.AsString());
        }
    }

    /// <summary>
    /// セット3-6
    /// </summary>
    public int Set3_6
    {
        get
        {
            return GetValueConf(HpId, 306).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 306, value.AsString());
        }
    }

    /// <summary>
    /// セット4
    /// </summary>
    public int Set4
    {
        get
        {
            return GetValueConf(HpId, 400).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 400, value.AsString());
        }

    }
    /// <summary>
    /// セット4-1
    /// </summary>
    public int Set4_1
    {
        get
        {
            return GetValueConf(HpId, 401).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 401, value.AsString());
        }
    }

    /// <summary>
    /// セット4-2
    /// </summary>
    public int Set4_2
    {
        get
        {
            return GetValueConf(HpId, 402).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 402, value.AsString());
        }
    }

    /// <summary>
    /// セット4-3
    /// </summary>
    public int Set4_3
    {
        get
        {
            return GetValueConf(HpId, 403).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 403, value.AsString());
        }
    }

    /// <summary>
    /// セット4-4
    /// </summary>
    public int Set4_4
    {
        get
        {
            return GetValueConf(HpId, 404).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 404, value.AsString());
        }
    }

    /// <summary>
    /// セット4-5
    /// </summary>
    public int Set4_5
    {
        get
        {
            return GetValueConf(HpId, 405).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 405, value.AsString());
        }
    }

    /// <summary>
    /// セット4-6
    /// </summary>
    public int Set4_6
    {
        get
        {
            return GetValueConf(HpId, 406).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 406, value.AsString());
        }
    }

    /// <summary>
    /// セット5
    /// </summary>
    public int Set5
    {
        get
        {
            return GetValueConf(HpId, 500).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 500, value.AsString());
        }
    }

    /// <summary>
    /// セット5-1
    /// </summary>
    public int Set5_1
    {
        get
        {
            return GetValueConf(HpId, 501).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 501, value.AsString());
        }
    }

    /// <summary>
    /// セット5-2
    /// </summary>
    public int Set5_2
    {
        get
        {
            return GetValueConf(HpId, 502).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 502, value.AsString());
        }
    }

    /// <summary>
    /// セット5-3
    /// </summary>
    public int Set5_3
    {
        get
        {
            return GetValueConf(HpId, 503).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 503, value.AsString());
        }
    }

    /// <summary>
    /// セット5-4
    /// </summary>
    public int Set5_4
    {
        get
        {
            return GetValueConf(HpId, 504).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 504, value.AsString());
        }
    }

    /// <summary>
    /// セット5-5
    /// </summary>
    public int Set5_5
    {
        get
        {
            return GetValueConf(HpId, 505).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 505, value.AsString());
        }
    }

    /// <summary>
    /// セット5-6
    /// </summary>
    public int Set5_6
    {
        get
        {
            return GetValueConf(HpId, 506).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 506, value.AsString());
        }
    }

    /// <summary>
    /// セット6
    /// </summary>
    public int Set6
    {
        get
        {
            return GetValueConf(HpId, 600).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 600, value.AsString());
        }
    }

    /// <summary>
    /// セット6-1
    /// </summary>
    public int Set6_1
    {
        get
        {
            return GetValueConf(HpId, 601).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 601, value.AsString());
        }
    }

    /// <summary>
    /// セット6-2
    /// </summary>
    public int Set6_2
    {
        get
        {
            return GetValueConf(HpId, 602).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 602, value.AsString());
        }
    }

    /// <summary>
    /// セット6-3
    /// </summary>
    public int Set6_3
    {
        get
        {
            return GetValueConf(HpId, 603).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 603, value.AsString());
        }
    }

    /// <summary>
    /// セット6-4
    /// </summary>
    public int Set6_4
    {
        get
        {
            return GetValueConf(HpId, 604).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 604, value.AsString());
        }
    }

    /// <summary>
    /// セット6-5
    /// </summary>
    public int Set6_5
    {
        get
        {
            return GetValueConf(HpId, 605).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 605, value.AsString());
        }
    }

    /// <summary>
    /// セット6-6
    /// </summary>
    public int Set6_6
    {
        get
        {
            return GetValueConf(HpId, 606).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 606, value.AsString());
        }
    }

    /// <summary>
    /// セット7
    /// </summary>
    public int Set7
    {
        get
        {
            return GetValueConf(HpId, 700).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 700, value.AsString());
        }
    }

    /// <summary>
    /// セット7-1
    /// </summary>
    public int Set7_1
    {
        get
        {
            return GetValueConf(HpId, 701).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 701, value.AsString());
        }
    }

    /// <summary>
    /// セット7-2
    /// </summary>
    public int Set7_2
    {
        get
        {
            return GetValueConf(HpId, 702).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 702, value.AsString());
        }
    }

    /// <summary>
    /// セット7-3
    /// </summary>
    public int Set7_3
    {
        get
        {
            return GetValueConf(HpId, 703).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 703, value.AsString());
        }
    }

    /// <summary>
    /// セット7-4
    /// </summary>
    public int Set7_4
    {
        get
        {
            return GetValueConf(HpId, 704).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 704, value.AsString());
        }
    }

    /// <summary>
    /// セット7-5
    /// </summary>
    public int Set7_5
    {
        get
        {
            return GetValueConf(HpId, 705).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 705, value.AsString());
        }
    }

    /// <summary>
    /// セット7-6
    /// </summary>
    public int Set7_6
    {
        get
        {
            return GetValueConf(HpId, 706).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 706, value.AsString());
        }
    }

    /// <summary>
    /// セット8
    /// </summary>
    public int Set8
    {
        get
        {
            return GetValueConf(HpId, 800).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 800, value.AsString());
        }
    }

    /// <summary>
    /// セット8-1
    /// </summary>
    public int Set8_1
    {
        get
        {
            return GetValueConf(HpId, 801).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 801, value.AsString());
        }
    }

    /// <summary>
    /// セット8-2
    /// </summary>
    public int Set8_2
    {
        get
        {
            return GetValueConf(HpId, 802).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 802, value.AsString());
        }
    }

    /// <summary>
    /// セット8-3
    /// </summary>
    public int Set8_3
    {
        get
        {
            return GetValueConf(HpId, 803).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 803, value.AsString());
        }
    }

    /// <summary>
    /// セット8-4
    /// </summary>
    public int Set8_4
    {
        get
        {
            return GetValueConf(HpId, 804).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 804, value.AsString());
        }
    }

    /// <summary>
    /// セット8-5
    /// </summary>
    public int Set8_5
    {
        get
        {
            return GetValueConf(HpId, 805).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 805, value.AsString());
        }
    }

    /// <summary>
    /// セット8-6
    /// </summary>
    public int Set8_6
    {
        get
        {
            return GetValueConf(HpId, 806).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 806, value.AsString());
        }
    }

    /// <summary>
    /// セット9
    /// </summary>
    public int Set9
    {
        get
        {
            return GetValueConf(HpId, 900).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 900, value.AsString());
        }
    }

    /// <summary>
    /// セット9-1
    /// </summary>
    public int Set9_1
    {
        get
        {
            return GetValueConf(HpId, 901).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 901, value.AsString());
        }
    }

    /// <summary>
    /// セット9-2
    /// </summary>
    public int Set9_2
    {
        get
        {
            return GetValueConf(HpId, 902).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 902, value.AsString());
        }
    }

    /// <summary>
    /// セット9-3
    /// </summary>
    public int Set9_3
    {
        get
        {
            return GetValueConf(HpId, 903).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 903, value.AsString());
        }
    }

    /// <summary>
    /// セット9-4
    /// </summary>
    public int Set9_4
    {
        get
        {
            return GetValueConf(HpId, 904).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 904, value.AsString());
        }
    }

    /// <summary>
    /// セット9-5
    /// </summary>
    public int Set9_5
    {
        get
        {
            return GetValueConf(HpId, 905).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 905, value.AsString());
        }
    }

    /// <summary>
    /// セット9-6
    /// </summary>
    public int Set9_6
    {
        get
        {
            return GetValueConf(HpId, 906).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 906, value.AsString());
        }
    }

    /// <summary>
    /// セット10
    /// </summary>
    public int Set10
    {
        get
        {
            return GetValueConf(HpId, 1000).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 1000, value.AsString());
        }
    }

    /// <summary>
    /// セット10-1
    /// </summary>
    public int Set10_1
    {
        get
        {
            return GetValueConf(HpId, 1001).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 1001, value.AsString());
        }
    }

    /// <summary>
    /// セット10-2
    /// </summary>
    public int Set10_2
    {
        get
        {
            return GetValueConf(HpId, 1002).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 1002, value.AsString());
        }
    }

    /// <summary>
    /// セット10-3
    /// </summary>
    public int Set10_3
    {
        get
        {
            return GetValueConf(HpId, 1003).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 1003, value.AsString());
        }
    }

    /// <summary>
    /// セット10-4
    /// </summary>
    public int Set10_4
    {
        get
        {
            return GetValueConf(HpId, 1004).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 1004, value.AsString());
        }
    }

    /// <summary>
    /// セット10-5
    /// </summary>
    public int Set10_5
    {
        get
        {
            return GetValueConf(HpId, 1005).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 1005, value.AsString());
        }
    }

    /// <summary>
    /// セット10-6
    /// </summary>
    public int Set10_6
    {
        get
        {
            return GetValueConf(HpId, 1006).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 1006, value.AsString());
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
            return GetValueConf(HpId, 30).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 30, value.AsString());
        }
    }

    /// <summary>
    /// セット内の他の項目 
    ///     0:含まない 
    ///     1: 含む
    /// </summary>
    public int OtherItemOpt
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
    /// 検索ワード 
    /// </summary>
    public string SearchWord
    {
        get
        {
            return GetValueConf(HpId, 32);
        }
        set
        {
            SettingConfig(HpId, 32, value);
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
            return GetValueConf(HpId, 33).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 33, value.AsString());
        }
    }

    /// <summary>
    /// 検索項目 
    /// </summary>
    public string ItemCds
    {
        get
        {
            return GetValueConf(HpId, 34);
        }
        set
        {
            SettingConfig(HpId, 34, value);
        }
    }

    /// <summary>
    /// 検索項目の検索オプション 
    /// </summary>
    public int ItemCdOpt
    {
        get
        {
            return GetValueConf(HpId, 35).AsInteger();
        }
        set
        {
            SettingConfig(HpId, 35, value.AsString());
        }
    }

    /// <summary>
    /// Get 検索項目
    /// </summary>
    /// <returns></returns>
    public List<string> GetListItemCd()
    {
        if (string.IsNullOrEmpty(ItemCds))
        {
            return new List<string>();
        }

        return ItemCds.Split(' ').ToList();
    }

    //public InputItemTenMstViewModel ListTenMstInput { get; set; }

    //private void GetListItemTenMst()
    //{
    //    if (string.IsNullOrEmpty(ItemCds))
    //    {
    //        ListTenMstInput.SetListItem(new List<string>());
    //    }
    //    else
    //    {
    //        var listItem = ItemCds.Split(' ').ToList();
    //        ListTenMstInput.SetListItem(listItem);
    //    }

    //    //ListTenMstInput.ReadDataCommand = new RelayCommand(ReadListTenMstInput);
    //    ListTenMstInput.AddGetItemCdListAction(ReadListTenMstInput);
    //}

    //private void ReadListTenMstInput()
    //{
    //    ItemCds = ListTenMstInput.GetListItemInput();
    //}

    public void CopyConfig(ConfigStatistic3010Model configSource)
    {
        this.MenuName = configSource.MenuName;
        this.ReportName = configSource.ReportName;
        this.FormReport = configSource.FormReport;
        this.BreakPage1 = configSource.BreakPage1;
        this.Set1 = configSource.Set1;
        this.Set1_1 = configSource.Set1_1;
        this.Set1_2 = configSource.Set1_2;
        this.Set1_3 = configSource.Set1_3;
        this.Set1_4 = configSource.Set1_4;
        this.Set1_5 = configSource.Set1_5;
        this.Set1_6 = configSource.Set1_6;
        this.Set2 = configSource.Set2;
        this.Set2_1 = configSource.Set2_1;
        this.Set2_2 = configSource.Set2_2;
        this.Set2_3 = configSource.Set2_3;
        this.Set2_4 = configSource.Set2_4;
        this.Set2_5 = configSource.Set2_5;
        this.Set2_6 = configSource.Set2_6;
        this.Set3 = configSource.Set3;
        this.Set3_1 = configSource.Set3_1;
        this.Set3_2 = configSource.Set3_2;
        this.Set3_3 = configSource.Set3_3;
        this.Set3_4 = configSource.Set3_4;
        this.Set3_5 = configSource.Set3_5;
        this.Set3_6 = configSource.Set3_6;
        this.Set4 = configSource.Set4;
        this.Set4_1 = configSource.Set4_1;
        this.Set4_2 = configSource.Set4_2;
        this.Set4_3 = configSource.Set4_3;
        this.Set4_4 = configSource.Set4_4;
        this.Set4_5 = configSource.Set4_5;
        this.Set4_6 = configSource.Set4_6;
        this.Set5 = configSource.Set5;
        this.Set5_1 = configSource.Set5_1;
        this.Set5_2 = configSource.Set5_2;
        this.Set5_3 = configSource.Set5_3;
        this.Set5_4 = configSource.Set5_4;
        this.Set5_5 = configSource.Set5_5;
        this.Set5_6 = configSource.Set5_6;
        this.Set6 = configSource.Set6;
        this.Set6_1 = configSource.Set6_1;
        this.Set6_2 = configSource.Set6_2;
        this.Set6_3 = configSource.Set6_3;
        this.Set6_4 = configSource.Set6_4;
        this.Set6_5 = configSource.Set6_5;
        this.Set6_6 = configSource.Set6_6;
        this.Set7 = configSource.Set7;
        this.Set7_1 = configSource.Set7_1;
        this.Set7_2 = configSource.Set7_2;
        this.Set7_3 = configSource.Set7_3;
        this.Set7_4 = configSource.Set7_4;
        this.Set7_5 = configSource.Set7_5;
        this.Set7_6 = configSource.Set7_6;
        this.Set8 = configSource.Set8;
        this.Set8_1 = configSource.Set8_1;
        this.Set8_2 = configSource.Set8_2;
        this.Set8_3 = configSource.Set8_3;
        this.Set8_4 = configSource.Set8_4;
        this.Set8_5 = configSource.Set8_5;
        this.Set8_6 = configSource.Set8_6;
        this.Set9 = configSource.Set9;
        this.Set9_1 = configSource.Set9_1;
        this.Set9_2 = configSource.Set9_2;
        this.Set9_3 = configSource.Set9_3;
        this.Set9_4 = configSource.Set9_4;
        this.Set9_5 = configSource.Set9_5;
        this.Set9_6 = configSource.Set9_6;
        this.Set10 = configSource.Set10;
        this.Set10_1 = configSource.Set10_1;
        this.Set10_2 = configSource.Set10_2;
        this.Set10_3 = configSource.Set10_3;
        this.Set10_4 = configSource.Set10_4;
        this.Set10_5 = configSource.Set10_5;
        this.Set10_6 = configSource.Set10_6;
        this.TargetData = configSource.TargetData;
        this.OtherItemOpt = configSource.OtherItemOpt;
        this.SearchWord = configSource.SearchWord;
        this.SearchOpt = configSource.SearchOpt;
        this.ItemCds = configSource.ItemCds;
        this.ItemCdOpt = configSource.ItemCdOpt;
        //this.ListTenMstInput = configSource.ListTenMstInput;
    }
}
