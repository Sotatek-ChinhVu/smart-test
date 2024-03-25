using Domain.Models.SuperSetDetail;
using Helper.Constants;

namespace CloudUnitTest.Model.SetOrderInf
{
    public class SetOrderInfDetailModelTest
    {
        [Test]
        public void SetOrderInfDetailModel_001_IsSpecialItem()
        {
            string masterSbt = "S";
            int sinKouiKbn = 20;
            int drugKbn = 0;
            string itemCd = "test";
            #region Data Sample

            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                sinKouiKbn, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                drugKbn, // drugKbn
                5, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                masterSbt, // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(model.IsSpecialItem);
        }

        [Test]
        public void SetOrderInfDetailModel_002_IsSpecialItem()
        {
            string masterSbt = "S";
            int sinKouiKbn = 20;
            int drugKbn = 0;
            string itemCd = ItemCdConst.Con_TouyakuOrSiBunkatu;
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                sinKouiKbn, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                drugKbn, // drugKbn
                5, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                masterSbt, // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(!model.IsSpecialItem);
        }

        [Test]
        public void SetOrderInfDetailModel_003_IsSpecialItem()
        {
            string masterSbt = "A";
            int sinKouiKbn = 20;
            int drugKbn = 0;
            string itemCd = "test";
            #region Data Sample

            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                sinKouiKbn, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                drugKbn, // drugKbn
                5, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                masterSbt, // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(!model.IsSpecialItem);
        }

        [Test]
        public void SetOrderInfDetailModel_004_IsSpecialItem()
        {
            string masterSbt = "S";
            int sinKouiKbn = 21;
            int drugKbn = 0;
            string itemCd = "test";
            #region Data Sample

            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                sinKouiKbn, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                drugKbn, // drugKbn
                5, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                masterSbt, // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(!model.IsSpecialItem);
        }

        [Test]
        public void SetOrderInfDetailModel_005_IsSpecialItem()
        {
            string masterSbt = "S";
            int sinKouiKbn = 20;
            int drugKbn = 1;
            string itemCd = "test";
            #region Data Sample

            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                sinKouiKbn, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                drugKbn, // drugKbn
                5, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                masterSbt, // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(!model.IsSpecialItem);
        }

        [Test]
        public void SetOrderInfDetailModel_006_IsYoho()
        {
            int yohoKbn = 1;
            #region Data Sample

            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                0, // sinKouiKbn
                "test", // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                0, // drugKbn
                yohoKbn, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(model.IsYoho);
        }

        [Test]
        public void SetOrderInfDetailModel_007_IsYoho()
        {
            int yohoKbn = 0;
            #region Data Sample

            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                0, // sinKouiKbn
                "test", // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                0, // drugKbn
                yohoKbn, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(!model.IsYoho);
        }

        [Test]
        public void SetOrderInfDetailModel_008_IsKensa()
        {
            List<int> sinKouiKbn = new List<int>() { 61, 64 };
            foreach (int i in sinKouiKbn)
            {
                #region Data Sample
                SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                    1, // hpId
                    2, // setCd
                    1000, // rpNo
                    1, // rpEdaNo
                    1, // rowNo
                    i, // sinKouiKbn
                    "test", // itemCd
                    "itemName", // itemName
                    "displayItemName", // displayItemName
                    5.0, // suryo
                    "unitName", // unitName
                    2, // unitSBT
                    3.0, // termVal
                    1, // kohatuKbn
                    2, // syohoKbn
                    3, // syohoLimitKbn
                    0, // drugKbn
                    0, // yohoKbn
                    "kokuji1", // kokuji1
                    "kokuji2", // kokuji2
                    1, // isNodspRece
                    "ipnCd", // ipnCd
                    "ipnName", // ipnName
                    "bunkatu", // bunkatu
                    "cmtName", // cmtName
                    "cmtOpt", // cmtOpt
                    "fontColor", // fontColor
                    1, // commentNewline
                    "masterSbt", // masterSbt
                    1, // inOutKbn
                    10.0, // yakka
                    true, // isGetPriceInYakka
                    20.0, // ten
                    1, // bunkatuKoui
                    2, // alternationIndex
                    1, // kensaGaichu
                    30.0, // odrTermVal
                    40.0, // cnvTermVal
                    "yjCd", // yjCd
                    "centerItemCd1", // centerItemCd1
                    "centerItemCd2", // centerItemCd2
                    1, // kasan1
                    2, // kasan2
                    new(), // yohoSets
                    1, // cmtColKeta1
                    2, // cmtColKeta2
                    3, // cmtColKeta3
                    4, // cmtColKeta4
                    5, // cmtCol1
                    6, // cmtCol2
                    7, // cmtCol3
                    8, // cmtCol4
                    1, // handanGrpKbn
                    true // isKensaMstEmpty
                );
                #endregion
                Assert.True(model.IsKensa);
            }

        }

        [Test]
        public void SetOrderInfDetailModel_009_IsKensa()
        {
            List<int> sinKouiKbn = new List<int>() { 0, 60, 62, 63, 65 };
            foreach (int i in sinKouiKbn)
            {
                #region Data Sample
                SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                    1, // hpId
                    2, // setCd
                    1000, // rpNo
                    1, // rpEdaNo
                    1, // rowNo
                    i, // sinKouiKbn
                    "test", // itemCd
                    "itemName", // itemName
                    "displayItemName", // displayItemName
                    5.0, // suryo
                    "unitName", // unitName
                    2, // unitSBT
                    3.0, // termVal
                    1, // kohatuKbn
                    2, // syohoKbn
                    3, // syohoLimitKbn
                    0, // drugKbn
                    0, // yohoKbn
                    "kokuji1", // kokuji1
                    "kokuji2", // kokuji2
                    1, // isNodspRece
                    "ipnCd", // ipnCd
                    "ipnName", // ipnName
                    "bunkatu", // bunkatu
                    "cmtName", // cmtName
                    "cmtOpt", // cmtOpt
                    "fontColor", // fontColor
                    1, // commentNewline
                    "masterSbt", // masterSbt
                    1, // inOutKbn
                    10.0, // yakka
                    true, // isGetPriceInYakka
                    20.0, // ten
                    1, // bunkatuKoui
                    2, // alternationIndex
                    1, // kensaGaichu
                    30.0, // odrTermVal
                    40.0, // cnvTermVal
                    "yjCd", // yjCd
                    "centerItemCd1", // centerItemCd1
                    "centerItemCd2", // centerItemCd2
                    1, // kasan1
                    2, // kasan2
                    new(), // yohoSets
                    1, // cmtColKeta1
                    2, // cmtColKeta2
                    3, // cmtColKeta3
                    4, // cmtColKeta4
                    5, // cmtCol1
                    6, // cmtCol2
                    7, // cmtCol3
                    8, // cmtCol4
                    1, // handanGrpKbn
                    true // isKensaMstEmpty
                );
                #endregion
                Assert.True(!model.IsKensa);
            }

        }

        [Test]
        public void SetOrderInfDetailModel_010_IsDrug()
        {
            int sinKouiKbn = 20;
            int drugKbn = 1;
            string itemCd = "test";
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                sinKouiKbn, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                drugKbn, // drugKbn
                0, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(model.IsDrug);
        }

        [Test]
        public void SetOrderInfDetailModel_011_IsDrug()
        {
            int sinKouiKbn = 21;
            int drugKbn = 1;
            string itemCd = "test";
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                sinKouiKbn, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                drugKbn, // drugKbn
                0, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(!model.IsDrug);
        }

        [Test]
        public void SetOrderInfDetailModel_012_IsDrug()
        {
            int sinKouiKbn = 20;
            int drugKbn = 0;
            string itemCd = ItemCdConst.TouyakuChozaiNaiTon;
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                sinKouiKbn, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                drugKbn, // drugKbn
                0, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(model.IsDrug);
        }

        [Test]
        public void SetOrderInfDetailModel_013_IsDrug()
        {
            int sinKouiKbn = 20;
            int drugKbn = 0;
            string itemCd = ItemCdConst.TouyakuChozaiGai;
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                sinKouiKbn, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                drugKbn, // drugKbn
                0, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(model.IsDrug);
        }

        [Test]
        public void SetOrderInfDetailModel_014_IsDrug()
        {
            int sinKouiKbn = 20;
            int drugKbn = 0;
            string itemCd = "Zalo";
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                sinKouiKbn, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                drugKbn, // drugKbn
                0, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(model.IsDrug);
        }

        [Test]
        public void SetOrderInfDetailModel_015_IsDrug()
        {
            int sinKouiKbn = 19;
            int drugKbn = 0;
            string itemCd = "Zalo";
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                sinKouiKbn, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                drugKbn, // drugKbn
                0, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(!model.IsDrug);
        }

        [Test]
        public void SetOrderInfDetailModel_016_IsInjection()
        {
            int sinKouiKbn = 30;
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                sinKouiKbn, // sinKouiKbn
                "test", // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                0, // drugKbn
                0, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(model.IsInjection);
        }

        [Test]
        public void SetOrderInfDetailModel_017_IsInjection()
        {
            List<int> sinKouiKbn = new List<int>() { 29, 31 };
            foreach (int i in sinKouiKbn)
            {
                #region Data Sample
                SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                    1, // hpId
                    2, // setCd
                    1000, // rpNo
                    1, // rpEdaNo
                    1, // rowNo
                    i, // sinKouiKbn
                    "test", // itemCd
                    "itemName", // itemName
                    "displayItemName", // displayItemName
                    5.0, // suryo
                    "unitName", // unitName
                    2, // unitSBT
                    3.0, // termVal
                    1, // kohatuKbn
                    2, // syohoKbn
                    3, // syohoLimitKbn
                    0, // drugKbn
                    0, // yohoKbn
                    "kokuji1", // kokuji1
                    "kokuji2", // kokuji2
                    1, // isNodspRece
                    "ipnCd", // ipnCd
                    "ipnName", // ipnName
                    "bunkatu", // bunkatu
                    "cmtName", // cmtName
                    "cmtOpt", // cmtOpt
                    "fontColor", // fontColor
                    1, // commentNewline
                    "masterSbt", // masterSbt
                    1, // inOutKbn
                    10.0, // yakka
                    true, // isGetPriceInYakka
                    20.0, // ten
                    1, // bunkatuKoui
                    2, // alternationIndex
                    1, // kensaGaichu
                    30.0, // odrTermVal
                    40.0, // cnvTermVal
                    "yjCd", // yjCd
                    "centerItemCd1", // centerItemCd1
                    "centerItemCd2", // centerItemCd2
                    1, // kasan1
                    2, // kasan2
                    new(), // yohoSets
                    1, // cmtColKeta1
                    2, // cmtColKeta2
                    3, // cmtColKeta3
                    4, // cmtColKeta4
                    5, // cmtCol1
                    6, // cmtCol2
                    7, // cmtCol3
                    8, // cmtCol4
                    1, // handanGrpKbn
                    true // isKensaMstEmpty
                );
                #endregion
                Assert.True(!model.IsInjection);
            }
        }

        [Test]
        public void SetOrderInfDetailModel_018_IsDrugUsage()
        {
            int yohoKbn = 1;
            string itemCd = "test";
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                0, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                0, // drugKbn
                yohoKbn, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(model.IsDrugUsage);
        }

        [Test]
        public void SetOrderInfDetailModel_019_IsDrugUsage()
        {
            int yohoKbn = 0;
            string itemCd = ItemCdConst.TouyakuChozaiNaiTon;
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                0, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                0, // drugKbn
                yohoKbn, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(model.IsDrugUsage);
        }

        [Test]
        public void SetOrderInfDetailModel_020_IsDrugUsage()
        {
            int yohoKbn = 0;
            string itemCd = ItemCdConst.TouyakuChozaiGai;
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                0, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                0, // drugKbn
                yohoKbn, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(model.IsDrugUsage);
        }

        [Test]
        public void SetOrderInfDetailModel_021_IsDrugUsage()
        {
            int yohoKbn = 0;
            string itemCd = "test";
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                0, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                0, // drugKbn
                yohoKbn, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(!model.IsDrugUsage);
        }

        [Test]
        public void SetOrderInfDetailModel_022_IsStandardUsage()
        {
            int yohoKbn = 1;
            string itemCd = "test";
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                0, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                0, // drugKbn
                yohoKbn, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(model.IsStandardUsage);
        }

        [Test]
        public void SetOrderInfDetailModel_023_IsStandardUsage()
        {
            int yohoKbn = 0;
            string itemCd = ItemCdConst.TouyakuChozaiNaiTon;
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                0, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                0, // drugKbn
                yohoKbn, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(model.IsStandardUsage);
        }

        [Test]
        public void SetOrderInfDetailModel_024_IsStandardUsage()
        {
            int yohoKbn = 0;
            string itemCd = ItemCdConst.TouyakuChozaiGai;
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                0, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                0, // drugKbn
                yohoKbn, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(model.IsStandardUsage);
        }

        [Test]
        public void SetOrderInfDetailModel_025_IsStandardUsage()
        {
            List<int> yohoKbn = new List<int>() { 0, 2 };
            string itemCd = "test";
            foreach (int i in yohoKbn)
            {
                #region Data Sample
                SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                    1, // hpId
                    2, // setCd
                    1000, // rpNo
                    1, // rpEdaNo
                    1, // rowNo
                    0, // sinKouiKbn
                    itemCd, // itemCd
                    "itemName", // itemName
                    "displayItemName", // displayItemName
                    5.0, // suryo
                    "unitName", // unitName
                    2, // unitSBT
                    3.0, // termVal
                    1, // kohatuKbn
                    2, // syohoKbn
                    3, // syohoLimitKbn
                    0, // drugKbn
                    i, // yohoKbn
                    "kokuji1", // kokuji1
                    "kokuji2", // kokuji2
                    1, // isNodspRece
                    "ipnCd", // ipnCd
                    "ipnName", // ipnName
                    "bunkatu", // bunkatu
                    "cmtName", // cmtName
                    "cmtOpt", // cmtOpt
                    "fontColor", // fontColor
                    1, // commentNewline
                    "masterSbt", // masterSbt
                    1, // inOutKbn
                    10.0, // yakka
                    true, // isGetPriceInYakka
                    20.0, // ten
                    1, // bunkatuKoui
                    2, // alternationIndex
                    1, // kensaGaichu
                    30.0, // odrTermVal
                    40.0, // cnvTermVal
                    "yjCd", // yjCd
                    "centerItemCd1", // centerItemCd1
                    "centerItemCd2", // centerItemCd2
                    1, // kasan1
                    2, // kasan2
                    new(), // yohoSets
                    1, // cmtColKeta1
                    2, // cmtColKeta2
                    3, // cmtColKeta3
                    4, // cmtColKeta4
                    5, // cmtCol1
                    6, // cmtCol2
                    7, // cmtCol3
                    8, // cmtCol4
                    1, // handanGrpKbn
                    true // isKensaMstEmpty
                );
                #endregion
                Assert.True(!model.IsStandardUsage);
            }
        }

        [Test]
        public void SetOrderInfDetailModel_026_IsSuppUsage()
        {
            int yohoKbn = 2;
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                0, // sinKouiKbn
                "test", // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                0, // drugKbn
                yohoKbn, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                "masterSbt", // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(model.IsSuppUsage);
        }

        [Test]
        public void SetOrderInfDetailModel_027_IsSuppUsage()
        {
            List<int> yohoKbn = new List<int>() { 1, 3 };
            foreach (int i in yohoKbn)
            {
                #region Data Sample
                SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                    1, // hpId
                    2, // setCd
                    1000, // rpNo
                    1, // rpEdaNo
                    1, // rowNo
                    0, // sinKouiKbn
                    "test", // itemCd
                    "itemName", // itemName
                    "displayItemName", // displayItemName
                    5.0, // suryo
                    "unitName", // unitName
                    2, // unitSBT
                    3.0, // termVal
                    1, // kohatuKbn
                    2, // syohoKbn
                    3, // syohoLimitKbn
                    0, // drugKbn
                    i, // yohoKbn
                    "kokuji1", // kokuji1
                    "kokuji2", // kokuji2
                    1, // isNodspRece
                    "ipnCd", // ipnCd
                    "ipnName", // ipnName
                    "bunkatu", // bunkatu
                    "cmtName", // cmtName
                    "cmtOpt", // cmtOpt
                    "fontColor", // fontColor
                    1, // commentNewline
                    "masterSbt", // masterSbt
                    1, // inOutKbn
                    10.0, // yakka
                    true, // isGetPriceInYakka
                    20.0, // ten
                    1, // bunkatuKoui
                    2, // alternationIndex
                    1, // kensaGaichu
                    30.0, // odrTermVal
                    40.0, // cnvTermVal
                    "yjCd", // yjCd
                    "centerItemCd1", // centerItemCd1
                    "centerItemCd2", // centerItemCd2
                    1, // kasan1
                    2, // kasan2
                    new(), // yohoSets
                    1, // cmtColKeta1
                    2, // cmtColKeta2
                    3, // cmtColKeta3
                    4, // cmtColKeta4
                    5, // cmtCol1
                    6, // cmtCol2
                    7, // cmtCol3
                    8, // cmtCol4
                    1, // handanGrpKbn
                    true // isKensaMstEmpty
                );
                #endregion
                Assert.True(!model.IsSuppUsage);
            }
        }

        [Test]
        public void SetOrderInfDetailModel_028_IsInjectionUsage()
        {
            List<int> sinKouiKbn = new List<int>() { 31, 32, 33, 34 };
            string masterSbt = string.Empty;
            string itemCd = ItemCdConst.TouyakuChozaiGai;
            foreach (int i in sinKouiKbn)
            {
                #region Data Sample
                SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                    1, // hpId
                    2, // setCd
                    1000, // rpNo
                    1, // rpEdaNo
                    1, // rowNo
                    i, // sinKouiKbn
                    itemCd, // itemCd
                    "itemName", // itemName
                    "displayItemName", // displayItemName
                    5.0, // suryo
                    "unitName", // unitName
                    2, // unitSBT
                    3.0, // termVal
                    1, // kohatuKbn
                    2, // syohoKbn
                    3, // syohoLimitKbn
                    0, // drugKbn
                    0, // yohoKbn
                    "kokuji1", // kokuji1
                    "kokuji2", // kokuji2
                    1, // isNodspRece
                    "ipnCd", // ipnCd
                    "ipnName", // ipnName
                    "bunkatu", // bunkatu
                    "cmtName", // cmtName
                    "cmtOpt", // cmtOpt
                    "fontColor", // fontColor
                    1, // commentNewline
                    masterSbt, // masterSbt
                    1, // inOutKbn
                    10.0, // yakka
                    true, // isGetPriceInYakka
                    20.0, // ten
                    1, // bunkatuKoui
                    2, // alternationIndex
                    1, // kensaGaichu
                    30.0, // odrTermVal
                    40.0, // cnvTermVal
                    "yjCd", // yjCd
                    "centerItemCd1", // centerItemCd1
                    "centerItemCd2", // centerItemCd2
                    1, // kasan1
                    2, // kasan2
                    new(), // yohoSets
                    1, // cmtColKeta1
                    2, // cmtColKeta2
                    3, // cmtColKeta3
                    4, // cmtColKeta4
                    5, // cmtCol1
                    6, // cmtCol2
                    7, // cmtCol3
                    8, // cmtCol4
                    1, // handanGrpKbn
                    true // isKensaMstEmpty
                );
                #endregion
                Assert.True(model.IsInjectionUsage);
            }
        }

        [Test]
        public void SetOrderInfDetailModel_029_IsInjectionUsage()
        {
            int sinKouiKbn = 30;
            string masterSbt = "S";
            string itemCd = "Zalo";
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                sinKouiKbn, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                0, // drugKbn
                0, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                masterSbt, // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(model.IsInjectionUsage);
        }

        [Test]
        public void SetOrderInfDetailModel_030_IsInjectionUsage()
        {
            int sinKouiKbn = 30;
            string masterSbt = "S";
            string itemCd = "test";
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                sinKouiKbn, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                0, // drugKbn
                0, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                masterSbt, // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(!model.IsInjectionUsage);
        }

        [Test]
        public void SetOrderInfDetailModel_031_IsInjectionUsage()
        {
            int sinKouiKbn = 30;
            string masterSbt = "A";
            string itemCd = "Zalo";
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                sinKouiKbn, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                0, // drugKbn
                0, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                masterSbt, // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(!model.IsInjectionUsage);
        }

        [Test]
        public void SetOrderInfDetailModel_032_IsInjectionUsage()
        {
            int sinKouiKbn = 29;
            string masterSbt = "S";
            string itemCd = "Zalo";
            #region Data Sample
            SetOrderInfDetailModel model = new SetOrderInfDetailModel(
                1, // hpId
                2, // setCd
                1000, // rpNo
                1, // rpEdaNo
                1, // rowNo
                sinKouiKbn, // sinKouiKbn
                itemCd, // itemCd
                "itemName", // itemName
                "displayItemName", // displayItemName
                5.0, // suryo
                "unitName", // unitName
                2, // unitSBT
                3.0, // termVal
                1, // kohatuKbn
                2, // syohoKbn
                3, // syohoLimitKbn
                0, // drugKbn
                0, // yohoKbn
                "kokuji1", // kokuji1
                "kokuji2", // kokuji2
                1, // isNodspRece
                "ipnCd", // ipnCd
                "ipnName", // ipnName
                "bunkatu", // bunkatu
                "cmtName", // cmtName
                "cmtOpt", // cmtOpt
                "fontColor", // fontColor
                1, // commentNewline
                masterSbt, // masterSbt
                1, // inOutKbn
                10.0, // yakka
                true, // isGetPriceInYakka
                20.0, // ten
                1, // bunkatuKoui
                2, // alternationIndex
                1, // kensaGaichu
                30.0, // odrTermVal
                40.0, // cnvTermVal
                "yjCd", // yjCd
                "centerItemCd1", // centerItemCd1
                "centerItemCd2", // centerItemCd2
                1, // kasan1
                2, // kasan2
                new(), // yohoSets
                1, // cmtColKeta1
                2, // cmtColKeta2
                3, // cmtColKeta3
                4, // cmtColKeta4
                5, // cmtCol1
                6, // cmtCol2
                7, // cmtCol3
                8, // cmtCol4
                1, // handanGrpKbn
                true // isKensaMstEmpty
            );
            #endregion
            Assert.True(!model.IsInjectionUsage);
        }
    }
}
