using Domain.Models.NextOrder;

namespace CloudUnitTest.Model.NextOrder
{
    public class RsvkrtOrderInfModelTest
    {
        [Test]
        public void RsvkrtOrderInfModel_001_IsDrug()
        {
            List<int> odrKouiKbn = new List<int>() { 21, 22, 23 };
            foreach (int i in odrKouiKbn)
            {
                var model = new RsvkrtOrderInfModel(i);
                Assert.True(model.IsDrug);
            }
        }

        [Test]
        public void RsvkrtOrderInfModel_002_IsDrug()
        {
            List<int> odrKouiKbn = new List<int>() { 20, 24 };
            foreach (int i in odrKouiKbn)
            {
                var model = new RsvkrtOrderInfModel(i);
                Assert.True(!model.IsDrug);
            }
        }

        [Test]
        public void RsvkrtOrderInfModel_003_IsInjection()
        {
            List<int> odrKouiKbn = new List<int>() { 30, 31, 32, 33, 34 };
            foreach (int i in odrKouiKbn)
            {
                var model = new RsvkrtOrderInfModel(i);
                Assert.True(model.IsInjection);
            }
        }

        [Test]
        public void RsvkrtOrderInfModel_004_IsInjection()
        {
            List<int> odrKouiKbn = new List<int>() { 29, 35 };
            foreach (int i in odrKouiKbn)
            {
                var model = new RsvkrtOrderInfModel(i);
                Assert.True(!model.IsInjection);
            }
        }
    }
}
