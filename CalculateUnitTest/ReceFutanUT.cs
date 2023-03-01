using EmrCalculateApi.Futan.Models;
using EmrCalculateApi.Futan.ViewModels;
using EmrCalculateApi.Interface;
using Entity.Tenant;
using Moq;

namespace CalculateUnitTest
{
    public class ReceFutanUT : BaseUT
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

        private void SetHokenPattern(FutancalcViewModel futancalc,
            List<PtHokenPatternModel> hokenPatterns,
            List<PtHokenInfModel> ptHokens,
            List<PtKohiModel> ptKohis)
        {
            //保険組合せ
            futancalc.HokenPattern = hokenPatterns.Find(x => x.HokenPid == futancalc.RaiinTensu.HokenPid);
            //主保険
            futancalc.PtHoken = ptHokens.Find(x => x.HokenId == futancalc.HokenPattern.HokenId);
            //公費
            futancalc.PtKohis.Clear();
            for (int i = 1; i <= 4; i++)
            {
                int kohiId = (i == 1) ? futancalc.HokenPattern.Kohi1Id :
                    (i == 2) ? futancalc.HokenPattern.Kohi2Id :
                    (i == 3) ? futancalc.HokenPattern.Kohi3Id :
                    futancalc.HokenPattern.Kohi4Id;

                if (kohiId == 0 || ptKohis.Find(x => x.HokenId == kohiId) == null)
                {
                    break;
                }
                futancalc.PtKohis.Add(ptKohis.Find(x => x.HokenId == kohiId));
            }
        }

        [Test]
        public void T001_SyahoH3008_KoreiJirei31()
        {
            var futanCalcVM = newFutanCalcVM();
            futanCalcVM.RaiinTensu = new RaiinTensuModel()
            {
                HpId = 1,
                PtId = 1,
                SinDate = 20180801,
                RaiinNo = 1001,
                HokenPid = 1,
                OyaRaiinNo = 1001,
                SinStartTime = "120000",
                Tensu = 87300
            };

            //患者情報
            futanCalcVM.PtInf = new PtInfModel(
                new PtInf()
                {
                    PtId = 1,
                    Birthday = 19450101
                },
                futanCalcVM.RaiinTensu.SinDate
            );

            //主保険
            List<PtHokenInfModel> wrkHokens = new List<PtHokenInfModel>();
            wrkHokens.Add(
                new PtHokenInfModel(
                    new PtHokenInf()
                    {
                        PtId = 1,
                        HokenId = 1,
                        HokenNo = 1,
                        HokenEdaNo = 0,
                        HonkeKbn = 1,
                        Houbetu = "01",
                        KogakuKbn = 26
                    },
                    new HokenMst()
                    {
                        HokenSbtKbn = 1,
                        EnTen = 10,
                        FutanKbn = 1,
                        FutanRate = 30
                    },
                    27
                )
            );
            //公費
            List<PtKohiModel> wrkKohis = new List<PtKohiModel>();

            //保険組合せ
            List<PtHokenPatternModel> wrkHokenPattern = new List<PtHokenPatternModel>();
            wrkHokenPattern.Add(
                new PtHokenPatternModel(
                    new PtHokenPattern()
                    {
                        HokenPid = 1,
                        HokenKbn = 1,
                        HokenSbtCd = 111,
                        HokenId = 1
                    }
                )
            );

            //保険情報設定
            SetHokenPattern(futanCalcVM, wrkHokenPattern, wrkHokens, wrkKohis);

            futanCalcVM.KaikeiDetails = new List<KaikeiDetailModel>();
            futanCalcVM.KaikeiAdjustDetails = new List<KaikeiDetailModel>();
            futanCalcVM.LimitListOthers = new List<LimitListInfModel>();
            futanCalcVM.LimitCntListOthers = new List<LimitCntListInfModel>();

            //計算
            futanCalcVM.DetailCalculate(false);

            Assert.AreEqual(87300, futanCalcVM.KaikeiDetail.Tensu);
            Assert.AreEqual(873000, futanCalcVM.KaikeiDetail.TotalIryohi);
            Assert.AreEqual(611100, futanCalcVM.KaikeiDetail.HokenFutan);
            Assert.AreEqual(8990, futanCalcVM.KaikeiDetail.KogakuFutan);
            Assert.AreEqual(252910, futanCalcVM.KaikeiDetail.IchibuFutan);
            Assert.AreEqual(252910, futanCalcVM.KaikeiDetail.PtFutan);


            //ToDo DuongLe: Need to uncomment below code to execute test

            //futanCalcVM.KaikeiDetails.Add(futanCalcVM.KaikeiDetail);

            ////レセプト
            //ReceFutanViewModel receFutanVM = new ReceFutanViewModel();
            //SetReceFutanData(futanCalcVM, wrkKohis, receFutanVM);

            ////集計
            //receFutanVM.ReceCalculate(futanCalcVM.RaiinTensu.SinDate / 100);

            //Assert.AreEqual(87300, receFutanVM.ReceInfs[0].HokenReceTensu);
            //Assert.AreEqual(252910, receFutanVM.ReceInfs[0].HokenReceFutan);
            //Assert.AreEqual(true, receFutanVM.ReceInfs[0].Tokki.Contains("26"));
            //Assert.AreEqual("1110", receFutanVM.ReceInfs[0].ReceSbt);
        }
    }
}
