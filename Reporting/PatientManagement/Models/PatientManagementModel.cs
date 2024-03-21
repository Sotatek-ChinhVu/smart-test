using Helper.Common;
using Helper.Extension;
using Reporting.Statistics.Sta9000.Models;
using static Reporting.Statistics.Sta9000.Models.CoSta9000HokenConf;
using static Reporting.Statistics.Sta9000.Models.CoSta9000PtConf;

namespace Reporting.PatientManagement.Models;

public class PatientManagementModel
{
    public PatientManagementModel()
    {
    }

    #region 患者情報
    public int OutputOrder { get; set; }
    public int OutputOrder2 { get; set; } = -1;

    public int OutputOrder3 { get; set; } = -1;

    /// <summary>
    /// 帳票の種類
    /// 0:患者一覧 1:患者一覧（処方・病名リスト） 2:宛名ラベル 3:患者来院歴一覧
    /// </summary>
    public int ReportType { get; set; }

    public bool IsVisibleVisitingOuputOrder => ReportType == 3;

    /// <summary>
    /// 患者番号From
    /// </summary>
    public long PtNumFrom { get; set; }

    public long PtNumTo
    {
        get; set;
    }

    public string KanaName { get; set; }

    public string Name { get; set; }
    public int BirthDayFrom { get; set; }

    public DateTime? BirthDayFromBinding => CIUtil.IntToDate(BirthDayFrom);

    public int BirthDayTo
    {
        get; set;
    }

    public DateTime? BirthDayToBinding => CIUtil.IntToDate(BirthDayTo);

    public string AgeFrom
    {
        get; set;
    }

    public string AgeTo { get; set; }
    public int AgeRefDate { get; set; }

    public DateTime? AgeRefDateBinding => CIUtil.IntToDate(AgeRefDate);
    public int Sex { get; set; }

    /// <summary>
    /// 郵便番号
    /// </summary>
    public string HomePost
    {
        get => ZipCD1 + ZipCD2;
    }

    public string ZipCD1 { get; set; } = string.Empty;
    public string ZipCD2 { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public int RegistrationDateFrom { get; set; }

    public DateTime? RegistrationDateFromBinding => CIUtil.IntToDate(RegistrationDateFrom);

    public int RegistrationDateTo { get; set; }

    public DateTime? RegistrationDateToBinding => CIUtil.IntToDate(RegistrationDateTo);

    public int IncludeTestPt { get; set; }
    public List<long> ListPtNums { get; set; } = new();
    public string GroupSelected
    {
        get; set;
    } = string.Empty;

    public CoSta9000PtConf? AsCoSta9000PtConf(List<PtGrp> ptGrps)
    {
        int? ageFrom = null;
        if (!string.IsNullOrEmpty(AgeFrom))
        {
            ageFrom = AgeFrom.AsInteger();
        }
        int? ageTo = null;
        if (!string.IsNullOrEmpty(AgeTo))
        {
            ageTo = AgeTo.AsInteger();
        }

        if (IsDefaultInputPtConfCondition())
        {
            return null;
        }
        return new CoSta9000PtConf()
        {
            IsTester = IncludeTestPt != 0,
            StartPtNum = PtNumFrom,
            EndPtNum = PtNumTo,
            KanaName = KanaName.AsString(),
            Name = Name.AsString(),
            StartBirthday = BirthDayFrom,
            EndBirthday = BirthDayTo,
            StartAge = ageFrom,
            EndAge = ageTo,
            AgeBaseDate = AgeRefDate,
            Sex = Sex,
            HomePost = HomePost.AsString(),
            HomeAddress = Address.AsString(),
            Tel = PhoneNumber.AsString(),
            StartRegDate = RegistrationDateFrom,
            EndRegDate = RegistrationDateTo,
            PtGrps = ptGrps,
            PtNums = ListPtNums
        };
    }

    private bool IsDefaultInputPtConfCondition()
    {
        return IncludeTestPt == 0 && PtNumFrom == 0 && PtNumTo == 0 &&
                string.IsNullOrEmpty(KanaName) && string.IsNullOrEmpty(Name) &&
                BirthDayFrom == 0 && BirthDayTo == 0 && string.IsNullOrEmpty(AgeFrom) && string.IsNullOrEmpty(AgeTo) &&
                Sex == 0 && string.IsNullOrEmpty(HomePost) && string.IsNullOrEmpty(Address) &&
                string.IsNullOrEmpty(PhoneNumber) && RegistrationDateFrom == 0 && RegistrationDateTo == 0 &&
                string.IsNullOrEmpty(GroupSelected) && (ListPtNums == null || ListPtNums.Count == 0);
    }

    public bool IsDefaultPtConfInView()
    {
        return IsDefaultInputPtConfCondition() && AgeRefDate == 0;
    }
    #endregion

    #region 保険情報
    public string HokensyaNoFrom { get; set; } = string.Empty;

    public string HokensyaNoTo { get; set; } = string.Empty;
    public string Kigo { get; set; } = string.Empty;
    public string Bango { get; set; } = string.Empty;
    public string EdaNo { get; set; } = string.Empty;

    public int HokenKbn { get; set; }
    public string KohiFutansyaNoFrom { get; set; } = string.Empty;

    public string KohiFutansyaNoTo { get; set; } = string.Empty;
    public string KohiTokusyuNoFrom { get; set; } = string.Empty;
    public string KohiTokusyuNoTo { get; set; } = string.Empty;
    public int ExpireDateFrom { get; set; }

    public DateTime? ExpireDateFromBinding => CIUtil.IntToDate(ExpireDateFrom);
    public int ExpireDateTo
    {
        get; set;
    }

    public DateTime? ExpireDateToBinding => CIUtil.IntToDate(ExpireDateTo);
    public List<int> HokenSbt { get; set; } = new();

    public string HokenSbtStr
    {
        get => string.Join(",", HokenSbt.ToArray());
    }

    public string Houbetu1 { get; set; } = string.Empty;
    public string Houbetu2 { get; set; } = string.Empty;
    public string Houbetu3 { get; set; } = string.Empty;
    public string Houbetu4 { get; set; } = string.Empty;
    public string Houbetu5 { get; set; } = string.Empty;
    public string Kogaku { get; set; } = string.Empty;
    public List<int> KogakuKbns
    {
        get => string.IsNullOrEmpty(Kogaku) ? new List<int>() : Kogaku.Split(',').Select(x => x.AsInteger()).ToList();
    }

    public int KohiHokenNoFrom { get; set; }
    public int KohiHokenEdaNoFrom { get; set; }
    public string KohiHokenMasterFrom { get; set; } = string.Empty;

    private HokenMstModel _hokenMstModelFrom;
    public HokenMstModel HokenMstModelFrom
    {
        get => _hokenMstModelFrom;
        set
        {
            if (value == null)
            {
                KohiHokenNoFrom = 0;
                KohiHokenEdaNoFrom = 0;
                return;
            }
            KohiHokenNoFrom = HokenMstModelFrom.HokenNumber;
            KohiHokenEdaNoFrom = HokenMstModelFrom.HokenEdaNo;
            if (HokenMstModelTo == null)
            {
                KohiHokenMasterTo = value.SelectedValueMaster;
            }
        }
    }
    public int KohiHokenNoTo { get; set; }
    public int KohiHokenEdaNoTo { get; set; }
    public string KohiHokenMasterTo { get; set; }

    private HokenMstModel _hokenMstModelTo;
    public HokenMstModel HokenMstModelTo
    {
        get => _hokenMstModelTo;
        set
        {
            if (value == null)
            {
                KohiHokenNoTo = 0;
                KohiHokenEdaNoTo = 0;
                return;
            }
            KohiHokenNoTo = HokenMstModelTo.HokenNumber;
            KohiHokenEdaNoTo = HokenMstModelTo.HokenEdaNo;
        }
    }

    public CoSta9000HokenConf AsCoSta9000HokenConf()
    {
        return new CoSta9000HokenConf()
        {
            StartHokensyaNo = HokensyaNoFrom.AsString(),
            EndHokensyaNo = HokensyaNoTo.AsString(),
            Kigo = Kigo.AsString(),
            Bango = Bango.AsString(),
            HonkeKbn = HokenKbn,
            StartFutansyaNo = KohiFutansyaNoFrom.AsString(),
            EndFutansyaNo = KohiFutansyaNoTo.AsString(),
            StartTokusyuNo = KohiTokusyuNoFrom.AsString(),
            EndTokusyuNo = KohiTokusyuNoTo.AsString(),
            StartDate = ExpireDateFrom,
            EndDate = ExpireDateTo,
            HokenSbts = HokenSbt,
            Houbetu0 = Houbetu1.AsString(),
            Houbetu1 = Houbetu2.AsString(),
            Houbetu2 = Houbetu3.AsString(),
            Houbetu3 = Houbetu4.AsString(),
            Houbetu4 = Houbetu5.AsString(),
            StartKohiHokenNo = new KohiHokenMst(KohiHokenNoFrom, KohiHokenEdaNoFrom),
            EndKohiHokenNo = new KohiHokenMst(KohiHokenNoTo, KohiHokenEdaNoTo),
            KogakuKbns = KogakuKbns,
            EdaNo = EdaNo
        };
    }

    public bool IsDefaultHokenConfInView()
    {
        bool isDefaultHokenSbt = HokenSbt.Count == 0;
        return string.IsNullOrEmpty(HokensyaNoFrom) && string.IsNullOrEmpty(HokensyaNoTo) &&
            string.IsNullOrEmpty(Kigo) && string.IsNullOrEmpty(Bango) && HokenKbn == 0 &&
            string.IsNullOrEmpty(KohiFutansyaNoFrom) && string.IsNullOrEmpty(KohiFutansyaNoTo) &&
            string.IsNullOrEmpty(KohiTokusyuNoFrom) && string.IsNullOrEmpty(KohiTokusyuNoTo) &&
            ExpireDateFrom == 0 && ExpireDateTo == 0 && isDefaultHokenSbt && string.IsNullOrEmpty(Houbetu1) &&
            string.IsNullOrEmpty(Houbetu2) && string.IsNullOrEmpty(Houbetu3) && string.IsNullOrEmpty(Houbetu4) && string.IsNullOrEmpty(Houbetu5) &&
            KohiHokenNoFrom == 0 && KohiHokenEdaNoFrom == 0 &&
            KohiHokenNoTo == 0 && KohiHokenEdaNoTo == 0 && KogakuKbns.Count == 0 && string.IsNullOrEmpty(EdaNo);
    }
    #endregion

    #region 病名情報
    public int StartDateFrom { get; set; }

    public DateTime? StartDateFromBinding => CIUtil.IntToDate(StartDateFrom);

    public int StartDateTo { get; set; }
    public DateTime? StartDateToBinding => CIUtil.IntToDate(StartDateTo);
    public int TenkiDateFrom
    {
        get; set;
    }

    public DateTime? TenkiDateFromBinding => CIUtil.IntToDate(TenkiDateFrom);
    public int TenkiDateTo
    {
        get; set;
    }

    public DateTime? TenkiDateToBinding => CIUtil.IntToDate(TenkiDateTo);

    /// <summary>
    /// 転帰区分
    /// 1:継続 2:治ゆ 3:死亡 4:中止 9:その他
    /// </summary>
    public List<int> TenkiKbns { get; set; }
    public string TenkiKbnStr
    {
        get => string.Join(",", TenkiKbns.ToArray());
    }

    /// <summary>
    /// 疾患区分
    /// 3:皮(1) 4:皮(2) 5:特疾 7:てんかん 8:特疾又はてんかん
    /// </summary>
    public List<int> SikkanKbns { get; set; } = new();

    public string SikkanKbnStr
    {
        get => string.Join(",", SikkanKbns.ToArray());
    }
    public int IsDoubt { get; set; }

    /// <summary>
    /// 主病名
    ///     0: すべて、1:主病名のみ、2:主病名以外
    /// </summary>
    public int Syubyo { get; set; }

    public string SearchWord { get; set; } = string.Empty;
    public int SearchWordMode { get; set; }
    public List<string> ByomeiCds
    {
        get; set;
    } = new();

    public string ByomeiCdStr
    {
        get => string.Join(",", ByomeiCds.ToArray());
    }

    public List<string> FreeByomeis { get; set; }

    public string FreeByomeiStr
    {
        get => string.Join(",", FreeByomeis.ToArray());
    }

    /// <summary>
    /// （or, and）
    /// and:1
    /// </summary>
    public int ByomeiCdOpt { get; set; }
    public List<int> NanbyoCds { get; set; } = new();

    public string NanbyoCdsStr
    {
        get => string.Join(",", NanbyoCds.ToArray());
    }

    public CoSta9000ByomeiConf AsCoSta9000ByomeiConf()
    {
        if (IsDefaultInputByomeiConfCondition())
        {
            return null;
        }
        return new CoSta9000ByomeiConf()
        {
            StartStartDate = StartDateFrom,
            EndStartDate = StartDateTo,
            StartTenkiDate = TenkiDateFrom,
            EndTenkiDate = TenkiDateTo,
            TenkiKbns = TenkiKbns,
            SikkanKbns = SikkanKbns,
            IsDoubt = IsDoubt,
            Syubyo = Syubyo,
            SearchWord = SearchWord.AsString(),
            WordOpt = SearchWordMode,
            ByomeiCdOpt = ByomeiCdOpt,
            ByomeiCds = ByomeiCds,
            Byomeis = FreeByomeis,
            NanbyoCds = NanbyoCds
        };
    }

    private bool IsDefaultInputByomeiConfCondition()
    {
        bool isDefaultTenkiKbn = TenkiKbns.Count == 0;
        bool isDefaultSikkan = SikkanKbns.Count == 0;
        bool isDefaultFreeByomei = FreeByomeis.Count == 0;
        bool isDefaultNanbyoCds = NanbyoCds.Count == 0;
        bool isMainByomei = Syubyo == 0;
        return StartDateFrom == 0 && StartDateTo == 0 && TenkiDateFrom == 0 && isDefaultTenkiKbn && isDefaultSikkan &&
            TenkiDateTo == 0 && IsDoubt == 0 && string.IsNullOrEmpty(SearchWord) && ByomeiCds.Count == 0 && isDefaultFreeByomei && isDefaultNanbyoCds && isMainByomei;
    }

    public bool IsDefaultByomeiConfInView()
    {
        return IsDefaultInputByomeiConfCondition() && SearchWordMode == 0 && ByomeiCdOpt == 0;
    }
    #endregion

    #region 来院情報
    public int SindateFrom { get; set; }

    public DateTime? SindateFromBinding => CIUtil.IntToDate(SindateFrom);
    public int SindateTo { get; set; }
    public DateTime? SindateToBinding => CIUtil.IntToDate(SindateTo);
    public int LastVisitDateFrom
    {
        get; set;
    }

    public DateTime? LastVisitDateFromBinding => CIUtil.IntToDate(LastVisitDateFrom);
    public int LastVisitDateTo
    {
        get; set;
    }

    public DateTime? LastVisitDateToBinding => CIUtil.IntToDate(LastVisitDateTo);
    public List<int> Statuses { get; set; } = new();
    public string StatuseStr
    {
        get => string.Join(",", Statuses.ToArray());
    }

    public List<int> UketukeSbtId
    {
        get; set;
    } = new();

    public string UketukeSbtStr
    {
        get => string.Join(",", UketukeSbtId.ToArray());
    }

    /// <summary>
    /// KA_MST.KA_ID をカンマ区切り
    /// </summary>
    public List<int> KaMstId { get; set; } = new();

    public string KaMstStr
    {
        get => string.Join(",", KaMstId.ToArray());
    }

    public List<int> UserMstId { get; set; } = new();
    public string UserMstStr
    {
        get => string.Join(",", UserMstId.ToArray());
    }

    /// <summary>
    /// 時間枠区分
    ///     1:時間内 2:時間外 3:休日 4:深夜 5:夜・早
    /// </summary>
    public List<int> JikanKbns { get; set; } = new();
    public string JikanKbnStr
    {
        get => string.Join(",", JikanKbns.ToArray());
    }

    public int IsSinkan { get; set; }
    public string RaiinAgeFrom { get; set; } = string.Empty;
    public string RaiinAgeTo { get; set; } = string.Empty;
    public CoSta9000RaiinConf AsCoSta9000RaiinConf()
    {
        return new CoSta9000RaiinConf()
        {
            StartSinDate = SindateFrom,
            EndSinDate = SindateTo,
            StartLastVisitDate = LastVisitDateFrom,
            EndLastVisitDate = LastVisitDateTo,
            Statuses = Statuses,
            UketukeSbts = UketukeSbtId,
            KaIds = KaMstId,
            TantoIds = UserMstId,
            IsSinkan = IsSinkan,
            AgeFrom = string.IsNullOrEmpty(RaiinAgeFrom) ? -1 : RaiinAgeFrom.AsInteger(),
            AgeTo = string.IsNullOrEmpty(RaiinAgeTo) ? -1 : RaiinAgeTo.AsInteger(),
            JikanKbns = JikanKbns,
        };
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public bool IsDefaultRaiinConfInView()
    {
        bool isDefaultStatus = Statuses.Count == 0;
        bool isDefaultUkesuke = UketukeSbtId.Count == 0;
        bool isDefaultKaMst = KaMstId.Count == 0;
        bool isDefaultUserMst = UserMstId.Count == 0;
        bool isDefaultJikanKbn = JikanKbns.Count == 0;
        return SindateFrom == 0 && SindateTo == 0 && LastVisitDateFrom == 0 &&
            LastVisitDateTo == 0 && isDefaultStatus && isDefaultUkesuke && isDefaultKaMst &&
            isDefaultUserMst && RaiinAgeFrom.AsInteger() == 0 && RaiinAgeTo.AsInteger() == 0 && IsSinkan == 0 && isDefaultJikanKbn;
    }
    #endregion

    #region 診療情報
    public int DataKind { get; set; }
    public List<string> ItemCds { get; set; } = new();
    public string ItemCdStr
    {
        get => string.Join(",", ItemCds.ToArray());
    }

    public int ItemCdOpt { get; set; }

    public string MedicalSearchWord { get; set; } = string.Empty;
    public int WordOpt { get; set; }
    public List<string> ItemCmts { get; set; } = new();
    public string ItemCmtStr
    {
        get => string.Join(",", ItemCmts.ToArray());
    }

    public CoSta9000SinConf AsCoSta9000SinConf()
    {
        if (IsDefaultInputSinConfCondition())
        {
            return null;
        }
        return new CoSta9000SinConf()
        {
            DataKind = DataKind,
            ItemCds = ItemCds,
            ItemCdOpt = ItemCdOpt,
            SearchWord = MedicalSearchWord.AsString(),
            WordOpt = WordOpt,
            ItemCmts = ItemCmts
        };
    }

    private bool IsDefaultInputSinConfCondition()
    {
        return ItemCds.Count == 0 && string.IsNullOrEmpty(MedicalSearchWord) && ItemCmts.Count == 0;
    }

    public bool IsDefaultSinConfInView()
    {
        return DataKind == 0 && ItemCdOpt == 0 && WordOpt == 0 && IsDefaultInputSinConfCondition();
    }
    #endregion

    #region カルテ情報
    public List<int> KarteKbns { get; set; } = new();
    public string KarteKbnsStr
    {
        get => string.Join(",", KarteKbns.ToArray());
    }

    public List<string> KarteSearchWords { get; set; } = new();
    public string KarteSearchWordsStr
    {
        get => string.Join(",", KarteSearchWords.ToArray());
    }

    public int KarteWordOpt { get; set; }
    public CoSta9000KarteConf AsCoSta9000KarteConf()
    {
        if (IsDefaultInputKarteConfCondition())
        {
            return null;
        }
        return new CoSta9000KarteConf()
        {
            KarteKbns = KarteKbns,
            SearchWords = KarteSearchWords,
            WordOpt = KarteWordOpt,
        };
    }

    private bool IsDefaultInputKarteConfCondition()
    {
        bool isDefautKarteKbn = KarteKbns.Count == 0;
        return isDefautKarteKbn && KarteSearchWords.Count == 0;
    }

    public bool IsDefaultKarteConfInView()
    {
        return IsDefaultInputKarteConfCondition() && KarteWordOpt == 0;
    }
    #endregion

    #region 検査情報
    public int StartIraiDate { get; set; }
    public DateTime? StartIraiDateBinding => CIUtil.IntToDate(StartIraiDate);
    public int EndIraiDate { get; set; }

    public DateTime? EndIraiDateBinding => CIUtil.IntToDate(EndIraiDate);

    /// <summary>
    /// 検査項目
    ///     項目コード,結果値下限,結果値上限,異常値<1:H 2:L 3:HorL>,… の繰り返し
    /// </summary>
    public List<string> KensaItemCds
    {
        get; set;
    } = new();

    public string KensaItemCdsStr
    {
        get => string.Join(",", KensaItemCds.ToArray());
    }

    /// <summary>
    /// 検査項目の検索オプション
    ///     0:or 1:and
    /// </summary>
    public int KensaItemCdOpt { get; set; }
    public CoSta9000KensaConf AsCoSta9000KensaConf()
    {
        if (IsDefaultInputKensaConfCondition())
        {
            return null;
        }
        return new CoSta9000KensaConf()
        {
            StartIraiDate = StartIraiDate,
            EndIraiDate = EndIraiDate,
            ItemCdOpt = KensaItemCdOpt,
            ItemCds = KensaItemCds,
        };
    }

    private bool IsDefaultInputKensaConfCondition()
    {
        return StartIraiDate == 0 && EndIraiDate == 0 && KensaItemCds.Count == 0;
    }

    public bool IsDefaultKensaConfInView()
    {
        return IsDefaultInputKensaConfCondition() && KensaItemCdOpt == 0;
    }
    #endregion
}
