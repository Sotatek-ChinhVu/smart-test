using CalculateService.Constants;
using CalculateService.Futan.Models;
using CalculateService.Futan.ViewModels;
using CalculateService.Interface;
using CalculateService.ReceFutan.Models;
using CalculateService.ReceFutan.ViewModels;
using Castle.Core.Logging;
using Domain.Models.CalculateModel;
using Entity.Tenant;
using Helper.Constants;
using Helper.Messaging;
using Moq;
using NuGet.Frameworks;
using System.Linq.Dynamic.Core.Tokenizer;

namespace CalculateUnitTest
{
    public class ReceFutanUT : BaseUT
    {
        public class InitMessenger : IMessenger
        {
            public void Deregister<T>(object receiver, Action<T> action)
            {
                throw new NotImplementedException();
            }

            public void DeregisterAll()
            {
                throw new NotImplementedException();
            }

            public void Register<T>(object receiver, Action<T> action)
            {
                throw new NotImplementedException();
            }

            public void Send<T>(T message)
            {
                throw new NotImplementedException();
            }

            public Task<CallbackMessageResult<T>> SendAsync<T>(CallbackMessage<T> message)
            {
                throw new NotImplementedException();
            }
        }

        private CalculateService.ReceFutan.Models.ReceInfModel runReceCalculate(FutancalcUT futancalcUT, FutancalcViewModel futanVm, int prefNo,
            int chokiTokki = 0, int receKyufuKisai = 0, int receKyufuKisai2 = 0, double jibaiRousaiRate = 0)
        {
            int hpId = 1;
            futancalcUT.AddCalcResult(futanVm);

            var mockSystemConfigProvider = new Mock<ISystemConfigProvider>();
            mockSystemConfigProvider.Setup(repo => repo.GetChokiTokki()).Returns(chokiTokki);
            mockSystemConfigProvider.Setup(repo => repo.GetReceKyufuKisai()).Returns(receKyufuKisai);
            mockSystemConfigProvider.Setup(repo => repo.GetReceKyufuKisai2()).Returns(receKyufuKisai2);
            mockSystemConfigProvider.Setup(repo => repo.GetJibaiRousaiRate()).Returns(jibaiRousaiRate);

            var mockLogger = new Mock<IEmrLogger>();
            var messenger = new InitMessenger();

            var kogakuLimitModels = new List<CalculateService.ReceFutan.Models.KogakuLimitModel>();
            foreach (var kogakuLimit in futancalcUT.KogakuLimitModels)
            {
                kogakuLimitModels.Add(new CalculateService.ReceFutan.Models.KogakuLimitModel(kogakuLimit.KogakuLimit));
            }

            ReceFutanViewModel receFutanVm = new ReceFutanViewModel(TenantProvider, mockSystemConfigProvider.Object, mockLogger.Object, messenger, new List<TokkiMstModel>(), kogakuLimitModels);

            foreach (var wrkDetail in futanVm.KaikeiDetails)
            {
                receFutanVm.KaikeiDetails.Add(new CalculateService.ReceFutan.Models.KaikeiDetailModel(wrkDetail.KaikeiDetail, 0, 0, 0));
            }
            foreach (var wrkDetail in futanVm.KaikeiAdjustDetails)
            {
                receFutanVm.KaikeiDetails.Add(new CalculateService.ReceFutan.Models.KaikeiDetailModel(wrkDetail.KaikeiDetail, 0, 0, 0));
            }
            receFutanVm.PtHoken = new CalculateService.ReceFutan.Models.PtHokenInfModel(futanVm.PtHoken.PtHokenInf, futanVm.PtHoken.HokenMst, 0, 0);
            receFutanVm.PtInf = new CalculateService.ReceFutan.Models.PtInfModel(futanVm.PtInf.PtInf);
            receFutanVm.PtKohis = new List<CalculateService.ReceFutan.Models.PtKohiModel>();
            foreach (var wrkKohi in futancalcUT.PtKohis)
            {
                receFutanVm.PtKohis.Add(new CalculateService.ReceFutan.Models.PtKohiModel(wrkKohi.PtKohi, wrkKohi.HokenMst, wrkKohi.ExceptHokensya, receFutanVm.PtHoken.PtHokenInf.HokenKbn));
            }
            receFutanVm.HpPrefCd = prefNo;

            //レセ集計
            int seikyuYm = futanVm.RaiinTensu.SinDate / 100;
            receFutanVm.ReceCalculate(hpId, seikyuYm);

            return receFutanVm.ReceInfs.First();
        }

        private void AssertEqualToReceKisai(CalculateService.ReceFutan.Models.ReceInfModel receInf,
            bool kohi1ReceKisai, bool kohi2ReceKisai, bool kohi3ReceKisai, bool kohi4ReceKisai)
        {
            Assert.That(receInf.Kohi1ReceKisai, Is.EqualTo(kohi1ReceKisai));
            Assert.That(receInf.Kohi2ReceKisai, Is.EqualTo(kohi2ReceKisai));
            Assert.That(receInf.Kohi3ReceKisai, Is.EqualTo(kohi3ReceKisai));
            Assert.That(receInf.Kohi4ReceKisai, Is.EqualTo(kohi4ReceKisai));
        }

        private void AssertEqualToTensu(CalculateService.ReceFutan.Models.ReceInfModel receInf,
            int? hokenTensu, int? kohi1Tensu, int? kohi2Tensu, int? kohi3Tensu, int? kohi4Tensu)
        {
            Assert.That(receInf.HokenReceTensu, Is.EqualTo(hokenTensu));

            if (receInf.Kohi1ReceKisai) Assert.That(receInf.Kohi1ReceTensu, Is.EqualTo(kohi1Tensu));
            if (receInf.Kohi2ReceKisai) Assert.That(receInf.Kohi2ReceTensu, Is.EqualTo(kohi2Tensu));
            if (receInf.Kohi3ReceKisai) Assert.That(receInf.Kohi3ReceTensu, Is.EqualTo(kohi3Tensu));
            if (receInf.Kohi4ReceKisai) Assert.That(receInf.Kohi4ReceTensu, Is.EqualTo(kohi4Tensu));
        }

        private void AssertEqualToKyufu(CalculateService.ReceFutan.Models.ReceInfModel receInf,
            int? kohi1Kyufu, int? kohi2Kyufu, int? kohi3Kyufu, int? kohi4Kyufu)
        {
            if (receInf.Kohi1ReceKisai) Assert.That(receInf.Kohi1ReceKyufu, Is.EqualTo(kohi1Kyufu));
            if (receInf.Kohi2ReceKisai) Assert.That(receInf.Kohi2ReceKyufu, Is.EqualTo(kohi2Kyufu));
            if (receInf.Kohi3ReceKisai) Assert.That(receInf.Kohi3ReceKyufu, Is.EqualTo(kohi3Kyufu));
            if (receInf.Kohi4ReceKisai) Assert.That(receInf.Kohi4ReceKyufu, Is.EqualTo(kohi4Kyufu));
        }

        private void AssertEqualToFutan(CalculateService.ReceFutan.Models.ReceInfModel receInf,
            int? hokenFutan, int? kohi1Futan, int? kohi2Futan, int? kohi3Futan, int? kohi4Futan)
        {
            Assert.That(receInf.HokenReceFutan, Is.EqualTo(hokenFutan));

            if (receInf.Kohi1ReceKisai) Assert.That(receInf.Kohi1ReceFutan, Is.EqualTo(kohi1Futan));
            if (receInf.Kohi2ReceKisai) Assert.That(receInf.Kohi2ReceFutan, Is.EqualTo(kohi2Futan));
            if (receInf.Kohi3ReceKisai) Assert.That(receInf.Kohi3ReceFutan, Is.EqualTo(kohi3Futan));
            if (receInf.Kohi4ReceKisai) Assert.That(receInf.Kohi4ReceFutan, Is.EqualTo(kohi4Futan));
        }

        //----------------------------------------------------------------------------------------------------
        // T001_高額療養費の自己負担限度額の見直しに係る請求計算事例（高齢受給者） 平成30年8月
        //----------------------------------------------------------------------------------------------------

        /// <summary>
        /// 高齢受給者
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei31()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 26
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 87300);

            //結果
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 87300, 611100, 8990, 0, 0, 0, 0, 252910, 252910);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 87300, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 252910, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("26"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1110"));
        }

        /// <summary>
        /// 高齢受給者（75歳到達月）（多数回該当）
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei32()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 26,
                tasukaiYm: 201808,
                tokureiYm1: 201808
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 24800);

            //結果
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 24800, 173600, 4350, 0, 0, 0, 0, 70050, 70050);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 24800, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 70050, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("26"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1110"));
        }

        /// <summary>
        /// 高齢受給者（多数回該当）
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei33()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 27,
                tasukaiYm: 201808
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 34600);

            //結果
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 34600, 242200, 10800, 0, 0, 0, 0, 93000, 93000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 34600, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 93000, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("27"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1110"));
        }

        /// <summary>
        /// 高齢受給者
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei34()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 30300);

            //結果
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 30300, 212100, 10440, 0, 0, 0, 0, 80460, 80460);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 30300, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 80460, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1110"));
        }

        /// <summary>
        /// 高齢受給者・難病医療（多数回該当）
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei35()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 26,
                tasukaiYm: 201808
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 13500, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 13500, 94500, 0, 30500, 0, 0, 0, 10000, 10000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 47000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 47000, 329000, 900, 0, 0, 0, 0, 140100, 140100);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 10000, 0, 0, 0, 0, -10000, -10000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 60500, 13500, null, null, null);
            AssertEqualToKyufu(retReceInf, 40500, null, null, null);
            AssertEqualToFutan(retReceInf, 170600, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("26"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1120"));
        }

        /// <summary>
        /// 高齢受給者・難病医療（多数回該当）
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei36()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 27,
                tasukaiYm: 201808
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 3100, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 3100, 21700, 0, 4300, 0, 0, 0, 5000, 5000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 34900, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 34900, 244300, 11700, 0, 0, 0, 0, 93000, 93000);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 5000, 0, 0, 0, 0, -5000, -5000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 38000, 3100, null, null, null);
            AssertEqualToKyufu(retReceInf, 9300, null, null, null);
            AssertEqualToFutan(retReceInf, 97300, 5000, null, null, null);
            Assert.That(retReceInf.TokkiContains("27"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1120"));
        }

        /// <summary>
        /// 高齢受給者・難病医療
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei37()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 31100, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 31100, 217700, 12760, 70540, 0, 0, 0, 10000, 10000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 31100, 31100, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 80540, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1120"));
        }

        /// <summary>
        /// 高齢受給者・難病医療（多数回該当）
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei38()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28,
                tasukaiYm: 201808
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 1800, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1800, 12600, 0, 1800, 0, 0, 0, 3600, 3600);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 18600, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 18600, 130200, 11400, 0, 0, 0, 0, 44400, 44400);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 3600, 0, 0, 0, 0, -3600, -3600);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 20400, 1800, null, null, null);
            AssertEqualToKyufu(retReceInf, 5400, null, null, null);
            AssertEqualToFutan(retReceInf, 46200, 3600, null, null, null);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1120"));
        }

        /// <summary>
        /// 高齢受給者
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei39()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 11000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 11000, 88000, 4000, 0, 0, 0, 0, 18000, 18000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 11000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1118"));
        }

        /// <summary>
        /// 高齢受給者（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei40()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19440101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 9900);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 9900, 89100, 0, 0, 0, 0, 0, 9900, 9900);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 9900, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 9900, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1118"));
        }

        /// <summary>
        /// 高齢受給者（75歳到達月）（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei41()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190301,
                birthDay: 19440310
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0,
                tokureiYm1: 201903
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190301, tensu: 6300);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 6300, 56700, 0, 0, 0, 0, 0, 6300, 6300);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 6300, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 6300, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1118"));
        }

        /// <summary>
        /// 高齢受給者（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei42()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190301,
                birthDay: 19440310
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 4
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190301, tensu: 4600);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4600, 41400, 0, 0, 0, 0, 0, 4600, 4600);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 4600, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 4600, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("30"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1118"));
        }

        /// <summary>
        /// 高齢受給者
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei43()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 5
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 5100);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5100, 40800, 2200, 0, 0, 0, 0, 8000, 8000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 5100, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 8000, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("30"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1118"));
        }

        /// <summary>
        /// 高齢受給者・難病医療
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei44()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 9500, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 9500, 76000, 1000, 13000, 0, 0, 0, 5000, 5000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 9500, 9500, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, 5000, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・難病医療
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei45()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 5500, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5500, 44000, 0, 6000, 0, 0, 0, 5000, 5000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 10000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 2000, 0, 0, 0, 0, 18000, 18000);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 5000, 0, 0, 0, 0, -5000, -5000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 15500, 5500, null, null, null);
            AssertEqualToKyufu(retReceInf, 11000, null, null, null);
            AssertEqualToFutan(retReceInf, 24000, 5000, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・難病医療（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei46()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190301,
                birthDay: 19440310
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190301, tensu: 9800, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 9800, 88200, 0, 4800, 0, 0, 0, 5000, 5000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190302, tensu: 11800, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 11800, 106200, 0, 0, 0, 0, 0, 11800, 11800);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 21600, 9800, null, null, null);
            AssertEqualToKyufu(retReceInf, 9800, null, null, null);
            AssertEqualToFutan(retReceInf, 21600, 5000, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・難病医療（75歳到達月）
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei47()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0,
                tokureiYm1: 201808
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 4800);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4800, 38400, 600, 4000, 0, 0, 0, 5000, 5000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 4800, 4800, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 9000, 5000, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・難病医療
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei48()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 12000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 12000, 96000, 6000, 13000, 0, 0, 0, 5000, 5000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 1200, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1200, 9600, 0, 0, 0, 0, 0, 2400, 2400);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 13200, 12000, null, null, null);
            AssertEqualToKyufu(retReceInf, 18000, null, null, null);
            AssertEqualToFutan(retReceInf, 20400, 5000, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・難病医療
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei49()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 12400, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 12400, 99200, 6800, 13000, 0, 0, 0, 5000, 5000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 9600, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 9600, 76800, 1200, 0, 0, 0, 0, 18000, 18000);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 5000, 0, 0, 0, 0, -5000, -5000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 22000, 12400, null, null, null);
            AssertEqualToKyufu(retReceInf, 18000, null, null, null);
            AssertEqualToFutan(retReceInf, 31000, 5000, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・難病医療（75歳到達月）
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei50()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0,
                tokureiYm1: 201808
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 5100, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5100, 40800, 1200, 4000, 0, 0, 0, 5000, 5000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 7400, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 7400, 59200, 5800, 0, 0, 0, 0, 9000, 9000);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 5000, 0, 0, 0, 0, -5000, -5000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 12500, 5100, null, null, null);
            AssertEqualToKyufu(retReceInf, 9000, null, null, null);
            AssertEqualToFutan(retReceInf, 13000, 5000, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・更生医療
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei51()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 20000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 10800, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10800, 86400, 3600, 7200, 0, 0, 0, 10800, 10800);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 10800, 10800, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, 10800, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・更生医療
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei52()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 20000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 16400, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 16400, 131200, 14800, 1600, 0, 0, 0, 16400, 16400);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 11400, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 11400, 91200, 4800, 0, 0, 0, 0, 18000, 18000);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 16400, 0, 0, 0, 0, -16400, -16400);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 27800, 16400, null, null, null);
            AssertEqualToKyufu(retReceInf, 18000, null, null, null);
            AssertEqualToFutan(retReceInf, 19600, 16400, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・精神通院（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei53()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190301,
                birthDay: 19440310
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 20000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 21併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190301, tensu: 4300, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4300, 38700, 0, 0, 0, 0, 0, 4300, 4300);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190302, tensu: 9600, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 9600, 86400, 0, 0, 0, 0, 0, 9600, 9600);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 13900, 4300, null, null, null);
            AssertEqualToKyufu(retReceInf, 4300, null, null, null);
            AssertEqualToFutan(retReceInf, 13900, 4300, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・精神通院（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei54()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190301,
                birthDay: 19440310
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 20000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 21併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190301, tensu: 9500, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 9500, 85500, 0, 0, 0, 0, 0, 9500, 9500);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190302, tensu: 400, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 400, 3600, 0, 0, 0, 0, 0, 400, 400);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 9900, 9500, null, null, null);
            AssertEqualToKyufu(retReceInf, 9500, null, null, null);
            AssertEqualToFutan(retReceInf, 9900, 9500, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・精神通院（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei55()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190301,
                birthDay: 19440310
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 20000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 21併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190301, tensu: 9300, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 9300, 83700, 0, 0, 0, 0, 0, 9300, 9300);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190302, tensu: 5100, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5100, 45900, 0, 0, 0, 0, 0, 5100, 5100);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 14400, 9300, null, null, null);
            AssertEqualToKyufu(retReceInf, 9300, null, null, null);
            AssertEqualToFutan(retReceInf, 14400, 9300, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・難病医療・生活保護
        /// </summary>
        [Test]
        public void T001_SyahoH3008_KoreiJirei56()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 5
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "12",
                monthLimitFutan: 0
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 4800, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4800, 38400, 1600, 8000, 0, 0, 0, 0, 0);
            //2日目 12併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 4500, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4500, 36000, 1000, 8000, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 9300, 4800, 4500, null, null);
            AssertEqualToKyufu(retReceInf, 8000, 8000, null, null);
            AssertEqualToFutan(retReceInf, 16000, 0, 0, null, null);  //Kohi2ReceFutan: 事例ではnullだがこれまでの請求実績からマスタ設定に関わらず0にする
            Assert.That(retReceInf.TokkiContains("30"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1138"));
        }

        //----------------------------------------------------------------------------------------------------
        // T002_高額療養費の自己負担限度額の見直しに係る請求計算事例（70歳未満） 平成27年1月
        //----------------------------------------------------------------------------------------------------
        /// <summary>
        /// 本人入院外（標準報酬月額53万～79万円）・公費（結核患者の適正医療）
        /// ※併用部分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T002_SyahoH2701_Under70Jirei18()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19900410
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 27
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "10",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 10併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 85000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 85000, 595000, 169070, 43430, 0, 0, 0, 42500, 42500);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 5000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 0, 0, 0, 0, 15000, 15000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 90000, 85000, null, null, null);
            AssertEqualToKyufu(retReceInf, 85930, null, null, null);
            AssertEqualToFutan(retReceInf, 100930, 57500, null, null, null);
            Assert.That(retReceInf.TokkiContains("27"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（標準報酬月額53万～79万円）・公費（結核患者の適正医療）
        /// ※医保単独部分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T002_SyahoH2701_Under70Jirei19()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19900410
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 27
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "10",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 10併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 5000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 12500, 0, 0, 0, 2500, 2500);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 95000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 95000, 665000, 113680, 0, 0, 0, 0, 171320, 171320);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 100000, 5000, null, null, null);
            AssertEqualToKyufu(retReceInf, 15000, null, null, null);
            AssertEqualToFutan(retReceInf, 186320, 173820, null, null, null);
            Assert.That(retReceInf.TokkiContains("27"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（標準報酬月額53万～79万円）・公費（結核患者の適正医療）
        /// ※医保単独部分で高額療養費が発生する場合（合算対象）
        /// </summary>
        [Test]
        public void T002_SyahoH2701_Under70Jirei20()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19900410
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 27
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "10",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 10併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 10000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 70000, 0, 25000, 0, 0, 0, 5000, 5000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 110000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 110000, 770000, 157180, 0, 0, 0, 0, 172820, 172820);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 4000, 0, 0, 0, 0, -4000, -4000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 120000, 10000, null, null, null);
            AssertEqualToKyufu(retReceInf, 30000, null, null, null);
            AssertEqualToFutan(retReceInf, 198820, 173820, null, null, null);
            Assert.That(retReceInf.TokkiContains("27"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（標準報酬月額53万～79万円）・公費（結核患者の適正医療）
        /// ※医療保険部分及び併用部分で高額療養費が発生する場合（合算対象）
        /// </summary>
        [Test]
        public void T002_SyahoH2701_Under70Jirei21()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19900410
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 27
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "10",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 10併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 85000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 85000, 595000, 169070, 43430, 0, 0, 0, 42500, 42500);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 95000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 95000, 665000, 113680, 0, 0, 0, 0, 171320, 171320);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 34000, 0, 0, 0, 0, -34000, -34000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 180000, 85000, null, null, null);
            AssertEqualToKyufu(retReceInf, 85930, null, null, null);
            AssertEqualToFutan(retReceInf, 223250, 179820, null, null, null);
            Assert.That(retReceInf.TokkiContains("27"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        //----------------------------------------------------------------------------------------------------
        // T003_外来高額療養費の現物給付化に係る計算事例集（本人） 平成24年4月
        //----------------------------------------------------------------------------------------------------

        /// <summary>
        /// 本人入院外（一般所得）
        /// ※高額療養費が発生しない場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei01()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 18
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 4000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 28000, 0, 0, 0, 0, 0, 12000, 12000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 4000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, null, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1112"));
        }

        /// <summary>
        /// 本人入院外（一般所得）
        /// ※高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei02()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 18
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 30000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 210000, 9570, 0, 0, 0, 0, 80430, 80430);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 30000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 80430, null, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1112"));
        }

        /// <summary>
        /// 本人入院外（一般所得）（マル長）
        /// ※一般でマル長
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei03()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 18
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 28000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 28000, 196000, 74000, 0, 0, 0, 0, 10000, 10000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 28000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 10000, null, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1112"));
        }

        /// <summary>
        /// 本人入院外（一般所得）（マル長）
        /// ※特記事項に　02　長　のみ
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei04()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 28000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 28000, 196000, 74000, 0, 0, 0, 0, 10000, 10000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 28000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, null, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1112"));
        }

        /// <summary>
        /// 本人入院外（一般所得）（マル長）
        /// ※特記事項に　16　長　のみ
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei05()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 20000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 28000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 28000, 196000, 64000, 0, 0, 0, 0, 20000, 20000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 28000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, null, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.TokkiContains("16"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1112"));
        }

        /// <summary>
        /// 本人入院外（低所得）
        /// ※高額療養費が発生しない場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei06()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 5000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 0, 0, 0, 0, 15000, 15000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 5000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, null, null, null, null);
            //Assert.That(retReceInf.TokkiContains("19"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1112"));
        }

        /// <summary>
        /// 本人入院外（低所得）
        /// ※高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei07()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 13000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 13000, 91000, 3600, 0, 0, 0, 0, 35400, 35400);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 13000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 35400, null, null, null, null);
            //Assert.That(retReceInf.TokkiContains("19"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1112"));
        }

        /// <summary>
        /// 本人入院外（低所得）（マル長）
        /// ※低所でマル長
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei08()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 15000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 15000, 105000, 35000, 0, 0, 0, 0, 10000, 10000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 15000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 10000, null, null, null, null);
            //Assert.That(retReceInf.TokkiContains("19"), Is.True);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1112"));
        }

        /// <summary>
        /// 本人入院外（上位所得）
        /// ※高額療養費が発生しない場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei09()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 8000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 8000, 56000, 0, 0, 0, 0, 0, 24000, 24000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 8000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, null, null, null, null);
            //Assert.That(retReceInf.TokkiContains("17"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1112"));
        }

        /// <summary>
        /// 本人入院外（上位所得）
        /// ※高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei10()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 60000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 60000, 420000, 29000, 0, 0, 0, 0, 151000, 151000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 60000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 151000, null, null, null, null);
            //Assert.That(retReceInf.TokkiContains("17"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1112"));
        }

        /// <summary>
        /// 本人入院外（上位所得）（マル長）
        /// ※上位で長２
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei11()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 20000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 54000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 54000, 378000, 142000, 0, 0, 0, 0, 20000, 20000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 54000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 20000, null, null, null, null);
            //Assert.That(retReceInf.TokkiContains("17"), Is.True);
            Assert.That(retReceInf.TokkiContains("16"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1112"));
        }

        /// <summary>
        /// 本人入院外（一般所得）・公費（特定疾患）
        /// ※高額療養費が発生しない場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei12()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 18
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 5500
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 15000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 15000, 105000, 0, 39500, 0, 0, 0, 5500, 5500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 15000, 15000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 5500, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（一般所得）・公費（特定疾患）
        /// ※高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei13()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 18
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 5500
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 30000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 210000, 9570, 74930, 0, 0, 0, 5500, 5500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 30000, 30000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 80430, 5500, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（一般所得）・公費（特定疾患）
        /// ※高額療養費が発生しない場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei14()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 18
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 5500
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 6000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 42000, 0, 12500, 0, 0, 0, 5500, 5500);

            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 9000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 9000, 63000, 0, 0, 0, 0, 0, 27000, 27000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 15000, 6000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 5500, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（一般所得）・公費（特定疾患）
        /// ※医保単独部分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei15()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 18
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 5500
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 6000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 42000, 0, 12500, 0, 0, 0, 5500, 5500);

            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 32000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 32000, 224000, 15370, 0, 0, 0, 0, 80630, 80630);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 38000, 6000, null, null, null);
            AssertEqualToKyufu(retReceInf, 18000, null, null, null);
            AssertEqualToFutan(retReceInf, 98630, 5500, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（一般所得）・公費（特定疾患）
        /// ※併用部分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei16()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 18
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 9350
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 29000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 29000, 203000, 6670, 70980, 0, 0, 0, 9350, 9350);

            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 4000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 28000, 0, 0, 0, 0, 0, 12000, 12000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 33000, 29000, null, null, null);
            AssertEqualToKyufu(retReceInf, 80330, null, null, null);
            AssertEqualToFutan(retReceInf, 92330, 9350, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（一般所得）・公費（特定疾患）
        /// ※医保単独部分及び併用部分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei17()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 18
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 0
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 29000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 29000, 203000, 6670, 80330, 0, 0, 0, 0, 0);

            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 27000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 27000, 189000, 870, 0, 0, 0, 0, 80130, 80130);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 56000, 29000, null, null, null);
            AssertEqualToKyufu(retReceInf, 80330, null, null, null);
            AssertEqualToFutan(retReceInf, 160460, 0, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（一般所得）・公費（特定疾患）
        /// ※医保単独部分で高額療養費が発生する場合 (合算対象)
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei18()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 18
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 5500
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 9000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 9000, 63000, 0, 21500, 0, 0, 0, 5500, 5500);

            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 28000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 28000, 196000, 3770, 0, 0, 0, 0, 80230, 80230);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 4600, 0, 0, 0, 0, -4600, -4600);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 37000, 9000, null, null, null);
            AssertEqualToKyufu(retReceInf, 27000, null, null, null);
            AssertEqualToFutan(retReceInf, 102630, 5500, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（一般所得）・公費（特定疾患）
        /// ※医保単独部分及び併用部分で高額療養費が発生する場合 （合算対象）
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei19()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 18
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 5500
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 28000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 28000, 196000, 3770, 74730, 0, 0, 0, 5500, 5500);

            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 32000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 32000, 224000, 15370, 0, 0, 0, 0, 80630, 80630);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 2700, 0, 0, 0, 0, -2700, -2700);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 60000, 28000, null, null, null);
            AssertEqualToKyufu(retReceInf, 80230, null, null, null);
            AssertEqualToFutan(retReceInf, 158160, 5500, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（特定疾患）
        /// ※高額療養費が発生しない場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei20()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 2250
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 8000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 8000, 56000, 0, 21750, 0, 0, 0, 2250, 2250);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 8000, 8000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 2250, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（特定疾患）
        /// ※高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei21()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 2250
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 15000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 15000, 105000, 9600, 33150, 0, 0, 0, 2250, 2250);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 15000, 15000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 35400, 2250, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（特定疾患）
        /// ※高額療養費が発生しない場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei22()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 3450
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 6000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 42000, 0, 14550, 0, 0, 0, 3450, 3450);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 7000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 7000, 49000, 0, 0, 0, 0, 0, 21000, 21000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 13000, 6000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 3450, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（特定疾患）
        /// ※医保単独部分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei23()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 2250
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 4000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 28000, 0, 9750, 0, 0, 0, 2250, 2250);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 14000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 14000, 98000, 6600, 0, 0, 0, 0, 35400, 35400);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 18000, 4000, null, null, null);
            AssertEqualToKyufu(retReceInf, 12000, null, null, null);
            AssertEqualToFutan(retReceInf, 47400, 2250, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（一般所得）・公費（特定疾患）
        /// ※併用部分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei24()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 2250
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 14000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 14000, 98000, 6600, 33150, 0, 0, 0, 2250, 2250);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 6000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 42000, 0, 0, 0, 0, 0, 18000, 18000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 20000, 14000, null, null, null);
            AssertEqualToKyufu(retReceInf, 35400, null, null, null);
            AssertEqualToFutan(retReceInf, 53400, 2250, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（特定疾患）
        /// ※医保単独部分及び併用部分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei25()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 0
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 16000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 16000, 112000, 12600, 35400, 0, 0, 0, 0, 0);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 16000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 16000, 112000, 12600, 0, 0, 0, 0, 35400, 35400);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 32000, 16000, null, null, null);
            AssertEqualToKyufu(retReceInf, 35400, null, null, null);
            AssertEqualToFutan(retReceInf, 70800, 0, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（特定疾患）
        /// ※医保単独部分で高額療養費が発生する場合　（合算対象）
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei26()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 2250
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 8000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 8000, 56000, 0, 21750, 0, 0, 0, 2250, 2250);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 18000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 18000, 126000, 18600, 0, 0, 0, 0, 35400, 35400);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 2250, 0, 0, 0, 0, -2250, -2250);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 26000, 8000, null, null, null);
            AssertEqualToKyufu(retReceInf, 24000, null, null, null);
            AssertEqualToFutan(retReceInf, 57150, 2250, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（特定疾患）
        /// ※医保単独部分及び併用部分で高額療養費が発生する場合　（合算対象）
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei27()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 2250
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 14000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 14000, 98000, 6600, 33150, 0, 0, 0, 2250, 2250);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 13000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 13000, 91000, 3600, 0, 0, 0, 0, 35400, 35400);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 2250, 0, 0, 0, 0, -2250, -2250);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 27000, 14000, null, null, null);
            AssertEqualToKyufu(retReceInf, 35400, null, null, null);
            AssertEqualToFutan(retReceInf, 68550, 2250, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（上位所得）・公費（特定疾患）
        /// ※高額療養費が発生しない場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei28()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 11550
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 12000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 12000, 84000, 0, 24450, 0, 0, 0, 11550, 11550);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 12000, 12000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 11550, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（上位所得）・公費（特定疾患）
        /// ※高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei29()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 11550
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 60000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 60000, 420000, 29000, 139450, 0, 0, 0, 11550, 11550);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 60000, 60000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 151000, 11550, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（上位所得）・公費（特定疾患）
        /// ※高額療養費が発生しない場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei30()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 11550
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 6000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 42000, 0, 6450, 0, 0, 0, 11550, 11550);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 9000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 9000, 63000, 0, 0, 0, 0, 0, 27000, 27000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 15000, 6000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 11550, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（上位所得）・公費（特定疾患）
        /// ※医保単独部分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei31()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 11550
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 5000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 3450, 0, 0, 0, 11550, 11550);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 55000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 55000, 385000, 14500, 0, 0, 0, 0, 150500, 150500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 60000, 5000, null, null, null);
            AssertEqualToKyufu(retReceInf, 15000, null, null, null);
            AssertEqualToFutan(retReceInf, 165500, 11550, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（上位所得）・公費（特定疾患）
        /// ※併用部分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei32()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 11550
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 52000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 52000, 364000, 5800, 138650, 0, 0, 0, 11550, 11550);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 8000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 8000, 56000, 0, 0, 0, 0, 0, 24000, 24000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 60000, 52000, null, null, null);
            AssertEqualToKyufu(retReceInf, 150200, null, null, null);
            AssertEqualToFutan(retReceInf, 174200, 11550, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（上位所得）・公費（特定疾患）
        /// ※医保単独部分で高額療養費が発生する場合　（合算対象）
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei33()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 11550
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 12000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 12000, 84000, 0, 24450, 0, 0, 0, 11550, 11550);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 68000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 68000, 476000, 52200, 0, 0, 0, 0, 151800, 151800);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 10350, 0, 0, 0, 0, -10350, -10350);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 80000, 12000, null, null, null);
            AssertEqualToKyufu(retReceInf, 36000, null, null, null);
            AssertEqualToFutan(retReceInf, 177450, 11550, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（上位所得）・公費（特定疾患）
        /// ※医保単独部分及び併用部分で高額療養費が発生する場合 （合算対象）
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei34()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 11550
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 52000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 52000, 364000, 5800, 138650, 0, 0, 0, 11550, 11550);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 63000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 63000, 441000, 37700, 0, 0, 0, 0, 151300, 151300);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 6350, 0, 0, 0, 0, -6350, -6350);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 115000, 52000, null, null, null);
            AssertEqualToKyufu(retReceInf, 150200, null, null, null);
            AssertEqualToFutan(retReceInf, 295150, 11550, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（自立支援更生医療）
        /// ※高額療養費が発生しない場合　・　公費に係る自己負担額が１割の場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei35()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 4500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4500, 31500, 0, 9000, 0, 0, 0, 4500, 4500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 4500, 4500, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 4500, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（自立支援更生医療）
        /// ※高額療養費が発生しない場合　・　公費に係る自己負担額が１割の場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei36()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 32000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 32000, 224000, 15370, 48630, 0, 0, 0, 32000, 32000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 32000, 32000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 80630, 32000, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（自立支援更生医療）
        /// ※高額療養費が発生しない場合　・　公費に係る自己負担額が１割の場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei37()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 7000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 7000, 49000, 0, 14000, 0, 0, 0, 7000, 7000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 9000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 9000, 63000, 0, 0, 0, 0, 0, 27000, 27000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 16000, 7000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 7000, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（自立支援更生医療）
        /// ※医保単独部分で高額療養費が発生する場合 ・ 公費に係る自己負担額が１割の場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei38()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 5000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 10000, 0, 0, 0, 5000, 5000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 16000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 16000, 112000, 12600, 0, 0, 0, 0, 35400, 35400);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 21000, 5000, null, null, null);
            AssertEqualToKyufu(retReceInf, 15000, null, null, null);
            AssertEqualToFutan(retReceInf, 50400, 5000, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（自立支援更生医療）
        /// ※併用部分で高額療養費が発生する場合 ・ 公費に係る自己負担額が１割の場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei39()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 32000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 32000, 224000, 15370, 48630, 0, 0, 0, 32000, 32000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 3000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 21000, 0, 0, 0, 0, 0, 9000, 9000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 35000, 32000, null, null, null);
            AssertEqualToKyufu(retReceInf, 80630, null, null, null);
            AssertEqualToFutan(retReceInf, 89630, 32000, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（自立支援更生医療）
        /// ※医保単独部分及び併用部分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei40()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 0
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 30000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 210000, 9570, 80430, 0, 0, 0, 0, 0);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 16000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 16000, 112000, 12600, 0, 0, 0, 0, 35400, 35400);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 46000, 30000, null, null, null);
            AssertEqualToKyufu(retReceInf, 80430, null, null, null);
            AssertEqualToFutan(retReceInf, 115830, 0, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（自立支援更生医療）
        /// ※医保単独部分で高額療養費が発生する場合 （合算対象）・　公費に係る自己負担額が１割の場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei41()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 16000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 16000, 112000, 0, 32000, 0, 0, 0, 16000, 16000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 18000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 18000, 126000, 18600, 0, 0, 0, 0, 35400, 35400);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 16000, 0, 0, 0, 0, -16000, -16000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 34000, 16000, null, null, null);
            AssertEqualToKyufu(retReceInf, 48000, null, null, null);
            AssertEqualToFutan(retReceInf, 67400, 16000, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（自立支援更生医療）
        /// ※医保単独部分及び併用部分で高額療養費が発生する場合 (合算対象)・公費に係る自己負担額が１割の場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei42()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 33000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 33000, 231000, 18270, 47730, 0, 0, 0, 33000, 33000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 13500, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 13500, 94500, 5100, 0, 0, 0, 0, 35400, 35400);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 33000, 0, 0, 0, 0, -33000, -33000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 46500, 33000, null, null, null);
            AssertEqualToKyufu(retReceInf, 80730, null, null, null);
            AssertEqualToFutan(retReceInf, 83130, 33000, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（自立支援更生医療）
        /// ※高額療養費が発生しない場合　・　公費に係る自己負担額が１割の場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei43()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 16000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 16000, 112000, 0, 32000, 0, 0, 0, 16000, 16000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 16000, 16000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 16000, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（上位所得）・公費（自立支援更生医療）
        /// ※高額療養費が発生する場合　・　公費に係る自己負担額が１割の場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei44()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 53000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 53000, 371000, 76270, 29730, 0, 0, 0, 53000, 53000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 53000, 53000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 82730, 53000, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（上位所得）・公費（自立支援更生医療）
        /// ※高額療養費が発生しない場合　・　公費に係る自己負担額が１割の場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei45()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 12000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 12000, 84000, 0, 24000, 0, 0, 0, 12000, 12000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 11000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 11000, 77000, 0, 0, 0, 0, 0, 33000, 33000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 23000, 12000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 12000, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外本人入院外（上位所得）・公費（自立支援更生医療）
        /// ※医保単独部分で高額療養費が発生する場合 ・ 公費に係る自己負担額が１割の場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei46()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 6000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 42000, 0, 12000, 0, 0, 0, 6000, 6000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 55000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 55000, 385000, 14500, 0, 0, 0, 0, 150500, 150500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 61000, 6000, null, null, null);
            AssertEqualToKyufu(retReceInf, 18000, null, null, null);
            AssertEqualToFutan(retReceInf, 168500, 6000, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（上位所得）・公費（自立支援更生医療）
        /// ※併用部分で高額療養費が発生する場合 ・ 公費に係る自己負担額が１割の場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei47()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 29000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 29000, 203000, 6670, 51330, 0, 0, 0, 29000, 29000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 5000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 0, 0, 0, 0, 15000, 15000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 34000, 29000, null, null, null);
            AssertEqualToKyufu(retReceInf, 80330, null, null, null);
            AssertEqualToFutan(retReceInf, 95330, 29000, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（自立支援更生医療）
        /// ※医保単独部分及び併用部分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei48()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 0
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 30000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 210000, 9570, 80430, 0, 0, 0, 0, 0);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 52000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 52000, 364000, 5800, 0, 0, 0, 0, 150200, 150200);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 82000, 30000, null, null, null);
            AssertEqualToKyufu(retReceInf, 80430, null, null, null);
            AssertEqualToFutan(retReceInf, 230630, 0, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（上位所得）・公費（自立支援更生医療）
        /// ※医保単独部分で高額療養費が発生する場合 (合算対象)・　公費に係る自己負担額が１割の場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei49()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 23000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 23000, 161000, 0, 46000, 0, 0, 0, 23000, 23000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 62000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 62000, 434000, 34800, 0, 0, 0, 0, 151200, 151200);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 85000, 23000, null, null, null);
            AssertEqualToKyufu(retReceInf, 69000, null, null, null);
            AssertEqualToFutan(retReceInf, 199500, 23000, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（上位所得）・公費（結核患者の適正医療）
        /// ※医保単独部分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei50()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "10",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 10併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 5000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 12500, 0, 0, 0, 2500, 2500);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 57500, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 57500, 402500, 21750, 0, 0, 0, 0, 150750, 150750);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 62500, 5000, null, null, null);
            AssertEqualToKyufu(retReceInf, 15000, null, null, null);
            AssertEqualToFutan(retReceInf, 165750, 153250, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（上位所得）・公費（結核患者の適正医療）
        /// ※併用部分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei51()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "10",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 10併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 31000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 31000, 217000, 12470, 65030, 0, 0, 0, 15500, 15500);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 4500, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4500, 31500, 0, 0, 0, 0, 0, 13500, 13500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 35500, 31000, null, null, null);
            AssertEqualToKyufu(retReceInf, 80530, null, null, null);
            AssertEqualToFutan(retReceInf, 94030, 29000, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（自立支援更生医療）
        /// ※高額療養費が発生しない場合　・　公費に係る自己負担額が１割の場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei52()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 17
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "10",
                monthLimitFutan: null
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 10併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 8000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 8000, 56000, 0, 20000, 0, 0, 0, 4000, 4000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 53000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 53000, 371000, 8700, 0, 0, 0, 0, 150300, 150300);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 3200, 0, 0, 0, 0, -3200, -3200);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 61000, 8000, null, null, null);
            AssertEqualToKyufu(retReceInf, 24000, null, null, null);
            AssertEqualToFutan(retReceInf, 171100, 151100, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（低所得）・公費（生活保護）
        /// ※高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei53()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "12",
                monthLimitFutan: 0
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 12併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 30000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 210000, 54600, 35400, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 30000, 30000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 35400, null, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 本人入院外（一般所得）・公費（特定疾患）・公費（生活保護）
        /// ※高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei54()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 18
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "12",
                monthLimitFutan: 0
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 120000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 120000, 840000, 270570, 89430, 0, 0, 0, 0, 0);
            //2日目 12併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 45000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 45000, 315000, 99600, 35400, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 165000, 120000, 45000, null, null);
            AssertEqualToKyufu(retReceInf, 89430, 35400, null, null);
            AssertEqualToFutan(retReceInf, 124830, 0, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1132"));
        }

        /// <summary>
        /// 本人入院外（一般所得）・公費（特定疾患）・公費（生活保護）
        /// ※高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T003_SyahoH2404_Under70Jirei55()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20120401,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 19
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "51",
                monthLimitFutan: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "12",
                monthLimitFutan: 0
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //1日目 51併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120401, tensu: 120000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 120000, 840000, 324600, 35400, 0, 0, 0, 0, 0);
            //2日目 12併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20120402, tensu: 65000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 65000, 455000, 159600, 35400, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 185000, 120000, 65000, null, null);
            AssertEqualToKyufu(retReceInf, 35400, 35400, null, null);
            AssertEqualToFutan(retReceInf, 70800, 0, null, null, null);
            //Assert.That(retReceInf.TokkiContains("18"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1132"));
        }

        //----------------------------------------------------------------------------------------------------
        // T005_指定公費
        //----------------------------------------------------------------------------------------------------

        /// <summary>
        /// 高齢受給者・難病医療（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// 公費分が2割換算で超過
        /// </summary>
        [Test]
        public void T005_01_Syaho_KoreiSitei01()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19440101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 9900, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 9900, 89100, 0, 4900, 0, 0, 0, 5000, 5000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 1000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 10900, 9900, null, null, null);
            AssertEqualToKyufu(retReceInf, 9900, null, null, null);
            AssertEqualToFutan(retReceInf, 10900, 5000, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・難病医療（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// 公費分が2割換算で限度額丁度
        /// </summary>
        [Test]
        public void T005_01_Syaho_KoreiSitei02()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19440101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 9000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 9000, 81000, 0, 4000, 0, 0, 0, 5000, 5000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 1000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 10000, 9000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 5000, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・難病医療（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// 保険単独分が2割換算で超過
        /// </summary>
        [Test]
        public void T005_01_Syaho_KoreiSitei03()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19440101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 1000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 9900, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 9900, 89100, 0, 0, 0, 0, 0, 9900, 9900);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 10900, 1000, null, null, null);
            AssertEqualToKyufu(retReceInf, 1000, null, null, null);
            AssertEqualToFutan(retReceInf, 10900, 2000, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・難病医療（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// 合算が2割換算で超過
        /// </summary>
        [Test]
        public void T005_01_Syaho_KoreiSitei04()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19440101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 2000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 0, 0, 0, 0, 0, 2000, 2000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 7100, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 7100, 63900, 0, 0, 0, 0, 0, 7100, 7100);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 9100, 2000, null, null, null);
            AssertEqualToKyufu(retReceInf, 2000, null, null, null);
            AssertEqualToFutan(retReceInf, 9100, 4000, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・難病医療（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// 合算が2割換算で限度額丁度
        /// </summary>
        [Test]
        public void T005_01_Syaho_KoreiSitei05()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19440101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 2000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 18000, 0, 0, 0, 0, 0, 2000, 2000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 7000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 7000, 63000, 0, 0, 0, 0, 0, 7000, 7000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 9000, 2000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 4000, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・マル長・難病医療（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// 公費分が2割換算で超過
        /// </summary>
        [Test]
        public void T005_02_Syaho_ChokiSitei01()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19440101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 2500
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 5100, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5100, 45900, 0, 0, 2600, 0, 0, 2500, 2500);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 1000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 6100, null, 5100, null, null);
            AssertEqualToKyufu(retReceInf, null, 5100, null, null);
            AssertEqualToFutan(retReceInf, 6100, null, 2500, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・マル長・難病医療（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// 公費分が2割換算で限度額丁度
        /// </summary>
        [Test]
        public void T005_02_Syaho_ChokiSitei02()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19440101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 2500
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 4900, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4900, 44100, 0, 0, 2400, 0, 0, 2500, 2500);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 1000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 5900, null, 4900, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, null, 2500, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.False);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・マル長・難病医療（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// 保険単独分が2割換算で超過
        /// </summary>
        [Test]
        public void T005_02_Syaho_ChokiSitei03()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19440101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 2500
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 1000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 4900, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4900, 44100, 0, 0, 0, 0, 0, 4900, 4900);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 5900, null, 1000, null, null);
            AssertEqualToKyufu(retReceInf, null, 1000, null, null);
            AssertEqualToFutan(retReceInf, 5900, null, 2000, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・マル長・難病医療（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// 合算が2割換算で超過
        /// </summary>
        [Test]
        public void T005_02_Syaho_ChokiSitei04()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19440101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 2500
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 1000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 4100, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4100, 36900, 0, 0, 0, 0, 0, 4100, 4100);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 5100, null, 1000, null, null);
            AssertEqualToKyufu(retReceInf, null, 1000, null, null);
            AssertEqualToFutan(retReceInf, 5100, null, 2000, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 高齢受給者・マル長・難病医療（特例措置対象者：生年月日が昭和19年4月1日以前）
        /// 合算が2割換算で限度額丁度
        /// </summary>
        [Test]
        public void T005_02_Syaho_ChokiSitei05()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19440101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 2500
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 1000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 9000, 0, 0, 0, 0, 0, 1000, 1000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 4000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 36000, 0, 0, 0, 0, 0, 4000, 4000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 5000, null, 1000, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, null, 2000, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.False);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        //----------------------------------------------------------------------------------------------------
        // T006_高額療養費計算事例（合算対象となる条件の確認） 2018/09/19まとめ
        //----------------------------------------------------------------------------------------------------

        /// <summary>
        /// 70歳未満一般(28区ウ) + 54(10,000)
        /// ※公費対象分の一部負担金相当額が21,000円以上／合算対象
        /// </summary>
        [Test]
        public void T006_KogakuJirei01()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 7918, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 7918, 55426, 0, 13754, 0, 0, 0, 10000, 10000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 67472, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 67472, 472304, 118239, 0, 0, 0, 0, 84177, 84177);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 9208, 0, 0, 0, 0, -9208, -9208);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 75390, 7918, null, null, null);
            AssertEqualToKyufu(retReceInf, 23750, null, null, null);
            AssertEqualToFutan(retReceInf, 98719, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 70歳未満一般(28区ウ) + 54(10,000)
        /// ※公費対象分の一部負担金相当額が21,000円未満／合算対象外
        /// </summary>
        [Test]
        public void T006_KogakuJirei02()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 5000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 5000, 0, 0, 0, 10000, 10000);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 67472, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 67472, 472304, 118239, 0, 0, 0, 0, 84177, 84177);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 72472, 5000, null, null, null);
            AssertEqualToKyufu(retReceInf, 15000, null, null, null);
            AssertEqualToFutan(retReceInf, 99177, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 社保 70歳未満一般(28区ウ) + 52(10,000) + 大阪82(500)
        /// ※公費対象分の一部負担金相当額が21,000円超だが公２に合算対象外の地単／合算対象外
        /// </summary>
        [Test]
        public void T006_KogakuJirei03()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTotalKbn: 2
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 7918, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 7918, 55426, 0, 13754, 9500, 0, 0, 500, 500);
            //保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 67472, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 67472, 472304, 118239, 84177, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 75390, 7918, 75390, null, null);
            AssertEqualToKyufu(retReceInf, 23750, 84177, null, null);
            AssertEqualToFutan(retReceInf, 107927, 10000, 500, null, null);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1132"));
        }

        /// <summary>
        /// 社保 70歳未満一般(28区ウ) + 52(10,000) + 大阪82(500)
        /// ※全分点／公２に合算対象外の地単
        /// </summary>
        [Test]
        public void T006_KogakuJirei04()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19900101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTotalKbn: 2
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 75390, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 75390, 527730, 141201, 74969, 9500, 0, 0, 500, 500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo, receKyufuKisai: 3);

            AssertEqualToTensu(retReceInf, 75390, 75390, 75390, null, null);
            AssertEqualToKyufu(retReceInf, 84969, null, null, null);
            AssertEqualToFutan(retReceInf, 84969, 10000, 500, null, null);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1132"));
        }

        /// <summary>
        /// 国保減免（自立支援減免）
        /// </summary>
        [Test]
        public void T007_01_KokhoGenmen_Jiritusien()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0,
                genmenKbn: 4
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "90",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //1日目 21併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181001, tensu: 417, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 417, 3336, 0, 417, 0, 0, 0, 417, 0, 417);
            //2日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181002, tensu: 11860, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 11860, 94880, 5720, 17500, 0, 0, 0, 500, 500);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 417, -417, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 12277, 417, 12277, null, null);
            AssertEqualToKyufu(retReceInf, 830, 17580, null, null);
            AssertEqualToFutan(retReceInf, 18410, 420, 500, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1138"));
        }

        /// <summary>
        ///  国保減免（自立支援減免）
        /// </summary>
        [Test]
        public void T007_02_KokhoGenmen_Jiritusien()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181001,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0,
                genmenKbn: 4
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 2500
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 21併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181001, tensu: 5000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 7500, 0, 0, 0, 2500, 0, 2500);
            //2日目 21併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181002, tensu: 5000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 2000, 8000, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 10000, 10000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null); ;
            AssertEqualToFutan(retReceInf, 18000, 2500, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 社保高齢+マル長+15(5000) 異点数（公費分→保険分）
        /// </summary>
        [Test]
        public void T008_MarucyoKosei01Syaho01()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 2500
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 20000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 160000, 30000, 0, 7500, 0, 0, 2500, 2500);
            //2日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 10000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 10000, 0, 0, 0, 0, 10000, 10000);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 10000, 0, 0, 0, 0, -10000, -10000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 30000, 30000, 20000, null, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null); ;
            AssertEqualToFutan(retReceInf, 10000, 10000, 2500, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 社保高齢+マル長+15(5000) 異点数（保険分→公費分）
        /// </summary>
        [Test]
        public void T008_MarucyoKosei01Syaho02()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 2500
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 10000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 10000, 0, 0, 0, 0, 10000, 10000);
            //2日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 20000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 160000, 30000, 0, 7500, 0, 0, 2500, 2500);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 1, 10000, 0, 0, 0, 0, -10000, -10000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 30000, 30000, 20000, null, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null); ;
            AssertEqualToFutan(retReceInf, 10000, 10000, 2500, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 国保高齢+マル長+15(5000) 異点数（公費分→保険分）
        /// ※保険単独分にはマル長が適用されない（大阪のケース）
        /// </summary>
        [Test]
        public void T008_MarucyoKosei02Kokho01()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 2500
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 20000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 160000, 30000, 0, 7500, 0, 0, 2500, 2500);
            //2日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 10000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 2000, 0, 0, 0, 0, 18000, 18000);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 2500, 0, 0, 0, 0, -2500, -2500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 30000, 20000, 20000, null, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null); ;
            AssertEqualToFutan(retReceInf, 25500, 10000, 2500, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 国保高齢+マル長+15(5000) 異点数（保険分→公費分）
        /// ※保険単独分にはマル長が適用されない（大阪のケース）
        /// </summary>
        [Test]
        public void T008_MarucyoKosei02Kokho02()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 2500
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 10000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 2000, 0, 0, 0, 0, 18000, 18000);
            //2日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 20000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 160000, 30000, 0, 7500, 0, 0, 2500, 2500);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 1, 2500, 0, 0, 0, 0, -2500, -2500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 30000, 20000, 20000, null, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null); ;
            AssertEqualToFutan(retReceInf, 25500, 10000, 2500, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 国保高齢+マル長+15(5000) 異点数（公費分→保険分）
        /// ※高知県国保連合会H28.7事例（高額療養費と同じような考え方）
        /// </summary>
        [Test]
        public void T008_MarucyoKosei03Kokho01()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM(chokiFutan: 1);
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 2500
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 20000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 160000, 30000, 0, 7500, 0, 0, 2500, 2500);
            //2日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 10000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 10000, 0, 0, 0, 0, 10000, 10000);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 2500, 0, 0, 0, 0, -2500, -2500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 30000, 30000, 20000, null, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null); ;
            AssertEqualToFutan(retReceInf, 17500, 17500, 2500, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 国保高齢+マル長+15(5000) 異点数（保険分→公費分）
        /// ※高知県国保連合会H28.7事例（高額療養費と同じような考え方）
        /// </summary>
        [Test]
        public void T008_MarucyoKosei03Kokho02()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM(chokiFutan: 1);
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 2500
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 20000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 160000, 30000, 0, 0, 0, 0, 10000, 10000);
            //2日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 10000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 10000, 0, 7500, 0, 0, 2500, 2500);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 1, 2500, 0, 0, 0, 0, -2500, -2500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 30000, 30000, 10000, null, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null); ;
            AssertEqualToFutan(retReceInf, 17500, 17500, 2500, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 国保高齢+マル長+15(5000) 異点数（公費分→保険分）
        /// ※高知県国保連合会H28.7事例（高額療養費と同じような考え方）
        /// 公費分が7000点未満
        /// </summary>
        [Test]
        public void T008_MarucyoKosei03Kokho03()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM(chokiFutan: 1);
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 19450101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 2500
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 6000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 48000, 2000, 0, 7500, 0, 0, 2500, 2500);
            //2日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 10000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 10000, 0, 0, 0, 0, 10000, 10000);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 2500, 0, 0, 0, 0, -2500, -2500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 16000, 16000, 6000, null, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null); ;
            AssertEqualToFutan(retReceInf, 17500, 17500, 2500, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 国保70歳未満+マル長+15(5000) 異点数（公費分→保険分）
        /// ※高知県国保連合会H28.7事例（高額療養費と同じような考え方）
        /// 公費分が7000点未満
        /// </summary>
        [Test]
        public void T008_MarucyoKosei03Kokho04()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM(chokiFutan: 1);
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20180801,
                birthDay: 20150101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 2500
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180801, tensu: 6000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 48000, 2000, 0, 7500, 0, 0, 2500, 2500);
            //2日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20180802, tensu: 10000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 10000, 0, 0, 0, 0, 10000, 10000);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 2500, 0, 0, 0, 0, -2500, -2500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToTensu(retReceInf, 16000, 16000, 6000, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null); ;
            AssertEqualToFutan(retReceInf, null, 17500, 2500, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1124"));
        }

        /// <summary>
        /// 国保+マル長+15更生+大阪80
        ///     公２分レセ給付対象額のまるめ誤差
        /// </summary>
        [Test]
        public void T009_01_Kokho_Choki_k15_p27k80()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM(chokiFutan: 1, chokiDateRange: 1);
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20190101,
                birthDay: 19471010
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1,
                receKisai2: 1,
                kogakuTotalKbn: 2
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 3, 0);
            futancalcUT.NewHokenPattern(1, 3, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190101, tensu: 5331, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190101, tensu: 225, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190102, tensu: 2802, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190102, tensu: 225, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190103, tensu: 2802, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190103, tensu: 536, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190104, tensu: 2701, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190104, tensu: 92, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190105, tensu: 2908, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190105, tensu: 211, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190106, tensu: 3182, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190107, tensu: 2701, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190107, tensu: 74, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190108, tensu: 3008, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190109, tensu: 2830, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190110, tensu: 2701, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190110, tensu: 68, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190111, tensu: 2808, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190111, tensu: 72, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190112, tensu: 2802, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190113, tensu: 2701, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20190113, tensu: 74, hokenPid: 2, newRaiin: false);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, true, true, false);
            AssertEqualToTensu(retReceInf, 40854, null, 39277, 40854, null);
            AssertEqualToKyufu(retReceInf, null, 10000, 8490, null); ;
            AssertEqualToFutan(retReceInf, 13160, null, 5330, 2630, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1138"));
        }

        /// <summary>
        /// 国保+マル長+15更生+大阪80
        ///     公２分レセ給付対象額のまるめ誤差2
        /// </summary>
        [Test]
        public void T009_02_Kokho_Choki_k15_p27k80()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM(chokiFutan: 0, chokiDateRange: 1);
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20201103,
                birthDay: 19521008
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 29
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1,
                receKisai2: 1,
                kogakuTotalKbn: 2
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 3, 0);
            futancalcUT.NewHokenPattern(1, 3, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201103, tensu: 3267, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201103, tensu: 4687, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201104, tensu: 1012, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201105, tensu: 2981, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201105, tensu: 35, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201107, tensu: 2955, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201107, tensu: 35, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201110, tensu: 2887, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201110, tensu: 35, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201112, tensu: 2881, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201112, tensu: 128, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201114, tensu: 2887, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201114, tensu: 126, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201117, tensu: 5265, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201117, tensu: 1785, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201118, tensu: 712, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201118, tensu: 149, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201119, tensu: 2881, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201119, tensu: 35, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201121, tensu: 2970, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201121, tensu: 108, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201124, tensu: 2970, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201124, tensu: 35, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201125, tensu: 712, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201125, tensu: 270, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201126, tensu: 2964, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201126, tensu: 110, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201128, tensu: 3044, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20201128, tensu: 108, hokenPid: 2, newRaiin: false);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, true, true, false);
            AssertEqualToTensu(retReceInf, 48034, null, 37952, 48034, null);
            AssertEqualToKyufu(retReceInf, null, 10000, 33710, null); ;
            AssertEqualToFutan(retReceInf, 40240, null, 3470, 3000, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1132"));
        }

        /// <summary>
        /// 国保+マル長+15更生+大阪80
        ///     保険分一部負担金のまるめ誤差
        /// </summary>
        [Test]
        public void T009_03_Kokho_Choki_k15_p27k80()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM(chokiFutan: 0, chokiDateRange: 1);
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20191001,
                birthDay: 19490830
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 4
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1,
                receKisai2: 1,
                kogakuTotalKbn: 2
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 3, 0);
            futancalcUT.NewHokenPattern(1, 3, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191001, tensu: 1010, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191002, tensu: 4965, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191002, tensu: 1285, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191004, tensu: 2715, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191007, tensu: 2715, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191008, tensu: 710, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191009, tensu: 2715, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191009, tensu: 92, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191011, tensu: 2721, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191014, tensu: 3295, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191015, tensu: 710, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191016, tensu: 2715, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191016, tensu: 92, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191018, tensu: 2721, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191021, tensu: 2772, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191021, tensu: 140, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191023, tensu: 2715, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191025, tensu: 2721, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191028, tensu: 2715, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191028, tensu: 545, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191030, tensu: 2715, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20191030, tensu: 74, hokenPid: 2, newRaiin: false);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, true, true, false);
            AssertEqualToTensu(retReceInf, 42858, null, 38200, 42858, null);
            AssertEqualToKyufu(retReceInf, null, 10000, 8000, null); ;
            AssertEqualToFutan(retReceInf, 13030, null, 4970, 1000, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("30"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1138"));
        }

        /// <summary>
        /// レセ負担額まるめ誤差
        /// </summary>
        [Test]
        public void T010_01_ReceFutan_Marume()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20181201,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 5
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 2500
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "88",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1,
                receKisai2: 1,
                kogakuTotalKbn: 2
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181201, tensu: 300, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181201, tensu: 394, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181201, tensu: 260, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181202, tensu: 294, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181202, tensu: 260, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181203, tensu: 260, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181203, tensu: 294, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181204, tensu: 203, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181204, tensu: 162, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181204, tensu: 351, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181204, tensu: 1807, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181205, tensu: 34, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181206, tensu: 1083, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181207, tensu: 64, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181207, tensu: 8111, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181208, tensu: 164, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181208, tensu: 43, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181209, tensu: 64, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181209, tensu: 43, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181210, tensu: 64, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181210, tensu: 43, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181211, tensu: 2843, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181211, tensu: 438, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181211, tensu: 1218, hokenPid: 2, newRaiin: false);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181212, tensu: 43, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181212, tensu: 64, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181213, tensu: 34, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181214, tensu: 134, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181215, tensu: 34, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181216, tensu: 34, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181217, tensu: 34, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181218, tensu: 203, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181225, tensu: 203, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181227, tensu: 174, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20181228, tensu: 601, hokenPid: 1);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, true, false, false);
            AssertEqualToTensu(retReceInf, 20355, 5316, 20355, null, null);
            AssertEqualToKyufu(retReceInf, 5320, 8000, null, null);          //5316-> 5320
            AssertEqualToFutan(retReceInf, 10820, 2500, 3000, null, null);   //10820 (5320 + 8000 - 2500 = 10820)
            Assert.That(retReceInf.TokkiContains("30"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1338"));
        }

        /// <summary>
        /// レセ負担額まるめ誤差
        /// </summary>
        [Test]
        public void T010_02_ReceFutan_Marume()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20200201,
                birthDay: 19450801
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 4
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 2500
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20200201, tensu: 74, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20200202, tensu: 5018, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20200203, tensu: 1218, hokenPid: 2);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 6310, 74, null, null, null);
            AssertEqualToKyufu(retReceInf, 150, null, null, null);        //148 -> 150
            AssertEqualToFutan(retReceInf, 8080, 70, null, null, null);   //8074 -> 8080, 74 -> 70
            Assert.That(retReceInf.TokkiContains("30"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// レセ負担額まるめ誤差
        /// </summary>
        [Test]
        public void T010_03_ReceFutan_Marume()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20220701,
                birthDay: 19500101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                kogakuKbn: 4
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "21",
                monthLimitFutan: 5000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20220701, tensu: 213, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20220702, tensu: 550, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20220703, tensu: 213, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20220704, tensu: 3601, hokenPid: 2);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, true, false, false);
            AssertEqualToTensu(retReceInf, 4577, 213, 4364, null, null);
            AssertEqualToKyufu(retReceInf, 430, 8000, null, null);         //426 -> 430
            AssertEqualToFutan(retReceInf, 8430, 210, 5000, null, null);   //8426 -> 8430, 213 -> 210
            Assert.That(retReceInf.TokkiContains("30"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1138"));
        }

        /// <summary>
        /// レセ負担額まるめ誤差
        /// </summary>
        [Test]
        public void T010_04_ReceFutan_Marume()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Hyogo;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20230801,
                birthDay: 19410612
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "59",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 600,
                monthLimitCount: 2,
                kogakuTekiyo: 11,
                futanYusen: 1
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20230801, tensu: 79, hokenPid: 1);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20230803, tensu: 851, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20230810, tensu: 213, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20230824, tensu: 11594, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20230829, tensu: 79, hokenPid: 2);
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20230830, tensu: 213, hokenPid: 2);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 13029, 12950, null, null, null);
            AssertEqualToKyufu(retReceInf, 17840, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, 1030, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// 社保 28感染症 + 滋賀43
        /// ※公費対象分の一部負担金相当額が21,000円未満だが公２に合算対象の地単
        /// </summary>
        [Test]
        public void T011_P25_KogakuTotal()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Shiga;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210615,
                birthDay: 19920502
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "28",
                monthLimitFutan: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "43",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 0,
                receFutanRound: 3,
                kogakuTekiyo: 11,
                kogakuTotalAll: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210615, tensu: 1950, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1950, 13650, 0, 5850, 0, 0, 0, 0, 0);

            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210615, tensu: 556, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 556, 3892, 0, 1668, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210617, tensu: 68538, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 68538, 479766, 122748, 82866, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210618, tensu: 131, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 131, 917, 379, 14, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210619, tensu: 131, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 131, 917, 380, 13, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210625, tensu: 249, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 249, 1743, 722, 25, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, true, false, false);
            AssertEqualToTensu(retReceInf, 71555, 1950, 71555, null, null);
            AssertEqualToKyufu(retReceInf, 5850, 84586, null, null);
            AssertEqualToFutan(retReceInf, 90436, 0, null, null, null);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1132"));
        }

        /// <summary>
        /// 京都府地単　特記事項(01公) 70歳未満
        ///     限度額認定証の提示なし
        /// </summary>
        [Test]
        public void T012_P026_Tokki01_01()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Kyoto;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210801,
                birthDay: 19800101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "45",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                kogakuTekiyo: 31,
                receSeikyuKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 30000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 210000, 0, 89500, 0, 0, 0, 500, 500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 30000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("01"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1112"));
        }

        /// <summary>
        /// 京都府地単　特記事項(01公) 70歳未満
        ///     限度額認定証の提示あり（区ウ）
        /// </summary>
        [Test]
        public void T012_P026_Tokki01_02()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Kyoto;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210801,
                birthDay: 19800101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "45",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                kogakuTekiyo: 31,
                receSeikyuKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 30000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 210000, 9570, 79930, 0, 0, 0, 500, 500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 30000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 80430, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("01"), Is.True);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1112"));
        }

        /// <summary>
        /// 京都府地単　特記事項(01公) 70歳未満
        ///     限度額認定証の提示あり（区イ）
        /// </summary>
        [Test]
        public void T012_P026_Tokki01_03()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Kyoto;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210801,
                birthDay: 19800101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 27
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "45",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                kogakuTekiyo: 31,
                receSeikyuKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 60000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 60000, 420000, 96570, 82930, 0, 0, 0, 500, 500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 60000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 83430, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("01"), Is.True);
            Assert.That(retReceInf.TokkiContains("27"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1112"));
        }

        /// <summary>
        /// 京都府地単　特記事項(01公) 70歳未満
        ///     限度額認定証の提示あり（区オ）
        /// </summary>
        [Test]
        public void T012_P026_Tokki01_04()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Kyoto;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210801,
                birthDay: 19800101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "45",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                kogakuTekiyo: 31,
                receSeikyuKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 30000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 210000, 9570, 79930, 0, 0, 0, 500, 500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 30000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 80430, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("01"), Is.True);
            Assert.That(retReceInf.TokkiContains("30"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1112"));
        }

        /// <summary>
        /// 京都府地単　特記事項(01公) 70歳未満
        ///     限度額認定証の提示あり（区オ）区分ウの上限未満
        /// </summary>
        [Test]
        public void T012_P026_Tokki01_05()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Kyoto;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210801,
                birthDay: 19800101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 30
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "45",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                kogakuTekiyo: 31,
                receSeikyuKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 20000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 140000, 0, 59500, 0, 0, 0, 500, 500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 20000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("01"), Is.True);
            Assert.That(retReceInf.TokkiContains("30"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1112"));
        }

        /// <summary>
        /// 京都府地単　特記事項(01公) 70歳未満 3併
        ///     限度額認定証の提示あり（区ウ）
        ///     医保分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T012_P026_Tokki01_11()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Kyoto;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210801,
                birthDay: 19800101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "45",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                kogakuTekiyo: 31,
                receSeikyuKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //52併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 5000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 5000, 9500, 0, 0, 500, 500);
            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 30000, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 210000, 9570, 80430, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 35000, 5000, null, null, null);
            AssertEqualToKyufu(retReceInf, 15000, null, null, null);
            AssertEqualToFutan(retReceInf, 95430, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("01"), Is.True);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 京都府地単　特記事項(01公) 70歳未満 3併
        ///     限度額認定証の提示あり（区ウ）
        ///     併用分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T012_P026_Tokki01_12()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Kyoto;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210801,
                birthDay: 19800101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "45",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                kogakuTekiyo: 31,
                receSeikyuKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //52併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 30000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 210000, 9570, 70430, 9500, 0, 0, 500, 500);
            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 5000, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 15000, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 35000, 30000, null, null, null);
            AssertEqualToKyufu(retReceInf, 80430, null, null, null);
            AssertEqualToFutan(retReceInf, 95430, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("01"), Is.False);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 京都府地単　特記事項(01公) 70歳未満 3併
        ///     限度額認定証の提示あり（区ウ）
        ///     医保分と併用分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T012_P026_Tokki01_13()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Kyoto;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210801,
                birthDay: 19800101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "45",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                kogakuTekiyo: 31,
                receSeikyuKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //52併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 30000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 210000, 9570, 70430, 9500, 0, 0, 500, 500);
            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 30000, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 30000, 210000, 9570, 80430, 0, 0, 0, 0, 0);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 7000, -7000, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 60000, 30000, null, null, null);
            AssertEqualToKyufu(retReceInf, 80430, null, null, null);
            AssertEqualToFutan(retReceInf, 153860, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("01"), Is.True);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 京都府地単　特記事項(01公) 70歳未満 3併
        ///     限度額認定証の提示あり（区ウ）
        ///     合算で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T012_P026_Tokki01_14()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Kyoto;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210801,
                birthDay: 19800101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "45",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                kogakuTekiyo: 31,
                receSeikyuKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //52併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 25000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 25000, 175000, 0, 65000, 9500, 0, 0, 500, 500);
            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 25000, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 25000, 175000, 0, 75000, 0, 0, 0, 0, 0);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 2570, -2570, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 50000, 25000, null, null, null);
            AssertEqualToKyufu(retReceInf, 75000, null, null, null);
            AssertEqualToFutan(retReceInf, 147430, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("01"), Is.True);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 京都府地単　特記事項(01公) 70歳未満 3併
        ///     限度額認定証の提示あり（区ウ）
        ///     合算対象外
        /// </summary>
        [Test]
        public void T012_P026_Tokki01_15()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Kyoto;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210801,
                birthDay: 19800101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "45",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                kogakuTekiyo: 31,
                receSeikyuKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //52併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 5000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 5000, 9500, 0, 0, 500, 500);
            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 25000, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 25000, 175000, 0, 75000, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 30000, 5000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("01"), Is.False);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 京都府地単　特記事項(01公) 70歳未満 3併
        ///     限度額認定証の提示あり（区イ）
        ///     医保分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T012_P026_Tokki01_21()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Kyoto;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210801,
                birthDay: 19800101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 27
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "45",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                kogakuTekiyo: 31,
                receSeikyuKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //52併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 5000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 5000, 9500, 0, 0, 500, 500);
            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 60000, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 60000, 420000, 12180, 167820, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 65000, 5000, null, null, null);
            AssertEqualToKyufu(retReceInf, 15000, null, null, null);
            AssertEqualToFutan(retReceInf, 182820, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("01"), Is.True);
            Assert.That(retReceInf.TokkiContains("27"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 京都府地単　特記事項(01公) 70歳未満 3併
        ///     限度額認定証の提示あり（区イ）
        ///     併用分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T012_P026_Tokki01_22()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Kyoto;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210801,
                birthDay: 19800101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 27
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "45",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                kogakuTekiyo: 31,
                receSeikyuKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //52併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 60000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 60000, 420000, 12180, 157820, 9500, 0, 0, 500, 500);
            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 5000, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 0, 15000, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 65000, 60000, null, null, null);
            AssertEqualToKyufu(retReceInf, 167820, null, null, null);
            AssertEqualToFutan(retReceInf, 182820, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("01"), Is.False);
            Assert.That(retReceInf.TokkiContains("27"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 京都府地単　特記事項(01公) 70歳未満 3併
        ///     限度額認定証の提示あり（区イ）
        ///     医保分と併用分で高額療養費が発生する場合
        /// </summary>
        [Test]
        public void T012_P026_Tokki01_23()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Kyoto;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20210801,
                birthDay: 19800101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 27
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "52",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "45",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                kogakuTekiyo: 31,
                receSeikyuKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //52併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 60000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 60000, 420000, 12180, 157820, 9500, 0, 0, 500, 500);
            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20210801, tensu: 60000, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 60000, 420000, 12180, 167820, 0, 0, 0, 0, 0);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 4000, -4000, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 120000, 60000, null, null, null);
            AssertEqualToKyufu(retReceInf, 167820, null, null, null);
            AssertEqualToFutan(retReceInf, 331640, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("01"), Is.True);
            Assert.That(retReceInf.TokkiContains("27"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 大阪 特殊処理
        ///     【2021.01.21】 大阪府福祉医療費助成制度の医療証の提示があった場合の窓口徴収額について
        ///     【事例１】府外保険者　限度額認定証提示なし　大阪府福祉医療費助成制度の併用
        ///     ①若人(3割負担)
        /// </summary>
        [Test]
        public void T012_P027_80Kogaku_01_1()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20220401,
                birthDay: 19800101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                hokensyaNo: "280000",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                //dayLimitFutan: 500,   //1日で事例通りにするため除外
                monthLimitFutan: 3000,
                calcSpKbn: 1,
                kogakuTekiyo: 1,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20220401, tensu: 100000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 100000, 700000, 0, 84430, 0, 0, 0, 3000, 215570, -212570);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 100000, 100000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 3000, null, null, null);
            Assert.That(retReceInf.Tokki, Is.Empty);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 大阪 特殊処理
        ///     【2021.01.21】 大阪府福祉医療費助成制度の医療証の提示があった場合の窓口徴収額について
        ///     【事例１】府外保険者　限度額認定証提示なし　大阪府福祉医療費助成制度の併用
        ///     ②高齢受給者(2割負担)
        /// </summary>
        [Test]
        public void T012_P027_80Kogaku_01_2()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20220701,
                birthDay: 19500101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                hokensyaNo: "280000",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                //dayLimitFutan: 500,   //1日で事例通りにするため除外
                monthLimitFutan: 3000,
                calcSpKbn: 1,
                kogakuTekiyo: 1,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20220701, tensu: 100000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 100000, 800000, 182000, 15000, 0, 0, 0, 3000, 3000, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 100000, 100000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, 3000, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 大阪 特殊処理
        ///     【2021.01.21】 大阪府福祉医療費助成制度の医療証の提示があった場合の窓口徴収額について
        ///     【事例１】府外保険者　限度額認定証提示なし　大阪府福祉医療費助成制度の併用
        ///     ③後期高齢者(1割負担)
        /// </summary>
        [Test]
        public void T012_P027_80Kogaku_01_3()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20220701,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                hokensyaNo: "39280000",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                //dayLimitFutan: 500,   //1日で事例通りにするため除外
                monthLimitFutan: 3000,
                calcSpKbn: 1,
                kogakuTekiyo: 1,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20220701, tensu: 100000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 100000, 900000, 82000, 15000, 0, 0, 0, 3000, 3000, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 100000, 100000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, 3000, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// 大阪 特殊処理
        ///     【2021.01.21】 大阪府福祉医療費助成制度の医療証の提示があった場合の窓口徴収額について
        ///     【事例２】府外保険者　限度額認定証提示あり　大阪府福祉医療費助成制度の併用
        ///     ①若人(3割負担)
        /// </summary>
        [Test]
        public void T012_P027_80Kogaku_02_1()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20220401,
                birthDay: 19800101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                hokensyaNo: "280000",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                //dayLimitFutan: 500,   //1日で事例通りにするため除外
                monthLimitFutan: 3000,
                calcSpKbn: 1,
                kogakuTekiyo: 1,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20220401, tensu: 100000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 100000, 700000, 212570, 84430, 0, 0, 0, 3000, 3000, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 100000, 100000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 87430, 3000, null, null, null);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 大阪 特殊処理
        ///     【2021.01.21】 大阪府福祉医療費助成制度の医療証の提示があった場合の窓口徴収額について
        ///     【事例２】府外保険者　限度額認定証提示あり　大阪府福祉医療費助成制度の併用
        ///     ②高齢受給者(2割負担)
        /// </summary>
        [Test]
        public void T012_P027_80Kogaku_02_2()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20220701,
                birthDay: 19500101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                hokensyaNo: "280000",
                kogakuKbn: 4
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                //dayLimitFutan: 500,   //1日で事例通りにするため除外
                monthLimitFutan: 3000,
                calcSpKbn: 1,
                kogakuTekiyo: 1,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20220701, tensu: 100000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 100000, 800000, 192000, 5000, 0, 0, 0, 3000, 3000, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 100000, 100000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 8000, 3000, null, null, null);
            Assert.That(retReceInf.TokkiContains("30"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 大阪 特殊処理
        ///     【2021.01.21】 大阪府福祉医療費助成制度の医療証の提示があった場合の窓口徴収額について
        ///     【事例２】府外保険者　限度額認定証提示あり　大阪府福祉医療費助成制度の併用
        ///     ③後期高齢者(1割負担)
        /// </summary>
        [Test]
        public void T012_P027_80Kogaku_02_3()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20220701,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                hokensyaNo: "39280000",
                kogakuKbn: 4
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                //dayLimitFutan: 500,   //1日で事例通りにするため除外
                monthLimitFutan: 3000,
                calcSpKbn: 1,
                kogakuTekiyo: 1,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20220701, tensu: 100000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 100000, 900000, 92000, 5000, 0, 0, 0, 3000, 3000, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 100000, 100000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 8000, 3000, null, null, null);
            Assert.That(retReceInf.TokkiContains("30"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// 大阪 特殊処理
        ///     【2021.01.21】 大阪府福祉医療費助成制度の医療証の提示があった場合の窓口徴収額について
        ///     【事例３】府内保険者　限度額認定証提示なし　大阪府福祉医療費助成制度の併用
        ///     ①若人(3割負担)
        /// </summary>
        [Test]
        public void T012_P027_80Kogaku_03_1()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20220401,
                birthDay: 19800101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                hokensyaNo: "270000",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                //dayLimitFutan: 500,   //1日で事例通りにするため除外
                monthLimitFutan: 3000,
                calcSpKbn: 1,
                kogakuTekiyo: 1,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20220401, tensu: 100000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 100000, 700000, 0, 297000, 0, 0, 0, 3000, 3000, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 100000, 100000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 3000, null, null, null);
            Assert.That(retReceInf.Tokki, Is.Empty);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 大阪 特殊処理
        ///     【2021.01.21】 大阪府福祉医療費助成制度の医療証の提示があった場合の窓口徴収額について
        ///     【事例３】府内保険者　限度額認定証提示なし　大阪府福祉医療費助成制度の併用
        ///     ②高齢受給者(2割負担)
        /// </summary>
        [Test]
        public void T012_P027_80Kogaku_03_2()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20220701,
                birthDay: 19500101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "100",
                hokensyaNo: "270000",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                //dayLimitFutan: 500,   //1日で事例通りにするため除外
                monthLimitFutan: 3000,
                calcSpKbn: 1,
                kogakuTekiyo: 1,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20220701, tensu: 100000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 100000, 800000, 182000, 15000, 0, 0, 0, 3000, 3000, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 100000, 100000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, 3000, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1128"));
        }

        /// <summary>
        /// 大阪 特殊処理
        ///     【2021.01.21】 大阪府福祉医療費助成制度の医療証の提示があった場合の窓口徴収額について
        ///     【事例３】府内保険者　限度額認定証提示なし　大阪府福祉医療費助成制度の併用
        ///     ③後期高齢者(1割負担)
        /// </summary>
        [Test]
        public void T012_P027_80Kogaku_03_3()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20220701,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                hokensyaNo: "39270000",
                kogakuKbn: 0
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                //dayLimitFutan: 500,   //1日で事例通りにするため除外
                monthLimitFutan: 3000,
                calcSpKbn: 1,
                kogakuTekiyo: 1,
                receZeroKisai: 1,
                receKisai2: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20220701, tensu: 100000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 100000, 900000, 82000, 15000, 0, 0, 0, 3000, 3000, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 100000, 100000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, 3000, null, null, null);
            Assert.That(retReceInf.TokkiContains("29"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// 社保+マル長+15更生
        ///     月単位計算（公費給付分を含む）
        /// </summary>
        [Test]
        public void T013_01_Syaho_Choki_k15()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20211001,
                birthDay: 19850606
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);

            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211001, tensu: 3000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 21000, 0, 0, 0, 0, 0, 9000, 9000);
            //15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211002, tensu: 5000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 5000, 0, 5000, 0, 0, 5000, 5000);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 1, 2, 9000, 0, 0, 0, 0, -9000, -9000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, true, false, false);
            AssertEqualToTensu(retReceInf, 8000, null, 5000, null, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null);
            AssertEqualToFutan(retReceInf, 10000, null, 5000, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1122"));
        }

        /// <summary>
        /// 社保+マル長+15更生+大阪80
        ///     公費負担額を含む／月単位
        /// </summary>
        [Test]
        public void T013_02_Syaho_Choki_k15_p27k80()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20211001,
                birthDay: 19850606
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1,
                kogakuTotalKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );

            futancalcUT.NewHokenPattern(1, 1, 3, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 2, 3, 0);

            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211001, tensu: 3000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 21000, 0, 0, 8500, 0, 0, 500, 500);
            //15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211002, tensu: 5000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 5000, 0, 5000, 4500, 0, 500, 500);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 1, 2, 9000, 0, -9000, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, true, true, false);
            AssertEqualToTensu(retReceInf, 8000, null, 5000, 8000, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null);
            AssertEqualToFutan(retReceInf, 10000, null, 5000, 1000, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1132"));
        }

        /// <summary>
        /// 社保+マル長+15更生+大阪80
        ///     公費負担額を含む／月単位
        /// </summary>
        [Test]
        public void T013_03_Syaho_Choki_k15_p27k80()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20211001,
                birthDay: 19850606
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1,
                kogakuTotalKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );

            futancalcUT.NewHokenPattern(1, 1, 3, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 2, 3, 0);

            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211001, tensu: 4000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 28000, 2000, 0, 9500, 0, 0, 500, 500);
            //15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211002, tensu: 5000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 5000, 0, 5000, 5000, 0, 0, 0);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 1, 2, 10000, 0, -10000, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, true, true, false);
            AssertEqualToTensu(retReceInf, 9000, null, 5000, 9000, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null);
            AssertEqualToFutan(retReceInf, 10000, null, 5000, 500, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1132"));
        }

        /// <summary>
        /// 社保+マル長+15更生+大阪80
        ///     公費負担額を含む／月単位
        /// </summary>
        [Test]
        public void T013_04_Syaho_Choki_k15_p27k80()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20211001,
                birthDay: 19850606
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1,
                kogakuTotalKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );

            futancalcUT.NewHokenPattern(1, 1, 3, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 2, 3, 0);

            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211001, tensu: 3200, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 3200, 22400, 0, 0, 9100, 0, 0, 500, 500);
            //15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211002, tensu: 5000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 5000, 0, 5000, 4600, 0, 400, 400);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 1, 2, 9600, 0, -9600, 0, 0, 0, 0);
            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211003, tensu: 1000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 2600, 0, 0, 0, 0, 400, 400);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 1, 1, 400, 0, 0, 0, 0, -400, -400);
            //15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211004, tensu: 1000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 3000, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, true, true, false);
            AssertEqualToTensu(retReceInf, 10200, null, 6000, 10200, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null);
            AssertEqualToFutan(retReceInf, 10000, null, 5000, 900, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1132"));
        }

        /// <summary>
        /// 社保+マル長+15更生+大阪80
        ///     公費負担額を含む／月単位
        /// </summary>
        [Test]
        public void T013_05_Syaho_Choki_k15_p27k80()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20211001,
                birthDay: 19850606
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1,
                kogakuTotalKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );

            futancalcUT.NewHokenPattern(1, 1, 3, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 2, 3, 0);

            //15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211001, tensu: 5000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 35000, 5000, 0, 5000, 4500, 0, 500, 500);
            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211002, tensu: 3000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 21000, 0, 0, 8500, 0, 0, 500, 500);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 1, 1, 9000, 0, -8500, 0, 0, -500, -500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, true, true, false);
            AssertEqualToTensu(retReceInf, 8000, null, 5000, 8000, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null);
            AssertEqualToFutan(retReceInf, 10000, null, 5000, 500, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1132"));
        }

        /// <summary>
        /// 社保+マル長+15更生+大阪80
        ///     公費負担額を含む／月単位
        /// </summary>
        [Test]
        public void T013_06_Syaho_Choki_k15_p27k80()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20211001,
                birthDay: 19850606
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1,
                kogakuTotalKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );

            futancalcUT.NewHokenPattern(1, 1, 3, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 2, 3, 0);

            //15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211001, tensu: 2000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 0, 0, 4000, 1500, 0, 500, 500);
            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211001, tensu: 1000, hokenPid: 1, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 0, 3000, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211002, tensu: 2000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 2000, 0, 2000, 1500, 0, 500, 500);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 1, 2, 3000, 0, -3000, 0, 0, 0, 0);
            //15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211003, tensu: 2000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 6000, 0, -1000, 1000, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, true, true, false);
            AssertEqualToTensu(retReceInf, 7000, null, 6000, 7000, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null);
            AssertEqualToFutan(retReceInf, 10000, null, 5000, 1000, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1132"));
        }

        /// <summary>
        /// 社保+マル長+15更生+大阪80
        ///     公費負担額を含む／月単位
        /// </summary>
        [Test]
        public void T013_07_Syaho_Choki_k15_p27k80()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20211001,
                birthDay: 19850606
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1,
                kogakuTotalKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );

            futancalcUT.NewHokenPattern(1, 1, 3, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 2, 3, 0);

            //15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211001, tensu: 2000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 0, 0, 4000, 1500, 0, 500, 500);
            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211001, tensu: 1000, hokenPid: 1, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 0, 3000, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211002, tensu: 2000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 2000, 0, 2000, 1500, 0, 500, 500);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 1, 2, 3000, 0, -3000, 0, 0, 0, 0);
            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211003, tensu: 2000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 14000, 0, 0, 5500, 0, 0, 500, 500);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 1, 1, 6000, 0, -5500, 0, 0, -500, -500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, true, true, false);
            AssertEqualToTensu(retReceInf, 7000, null, 4000, 7000, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null);
            AssertEqualToFutan(retReceInf, 10000, null, 4000, 1000, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1132"));
        }

        /// <summary>
        /// 社保+マル長+15更生+大阪80
        ///     公費負担額を含む／月単位
        /// </summary>
        [Test]
        public void T013_08_Syaho_Choki_k15_p27k80()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20211001,
                birthDay: 19850606
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "01",
                kogakuKbn: 28
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTekiyo: 1,
                kogakuTotalKbn: 2,
                receZeroKisai: 1,
                receKisai2: 1
            );

            futancalcUT.NewHokenPattern(1, 1, 3, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 2, 3, 0);

            //15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211001, tensu: 4800, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4800, 33600, 4400, 0, 5200, 4300, 0, 500, 500);
            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211001, tensu: 1000, hokenPid: 1, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 0, 3000, 0, 0, 0, 0);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 1, 1, 3000, 0, -3000, 0, 0, 0, 0);
            //15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211002, tensu: 3000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 21000, 9000, 0, -200, 200, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211002, tensu: 1000, hokenPid: 1, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 7000, 0, 0, 2500, 0, 0, 500, 500);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 1, 1, 3000, 0, -2500, 0, 0, -500, -500);
            //15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20211003, tensu: 3000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 21000, 9000, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, true, true, false);
            AssertEqualToTensu(retReceInf, 12800, null, 10800, 12800, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null);
            AssertEqualToFutan(retReceInf, 10000, null, 5000, 500, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("28"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1132"));
        }

        /// <summary>
        /// 後期高齢者医療制度の負担割合見直しに係る計算事例集
        /// 【事例１】後期高齢者２割負担外来
        ///     ※医療費が30,000円未満のため配慮措置対象外
        /// </summary>
        [Test]
        public void T014_01_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );

            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 2000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 16000, 0, 0, 0, 0, 0, 4000, 4000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 2000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// 後期高齢者医療制度の負担割合見直しに係る計算事例集
        /// 【事例２】後期高齢者２割負担外来
        ///     ※配慮措置計算額よりも高額療養費限度額が低いため高額療養費限度額適用
        /// </summary>
        [Test]
        public void T014_02_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );

            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 20000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 160000, 22000, 0, 0, 0, 0, 18000, 18000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 20000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// 後期高齢者医療制度の負担割合見直しに係る計算事例集
        /// 【事例３】後期高齢者２割負担外来（配慮措置）
        ///     ※高額療養費限度額よりも配慮措置計算額が低いため配慮措置を適用
        /// </summary>
        [Test]
        public void T014_03_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );

            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 8000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 8000, 64000, 5000, 0, 0, 0, 0, 11000, 11000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 8000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 11000, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// 後期高齢者医療制度の負担割合見直しに係る計算事例集
        /// 【事例４】後期高齢者２割負担外来（配慮措置）
        ///     ※高額療養費限度額よりも配慮措置計算額が低いため配慮措置を適用
        /// </summary>
        [Test]
        public void T014_04_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );

            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 13000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 13000, 104000, 10000, 0, 0, 0, 0, 16000, 16000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 13000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 16000, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// 後期高齢者医療制度の負担割合見直しに係る計算事例集
        /// 【事例５】後期高齢者２割負担外来（75歳到達月）
        ///     ※配慮措置計算額よりも高額療養費限度額が低いため高額療養費限度額適用
        /// </summary>
        [Test]
        public void T014_05_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41,
                tokureiYm1: 202210
            );

            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 10000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 11000, 0, 0, 0, 0, 9000, 9000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 10000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 9000, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// 後期高齢者医療制度の負担割合見直しに係る計算事例集
        /// 【事例６】後期高齢者２割負担外来（75歳到達月）
        ///     ※高額療養費限度額適用よりも配慮措置計算額が低いため配慮措置適用
        /// </summary>
        [Test]
        public void T014_06_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41,
                tokureiYm1: 202210
            );

            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 4000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 32000, 1000, 0, 0, 0, 0, 7000, 7000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 4000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 7000, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// 後期高齢者医療制度の負担割合見直しに係る計算事例集
        /// 【事例７】後期高齢者２割負担外来（75歳到達月）
        ///     ※高額療養費限度額適用よりも配慮措置計算額が低いため配慮措置適用
        /// </summary>
        [Test]
        public void T014_07_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41,
                tokureiYm1: 202210
            );

            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 5000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 2000, 0, 0, 0, 0, 8000, 8000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 5000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 8000, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// 後期高齢者医療制度の負担割合見直しに係る計算事例集
        /// 【事例８】後期高齢者２割負担外来（マル長）
        ///     ※特定疾病療養につき配慮措置適用外
        /// </summary>
        [Test]
        public void T014_08_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 15000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 15000, 120000, 20000, 0, 0, 0, 0, 10000, 10000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 15000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 10000, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// 後期高齢者医療制度の負担割合見直しに係る計算事例集
        /// 【事例９】後期高齢者２割負担外来（マル長）（75歳到達月）
        ///     ※特定疾病療養につき配慮措置適用外
        /// </summary>
        [Test]
        public void T014_09_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41,
                tokureiYm1: 202210
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 7000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 7000, 56000, 9000, 0, 0, 0, 0, 5000, 5000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 7000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 5000, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// 後期高齢者医療制度の負担割合見直しに係る計算事例集
        /// 【事例１０】後期高齢者２割負担外来（難病）
        ///     ※特定疾病療養につき配慮措置適用外
        /// </summary>
        [Test]
        public void T014_10_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //計算
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 10000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 2000, 13000, 0, 0, 0, 5000, 5000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 10000, 10000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, 5000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// JAHIS 2022.06 事例集
        ///     例１　法別54（自己負担上限額10,000円）　難病適用外の診療がある場合
        /// </summary>
        [Test]
        public void T015_JAHIS2206_01_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 5000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 0, 0, 0, 0, 10000, 10000);
            //保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 6000, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 48000, 3000, 0, 0, 0, 0, 9000, 9000);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 1000, 0, 0, 0, 0, -1000, -1000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 11000, 5000, null, null, null);
            AssertEqualToKyufu(retReceInf, 10000, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// JAHIS 2022.06 事例集
        ///     例２　法別54（自己負担上限額10,000円）　難病適用外の診療がある場合
        /// </summary>
        [Test]
        public void T015_JAHIS2206_02_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 5000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 0, 0, 0, 0, 0, 10000, 10000);
            //保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 4000, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 32000, 1000, 0, 0, 0, 0, 7000, 7000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 9000, 5000, null, null, null);
            AssertEqualToKyufu(retReceInf, 10000, null, null, null);
            AssertEqualToFutan(retReceInf, 17000, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// JAHIS 2022.06 事例集
        ///     例３　法別54（自己負担上限額10,000円）　難病適用外の診療がある場合
        /// </summary>
        [Test]
        public void T015_JAHIS2206_03_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 7000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 7000, 56000, 0, 4000, 0, 0, 0, 10000, 10000);
            //保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 1000, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 0, 0, 0, 0, 2000, 2000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 8000, 7000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// JAHIS 2022.06 事例集
        ///     例４　法別54（自己負担上限額10,000円）　難病適用外の診療がある場合
        /// </summary>
        [Test]
        public void T015_JAHIS2206_04_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 1000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 0, 0, 0, 0, 2000, 2000);
            //保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 1500, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1500, 12000, 0, 0, 0, 0, 0, 3000, 3000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 2500, 1000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 2000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// JAHIS 2022.06 事例集
        ///     例５　法別54（自己負担上限額10,000円）　難病適用外の診療がある場合
        /// </summary>
        [Test]
        public void T015_JAHIS2206_05_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 10000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 2000, 8000, 0, 0, 0, 10000, 10000);
            //保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 6000, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 48000, 3000, 0, 0, 0, 0, 9000, 9000);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 1000, 0, 0, 0, 0, -1000, -1000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 16000, 10000, null, null, null);
            AssertEqualToKyufu(retReceInf, 18000, null, null, null);
            AssertEqualToFutan(retReceInf, 26000, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// JAHIS 2022.06 事例集
        ///     例６　法別54（自己負担上限額10,000円）　難病適用外の診療がある場合
        /// </summary>
        [Test]
        public void T015_JAHIS2206_06_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 10000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 2000, 8000, 0, 0, 0, 10000, 10000);
            //保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 1000, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 0, 0, 0, 0, 0, 2000, 2000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 11000, 10000, null, null, null);
            AssertEqualToKyufu(retReceInf, 18000, null, null, null);
            AssertEqualToFutan(retReceInf, 20000, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// JAHIS 2022.06 事例集
        ///     独自事例01　法別54（自己負担上限額10,000円）
        /// </summary>
        [Test]
        public void T015_JAHIS2206_A01_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 10000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 2000, 8000, 0, 0, 0, 10000, 10000);
            //保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 4000, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 32000, 1000, 0, 0, 0, 0, 7000, 7000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 14000, 10000, null, null, null);
            AssertEqualToKyufu(retReceInf, 18000, null, null, null);
            AssertEqualToFutan(retReceInf, 25000, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// JAHIS 2022.06 事例集
        ///     独自事例02　法別15（自己負担上限額10,000円）
        /// </summary>
        [Test]
        public void T015_JAHIS2206_A02_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 10000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 2000, 8000, 0, 0, 0, 10000, 10000);
            //1日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 4000, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 32000, 1000, 0, 0, 0, 0, 7000, 7000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 1000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 2000, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 2000, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2000, 16000, 2000, 0, 0, 0, 0, 2000, 2000);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 1000, 0, 0, 0, 0, -1000, -1000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 17000, 11000, null, null, null);
            AssertEqualToKyufu(retReceInf, 18000, null, null, null);
            AssertEqualToFutan(retReceInf, 26000, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// JAHIS 2022.06 事例集
        ///     独自事例02　法別16（自己負担上限額10,000円）
        /// </summary>
        [Test]
        public void T015_JAHIS2206_A03_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "16",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 4000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 32000, 0, 4000, 0, 0, 0, 4000, 4000);
            //2日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 6000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 48000, 2000, 4000, 0, 0, 0, 6000, 6000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221003, tensu: 1000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1000, 8000, 2000, 0, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 11000, 11000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, 10000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// JAHIS 2022.07 事例集
        ///     例２　マル長の認定を受けている場合
        /// </summary>
        [Test]
        public void T015_JAHIS2207_02_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 8000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 8000, 64000, 6000, 0, 0, 0, 0, 10000, 10000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 8000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 10000, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// JAHIS 2022.07 事例集
        ///     例３　マル長の認定を受けている場合
        /// </summary>
        [Test]
        public void T015_JAHIS2207_03_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 4000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 32000, 0, 0, 0, 0, 0, 8000, 8000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 4000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// JAHIS 2022.07 事例集
        ///     独自事例A01　窓口端数確認　高額上限未満
        /// </summary>
        [Test]
        public void T015_JAHIS2207_A01_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 2003);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2003, 16024, 0, 0, 0, 0, 0, 4006, 4010);
            //2日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 2003);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2003, 16024, 1006, 0, 0, 0, 0, 3000, 2996);
            //3日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221003, tensu: 5005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5005, 40040, 5005, 0, 0, 0, 0, 5005, 5005);
            //4日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221004, tensu: 5005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5005, 40040, 5005, 0, 0, 0, 0, 5005, 5005);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 14016, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 17016, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// JAHIS 2022.07 事例集
        ///     独自事例A01　窓口端数確認　高額上限未満
        ///     ※10円単位徴収（四捨五入）に設定
        /// </summary>
        [Test]
        public void T015_JAHIS2207_A01R_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM(roundKogakuPtFutan: 1);
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 2003);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2003, 16024, 0, 0, 0, 0, 0, 4006, 4010);
            //2日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 2003);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2003, 16024, 1006, 0, 0, 0, 0, 3000, 3000);
            //3日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221003, tensu: 5005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5005, 40040, 5005, 0, 0, 0, 0, 5005, 5000);
            //4日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221004, tensu: 5005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5005, 40040, 5005, 0, 0, 0, 0, 5005, 5010);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 14016, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 17016, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// JAHIS 2022.07 事例集
        ///     独自事例A01　窓口端数確認　高額上限未満
        ///     ※10円単位徴収（切り捨て）に設定
        /// </summary>
        [Test]
        public void T015_JAHIS2207_A01T_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM(roundKogakuPtFutan: 2);
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 2003);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2003, 16024, 0, 0, 0, 0, 0, 4006, 4010);
            //2日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 2003);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2003, 16024, 1006, 0, 0, 0, 0, 3000, 2990);
            //3日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221003, tensu: 5005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5005, 40040, 5005, 0, 0, 0, 0, 5005, 5010);
            //4日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221004, tensu: 5005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5005, 40040, 5005, 0, 0, 0, 0, 5005, 5000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 14016, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 17016, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// JAHIS 2022.07 事例集
        ///     独自事例A02　窓口端数確認　高額上限未満
        /// </summary>
        [Test]
        public void T015_JAHIS2207_A02_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 2003);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2003, 16024, 0, 0, 0, 0, 0, 4006, 4010);
            //2日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 2003);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2003, 16024, 1006, 0, 0, 0, 0, 3000, 2996);
            //3日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221003, tensu: 5005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5005, 40040, 5005, 0, 0, 0, 0, 5005, 5005);
            //4日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221004, tensu: 5005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5005, 40040, 5005, 0, 0, 0, 0, 5005, 5005);
            //5日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221005, tensu: 5005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5005, 40040, 9026, 0, 0, 0, 0, 984, 984);
            //6日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221006, tensu: 1005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1005, 8040, 2010, 0, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 20026, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// JAHIS 2022.07 事例集
        ///     独自事例A02　窓口端数確認　高額上限未満
        ///     ※10円単位徴収（四捨五入）に設定
        /// </summary>
        [Test]
        public void T015_JAHIS2207_A02R_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM(roundKogakuPtFutan: 1);
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 2003);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2003, 16024, 0, 0, 0, 0, 0, 4006, 4010);
            //2日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 2003);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2003, 16024, 1006, 0, 0, 0, 0, 3000, 3000);
            //3日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221003, tensu: 5005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5005, 40040, 5005, 0, 0, 0, 0, 5005, 5000);
            //4日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221004, tensu: 5005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5005, 40040, 5005, 0, 0, 0, 0, 5005, 5010);
            //5日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221005, tensu: 5005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5005, 40040, 9026, 0, 0, 0, 0, 984, 980);
            //6日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221006, tensu: 1005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1005, 8040, 2010, 0, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 20026, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// JAHIS 2022.07 事例集
        ///     独自事例A02　窓口端数確認　高額上限未満
        ///     ※10円単位徴収（切り捨て）に設定
        /// </summary>
        [Test]
        public void T015_JAHIS2207_A02T_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM(roundKogakuPtFutan: 2);
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 2003);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2003, 16024, 0, 0, 0, 0, 0, 4006, 4010);
            //2日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 2003);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2003, 16024, 1006, 0, 0, 0, 0, 3000, 2990);
            //3日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221003, tensu: 5005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5005, 40040, 5005, 0, 0, 0, 0, 5005, 5010);
            //4日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221004, tensu: 5005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5005, 40040, 5005, 0, 0, 0, 0, 5005, 5000);
            //5日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221005, tensu: 5005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5005, 40040, 9026, 0, 0, 0, 0, 984, 990);
            //6日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221006, tensu: 1005);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 1005, 8040, 2010, 0, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 20026, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// JAHIS 2022.07 事例集
        ///     独自事例C01 マル長+15(5,000)
        ///         マル長
        ///         　保険単独分を含む（＝配慮措置適用外）
        ///         　公費給付額を含まない
        /// </summary>
        [Test]
        public void T015_JAHIS2207_C01_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM(chokiFutan: 3);
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 4000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 32000, 0, 0, 0, 0, 0, 8000, 8000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 4000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// JAHIS 2022.07 事例集
        ///     独自事例C02 マル長+15(5,000)
        ///         マル長
        ///         　保険単独分を含む（＝配慮措置適用外）
        ///         　公費給付額を含まない
        /// </summary>
        [Test]
        public void T015_JAHIS2207_C02_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM(chokiFutan: 3);
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);

            //1日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 4000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 32000, 0, 0, 0, 0, 0, 8000, 8000);
            //2日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 6000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 48000, 2000, 0, 5000, 0, 0, 5000, 5000);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 1, 2, 3000, 0, 0, 0, 0, -3000, -3000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, true, false, false);
            AssertEqualToTensu(retReceInf, 10000, null, 6000, null, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null);
            AssertEqualToFutan(retReceInf, 15000, null, 5000, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// JAHIS 2022.07 事例集
        ///     独自事例C03 マル長+15(5,000)
        ///         マル長
        ///         　保険単独分を含む（＝配慮措置適用外）
        ///         　公費給付額を含む
        /// </summary>
        [Test]
        public void T015_JAHIS2207_C03_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);

            //1日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 4000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 32000, 0, 0, 0, 0, 0, 8000, 8000);
            //2日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 6000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 48000, 2000, 0, 5000, 0, 0, 5000, 5000);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 1, 2, 8000, 0, 0, 0, 0, -8000, -8000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, true, false, false);
            AssertEqualToTensu(retReceInf, 10000, null, 6000, null, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null);
            AssertEqualToFutan(retReceInf, 10000, null, 5000, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// JAHIS 2022.07 事例集
        ///     独自事例C04 マル長+15(5,000)
        ///         マル長
        ///         　保険単独分を含まない（＝配慮措置適用）
        ///         　公費給付額を含まない
        /// </summary>
        [Test]
        public void T015_JAHIS2207_C04_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM(chokiFutan: 3);
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);

            //1日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 4000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 32000, 1000, 0, 0, 0, 0, 7000, 7000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, false, false, false);
            AssertEqualToTensu(retReceInf, 4000, null, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 7000, null, null, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.False);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1318"));
        }

        /// <summary>
        /// JAHIS 2022.07 事例集
        ///     独自事例C05 マル長+15(5,000)
        ///         マル長
        ///         　保険単独分を含まない（＝配慮措置適用）
        ///         　公費給付額を含まない
        /// </summary>
        [Test]
        public void T015_JAHIS2207_C05_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM(chokiFutan: 3);
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);

            //1日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 4000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 32000, 1000, 0, 0, 0, 0, 7000, 7000);
            //2日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 6000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 48000, 2000, 0, 5000, 0, 0, 5000, 5000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, true, false, false);
            AssertEqualToTensu(retReceInf, 10000, null, 6000, null, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null);
            AssertEqualToFutan(retReceInf, 17000, null, 5000, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// JAHIS 2022.07 事例集
        ///     独自事例C06 マル長+15(5,000)
        ///         マル長
        ///         　保険単独分を含まない（＝配慮措置適用）
        ///         　公費給付額を含む
        /// </summary>
        [Test]
        public void T015_JAHIS2207_C06_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "102",
                monthLimitFutan: 10000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "15",
                monthLimitFutan: 5000
            );
            futancalcUT.NewHokenPattern(1, 0, 0, 0, 0);
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);

            //1日目 保険単独分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 4000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 4000, 32000, 1000, 0, 0, 0, 0, 7000, 7000);
            //2日目 15併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 6000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 6000, 48000, 2000, 0, 5000, 0, 0, 5000, 5000);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, false, true, false, false);
            AssertEqualToTensu(retReceInf, 10000, null, 6000, null, null);
            AssertEqualToKyufu(retReceInf, null, 10000, null, null);
            AssertEqualToFutan(retReceInf, 17000, null, 5000, null, null);
            Assert.That(retReceInf.TokkiContains("02"), Is.True);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// 大阪府 後期高齢者２割負担外来レセプトの計算事例 2022.07.20
        ///     【事例９】大阪府福祉医療費助成併用（府内保険者）
        /// </summary>
        [Test]
        public void T016_Osaka220720_09_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                hokensyaNo: "39270000",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTotalKbn: 2,
                kogakuHairyoKbn: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //2日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //3日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221003, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //4日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221004, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //5日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221005, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //6日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221006, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 3000, 3000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 3000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// 大阪府 後期高齢者２割負担外来レセプトの計算事例 2022.07.20
        ///     【事例１０】大阪府福祉医療費助成併用（府内保険者）
        /// </summary>
        [Test]
        public void T016_Osaka220720_10_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                hokensyaNo: "39270000",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTotalKbn: 2,
                kogakuHairyoKbn: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //2日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //3日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221003, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //4日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221004, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //5日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221005, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //6日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221006, tensu: 2500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2500, 20000, 0, 4500, 0, 0, 0, 500, 500);
            //7日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221007, tensu: 3000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 24000, 0, 6000, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 8000, 8000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, null, 3000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// 大阪府 後期高齢者２割負担外来レセプトの計算事例 2022.07.20
        ///     【事例１１】大阪府福祉医療費助成併用（府内保険者）
        /// </summary>
        [Test]
        public void T016_Osaka220720_11_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                hokensyaNo: "39270000",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTotalKbn: 2,
                kogakuHairyoKbn: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //2日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //3日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221003, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //4日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221004, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //5日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221005, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //6日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221006, tensu: 2500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2500, 20000, 0, 4500, 0, 0, 0, 500, 500);
            //7日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221007, tensu: 8000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 8000, 64000, 8000, 8000, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 13000, 13000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, 3000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// 大阪府 後期高齢者２割負担外来レセプトの計算事例 2022.07.20
        ///     【事例１２】大阪府福祉医療費助成併用（府内保険者）
        /// </summary>
        [Test]
        public void T016_Osaka220720_12_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                hokensyaNo: "39270000",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                //dayLimitFutan: 500,   １日で事例と結果を合わせるため除外
                monthLimitFutan: 3000,
                kogakuTotalKbn: 2,
                kogakuHairyoKbn: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 20000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 20000, 160000, 22000, 15000, 0, 0, 0, 3000, 3000);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 20000, 20000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, 3000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// 大阪府 後期高齢者２割負担外来レセプトの計算事例 2022.07.20
        ///     【事例１３】大阪府福祉医療費助成併用（府外保険者）
        /// </summary>
        [Test]
        public void T016_Osaka220720_13_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                hokensyaNo: "39280000",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTotalKbn: 2,
                kogakuHairyoKbn: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //2日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //3日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221003, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //4日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221004, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //5日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221005, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //6日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221006, tensu: 2500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2500, 20000, 2000, 2500, 0, 0, 0, 500, 500);
            //7日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221007, tensu: 3000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 3000, 24000, 3000, 3000, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 8000, 8000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 11000, 3000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// 大阪府 後期高齢者２割負担外来レセプトの計算事例 2022.07.20
        ///     【事例１４】大阪府福祉医療費助成併用（府外保険者）
        /// </summary>
        [Test]
        public void T016_Osaka220720_14_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                hokensyaNo: "39280000",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTotalKbn: 2,
                kogakuHairyoKbn: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 0, 0, 0);

            //1日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //2日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //3日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221003, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //4日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221004, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //5日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221005, tensu: 500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            //6日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221006, tensu: 2500);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2500, 20000, 2000, 2500, 0, 0, 0, 500, 500);
            //7日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221007, tensu: 8000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 8000, 64000, 8000, 8000, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 13000, 13000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 16000, 3000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));

            //以下追加・高額上限超
            //8日目
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221008, tensu: 5000);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 5000, 40000, 8000, 2000, 0, 0, 0, 0, 0);

            //レセ集計
            retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, false, false, false);
            AssertEqualToTensu(retReceInf, 18000, 18000, null, null, null);
            AssertEqualToKyufu(retReceInf, null, null, null, null);
            AssertEqualToFutan(retReceInf, 18000, 3000, null, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1328"));
        }

        /// <summary>
        /// 大阪府 後期高齢者２割負担外来レセプトの計算事例 2022.07.20
        ///     【追加事例A01】大阪府福祉医療費助成併用（府内保険者）
        /// </summary>
        [Test]
        public void T016_Osaka220720_A01_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                hokensyaNo: "39270000",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTotalKbn: 2,
                kogakuHairyoKbn: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 10000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 2000, 13000, 4500, 0, 0, 500, 500);
            //1日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 500, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 500, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221003, tensu: 500, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //4日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221004, tensu: 500, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //5日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221005, tensu: 500, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //6日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221006, tensu: 2500, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2500, 20000, 0, 4500, 0, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //7日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221007, tensu: 8000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 8000, 64000, 8000, 8000, 0, 0, 0, 0, 0);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 5000, -5000, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, true, false, false);
            AssertEqualToTensu(retReceInf, 23000, 10000, 23000, null, null);
            AssertEqualToKyufu(retReceInf, 18000, 18000, null, null);
            AssertEqualToFutan(retReceInf, 31000, 5000, 3000, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1338"));
        }

        /// <summary>
        /// 大阪府 後期高齢者２割負担外来レセプトの計算事例 2022.07.20
        ///     【追加事例A02】大阪府福祉医療費助成併用（府外保険者）
        /// </summary>
        [Test]
        public void T016_Osaka220720_A02_Kouki202210()
        {
            var futancalcUT = new FutancalcUT();

            var futanCalcVm = futancalcUT.NewFutanCalcVM();
            int prefNo = PrefCode.Osaka;

            futancalcUT.NewPtInf
            (
                futanVm: futanCalcVm,
                sinDate: 20221001,
                birthDay: 19400101
            );
            futancalcUT.NewPtHoken
            (
                prefNo: prefNo,
                honkeKbn: HonkeKbn.Mine,
                houbetu: "39",
                hokensyaNo: "39280000",
                kogakuKbn: 41
            );
            futancalcUT.NewPtKohi
            (
                prefNo: 0,
                houbetu: "54",
                monthLimitFutan: 5000
            );
            futancalcUT.NewPtKohi
            (
                prefNo: prefNo,
                houbetu: "80",
                hokenSbtKbn: HokenSbtKbn.Ippan,
                futanKbn: 1,
                dayLimitFutan: 500,
                monthLimitFutan: 3000,
                kogakuTotalKbn: 2,
                kogakuHairyoKbn: 1
            );
            futancalcUT.NewHokenPattern(1, 1, 2, 0, 0);
            futancalcUT.NewHokenPattern(1, 2, 0, 0, 0);

            //1日目 54併用分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 10000, hokenPid: 1);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 10000, 80000, 2000, 13000, 4500, 0, 0, 500, 500);
            //1日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221001, tensu: 500, hokenPid: 2, newRaiin: false);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 1000, 0, 0, 0, 0, 0);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //2日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221002, tensu: 500, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //3日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221003, tensu: 500, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //4日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221004, tensu: 500, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //5日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221005, tensu: 500, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 500, 4000, 0, 500, 0, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //6日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221006, tensu: 2500, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 2500, 20000, 2000, 2500, 0, 0, 0, 500, 500);
            Assert.That(futanCalcVm.AdjustDetails.Count, Is.Zero);
            //7日目 保険分
            futancalcUT.RunCalculate(futanVm: futanCalcVm, sinDate: 20221007, tensu: 8000, hokenPid: 2);
            futancalcUT.AssertEqualTo(futanCalcVm.KaikeiDetail, 8000, 64000, 8000, 8000, 0, 0, 0, 0, 0);
            futancalcUT.AssertEqualTo(futanCalcVm.AdjustDetails, 2, 2, 3000, -3000, 0, 0, 0, 0, 0);

            //レセ集計
            var retReceInf = runReceCalculate(futancalcUT, futanCalcVm, prefNo);

            AssertEqualToReceKisai(retReceInf, true, true, false, false);
            AssertEqualToTensu(retReceInf, 23000, 10000, 23000, null, null);
            AssertEqualToKyufu(retReceInf, 18000, 18000, null, null);
            AssertEqualToFutan(retReceInf, 31000, 5000, 3000, null, null);
            Assert.That(retReceInf.TokkiContains("41"), Is.True);
            Assert.That(retReceInf.ReceSbt, Is.EqualTo("1338"));
        }
    }
}
