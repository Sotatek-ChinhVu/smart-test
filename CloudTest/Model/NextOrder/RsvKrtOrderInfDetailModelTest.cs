using Domain.Models.NextOrder;
using Helper.Constants;

namespace CloudUnitTest.Model.NextOrder
{
    public class RsvKrtOrderInfDetailModelTest
    {
        [Test]
        public void RsvKrtOrderInfDetailModel_001_IsSuppUsage()
        {
            int yohoKbn = 2;
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, "test", 0, "test", 0);
            Assert.True(model.IsSuppUsage);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_002_IsSuppUsage()
        {
            List<int> yohoKbn = new List<int>() { 1, 3 };
            foreach (int i in yohoKbn)
            {
                var model = new RsvKrtOrderInfDetailModel(i, "test", 0, "test", 0);
                Assert.True(!model.IsSuppUsage);
            }
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_003_IsStandardUsage()
        {
            int yohoKbn = 1;
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, "test", 0, "test", 0);
            Assert.True(model.IsStandardUsage);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_004_IsStandardUsage()
        {
            int yohoKbn = 0;
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, ItemCdConst.TouyakuChozaiNaiTon, 0, "test", 0);
            Assert.True(model.IsStandardUsage);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_005_IsStandardUsage()
        {
            int yohoKbn = 0;
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, ItemCdConst.TouyakuChozaiGai, 0, "test", 0);
            Assert.True(model.IsStandardUsage);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_006_IsStandardUsage()
        {
            int yohoKbn = 0;
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, "test", 0, "test", 0);
            Assert.True(!model.IsStandardUsage);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_007_IsInjectionUsage()
        {
            List<int> sinKouiKbn = new List<int>() { 31, 32, 33, 34 };
            int yohoKbn = 0;
            string masterSbt = string.Empty;
            string itemCd = ItemCdConst.TouyakuChozaiGai;
            foreach (int i in sinKouiKbn)
            {
                var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, i, masterSbt, 0);
                Assert.True(model.IsInjectionUsage);
            }
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_008_IsInjectionUsage()
        {
            int sinKouiKbn = 30;
            int yohoKbn = 0;
            string masterSbt = "S";
            string itemCd = "Zalo";
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, 0);
            Assert.True(model.IsInjectionUsage);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_009_IsInjectionUsage()
        {
            int sinKouiKbn = 30;
            int yohoKbn = 0;
            string masterSbt = "S";
            string itemCd = "test";
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, 0);
            Assert.True(!model.IsInjectionUsage);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_010_IsInjectionUsage()
        {
            int sinKouiKbn = 30;
            int yohoKbn = 0;
            string masterSbt = "A";
            string itemCd = "Zalo";
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, 0);
            Assert.True(!model.IsInjectionUsage);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_011_IsInjectionUsage()
        {
            int sinKouiKbn = 29;
            int yohoKbn = 0;
            string masterSbt = "S";
            string itemCd = "Zalo";
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, 0);
            Assert.True(!model.IsInjectionUsage);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_012_IsInDrugOdr()
        {
            List<int> sinKouiKbn = new List<int>() { 20, 21, 22, 23, 28 };
            int yohoKbn = 0;
            string masterSbt = "test";
            string itemCd = "test";
            foreach (int i in sinKouiKbn)
            {
                var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, i, masterSbt, 0);
                Assert.True(model.IsInDrugOdr);
            }
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_013_IsInDrugOdr()
        {
            List<int> sinKouiKbn = new List<int>() { 19, 24 };
            int yohoKbn = 0;
            string masterSbt = "test";
            string itemCd = "test";
            foreach (int i in sinKouiKbn)
            {
                var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, i, masterSbt, 0);
                Assert.True(!model.IsInDrugOdr);
            }
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_014_IsSpecialItem()
        {
            int sinKouiKbn = 20;
            int drugKbn = 0;
            int yohoKbn = 0;
            string masterSbt = "S";
            string itemCd = "test";
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(model.IsSpecialItem);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_015_IsSpecialItem()
        {
            int sinKouiKbn = 20;
            int drugKbn = 1;
            int yohoKbn = 0;
            string masterSbt = "S";
            string itemCd = "test";
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(!model.IsSpecialItem);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_016_IsSpecialItem()
        {
            int sinKouiKbn = 20;
            int drugKbn = 0;
            int yohoKbn = 0;
            string masterSbt = "A";
            string itemCd = "test";
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(!model.IsSpecialItem);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_017_IsSpecialItem()
        {
            int sinKouiKbn = 21;
            int drugKbn = 0;
            int yohoKbn = 0;
            string masterSbt = "S";
            string itemCd = "test";
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(!model.IsSpecialItem);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_018_IsSpecialItem()
        {
            int sinKouiKbn = 20;
            int drugKbn = 0;
            int yohoKbn = 0;
            string masterSbt = "S";
            string itemCd = ItemCdConst.Con_TouyakuOrSiBunkatu;
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(!model.IsSpecialItem);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_019_IsSpecialItem()
        {
            int sinKouiKbn = 20;
            int drugKbn = 0;
            int yohoKbn = 0;
            string masterSbt = "S";
            string itemCd = ItemCdConst.Con_Refill;
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(!model.IsSpecialItem);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_020_IsDrugUsage()
        {
            int sinKouiKbn = 0;
            int drugKbn = 0;
            int yohoKbn = 1;
            string masterSbt = "test";
            string itemCd = "test";
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(model.IsDrugUsage);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_021_IsDrugUsage()
        {
            int sinKouiKbn = 0;
            int drugKbn = 0;
            int yohoKbn = 0;
            string masterSbt = "test";
            string itemCd = ItemCdConst.TouyakuChozaiNaiTon;
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(model.IsDrugUsage);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_022_IsDrugUsage()
        {
            int sinKouiKbn = 0;
            int drugKbn = 0;
            int yohoKbn = 0;
            string masterSbt = "test";
            string itemCd = ItemCdConst.TouyakuChozaiGai;
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(model.IsDrugUsage);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_023_IsDrugUsage()
        {
            int sinKouiKbn = 0;
            int drugKbn = 0;
            int yohoKbn = 0;
            string masterSbt = "test";
            string itemCd = "test";
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(!model.IsDrugUsage);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_024_IsDrug()
        {
            int sinKouiKbn = 20;
            int drugKbn = 1;
            int yohoKbn = 0;
            string masterSbt = "test";
            string itemCd = "test";
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(model.IsDrug);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_025_IsDrug()
        {
            int sinKouiKbn = 21;
            int drugKbn = 1;
            int yohoKbn = 0;
            string masterSbt = "test";
            string itemCd = "test";
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(!model.IsDrug);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_026_IsDrug()
        {
            int sinKouiKbn = 20;
            int drugKbn = 0;
            int yohoKbn = 0;
            string masterSbt = "test";
            string itemCd = ItemCdConst.TouyakuChozaiNaiTon;
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(model.IsDrug);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_027_IsDrug()
        {
            int sinKouiKbn = 20;
            int drugKbn = 0;
            int yohoKbn = 0;
            string masterSbt = "test";
            string itemCd = ItemCdConst.TouyakuChozaiGai;
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(model.IsDrug);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_028_IsDrug()
        {
            int sinKouiKbn = 20;
            int drugKbn = 0;
            int yohoKbn = 0;
            string masterSbt = "test";
            string itemCd = "Zalo";
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(model.IsDrug);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_029_IsDrug()
        {
            int sinKouiKbn = 19;
            int drugKbn = 0;
            int yohoKbn = 0;
            string masterSbt = "test";
            string itemCd = "Zalo";
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(!model.IsDrug);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_030_IsInjection()
        {
            int sinKouiKbn = 30;
            int drugKbn = 0;
            int yohoKbn = 0;
            string masterSbt = "A";
            string itemCd = "test";
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(model.IsInjection);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_031_IsInjection()
        {
            int sinKouiKbn = 30;
            int drugKbn = 0;
            int yohoKbn = 0;
            string masterSbt = "S";
            string itemCd = "test";
            var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, sinKouiKbn, masterSbt, drugKbn);
            Assert.True(!model.IsInjection);
        }

        [Test]
        public void RsvKrtOrderInfDetailModel_032_IsInjection()
        {
            List<int> sinKouiKbn = new List<int>() { 29, 31 };
            int drugKbn = 0;
            int yohoKbn = 0;
            string masterSbt = "A";
            string itemCd = "test";
            foreach (int i in sinKouiKbn)
            {
                var model = new RsvKrtOrderInfDetailModel(yohoKbn, itemCd, i, masterSbt, drugKbn);
                Assert.True(!model.IsInjection);
            }
        }
    }
}
