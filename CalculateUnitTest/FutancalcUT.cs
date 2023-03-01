using EmrCalculateApi.Constants;
using EmrCalculateApi.Futan.Models;
using EmrCalculateApi.Futan.ViewModels;
using EmrCalculateApi.Interface;
using Entity.Tenant;
using Helper.Constants;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
using Moq;
using System.Linq.Dynamic.Core.Tokenizer;

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
            if (futanVm.KaikeiDetail.RaiinNo > 0)
            {
                futanVm.KaikeiDetails.Add(futanVm.KaikeiDetail);
                futanVm.KaikeiDetail = new KaikeiDetailModel(new KaikeiDetail());
            }

            futanVm.RaiinTensu = new RaiinTensuModel()
            {
                HpId = 1,
                PtId = 1,
                SinDate = sinDate,
                RaiinNo = newRaiin ? newRaiinNo(futanVm) : seqRaiinNo,
                OyaRaiinNo = seqRaiinNo,
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
        private void newPtHoken(int prefNo, int honkeKbn, string houbetu, int kogakuKbn, string hokensyaNo = "")
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
                        KogakuKbn = kogakuKbn
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
            int monthSpLimit = 0, int calcSpKbn = 0, int isLimitList = 0, int isLimitListSum = 0, bool exceptHokensya = false, int futanYusen = 0)
        {
            ptKohis.Add(
                new PtKohiModel(
                    new PtKohi()
                    {
                        HokenId = ptKohis.Count + 1,
                        PrefNo = prefNo
                    },
                    new HokenMst()
                    {
                        Houbetu = houbetu,
                        HokenSbtKbn = hokenSbtKbn,
                        FutanKbn = futanKbn,
                        FutanRate = futanRate,
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
                    "99999"
                )
            );
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

        private void AssertEqualTo(KaikeiDetailModel kaikeiDetail,
            int tensu, int hokenFutan, int kogakuFutan,
            int kohi1Futan, int kohi2Futan, int kohi3Futan, int kohi4Futan,
            int ichibuFutan, int ptFutan)
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
                birthDay:19850101
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
                hokenSbtKbn: HokenSbtKbn.Bunten,
                futanKbn: 1,
                futanRate: 10,
                monthLimitFutan: 2500,
                isLimitList: 1,
                kogakuTekiyo: 0
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
                hokenSbtKbn: HokenSbtKbn.Bunten,
                futanKbn: 1,
                futanRate: 10,
                monthLimitFutan: 2500,
                isLimitList: 1,
                kogakuTekiyo: 0
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
                hokenSbtKbn: HokenSbtKbn.Bunten,
                futanKbn: 1,
                futanRate: 10,
                monthLimitFutan: 2500,
                isLimitList: 1,
                kogakuTekiyo: 0
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
                hokenSbtKbn: HokenSbtKbn.Bunten,
                futanKbn: 0,
                isLimitList: 1,
                isLimitListSum: 1
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
                hokenSbtKbn: HokenSbtKbn.Bunten,
                futanKbn: 1,
                futanRate: 20,
                monthLimitFutan: 2500,
                isLimitList: 1,
                isLimitListSum: 1
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
                hokenSbtKbn: HokenSbtKbn.Bunten,
                futanKbn: 1,
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
                prefNo: prefNo,
                houbetu: "15",
                hokenSbtKbn: HokenSbtKbn.Bunten,
                futanKbn: 1,
                futanRate: 10,
                monthLimitFutan: 5000,
                isLimitList: 1,
                isLimitListSum: 1,
                kogakuTekiyo: 0
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
                prefNo: prefNo,
                houbetu: "52",
                hokenSbtKbn: HokenSbtKbn.Bunten,
                futanKbn: 1,
                futanRate: 20,
                monthLimitFutan: 10000,
                isLimitList: 1,
                isLimitListSum: 0,
                kogakuTekiyo: 11
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
                prefNo: prefNo,
                houbetu: "102",
                hokenSbtKbn: HokenSbtKbn.Choki,
                futanKbn: 1,
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
                prefNo: prefNo,
                houbetu: "102",
                hokenSbtKbn: HokenSbtKbn.Choki,
                futanKbn: 1,
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
    } 
}
