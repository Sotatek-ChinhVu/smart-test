using CommonChecker.Models.OrdInfDetailModel;
using Helper.Constants;

namespace CloudUnitTest.CommonChecker.Models.OdrInf
{
    public class OrdInfoModelTest : BaseUT
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


            Assert.AreEqual(ordInfDetail.Id, "id1");
            Assert.AreEqual(ordInfDetail.SinKouiKbn, 20);
            Assert.AreEqual(ordInfDetail.ItemCd, "@REFILL");
            Assert.AreEqual(ordInfDetail.ItemName, "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」");
            Assert.AreEqual(ordInfDetail.Suryo, 1);
            Assert.AreEqual(ordInfDetail.UnitName, "錠");
            Assert.AreEqual(ordInfDetail.TermVal, 0);
            Assert.AreEqual(ordInfDetail.SyohoKbn, 2);
            Assert.AreEqual(ordInfDetail.SyohoLimitKbn, 0);
            Assert.AreEqual(ordInfDetail.DrugKbn, 1);
            Assert.AreEqual(ordInfDetail.YohoKbn, 0);
            Assert.AreEqual(ordInfDetail.IpnCd, "1124017F4");
            Assert.AreEqual(ordInfDetail.Bunkatu, "きみがすごくきれいだよ。");
            Assert.AreEqual(ordInfDetail.MasterSbt, "Y");
            Assert.AreEqual(ordInfDetail.BunkatuKoui, 0);
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
                                         sinKouiKbn: 101,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
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

            var ordInfDetail3 = new OrdInfoDetailModel(
                                         id: "id1",
                                         sinKouiKbn: 101,
                                         itemCd: "1234",
                                         itemName: "ドネペジル塩酸塩ＯＤ錠５ｍｇ「ＤＳＰ」",
                                         suryo: 1,
                                         unitName: "錠",
                                         termVal: 0,
                                         syohoKbn: 3,
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
    }
}
