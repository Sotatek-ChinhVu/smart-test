using Entity.Tenant;
using Helper.Constants;
using Helper.Extension;

namespace Reporting.DailyStatic.Model;

public class ConfigStatistic3001Model : StatisticModelBase
{
    public ConfigStatistic3001Model(StaMenu staMenu, List<StaConf> listStaConf)
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

    public ConfigStatistic3001Model(int groupId = 0, int reportId = 0, int sortNo = 0)
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
    public int BreakPage1
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
    public int OrderBy1
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
    public int OrderBy2
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
    public int OrderBy3
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
    /// 薬剤区分－内用薬
    ///    0:false 1:true
    /// </summary>
    public int DrugCategoryInternal
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
    /// 薬剤区分－外用薬
    ///    0:false 1:true
    /// </summary>
    public int DrugCategoryTopical
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
    /// 薬剤区分－注射薬
    ///    0:false 1:true
    /// </summary>
    public int DrugCategoryInjection
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
    /// 薬剤区分－歯科用薬剤
    ///    0:false 1:true
    /// </summary>
    public int DrugCategoryDental
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
    /// 薬剤区分－その他
    ///    0:false 1:true
    /// </summary>
    public int DrugCategoryOther
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
    /// 麻毒区分－麻毒等以外
    ///    0:false 1:true
    /// </summary>
    public int MalignantCategoryOtherThanNarcotic
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
    /// 麻毒区分－麻薬
    ///    0:false 1:true
    /// </summary>
    public int MalignantCategoryNarcotic
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
    /// 麻毒区分－毒薬
    ///    0:false 1:true
    /// </summary>
    public int MalignantCategorynPoisonousDrug
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
    /// 麻毒区分－覚せい剤
    ///    0:false 1:true
    /// </summary>
    public int MalignantCategoryAntipsychotics
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
    /// 麻毒区分－向精神薬
    ///    0:false 1:true
    /// </summary>
    public int MalignantCategoryPsychotropicDrug
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
    /// 向精神薬区分－向精神薬以外
    ///    0:false 1:true
    /// </summary>
    public int PsychotropicDrugCategoryOtherThanPsychotropicDrugs
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
    /// 向精神薬区分－抗不安薬
    ///    0:false 1:true
    /// </summary>
    public int PsychotropicDrugCategoryAnxiolytics
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
    /// 向精神薬区分－睡眠薬
    ///    0:false 1:true
    /// </summary>
    public int PsychotropicDrugCategorySleepingPills
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
    /// 向精神薬区分－抗うつ薬
    ///    0:false 1:true
    /// </summary>
    public int PsychotropicDrugCategoryAntidepressants
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
    /// 向精神薬区分－抗精神病薬
    ///    0:false 1:true
    /// </summary>
    public int PsychotropicDrugCategoryAntipsychoticDrugs
    {
        get
        {
            return GetValueConf(44).AsInteger();
        }
        set
        {
            SettingConfig(44, value.AsString());
        }
    }

    /// <summary>
    /// 後発フラグ－先発品（後発品なし）
    ///    0:false 1:true
    /// </summary>
    public int GeneralFlagOriginalProductNoGeneric
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
    /// 後発フラグ－先発品（後発品あり）
    ///    0:false 1:true
    /// </summary>
    public int GeneralFlagOriginalProductWithGeneric
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
    /// 後発フラグ－後発品
    ///    0:false 1:true
    /// </summary>
    public int GeneralFlagGeneralProcduct
    {
        get
        {
            return GetValueConf(47).AsInteger();
        }
        set
        {
            SettingConfig(47, value.AsString());
        }
    }

    /// <summary>
    /// 開始日－From
    /// </summary>
    public int StartDateFrom{get;set;}

    /// <summary>
    /// 開始日－To
    /// </summary>
    public int StartDateTo { get; set; }
    
    /// <summary>
    /// 終了日－From
    /// </summary>
    public int EndDateFrom { get; set; }

    /// <summary>
    /// 終了日－To
    /// </summary>
    public int EndDateTo { get; set; }

    /// <summary>
    /// オプション－一般名
    ///    0:false 1:true
    /// </summary>
    public int OptionCommonName
    {
        get
        {
            return GetValueConf(48).AsInteger();
        }
        set
        {
            SettingConfig(48, value.AsString());
        }
    }

    /// <summary>
    /// オプション－レセプト名称
    ///    0:false 1:true
    /// </summary>
    public int OptionReceiptName
    {
        get
        {
            return GetValueConf(49).AsInteger();
        }
        set
        {
            SettingConfig(49, value.AsString());
        }
    }

    public void CopyConfig(ConfigStatistic3001Model configSource)
    {
        this.MenuName = configSource.MenuName;
        this.ReportName = configSource.ReportName;
        this.FormReport = configSource.FormReport;
        this.BreakPage1 = configSource.BreakPage1;
        this.SortOrder1 = configSource.SortOrder1;
        this.OrderBy1 = configSource.OrderBy1;
        this.SortOrder2 = configSource.SortOrder2;
        this.OrderBy2 = configSource.OrderBy2;
        this.SortOrder3 = configSource.SortOrder3;
        this.OrderBy3 = configSource.OrderBy3;
        this.DrugCategoryDental = configSource.DrugCategoryDental;
        this.DrugCategoryInternal = configSource.DrugCategoryInternal;
        this.DrugCategoryOther = configSource.DrugCategoryOther;
        this.DrugCategoryTopical = configSource.DrugCategoryTopical;
        this.DrugCategoryInjection = configSource.DrugCategoryInjection;
        this.MalignantCategoryAntipsychotics = configSource.MalignantCategoryAntipsychotics;
        this.MalignantCategoryNarcotic = configSource.MalignantCategoryNarcotic;
        this.MalignantCategoryOtherThanNarcotic = configSource.MalignantCategoryOtherThanNarcotic;
        this.MalignantCategorynPoisonousDrug = configSource.MalignantCategorynPoisonousDrug;
        this.MalignantCategoryPsychotropicDrug = configSource.MalignantCategoryPsychotropicDrug;
        this.PsychotropicDrugCategoryAntidepressants = configSource.PsychotropicDrugCategoryAntidepressants;
        this.PsychotropicDrugCategoryAntipsychoticDrugs = configSource.PsychotropicDrugCategoryAntipsychoticDrugs;
        this.PsychotropicDrugCategoryAnxiolytics = configSource.PsychotropicDrugCategoryAnxiolytics;
        this.PsychotropicDrugCategoryOtherThanPsychotropicDrugs = configSource.PsychotropicDrugCategoryOtherThanPsychotropicDrugs;
        this.PsychotropicDrugCategorySleepingPills = configSource.PsychotropicDrugCategorySleepingPills;
        this.GeneralFlagGeneralProcduct = configSource.GeneralFlagGeneralProcduct;
        this.GeneralFlagOriginalProductNoGeneric = configSource.GeneralFlagOriginalProductNoGeneric;
        this.GeneralFlagOriginalProductWithGeneric = configSource.GeneralFlagOriginalProductWithGeneric;
        this.StartDateFrom = configSource.StartDateFrom;
        this.EndDateFrom = configSource.EndDateFrom;
        this.StartDateTo = configSource.StartDateTo;
        this.EndDateTo = configSource.EndDateTo;
        this.OptionCommonName = configSource.OptionCommonName;
        this.OptionReceiptName = configSource.OptionReceiptName;
    }
}
