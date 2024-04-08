using CommonChecker.Models;
using CommonChecker.Models.Futan;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.Models;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;

namespace CloudUnitTest.CommonChecker.Models
{
    public class CommonCheckerModelTest : BaseUT
    {
        [Test]
        public void TEST_001_OrdInfoDetailModel()
        {
            //Setup
            var ordInfDetail = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: "@REFILL",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);


            Assert.That("id1", Is.EqualTo(ordInfDetail.Id));
            Assert.That(20, Is.EqualTo(ordInfDetail.SinKouiKbn));
            Assert.That("@REFILL", Is.EqualTo(ordInfDetail.ItemCd));
            Assert.That("ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」", Is.EqualTo(ordInfDetail.ItemName));
            Assert.That(1, Is.EqualTo(ordInfDetail.Suryo));
            Assert.That("錠", Is.EqualTo(ordInfDetail.UnitName));
            Assert.That(0, Is.EqualTo(ordInfDetail.TermVal));
            Assert.That(2, Is.EqualTo(ordInfDetail.SyohoKbn));
            Assert.That(0, Is.EqualTo(ordInfDetail.SyohoLimitKbn));
            Assert.That(1, Is.EqualTo(ordInfDetail.DrugKbn));
            Assert.That(0, Is.EqualTo(ordInfDetail.YohoKbn));
            Assert.That("1124017F4", Is.EqualTo(ordInfDetail.IpnCd));
            Assert.That("きみがすごくきれいだよ。", Is.EqualTo(ordInfDetail.Bunkatu));
            Assert.That("Y", Is.EqualTo(ordInfDetail.MasterSbt));
            Assert.That(0, Is.EqualTo(ordInfDetail.BunkatuKoui));
        }

        /// <summary>
        /// Case YohoKbn > 0
        /// ItemCd != TouyakuChozaiNaiTon != TouyakuChozaiGai
        /// </summary>
        [Test]
        public void TEST_002_OrdInfoDetailModel_IsDrugUsage()
        {
            //Setup
            var ordInfDetail1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: "@REFILL",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            var ordInfDetail2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: "@REFILL",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: 1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            var ordInfDetail3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: "@REFILL",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: 99,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(ordInfDetail1.IsDrugUsage, false);
            Assert.AreEqual(ordInfDetail2.IsDrugUsage, true);
            Assert.AreEqual(ordInfDetail3.IsDrugUsage, true);
        }

        /// <summary>
        /// Case YohoKbn <= 0
        /// ItemCd == TouyakuChozaiNaiTon 
        /// </summary>
        [Test]
        public void TEST_003_OrdInfoDetailModel_IsDrugUsage()
        {
            //Setup
            var ordInfDetail1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            var ordInfDetail2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: ItemCdConst.TouyakuChozaiNaiTon,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: -1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            var ordInfDetail3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: ItemCdConst.TouyakuChozaiNaiTon,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(ordInfDetail1.IsDrugUsage, false);
            Assert.AreEqual(ordInfDetail2.IsDrugUsage, true);
            Assert.AreEqual(ordInfDetail3.IsDrugUsage, true);
        }

        /// <summary>
        /// Case YohoKbn <= 0
        /// ItemCd == TouyakuChozaiGai 
        /// </summary>
        [Test]
        public void TEST_004_OrdInfoDetailModel_IsDrugUsage()
        {
            //Setup
            var ordInfDetail1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            var ordInfDetail2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: ItemCdConst.TouyakuChozaiGai,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: -1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            var ordInfDetail3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: ItemCdConst.TouyakuChozaiGai,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(ordInfDetail1.IsDrugUsage, false);
            Assert.AreEqual(ordInfDetail2.IsDrugUsage, true);
            Assert.AreEqual(ordInfDetail3.IsDrugUsage, true);
        }

        /// <summary>
        /// Case SinKouiKbn == 20 && DrugKbn > 0
        /// ItemCd != TouyakuChozaiNaiTon, TouyakuChozaiGai
        /// ItemCd Not Is start with "Z"
        /// </summary>
        [Test]
        public void TEST_005_OrdInfoDetailModel_IsDrug()
        {
            //Setup
            var ordInfDetail1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            var ordInfDetail2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 21,
                                         itemCd: "1124017F4",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: -1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            var ordInfDetail3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: "@REFILL",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(ordInfDetail1.IsDrug, false);
            Assert.AreEqual(ordInfDetail2.IsDrug, false);
            Assert.AreEqual(ordInfDetail3.IsDrug, true);
        }

        /// <summary>
        /// Case (SinKouiKbn == 20 && DrugKbn > 0) is false
        /// ItemCd == TouyakuChozaiNaiTon,
        /// ItemCd != TouyakuChozaiGai
        /// ItemCd Not Is start with "Z"
        /// </summary>
        [Test]
        public void TEST_006_OrdInfoDetailModel_IsDrug()
        {
            //Setup
            var ordInfDetail1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            var ordInfDetail2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 21,
                                         itemCd: ItemCdConst.TouyakuChozaiNaiTon,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: -1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            var ordInfDetail3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: ItemCdConst.TouyakuChozaiNaiTon,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(ordInfDetail1.IsDrug, false);
            Assert.AreEqual(ordInfDetail2.IsDrug, true);
            Assert.AreEqual(ordInfDetail3.IsDrug, true);
        }

        /// <summary>
        /// Case (SinKouiKbn == 20 && DrugKbn > 0) is false
        /// ItemCd != TouyakuChozaiNaiTon,
        /// ItemCd == TouyakuChozaiGai
        /// ItemCd Not Is start with "Z"
        /// </summary>
        [Test]
        public void TEST_007_OrdInfoDetailModel_IsDrug()
        {
            //Setup
            var ordInfDetail1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            var ordInfDetail2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 21,
                                         itemCd: ItemCdConst.TouyakuChozaiGai,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: -1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            var ordInfDetail3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: ItemCdConst.TouyakuChozaiGai,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(ordInfDetail1.IsDrug, false);
            Assert.AreEqual(ordInfDetail2.IsDrug, true);
            Assert.AreEqual(ordInfDetail3.IsDrug, true);
        }

        /// <summary>
        /// Case (SinKouiKbn == 20 && DrugKbn > 0) is false
        /// ItemCd != TouyakuChozaiNaiTon,
        /// ItemCd != TouyakuChozaiGai
        /// ItemCd Is start with "Z"
        /// </summary>
        [Test]
        public void TEST_008_OrdInfoDetailModel_IsDrug()
        {
            //Setup
            var ordInfDetail1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 21,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            var ordInfDetail2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: "Zqweqweqe",
                                         itemName: "Zqweqweqe",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: -1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            var ordInfDetail3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: "zasdasdasd",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            var ordInfDetail4 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: "asdasdasd",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(ordInfDetail1.IsDrug, false);
            Assert.AreEqual(ordInfDetail2.IsDrug, true);
            Assert.AreEqual(ordInfDetail3.IsDrug, false);
            Assert.AreEqual(ordInfDetail4.IsDrug, false);
        }

        /// <summary>
        /// SinKouiKbn == 30 => IsInjection = true
        /// </summary>
        [Test]
        public void TEST_009_OrdInfoDetailModel_IsInjection()
        {
            //Setup
            var ordInfDetail1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 30,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            var ordInfDetail2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: "Zqweqweqe",
                                         itemName: "Zqweqweqe",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: -1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            var ordInfDetail3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 21,
                                         itemCd: "zasdasdasd",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            var ordInfDetail4 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 31,
                                         itemCd: "asdasdasd",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(ordInfDetail1.IsInjection, true);
            Assert.AreEqual(ordInfDetail2.IsInjection, false);
            Assert.AreEqual(ordInfDetail3.IsInjection, false);
            Assert.AreEqual(ordInfDetail4.IsInjection, false);
        }

        [Test]
        public void TEST_010_OrdInfoDetailModel_Is_Comment_Item()
        {
            //Setup

            var itemCd = "000010";
            //Is820Cmt true
            var ordInfDetail1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 30,
                                         itemCd: ItemCdConst.Comment820Pattern + itemCd,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            /// Is830Cmt is true
            var ordInfDetail2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: ItemCdConst.Comment830Pattern + itemCd,
                                         itemName: "Zqweqweqe",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: -1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            // Is831Cmt is true
            var ordInfDetail3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 21,
                                         itemCd: ItemCdConst.Comment831Pattern + itemCd,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Is850Cmt is true
            var ordInfDetail4 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 31,
                                         itemCd: ItemCdConst.Comment850Pattern + itemCd,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Is851Cmt is true
            var ordInfDetail5 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 31,
                                         itemCd: ItemCdConst.Comment851Pattern + itemCd,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Is852Cmt is true
            var ordInfDetail6 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 31,
                                         itemCd: ItemCdConst.Comment852Pattern + itemCd,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Is853Cmt is true
            var ordInfDetail7 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 31,
                                         itemCd: ItemCdConst.Comment853Pattern + itemCd,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Is840Cmt is true
            var ordInfDetail8 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 31,
                                         itemCd: ItemCdConst.Comment840Pattern + itemCd,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Is840Cmt is false
            var ordInfDetail9 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 31,
                                         itemCd: ItemCdConst.GazoDensibaitaiHozon,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Is842Cmt is false
            var ordInfDetail10 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 31,
                                         itemCd: ItemCdConst.Comment842Pattern + itemCd,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Is880Cmt is false
            var ordInfDetail11 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 31,
                                         itemCd: ItemCdConst.Comment880Pattern + itemCd,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //ItemCd is null
            var ordInfDetail12 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 31,
                                         itemCd: null,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(ordInfDetail1.Is820Cmt, true);
            Assert.AreEqual(ordInfDetail2.Is830Cmt, true);
            Assert.AreEqual(ordInfDetail3.Is831Cmt, true);
            Assert.AreEqual(ordInfDetail4.Is850Cmt, true);
            Assert.AreEqual(ordInfDetail5.Is851Cmt, true);
            Assert.AreEqual(ordInfDetail6.Is852Cmt, true);
            Assert.AreEqual(ordInfDetail7.Is853Cmt, true);
            Assert.AreEqual(ordInfDetail8.Is840Cmt, true);
            Assert.AreEqual(ordInfDetail9.Is840Cmt, false);
            Assert.AreEqual(ordInfDetail10.Is842Cmt, true);
            Assert.AreEqual(ordInfDetail11.Is880Cmt, true);
            Assert.True(ordInfDetail12.Is820Cmt == false &&
                        ordInfDetail12.Is830Cmt == false &&
                        ordInfDetail12.Is831Cmt == false &&
                        ordInfDetail12.Is850Cmt == false &&
                        ordInfDetail12.Is851Cmt == false &&
                        ordInfDetail12.Is852Cmt == false &&
                        ordInfDetail12.Is840Cmt == false &&
                        ordInfDetail12.Is842Cmt == false &&
                        ordInfDetail12.Is880Cmt == false
                );
        }

        /// <summary>
        /// SinKouiKbn == 100 => IsShohoComment = true
        /// </summary>
        [Test]
        public void TEST_011_OrdInfoDetailModel_IsShohoComment()
        {
            //Setup
            var ordInfDetail1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 100,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            var ordInfDetail2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 99,
                                         itemCd: "Zqweqweqe",
                                         itemName: "Zqweqweqe",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: -1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            var ordInfDetail3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 101,
                                         itemCd: "zasdasdasd",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            var ordInfDetail4 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: "asdasdasd",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(ordInfDetail1.IsShohoComment, true);
            Assert.AreEqual(ordInfDetail2.IsShohoComment, false);
            Assert.AreEqual(ordInfDetail3.IsShohoComment, false);
            Assert.AreEqual(ordInfDetail4.IsShohoComment, false);
        }

        /// <summary>
        /// SinKouiKbn == 101 => IsShohoBiko = true
        /// </summary>
        [Test]
        public void TEST_012_OrdInfoDetailModel_IsShohoBiko()
        {
            //Setup
            var ordInfDetail1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 101,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            var ordInfDetail2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 100,
                                         itemCd: "Zqweqweqe",
                                         itemName: "Zqweqweqe",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: -1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            var ordInfDetail3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 102,
                                         itemCd: "zasdasdasd",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            var ordInfDetail4 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 20,
                                         itemCd: "asdasdasd",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(ordInfDetail1.IsShohoBiko, true);
            Assert.AreEqual(ordInfDetail2.IsShohoBiko, false);
            Assert.AreEqual(ordInfDetail3.IsShohoBiko, false);
            Assert.AreEqual(ordInfDetail4.IsShohoBiko, false);
        }

        /// <summary>
        /// YohoKbn == 1 || ItemCd == ItemCdConst.TouyakuChozaiNaiTon || ItemCd == ItemCdConst.TouyakuChozaiGai
        /// => IsStandardUsage is true
        /// </summary>
        [Test]
        public void TEST_013_OrdInfoDetailModel_IsStandardUsage()
        {
            //Setup

            // YohoKbn == 1, ItemCd != TouyakuChozaiNaiTon, ItemCd != TouyakuChozaiGai

            var ordInfDetail1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 101,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 1,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            // YohoKbn != 1, ItemCd == TouyakuChozaiNaiTon, ItemCd != TouyakuChozaiGai
            var ordInfDetail2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 100,
                                         itemCd: ItemCdConst.TouyakuChozaiNaiTon,
                                         itemName: "Zqweqweqe",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 1,
                                         yohoKbn: 2,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            // YohoKbn != 1, ItemCd != TouyakuChozaiNaiTon, ItemCd == TouyakuChozaiGai
            var ordInfDetail3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 102,
                                         itemCd: ItemCdConst.TouyakuChozaiGai,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(ordInfDetail1.IsStandardUsage, true);
            Assert.AreEqual(ordInfDetail2.IsStandardUsage, true);
            Assert.AreEqual(ordInfDetail3.IsStandardUsage, true);
        }

        [Test]
        public void TEST_014_OrdInfoDetailModel_IsInjectionUsage()
        {
            //Setup

            /// CASE1: SinKouiKbn >= 31 && SinKouiKbn <= 34, MasterSbt <> "S", ItemCd not startwith "Z", 
            #region
            //fase
            var TC1_CASE1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 30,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 1,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //true
            var TC2_CASE1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 31,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 1,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //true
            var TC3_CASE1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 33,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 1,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //true
            var TC4_CASE1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 34,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 1,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            //false
            var TC5_CASE1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 35,
                                         itemCd: "Z1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 1,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "S",
                                         bunkatuKoui: 0);
            #endregion

            /// CASE2: SinKouiKbn = 30 && ItemCd StartWith "Z", MasterSbt = "S", ItemCd not startwith "Z", 
            #region
            //true
            var TC1_CASE2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 30,
                                         itemCd: "Z1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 1,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "S",
                                         bunkatuKoui: 0);

            //false
            var TC2_CASE2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 30,
                                         itemCd: "Z1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 1,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //false
            var TC3_CASE2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 30,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 1,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "S",
                                         bunkatuKoui: 0);

            #endregion

            //Assert
            Assert.AreEqual(TC1_CASE1.IsInjectionUsage, false);
            Assert.AreEqual(TC2_CASE1.IsInjectionUsage, true);
            Assert.AreEqual(TC3_CASE1.IsInjectionUsage, true);
            Assert.AreEqual(TC4_CASE1.IsInjectionUsage, true);
            Assert.AreEqual(TC5_CASE1.IsInjectionUsage, false);
            Assert.AreEqual(TC1_CASE2.IsInjectionUsage, true);
            Assert.AreEqual(TC2_CASE2.IsInjectionUsage, false);
            Assert.AreEqual(TC3_CASE2.IsInjectionUsage, false);
        }

        /// <summary>
        /// YohoKbn == 2 => IsSuppUsage = true
        /// </summary>
        [Test]
        public void TEST_015_OrdInfoDetailModel_IsSuppUsage()
        {
            //Setup
            var ordInfDetail1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 101,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 2,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            var ordInfDetail2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 101,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            var ordInfDetail3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 101,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 3,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(ordInfDetail1.IsSuppUsage, true);
            Assert.AreEqual(ordInfDetail2.IsSuppUsage, false);
            Assert.AreEqual(ordInfDetail3.IsSuppUsage, false);
        }

        [Test]
        public void TEST_016_OrdInfoDetailModel_DisplayedUnit()
        {
            //Setup
            /// case 1: Suryo == 0
            var case_1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 101,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 0,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            /// case 2: Suryo != 0
            var case_2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 101,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 99678999,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 1,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(string.Empty, case_1.DisplayedUnit);
            Assert.AreEqual("錠", case_2.DisplayedUnit);
        }

        [Test]
        public void TEST_017_OrdInfoDetailModel_DisplayedQuantity()
        {
            //Setup
            /// DisplayedUnit is empty
            var case_1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 101,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 0,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            /// case 2: Suryo != 0
            /// actual: DisplayedQuantity = "99678999"
            var case_2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 101,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 99678999,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 1,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            /// case 3: Suryo != 0, ItemCd =  Con_TouyakuOrSiBunkatu 
            /// actual: DisplayedQuantity = empty
            var case_3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 101,
                                         itemCd: ItemCdConst.Con_TouyakuOrSiBunkatu,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 99678999,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 1,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(string.Empty, case_1.DisplayedQuantity);
            Assert.AreEqual("99678999", case_2.DisplayedQuantity);
            Assert.AreEqual(string.Empty, case_3.DisplayedQuantity);
        }

        [Test]
        public void TEST_018_OrdInfoDetailModel_IsEmpty()
        {
            //Setup
            /// Actual IsEmpty = false
            /// ItemCd is empty
            var case_1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 0,
                                         itemCd: string.Empty,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 0,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            /// Actual IsEmpty = false
            /// ItemCd is null
            var case_2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 0,
                                         itemCd: null,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 0,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            /// Actual IsEmpty = false
            /// ItemName is null
            var case_3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 0,
                                         itemCd: "ABC",
                                         itemName: null,
                                         suryo: 0,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            /// Actual IsEmpty = false
            /// ItemName is empty
            var case_4 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 0,
                                         itemCd: "ABC",
                                         itemName: string.Empty,
                                         suryo: 0,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            /// Actual IsEmpty = false
            /// SinKouiKbn != 0
            var case_5 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 1,
                                         itemCd: "ABC",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 0,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            /// Actual IsEmpty = true
            var case_6 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 0,
                                         itemCd: "",
                                         itemName: "",
                                         suryo: 0,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(false, case_1.IsEmpty);
            Assert.AreEqual(false, case_2.IsEmpty);
            Assert.AreEqual(false, case_3.IsEmpty);
            Assert.AreEqual(false, case_4.IsEmpty);
            Assert.AreEqual(false, case_5.IsEmpty);
            Assert.AreEqual(true, case_6.IsEmpty);
        }

        [Test]
        public void TEST_019_OrdInfoDetailModel_IsUsage()
        {
            //Setup
            /// IsStandardUsage = true
            var case_1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 30,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 0,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            /// IsSuppUsage = true
            var case_2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 30,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 0,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 2,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            /// IsInjectionUsage = true
            var case_3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 31,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 0,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //Assert
            Assert.AreEqual(true, case_1.IsUsage);
            Assert.AreEqual(true, case_2.IsUsage);
            Assert.AreEqual(true, case_3.IsUsage);
        }

        [Test]
        public void TEST_020_OrdInfoDetailModel_ReleasedType()
        {
            //Setup
            var case_1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 30,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 0,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 0,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            var case_2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 30,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 0,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 1,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 1,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            //Assert
            Assert.AreEqual(ReleasedDrugType.None, case_1.ReleasedType);
            Assert.AreEqual(ReleasedDrugType.Unchangeable, case_2.ReleasedType);
        }

        [Test]
        public void TEST_021_OrdInfoDetailModel_DisplayItemName()
        {
            //Setup

            //ItemCd Is @Bunkatu
            var ordInfDetail1 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 30,
                                         itemCd: ItemCdConst.Con_TouyakuOrSiBunkatu,
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);

            //ItemCd != @Bunkatu
            var ordInfDetail2 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 30,
                                         itemCd: "ABC",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 2,
                                         syohoLimitKbn: 0,
                                         drugKbn: 0,
                                         yohoKbn: 0,
                                         ipnCd: "1124017F4",
                                         bunkatu: "きみがすごくきれいだよ。",
                                         masterSbt: "Y",
                                         bunkatuKoui: 0);
            //Assert
            Assert.AreEqual("ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」" + TenUtils.GetBunkatu(0, "きみがすごくきれいだよ。"), ordInfDetail1.DisplayItemName);
            Assert.AreEqual("ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」", ordInfDetail2.DisplayItemName);
        }

        [Test]
        public void TEST_022_AgeResultModel()
        {
            //Setup

            var ageResult = new AgeResultModel()
            {
                ItemCd = "ABC123!@#",
                YjCd = "!@#$YJCODE",
                TenpuLevel = "10",
                AttentionCmtCd = "0",
                WorkingMechanism = "0",
                Id = "0010",
            };

            //Assert
            Assert.AreEqual("ABC123!@#", ageResult.ItemCd);
            Assert.AreEqual("!@#$YJCODE", ageResult.YjCd);
            Assert.AreEqual("10", ageResult.TenpuLevel);
            Assert.AreEqual("0", ageResult.AttentionCmtCd);
            Assert.AreEqual("0", ageResult.WorkingMechanism);
            Assert.AreEqual("0010", ageResult.Id);
        }

        [Test]
        public void TEST_023_AutoCheckResultModel()
        {
            //Setup

            var test = new AutoCheckResultModel()
            {
                ItemCd = "ABC123!@#",
                ItemName = "きみがすごくきれいだよ。",
                Id = "0010",
            };

            //Assert
            Assert.AreEqual("ABC123!@#", test.ItemCd);
            Assert.AreEqual("きみがすごくきれいだよ。", test.ItemName);
            Assert.AreEqual("0010", test.Id);
        }

        [Test]
        public void TEST_024_DayLimitResultModel()
        {
            //Setup

            var test = new DayLimitResultModel()
            {
                ItemCd = "ABC123!@#",
                ItemName = "きみがすごくきれいだよ。",
                Id = "0010",
                LimitDay = 10.0,
                UsingDay = 10.0,
                YjCd = "0010",
            };

            //Assert
            Assert.AreEqual("ABC123!@#", test.ItemCd);
            Assert.AreEqual("きみがすごくきれいだよ。", test.ItemName);
            Assert.AreEqual("0010", test.Id);
            Assert.AreEqual(10.0, test.UsingDay);
            Assert.AreEqual(10.0, test.LimitDay);
        }

        [Test]
        public void TEST_025_DiseaseResultModel()
        {
            //Setup

            var diseaseResult = new DiseaseResultModel()
            {
                DiseaseType = 3,
                CmtCd = "0010",
                Id = "0011",
                TenpuLevel = 10,
                ByotaiCd = "0012",
                KijyoCd = "0013",
                YjCd = "0014",
            };

            // Act & Assert
            Assert.AreEqual(3, diseaseResult.DiseaseType);
            Assert.AreEqual("0010", diseaseResult.CmtCd);
            Assert.AreEqual("0011", diseaseResult.Id);
            Assert.AreEqual("0012", diseaseResult.ByotaiCd);
            Assert.AreEqual(10, diseaseResult.TenpuLevel);
            Assert.AreEqual("0013", diseaseResult.KijyoCd);
            Assert.AreEqual("0014", diseaseResult.YjCd);
        }

        [Test]
        public void TEST_026_DosageResultModel()
        {
            //Setup

            var test = new DosageResultModel()
            {
                ItemCd = "00110",
                ItemName = "ItemName",
                Id = "0011",
                CurrentValue = 10.0,
                SuggestedValue = 9.0,
                UnitName = "Unit Name",
                YjCd = "0014",
                LabelChecking = DosageLabelChecking.OneMin,
            };

            // Act & Assert
            Assert.AreEqual("ItemName", test.ItemName);
            Assert.AreEqual("00110", test.ItemCd);
            Assert.AreEqual("0011", test.Id);
            Assert.AreEqual(10.0, test.CurrentValue);
            Assert.AreEqual(9.0, test.SuggestedValue);
            Assert.AreEqual("Unit Name", test.UnitName);
            Assert.AreEqual("0014", test.YjCd);
            Assert.AreEqual(DosageLabelChecking.OneMin, test.LabelChecking);
            Assert.AreEqual(false, test.IsFromUserDefined);
        }

        [Test]
        public void TEST_027_DrugAllergyResultModel()
        {
            //Setup

            var test = new DrugAllergyResultModel()
            {
                Level = 10,
                ItemCd = "ItemName",
                YjCd = "0011",
                AllergyItemCd = "00112",
                AllergyYjCd = "ALLERGYYJCD",
                SeibunCd = "SEIBUNCODE",
                AllergySeibunCd = "0012",
                Tag = "0013",
                SeqNo = "0014",
            };

            // Act & Assert
            Assert.AreEqual(10, test.Level);
            Assert.AreEqual("ItemName", test.ItemCd);
            Assert.AreEqual("0011", test.YjCd);
            Assert.AreEqual("00112", test.AllergyItemCd);
            Assert.AreEqual("ALLERGYYJCD", test.AllergyYjCd);
            Assert.AreEqual("SEIBUNCODE", test.SeibunCd);
            Assert.AreEqual("0012", test.AllergySeibunCd);
            Assert.AreEqual("0013", test.Tag);
            Assert.AreEqual("0014", test.SeqNo);
        }

        [Test]
        public void TEST_028_DrugInfo()
        {
            //Setup

            var test = new DrugInfo()
            {
                Id = "001",
                UsageQuantity = 10.0,
                ItemCD = "@REFILL",
                ItemName = "ITEM NAME TEST",
                Suryo = 9.0,
                UnitName = "YAKKA"
            };

            // Act & Assert
            Assert.AreEqual(10.0, test.UsageQuantity);
            Assert.AreEqual("001", test.Id);
            Assert.AreEqual("@REFILL", test.ItemCD);
            Assert.AreEqual("ITEM NAME TEST", test.ItemName);
            Assert.AreEqual(9.0, test.Suryo);
            Assert.AreEqual("YAKKA", test.UnitName);
        }

        [Test]
        public void TEST_029_DuplicationResultModel()
        {
            //Setup

            var test = new DuplicationResultModel()
            {
                Level = 3,
                ItemCd = "@REFILL",
                DuplicatedItemCd = "830011000",
                IsIppanCdDuplicated = true,
                IppanCode = "830011003",
                SeibunCd = "830011004",
                AllergySeibunCd = "830011005",
                Tag = "830011006",
            };

            // Act & Assert
            Assert.AreEqual(false, test.IsComponentDuplicated);
            Assert.AreEqual("", test.Id);
            Assert.AreEqual(3, test.Level);
            Assert.AreEqual("@REFILL", test.ItemCd);
            Assert.AreEqual("830011000", test.DuplicatedItemCd);
            Assert.AreEqual(true, test.IsIppanCdDuplicated);
            Assert.AreEqual("830011003", test.IppanCode);
            Assert.AreEqual("830011004", test.SeibunCd);
            Assert.AreEqual("830011005", test.AllergySeibunCd);
            Assert.AreEqual("830011006", test.Tag);
        }

        [Test]
        public void TEST_030_ErrorInfoModel()
        {
            //Setup

            var test = new ErrorInfoModel()
            {
                Id = "1",
                FirstCellContent = "Duplicate item",
                SecondCellContent = "Duplicate item @REFILL",
                ThridCellContent = "Cannot Order",
                FourthCellContent = "",
                HighlightColorCode = "#000099",
                SuggestedContent = "Suggest",
                CheckingItemCd = "8301110",
                CurrentItemCd = "8301112",
                ErrorType = CommonCheckerType.DuplicationChecker,
                ListLevelInfo = new(),
            };

            // Act & Assert
            Assert.AreEqual("1", test.Id);
            Assert.AreEqual("Duplicate item", test.FirstCellContent);
            Assert.AreEqual("Duplicate item @REFILL", test.SecondCellContent);
            Assert.AreEqual("Cannot Order", test.ThridCellContent);
            Assert.AreEqual("", test.FourthCellContent);
            Assert.AreEqual("#000099", test.HighlightColorCode);
            Assert.AreEqual("Suggest", test.SuggestedContent);
            Assert.AreEqual("8301110", test.CheckingItemCd);
            Assert.AreEqual("8301112", test.CurrentItemCd);
            Assert.AreEqual(CommonCheckerType.DuplicationChecker, test.ErrorType);
            Assert.AreEqual(0, test.ListLevelInfo.Count);
        }

        [Test]
        public void TEST_031_FoodAllergyResultModel()
        {
            //Setup

            var test = new FoodAllergyResultModel()
            {
                PtId = 123111111111111,
                AlrgyKbn = "Duplicate item",
                ItemCd = "Duplicate item @REFILL",
                YjCd = "Cannot Order",
                TenpuLevel = "3",
                AttentionCmt = "#000099",
                WorkingMechanism = "Suggest",
            };

            // Act & Assert
            Assert.AreEqual("", test.Id);
            Assert.AreEqual("Duplicate item", test.AlrgyKbn);
            Assert.AreEqual("Duplicate item @REFILL", test.ItemCd);
            Assert.AreEqual("Cannot Order", test.YjCd);
            Assert.AreEqual("3", test.TenpuLevel);
            Assert.AreEqual("#000099", test.AttentionCmt);
            Assert.AreEqual("Suggest", test.WorkingMechanism);
            Assert.AreEqual(123111111111111, test.PtId);
        }

        [Test]
        public void TEST_032_InvalidDataOrder()
        {
            //Setup

            var test = new InvalidDataOrder()
            {
                ErrorType = ErrorType.Quantity,
                ItemName = "item @REFILL",
            };

            // Act & Assert
            Assert.AreEqual(ErrorType.Quantity, test.ErrorType);
            Assert.AreEqual("item @REFILL", test.ItemName);
        }

        [Test]
        public void TEST_033_ItemCodeModel()
        {
            //Setup

            var test = new ItemCodeModel("12345", "ID5");

            // Act & Assert
            Assert.AreEqual("12345", test.ItemCd);
            Assert.AreEqual("ID5", test.Id);
        }

        [Test]
        public void TEST_034_KinkiErrorDetail()
        {
            //Setup

            var test = new KinkiErrorDetail()
            {
                Level = 3,
                CommentContent = "ABC",
                SayokijyoContent = "Sayoki content",
            };

            // Act & Assert
            Assert.AreEqual(3, test.Level);
            Assert.AreEqual("ABC", test.CommentContent);
            Assert.AreEqual("Sayoki content", test.SayokijyoContent);
        }

        [Test]
        public void TEST_035_LevelInfoModel()
        {
            //Setup

            var test = new LevelInfoModel()
            {
                Level = 3,
                Title = "ABC",
                BackgroundCode = "#000001F",
                BorderBrushCode = "#000011F",
                FirstItemName = "@REFILL",
                SecondItemName = "DRUG",
                Comment = "ITEM Refill",
            };

            // Act & Assert
            Assert.AreEqual(3, test.Level);
            Assert.AreEqual("ABC", test.Title);
            Assert.AreEqual("#000001F", test.BackgroundCode);
            Assert.AreEqual("#000011F", test.BorderBrushCode);
            Assert.AreEqual("@REFILL", test.FirstItemName);
            Assert.AreEqual("DRUG", test.SecondItemName);
            Assert.AreEqual("ITEM Refill", test.Comment);
            Assert.AreEqual("@REFILL × DRUG", test.Caption);
            Assert.AreEqual(true, test.IsShowLevelButton);
        }

        [Test]
        public void TEST_036_PtAlrgyDrugModel()
        {
            //Setup

            var test = new PtAlrgyDrugModel(1, 1231, 1, 1, "@REFILL", "DRUG REFILL", 20230101, 20231212, "Item cmt", 1);

            // Act & Assert
            Assert.AreEqual(1, test.HpId);
            Assert.AreEqual(1231, test.PtId);
            Assert.AreEqual(1, test.SeqNo);
            Assert.AreEqual(1, test.SortNo);
            Assert.AreEqual("@REFILL", test.ItemCd);
            Assert.AreEqual("DRUG REFILL", test.DrugName);
            Assert.AreEqual(20230101, test.StartDate);
            Assert.AreEqual(20231212, test.EndDate);
            Assert.AreEqual("Item cmt", test.Cmt);
            Assert.AreEqual(1, test.IsDeleted);
        }

        [Test]
        public void TEST_037_PtAlrgyFoodModel()
        {
            //Setup

            var test = new PtAlrgyFoodModel(1, 1231, 1, 1, "@REFILL", 20230101, 20231212, "Item cmt", 1, "CAKE");

            // Act & Assert
            Assert.AreEqual(1, test.HpId);
            Assert.AreEqual(1231, test.PtId);
            Assert.AreEqual(1, test.SeqNo);
            Assert.AreEqual(1, test.SortNo);
            Assert.AreEqual("@REFILL", test.AlrgyKbn);
            Assert.AreEqual(20230101, test.StartDate);
            Assert.AreEqual(20231212, test.EndDate);
            Assert.AreEqual("Item cmt", test.Cmt);
            Assert.AreEqual(1, test.IsDeleted);
            Assert.AreEqual("CAKE", test.FoodName);
        }

        [Test]
        public void TEST_038_PtKioRekiModel()
        {
            //Setup

            var test = new PtKioRekiModel(1, 1231, 1, 1, "@REFILL", "1234F", "1235F", 20230110, "CAKE", 1);

            // Act & Assert
            Assert.AreEqual(1, test.HpId);
            Assert.AreEqual(1231, test.PtId);
            Assert.AreEqual(1, test.SeqNo);
            Assert.AreEqual(1, test.SortNo);
            Assert.AreEqual("@REFILL", test.ByomeiCd);
            Assert.AreEqual("1234F", test.ByotaiCd);
            Assert.AreEqual("1235F", test.Byomei);
            Assert.AreEqual(20230110, test.StartDate);
            Assert.AreEqual("CAKE", test.Cmt);
            Assert.AreEqual(1, test.IsDeleted);
        }

        [Test]
        public void TEST_039_PtOtcDrugModel()
        {
            //Setup

            var test = new PtOtcDrugModel(1, 1231, 1, 1, 8400, "1234F", 20230110, 20241212, "CAKE", 1);

            // Act & Assert
            Assert.AreEqual(1, test.HpId);
            Assert.AreEqual(1231, test.PtId);
            Assert.AreEqual(1, test.SeqNo);
            Assert.AreEqual(1, test.SortNo);
            Assert.AreEqual(8400, test.SerialNum);
            Assert.AreEqual("1234F", test.TradeName);
            Assert.AreEqual(20230110, test.StartDate);
            Assert.AreEqual(20241212, test.EndDate);
            Assert.AreEqual("CAKE", test.Cmt);
            Assert.AreEqual(1, test.IsDeleted);
        }

        [Test]
        public void TEST_040_PtOtherDrugModel()
        {
            //Setup

            var test = new PtOtherDrugModel(1, 1231, 1, 1, "1234F", "REFILL Drug", 20230101, 20231212, "Item Cmt", 1);

            // Act & Assert
            Assert.AreEqual(1, test.HpId);
            Assert.AreEqual(1231, test.PtId);
            Assert.AreEqual(1, test.SeqNo);
            Assert.AreEqual(1, test.SortNo);
            Assert.AreEqual("1234F", test.ItemCd);
            Assert.AreEqual("REFILL Drug", test.DrugName);
            Assert.AreEqual(20230101, test.StartDate);
            Assert.AreEqual(20231212, test.EndDate);
            Assert.AreEqual("Item Cmt", test.Cmt);
            Assert.AreEqual(1, test.IsDeleted);
        }

        [Test]
        public void TEST_041_PtSuppleModel()
        {
            //Setup

            var test = new PtSuppleModel(1, 1231, 1, 1, "1234F", "REFILL Drug", 20230101, 20231212, "Item Cmt", 1);

            // Act & Assert
            Assert.AreEqual(1, test.HpId);
            Assert.AreEqual(1231, test.PtId);
            Assert.AreEqual(1, test.SeqNo);
            Assert.AreEqual(1, test.SortNo);
            Assert.AreEqual("1234F", test.IndexCd);
            Assert.AreEqual("REFILL Drug", test.IndexWord);
            Assert.AreEqual(20230101, test.StartDate);
            Assert.AreEqual(20231212, test.EndDate);
            Assert.AreEqual("Item Cmt", test.Cmt);
            Assert.AreEqual(1, test.IsDeleted);
        }

        [Test]
        public void TEST_042_OrdInfoModel_Test_IsDrug()
        {
            //Setup

            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "UT", "Item Name", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 21, "UT", "Item Name", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var test1 = new OrdInfoModel(21, 0, ordInfDetails);
            var test2 = new OrdInfoModel(22, 0, ordInfDetails);
            var test3 = new OrdInfoModel(23, 0, ordInfDetails);
            var test4 = new OrdInfoModel(19, 0, ordInfDetails);
            var test5 = new OrdInfoModel(24, 0, ordInfDetails);

            //Assert
            Assert.AreEqual(true, test1.IsDrug);
            Assert.AreEqual(true, test2.IsDrug);
            Assert.AreEqual(true, test3.IsDrug);
            Assert.AreEqual(false, test4.IsDrug);
            Assert.AreEqual(false, test5.IsDrug);
        }

        [Test]
        public void TEST_043_OrdInfoModel_Test_IsInjection()
        {
            //Setup

            var ordInfDetails = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "UT", "Item Name", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 21, "UT", "Item Name", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var test1 = new OrdInfoModel(30, 0, ordInfDetails);
            var test2 = new OrdInfoModel(31, 0, ordInfDetails);
            var test3 = new OrdInfoModel(34, 0, ordInfDetails);
            var test4 = new OrdInfoModel(35, 0, ordInfDetails);
            var test5 = new OrdInfoModel(29, 0, ordInfDetails);

            //Assert
            Assert.AreEqual(true, test1.IsInjection);
            Assert.AreEqual(true, test2.IsInjection);
            Assert.AreEqual(true, test3.IsInjection);
            Assert.AreEqual(false, test4.IsInjection);
            Assert.AreEqual(false, test5.IsInjection);
        }

        [Test]
        public void TEST_044_OrdInfoModel_Test_OdrInfDetailModelsIgnoreEmpty()
        {
            //Setup

            List<OrdInfoDetailModel> ordInfDetails1 = null;

            var ordInfDetails2 = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 20, "UT", "Item Name", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 21, "UT", "Item Name", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var ordInfDetails3 = new List<OrdInfoDetailModel>()
            {
                new OrdInfoDetailModel("id1", 0, "", "", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
                new OrdInfoDetailModel("id2", 0, "UT", "Item Name", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
            };

            var test1 = new OrdInfoModel(30, 0, ordInfDetails1);
            var test2 = new OrdInfoModel(31, 0, ordInfDetails2);
            var test3 = new OrdInfoModel(34, 0, ordInfDetails3);

            //Assert
            Assert.AreEqual(0, test1.OdrInfDetailModelsIgnoreEmpty.Count);
            Assert.AreEqual(true, test2.OdrInfDetailModelsIgnoreEmpty.Count == 2);
            Assert.AreEqual(true, test3.OdrInfDetailModelsIgnoreEmpty.Count == 1);
        }

        [Test]
        public void TEST_045_SanteiCntCheckModel()
        {
            //Setup
            var santeicntCheck1 = new SanteiCntCheck()
            {
                TermCnt = 1,
                TermSbt = 2,
                CntType = 3,
                MaxCnt = 123456789,
                TargetCd = "1234F"
            };

            var santeicntCheck2 = new SanteiCntCheck()
            {
                TargetCd = null
            };

            var test1 = new SanteiCntCheckModel(santeicntCheck1);
            var test2 = new SanteiCntCheckModel(santeicntCheck2);
            Assert.AreEqual(1, test1.TermCnt);
            Assert.AreEqual(2, test1.TermSbt);
            Assert.AreEqual(3, test1.CntType);
            Assert.AreEqual(123456789, test1.MaxCnt);
            Assert.AreEqual("1234F", test1.TargetCd);
            Assert.AreEqual("", test2.TargetCd);
        }

        [Test]
        public void TEST_046_SanteiGrpDetailModel()
        {
            //Setup
            

            var santeiGrpDetail = new SanteiGrpDetail()
            {
                SanteiGrpCd = 2
            };

            var test = new SanteiGrpDetailModel(santeiGrpDetail);

            Assert.AreEqual(2, test.SanteiGroupCd);
        }

        [Test]
        public void TEST_047_LevelInfoModel()
        {
            //Setup

            var test = new LevelInfoModel()
            {
                Level = 3,
                Title = "ABC",
                BackgroundCode = "#000001F",
                BorderBrushCode = "#000011F",
                FirstItemName = "@REFILL",
                SecondItemName = "",
                Comment = "ITEM Refill",
            };

            var test2 = new LevelInfoModel()
            {
                Level = 3,
                Title = "ABC",
                BackgroundCode = "#000001F",
                BorderBrushCode = "#000011F",
                FirstItemName = "@REFILL",
                SecondItemName = null,
                Comment = "ITEM Refill",
            };
            // Act & Assert
            Assert.AreEqual(3, test.Level);
            Assert.AreEqual("ABC", test.Title);
            Assert.AreEqual("#000001F", test.BackgroundCode);
            Assert.AreEqual("#000011F", test.BorderBrushCode);
            Assert.AreEqual("@REFILL", test.FirstItemName);
            Assert.AreEqual("", test.SecondItemName);
            Assert.AreEqual("ITEM Refill", test.Comment);
            Assert.AreEqual("@REFILL", test.Caption);
            Assert.AreEqual(true, test.IsShowLevelButton);
            Assert.AreEqual("@REFILL", test2.Caption);
        }
    }
}
