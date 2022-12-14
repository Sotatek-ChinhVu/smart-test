using Domain.Models.HpInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using Domain.Models.PatientInfor;
using Domain.Models.RaiinFilterMst;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;
using Domain.Models.User;
using Helper.Common;
using UseCase.Document;
using UseCase.Document.GetListParamTemplate;

namespace Interactor.Document.CommonGetListParam;

public class CommonGetListParam : ICommonGetListParam
{
    private readonly IInsuranceRepository _insuranceRepository;
    private readonly IHpInfRepository _hpInfRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRaiinFilterMstRepository _raiinFilterMstRepository;
    private readonly ISummaryInfRepository _summaryInfRepository;
    private readonly IPatientInfoRepository _patientInfoRepository;
    private readonly IInsuranceMstRepository _insuranceMstRepository;

    public CommonGetListParam(IInsuranceRepository insuranceRepository, IHpInfRepository hpInfRepository, IPatientInforRepository patientInforRepository, IUserRepository userRepository, IRaiinFilterMstRepository raiinFilterMstRepository, ISummaryInfRepository summaryInfRepository, IPatientInfoRepository patientInfoRepository, IInsuranceMstRepository insuranceMstRepository)
    {
        _insuranceRepository = insuranceRepository;
        _hpInfRepository = hpInfRepository;
        _patientInforRepository = patientInforRepository;
        _userRepository = userRepository;
        _raiinFilterMstRepository = raiinFilterMstRepository;
        _summaryInfRepository = summaryInfRepository;
        _patientInfoRepository = patientInfoRepository;
        _insuranceMstRepository = insuranceMstRepository;
    }

    public List<ItemDisplayParamModel> GetListParam(int hpId, int userId, long ptId, int sinDate, long raiinNo, int hokenPId)
    {
        // HpInf and PtInf
        UserMstModel docInf = new();
        UserMstModel tantoInf = new();
        int age = 0;
        var hpInf = _hpInfRepository.GetHpInf(hpId);
        var ptInf = _patientInforRepository.GetById(hpId, ptId, 0, 0);
        var userLogin = _userRepository.GetByUserId(userId, sinDate);
        var tantoId = _raiinFilterMstRepository.GetTantoId(ptId, sinDate, raiinNo);
        if (ptInf != null)
        {
            docInf = _userRepository.GetByUserId(ptInf.PrimaryDoctor, sinDate);
            age = CIUtil.SDateToAge(ptInf.Birthday, sinDate);
        }
        if (tantoId > 0)
        {
            tantoInf = _userRepository.GetByUserId(tantoId, sinDate);
        }
        var sumaryContent = _summaryInfRepository.Get(hpId, ptId).Text;
        int lastTimeDate = _raiinFilterMstRepository.GetLastTimeDate(hpId, ptId, sinDate);
        var listKensaInf = _patientInfoRepository.GetListKensaInfModel(hpId, ptId, sinDate);

        // hoken
        var hokenPattern = _insuranceRepository.GetInsuranceListById(hpId, ptId, sinDate, hokenPId);
        var insuranceMstModel = _insuranceMstRepository.GetDataInsuranceMst(hpId, ptId, sinDate);
        var hokenModelToReplace = GetListParamHokenInf(age, hokenPattern, insuranceMstModel);

        // Get list param
        List<ItemDisplayParamModel> result = new();
        result.AddRange(GetListParamHpAndPtInf(sinDate, age, lastTimeDate, sumaryContent, hpInf, ptInf != null ? ptInf : new(), docInf, tantoInf, userLogin, listKensaInf));
        result.AddRange(ReplaceParamHokenAction(hokenModelToReplace));
        return result;
    }

    private List<ItemDisplayParamModel> GetListParamHpAndPtInf(int sindate, int age, int lastTimeDate, string sumaryContent, HpInfModel hpInf, PatientInforModel ptInf, UserMstModel docInf, UserMstModel tantoInf, UserMstModel userLogin, List<KensaInfDetailModel> listKensaInf)
    {
        List<ItemDisplayParamModel> listParam = new();

        var kensaHeightValue = string.Empty;
        var kensaWeightValue = string.Empty;
        var kensaBMIValue = string.Empty;
        if (listKensaInf.Any())
        {
            var kensaHeight = listKensaInf.FirstOrDefault(u => u.KensaItemCd.Contains("V0001") && !string.IsNullOrEmpty(u.ResultVal));
            kensaHeightValue = kensaHeight != null ? kensaHeight.ResultVal + "cm" + "(" + CIUtil.SDateToShowSDate(CIUtil.DateTimeToInt(kensaHeight.UpdateDate)) + ")" : string.Empty;

            var kensaWeight = listKensaInf.FirstOrDefault(u => u.KensaItemCd.Contains("V0002") && !string.IsNullOrEmpty(u.ResultVal));
            kensaWeightValue = kensaWeight != null ? kensaWeight.ResultVal + "kg" + "(" + CIUtil.SDateToShowSDate(CIUtil.DateTimeToInt(kensaWeight.UpdateDate)) + ")" : string.Empty;

            var kensaBMI = listKensaInf.FirstOrDefault(u => u.KensaItemCd.Contains("V0003") && !string.IsNullOrEmpty(u.ResultVal));
            kensaBMIValue = kensaBMI != null ? kensaBMI.ResultVal + "(" + CIUtil.SDateToShowSDate(CIUtil.DateTimeToInt(kensaBMI.UpdateDate)) + ")" : string.Empty;
        }

        // HpInf
        listParam.Add(new ItemDisplayParamModel("基本情報", "作成日(西暦K)", CIUtil.SDateToShowSWDate(int.Parse(DateTime.UtcNow.ToString("yyyMMdd")), 0, 0, 1)));
        listParam.Add(new ItemDisplayParamModel("基本情報", "作成日(西暦/)", CIUtil.SDateToShowSDate(int.Parse(DateTime.UtcNow.ToString("yyyMMdd")))));
        listParam.Add(new ItemDisplayParamModel("基本情報", "作成日(和暦K)", CIUtil.SDateToShowWDate2(int.Parse(DateTime.UtcNow.ToString("yyyMMdd")))));
        listParam.Add(new ItemDisplayParamModel("基本情報", "医療機関名称", hpInf.HpName));
        listParam.Add(new ItemDisplayParamModel("基本情報", "医療機関住所１", hpInf.Address1));
        listParam.Add(new ItemDisplayParamModel("基本情報", "医療機関住所２", hpInf.Address2));
        listParam.Add(new ItemDisplayParamModel("基本情報", "医療機関電話番号", hpInf.Tel));

        // PtInf
        listParam.Add(new ItemDisplayParamModel("患者情報", "患者番号", ptInf.PtNum.ToString()));
        listParam.Add(new ItemDisplayParamModel("患者情報", "氏名", ptInf.Name));
        listParam.Add(new ItemDisplayParamModel("患者情報", "カナ氏名", ptInf.KanaName));
        listParam.Add(new ItemDisplayParamModel("患者情報", "氏名(旧姓)"));
        listParam.Add(new ItemDisplayParamModel("患者情報", "カナ氏名(旧姓)"));
        listParam.Add(new ItemDisplayParamModel("患者情報", "郵便番号", ptInf.HomePost));
        listParam.Add(new ItemDisplayParamModel("患者情報", "住所１", ptInf.HomeAddress1));
        listParam.Add(new ItemDisplayParamModel("患者情報", "住所２", ptInf.HomeAddress2));
        listParam.Add(new ItemDisplayParamModel("患者情報", "電話番号１", ptInf.Tel1));
        listParam.Add(new ItemDisplayParamModel("患者情報", "電話番号２", ptInf.Tel2));
        listParam.Add(new ItemDisplayParamModel("患者情報", "生年月日(西暦/)", CIUtil.SDateToShowSDate(ptInf.Birthday)));
        listParam.Add(new ItemDisplayParamModel("患者情報", "生年月日(西暦K)", CIUtil.SDateToShowSWDate(ptInf.Birthday, 0, 0, 1)));
        listParam.Add(new ItemDisplayParamModel("患者情報", "生年月日(和暦/)", CIUtil.SDateToShowWDate(ptInf.Birthday)));
        listParam.Add(new ItemDisplayParamModel("患者情報", "生年月日(和暦K)", CIUtil.SDateToShowWDate2(ptInf.Birthday)));
        listParam.Add(new ItemDisplayParamModel("患者情報", "年齢", age.ToString()));
        listParam.Add(new ItemDisplayParamModel("患者情報", "性別", ptInf.Sex == 1 ? "男" : "女"));
        listParam.Add(new ItemDisplayParamModel("患者情報", "世帯主氏名", ptInf.Setanusi));
        listParam.Add(new ItemDisplayParamModel("患者情報", "死亡日(西暦/)", CIUtil.SDateToShowSDate(ptInf.DeathDate)));
        listParam.Add(new ItemDisplayParamModel("患者情報", "死亡日(西暦K)", CIUtil.SDateToShowSWDate(ptInf.DeathDate, 0, 0, 1)));
        listParam.Add(new ItemDisplayParamModel("患者情報", "死亡日(和暦/)", CIUtil.SDateToShowWDate(ptInf.DeathDate)));
        listParam.Add(new ItemDisplayParamModel("患者情報", "死亡日(和暦K)", CIUtil.SDateToShowWDate2(ptInf.DeathDate)));
        listParam.Add(new ItemDisplayParamModel("患者情報", "勤務先", ptInf.OfficeName));
        listParam.Add(new ItemDisplayParamModel("患者情報", "勤務先住所", ptInf.OfficeAddress1 + ptInf.OfficeAddress2));
        listParam.Add(new ItemDisplayParamModel("患者情報", "職業", ptInf.Job));
        listParam.Add(new ItemDisplayParamModel("患者情報", "メールアドレス", ptInf.Mail));
        listParam.Add(new ItemDisplayParamModel("患者情報", "メモ", !string.IsNullOrEmpty(ptInf.Memo) ? ptInf.Memo.Replace("\r\n", string.Empty).Replace("\n", string.Empty) : string.Empty));
        listParam.Add(new ItemDisplayParamModel("患者情報", "主治医氏名", docInf.Name));
        listParam.Add(new ItemDisplayParamModel("患者情報", "サマリ", sumaryContent.Replace("\r\n", string.Empty).Replace("\n", string.Empty)));
        listParam.Add(new ItemDisplayParamModel("患者情報", "担当医", tantoInf.Name));
        listParam.Add(new ItemDisplayParamModel("患者情報", "保険医師名", userLogin.DrName));
        listParam.Add(new ItemDisplayParamModel("患者情報", "最終来院日(西暦/)", lastTimeDate > 0 ? CIUtil.SDateToShowSDate(lastTimeDate) : string.Empty));
        listParam.Add(new ItemDisplayParamModel("患者情報", "最終来院日(西暦K)", lastTimeDate > 0 ? CIUtil.SDateToShowSWDate(lastTimeDate, 0, 0, 1) : string.Empty));
        listParam.Add(new ItemDisplayParamModel("患者情報", "最終来院日(和暦/)", lastTimeDate > 0 ? CIUtil.SDateToShowWDate(lastTimeDate) : string.Empty));
        listParam.Add(new ItemDisplayParamModel("患者情報", "最終来院日(和暦K)", lastTimeDate > 0 ? CIUtil.SDateToShowWDate2(lastTimeDate) : string.Empty));
        listParam.Add(new ItemDisplayParamModel("患者情報", "身長", kensaHeightValue));
        listParam.Add(new ItemDisplayParamModel("患者情報", "体重", kensaWeightValue));
        listParam.Add(new ItemDisplayParamModel("患者情報", "BMI", kensaBMIValue));
        listParam.Add(new ItemDisplayParamModel("患者情報", "診療日(西暦K)", CIUtil.SDateToShowSWDate(sindate, 0, 0, 1)));
        listParam.Add(new ItemDisplayParamModel("患者情報", "診療日(和暦K)", CIUtil.SDateToShowWDate2(sindate)));
        listParam.Add(new ItemDisplayParamModel("患者情報", "診療日(元号)", CIUtil.GetEraFromDate(sindate)));
        listParam.Add(new ItemDisplayParamModel("患者情報", "診療日(元号略)", CIUtil.GetEraRekiFromDate(sindate)));
        listParam.Add(new ItemDisplayParamModel("患者情報", "診療日(元号略K)", CIUtil.GetEraRekiFromDate(sindate, 1)));

        return listParam;
    }

    private HokenParamReplaceModel GetListParamHokenInf(int age, InsuranceDataModel hokenPattern, InsuranceMstModel insuranceMstModel)
    {
        var hokenInf = hokenPattern.ListHokenInf.FirstOrDefault();
        var insuranceModel = hokenPattern.ListInsurance.FirstOrDefault();
        var roudouMsts = insuranceMstModel.RoudouMst;
        var kantokuMsts = insuranceMstModel.KantokuMstData;
        var byomeiMstAftercares = insuranceMstModel.ByomeiMstAftercareData;
        var tokkiMsts = insuranceMstModel.ListTokkiMstModel;
        var tokkiDict = tokkiMsts.ToDictionary(x => x.TokkiCd, x => x.TokkiCd + x.TokkiName);

        HokenParamReplaceModel replaceModel = new();

        #region get params
        if (hokenInf != null && hokenInf.HokenId != 0 && !hokenInf.IsNoHoken)
        {
            if (hokenInf.IsRousai)
            {
                if (hokenInf.RousaiSaigaiKbn == 1)
                {
                    replaceModel.RousaiSaigaiKbn = "業務中の災害";
                }
                else if (hokenInf.RousaiSaigaiKbn == 2)
                {
                    replaceModel.RousaiSaigaiKbn = "通勤途上の災害";
                }

                if (!string.IsNullOrEmpty(hokenInf.RousaiRoudouCd))
                {
                    var roudoMst = roudouMsts.Find(x => x.RoudouCD == hokenInf.RousaiRoudouCd);
                    if (roudoMst != null)
                    {
                        replaceModel.RousaiRoudouKyoku = roudoMst.RoudouDisplay;
                    }
                }

                if (!string.IsNullOrEmpty(hokenInf.RousaiKantokuCd))
                {
                    var kantokuMst = kantokuMsts.Find(x => x.KantokuCD == hokenInf.RousaiKantokuCd);
                    if (kantokuMst != null)
                    {
                        replaceModel.RousaiKantoku = kantokuMst.KantokuName;
                    }
                }
                if (!string.IsNullOrEmpty(hokenInf.RousaiSyobyoCd))
                {
                    var syobyoMst = byomeiMstAftercares.Find(x => x.ByomeiCd == hokenInf.RousaiSyobyoCd);
                    if (syobyoMst != null)
                    {
                        replaceModel.RousaiShyobyoCode = syobyoMst.ByomeiDisplay;
                    }
                }

                replaceModel.RousaiRoudouHokenNo = hokenInf.RousaiKofuNo;
                replaceModel.RousaiJigyouName = hokenInf.RousaiJigyosyoName;
                replaceModel.RousaiJigyuoAddress = hokenInf.RousaiPrefName + hokenInf.RousaiCityName;
                replaceModel.RousaiShyobyoDateWest = CIUtil.SDateToShowSDate(hokenInf.RousaiSyobyoDate);
                replaceModel.RousaiShyobyoDateWestK = CIUtil.SDateToShowSWDate(hokenInf.RousaiSyobyoDate, 0, 0, 1);
                replaceModel.RousaiShyobyoDateJapan = CIUtil.SDateToShowWDate(hokenInf.RousaiSyobyoDate);
                replaceModel.RousaiShyobyoDateJapanK = CIUtil.SDateToShowWDate2(hokenInf.RousaiSyobyoDate);
                replaceModel.RousaiRyouyouPeriodWestS = CIUtil.SDateToShowSDate(hokenInf.StartDate);
                replaceModel.RousaiRyouyouPeriodWestKS = CIUtil.SDateToShowSWDate(hokenInf.StartDate, 0, 0, 1);
                replaceModel.RousaiRyouyouPeriodJapanS = CIUtil.SDateToShowWDate(hokenInf.StartDate);
                replaceModel.RousaiRyouyouPeriodJapanKS = CIUtil.SDateToShowWDate2(hokenInf.StartDate);
                replaceModel.RousaiRyouyouPeriodWestE = CIUtil.SDateToShowSDate(hokenInf.EndDate);
                replaceModel.RousaiRyouyouPeriodWestKE = CIUtil.SDateToShowSWDate(hokenInf.EndDate, 0, 0, 1);
                replaceModel.RousaiRyouyouPeriodJapanE = CIUtil.SDateToShowWDate(hokenInf.EndDate);
                replaceModel.RousaiRyouyouPeriodJapanKE = CIUtil.SDateToShowWDate2(hokenInf.EndDate);
            }
            else if (hokenInf.IsJibai)
            {
                replaceModel.JibaiHokenCompanyName = hokenInf.JibaiHokenName;
                replaceModel.JibaiHokenTanto = hokenInf.JibaiHokenTanto;
                replaceModel.JibaiHokenContact = hokenInf.JibaiHokenTel;
                replaceModel.JibaiJushouDateWest = CIUtil.SDateToShowSDate(hokenInf.JibaiJyusyouDate);
                replaceModel.JibaiJushouDateWestK = CIUtil.SDateToShowSWDate(hokenInf.JibaiJyusyouDate, 0, 0, 1);
                replaceModel.JibaiJushouDateJapan = CIUtil.SDateToShowWDate(hokenInf.JibaiJyusyouDate);
                replaceModel.JibaiJushouDateJapanK = CIUtil.SDateToShowWDate2(hokenInf.JibaiJyusyouDate);
                replaceModel.JibaiSinDateWest = CIUtil.SDateToShowSDate(hokenInf.RyoyoStartDate);
                replaceModel.JibaiSinDateWestK = CIUtil.SDateToShowSWDate(hokenInf.RyoyoStartDate, 0, 0, 1);
                replaceModel.JibaiSinDateJapan = CIUtil.SDateToShowWDate(hokenInf.RyoyoStartDate);
                replaceModel.JibaiSinDateJapanK = CIUtil.SDateToShowWDate2(hokenInf.RyoyoStartDate);
            }
            else
            {
                replaceModel.HokenNo = hokenInf.HokensyaNo.ToString();
                replaceModel.HokenKigo = hokenInf.Kigo;
                replaceModel.HokenBango = hokenInf.Bango;
                if (!string.IsNullOrEmpty(hokenInf.EdaNo))
                {
                    replaceModel.EdaNo = hokenInf.EdaNo?.PadLeft(2, '0') ?? string.Empty;
                }

                if (hokenInf.HonkeKbn == 1)
                {
                    replaceModel.HokenHonki = "本人";
                }
                else if (hokenInf.HonkeKbn == 2)
                {
                    replaceModel.HokenHonki = "家族";
                }

                string kongaku = "";
                if (age > 70)
                {
                    switch (hokenInf.KogakuKbn)
                    {
                        case 0:
                            kongaku = "一般";
                            break;
                        case 3:
                            kongaku = "上位(～2018/07)";
                            break;
                        case 4:
                            kongaku = "低所Ⅱ";
                            break;
                        case 5:
                            kongaku = "低所Ⅰ";
                            break;
                        case 6:
                            kongaku = "特定収入(～2008/12)";
                            break;
                        case 26:
                            kongaku = "現役Ⅲ";
                            break;
                        case 27:
                            kongaku = "現役Ⅱ";
                            break;
                        case 28:
                            kongaku = "現役Ⅰ";
                            break;
                    }
                }
                else
                {
                    switch (hokenInf.KogakuKbn)
                    {
                        case 0:
                            kongaku = "限度額認定証なし";
                            break;
                        case 17:
                            kongaku = "上位[A] (～2014/12)";
                            break;
                        case 18:
                            kongaku = "一般[B] (～2014/12)";
                            break;
                        case 19:
                            kongaku = "低所[C] (～2014/12)";
                            break;
                        case 26:
                            kongaku = "区ア／標準報酬月額83万円以上";
                            break;
                        case 27:
                            kongaku = "区イ／標準報酬月額53..79万円";
                            break;
                        case 28:
                            kongaku = "区ウ／標準報酬月額28..50万円";
                            break;
                        case 29:
                            kongaku = "区エ／標準報酬月額26万円以下";
                            break;
                        case 30:
                            kongaku = "区オ／低所得者";
                            break;
                    }
                }

                replaceModel.HokenKogakuKbn = kongaku;
                replaceModel.HokenPeriodWestS = CIUtil.SDateToShowSDate(hokenInf.StartDate);
                replaceModel.HokenPeriodWestKS = CIUtil.SDateToShowSWDate(hokenInf.StartDate, 0, 0, 1);
                replaceModel.HokenPeriodJapanS = CIUtil.SDateToShowWDate(hokenInf.StartDate);
                replaceModel.HokenPeriodJapanKS = CIUtil.SDateToShowWDate2(hokenInf.StartDate);
                replaceModel.HokenPeriodWestE = CIUtil.SDateToShowSDate(hokenInf.EndDate);
                replaceModel.HokenPeriodWestKE = CIUtil.SDateToShowSWDate(hokenInf.EndDate, 0, 0, 1);
                replaceModel.HokenPeriodJapanE = CIUtil.SDateToShowWDate(hokenInf.EndDate);
                replaceModel.HokenPeriodJapanKE = CIUtil.SDateToShowWDate2(hokenInf.EndDate);

                string genmen = "";
                switch (hokenInf.GenmenKbn)
                {
                    case 1:
                        genmen = "減額";
                        break;
                    case 2:
                        genmen = "免除";
                        break;
                    case 3:
                        genmen = "支払猶予";
                        break;
                }

                replaceModel.HokenKokuhoGenmen = genmen;

                string syokumu = "";
                switch (hokenInf.SyokumuKbn)
                {
                    case 1:
                        syokumu = "職務上";
                        break;
                    case 2:
                        syokumu = "下船後３月以内";
                        break;
                    case 3:
                        syokumu = "通勤災害";
                        break;
                }
                replaceModel.HokenSenin = syokumu;

                if (hokenInf.Tokki1 != null)
                {
                    replaceModel.HokenSpecialNote1 = tokkiDict.ContainsKey(hokenInf.Tokki1) ? tokkiDict[hokenInf.Tokki1] : string.Empty;
                }
                if (hokenInf.Tokki2 != null)
                {
                    replaceModel.HokenSpecialNote2 = tokkiDict.ContainsKey(hokenInf.Tokki2) ? tokkiDict[hokenInf.Tokki2] : string.Empty;
                }
                if (hokenInf.Tokki3 != null)
                {
                    replaceModel.HokenSpecialNote3 = tokkiDict.ContainsKey(hokenInf.Tokki3) ? tokkiDict[hokenInf.Tokki3] : string.Empty;
                }
                if (hokenInf.Tokki4 != null)
                {
                    replaceModel.HokenSpecialNote4 = tokkiDict.ContainsKey(hokenInf.Tokki4) ? tokkiDict[hokenInf.Tokki4] : string.Empty;
                }
                if (hokenInf.Tokki5 != null)
                {
                    replaceModel.HokenSpecialNote5 = tokkiDict.ContainsKey(hokenInf.Tokki5) ? tokkiDict[hokenInf.Tokki5] : string.Empty;
                }
            }
        }

        if (insuranceModel != null && !insuranceModel.IsEmptyKohi1)
        {
            replaceModel.Kohi1FutanshaNo = insuranceModel.Kohi1.FutansyaNo;
            replaceModel.Kohi1JukyuushaNo = insuranceModel.Kohi1.JyukyusyaNo;
            replaceModel.Kohi1KoufuNo = insuranceModel.Kohi1.TokusyuNo;
            replaceModel.Kohi1PeriodWestS = CIUtil.SDateToShowSDate(insuranceModel.Kohi1.StartDate);
            replaceModel.Kohi1PeriodWestKS = CIUtil.SDateToShowSWDate(insuranceModel.Kohi1.StartDate, 0, 0, 1);
            replaceModel.Kohi1PeriodJapanS = CIUtil.SDateToShowWDate(insuranceModel.Kohi1.StartDate);
            replaceModel.Kohi1PeriodJapanKS = CIUtil.SDateToShowWDate2(insuranceModel.Kohi1.StartDate);
            replaceModel.Kohi1PeriodWestE = CIUtil.SDateToShowSDate(insuranceModel.Kohi1.EndDate);
            replaceModel.Kohi1PeriodWestKE = CIUtil.SDateToShowSWDate(insuranceModel.Kohi1.EndDate, 0, 0, 1);
            replaceModel.Kohi1PeriodJapanE = CIUtil.SDateToShowWDate(insuranceModel.Kohi1.EndDate);
            replaceModel.Kohi1PeriodJapanKE = CIUtil.SDateToShowWDate2(insuranceModel.Kohi1.EndDate);
            replaceModel.Kohi1ShutokuWest = CIUtil.SDateToShowSWDate(insuranceModel.Kohi1.SikakuDate, 0, 0, 1);
            replaceModel.Kohi1ShutokuWestK = CIUtil.SDateToShowSDate(insuranceModel.Kohi1.SikakuDate);
            replaceModel.Kohi1ShutokuJapan = CIUtil.SDateToShowWDate(insuranceModel.Kohi1.SikakuDate);
            replaceModel.Kohi1ShutokuJapanK = CIUtil.SDateToShowWDate2(insuranceModel.Kohi1.SikakuDate);
            replaceModel.Kohi1KoufuWest = CIUtil.SDateToShowSDate(insuranceModel.Kohi1.KofuDate);
            replaceModel.Kohi1KoufuWestK = CIUtil.SDateToShowSWDate(insuranceModel.Kohi1.KofuDate, 0, 0, 1);
            replaceModel.Kohi1KoufuJapan = CIUtil.SDateToShowWDate(insuranceModel.Kohi1.KofuDate);
            replaceModel.Kohi1KoufuJapanK = CIUtil.SDateToShowWDate2(insuranceModel.Kohi1.KofuDate);
        }

        if (insuranceModel != null && !insuranceModel.IsEmptyKohi2)
        {
            replaceModel.Kohi2FutanshaNo = insuranceModel.Kohi2.FutansyaNo;
            replaceModel.Kohi2JukyuushaNo = insuranceModel.Kohi2.JyukyusyaNo;
            replaceModel.Kohi2KoufuNo = insuranceModel.Kohi2.TokusyuNo;
            replaceModel.Kohi2PeriodWestS = CIUtil.SDateToShowSDate(insuranceModel.Kohi2.StartDate);
            replaceModel.Kohi2PeriodWestKS = CIUtil.SDateToShowSWDate(insuranceModel.Kohi2.StartDate, 0, 0, 1);
            replaceModel.Kohi2PeriodJapanS = CIUtil.SDateToShowWDate(insuranceModel.Kohi2.StartDate);
            replaceModel.Kohi2PeriodJapanKS = CIUtil.SDateToShowWDate2(insuranceModel.Kohi2.StartDate);
            replaceModel.Kohi2PeriodWestE = CIUtil.SDateToShowSDate(insuranceModel.Kohi2.EndDate);
            replaceModel.Kohi2PeriodWestKE = CIUtil.SDateToShowSWDate(insuranceModel.Kohi2.EndDate, 0, 0, 1);
            replaceModel.Kohi2PeriodJapanE = CIUtil.SDateToShowWDate(insuranceModel.Kohi2.EndDate);
            replaceModel.Kohi2PeriodJapanKE = CIUtil.SDateToShowWDate2(insuranceModel.Kohi2.EndDate);
            replaceModel.Kohi2ShutokuWest = CIUtil.SDateToShowSDate(insuranceModel.Kohi2.SikakuDate);
            replaceModel.Kohi2ShutokuWestK = CIUtil.SDateToShowSWDate(insuranceModel.Kohi2.SikakuDate, 0, 0, 1);
            replaceModel.Kohi2ShutokuJapan = CIUtil.SDateToShowWDate(insuranceModel.Kohi2.SikakuDate);
            replaceModel.Kohi2ShutokuJapanK = CIUtil.SDateToShowWDate2(insuranceModel.Kohi2.SikakuDate);
            replaceModel.Kohi2KoufuWest = CIUtil.SDateToShowSDate(insuranceModel.Kohi2.KofuDate);
            replaceModel.Kohi2KoufuWestK = CIUtil.SDateToShowSWDate(insuranceModel.Kohi2.KofuDate, 0, 0, 1);
            replaceModel.Kohi2KoufuJapan = CIUtil.SDateToShowWDate(insuranceModel.Kohi2.KofuDate);
            replaceModel.Kohi2KoufuJapanK = CIUtil.SDateToShowWDate2(insuranceModel.Kohi2.KofuDate);
        }

        if (insuranceModel != null && !insuranceModel.IsEmptyKohi3)
        {
            replaceModel.Kohi3FutanshaNo = insuranceModel.Kohi3.FutansyaNo;
            replaceModel.Kohi3JukyuushaNo = insuranceModel.Kohi3.JyukyusyaNo;
            replaceModel.Kohi3KoufuNo = insuranceModel.Kohi3.TokusyuNo;
            replaceModel.Kohi3PeriodWestS = CIUtil.SDateToShowSDate(insuranceModel.Kohi3.StartDate);
            replaceModel.Kohi3PeriodWestKS = CIUtil.SDateToShowSWDate(insuranceModel.Kohi3.StartDate, 0, 0, 1);
            replaceModel.Kohi3PeriodJapanS = CIUtil.SDateToShowWDate(insuranceModel.Kohi3.StartDate);
            replaceModel.Kohi3PeriodJapanKS = CIUtil.SDateToShowWDate2(insuranceModel.Kohi3.StartDate);
            replaceModel.Kohi3PeriodWestE = CIUtil.SDateToShowSDate(insuranceModel.Kohi3.EndDate);
            replaceModel.Kohi3PeriodWestKE = CIUtil.SDateToShowSWDate(insuranceModel.Kohi3.EndDate, 0, 0, 1);
            replaceModel.Kohi3PeriodJapanE = CIUtil.SDateToShowWDate(insuranceModel.Kohi3.EndDate);
            replaceModel.Kohi3PeriodJapanKE = CIUtil.SDateToShowWDate2(insuranceModel.Kohi3.EndDate);
            replaceModel.Kohi3ShutokuWest = CIUtil.SDateToShowSDate(insuranceModel.Kohi3.SikakuDate);
            replaceModel.Kohi3ShutokuWestK = CIUtil.SDateToShowSWDate(insuranceModel.Kohi3.SikakuDate, 0, 0, 1);
            replaceModel.Kohi3ShutokuJapan = CIUtil.SDateToShowWDate(insuranceModel.Kohi3.SikakuDate);
            replaceModel.Kohi3ShutokuJapanK = CIUtil.SDateToShowWDate2(insuranceModel.Kohi3.SikakuDate);
            replaceModel.Kohi3KoufuWest = CIUtil.SDateToShowSDate(insuranceModel.Kohi3.KofuDate);
            replaceModel.Kohi3KoufuWestK = CIUtil.SDateToShowSWDate(insuranceModel.Kohi3.KofuDate, 0, 0, 1);
            replaceModel.Kohi3KoufuJapan = CIUtil.SDateToShowWDate(insuranceModel.Kohi3.KofuDate);
            replaceModel.Kohi3KoufuJapanK = CIUtil.SDateToShowWDate2(insuranceModel.Kohi3.KofuDate);
        }

        if (insuranceModel != null && !insuranceModel.IsEmptyKohi4)
        {
            replaceModel.Kohi4FutanshaNo = insuranceModel.Kohi4.FutansyaNo;
            replaceModel.Kohi4JukyuushaNo = insuranceModel.Kohi4.JyukyusyaNo;
            replaceModel.Kohi4KoufuNo = insuranceModel.Kohi4.TokusyuNo;
            replaceModel.Kohi4PeriodWestS = CIUtil.SDateToShowSDate(insuranceModel.Kohi4.StartDate);
            replaceModel.Kohi4PeriodWestKS = CIUtil.SDateToShowSWDate(insuranceModel.Kohi4.StartDate, 0, 0, 1);
            replaceModel.Kohi4PeriodJapanS = CIUtil.SDateToShowWDate(insuranceModel.Kohi4.StartDate);
            replaceModel.Kohi4PeriodJapanKS = CIUtil.SDateToShowWDate2(insuranceModel.Kohi4.StartDate);
            replaceModel.Kohi4PeriodWestE = CIUtil.SDateToShowSDate(insuranceModel.Kohi4.EndDate);
            replaceModel.Kohi4PeriodWestKE = CIUtil.SDateToShowSWDate(insuranceModel.Kohi4.EndDate, 0, 0, 1);
            replaceModel.Kohi4PeriodJapanE = CIUtil.SDateToShowWDate(insuranceModel.Kohi4.EndDate);
            replaceModel.Kohi4PeriodJapanKE = CIUtil.SDateToShowWDate2(insuranceModel.Kohi4.EndDate);
            replaceModel.Kohi4ShutokuWest = CIUtil.SDateToShowSDate(insuranceModel.Kohi4.SikakuDate);
            replaceModel.Kohi4ShutokuWestK = CIUtil.SDateToShowSWDate(insuranceModel.Kohi4.SikakuDate, 0, 0, 1);
            replaceModel.Kohi4ShutokuJapan = CIUtil.SDateToShowWDate(insuranceModel.Kohi4.SikakuDate);
            replaceModel.Kohi4ShutokuJapanK = CIUtil.SDateToShowWDate2(insuranceModel.Kohi4.SikakuDate);
            replaceModel.Kohi4KoufuWest = CIUtil.SDateToShowSDate(insuranceModel.Kohi4.KofuDate);
            replaceModel.Kohi4KoufuWestK = CIUtil.SDateToShowSWDate(insuranceModel.Kohi4.KofuDate, 0, 0, 1);
            replaceModel.Kohi4KoufuJapan = CIUtil.SDateToShowWDate(insuranceModel.Kohi4.KofuDate);
            replaceModel.Kohi4KoufuJapanK = CIUtil.SDateToShowWDate2(insuranceModel.Kohi4.KofuDate);
        }
        #endregion
        return replaceModel;
    }

    private List<ItemDisplayParamModel> ReplaceParamHokenAction(HokenParamReplaceModel model)
    {
        List<ItemDisplayParamModel> listParam = new();

        // Hoken Inf
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/保険者番号", model.HokenNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/被保険者証記号", model.HokenKigo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/被保険者証番号", model.HokenBango));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/被保険者証枝番", model.EdaNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/本人家族", model.HokenHonki));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/高額区分", model.HokenKogakuKbn));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/有効期限S(西暦/)", model.HokenPeriodWestS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/有効期限S(西暦K)", model.HokenPeriodWestKS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/有効期限S(和暦/)", model.HokenPeriodJapanS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/有効期限S(和暦K)", model.HokenPeriodJapanKS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/有効期限E(西暦/)", model.HokenPeriodWestE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/有効期限E(西暦K)", model.HokenPeriodWestKE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/有効期限E(和暦/)", model.HokenPeriodJapanE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/有効期限E(和暦K)", model.HokenPeriodJapanKE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/国保減免", model.HokenKokuhoGenmen));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/職務上区分", model.HokenSenin));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/特記事項１", model.HokenSpecialNote1));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/特記事項２", model.HokenSpecialNote2));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/特記事項３", model.HokenSpecialNote3));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/特記事項４", model.HokenSpecialNote4));
        listParam.Add(new ItemDisplayParamModel("保険情報", "保険/特記事項５", model.HokenSpecialNote5));

        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/負担者番号", model.Kohi1FutanshaNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/受給者番号", model.Kohi1JukyuushaNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/交付番号", model.Kohi1KoufuNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/有効期限S(西暦/)", model.Kohi1PeriodWestS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/有効期限S(西暦K)", model.Kohi1PeriodWestKS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/有効期限S(和暦/)", model.Kohi1PeriodJapanS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/有効期限S(和暦K)", model.Kohi1PeriodJapanKS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/有効期限E(西暦/)", model.Kohi1PeriodWestE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/有効期限E(西暦K)", model.Kohi1PeriodWestKE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/有効期限E(和暦/)", model.Kohi1PeriodJapanE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/有効期限E(和暦K)", model.Kohi1PeriodJapanKE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/取得日(西暦/)", model.Kohi1ShutokuWest));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/取得日(西暦K)", model.Kohi1ShutokuWestK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/取得日(和暦/)", model.Kohi1ShutokuJapan));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/取得日(和暦K)", model.Kohi1ShutokuJapanK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/交付日(西暦/)", model.Kohi1KoufuWest));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/交付日(西暦K)", model.Kohi1KoufuWestK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/交付日(和暦/)", model.Kohi1KoufuJapan));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公１/交付日(和暦K)", model.Kohi1KoufuJapanK));

        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/負担者番号", model.Kohi2FutanshaNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/受給者番号", model.Kohi2JukyuushaNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/交付番号", model.Kohi2KoufuNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/有効期限S(西暦/)", model.Kohi2PeriodWestS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/有効期限S(西暦K)", model.Kohi2PeriodWestKS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/有効期限S(和暦/)", model.Kohi2PeriodJapanS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/有効期限S(和暦K)", model.Kohi2PeriodJapanKS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/有効期限E(西暦/)", model.Kohi2PeriodWestE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/有効期限E(西暦K)", model.Kohi2PeriodWestKE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/有効期限E(和暦/)", model.Kohi2PeriodJapanE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/有効期限E(和暦K)", model.Kohi2PeriodJapanKE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/取得日(西暦/)", model.Kohi2ShutokuWest));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/取得日(西暦K)", model.Kohi2ShutokuWestK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/取得日(和暦/)", model.Kohi2ShutokuJapan));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/取得日(和暦K)", model.Kohi2ShutokuJapanK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/交付日(西暦/)", model.Kohi2KoufuWest));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/交付日(西暦K)", model.Kohi2KoufuWestK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/交付日(和暦/)", model.Kohi2KoufuJapan));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公２/交付日(和暦K)", model.Kohi2KoufuJapanK));

        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/負担者番号", model.Kohi3FutanshaNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/受給者番号", model.Kohi3JukyuushaNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/交付番号", model.Kohi3KoufuNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/有効期限S(西暦/)", model.Kohi3PeriodWestS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/有効期限S(西暦K)", model.Kohi3PeriodWestKS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/有効期限S(和暦/)", model.Kohi3PeriodJapanS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/有効期限S(和暦K)", model.Kohi3PeriodJapanKS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/有効期限E(西暦/)", model.Kohi3PeriodWestE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/有効期限E(西暦K)", model.Kohi3PeriodWestKE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/有効期限E(和暦/)", model.Kohi3PeriodJapanE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/有効期限E(和暦K)", model.Kohi3PeriodJapanKE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/取得日(西暦/)", model.Kohi3ShutokuWest));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/取得日(西暦K)", model.Kohi3ShutokuWestK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/取得日(和暦/)", model.Kohi3ShutokuJapan));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/取得日(和暦K)", model.Kohi3ShutokuJapanK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/交付日(西暦/)", model.Kohi3KoufuWest));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/交付日(西暦K)", model.Kohi3KoufuWestK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/交付日(和暦/)", model.Kohi3KoufuJapan));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公３/交付日(和暦K)", model.Kohi3KoufuJapanK));

        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/負担者番号", model.Kohi4FutanshaNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/受給者番号", model.Kohi4JukyuushaNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/交付番号", model.Kohi4KoufuNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/有効期限S(西暦/)", model.Kohi4PeriodWestS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/有効期限S(西暦K)", model.Kohi4PeriodWestKS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/有効期限S(和暦/)", model.Kohi4PeriodJapanS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/有効期限S(和暦K)", model.Kohi4PeriodJapanKS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/有効期限E(西暦/)", model.Kohi4PeriodWestE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/有効期限E(西暦K)", model.Kohi4PeriodWestKE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/有効期限E(和暦/)", model.Kohi4PeriodJapanE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/有効期限E(和暦K)", model.Kohi4PeriodJapanKE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/取得日(西暦/)", model.Kohi4ShutokuWest));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/取得日(西暦K)", model.Kohi4ShutokuWestK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/取得日(和暦/)", model.Kohi4ShutokuJapan));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/取得日(和暦K)", model.Kohi4ShutokuJapanK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/交付日(西暦/)", model.Kohi4KoufuWest));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/交付日(西暦K)", model.Kohi4KoufuWestK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/交付日(和暦/)", model.Kohi4KoufuJapan));
        listParam.Add(new ItemDisplayParamModel("保険情報", "公４/交付日(和暦K)", model.Kohi4KoufuJapanK));

        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/災害区分", model.RousaiSaigaiKbn));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/労働保険番号", model.RousaiRoudouHokenNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/年金証書番号", model.RousaiNenkinNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/健康管理手帳番号", model.RousaiRoudouKyoku));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/労働局", model.RousaiKantoku));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/健康管理手帳番号", model.RousaiKenkoKanriNo));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/傷病コード", model.RousaiShyobyoCode));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/事業名称", model.RousaiJigyouName));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/事業所住所", model.RousaiJigyuoAddress));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/傷病年月日(西暦/)", model.RousaiShyobyoDateWest));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/傷病年月日(西暦K)", model.RousaiShyobyoDateWestK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/傷病年月日(和暦/)", model.RousaiShyobyoDateJapan));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/傷病年月日(和暦K)", model.RousaiShyobyoDateJapanK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/療養期間S(西暦/)", model.RousaiRyouyouPeriodWestS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/療養期間S(西暦K)", model.RousaiRyouyouPeriodWestKS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/療養期間S(和暦/)", model.RousaiRyouyouPeriodJapanS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/療養期間S(和暦K)", model.RousaiRyouyouPeriodJapanKS));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/療養期間E(西暦/)", model.RousaiRyouyouPeriodWestE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/療養期間E(西暦K)", model.RousaiRyouyouPeriodWestKE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/療養期間E(和暦/)", model.RousaiRyouyouPeriodJapanE));
        listParam.Add(new ItemDisplayParamModel("保険情報", "労災/療養期間E(和暦K)", model.RousaiRyouyouPeriodJapanKE));

        listParam.Add(new ItemDisplayParamModel("保険情報", "自賠/保険会社名", model.JibaiHokenCompanyName));
        listParam.Add(new ItemDisplayParamModel("保険情報", "自賠/保険担当者", model.JibaiHokenTanto));
        listParam.Add(new ItemDisplayParamModel("保険情報", "自賠/保険連絡先", model.JibaiHokenContact));
        listParam.Add(new ItemDisplayParamModel("保険情報", "自賠/受傷日(西暦/)", model.JibaiJushouDateWest));
        listParam.Add(new ItemDisplayParamModel("保険情報", "自賠/受傷日(西暦K)", model.JibaiJushouDateWestK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "自賠/受傷日(和暦/)", model.JibaiJushouDateJapan));
        listParam.Add(new ItemDisplayParamModel("保険情報", "自賠/受傷日(和暦K)", model.JibaiJushouDateJapanK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "自賠/初診日(西暦/)", model.JibaiSinDateWest));
        listParam.Add(new ItemDisplayParamModel("保険情報", "自賠/初診日(西暦K)", model.JibaiSinDateWestK));
        listParam.Add(new ItemDisplayParamModel("保険情報", "自賠/初診日(和暦/)", model.JibaiSinDateJapan));
        listParam.Add(new ItemDisplayParamModel("保険情報", "自賠/初診日(和暦K)", model.JibaiSinDateJapanK));

        return listParam;
    }
}
