using EmrCalculateApi.Constants;
using EmrCalculateApi.Futan.Models;
using EmrCalculateApi.Futan.ViewModels;
using EmrCalculateApi.Interface;
using Entity.Tenant;
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
        private long newRaiinNo(FutancalcViewModel futanCalcVm)
        {

            if (futanCalcVm.KaikeiDetails.Count == 0)
            {
                seqRaiinNo = 0;
            }
            seqRaiinNo++;

            return seqRaiinNo;
        }

        private void newRaiinTensu(FutancalcViewModel futanCalcVm,
            int sinDate, long raiinNo, string sinStartTime, int hokenPid, int tensu, bool jituNisu = true)
        {
            if (futanCalcVm.KaikeiDetail.RaiinNo > 0)
            {
                futanCalcVm.KaikeiDetails.Add(futanCalcVm.KaikeiDetail);
                futanCalcVm.KaikeiDetail = new KaikeiDetailModel(new KaikeiDetail());
            }

            futanCalcVm.RaiinTensu = new RaiinTensuModel()
            {
                HpId = 1,
                PtId = 1,
                SinDate = sinDate,
                RaiinNo = raiinNo == 0 ? newRaiinNo(futanCalcVm) : raiinNo,
                OyaRaiinNo = seqRaiinNo,
                SinStartTime = sinStartTime == string.Empty ? "120000" : sinStartTime,
                HokenPid = hokenPid,
                Tensu = tensu,
                JituNisu = jituNisu
            };

            #region 保険組合せの設定
            //保険組合せ
            var hokenPattern = hokenPatterns.Find(x => x.HokenPid == futanCalcVm.RaiinTensu.HokenPid);
            if (hokenPattern != null) futanCalcVm.HokenPattern = hokenPattern;
            //主保険
            var ptHoken = ptHokens.Find(x => x.HokenId == futanCalcVm.HokenPattern.HokenId);
            if (ptHoken != null) futanCalcVm.PtHoken = ptHoken;
            //公費
            futanCalcVm.PtKohis.Clear();
            for (int i = 1; i <= 4; i++)
            {
                int kohiId =
                    (i == 1) ? futanCalcVm.HokenPattern.Kohi1Id :
                    (i == 2) ? futanCalcVm.HokenPattern.Kohi2Id :
                    (i == 3) ? futanCalcVm.HokenPattern.Kohi3Id :
                    futanCalcVm.HokenPattern.Kohi4Id;

                if (kohiId == 0 || ptKohis.Find(x => x.HokenId == kohiId) == null)
                {
                    break;
                }

                var ptKohi = ptKohis.Find(x => x.HokenId == kohiId);
                if (ptKohi != null) futanCalcVm.PtKohis.Add(ptKohi);
            }
            #endregion
        }

        private void newPtInf(FutancalcViewModel futanCalcVm,
            int sinDate, int birthDay)
        {
            futanCalcVm.PtInf = new PtInfModel(
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

            futanCalcVm.KaikeiDetails.Clear();
            futanCalcVm.KaikeiAdjustDetails.Clear();
            futanCalcVm.LimitListOthers.Clear();
            futanCalcVm.LimitCntListOthers.Clear();
            #endregion
        }

        private List<PtHokenInfModel> ptHokens = new List<PtHokenInfModel>();
        private void newPtHoken(int prefNo, int honkeKbn, string houbetu, int kogakuKbn)
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
                            1,
                        HokenEdaNo = 0,
                        HonkeKbn = honkeKbn,
                        Houbetu = houbetu,
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
        private void newPtKohi(int prefNo, int hokenSbtKbn, int futanKbn, int futanRate,
            int countKbn, int dayLimitFutan, int monthLimitFutan, int monthLimitCount, int calcSpKbn, int kogakuTekiyo)
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
                        HokenSbtKbn = hokenSbtKbn,
                        FutanKbn = futanKbn,
                        FutanRate = futanRate,
                        CountKbn = countKbn,
                        DayLimitFutan = dayLimitFutan,
                        MonthLimitFutan = monthLimitFutan,
                        MonthLimitCount = monthLimitCount,
                        CalcSpKbn = calcSpKbn,
                        KogakuTekiyo = kogakuTekiyo
                    },
                    false,
                    "99999"
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

        //70歳未満一般
        [Test]
        public void T001_Under70Ippan()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            newPtInf
            (
                futanCalcVm: futanCalcVm,
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
            newRaiinTensu(futanCalcVm, 20181001, 0, string.Empty, 1, 1000);
            futanCalcVm.DetailCalculate(false);

            //結果
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 0, 0, 0, 0, 3000, 3000);
        }

        //茨城88（特殊計算: 実日数をカウントしない日は負担なし、回数カウントもしない）
        [Test]
        public void T005_P08001_88()
        {
            var futanCalcVm = newFutanCalcVM();
            int prefNo = PrefCode.Ibaraki;

            newPtInf
            (
                futanCalcVm: futanCalcVm,
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
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                futanRate: 0,
                countKbn: 3,
                dayLimitFutan: 600,
                monthLimitFutan: 0,
                monthLimitCount: 2,
                calcSpKbn: 1,
                kogakuTekiyo: 11
            );
            newHokenPattern(1, 1, 0, 0, 0);

            //1日目
            newRaiinTensu(futanCalcVm, 20210401, 0, string.Empty, 1, 1000, true);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1400, 0, 0, 0, 600, 600);
            //2日目
            newRaiinTensu(futanCalcVm, 20210402, 0, string.Empty, 1, 1000, false);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 2000, 0, 0, 0, 0, 0);
            //3日目
            newRaiinTensu(futanCalcVm, 20210403, 0, string.Empty, 1, 1000, true);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 1400, 0, 0, 0, 600, 600);
            //4日目
            newRaiinTensu(futanCalcVm, 20210404, 0, string.Empty, 1, 1000, true);
            futanCalcVm.DetailCalculate(false);
            AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 2000, 0, 0, 0, 0, 0);
        }
    } 
}
