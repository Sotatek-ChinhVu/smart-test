using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;

namespace CloudUnitTest.Model.OrdInf
{
    public class OrdInfModelTest : BaseUT
    {
        [Test]
        public void OrdInfModel_001_IsDrug()
        {
            int inoutKbn = 0;
            List<int> odrKouiKbn = new List<int>() { 21, 22, 23 };
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>();
            foreach (int i in odrKouiKbn)
            {
                var ordInf = new OrdInfModel(inoutKbn, i, ordInfDetailModels);
                Assert.True(ordInf.IsDrug);
            }
        }

        [Test]
        public void OrdInfModel_002_IsDrug()
        {
            int inoutKbn = 0;
            List<int> odrKouiKbn = new List<int>() { 20, 24 };
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>();
            foreach (int i in odrKouiKbn)
            {
                var ordInf = new OrdInfModel(inoutKbn, i, ordInfDetailModels);
                Assert.True(!ordInf.IsDrug);
            }
        }

        [Test]
        public void OrdInfModel_003_IsInjection()
        {
            int inoutKbn = 0;
            List<int> odrKouiKbn = new List<int>() { 30, 31, 32, 33, 34 };
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>();
            foreach (int i in odrKouiKbn)
            {
                var ordInf = new OrdInfModel(inoutKbn, i, ordInfDetailModels);
                Assert.True(ordInf.IsInjection);
            }
        }

        [Test]
        public void OrdInfModel_004_IsInjection()
        {
            int inoutKbn = 0;
            List<int> odrKouiKbn = new List<int>() { 29, 35 };
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>();
            foreach (int i in odrKouiKbn)
            {
                var ordInf = new OrdInfModel(inoutKbn, i, ordInfDetailModels);
                Assert.True(!ordInf.IsInjection);
            }
        }
    }
}
