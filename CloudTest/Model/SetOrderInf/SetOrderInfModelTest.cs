using Domain.Models.SuperSetDetail;

namespace CloudUnitTest.Model.SetOrderInf
{
    public class SetOrderInfModelTest
    {
        [Test]
        public void SetOrderInfModel_001_IsDrug()
        {
            List<int> odrKouiKbn = new List<int>() { 21, 22, 23 };
            foreach (int i in odrKouiKbn)
            {
                var setOrdInf = new SetOrderInfModel(i);
                Assert.True(setOrdInf.IsDrug);
            }
        }

        [Test]
        public void SetOrderInfModel_002_IsDrug()
        {
            List<int> odrKouiKbn = new List<int>() { 20, 24 };
            foreach (int i in odrKouiKbn)
            {
                var setOrdInf = new SetOrderInfModel(i);
                Assert.True(!setOrdInf.IsDrug);
            }
        }

        [Test]
        public void SetOrderInfModel_003_IsInjection()
        {
            List<int> odrKouiKbn = new List<int>() { 30, 31, 32, 33, 34 };
            foreach (int i in odrKouiKbn)
            {
                var setOrdInf = new SetOrderInfModel(i);
                Assert.True(setOrdInf.IsInjection);
            }
        }

        [Test]
        public void SetOrderInfModel_004_IsInjection()
        {
            List<int> odrKouiKbn = new List<int>() { 29, 35 };
            foreach (int i in odrKouiKbn)
            {
                var setOrdInf = new SetOrderInfModel(i);
                Assert.True(!setOrdInf.IsInjection);
            }
        }

        [Test]
        public void SetOrderInfModel_005_IsKensa()
        {
            List<int> odrKouiKbn = new List<int>() { 60, 61, 62, 63, 64 };
            foreach (int i in odrKouiKbn)
            {
                var setOrdInf = new SetOrderInfModel(i);
                Assert.True(setOrdInf.IsKensa);
            }
        }

        [Test]
        public void SetOrderInfModel_006_IsKensa()
        {
            List<int> odrKouiKbn = new List<int>() { 59, 65 };
            foreach (int i in odrKouiKbn)
            {
                var setOrdInf = new SetOrderInfModel(i);
                Assert.True(!setOrdInf.IsKensa);
            }
        }

        [Test]
        public void SetOrderInfModel_007_IsSelfInjection()
        {
            int odrKouiKbn = 28;
            var setOrdInf = new SetOrderInfModel(odrKouiKbn);
            Assert.True(setOrdInf.IsSelfInjection);
        }

        [Test]
        public void SetOrderInfModel_008_IsSelfInjection()
        {
            List<int> odrKouiKbn = new List<int>() { 27, 29 };
            foreach (int i in odrKouiKbn)
            {
                var setOrdInf = new SetOrderInfModel(i);
                Assert.True(!setOrdInf.IsSelfInjection);
            }
        }
    }
}
