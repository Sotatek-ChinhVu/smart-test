using CalculateService.Constants;
using CalculateService.Futan.Models;
using CalculateService.Futan.ViewModels;
using CalculateService.Interface;
using Entity.Tenant;
using Helper.Constants;
using Moq;

namespace CalculateUnitTest
{
    public class FutancalcUT : BaseUT
    {
        private FutancalcViewModel newFutanCalcVM()
        {
            var mockSystemConfigProvider = new Mock<ISystemConfigProvider>();
            mockSystemConfigProvider.Setup(repo => repo.GetChokiFutan()).Returns(0);
            mockSystemConfigProvider.Setup(repo => repo.GetChokiDateRange()).Returns(0);
            mockSystemConfigProvider.Setup(repo => repo.GetRoundKogakuPtFutan()).Returns(0);

            var mockLogger = new Mock<IEmrLogger>();

            FutancalcViewModel futanCalcVM = new FutancalcViewModel(TenantProvider, mockSystemConfigProvider.Object, mockLogger.Object);
            return futanCalcVM;
        }

        private long seqRaiinNo;
        private long newRaiinNo(FutancalcViewModel futanVm)
        {

            if (futanVm.KaikeiDetails.Count == 0)
            {
                seqRaiinNo = 0;
            }
            seqRaiinNo++;

            return seqRaiinNo;
        }

        private void newRaiinTensu(FutancalcViewModel futanVm,
            int sinDate, bool newRaiin = true, string sinStartTime = "", int hokenPid = 1, int tensu = 0,
            bool jituNisu = true, int syoSaisin = -1)
        {
            addCalcResult(futanVm);

            futanVm.RaiinTensu = new RaiinTensuModel()
            {
                HpId = 1,
                PtId = 1,
                SinDate = sinDate,
                RaiinNo = newRaiin ? newRaiinNo(futanVm) : seqRaiinNo,
                OyaRaiinNo =
                    new int[] { SyosaiConst.Syosin2, SyosaiConst.Saisin2, SyosaiConst.SaisinDenwa2 }.Contains(syoSaisin) ?
                    futanVm.KaikeiDetails.Find(k => k.SinDate == sinDate)?.OyaRaiinNo ?? seqRaiinNo :
                    seqRaiinNo,
                SinStartTime = sinStartTime == string.Empty ? "120000" : sinStartTime,
                HokenPid = hokenPid,
                Tensu = tensu,
                JituNisu = jituNisu
            };

            #region 保険組合せの設定
            //保険組合せ
            var hokenPattern = hokenPatterns.Find(x => x.HokenPid == futanVm.RaiinTensu.HokenPid);
            if (hokenPattern != null) futanVm.HokenPattern = hokenPattern;
            //主保険
            var ptHoken = ptHokens.Find(x => x.HokenId == futanVm.HokenPattern.HokenId);
            if (ptHoken != null) futanVm.PtHoken = ptHoken;
            //公費
            futanVm.PtKohis.Clear();
            for (int i = 1; i <= 4; i++)
            {
                int kohiId =
                    (i == 1) ? futanVm.HokenPattern.Kohi1Id :
                    (i == 2) ? futanVm.HokenPattern.Kohi2Id :
                    (i == 3) ? futanVm.HokenPattern.Kohi3Id :
                    futanVm.HokenPattern.Kohi4Id;

                if (kohiId == 0 || ptKohis.Find(x => x.HokenId == kohiId) == null)
                {
                    break;
                }

                var ptKohi = ptKohis.Find(x => x.HokenId == kohiId);
                if (ptKohi != null) futanVm.PtKohis.Add(ptKohi);
            }
            #endregion

            #region 初再診の設定
            if (syoSaisin >= 0)
            {
                futanVm.OdrInfs.Add(
                    new OdrInfModel(
                        new OdrInf()
                        {
                            SinDate = sinDate,
                            RaiinNo = futanVm.RaiinTensu.RaiinNo,
                            HokenPid = hokenPid
                        },
                        new OdrInfDetail()
                        {
                            ItemCd = ItemCdConst.SyosaiKihon,
                            Suryo = syoSaisin
                        },
                        futanVm.HokenPattern.PtHokenPattern
                    )
                );
                futanVm.RaiinTensu.SyosaisinKbn = syoSaisin;
            }
            #endregion
        }

        private void newPtInf(FutancalcViewModel futanVm,
            int sinDate, int birthDay)
        {
            futanVm.PtInf = new PtInfModel(
                new PtInf()
                {
                    PtId = 1,
                    Birthday = birthDay
                },
                sinDate
            );

            #region 変数初期化
            ptHokens.Clear();
            ptKohis.Clear();
            hokenPatterns.Clear();

            futanVm.KaikeiDetails.Clear();
            futanVm.KaikeiAdjustDetails.Clear();
            futanVm.LimitListOthers.Clear();
            futanVm.LimitCntListOthers.Clear();
            futanVm.OdrInfs.Clear();
            #endregion
        }

        private List<PtHokenInfModel> ptHokens = new List<PtHokenInfModel>();
        private void newPtHoken(int prefNo, int honkeKbn, string houbetu, int kogakuKbn,
            string hokensyaNo = "", int tokureiYm1 = 0, int tokureiYm2 = 0,
            int genmenKbn = 0, int genmenGaku = 0, int genmenRate = 0)
        {
            ptHokens.Add(
                new PtHokenInfModel(
                    new PtHokenInf()
                    {
                        PtId = 1,
                        HokenId = ptHokens.Count + 1,
                        HokenNo =
                            houbetu == "0" ? 100 :
                            houbetu == "100" ? 60 :
                            houbetu == "39" ? 39 :
                            houbetu == "67" ? 67 :
                            houbetu == "103" ? 103 :
                            houbetu == "104" ? 104 :
                            1,
                        HokenEdaNo = 0,
                        HokenKbn =
                            houbetu == "0" ? HokenKbn.Jihi :
                            houbetu == "100" ? HokenKbn.Kokho :
                            houbetu == "39" ? HokenKbn.Kokho :
                            houbetu == "67" ? HokenKbn.Kokho :
                            houbetu == "103" ? HokenKbn.RousaiShort :
                            houbetu == "104" ? HokenKbn.Jibai :
                            HokenKbn.Syaho,
                        HonkeKbn = honkeKbn,
                        Houbetu = houbetu,
                        HokensyaNo = hokensyaNo,
                        KogakuKbn = kogakuKbn,
                        TokureiYm1 = tokureiYm1,
                        TokureiYm2 = tokureiYm2,
                        GenmenKbn = genmenKbn,
                        GenmenGaku = genmenGaku,
                        GenmenRate = genmenRate
                    },
                    new HokenMst()
                    {
                        HokenSbtKbn =
                            houbetu == "0" ? HokenSbtKbn.None :
                            HokenSbtKbn.Hoken,
                        EnTen = 10,
                        FutanKbn = 1,
                        FutanRate =
                            houbetu == "0" ? 100 :
                            houbetu == "39" ? 10 :
                            30,
                    },
                    prefNo
                )
            );
        }

        private List<PtKohiModel> ptKohis = new List<PtKohiModel>();
        private void newPtKohi(int prefNo, string houbetu, int hokenSbtKbn, int futanKbn, int kogakuTekiyo = 11,
            int futanRate = 0, int countKbn = 0, int kaiLimitFutan = 0, int dayLimitFutan = 0, int dayLimitCount = 0, int monthLimitFutan = 0, int monthLimitCount = 0,
            int monthSpLimit = 0, int calcSpKbn = 0, int isLimitList = 0, int isLimitListSum = 0, bool exceptHokensya = false, int futanYusen = 0, int inpLimitFutan = 0,
            int limitKbn = 0)
        {
            #region priorityNo
            string priorityNo = "99999";
            switch (prefNo)
            {
                case 0:
                    switch (houbetu)
                    {
                        case "10": priorityNo = "00060"; break;
                        case "11": priorityNo = "00070"; break;
                        case "12": priorityNo = "00270"; break;
                        case "13": priorityNo = "00010"; break;
                        case "14": priorityNo = "00020"; break;
                        case "15": priorityNo = "00100"; break;
                        case "16": priorityNo = "00110"; break;
                        case "17": priorityNo = "00150"; break;
                        case "18": priorityNo = "00030"; break;
                        case "19": priorityNo = "00170"; break;
                        case "20": priorityNo = "00080"; break;
                        case "21": priorityNo = "00090"; break;
                        case "22": priorityNo = "00130"; break;
                        case "23": priorityNo = "00180"; break;
                        case "24": priorityNo = "00120"; break;
                        case "25": priorityNo = "00260"; break;
                        case "28": priorityNo = "00140"; break;
                        case "29": priorityNo = "00040"; break;
                        case "30": priorityNo = "00050"; break;
                        case "38": priorityNo = "00220"; break;
                        case "51": priorityNo = "00210"; break;
                        case "52": priorityNo = "00190"; break;
                        case "53": priorityNo = "00230"; break;
                        case "54": priorityNo = "00200"; break;
                        case "62": priorityNo = "00250"; break;
                        case "66": priorityNo = "00240"; break;
                        case "79": priorityNo = "00160"; break;
                        case "102": priorityNo = "00001"; break;
                    }
                    break;
                case PrefCode.Tochigi:
                    switch (houbetu)
                    {
                        case "90": priorityNo = "09200"; break;
                    }
                    break;
                case PrefCode.Tokyo:
                    switch (houbetu)
                    {
                        case "82": priorityNo = "13100"; break;
                    }
                    break;
                case PrefCode.Ishikawa:
                    switch (houbetu)
                    {
                        case "82": priorityNo = "17190"; break;
                    }
                    break;
                case PrefCode.Kagawa:
                    switch (houbetu)
                    {
                        case "91": priorityNo = "37210"; break;
                        case "94": priorityNo = "37200"; break;
                    }
                    break;
                case PrefCode.Nagasaki:
                    switch (houbetu)
                    {
                        case "86": priorityNo = "00251"; break;
                    }
                    break;
            }
            #endregion

            ptKohis.Add(
                new PtKohiModel(
                    new PtKohi()
                    {
                        HokenId = ptKohis.Count + 1,
                        PrefNo = prefNo,
                        GendoGaku = inpLimitFutan
                    },
                    new HokenMst()
                    {
                        Houbetu = houbetu,
                        HokenSbtKbn = hokenSbtKbn,
                        FutanKbn = futanKbn,
                        FutanRate = futanRate,
                        LimitKbn = limitKbn,
                        CountKbn = countKbn,
                        KaiLimitFutan = kaiLimitFutan,
                        DayLimitFutan = dayLimitFutan,
                        DayLimitCount = dayLimitCount,
                        MonthLimitFutan = monthLimitFutan,
                        MonthLimitCount = monthLimitCount,
                        MonthSpLimit = monthSpLimit,
                        CalcSpKbn = calcSpKbn,
                        IsLimitList = isLimitList,
                        IsLimitListSum = isLimitListSum,
                        KogakuTekiyo = kogakuTekiyo,
                        FutanYusen = futanYusen
                    },
                    exceptHokensya,
                    priorityNo
                )
            );
        }

        private void newPtKohi(int prefNo, string houbetu, int monthLimitFutan = 0)
        {
            int futanKbn = monthLimitFutan == 0 ? 0 : 1;

            switch (prefNo)
            {
                case 0:
                    switch (houbetu)
                    {
                        case "12":
                            newPtKohi
                            (
                                prefNo: prefNo,
                                houbetu: houbetu,
                                hokenSbtKbn: HokenSbtKbn.Seiho,
                                futanKbn: futanKbn,
                                monthLimitFutan: monthLimitFutan,
                                kogakuTekiyo: 22
                            );
                            break;
                        case "15":
                        case "21":
                            newPtKohi
                            (
                                prefNo: prefNo,
                                houbetu: houbetu,
                                hokenSbtKbn: HokenSbtKbn.Bunten,
                                futanKbn: futanKbn,
                                futanRate: futanKbn == 1 ? 10 : 0,
                                monthLimitFutan: monthLimitFutan,
                                isLimitList: 1,
                                kogakuTekiyo: 0
                            );
                            break;
                        case "28":
                            newPtKohi
                            (
                                prefNo: prefNo,
                                houbetu: houbetu,
                                hokenSbtKbn: HokenSbtKbn.Bunten,
                                futanKbn: futanKbn,
                                monthLimitFutan: monthLimitFutan,
                                kogakuTekiyo: futanKbn == 1 ? 11 : 0
                            );
                            break;
                        case "38":
                            newPtKohi
                            (
                                prefNo: prefNo,
                                houbetu: houbetu,
                                hokenSbtKbn: HokenSbtKbn.Bunten,
                                futanKbn: futanKbn,
                                monthLimitFutan: monthLimitFutan,
                                isLimitList: 1,
                                kogakuTekiyo: 0
                            );
                            break;
                        case "52":
                        case "54":
                            newPtKohi
                            (
                                prefNo: prefNo,
                                houbetu: houbetu,
                                hokenSbtKbn: HokenSbtKbn.Bunten,
                                futanKbn: futanKbn,
                                futanRate: futanKbn == 1 ? 20 : 0,
                                monthLimitFutan: monthLimitFutan,
                                isLimitList: 1,
                                isLimitListSum: 1,
                                kogakuTekiyo: 11
                            );
                            break;
                        case "102":
                            newPtKohi
                            (
                                prefNo: prefNo,
                                houbetu: houbetu,
                                hokenSbtKbn: HokenSbtKbn.Choki,
                                futanKbn: futanKbn,
                                monthLimitFutan: monthLimitFutan,
                                kogakuTekiyo: 0,
                                limitKbn: 1
                            );
                            break;
                        default:
                            throw new Exception("未対応の法別番号です。");
                    }
                    break;
                default:
                    throw new Exception("未対応の都道府県番号です。");
            }
        }

        List<PtHokenPatternModel> hokenPatterns = new List<PtHokenPatternModel>();
        private void newHokenPattern(int hokenId, int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id)
        {
            var ptHoken = ptHokens.Find(p => p.HokenId == hokenId);

            if (ptHoken == null) return;

            #region 'HokenSbtCd'
            int hokenSbtCd = 1;
            switch (ptHoken.Houbetu)
            {
                case "103":
                case "104":
                case "108":
                case "109": hokenSbtCd = 0; break;
                case "100": hokenSbtCd = 2; break;
                case "39": hokenSbtCd = 3; break;
                case "67": hokenSbtCd = 4; break;
                case "0": hokenSbtCd = 5; break;
            }

            if (hokenSbtCd >= 1)
            {
                int pairCnt = 0;
                if (hokenSbtCd != 5) pairCnt++;
                if (kohi1Id >= 1) pairCnt++;
                if (kohi2Id >= 1) pairCnt++;
                if (kohi3Id >= 1) pairCnt++;
                if (kohi4Id >= 1) pairCnt++;

                int heiyoCnt = pairCnt;
                if (ptKohis.Find(p => p.HokenId == kohi1Id)?.Houbetu == "102") heiyoCnt--;
                if (ptKohis.Find(p => p.HokenId == kohi2Id)?.Houbetu == "102") heiyoCnt--;
                if (ptKohis.Find(p => p.HokenId == kohi3Id)?.Houbetu == "102") heiyoCnt--;
                if (ptKohis.Find(p => p.HokenId == kohi4Id)?.Houbetu == "102") heiyoCnt--;

                hokenSbtCd = hokenSbtCd * 100 + pairCnt * 10 + heiyoCnt;
            }
            #endregion

            hokenPatterns.Add(
                new PtHokenPatternModel(
                    new PtHokenPattern()
                    {
                        HokenPid = hokenPatterns.Count + 1,
                        HokenKbn =
                            new string[] { "100", "67", "39" }.Contains(ptHoken.Houbetu) ? HokenKbn.Kokho :
                            new string[] { "108", "109" }.Contains(ptHoken.Houbetu) ? HokenKbn.Jihi :
                            HokenKbn.Syaho,
                        HokenSbtCd = hokenSbtCd,
                        HokenId = hokenId,
                        Kohi1Id = kohi1Id,
                        Kohi2Id = kohi2Id,
                        Kohi3Id = kohi3Id,
                        Kohi4Id = kohi4Id
                    }
                )
            );
        }

        private void newLimitListOther(FutancalcViewModel futanVm,
            int sinDate, int futanGaku, int hokenPid = 1, int kohiId = 1)
        {
            addCalcResult(futanVm);

            LimitListInfModel limitListInfModel =
                new LimitListInfModel(
                    new LimitListInf()
                    {
                        HpId = 1,
                        PtId = 1,
                        KohiId = kohiId,
                        SinDate = sinDate,
                        HokenPid = hokenPid,
                        SortKey = String.Format("{0:D8}{1}{2:D10}0", sinDate, "120000", 1),
                        RaiinNo = 0,
                        FutanGaku = futanGaku,
                        TotalGaku = 0
                    }
                );
            futanVm.LimitListOthers.Add(limitListInfModel);
        }

        private void newLimitCntListOther(FutancalcViewModel futanVm,
            int sinDate, int hokenPid = 1, int kohiId = 1)
        {
            addCalcResult(futanVm);

            LimitCntListInfModel limitCntListInfModel =
                new LimitCntListInfModel(
                    new LimitCntListInf()
                    {
                        HpId = 1,
                        PtId = 1,
                        KohiId = kohiId,
                        SinDate = sinDate,
                        HokenPid = hokenPid,
                        SortKey = String.Format("{0:D8}{1}{2:D10}0", sinDate, "120000", 1),
                        OyaRaiinNo = 0
                    }
                );
            futanVm.LimitCntListOthers.Add(limitCntListInfModel);
        }

        private void addCalcResult(FutancalcViewModel futanVm)
        {
            if (futanVm.KaikeiDetail.RaiinNo > 0)
            {
                futanVm.KaikeiDetails.Add(futanVm.KaikeiDetail);
                futanVm.KaikeiDetail = new KaikeiDetailModel(new KaikeiDetail());

                futanVm.LimitListOthers.AddRange(futanVm.LimitListInfs);
                futanVm.LimitCntListOthers.AddRange(futanVm.LimitCntListInfs);
                futanVm.KaikeiAdjustDetails.AddRange(futanVm.AdjustDetails);
            }
        }

        private void AssertEqualTo(KaikeiDetailModel kaikeiDetail,
            int tensu, int hokenFutan, int kogakuFutan,
            int kohi1Futan, int kohi2Futan, int kohi3Futan, int kohi4Futan,
            int ichibuFutan, int ptFutan, int genmenGaku = 0)
        {
            Assert.That(kaikeiDetail.Tensu, Is.EqualTo(tensu));
            Assert.That(kaikeiDetail.HokenFutan, Is.EqualTo(hokenFutan));
            Assert.That(kaikeiDetail.KogakuFutan, Is.EqualTo(kogakuFutan));
            Assert.That(kaikeiDetail.Kohi1Futan, Is.EqualTo(kohi1Futan));
            Assert.That(kaikeiDetail.Kohi2Futan, Is.EqualTo(kohi2Futan));
            Assert.That(kaikeiDetail.Kohi3Futan, Is.EqualTo(kohi3Futan));
            Assert.That(kaikeiDetail.Kohi4Futan, Is.EqualTo(kohi4Futan));
            Assert.That(kaikeiDetail.IchibuFutan, Is.EqualTo(ichibuFutan));
            Assert.That(kaikeiDetail.PtFutan, Is.EqualTo(ptFutan));
            Assert.That(kaikeiDetail.GenmenGaku, Is.EqualTo(genmenGaku));
        }

        private void AssertEqualTo(List<KaikeiDetailModel> adjustDetails,
            int hokenPid, int adjustPid, int kogakuFutan,
            int kohi1Futan, int kohi2Futan, int kohi3Futan, int kohi4Futan,
            int ichibuFutan, int ptFutan,
            int count = 1, int index = 0)
        {
            Assert.That(adjustDetails.Count, Is.EqualTo(count));
            Assert.That(adjustDetails[index].HokenPid, Is.EqualTo(hokenPid));
            Assert.That(adjustDetails[index].AdjustPid, Is.EqualTo(adjustPid));
            Assert.That(adjustDetails[index].KogakuFutan, Is.EqualTo(kogakuFutan));
            Assert.That(adjustDetails[index].Kohi1Futan, Is.EqualTo(kohi1Futan));
            Assert.That(adjustDetails[index].Kohi2Futan, Is.EqualTo(kohi2Futan));
            Assert.That(adjustDetails[index].Kohi3Futan, Is.EqualTo(kohi3Futan));
            Assert.That(adjustDetails[index].Kohi4Futan, Is.EqualTo(kohi4Futan));
            Assert.That(adjustDetails[index].IchibuFutan, Is.EqualTo(ichibuFutan));
            Assert.That(adjustDetails[index].PtFutan, Is.EqualTo(ptFutan));
        }

        private void AssertEqualTo(List<LimitListInfModel> limitListInfs,
            int futanGaku, int totalGaku, int count = 1, int index = 0)
        {
            Assert.That(limitListInfs.Count, Is.EqualTo(count));
            Assert.That(limitListInfs[index].FutanGaku, Is.EqualTo(futanGaku));
            Assert.That(limitListInfs[index].TotalGaku, Is.EqualTo(totalGaku));
        }

        //70歳未満一般
        [Test]
        public void T001_Under70Ippan()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newHokenPattern(1, 0, 0, 0, 0);

            //計算
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000);
            futanCalcVm.DetailCalculate(false);

            //結果
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 0, 0, 0, 0, 3000, 3000);
        }

        //6歳未満未就学児
        [Test]
        public void T002_Under6()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newHokenPattern(1, 0, 0, 0, 0);

            //計算
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000);
            futanCalcVm.DetailCalculate(false);

            //結果
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 0, 0, 0, 0, 2000, 2000);
        }

        //前期1割
        [Test]
        public void T003_Over70_10per()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newHokenPattern(1, 0, 0, 0, 0);

            //計算
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000);
            futanCalcVm.DetailCalculate(false);

            //結果
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);
        }

        //前期2割
        [Test]
        public void T004_Over70_20per()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19450101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newHokenPattern(1, 0, 0, 0, 0);

            //計算
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000);
            futanCalcVm.DetailCalculate(false);

            //結果
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 0, 0, 0, 0, 2000, 2000);
        }

        //社保+宮城83(0円)（特殊計算[1]: 認定証の提示がない場合、高額療養費の上限を超えた分を患者負担とする）
        [Test]
        public void T005_P04001_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Miyagi;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 0,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 0, 60000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 2430, 0, 0, 0, 17570, 17570);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 1000, 0, 0, 0, 19000, 19000);
        }

        //社保+宮城83(1回500月1回)（特殊計算[1]: 認定証の提示がない場合、高額療養費の上限を超えた分を患者負担とする）
        [Test]
        public void T005_P04002_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Miyagi;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 500,
                monthLimitCount: 1,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 0, 59500, 0, 0, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 2430, 0, 0, 0, 17570, 17570);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 1000, 0, 0, 0, 19000, 19000);
        }

        //社保+宮城83(1日500円)（特殊計算[2]: 1の処理 + 初診時500円再診時無料）
        [Test]
        public void T005_P04003_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Miyagi;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 500,
                monthLimitCount: 1,
                calcSpKbn: 2
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000, syoSaisin: SyosaiConst.Syosin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 0, 59500, 0, 0, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 10000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 2430, 0, 0, 0, 17570, 17570);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 10000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 1000, 0, 0, 0, 19000, 19000);
        }

        //社保+宮城83(1日500円)（特殊計算[2]: 1の処理 + 初診時500円再診時無料）
        // 同日初診は負担なし
        [Test]
        public void T005_P04004_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Miyagi;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 500,
                monthLimitCount: 1,
                calcSpKbn: 2
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000, syoSaisin: SyosaiConst.Syosin2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 0, 60000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 10000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 2430, 0, 0, 0, 17570, 17570);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 10000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 1000, 0, 0, 0, 19000, 19000);
        }

        //市町村国保+宮城83(0円)（特殊計算[1]: 認定証の提示がない場合、高額療養費の上限を超えた分を患者負担とする）
        // 市町村国保は特殊処理対象外
        [Test]
        public void T005_P04005_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Miyagi;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "040014",  //仙台市
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 0,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 0, 60000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
        }

        //市町村国保+宮城83(0円)（特殊計算[1]: 認定証の提示がない場合、高額療養費の上限を超えた分を患者負担とする）
        // 市町村国保(退職)は特殊処理対象外
        [Test]
        public void T005_P04006_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Miyagi;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "67",
                hokensyaNo: "67040014",  //仙台市
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 0,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 0, 60000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
        }

        //市町村国保+宮城83(0円)（特殊計算[1]: 認定証の提示がない場合、高額療養費の上限を超えた分を患者負担とする）
        // 国保組合(宮城県建設業国保・全国土木建築国保・宮城県医師国保・宮城県歯科医師国保を除く)は特殊処理対象
        [Test]
        public void T005_P04007_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Miyagi;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "043000",  //仙台市
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 0,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 0, 60000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 2430, 0, 0, 0, 17570, 17570);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 1000, 0, 0, 0, 19000, 19000);
        }

        //市町村国保+宮城83(0円)（特殊計算[1]: 認定証の提示がない場合、高額療養費の上限を超えた分を患者負担とする）
        // 国保組合でも例外保険者（宮城県医師国保など）は特殊処理対象外
        [Test]
        public void T005_P04008_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Miyagi;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "043026",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 0,
                calcSpKbn: 1,
                exceptHokensya: true
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 0, 60000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
        }

        //社保+宮城83(0円)（特殊計算[1]: 認定証の提示がない場合、高額療養費の上限を超えた分を患者負担とする）
        // 1日で超える場合、1円単位になる場合
        [Test]
        public void T005_P04009_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Miyagi;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "01",
                hokensyaNo: "0001",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 0,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 50526);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 50526, 404208, 0, 82483, 0, 0, 0, 18569, 18570);
        }

        //社保+秋田74（特殊計算: 半額助成&月1000円）
        [Test]
        public void T005_P05001_74()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Akita;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "74",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthLimitFutan: 1000,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 105);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 105, 735, 0, 157, 0, 0, 0, 158, 160);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 305);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 305, 2135, 0, 457, 0, 0, 0, 458, 460);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 305);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 305, 2135, 0, 531, 0, 0, 0, 384, 380);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 305);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 305, 2135, 0, 915, 0, 0, 0, 0, 0);
        }

        //社保+21(10% 2500円)+秋田74（特殊計算: 半額助成&月1000円）
        [Test]
        public void T005_P05002_2174()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Akita;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 2500
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "74",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthLimitFutan: 1000,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目 (21併用分)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, hokenPid: 1, tensu: 105);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 105, 735, 0, 210, 0, 0, 0, 105, 110);
            //1日目 (保険分)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, hokenPid: 2, tensu: 200);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1400, 0, 300, 0, 0, 0, 300, 300);
            //2日目 (保険分)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, hokenPid: 2, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 905, 0, 0, 0, 595, 590);
        }

        //社保+21(10% 2500円)+秋田74（特殊計算: 半額助成&月1000円）
        [Test]
        public void T005_P05003_2174()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Akita;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 2500
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "74",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthLimitFutan: 1000,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目 (21併用分)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, hokenPid: 1, tensu: 105);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 105, 735, 0, 210, 0, 0, 0, 105, 110);
            //1日目 (保険分)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, hokenPid: 2, tensu: 200);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1400, 0, 300, 0, 0, 0, 300, 300);
            //2日目 (21併用分)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, hokenPid: 1, tensu: 600);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 600, 4200, 0, 1200, 5, 0, 0, 595, 590);
        }

        //前期10%+21(10% 2500円)+秋田74（特殊計算: 半額助成&月1000円）
        [Test]
        public void T005_P05004_2174()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Akita;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 2500
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "74",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthLimitFutan: 1000,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目 (21併用分)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 105);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 105, 945, 0, 0, 52, 0, 0, 53, 50);
            //2日目 (21併用分)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 0, 0, 1053, 0, 0, 947, 950);
            //3日目 (21併用分)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 605, 395, 0, 0, 0, 0);
        }

        //茨城88（特殊計算: 実日数をカウントしない日は負担なし、回数カウントもしない）
        [Test]
        public void T005_P08001_88()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Ibaraki;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210401,
                birthDay: 20180101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "88",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                countKbn: 3,
                dayLimitFutan: 600,
                monthLimitCount: 2,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210401, tensu: 1000, jituNisu: true);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1400, 0, 0, 0, 600, 600);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210402, tensu: 1000, jituNisu: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 2000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210403, tensu: 1000, jituNisu: true);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1400, 0, 0, 0, 600, 600);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210404, tensu: 1000, jituNisu: true);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 2000, 0, 0, 0, 0, 0);
        }

        //社保+宮城83(1日500円)（特殊計算[2]: 1の処理 + 初診時500円再診時無料）
        [Test]
        public void T005_P08002_52_88()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Ibaraki;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210401,
                birthDay: 20180101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "88",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                countKbn: 3,
                dayLimitFutan: 600,
                monthLimitCount: 2,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210401, hokenPid: 2, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1400, 0, 0, 0, 600, 600);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210402, hokenPid: 1, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 2000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210403, hokenPid: 2, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1400, 0, 0, 0, 600, 600);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210404, hokenPid: 2, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 2000, 0, 0, 0, 0, 0);
        }

        //茨城88（特殊計算: 実日数をカウントしない日は負担なし、回数カウントもしない）
        [Test]
        public void T005_P08003_52_88()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Ibaraki;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210401,
                birthDay: 20180101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 2500
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "88",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                countKbn: 3,
                dayLimitFutan: 600,
                monthLimitCount: 2,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210401, hokenPid: 1, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 16000, 0, 1500, 1900, 0, 0, 600, 600);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210402, hokenPid: 1, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 2000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210403, hokenPid: 2, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1400, 0, 0, 0, 600, 600);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210404, hokenPid: 2, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 2000, 0, 0, 0, 0, 0);
        }

        //社保+埼玉81（特殊計算: 21,000円まで無料、超えたら全額償還）
        [Test]
        public void T005_P11001_81()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Saitama;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "81",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 0,
                monthSpLimit: 21000,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 5000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 15000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 3000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 0, -18000, 0, 0, 0, 24000, 24000);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 0, 0, 0, 0, 3000, 3000);
        }

        //社保+埼玉81（特殊計算: 1,000円まで負担、1,000円を超えて21,000円まで無料、21,000円を超えたら全額償還）
        [Test]
        public void T005_P11002_81()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Saitama;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "81",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthLimitFutan: 1000,
                monthSpLimit: 21000,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 5000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 14000, 0, 0, 0, 1000, 1000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 3000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 0, -17000, 0, 0, 0, 23000, 23000);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 0, 0, 0, 0, 3000, 3000);
        }

        //社保+38(10,000円)+埼玉81（特殊計算: 21,000円まで無料、超えたら全額償還）
        [Test]
        public void T005_P11003_3881()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Saitama;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "38",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "81",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 0,
                monthSpLimit: 21000,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, hokenPid: 1, tensu: 8000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 8000, 56000, 0, 14000, 10000, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, hokenPid: 2, tensu: 3000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 21000, 0, 9000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, hokenPid: 2, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 0, -19000, 0, 0, 0, 25000, 25000);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, hokenPid: 2, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 0, 0, 0, 0, 3000, 3000);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, hokenPid: 1, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 3000, 0, 0, 0, 0, 0);
        }

        //国保+千葉85(0円)（特殊計算[2]: 認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする）
        //認定証提示なし・70歳未満・県外国保組合（対象）
        [Test]
        public void T005_P12001_85()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "85",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 0,
                kogakuTekiyo: 1,
                futanYusen: 1,
                calcSpKbn: 2
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 0, 60000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 2430, 0, 0, 0, 17570, 17570);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 1000, 0, 0, 0, 19000, 19000);
        }

        //国保+千葉85(0円)（特殊計算[2]: 認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする）
        //認定証提示なし・70歳未満・県内国保組合（対象外）
        [Test]
        public void T005_P12002_85()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "123000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "85",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 0,
                kogakuTekiyo: 1,
                futanYusen: 1,
                calcSpKbn: 2
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 0, 60000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
        }

        //国保+千葉85(0円)（特殊計算[2]: 認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする）
        //認定証提示なし・70歳以上・県外国保組合（対象外）
        [Test]
        public void T005_P12003_85()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19450101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "85",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 0,
                kogakuTekiyo: 1,
                futanYusen: 1,
                calcSpKbn: 2
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 42000, 18000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 20000, 0, 0, 0, 0, 0, 0);
        }

        //国保+千葉85(0円)（特殊計算[2]: 認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする）
        //認定証提示あり・70歳未満・県外国保組合（対象外）
        [Test]
        public void T005_P12004_85()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 28
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "85",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 0,
                kogakuTekiyo: 1,
                futanYusen: 1,
                calcSpKbn: 2
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 0, 60000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 17570, 2430, 0, 0, 0, 0, 0);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 19000, 1000, 0, 0, 0, 0, 0);
        }

        //国保+千葉85(200円)（特殊計算[2]: 認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする）
        //認定証提示あり・70歳未満・県外国保組合（対象外）
        //高額療養費の限度額に達した日以降も窓口負担を徴収する（上限変動）
        [Test]
        public void T005_P12005_85()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 28
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "85",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitCount: 1,
                dayLimitFutan: 200,
                kogakuTekiyo: 1,
                futanYusen: 1,
                calcSpKbn: 2
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 0, 59800, 0, 0, 0, 200, 200);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 19800, 0, 0, 0, 200, 200);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 17570, 2230, 0, 0, 0, 200, 200);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 19000, 800, 0, 0, 0, 200, 200);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 1900, -100, 0, 0, 0, 200, 200);
        }

        //国保+千葉85(200円)（特殊計算[2]: 認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする）
        //認定証提示あり・70歳未満・県外国保組合（対象外）
        //高額療養費の限度額に達した日以降も窓口負担を徴収する（上限固定）
        [Test]
        public void T005_P12006_85()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "85",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitCount: 1,
                dayLimitFutan: 200,
                kogakuTekiyo: 1,
                futanYusen: 1,
                calcSpKbn: 2
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 19800, 0, 0, 0, 200, 200);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 4600, 15200, 0, 0, 0, 200, 200);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 20000, -200, 0, 0, 0, 200, 200);
        }

        //国保+千葉85(200円)（特殊計算[2]: 認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする）
        //認定証提示なし・70歳未満・県外国保組合（対象）
        //高額療養費の限度額に達した日以降も窓口負担を徴収する（上限変動）
        [Test]
        public void T005_P12007_85()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "85",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitCount: 1,
                dayLimitFutan: 200,
                kogakuTekiyo: 1,
                futanYusen: 1,
                calcSpKbn: 2
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 0, 59800, 0, 0, 0, 200, 200);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 19800, 0, 0, 0, 200, 200);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 2230, 0, 0, 0, 17770, 17770);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 800, 0, 0, 0, 19200, 19200);
        }

        //国保+千葉85(200円)（特殊計算[2]: 認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする）
        //他公費との併用事例１
        [Test]
        public void T005_P12008_85()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "85",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitCount: 1,
                dayLimitFutan: 300,
                kogakuTekiyo: 1,
                futanYusen: 1,
                calcSpKbn: 2
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1000, 700, 0, 0, 300, 300);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1000, 700, 0, 0, 300, 300);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1000, 700, 0, 0, 300, 300);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1000, 700, 0, 0, 300, 300);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1000, 700, 0, 0, 300, 300);
            //6日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181006, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 2000, 0, 0, 0, 0, 0);
            //7日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181006, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 2000, 0, 0, 0, 0, 0);
        }

        //国保+千葉85(200円)（特殊計算[2]: 認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする）
        //他公費との併用事例２
        [Test]
        public void T005_P12009_85()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20000101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "85",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitCount: 1,
                dayLimitFutan: 300,
                kogakuTekiyo: 1,
                futanYusen: 1,
                calcSpKbn: 2
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 975);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 975, 6825, 0, 975, 1650, 0, 0, 300, 300);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 975);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 975, 6825, 0, 975, 1650, 0, 0, 300, 300);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 975);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 975, 6825, 0, 975, 1650, 0, 0, 300, 300);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 975);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 975, 6825, 0, 975, 1650, 0, 0, 300, 300);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, tensu: 975);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 975, 6825, 0, 975, 1650, 0, 0, 300, 300);
            //6日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181006, tensu: 975);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 975, 6825, 0, 2675, 0, 0, 0, 250, 250);
            //7日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181007, tensu: 975);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 975, 6825, 0, 2925, 0, 0, 0, 0, 0);
        }

        //国保+千葉83(300円/日, 5回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  上限以下カウントあり
        //  認定証提示なし・70歳未満・県外国保組合（対象）
        [Test]
        public void T005_P12010_01_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 5,
                calcSpKbn: 3,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 100, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 100, 800, 0, 0, 0, 0, 0, 200, 200);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230804, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230805, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //6日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230806, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 400, 0, 0, 0, 0, 0);
        }

        //国保+千葉83(300円/日, 3回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  同日再診
        //  認定証提示なし・70歳未満・県外国保組合（対象）
        [Test]
        public void T005_P12010_02_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 3,
                calcSpKbn: 3,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 200, syoSaisin: SyosaiConst.Syosin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //2日目(同日再診)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 400, 0, 0, 0, 0, 0);
        }

        //国保+千葉83(300円/日, 3回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  ２科受診（初診２科目）
        //  認定証提示なし・70歳未満・県外国保組合（対象）
        [Test]
        public void T005_P12010_03_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 3,
                calcSpKbn: 3,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 100, syoSaisin: SyosaiConst.Syosin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 100, 800, 0, 0, 0, 0, 0, 200, 200);
            //1日目(初診２科目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 200, syoSaisin: SyosaiConst.Syosin2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 400, 0, 0, 0, 0, 0);
            //2日目(再診)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230804, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 400, 0, 0, 0, 0, 0);
        }

        //国保+千葉83(300円/日, 3回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  ２科受診（再診２科目）
        //  認定証提示なし・70歳未満・県外国保組合（対象）
        [Test]
        public void T005_P12010_04_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 3,
                calcSpKbn: 3,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 200, syoSaisin: SyosaiConst.Syosin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //2日目(再診)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 100, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 100, 800, 0, 0, 0, 0, 0, 200, 200);
            //2日目(再診２科目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 200, syoSaisin: SyosaiConst.Saisin2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 400, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230804, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 400, 0, 0, 0, 0, 0);
        }

        //国保+千葉83(300円/日, 3回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  ２科受診（電話再診２科目）
        //  認定証提示なし・70歳未満・県外国保組合（対象）
        [Test]
        public void T005_P12010_05_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 3,
                calcSpKbn: 3,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 200, syoSaisin: SyosaiConst.Syosin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //2日目(電話再診)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 100, syoSaisin: SyosaiConst.SaisinDenwa);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 100, 800, 0, 0, 0, 0, 0, 200, 200);
            //2日目(電話再診２科目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 200, syoSaisin: SyosaiConst.SaisinDenwa2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 400, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230804, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 400, 0, 0, 0, 0, 0);
        }

        //国保+千葉83(300円/日, 3回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  ２科受診（初診２科目） ※CALC_SP_KBN=0の確認
        //  認定証提示なし・70歳未満・県外国保組合（対象）
        [Test]
        public void T005_P12010_06_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 3,
                calcSpKbn: 0,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 100, syoSaisin: SyosaiConst.Syosin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 100, 800, 0, 0, 0, 0, 0, 200, 200);
            //1日目(初診２科目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 200, syoSaisin: SyosaiConst.Syosin2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 300, 0, 0, 0, 100, 100);
            //2日目(再診)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230804, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 400, 0, 0, 0, 0, 0);
        }

        //国保+千葉83(300円/日, 2回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  月上限に達した後に超過した場合
        //  認定証提示なし・70歳未満・県外国保組合（対象）
        [Test]
        public void T005_P12010_07_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 2,
                calcSpKbn: 3,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 9700, 0, 0, 0, 300, 300);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 9700, 0, 0, 0, 300, 300);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 10000, 0, 0, 0, 0, 0);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230804, tensu: 30000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 0, 51930, 0, 0, 0, 8070, 8070);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230805, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 500, 0, 0, 0, 9500, 9500);
        }

        //国保+千葉83(300円/日, 2回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  月上限に達した後に超過した場合
        //  認定証提示あり・70歳未満・県外国保組合（対象外）
        [Test]
        public void T005_P12010_08_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 28
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 2,
                calcSpKbn: 3,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 9700, 0, 0, 0, 300, 300);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 9700, 0, 0, 0, 300, 300);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 10000, 0, 0, 0, 0, 0);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230804, tensu: 30000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 8070, 51930, 0, 0, 0, 0, 0);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230805, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 9500, 500, 0, 0, 0, 0, 0);
        }

        //国保+千葉83(300円/日, 2回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  月上限に達した後に超過した場合
        //  認定証提示なし・70歳未満・県内国保組合（対象外）
        [Test]
        public void T005_P12010_09_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "123000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 2,
                calcSpKbn: 3,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 9700, 0, 0, 0, 300, 300);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 9700, 0, 0, 0, 300, 300);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 10000, 0, 0, 0, 0, 0);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230804, tensu: 30000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 240000, 0, 60000, 0, 0, 0, 0, 0);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230805, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 10000, 0, 0, 0, 0, 0);
        }

        //国保+千葉83(300円/日, 3回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  月上限に達する前に超過した場合
        //  認定証提示なし・70歳未満・県外国保組合（対象）
        [Test]
        public void T005_P12010_10_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 3,
                calcSpKbn: 3,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 9700, 0, 0, 0, 300, 300);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 40000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 40000, 320000, 0, 71630, 0, 0, 0, 8370, 8370);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 200, 0, 0, 0, 9800, 9800);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230804, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 500, 0, 0, 0, 9500, 9500);
        }

        //国保+千葉83(300円/日, 2回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  上限超過後に２科受診
        //  認定証提示なし・70歳未満・県外国保組合（対象）
        [Test]
        public void T005_P12010_11_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 2,
                calcSpKbn: 3,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 50000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 50000, 400000, 0, 82130, 0, 0, 0, 17870, 17870);
            //1日目(２科目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 5000, syoSaisin: SyosaiConst.Saisin2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 500, 0, 0, 0, 9500, 9500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 200, 0, 0, 0, 9800, 9800);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 5000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 500, 0, 0, 0, 9500, 9500);
        }

        //国保+52小慢(5000)+千葉83(300円/日, 2回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  公１分で公２上限に達する場合
        //  認定証提示なし・70歳未満・県外国保組合（対象）
        [Test]
        public void T005_P12010_12_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 2,
                calcSpKbn: 3,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 200, syoSaisin: SyosaiConst.Saisin, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 0, 100, 0, 0, 300, 300);
            //1日目(保険)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 200, syoSaisin: SyosaiConst.Saisin, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 400, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 200, syoSaisin: SyosaiConst.Saisin, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 0, 100, 0, 0, 300, 300);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 200, syoSaisin: SyosaiConst.Saisin, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 0, 400, 0, 0, 0, 0);
            //4日目(保険)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230804, tensu: 200, syoSaisin: SyosaiConst.Saisin, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 400, 0, 0, 0, 0, 0);
        }

        //国保+52小慢(5000)+千葉83(300円/日, 2回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  合算で公２上限に達する場合
        //  認定証提示なし・70歳未満・県外国保組合（対象）
        [Test]
        public void T005_P12010_13_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 2,
                calcSpKbn: 3,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 100, syoSaisin: SyosaiConst.Saisin, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 100, 800, 0, 0, 0, 0, 0, 200, 200);
            //1日目(保険)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 100, syoSaisin: SyosaiConst.Saisin, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 100, 800, 0, 100, 0, 0, 0, 100, 100);
            //2日目(保険)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 200, syoSaisin: SyosaiConst.Saisin, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //3日目(保険)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 200, syoSaisin: SyosaiConst.Saisin, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 400, 0, 0, 0, 0, 0);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230804, tensu: 200, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 0, 400, 0, 0, 0, 0);
        }

        //国保+52小慢(5000)+千葉83(300円/日, 3回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  公１上限に達する場合
        //  認定証提示なし・70歳未満・県外国保組合（対象）
        [Test]
        public void T005_P12010_14_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 3,
                calcSpKbn: 3,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 3000, syoSaisin: SyosaiConst.Saisin, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 24000, 0, 1000, 4700, 0, 0, 300, 300);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 200, syoSaisin: SyosaiConst.Saisin, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 400, 0, 0, 0, 0, 0);
            //3日目(保険)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 200, syoSaisin: SyosaiConst.Saisin, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 100, 0, 0, 0, 300, 300);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230804, tensu: 200, syoSaisin: SyosaiConst.Saisin, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 400, 0, 0, 0, 0, 0);
        }

        //国保+52小慢(5000)+千葉83(300円/日, 3回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  1ヶ月の自己負担額が80,100円＋αを超過
        //  認定証提示なし・70歳未満・県外国保組合（対象）
        [Test]
        public void T005_P12010_15_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 2,
                calcSpKbn: 3,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 50000, syoSaisin: SyosaiConst.Saisin, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 50000, 400000, 0, 95000, 4700, 0, 0, 300, 300);
            //1日目(保険)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 10000, syoSaisin: SyosaiConst.Saisin, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 5000, syoSaisin: SyosaiConst.Saisin, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 10000, 0, 0, 0, 0, 0);
            //2日目(保険)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 40000, syoSaisin: SyosaiConst.Saisin, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 40000, 320000, 0, 62130, 0, 0, 0, 17870, 17870);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 5000, syoSaisin: SyosaiConst.Saisin, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 10000, 0, 0, 0, 0, 0);
            //3日目(保険)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 5000, syoSaisin: SyosaiConst.Saisin, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 500, 0, 0, 0, 9500, 9500);
        }

        //国保+52小慢(5000)+千葉83(300円/日, 3回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  1ヶ月の自己負担額が80,100円＋αを超過
        //  認定証提示なし・70歳未満・県内国保組合（対象外）
        [Test]
        public void T005_P12010_16_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "123000",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 2,
                calcSpKbn: 3,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 50000, syoSaisin: SyosaiConst.Saisin, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 50000, 400000, 0, 95000, 4700, 0, 0, 300, 300);
            //1日目(保険)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 10000, syoSaisin: SyosaiConst.Saisin, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 5000, syoSaisin: SyosaiConst.Saisin, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 10000, 0, 0, 0, 0, 0);
            //2日目(保険)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 40000, syoSaisin: SyosaiConst.Saisin, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 40000, 320000, 0, 79700, 0, 0, 0, 300, 300);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 5000, syoSaisin: SyosaiConst.Saisin, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 10000, 0, 0, 0, 0, 0);
            //3日目(保険)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 5000, syoSaisin: SyosaiConst.Saisin, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 10000, 0, 0, 0, 0, 0);
        }

        //国保+52小慢(5000)+千葉83(300円/日, 3回/月)
        //  特殊計算[3]:
        //      複数科受診２科目は負担なし
        //      認定証の提示がない70歳未満の県外国保組合の場合、高額療養費の上限を超えた分を患者負担とする
        //  1ヶ月の自己負担額が80,100円＋αを超過
        //  認定証提示あり・70歳未満・県外国保組合（対象外）
        [Test]
        public void T005_P12010_17_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Chiba;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 20200401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "100",
                hokensyaNo: "103000",
                kogakuKbn: 28
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 300,
                monthLimitCount: 2,
                calcSpKbn: 3,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 50000, syoSaisin: SyosaiConst.Saisin, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 50000, 400000, 17570, 77430, 4700, 0, 0, 300, 300);
            //1日目(保険)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230801, tensu: 10000, syoSaisin: SyosaiConst.Saisin, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 0, 20000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 5000, syoSaisin: SyosaiConst.Saisin, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 9500, 500, 0, 0, 0, 0, 0);
            //2日目(保険)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230802, tensu: 40000, syoSaisin: SyosaiConst.Saisin, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 40000, 320000, 17570, 62130, 0, 0, 0, 300, 300);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 5000, syoSaisin: SyosaiConst.Saisin, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 9500, 500, 0, 0, 0, 0, 0);
            //3日目(保険)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230803, tensu: 5000, syoSaisin: SyosaiConst.Saisin, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 9500, 500, 0, 0, 0, 0, 0);
        }

        //社保+東京82マル都(10,000)（特殊計算[1]: 上限額までは患者負担無、上限額を超えた分が患者負担）
        [Test]
        public void T005_P13001_82()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Tokyo;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "82",
                hokenSbtKbn: HokenSbtKbn.Bunten,
                futanKbn: 0,
                monthSpLimit: 10000,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 3000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 3000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 0, 4000, 0, 0, 0, 2000, 2000);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 0, 0, 0, 0, 3000, 3000);
        }

        //社保+マル長(10,000)+東京82マル都(10,000)（特殊計算[1]: 上限額までは患者負担無、上限額を超えた分が患者負担）
        // 10,000円を超えるとマル長が助成するので窓口負担が発生しない
        [Test]
        public void T005_P13002_Marucyo_82()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Tokyo;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "82",
                hokenSbtKbn: HokenSbtKbn.Bunten,
                futanKbn: 0,
                monthSpLimit: 10000,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 0, 3000, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 0, 3000, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 0, 2000, 4000, 0, 0, 0, 0);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 3000, 0, 0, 0, 0, 0);
        }

        //社保+マル長(20,000)+東京82マル都(10,000)（特殊計算[1]: 上限額までは患者負担無、上限額を超えた分が患者負担）
        // 1万円まで無料、1万円から2万円まで患者負担、2万円を超えた分はマル長が助成
        [Test]
        public void T005_P13003_Marucyo_82()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Tokyo;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 20000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "82",
                hokenSbtKbn: HokenSbtKbn.Bunten,
                futanKbn: 0,
                monthSpLimit: 10000,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 0, 3000, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 21000, 0, 0, 7000, 0, 0, 2000, 2000);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 0, 0, 0, 0, 0, 6000, 6000);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 0, 4000, 0, 0, 0, 2000, 2000);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 3000, 0, 0, 0, 0, 0);
        }

        //社保+東京88マル子(200)（特殊計算[2]: 実日数にカウントしない検査のみの来院は患者負担なし）
        [Test]
        public void T005_P13004_88()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Tokyo;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "88",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 200,
                dayLimitCount: 1,
                calcSpKbn: 2
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000, jituNisu: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 3000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000, jituNisu: true);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 2800, 0, 0, 0, 200, 200);
        }

        //社保+東京88マル子(200)（特殊計算[2]: 実日数にカウントしない検査のみの来院は患者負担なし）
        // 同日2回目以降は患者負担なし
        [Test]
        public void T005_P13005_88()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Tokyo;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "88",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 200,
                dayLimitCount: 1,
                calcSpKbn: 2
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 50, jituNisu: true);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 50, 350, 0, 0, 0, 0, 0, 150, 150);
            //1日目 (同日2回目）
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000, jituNisu: true);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 3000, 0, 0, 0, 0, 0);
        }

        //社保+東京88マル子(200)（特殊計算[2]: 実日数にカウントしない検査のみの来院は患者負担なし）
        // 複数科受診の場合は、200円を上限として徴収
        [Test]
        public void T005_P13006_88()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Tokyo;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "88",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 200,
                dayLimitCount: 1,
                calcSpKbn: 2
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 50, jituNisu: true);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 50, 350, 0, 0, 0, 0, 0, 150, 150);
            //1日目 (複数科受診の場合）
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000, jituNisu: true, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 2950, 0, 0, 0, 50, 50);
        }

        //国保+マル長(20,000)+東京82マル都(10,000)+80(1割/18,000)
        // 負担率の公費がある場合にマル長上限まで負担が発生することの確認
        [Test]
        public void T005_P13007_Marucyo_8280()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Tokyo;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210301,
                birthDay: 19710622
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 20000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "82",
                hokenSbtKbn: HokenSbtKbn.Bunten,
                futanKbn: 0,
                monthSpLimit: 10000,
                calcSpKbn: 1,
                kogakuTekiyo: 11
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                futanRate: 10,
                monthLimitFutan: 18000,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 2, 3, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210301, tensu: 5909);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5909, 41363, 0, 0, 10000, 1818, 0, 5909, 5910);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210302, tensu: 2914);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2914, 20398, 0, 6469, 0, -641, 0, 2914, 2910);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210303, tensu: 2857);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2857, 19999, 0, 8571, 0, -1177, 0, 1177, 1180);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210304, tensu: 2948);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2948, 20636, 0, 8844, 0, 0, 0, 0, 0);
        }

        //社保+28(0)+東京80(1割/18,000)
        // 負担率の公費がある場合にマル長上限まで負担が発生することの確認
        [Test]
        public void T005_P13008_2880()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Tokyo;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210301,
                birthDay: 20001010
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "28",
                monthLimitFutan: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                futanRate: 10,
                monthLimitFutan: 18000,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 2, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210301, tensu: 3000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 21000, 0, 6000, 0, 0, 0, 3000, 3000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210302, tensu: 2000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 0, 6000, 0, 0, 0, 0, 0);
        }

        //社保+マル長(20,000)+東京82マル都(10,000)（特殊計算[1]: 上限額までは患者負担無、上限額を超えた分が患者負担）
        // 1万円まで無料、1万円から2万円まで患者負担、2万円を超えた分はマル長が助成
        // 窓口負担のまるめ誤差
        [Test]
        public void T005_P13009_Marucyo_82_PtFutan()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Tokyo;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 20000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "82",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 0,
                monthSpLimit: 10000,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210301, tensu: 5575);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5575, 39025, 0, 0, 10000, 0, 0, 6725, 6730);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210302, tensu: 3328);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3328, 23296, 0, 6709, 0, 0, 0, 3275, 3270);
        }

        //社保+静岡83（特殊計算[1]: 月の上限回数に他院分を含める）
        // 他院分なし
        [Test]
        public void T005_P22001_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Shizuoka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 500,
                monthLimitCount: 4,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1000, 0, 0, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1000, 0, 0, 0, 500, 500);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1000, 0, 0, 0, 500, 500);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1000, 0, 0, 0, 500, 500);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1500, 0, 0, 0, 0, 0);
        }

        //社保+静岡83（特殊計算[1]: 月の上限回数に他院分を含める）
        // 他院分あり
        [Test]
        public void T005_P22002_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Shizuoka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 500,
                monthLimitCount: 4,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1000, 0, 0, 0, 500, 500);
            //2日目(他院)
            newLimitCntListOther(futanVm: futanCalcVm, sinDate: 20181002);
            //3日目(他院)
            newLimitCntListOther(futanVm: futanCalcVm, sinDate: 20181003);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1000, 0, 0, 0, 500, 500);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1500, 0, 0, 0, 0, 0);
        }

        //社保+静岡83（特殊計算[1]: 月の上限回数に他院分を含める）
        // 他院分あり、同一日他科受診（同一来院）あり
        [Test]
        public void T005_P22003_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Shizuoka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 500,
                monthLimitCount: 4,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 500, syoSaisin: SyosaiConst.Syosin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1000, 0, 0, 0, 500, 500);
            //1日目(他科受診)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 500, syoSaisin: SyosaiConst.Syosin2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1500, 0, 0, 0, 0, 0);
            //2日目(他院)
            newLimitCntListOther(futanVm: futanCalcVm, sinDate: 20181002);
            //3日目(他院)
            newLimitCntListOther(futanVm: futanCalcVm, sinDate: 20181003);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1000, 0, 0, 0, 500, 500);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1500, 0, 0, 0, 0, 0);
        }

        //社保+静岡83（特殊計算[1]: 月の上限回数に他院分を含める）
        // 他院分あり、同一日他科受診（同一来院）、1回目の受診時に上限未満
        [Test]
        public void T005_P22004_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Shizuoka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 500,
                monthLimitCount: 4,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 100, syoSaisin: SyosaiConst.Syosin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 100, 700, 0, 0, 0, 0, 0, 300, 300);
            //1日目(他科受診)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 500, syoSaisin: SyosaiConst.Syosin2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1300, 0, 0, 0, 200, 200);
            //2日目(他院)
            newLimitCntListOther(futanVm: futanCalcVm, sinDate: 20181002);
            //3日目(他院)
            newLimitCntListOther(futanVm: futanCalcVm, sinDate: 20181003);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1000, 0, 0, 0, 500, 500);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1500, 0, 0, 0, 0, 0);
        }

        //社保+静岡83（特殊計算[1]: 月の上限回数に他院分を含める）
        // 他院分あり、同日再診、1回目の受診時に上限未満
        [Test]
        public void T005_P22005_83()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Shizuoka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "83",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                kaiLimitFutan: 500,
                monthLimitCount: 4,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 100, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 100, 700, 0, 0, 0, 0, 0, 300, 300);
            //1日目(同日再診)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 500, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1000, 0, 0, 0, 500, 500);
            //2日目(他院)
            newLimitCntListOther(futanVm: futanCalcVm, sinDate: 20181002);
            //3日目(他院)
            newLimitCntListOther(futanVm: futanCalcVm, sinDate: 20181003);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1500, 0, 0, 0, 0, 0);
        }

        //滋賀42 月8000円 高額療養費低所得8000円(窓口優先)
        [Test]
        public void T005_P25001_42()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Shiga;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19450101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "42",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                futanRate: 10,
                monthLimitFutan: 8000,
                kogakuTekiyo: 11,
                futanYusen: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1500, 12000, 0, 1500, 0, 0, 0, 1500, 1500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 15000, -1500, 0, 0, 0, 6500, 6500);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 2000, 0, 0, 0, 0, 0, 0);
        }

        //滋賀42(上限なし) 高額療養費低所得8000円(窓口優先)
        [Test]
        public void T005_P25002_42()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Shiga;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19450101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "42",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                futanRate: 10,
                monthLimitFutan: 0,
                kogakuTekiyo: 11,
                futanYusen: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1500, 12000, 0, 1500, 0, 0, 0, 1500, 1500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 15000, -1500, 0, 0, 0, 6500, 6500);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 2000, 0, 0, 0, 0, 0, 0);
        }

        //滋賀42 月8000円 高額療養費低所得8000円(窓口優先3)
        [Test]
        public void T005_P25003_42()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Shiga;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210201,
                birthDay: 19460703
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "42",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                futanRate: 10,
                monthLimitFutan: 8000,
                kogakuTekiyo: 11,
                futanYusen: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210201, tensu: 5638);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5638, 45104, 3276, 2362, 0, 0, 0, 5638, 5640);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210202, tensu: 888);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 888, 7104, 1776, -888, 0, 0, 0, 888, 890);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210203, tensu: 888);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 888, 7104, 1776, -888, 0, 0, 0, 888, 890);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210204, tensu: 6388);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 6388, 51104, 12776, -586, 0, 0, 0, 586, 580);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210205, tensu: 2051);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2051, 16408, 4102, 0, 0, 0, 0, 0, 0);
        }

        //滋賀42(上限なし) 高額療養費低所得8000円(窓口優先4)
        [Test]
        public void T005_P25004_42()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Shiga;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210201,
                birthDay: 19460703
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "42",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                futanRate: 10,
                monthLimitFutan: 0,
                kogakuTekiyo: 11,
                futanYusen: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210201, tensu: 5638);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5638, 45104, 3276, 2362, 0, 0, 0, 5638, 5640);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210202, tensu: 888);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 888, 7104, 1776, -888, 0, 0, 0, 888, 890);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210203, tensu: 888);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 888, 7104, 1776, -888, 0, 0, 0, 888, 890);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210204, tensu: 6388);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 6388, 51104, 12776, -586, 0, 0, 0, 586, 580);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20210205, tensu: 2051);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2051, 16408, 4102, 0, 0, 0, 0, 0, 0);
        }

        //大阪80 1日500円 月3000円
        [Test]
        public void T005_P27001_80()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1500, 0, 0, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1500, 0, 0, 0, 500, 500);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1500, 0, 0, 0, 500, 500);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1500, 0, 0, 0, 500, 500);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1500, 0, 0, 0, 500, 500);
            //6日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181006, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1500, 0, 0, 0, 500, 500);
            //7日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181007, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 2000, 0, 0, 0, 0, 0);
        }

        //大阪54(20% 月2500円)+80(1日500円 月3000円)
        [Test]
        public void T005_P27002_5480()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 2500
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 1000, 1500, 0, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 2500, 0, 0, 0, 500, 500);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 3000, 0, 0, 0, 0, 0);
        }

        //大阪80 1日500円 月3000円 -> 1日600円
        [Test]
        public void T005_P27003_80change()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                inpLimitFutan: 600
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1400, 0, 0, 0, 600, 600);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1400, 0, 0, 0, 600, 600);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1400, 0, 0, 0, 600, 600);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1400, 0, 0, 0, 600, 600);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1400, 0, 0, 0, 600, 600);
            //6日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181006, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 2000, 0, 0, 0, 0, 0);
        }

        //大阪54(20% 月2500円 -> 3000円)
        [Test]
        public void T005_P27004_54change()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                hokenSbtKbn: HokenSbtKbn.Bunten,
                futanKbn: 1,
                futanRate: 20,
                monthLimitFutan: 2500,
                isLimitList: 1,
                isLimitListSum: 1,
                kogakuTekiyo: 11,
                inpLimitFutan: 3000
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 1000, 0, 0, 0, 2000, 2000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 2000, 0, 0, 0, 1000, 1000);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 3000, 0, 0, 0, 0, 0);
        }

        //大阪28(月0円)+86(1日500円 月2回)
        [Test]
        public void T005_P27005_2886()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "28",
                monthLimitFutan: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "86",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitCount: 2
            );
            newHokenPattern(1, 2, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 200, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 0, 0, 0, 0, 400, 400);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 200, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 400, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 200, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 400, 0, 0, 0, 0, 0);
        }

        //兵庫県 社保一般+マル長(10,000)+82(600円/月2回)
        //  マル長が日単位（公費計算）の場合
        [Test]
        public void T005_P28001_Marucyo_82()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Hyogo;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "82",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 600,
                monthLimitCount: 2,
                kogakuTekiyo: 1,
                futanYusen: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 3500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3500, 24500, 0, 500, 9400, 0, 0, 600, 600);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1500, -600, 0, 0, 600, 600);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 3000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 21000, 0, 9000, 0, 0, 0, 0, 0);
        }

        //兵庫県 高齢1割+マル長(1,000)+82(600円/月2回) ※公１上限が1,200円未満
        //  マル長が日単位（公費計算）の場合
        [Test]
        public void T005_P28002_Marucyo_82()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Hyogo;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 1000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "82",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 600,
                monthLimitCount: 2,
                kogakuTekiyo: 1,
                futanYusen: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 0, 1000, 400, 0, 0, 600, 600);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 0, 2000, -400, 0, 0, 400, 400);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 0, 2000, 0, 0, 0, 0, 0);
        }

        //兵庫県 高齢1割+マル長(2,100)+82(600円/月2回) ※1日目に公１上限未満
        //  マル長が日単位（公費計算）の場合
        [Test]
        public void T005_P28003_Marucyo_82()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Hyogo;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 2100
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "82",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 600,
                monthLimitCount: 2,
                kogakuTekiyo: 1,
                futanYusen: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 0, 0, 1400, 0, 0, 600, 600);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 0, 1900, -500, 0, 0, 600, 600);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 0, 2000, 0, 0, 0, 0, 0);
        }

        //兵庫県 社保一般(区分オ)+82(600円/月2回)
        [Test]
        public void T005_P28004_Kogaku30_82()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Hyogo;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "82",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 600,
                monthLimitCount: 2,
                kogakuTekiyo: 11,
                futanYusen: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 12000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 12000, 84000, 600, 34800, 0, 0, 0, 600, 600);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 1500, -600, 0, 0, 0, 600, 600);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 3000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 21000, 9000, 0, 0, 0, 0, 0, 0);
        }

        //兵庫県 高齢1割(低所)+54(7000円)+82(600円/月2回)
        [Test]
        public void T005_P28005_Kogaku4_5482()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Hyogo;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 7000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "82",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 600,
                monthLimitCount: 2,
                kogakuTekiyo: 11,
                futanYusen: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 10000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 90000, 2000, 1000, 0, 0, 0, 7000, 7000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 2000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 0, 1400, 0, 0, 0, 600, 600);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 1000, -1000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 2000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 0, 1400, 0, 0, 0, 600, 600);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 2000, -1800, 0, 0, 0, -200, -200);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 1000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 1000, 0, 0, 0, 0, 0);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 1000, -1000, 0, 0, 0, 0, 0);
        }

        //兵庫県 高齢1割(低所)+54(7000円)+82(600円/月2回)
        [Test]
        public void T005_P28005_Kogaku4_5482_2()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Hyogo;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 7000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "82",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 600,
                monthLimitCount: 2,
                kogakuTekiyo: 11,
                futanYusen: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 10000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 90000, 2000, 1000, 0, 0, 0, 7000, 7000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 7500, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 7500, 67500, 0, 6900, 0, 0, 0, 600, 600);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 6500, -6500, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 2000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 1500, -100, 0, 0, 0, 600, 600);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 500, -300, 0, 0, 0, -200, -200);
        }

        // 一般+岡山85子ども(1割 44,400円/月)
        //  小慢(52)・自立支援(16,15,21)・難病(54) に係る医療費の自己負担を全額助成
        [Test]
        public void T005_P33001_85()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Okayama;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20240101,
                birthDay: 20081001
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                hokensyaNo: "0001",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "85",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                futanRate: 10,
                monthLimitFutan: 44400,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20240101, tensu: 5000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 10000, 0, 0, 0, 5000, 5000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20240102, tensu: 20000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 140000, 0, 40000, 0, 0, 0, 20000, 20000);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20240103, tensu: 20000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 140000, 0, 40600, 0, 0, 0, 19400, 19400);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20240104, tensu: 5000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 15000, 0, 0, 0, 0, 0);
        }

        // 一般+52小慢(2割 5,000円/月)+岡山85子ども(1割 44,400円/月)
        //  小慢(52)・自立支援(16,15,21)・難病(54) に係る医療費の自己負担を全額助成
        [Test]
        public void T005_P33002_85()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Okayama;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20240101,
                birthDay: 20081001
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                hokensyaNo: "0001",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "85",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                futanRate: 10,
                monthLimitFutan: 44400,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20240101, tensu: 2000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 0, 2000, 4000, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20240102, tensu: 2000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 0, 5000, 1000, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20240103, tensu: 2000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 0, 6000, 0, 0, 0, 0, 0);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20240104, tensu: 40000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 40000, 280000, 0, 80000, 0, 0, 0, 40000, 40000);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20240105, tensu: 5000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 10600, 0, 0, 0, 4400, 4400);
            //6日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20240106, tensu: 5000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 15000, 0, 0, 0, 0, 0);
        }

        //社保+広島90(1日500円)（特殊計算[1]: 初診時1日500円/月4回）
        // 初診・同日初診は負担なし
        [Test]
        public void T005_P34001_90()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Hiroshima;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                hokensyaNo: "0001",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "90",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitCount: 4,
                countKbn: 3,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目 初診
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 200, syoSaisin: SyosaiConst.Syosin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 0, 0, 0, 0, 400, 400);
            //1日目 同日初診
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 200, syoSaisin: SyosaiConst.Syosin2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 200, 1600, 0, 300, 0, 0, 0, 100, 100);
            //2日目 再診
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 2000, 0, 0, 0, 0, 0);
            //3日目 初診
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000, syoSaisin: SyosaiConst.Syosin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1500, 0, 0, 0, 500, 500);
            //4日目 初診
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 1000, syoSaisin: SyosaiConst.Syosin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1500, 0, 0, 0, 500, 500);
            //5日目 初診
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, tensu: 1000, syoSaisin: SyosaiConst.Syosin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1500, 0, 0, 0, 500, 500);
            //6日目 初診
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, tensu: 1000, syoSaisin: SyosaiConst.Syosin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 2000, 0, 0, 0, 0, 0);
        }

        //社保+福岡81（特殊計算[3]: 3歳まで無料、以後600円）
        // 3歳の誕生月
        [Test]
        public void T005_P40001_81()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Fukuoka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190401,
                birthDay: 20160402
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "81",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthLimitFutan: 600,
                calcSpKbn: 3
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190401, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190402, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190403, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
        }

        //社保+福岡81（特殊計算[3]: 3歳まで無料、以後600円）
        // 3歳の誕生月の翌月
        [Test]
        public void T005_P40002_81()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Fukuoka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190501,
                birthDay: 20160402
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "81",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthLimitFutan: 600,
                calcSpKbn: 3
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190501, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 400, 0, 0, 0, 600, 600);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190502, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
        }

        //社保+福岡81（特殊計算[3]: 3歳まで無料、以後600円）
        // 3歳の誕生月 1日生まれ
        [Test]
        public void T005_P40003_81()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Fukuoka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190401,
                birthDay: 20160401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "81",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthLimitFutan: 600,
                calcSpKbn: 3
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190401, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 400, 0, 0, 0, 600, 600);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190402, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
        }

        //社保+福岡81（特殊計算[4]: 4歳まで無料、以後600円）
        // 4歳の誕生月
        [Test]
        public void T005_P40004_81()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Fukuoka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190401,
                birthDay: 20150402
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "81",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthLimitFutan: 600,
                calcSpKbn: 4
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190401, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190402, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190403, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
        }

        //社保+福岡81（特殊計算[4]: 4歳まで無料、以後600円）
        // 4歳の誕生月の翌月
        [Test]
        public void T005_P40005_81()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Fukuoka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190501,
                birthDay: 20150402
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "81",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthLimitFutan: 600,
                calcSpKbn: 4
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190501, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 400, 0, 0, 0, 600, 600);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190502, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
        }

        //社保+福岡81（特殊計算[5]: 5歳まで無料、以後600円）
        // 5歳の誕生月
        [Test]
        public void T005_P40006_81()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Fukuoka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190401,
                birthDay: 20140402
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "81",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthLimitFutan: 600,
                calcSpKbn: 5
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190401, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190402, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190403, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
        }

        //社保+福岡81（特殊計算[5]: 5歳まで無料、以後600円）
        // 5歳の誕生月の翌月
        [Test]
        public void T005_P40007_81()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Fukuoka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190501,
                birthDay: 20140402
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "81",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthLimitFutan: 600,
                calcSpKbn: 5
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190501, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 400, 0, 0, 0, 600, 600);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190502, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
        }

        //社保+福岡81（特殊計算[6]: 未就学児無料、以後600円）
        // 就学前(4/2～12/31生まれ、生年+7の4/1が就学日)
        [Test]
        public void T005_P40008_81()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Fukuoka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190331,
                birthDay: 20120402
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "81",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthLimitFutan: 600,
                calcSpKbn: 6
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190331, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
        }

        //社保+福岡81（特殊計算[6]: 未就学児無料、以後600円）
        // 就学後(4/2～12/31生まれ、生年+7の4/1が就学日)
        [Test]
        public void T005_P40009_81()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Fukuoka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190401,
                birthDay: 20120402
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "81",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthLimitFutan: 600,
                calcSpKbn: 6
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190401, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 900, 0, 0, 0, 600, 600);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190402, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1500, 0, 0, 0, 0, 0);
        }

        //社保+福岡81（特殊計算[6]: 未就学児無料、以後600円）
        // 就学前(1/1～4/1生まれ、生年+6の4/1が就学日)
        [Test]
        public void T005_P40010_81()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Fukuoka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180331,
                birthDay: 20120401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "81",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthLimitFutan: 600,
                calcSpKbn: 6
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180331, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
        }

        //社保+福岡81（特殊計算[6]: 未就学児無料、以後600円）
        // 就学後(1/1～4/1生まれ、生年+6の4/1が就学日)
        [Test]
        public void T005_P40011_81()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Fukuoka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180401,
                birthDay: 20120401
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "81",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthLimitFutan: 600,
                calcSpKbn: 6
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 900, 0, 0, 0, 600, 600);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180402, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 1500, 0, 0, 0, 0, 0);
        }

        //社保6未+熊本42（特殊計算[1]: 一部負担金の1/3を自己負担）
        [Test]
        public void T005_P43001_42()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Kumamoto;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "42",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1001);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1001, 8008, 0, 1334, 0, 0, 0, 668, 668);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 5000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 6666, 0, 0, 0, 3334, 3334);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 5000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 6666, 0, 0, 0, 3334, 3334);
        }

        //社保6未+熊本42（特殊計算[2]: 一部負担金の1/3を自己負担+21,000円超で全額償還）
        [Test]
        public void T005_P43002_42()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Kumamoto;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "42",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthSpLimit: 21000,
                calcSpKbn: 2
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1001);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1001, 8008, 0, 1334, 0, 0, 0, 668, 668);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 5000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 6666, 0, 0, 0, 3334, 3334);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 5000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, -8000, 0, 0, 0, 18000, 18000);
        }

        //社保6未+熊本41（特殊計算[3]: 患者負担なし、21,000円超で全額償還）
        [Test]
        public void T005_P43003_41()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Kumamoto;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "41",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 0,
                monthSpLimit: 21000,
                calcSpKbn: 3
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1001);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1001, 8008, 0, 2002, 0, 0, 0, 0, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 5000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 10000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 5000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, -12002, 0, 0, 0, 22002, 22000);
        }

        //社保+54(2,500円)+熊本42（特殊計算[1]: 一部負担金の1/3を自己負担）
        [Test]
        public void T005_P43004_5442()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Kumamoto;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 2500
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "42",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                calcSpKbn: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目 54併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1002, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1002, 7014, 0, 1002, 1336, 0, 0, 668, 668);
            //2日目 保険分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 302, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 302, 2114, 0, 604, 0, 0, 0, 302, 302);
        }

        //社保6未+熊本天草市80（特殊計算[4]: 一部負担金の1/3を自己負担10円単位+21,000円超で全額償還）
        [Test]
        public void T005_P43005_Amakusa80()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Kumamoto;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                monthSpLimit: 21000,
                calcSpKbn: 4
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1001);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1001, 8008, 0, 1335, 0, 0, 0, 667, 670);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 5000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 6667, 0, 0, 0, 3333, 3330);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 5000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, -8002, 0, 0, 0, 18002, 18000);
        }

        //まるめ誤差(四捨五入すると上限超)
        [Test]
        public void T006_01_MarumeOver()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目(1回目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 103);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 103, 824, 0, 0, 0, 0, 0, 206, 210);
            //1日目(2回目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 103);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 103, 824, 0, 0, 0, 0, 0, 206, 210);
            //1日目(3回目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 103);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 103, 824, 0, 118, 0, 0, 0, 88, 80);
        }

        //まるめ誤差(四捨五入すると不足)
        [Test]
        public void T006_02_MarumeUnder()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目(1回目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 107);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 107, 856, 0, 0, 0, 0, 0, 214, 210);
            //1日目(2回目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 107);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 107, 856, 0, 0, 0, 0, 0, 214, 210);
            //1日目(3回目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 107);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 107, 856, 0, 142, 0, 0, 0, 72, 80);
        }

        //月2000円+1日500円 患者負担が2000円に届かない
        [Test]
        public void T006_03_Marume3hei()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 2000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目(1回目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 103);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 103, 824, 0, 0, 0, 0, 0, 206, 210);
            //1日目(2回目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 103);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 103, 824, 0, 0, 0, 0, 0, 206, 210);
            //1日目(3回目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 103);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 103, 824, 0, 0, 118, 0, 0, 88, 80);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 243);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 243, 1944, 0, 0, 0, 0, 0, 486, 490);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 243);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 243, 1944, 0, 0, 0, 0, 0, 486, 490);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 243);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 243, 1944, 0, 76, 0, 0, 0, 410, 390);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, tensu: 243);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 243, 1944, 0, 486, 0, 0, 0, 0, 0);
        }

        //月3000円+1日500円 日上限と月上限の両方に対して調整あり
        [Test]
        public void T006_04_Marume3hei()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20150101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 3000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目(1回目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 103);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 103, 824, 0, 0, 0, 0, 0, 206, 210);
            //1日目(2回目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 103);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 103, 824, 0, 0, 0, 0, 0, 206, 210);
            //1日目(3回目)
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 51);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 51, 408, 0, 0, 14, 0, 0, 88, 80);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 243);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 243, 1944, 0, 0, 0, 0, 0, 486, 490);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 243);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 243, 1944, 0, 0, 0, 0, 0, 486, 490);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 243);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 243, 1944, 0, 0, 0, 0, 0, 486, 490);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, tensu: 243);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 243, 1944, 0, 0, 0, 0, 0, 486, 490);
            //6日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181006, tensu: 243);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 243, 1944, 0, 0, 0, 0, 0, 486, 490);
            //7日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181007, tensu: 243);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 243, 1944, 0, 430, 0, 0, 0, 56, 30);
            //8日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181008, tensu: 243);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 243, 1944, 0, 486, 0, 0, 0, 0, 0);
        }

        //まるめ誤差（高額療養＋地単なし）
        [Test]
        public void T006_05_01_MarumeOver()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19400101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 5228);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5228, 47052, 0, 0, 0, 0, 0, 5228, 5230);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 2615);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2615, 23535, 0, 0, 0, 0, 0, 2615, 2620);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 2710);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2710, 24390, 2553, 0, 0, 0, 0, 157, 150);
        }

        //まるめ誤差（高額療養＋地単あり）
        [Test]
        public void T006_05_02_MarumeOver()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19400101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 5228);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5228, 47052, 0, 4728, 0, 0, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 2615);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2615, 23535, 0, 2115, 0, 0, 0, 500, 500);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 2710);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2710, 24390, 2553, 0, 0, 0, 0, 157, 160);
        }

        //まるめ誤差（高額療養1円単位＋地単なし）
        [Test]
        public void T006_05_03_MarumeOver()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 19383);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 19383, 135681, 0, 0, 0, 0, 0, 58149, 58150);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 45553);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 45553, 318871, 110884, 0, 0, 0, 0, 25775, 25774);
            Assert.That(futanCalcVm.KaikeiDetail.TotalKogakuLimit, Is.EqualTo(83924));
        }

        //70歳以上 高額一般(18000円) 1日超
        [Test]
        public void T007_01_Over70_20per_k0_1day()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19450101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 2000, 0, 0, 0, 0, 18000, 18000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 2000, 0, 0, 0, 0, 0, 0);
        }

        //70歳以上 高額一般(18000円) 1日目公1限度額未満・高額超
        [Test]
        public void T007_02_Over70_20per_k0_1day_2hei()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19450101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 10000
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 9500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 9500, 76000, 1000, 8500, 0, 0, 0, 9500, 9500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 5000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 10000, -500, 0, 0, 0, 500, 500);
        }

        //70歳以上 高額一般(18000円) 2日超
        [Test]
        public void T007_02_Over70_20per_k0_2day()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19450101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 5000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 0, 0, 0, 0, 10000, 10000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 5000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 2000, 0, 0, 0, 0, 8000, 8000);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 2000, 0, 0, 0, 0, 0, 0);
        }

        //70歳以上 高額一般(18000円) 負担率調整・他院分あり
        [Test]
        public void T007_02_Over70_20per_k0_2hei_Tain()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19450101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 15000
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 4806);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 4806, 38448, 0, 4806, 0, 0, 0, 4806, 4810);
            AssertEqualTo(futanCalcVm.LimitListInfs, 4810, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 4806);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 4806, 38448, 1224, 3582, 0, 0, 0, 4806, 4810);
            AssertEqualTo(futanCalcVm.LimitListInfs, 4810, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 4806);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 4806, 38448, 9612, -4806, 0, 0, 0, 4806, 4810);
            AssertEqualTo(futanCalcVm.LimitListInfs, 4810, 0);
            //4日目 他院
            newLimitListOther(futanVm: futanCalcVm, sinDate: 20181004, futanGaku: 500);
            //5日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, tensu: 4806);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 4806, 38448, 9612, -82, 0, 0, 0, 82, 70);
            AssertEqualTo(futanCalcVm.LimitListInfs, 70, 0);
            //6日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181006, tensu: 4806);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 4806, 38448, 9612, 0, 0, 0, 0, 0, 0);
            AssertEqualTo(futanCalcVm.LimitListInfs, 0, 0);
        }

        //70歳以上 高額現役並みⅠ(80,100円+α) 1日超
        [Test]
        public void T007_03_Over70_20per_k28_1day()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19450101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 28
            );
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 50000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 50000, 350000, 67570, 0, 0, 0, 0, 82430, 82430);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1530);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1530, 10710, 4437, 0, 0, 0, 0, 153, 153);
        }

        //70歳以上 高額一般(18000円) 2日超 合算
        [Test]
        public void T007_04_Over70_10per_k0_2daySum1()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000
            );
            newHokenPattern(1, 2, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 20000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 180000, 2000, 17500, 0, 0, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 12000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 12000, 108000, 0, 7000, 4500, 0, 0, 500, 500);
            AssertEqualTo(futanCalcVm.AdjustDetails, 1, 2, 5000, -4500, 0, 0, 0, -500, -500);
        }

        //70歳以上 高額一般(18000円) 2日超 合算
        [Test]
        public void T007_04_Over70_10per_k0_2daySum2()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000
            );
            newHokenPattern(1, 2, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 15000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 15000, 135000, 0, 14500, 0, 0, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 12000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 12000, 108000, 0, 7000, 4500, 0, 0, 500, 500);
            AssertEqualTo(futanCalcVm.AdjustDetails, 1, 2, 2000, -2000, 0, 0, 0, 0, 0);
        }

        //70歳以上 高額低所(8000円)+21 2日超 合算 国保2併
        [Test]
        public void T007_05_Over70_10per_k4_2daySum_kok2hei()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 0, 0, 0, 0);
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 10000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 90000, 2000, 0, 0, 0, 0, 8000, 8000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 20000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 180000, 2000, 13000, 0, 0, 0, 5000, 5000);
            AssertEqualTo(futanCalcVm.AdjustDetails, 1, 2, 5000, 0, 0, 0, 0, -5000, -5000);
        }

        //70歳以上 高額低所(8000円)+21 2日超 合算 国保(公2適用区分-所得)
        [Test]
        public void T007_05_Over70_10per_k4_2daySum_kok3hei()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 2, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 10000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 90000, 2000, 7500, 0, 0, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 20000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 180000, 2000, 13000, 4500, 0, 0, 500, 500);
            AssertEqualTo(futanCalcVm.AdjustDetails, 1, 2, 5000, -4500, 0, 0, 0, -500, -500);
        }

        //70歳以上 高額低所(8000円)+21 2日超 合算 社保公2適用区分一般)
        [Test]
        public void T007_05_Over70_10per_k4_2daySum_sya3hei()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 2, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 10000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 90000, 0, 9500, 0, 0, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 20000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 180000, 2000, 13000, 4500, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 10000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 90000, 2000, 7500, 0, 0, 0, 500, 500);
            AssertEqualTo(futanCalcVm.AdjustDetails, 1, 1, 5000, -5000, 0, 0, 0, 0, 0);
        }

        /// <summary>
        /// 月中で保険変更（後期高齢/上限が通算）
        /// </summary>
        [Test]
        public void T007_06_ChangeHoken_Kouki_k0()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19400101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "39",
                kogakuKbn: 0
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 0
            );
            newHokenPattern(1, 0, 0, 0, 0);
            newHokenPattern(2, 0, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 20000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 180000, 2000, 0, 0, 0, 0, 18000, 18000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 1000, 0, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 1000, 0, 0, 0, 0, 0, 0);
        }

        /// <summary>
        /// 月中で保険変更（後期高齢/上限が通算/公費併用分 合算対象）
        /// </summary>
        [Test]
        public void T007_06_ChangeHoken_Kouki_k0_54_5000()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19400101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Family,
                houbetu: "39",
                kogakuKbn: 0
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(2, 0, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 20000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 180000, 2000, 13000, 0, 0, 0, 5000, 5000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 1000, 0, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 20000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 180000, 2000, 0, 0, 0, 0, 18000, 18000);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 5000, 0, 0, 0, 0, -5000, -5000);
        }

        /// <summary>
        /// 月中で保険変更（前期高齢/上限は保険毎）
        /// </summary>
        [Test]
        public void T007_06_ChangeHoken_Over70_20per_k0()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19450101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newHokenPattern(1, 0, 0, 0, 0);
            newHokenPattern(2, 0, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 10000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 2000, 0, 0, 0, 0, 18000, 18000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 2000, 0, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 10000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 2000, 0, 0, 0, 0, 18000, 18000);
        }

        //国保(区分ウ)+54(0円) 合算対象外
        [Test]
        public void T007_07_Under70_54_0()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 28
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 0
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 210000, 9570, 80430, 0, 0, 0, 0, 0);
            //1日目 保険分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 40000, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 40000, 280000, 38570, 0, 0, 0, 0, 81430, 81430);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //国保(区分ウ)+54(5,000円) 合算対象
        [Test]
        public void T007_07_Under70_54_5000()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 28
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 210000, 9570, 75430, 0, 0, 0, 5000, 5000);
            //1日目 保険分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 40000, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 40000, 280000, 38570, 0, 0, 0, 0, 81430, 81430);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 2000, 0, 0, 0, 0, -2000, -2000);
        }

        //国保(区分ウ) 1円単位の窓口負担
        [Test]
        public void T007_07_Under70_PtFutan1en()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 28
            );
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30011);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30011, 210077, 9602, 0, 0, 0, 0, 80431, 80431);
        }

        //国保(区分ウ)+52+86 1円単位の窓口負担
        [Test]
        public void T007_07_Under70_PtFutan1en_52_86()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 20100101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 28
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 15000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "86",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 15000,
                monthLimitCount: 2,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目 52併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 193, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 193, 1351, 0, 193, 0, 0, 0, 386, 390);
            //1日目 保険分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 94922, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 94922, 664454, 197844, 86808, 0, 0, 0, 114, 110);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //公費医療・難病医療ガイド　平成27年7月版
        //事例１ 自立支援医療（更生医療）：併用部分に高額療養費が発生する場合
        //　低所得（公費の負担上限5,000円）・合算対象外
        [Test]
        public void T008_GuidBook_Jirei01()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180401,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 32000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 32000, 224000, 15370, 75630, 0, 0, 0, 5000, 5000);
            //1日目 保険分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 3000, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 21000, 0, 0, 0, 0, 0, 9000, 9000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //事例２ 自立支援医療（更生医療）：併用部分と医保単独部分に高額療養費が発生する場合
        //　低所得（公費の負担上限5,000円）・合算対象（一部負担金等がそれぞれ21,000円以上）
        [Test]
        public void T008_GuidBook_Jirei02()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180401,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 33000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 33000, 231000, 18270, 75730, 0, 0, 0, 5000, 5000);
            //1日目 保険分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 13500, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 13500, 94500, 5100, 0, 0, 0, 0, 35400, 35400);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 5000, 0, 0, 0, 0, -5000, -5000);
        }

        //事例３ 自立支援医療（更生医療）：医保単独部分に高額療養費が発生する場合
        //　区分イ（公費の負担上限20,000円）・合算対象
        [Test]
        public void T008_GuidBook_Jirei03()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180401,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 27
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 20000
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 23000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 23000, 161000, 0, 49000, 0, 0, 0, 20000, 20000);
            //1日目 保険分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 62000, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 62000, 434000, 17980, 0, 0, 0, 0, 168020, 168020);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 17700, 0, 0, 0, 0, -17700, -17700);
        }

        //事例４ 指定難病の特定医療費：併用部分に高額療養費が発生する場合
        //　低所得（公費の負担上限5,000円）・合算対象外
        [Test]
        public void T008_GuidBook_Jirei04()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180401,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 14000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 14000, 98000, 6600, 30400, 0, 0, 0, 5000, 5000);
            //1日目 保険分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 6000, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 42000, 0, 0, 0, 0, 0, 18000, 18000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //事例５ 指定難病の特定医療費：併用部分と医保単独部分に高額療養費が発生する場合
        //　低所得（公費の負担上限5,000円）・合算対象
        [Test]
        public void T008_GuidBook_Jirei05()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180401,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 14000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 14000, 98000, 6600, 30400, 0, 0, 0, 5000, 5000);
            //1日目 保険分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 13000, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 13000, 91000, 3600, 0, 0, 0, 0, 35400, 35400);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 5000, 0, 0, 0, 0, -5000, -5000);
        }

        //事例６ 指定難病の特定医療費：併用部分と医保単独部分に高額療養費が発生する場合
        //　所得ウ（公費の負担上限20,000円）・合算対象
        [Test]
        public void T008_GuidBook_Jirei06()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180401,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 20000
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 52000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 52000, 364000, 73370, 62630, 0, 0, 0, 20000, 20000);
            //1日目 保険分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 63000, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 63000, 441000, 105270, 0, 0, 0, 0, 83730, 83730);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 14800, 0, 0, 0, 0, -14800, -14800);
        }

        //社保+マル長 1日目超（月単位）
        [Test]
        public void T009_01_01_Marucyo()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 15000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 15000, 135000, 5000, 0, 0, 0, 0, 10000, 10000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 1000, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長 1日目超（日単位）
        [Test]
        public void T009_01_02_Marucyo()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 15000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 15000, 135000, 0, 5000, 0, 0, 0, 10000, 10000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 1000, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長 2日目超（月単位）
        [Test]
        public void T009_02_01_Marucyo()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 8000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 8000, 72000, 0, 0, 0, 0, 0, 8000, 8000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 27000, 1000, 0, 0, 0, 0, 2000, 2000);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 3000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 27000, 3000, 0, 0, 0, 0, 0, 0);
        }

        //社保+マル長 2日目超（日単位）
        [Test]
        public void T009_02_02_Marucyo()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 8000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 8000, 72000, 0, 0, 0, 0, 0, 8000, 8000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 27000, 0, 1000, 0, 0, 0, 2000, 2000);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 3000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 27000, 0, 3000, 0, 0, 0, 0, 0);
        }

        //社保+マル長 まるめ誤差（日単位）
        [Test]
        public void T009_02_03_Marucyo_Marume()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230901,
                birthDay: 19621008
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 20000
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230901, tensu: 3082);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3082, 21574, 0, 0, 0, 0, 0, 9246, 9250);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230904, tensu: 2482);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2482, 17374, 0, 0, 0, 0, 0, 7446, 7450);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230906, tensu: 2322);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2322, 16254, 0, 3658, 0, 0, 0, 3308, 3300);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230908, tensu: 2935);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2935, 20545, 0, 8805, 0, 0, 0, 0, 0);
        }

        //社保+マル長(10,000円)+15(5,000円)
        // 2日目にマル長限度額を超える場合（公費負担額を含まない／月単位）
        [Test]
        public void T009_03_01_01_Marucyo_sya15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 3;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2974);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2974, 20818, 0, 0, 5948, 0, 0, 2974, 2970);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3737);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3737, 26159, 10133, 0, -948, 0, 0, 2026, 2030);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)
        // 2日目にマル長限度額を超える場合（公費負担額を含む／月単位）
        [Test]
        public void T009_03_01_02_Marucyo_sya15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2974);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2974, 20818, 0, 0, 5948, 0, 0, 2974, 2970);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3737);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3737, 26159, 10133, 0, -948, 0, 0, 2026, 2030);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)
        // 2日目にマル長限度額を超える場合（公費負担額を含む／日単位）
        [Test]
        public void T009_03_01_03_Marucyo_sya15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2974);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2974, 20818, 0, 0, 5948, 0, 0, 2974, 2970);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3737);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3737, 26159, 0, 10133, 0, 0, 0, 1078, 1080);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)
        // 区分オ／2日目にマル長限度額を超える場合（公費負担額を含まない／月単位）
        [Test]
        public void T009_03_02_01_Marucyo_sya15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 3;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2974);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2974, 20818, 0, 0, 5948, 0, 0, 2974, 2970);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3737);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3737, 26159, 10133, 0, -948, 0, 0, 2026, 2030);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)
        // 区分オ／2日目にマル長限度額を超える場合（公費負担額を含む／月単位）
        [Test]
        public void T009_03_02_02_Marucyo_sya15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2974);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2974, 20818, 0, 0, 5948, 0, 0, 2974, 2970);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3737);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3737, 26159, 10133, 0, -948, 0, 0, 2026, 2030);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)
        // 区分オ／2日目にマル長限度額を超える場合（公費負担額を含む／日単位）
        [Test]
        public void T009_03_02_03_Marucyo_sya15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2974);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2974, 20818, 0, 0, 5948, 0, 0, 2974, 2970);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3737);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3737, 26159, 0, 10133, 0, 0, 0, 1078, 1080);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)
        // 区分オ／2日目にマル長限度額を超える場合／異点数（公費負担額を含まない／月単位）
        [Test]
        public void T009_03_03_01_Marucyo_sya15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 3;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2974, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2974, 20818, 0, 0, 0, 0, 0, 8922, 8920);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3737, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3737, 26159, 1211, 0, 6263, 0, 0, 3737, 3740);
            AssertEqualTo(futanCalcVm.AdjustDetails, 1, 2, 2659, 0, 0, 0, 0, -2659, -2660);
        }

        //社保+マル長(10,000円)+15(5,000円)
        // 区分オ／2日目にマル長限度額を超える場合／異点数（公費負担額を含む／月単位）
        [Test]
        public void T009_03_03_02_Marucyo_sya15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2974, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2974, 20818, 0, 0, 0, 0, 0, 8922, 8920);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3737, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3737, 26159, 1211, 0, 6263, 0, 0, 3737, 3740);
            AssertEqualTo(futanCalcVm.AdjustDetails, 1, 2, 8922, 0, 0, 0, 0, -8922, -8920);
        }

        //社保+マル長(10,000円)+15(5,000円)
        // 区分オ／2日目にマル長限度額を超える場合／異点数）（公費負担額を含む／日単位）
        [Test]
        public void T009_03_03_03_Marucyo_sya15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2974, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2974, 20818, 0, 0, 0, 0, 0, 8922, 8920);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3737, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3737, 26159, 0, 10133, 0, 0, 0, 1078, 1080);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)+54(5,000円)
        // 区分オ　（公費負担額を含まない／月単位）
        [Test]
        public void T009_03_04_01_Marucyo_sya1554()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 3;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 1, 3, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2974, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2974, 20818, 0, 0, 5948, 0, 0, 2974, 2970);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3737, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3737, 26159, 10133, 0, -948, 0, 0, 2026, 2030);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 109779, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 109779, 768453, 319337, 0, 5000, 0, 0, 5000, 5000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)+54(5,000円)
        // 区分オ　（公費負担額を含む／月単位）
        [Test]
        public void T009_03_04_02_Marucyo_sya1554()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 1, 3, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2974, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2974, 20818, 0, 0, 5948, 0, 0, 2974, 2970);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3737, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3737, 26159, 10133, 0, -948, 0, 0, 2026, 2030);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 109779, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 109779, 768453, 319337, 0, 5000, 0, 0, 5000, 5000);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 10000, 0, -5000, 0, 0, -5000, -5000);
        }

        //社保+マル長(10,000円)+15(5,000円)+54(5,000円)
        // 区分オ（公費負担額を含む／日単位）
        [Test]
        public void T009_03_04_03_Marucyo_sya1554()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 1, 3, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2974, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2974, 20818, 0, 0, 5948, 0, 0, 2974, 2970);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3737, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3737, 26159, 0, 10133, 0, 0, 0, 1078, 1080);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 109779, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 109779, 768453, 293937, 35400, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)+87(500円)
        // 区分オ／2日目に上限超（公費負担額を含まない／月単位）
        [Test]
        public void T009_03_05_01_Marucyo_sya1587()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 3;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "87",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 3, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2974, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2974, 20818, 0, 0, 5948, 2474, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3737, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3737, 26159, 10133, 0, -948, 1526, 0, 500, 500);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 3000, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)+87(500円)
        // 区分オ／2日目に上限超（公費負担額を含む／月単位）
        [Test]
        public void T009_03_05_02_Marucyo_sya1587()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "87",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 3, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2974, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2974, 20818, 0, 0, 5948, 2474, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3737, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3737, 26159, 10133, 0, -948, 1526, 0, 500, 500);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 3000, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)+87(500円)
        // 区分オ／2日目に上限超（公費負担額を含む／日単位）
        [Test]
        public void T009_03_05_03_Marucyo_sya1587()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "87",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 3, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 2974, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2974, 20818, 0, 0, 5948, 2474, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3737, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3737, 26159, 0, 10133, 0, 578, 0, 500, 500);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 3000, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)+87(500円)
        // 区分オ／1日目に上限超（公費負担額を含まない／月単位）
        [Test]
        public void T009_03_06_01_Marucyo_sya1587()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 3;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "87",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 3, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 3400, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3400, 23800, 200, 0, 6600, 2900, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 3000, 0, -1000, 500, 0, 500, 500);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 3000, 0, -600, 100, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)+87(500円)
        // 区分オ／1日目に上限超（公費負担額を含む／月単位）
        [Test]
        public void T009_03_06_02_Marucyo_sya1587()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "87",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 3, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 3400, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3400, 23800, 200, 0, 6600, 2900, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 3000, 0, -1000, 1000, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 3000, 0, -600, 600, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)+87(500円)
        // 区分オ／1日目に上限超（公費負担額を含む／日単位）
        [Test]
        public void T009_03_06_03_Marucyo_sya1587()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "87",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 3, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 3400, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3400, 23800, 0, 200, 6600, 2900, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 3000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 3000, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)+87(500円)
        // 区分オ／1日目に上限超（公費負担額を含まない／月単位）
        [Test]
        public void T009_03_07_01_Marucyo_sya1587()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 3;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "87",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 3, 0, 0);
            newHokenPattern(1, 1, 2, 3, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 0, 2500, 0, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3500, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3500, 24500, 500, 0, 6500, 3000, 0, 500, 500);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 3000, 0, -1000, 500, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)+87(500円)
        // 区分オ／1日目に上限超（公費負担額を含む／月単位）
        [Test]
        public void T009_03_07_02_Marucyo_sya1587()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "87",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 3, 0, 0);
            newHokenPattern(1, 1, 2, 3, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 0, 2500, 0, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3500, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3500, 24500, 500, 0, 6500, 3000, 0, 500, 500);
            AssertEqualTo(futanCalcVm.AdjustDetails, 1, 2, 3000, 0, -3000, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 3000, 0, -1000, 1000, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長(10,000円)+15(5,000円)+87(500円)
        // 区分オ／1日目に上限超（公費負担額を含む／日単位）
        [Test]
        public void T009_03_07_03_Marucyo_sya1587()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "87",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 3, 0, 0);
            newHokenPattern(1, 1, 2, 3, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 0, 2500, 0, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 3500, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3500, 24500, 0, 3500, 3500, 3000, 0, 500, 500);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 3000, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //後期低所+マル長(10,000円)+15(5,000円)
        // 1日目に保険単独分の負担がある場合（公費負担額を含む／日単位）
        [Test]
        public void T009_03_08_01_Marucyo_kok15_fix18397()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19400101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 6000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 54000, 0, 0, 1000, 0, 0, 5000, 5000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 8000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 8000, 72000, 0, 5000, 3000, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //後期低所+マル長(10,000円)+15(5,000円)
        // 1日目に公費併用分でマル長限度額を超える場合（公費負担額を含む／日単位）
        [Test]
        public void T009_03_08_02_Marucyo_kok15_fix18397()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19400101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 20000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 180000, 2000, 8000, 5000, 0, 0, 5000, 5000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 2000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 2000, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 2000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 0, 2000, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //後期低所+マル長(10,000円)+15(10,000円)
        // 1日目に公費併用分でマル長限度額を超える場合（公費負担額を含む／日単位）
        [Test]
        public void T009_03_08_03_Marucyo_kok15_fix18397()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19400101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 10000
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 20000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 180000, 12000, 0, 0, 0, 0, 8000, 8000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 2000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 2000, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 2000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 0, 2000, 0, 0, 0, 0, 0);
            AssertEqualTo(futanCalcVm.AdjustDetails, 1, 1, 2000, -2000, 0, 0, 0, 0, 0);
        }

        //後期低所+マル長(10,000円)+54(5,000円)
        // 1日目に保険単独分の負担がある場合（公費負担額を含む／日単位）
        [Test]
        public void T009_03_08_04_Marucyo_kok54_fix18397()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19400101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 6000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 54000, 0, 0, 1000, 0, 0, 5000, 5000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 8000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 8000, 72000, 6000, 0, 2000, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //後期低所+マル長(10,000円)+54(5,000円)
        // 1日目に公費併用分でマル長限度額を超える場合（公費負担額を含む／日単位）
        [Test]
        public void T009_03_08_05_Marucyo_kok54_fix18397()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19400101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 12000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 12000, 108000, 4000, 0, 3000, 0, 0, 5000, 5000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 2000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 2000, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 2000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 0, 2000, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //国保+マル長(10,000円)+15(5,000円)
        // 区分エ（公費負担額を含まない／月単位）
        [Test]
        public void T009_04_01_Marucyo_kok15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 3;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 0, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 880, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 880, 6160, 0, 0, 0, 0, 0, 2640, 2640);
            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 3030, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3030, 21210, 0, 0, 6060, 0, 0, 3030, 3030);
            //2日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 311, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 311, 2177, 0, 0, 0, 0, 0, 933, 930);
            //2日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 2578, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2578, 18046, 6824, 0, -1060, 0, 0, 1970, 1970);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //国保+マル長(10,000円)+15(5,000円)
        // 区分エ（公費負担額を含む／月単位）
        [Test]
        public void T009_04_02_Marucyo_kok15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 0, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 880, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 880, 6160, 0, 0, 0, 0, 0, 2640, 2640);
            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 3030, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3030, 21210, 0, 0, 6060, 0, 0, 3030, 3030);
            //2日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 311, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 311, 2177, 0, 0, 0, 0, 0, 933, 930);
            //2日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 2578, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2578, 18046, 6824, 0, -1060, 0, 0, 1970, 1970);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //国保+マル長(10,000円)+15(5,000円)
        // 区分エ（公費負担額を含む／日単位）
        [Test]
        public void T009_04_03_Marucyo_kok15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 30
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 0, 0, 0, 0);
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 880, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 880, 6160, 0, 0, 0, 0, 0, 2640, 2640);
            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 3030, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3030, 21210, 0, 0, 6060, 0, 0, 3030, 3030);
            //2日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 311, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 311, 2177, 0, 0, 0, 0, 0, 933, 930);
            //2日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 2578, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2578, 18046, 0, 6824, 0, 0, 0, 910, 910);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長 限度額特例１
        [Test]
        public void T010_01_Marucyo_tokurei1()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30,
                tokureiYm1: 201810
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 15000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 15000, 135000, 10000, 0, 0, 0, 0, 5000, 5000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 1000, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //社保+マル長 限度額特例２
        [Test]
        public void T010_02_Marucyo_tokurei2()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0,
                tokureiYm2: 201810
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 15000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 15000, 135000, 10000, 0, 0, 0, 0, 5000, 5000);
        }

        //社保+マル長 限度額特例(年月違い)
        [Test]
        public void T010_03_Marucyo_not_tokurei()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0,
                tokureiYm1: 201809
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 15000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 15000, 135000, 5000, 0, 0, 0, 0, 10000, 10000);
        }

        //社保一般(区分ウ) 限度額特例
        [Test]
        public void T010_04_Kogaku28_tokurei()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28,
                tokureiYm1: 201810
            );
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 210000, 48285, 0, 0, 0, 0, 41715, 41715);
        }

        //社保一般(区分オ) 限度額特例
        [Test]
        public void T010_05_Kogaku30_tokurei()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30,
                tokureiYm1: 201810
            );
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 30000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 210000, 72300, 0, 0, 0, 0, 17700, 17700);
        }

        //社保一般(低所得)+54 限度額特例（公費上限より高額上限が低い場合）
        [Test]
        public void T010_06_54_tokurei1()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20220401,
                birthDay: 19470420
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 4,
                tokureiYm1: 202204
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20220401, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 0, 0, 0, 0, 2000, 2000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20220402, tensu: 800);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 800, 6400, 0, 0, 0, 0, 0, 1600, 1600);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20220403, tensu: 800);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 800, 6400, 1200, 0, 0, 0, 0, 400, 400);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20220404, tensu: 800);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 800, 6400, 1600, 0, 0, 0, 0, 0, 0);
        }

        //社保一般(低所得)+54 限度額特例（公費上限より高額上限が高い場合）
        [Test]
        public void T010_06_54_tokurei2()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20220401,
                birthDay: 19470420
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 4,
                tokureiYm1: 202204
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 2500
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20220401, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 0, 0, 0, 0, 2000, 2000);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20220402, tensu: 800);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 800, 6400, 0, 1100, 0, 0, 0, 500, 500);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20220403, tensu: 800);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 800, 6400, 1200, 400, 0, 0, 0, 0, 0);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20220404, tensu: 800);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 800, 6400, 1600, 0, 0, 0, 0, 0, 0);
        }

        //後期一般(18000)+54(10000)+80(P27) 限度額特例（公費上限より高額上限が低い場合）
        [Test]
        public void T010_07_54_P2780_tokurei1()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230401,
                birthDay: 19400420
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 0,
                tokureiYm1: 202304
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230401, tensu: 20000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 180000, 11000, 0, 8500, 0, 0, 500, 500);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230402, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 1000, 0, 0, 0, 0, 0, 0);
        }

        //国保(高齢1割)+54(2,500円、他院分500円×2)
        [Test]
        public void T011_01_10per_54tain()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 2500
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 496);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 496, 4464, 0, 0, 0, 0, 0, 496, 500);
            AssertEqualTo(futanCalcVm.LimitListInfs, 500, 4960);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181002, tensu: 496);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 496, 4464, 0, 0, 0, 0, 0, 496, 500);
            AssertEqualTo(futanCalcVm.LimitListInfs, 500, 4960);
            //3日目 他院
            newLimitListOther(futanVm: futanCalcVm, sinDate: 20181003, futanGaku: 500);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181004, tensu: 496);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 496, 4464, 0, 0, 0, 0, 0, 496, 500);
            AssertEqualTo(futanCalcVm.LimitListInfs, 500, 4960);
            //5日目 他院
            newLimitListOther(futanVm: futanCalcVm, sinDate: 20181005, futanGaku: 500);
            //6日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181006, tensu: 496);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 496, 4464, 0, 484, 0, 0, 0, 12, 0);
            AssertEqualTo(futanCalcVm.LimitListInfs, 0, 4960);
        }

        //国保(高齢1割)+54(2,500円、他院分500円×2)
        [Test]
        public void T011_02_10per_54tain()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 2500
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 504);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 504, 4536, 0, 0, 0, 0, 0, 504, 500);
            AssertEqualTo(futanCalcVm.LimitListInfs, 500, 5040);
            //2日目 他院
            newLimitListOther(futanVm: futanCalcVm, sinDate: 20181002, futanGaku: 500);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 504);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 504, 4536, 0, 0, 0, 0, 0, 504, 500);
            AssertEqualTo(futanCalcVm.LimitListInfs, 500, 5040);
            //4日目 他院
            newLimitListOther(futanVm: futanCalcVm, sinDate: 20181004, futanGaku: 500);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181005, tensu: 504);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 504, 4536, 0, 12, 0, 0, 0, 492, 500);
            AssertEqualTo(futanCalcVm.LimitListInfs, 500, 5040);
        }

        //国保(高齢2割)+54(10,000円、他院分9,170円)+88(500円)
        [Test]
        public void T011_03_20per_54tain88()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19450101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "88",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 413);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 413, 3304, 0, 0, 326, 0, 0, 500, 500);
            AssertEqualTo(futanCalcVm.LimitListInfs, 830, 4130);
            //2日目 他院
            newLimitListOther(futanVm: futanCalcVm, sinDate: 20181002, futanGaku: 9170);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181003, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1996, 0, 0, 0, 4, 0);
            AssertEqualTo(futanCalcVm.LimitListInfs, 0, 10000);
        }

        //生保単独(0円)
        [Test]
        public void T012_01_12only_0()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "0",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "12",
                monthLimitFutan: 0
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 0, 0, 100000, 0, 0, 0, 0, 0);
        }

        //生保単独(2500円)
        [Test]
        public void T012_02_12only_2500()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "0",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "12",
                monthLimitFutan: 2500
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 0, 0, 97500, 0, 0, 0, 2500, 2500);
        }

        //生保単独(1円単位の上限 1504円)
        [Test]
        public void T012_03_12only_1504()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "0",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "12",
                monthLimitFutan: 1504
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 500);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 0, 0, 3496, 0, 0, 0, 1504, 1504);
        }

        //社保+生保(0円)
        [Test]
        public void T012_04_sya12_kogaku0()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19440101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "12",
                monthLimitFutan: 0
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 10000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 90000, 2000, 8000, 0, 0, 0, 0, 0);
        }

        //国保減免（減額）
        [Test]
        public void T013_01_KokhoGenmen_Gaku()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0,
                genmenKbn: GenmenKbn.Gengaku,
                genmenGaku: 1000
            );
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1006);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1006, 7042, 0, 0, 0, 0, 0, 3018, 2020, 1000);
        }

        //国保減免（減免率）
        [Test]
        public void T013_02_KokhoGenmen_Rate()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0,
                genmenKbn: GenmenKbn.Gengaku,
                genmenRate: 50
            );
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1004);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1004, 7028, 0, 0, 0, 0, 0, 3012, 1510, 1506);
        }

        //国保減免（免除）
        [Test]
        public void T013_03_KokhoGenmen_Menjyo()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0,
                genmenKbn: GenmenKbn.Menjyo
            );
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1006);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1006, 7042, 0, 0, 0, 0, 0, 3018, 0, 3018);
        }

        //国保減免（支払猶予）
        [Test]
        public void T013_04_KokhoGenmen_Yuyo()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19850101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0,
                genmenKbn: GenmenKbn.Yuyo
            );
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181001, tensu: 1006);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1006, 7042, 0, 0, 0, 0, 0, 3018, 0, 3018);
        }

        //自立支援減免
        [Test]
        public void T014_01_JirituGenmen()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180401,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0,
                genmenKbn: GenmenKbn.Jiritusien
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 2500
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 1000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 2000, 0, 0, 0, 1000, 0, 1000);
            AssertEqualTo(futanCalcVm.LimitListInfs, 1000, 0);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180402, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 0, 4500, 0, 0, 0, 1500, 0, 1500);
            AssertEqualTo(futanCalcVm.LimitListInfs, 1500, 0);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180403, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 0, 6000, 0, 0, 0, 0, 0);
            AssertEqualTo(futanCalcVm.LimitListInfs, 0, 0);
        }

        //自立支援減免+大阪80(500円/日, 3000円/月)
        [Test]
        public void T014_02_JirituGenmen()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180401,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0,
                genmenKbn: GenmenKbn.Jiritusien
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 2500
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目 21併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 600, hokenPid: 1, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 600, 4200, 0, 1200, 0, 0, 0, 600, 0, 600);
            AssertEqualTo(futanCalcVm.LimitListInfs, 600, 0);
            //1日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 300, hokenPid: 2, syoSaisin: SyosaiConst.Saisin2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 300, 2100, 0, 400, 0, 0, 0, 500, 500, 0);
            //2日目 21併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180402, tensu: 600, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 600, 4200, 0, 1200, 0, 0, 0, 600, 0, 600);
            AssertEqualTo(futanCalcVm.LimitListInfs, 600, 0);
            //3日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180403, tensu: 300, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 300, 2100, 0, 400, 0, 0, 0, 500, 500, 0);
            //4日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180404, tensu: 300, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 300, 2100, 0, 400, 0, 0, 0, 500, 500, 0);
            //5日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180405, tensu: 300, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 300, 2100, 0, 400, 0, 0, 0, 500, 500, 0);
            //6日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180406, tensu: 300, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 300, 2100, 0, 400, 0, 0, 0, 500, 500, 0);
            //7日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180407, tensu: 300, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 300, 2100, 0, 400, 0, 0, 0, 500, 500, 0);
            //8日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180408, tensu: 300, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 300, 2100, 0, 900, 0, 0, 0, 0, 0, 0);
        }

        //自立支援減免+大阪82(500円/日, 2回/月)
        [Test]
        public void T014_03_JirituGenmen()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180401,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0,
                genmenKbn: GenmenKbn.Jiritusien
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 2500
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "82",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitCount: 2
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 2, 0, 0, 0);

            //1日目 21併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 600, hokenPid: 1, syoSaisin: SyosaiConst.Saisin);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 600, 4200, 0, 1200, 0, 0, 0, 600, 0, 600);
            AssertEqualTo(futanCalcVm.LimitListInfs, 600, 0);
            //1日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180401, tensu: 300, hokenPid: 2, syoSaisin: SyosaiConst.Saisin2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 300, 2100, 0, 400, 0, 0, 0, 500, 500, 0);
            //2日目 21併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180402, tensu: 600, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 600, 4200, 0, 1200, 0, 0, 0, 600, 0, 600);
            AssertEqualTo(futanCalcVm.LimitListInfs, 600, 0);
            //3日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180403, tensu: 300, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 300, 2100, 0, 900, 0, 0, 0, 0, 0);
        }

        //後期(低所)+マル長+15更生+大阪80(fix)
        [Test]
        public void T015_01_KoukiTeisyotoku_k15_p27k80()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181203,
                birthDay: 19400101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 3, 0);
            newHokenPattern(1, 3, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181203, tensu: 4966, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 4966, 44694, 0, 0, 0, 4466, 0, 500, 500);
            //1日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181203, tensu: 4270, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 4270, 38430, 0, 4270, 0, 0, 0, 0, 0);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 1236, -1236, 0, 0, 0, 0, 0);
            //2日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181205, tensu: 2716, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2716, 24444, 0, 0, 2716, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181205, tensu: 92, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 92, 828, 0, 0, 0, 0, 0, 92, 90);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 92, 0, 0, 0, 0, -92, -90);
            //3日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181207, tensu: 3066, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3066, 27594, 0, 748, 2318, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //4日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181210, tensu: 2716, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2716, 24444, 0, 2716, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //5日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181212, tensu: 2716, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2716, 24444, 0, 2716, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //5日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181212, tensu: 92, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 92, 828, 0, 0, 0, 0, 0, 92, 90);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 92, 0, 0, 0, 0, -92, -90);
            //6日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181214, tensu: 2796, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2796, 25164, 976, 1820, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //7日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181217, tensu: 2773, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2773, 24957, 2773, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //8日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181219, tensu: 2788, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2788, 25092, 2788, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //9日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181221, tensu: 2716, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2716, 24444, 2716, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //10日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181224, tensu: 3096, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3096, 27864, 3096, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //11日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181226, tensu: 2716, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2716, 24444, 2716, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //11日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181226, tensu: 1824, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1824, 16416, 0, 1324, 0, 0, 0, 500, 500);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 1824, -1324, 0, 0, 0, -500, -500);
            //12日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181228, tensu: 2716, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2716, 24444, 2716, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //12日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181228, tensu: 68, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 68, 612, 0, 0, 0, 0, 0, 68, 70);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 68, 0, 0, 0, 0, -68, -70);
            //13日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181231, tensu: 3096, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3096, 27864, 3096, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //後期(低所)+マル長+15更生+大阪80(負担なしにした場合)(fix)
        [Test]
        public void T015_01_KoukiTeisyotoku_k15_p27k80_0en()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181203,
                birthDay: 19400101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 0,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 3, 0);
            newHokenPattern(1, 3, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181203, tensu: 4966, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 4966, 44694, 0, 0, 0, 4966, 0, 0, 0);
            //1日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181203, tensu: 4270, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 4270, 38430, 0, 4270, 0, 0, 0, 0, 0);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 1236, -1236, 0, 0, 0, 0, 0);
            //2日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181205, tensu: 2716, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2716, 24444, 0, 0, 2716, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181205, tensu: 92, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 92, 828, 0, 92, 0, 0, 0, 0, 0);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 92, -92, 0, 0, 0, 0, 0);
            //3日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181207, tensu: 3066, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3066, 27594, 0, 748, 2318, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //4日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181210, tensu: 2716, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2716, 24444, 0, 2716, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //5日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181212, tensu: 2716, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2716, 24444, 0, 2716, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //5日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181212, tensu: 92, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 92, 828, 0, 92, 0, 0, 0, 0, 0);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 92, -92, 0, 0, 0, 0, 0);
            //6日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181214, tensu: 2796, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2796, 25164, 976, 1820, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //7日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181217, tensu: 2773, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2773, 24957, 2773, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //8日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181219, tensu: 2788, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2788, 25092, 2788, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //9日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181221, tensu: 2716, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2716, 24444, 2716, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //10日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181224, tensu: 3096, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3096, 27864, 3096, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //11日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181226, tensu: 2716, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2716, 24444, 2716, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //11日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181226, tensu: 1824, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1824, 16416, 0, 1824, 0, 0, 0, 0, 0);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 1824, -1824, 0, 0, 0, 0, 0);
            //12日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181228, tensu: 2716, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2716, 24444, 2716, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //12日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181228, tensu: 68, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 68, 612, 0, 68, 0, 0, 0, 0, 0);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 68, -68, 0, 0, 0, 0, 0);
            //13日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20181231, tensu: 3096, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3096, 27864, 3096, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //後期(低所)+マル長+15更生+大阪80(fix)
        [Test]
        public void T015_02_KoukiTeisyotoku_k15_p27k80()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190102,
                birthDay: 19400101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 3, 0);
            newHokenPattern(1, 3, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190102, tensu: 5948, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5948, 53532, 0, 0, 948, 4500, 0, 500, 500);
            //1日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190102, tensu: 1802, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1802, 16218, 0, 1802, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190104, tensu: 3075, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3075, 27675, 0, 0, 3075, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190104, tensu: 640, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 640, 5760, 0, 140, 0, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190105, tensu: 9779, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 9779, 88011, 802, 8000, 977, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //4日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190107, tensu: 3075, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3075, 27675, 3075, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //4日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190107, tensu: 841, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 841, 7569, 0, 341, 0, 0, 0, 500, 500);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 283, -283, 0, 0, 0, 0, 0);
            //5日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190109, tensu: 2718, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2718, 24462, 2718, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //5日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190109, tensu: 144, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 144, 1296, 0, 0, 0, 0, 0, 144, 140);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 144, 0, 0, 0, 0, -144, -140);
            //6日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190111, tensu: 3205, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3205, 28845, 3205, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //6日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190111, tensu: 52, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 52, 468, 0, 0, 0, 0, 0, 52, 50);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 52, 0, 0, 0, 0, -52, -50);
            //7日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190111, tensu: 9679, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 9679, 87111, 9679, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //後期(低所)+マル長+15更生+大阪80(fix)
        [Test]
        public void T015_03_KoukiTeisyotoku_k15_p27k80()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190401,
                birthDay: 19400101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 3, 0);
            newHokenPattern(1, 3, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190401, tensu: 5519, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5519, 49671, 0, 0, 519, 4500, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //1日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190401, tensu: 225, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 225, 2025, 0, 225, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190403, tensu: 12439, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 12439, 111951, 0, 7958, 4481, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190403, tensu: 225, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 225, 2025, 0, 0, 0, 0, 0, 225, 230);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190405, tensu: 3126, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3126, 28134, 3084, 42, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190405, tensu: 37, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 37, 333, 0, 0, 0, 0, 0, 37, 40);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //4日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190408, tensu: 3120, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3120, 28080, 3120, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //4日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190408, tensu: 37, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 37, 333, 0, 0, 0, 0, 0, 37, 40);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //5日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190409, tensu: 1009, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1009, 9081, 0, 509, 0, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //6日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190410, tensu: 12419, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 12419, 111771, 12419, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //6日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190410, tensu: 972, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 972, 8748, 0, 472, 0, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //7日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190412, tensu: 3226, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3226, 29034, 3226, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //8日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190415, tensu: 3177, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3177, 28593, 3177, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //9日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190416, tensu: 709, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 709, 6381, 0, 209, 0, 0, 0, 500, 500);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 214, -209, 0, 0, 0, -5, 0);
            //10日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190417, tensu: 12339, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 12339, 111051, 12339, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //11日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190419, tensu: 3126, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3126, 28134, 3126, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //12日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190422, tensu: 3120, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3120, 28080, 3120, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //13日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190424, tensu: 12339, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 12339, 111051, 12339, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //13日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190424, tensu: 92, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 92, 828, 0, 0, 0, 0, 0, 92, 90);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 92, 0, 0, 0, 0, -92, -90);
        }

        //マル長+大阪80(fixまるめ誤差)
        [Test]
        public void T015_04_KoukiMarucho_p27k80()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190501,
                birthDay: 19321210
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190501, tensu: 5988);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5988, 53892, 0, 0, 5488, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190503, tensu: 3796);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3796, 34164, 0, 0, 3296, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190506, tensu: 4121);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 4121, 37089, 0, 3905, 0, 0, 0, 216, 220);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190508, tensu: 3225);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3225, 29025, 0, 3225, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //後期+マル長+15更生(fixまるめ誤差)
        [Test]
        public void T015_05_KoukiMarucyo_k15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 1;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20200502,
                birthDay: 19470827
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200502, tensu: 4847, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 4847, 43623, 0, 0, 0, 0, 0, 4847, 4850);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //1日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200502, tensu: 2520, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2520, 22680, 0, 0, 0, 0, 0, 2520, 2520);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200505, tensu: 2877, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2877, 25893, 0, 0, 2724, 0, 0, 153, 150);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200505, tensu: 294, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 294, 2646, 0, 0, 0, 0, 0, 294, 290);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200507, tensu: 2497, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2497, 22473, 0, 221, 2276, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //4日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200509, tensu: 2497, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2497, 22473, 0, 2497, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //5日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200512, tensu: 2497, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2497, 22473, 0, 2497, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //6日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200514, tensu: 2497, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2497, 22473, 0, 2497, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //6日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200514, tensu: 93, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 93, 837, 0, 0, 0, 0, 0, 93, 90);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //7日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200516, tensu: 2503, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2503, 22527, 2215, 288, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //8日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200519, tensu: 2497, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2497, 22473, 2497, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //9日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200521, tensu: 2497, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2497, 22473, 2497, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //10日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200523, tensu: 2503, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2503, 22527, 2503, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //11日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200526, tensu: 2497, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2497, 22473, 2497, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //11日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200526, tensu: 469, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 469, 4221, 0, 0, 0, 0, 0, 469, 470);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 376, 0, 0, 0, 0, -376, -370);
        }

        //国保低所+限度額特例+マル長+15更生+大阪80(fix)
        [Test]
        public void T015_06_KoureiMarucyo_k15p27k80()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19430809
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4,
                tokureiYm1: 201808
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 3, 0);
            newHokenPattern(1, 3, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180801, tensu: 5411, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5411, 48699, 1411, 0, 0, 3500, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //1日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20180801, tensu: 4270, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 4270, 38430, 270, 4000, 0, 0, 0, 0, 0);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 4000, -4000, 0, 0, 0, 0, 0);
        }

        //国保低所+マル長(10000)+15更生(10000)+大阪80(fix)
        [Test]
        public void T015_07_KokhoMarucyo_k15p27k80()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190125,
                birthDay: 19600623
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 29
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 3, 0);
            newHokenPattern(1, 3, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190125, tensu: 5125, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5125, 35875, 5375, 0, 4875, 4625, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //1日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190125, tensu: 354, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 354, 2478, 0, 1062, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190128, tensu: 1991, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1991, 13937, 5973, 0, -1991, 1991, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20190130, tensu: 3000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 21000, 9000, 0, -2884, 2884, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //後期低所+マル長(10000)+15更生(5000)+大阪80(fix)
        [Test]
        public void T015_08_KokhoMarucyo_k15p27k80()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20200324,
                birthDay: 19420924
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 3, 0);
            newHokenPattern(1, 3, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200324, tensu: 5154, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5154, 46386, 0, 0, 154, 4500, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //1日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200324, tensu: 498, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 498, 4482, 0, 498, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200328, tensu: 3004, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3004, 27036, 0, 0, 3004, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200328, tensu: 86, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 86, 774, 0, 0, 0, 0, 0, 86, 90);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200330, tensu: 2738, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2738, 24642, 896, 0, 1842, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200330, tensu: 284, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 284, 2556, 0, 0, 0, 0, 0, 284, 280);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //4日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20200331, tensu: 3000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 27000, 0, 2500, 0, 0, 0, 500, 500);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 868, -868, 0, 0, 0, 0, 0);
        }

        //社保一般+マル長(10000)+15更生(10000)+大阪80(fix)
        [Test]
        public void T015_09_SyahoMarucyo_k15p27k80()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230201,
                birthDay: 19800101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            newHokenPattern(1, 1, 2, 3, 0);
            newHokenPattern(1, 1, 3, 0, 0);

            //1日目 1来院目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230201, tensu: 500, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 0, 1000, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //1日目 2来院目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230201, tensu: 5000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 5000, 0, 5000, 5000, 0, 0, 0);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 1, 1500, 0, -1500, 0, 0, 0, 0);
            //1日目 2来院目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230201, tensu: 500, hokenPid: 2, newRaiin: false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 3500, 0, 0, 1500, 0, 0, 0, 0);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 1500, 0, -1500, 0, 0, 0, 0);
        }

        //後期上位+マル長(10000)+15更生(20000)(fix #17111)
        [Test]
        public void T016_01_KoukiMarucyo_k15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230401,
                birthDay: 19350101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 26
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 20000
            );
            newHokenPattern(1, 1, 2, 0, 0);

            //1日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230401, tensu: 2600);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2600, 18200, 0, 0, 5200, 0, 0, 2600, 2600);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230402, tensu: 2600);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2600, 18200, 5600, 0, -400, 0, 0, 2600, 2600);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230403, tensu: 12000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 12000, 84000, 36000, 0, -4800, 0, 0, 4800, 4800);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //4日目
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230404, tensu: 2000);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 6000, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //後期低所+マル長(10000)+15更生(5000)(fix #18317)
        [Test]
        public void T016_02_01_KoukiMarucyo_k15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230401,
                birthDay: 19350101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230401, tensu: 11000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 11000, 99000, 1000, 0, 5000, 0, 0, 5000, 5000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230402, tensu: 1000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 1000, 0, 0, 0, 0, -1000, -1000);
        }

        //後期低所+マル長(10000)+15更生(5000)
        [Test]
        public void T016_02_02_KoukiMarucyo_k15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230401,
                birthDay: 19350101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230401, tensu: 6000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 54000, 0, 0, 1000, 0, 0, 5000, 5000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230402, tensu: 1000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230403, tensu: 3000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 27000, 0, 0, 3000, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //4日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230404, tensu: 3000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 27000, 2000, 0, 1000, 0, 0, 0, 0);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 1, 1000, 0, 0, 0, 0, -1000, -1000);
        }

        //後期低所+マル長(10000)+15更生(10000)
        [Test]
        public void T016_02_03_KoukiMarucyo_k15()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230401,
                birthDay: 19350101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 10000
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230401, tensu: 11000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 11000, 99000, 3000, 0, 0, 0, 0, 8000, 8000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230402, tensu: 1000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 1000, 0, 0, 0, 0, -1000, -1000);
        }

        //後期低所+マル長(10000)+54難病(5000)
        [Test]
        public void T016_02_04_KoukiMarucyo_k54()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230401,
                birthDay: 19350101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230401, tensu: 11000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 11000, 99000, 3000, 0, 3000, 0, 0, 5000, 5000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230402, tensu: 1000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 1000, 0, 0, 0, 0, -1000, -1000);
        }

        //後期低所+マル長(10000)+54難病(5000)
        [Test]
        public void T016_02_05_KoukiMarucyo_k54()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230401,
                birthDay: 19350101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230401, tensu: 6000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 54000, 0, 0, 1000, 0, 0, 5000, 5000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230402, tensu: 1000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230403, tensu: 3000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 27000, 1000, 0, 2000, 0, 0, 0, 0);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 1, 1000, 0, 0, 0, 0, -1000, -1000);
            //4日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230404, tensu: 3000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 27000, 3000, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
        }

        //後期低所+マル長(10000)+54難病(10000)
        [Test]
        public void T016_02_06_KoukiMarucyo_k54()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futanCalcVm.SystemConf.ChokiFutan = 0;
            futanCalcVm.SystemConf.ChokiDateRange = 0;

            newPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230401,
                birthDay: 19350101
            );
            newPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 4
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            newPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            newHokenPattern(1, 1, 2, 0, 0);
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目 15併用分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230401, tensu: 11000, hokenPid: 1);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 11000, 99000, 3000, 0, 0, 0, 0, 8000, 8000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 保険単独分
            newRaiinTensu(futanVm: futanCalcVm, sinDate: 20230402, tensu: 1000, hokenPid: 2);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);
            AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 1000, 0, 0, 0, 0, -1000, -1000);
        }
    }
}
