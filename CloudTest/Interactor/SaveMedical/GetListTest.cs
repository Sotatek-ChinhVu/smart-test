using Infrastructure.Repositories;

namespace CloudUnitTest.Interactor.SaveMedical
{
    public class GetListTest : BaseUT
    {
        [Test]
        public void TC_001_SaveMedicalInteractor_TestGetList_PtId_InvalidId()
        {
            //Arrange
            var receptionRepository = new ReceptionRepository(TenantProvider);
            //Mock data
            int hpId = 1;
            long ptId = -1;
            long raiinNo = 4981;
            int sinDate = 20100609;

            var result = receptionRepository.GetList(hpId, sinDate, raiinNo, ptId);

            Assert.IsNotNull(result);
        }

        [Test]
        public void TC_002_SaveMedicalInteractor_TestGetList_PtId()
        {
            //Arrange
            var receptionRepository = new ReceptionRepository(TenantProvider);
            //Mock data
            int hpId = 1;
            long ptId = 10065;
            long raiinNo = 4981;
            int sinDate = 20100609;

            var result = receptionRepository.GetList(hpId, sinDate, raiinNo, ptId);

            Assert.IsNotNull(result);
        }

        [Test]
        public void TC_003_SaveMedicalInteractor_TestGetList_SearchSameVisit_true()
        {
            //Arrange
            var receptionRepository = new ReceptionRepository(TenantProvider);
            //Mock data
            int hpId = 1;
            long ptId = 10065;
            long raiinNo = 4981;
            int sinDate = 20100609;
            bool searchSameVisit = true;

            var result = receptionRepository.GetList(hpId, sinDate, raiinNo, ptId, false, false, 2, searchSameVisit);

            Assert.IsNotNull(result);
        }

        [Test]
        public void TC_004_SaveMedicalInteractor_TestGetList_IsGetFamily_true()
        {
            //Arrange
            var receptionRepository = new ReceptionRepository(TenantProvider);
            //Mock data
            int hpId = 1;
            long ptId = 10065;
            long raiinNo = 4981;
            int sinDate = 20100609;
            bool isGetFamily = true;

            var result = receptionRepository.GetList(hpId, sinDate, raiinNo, ptId, false, isGetFamily, 2, false);

            Assert.IsNotNull(result);
        }

        [Test]
        public void TC_005_SaveMedicalInteractor_TestGetList_IsGetFamily_true()
        {
            //Arrange
            var receptionRepository = new ReceptionRepository(TenantProvider);
            //Mock data
            int hpId = 1;
            long ptId = 123;
            long raiinNo = -1;
            int sinDate = 20100609;
            bool isGetFamily = true;

            var result = receptionRepository.GetList(hpId, sinDate, raiinNo, ptId, false, isGetFamily, 2, false);

            Assert.IsNotNull(result);
        }
    }
}
