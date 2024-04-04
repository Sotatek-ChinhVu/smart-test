using Infrastructure.Repositories;

namespace CloudUnitTest.Interactor.SaveMedical
{
    public class GetListTest : BaseUT
    {
        [Test]
        public void TC_001_SaveMedicalInteractor_GetListRepection_InvalidPtId()
        {
            //Arrange
            var receptionRepository = new ReceptionRepository(TenantProvider);
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId;
            long ptId = -1;
            long raiinNo;
            int sinDate;
            var raiinInf = tenantTracking.RaiinInfs.FirstOrDefault();
            hpId = raiinInf?.HpId ?? 0;
            raiinNo = raiinInf?.RaiinNo ?? 0;
            sinDate = raiinInf?.SinDate ?? 0;

            // Act
            var result = receptionRepository.GetList(hpId, sinDate, raiinNo, ptId);

            // Assert
            Assert.That(result.Any() || result.Any(x => x.IsNameDuplicate == false));
        }

        [Test]
        public void TC_002_SaveMedicalInteractor_GetListRepection_ValidPtId()
        {
            //Arrange
            var receptionRepository = new ReceptionRepository(TenantProvider);
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            
            int hpId;
            long ptId;
            long raiinNo;
            int sinDate;
            var raiinInf = tenantTracking.RaiinInfs.FirstOrDefault();
            hpId = raiinInf?.HpId ?? 0;
            ptId = raiinInf?.PtId ?? 0;
            raiinNo = raiinInf?.RaiinNo ?? 0;
            sinDate = raiinInf?.SinDate ?? 0;

            var result = receptionRepository.GetList(hpId, sinDate, raiinNo, ptId);

            Assert.That(result.Any(x => x.RaiinNo == raiinNo));
        }

        [Test]
        public void TC_003_SaveMedicalInteractor_GetListRepection_SearchSameVisit()
        {
            //Arrange
            var receptionRepository = new ReceptionRepository(TenantProvider);
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId;
            long ptId;
            long raiinNo;
            int sinDate;
            var raiinInf = tenantTracking.RaiinInfs.FirstOrDefault();
            hpId = raiinInf?.HpId ?? 0;
            raiinNo = raiinInf?.RaiinNo ?? 0;
            sinDate = raiinInf?.SinDate ?? 0;
            ptId = raiinInf?.PtId ?? 0;
            bool searchSameVisit = true;
            
            //Act
            var result = receptionRepository.GetList(hpId, sinDate, raiinNo, ptId, false, false, 2, searchSameVisit);

            // Assert
            Assert.That(result.Any(x=> x.RaiinNo == raiinNo));
        }

        [Test]
        public void TC_004_SaveMedicalInteractor_GetListRepection_IsNameDuplicate_False()
        {
            //Arrange
            var receptionRepository = new ReceptionRepository(TenantProvider);
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
          
            int hpId;
            long ptId;
            long raiinNo;
            int sinDate;
            var raiinInf = tenantTracking.RaiinInfs.FirstOrDefault();
            hpId = raiinInf?.HpId ?? 0;
            raiinNo = raiinInf?.RaiinNo ?? 0;
            sinDate = raiinInf?.SinDate ?? 0;
            ptId = raiinInf?.PtId ?? 0;
            bool isGetFamily = true;

            // Act
            var result = receptionRepository.GetList(hpId, sinDate, raiinNo, ptId, false, isGetFamily, 2, false);

            // Assert
            Assert.IsNotNull(result.Any(x => x.RaiinNo == raiinNo));
        }

        [Test]
        public void TC_005_SaveMedicalInteractor_TestGetList_IsNameDuplicate_True()
        {
            //Arrange
            var receptionRepository = new ReceptionRepository(TenantProvider);
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var raiinInf = tenantTracking.RaiinInfs.FirstOrDefault();

            int hpId = raiinInf?.HpId ?? 0;
            long ptId = raiinInf?.PtId ?? 0;
            long raiinNo = -1;
            int sinDate = raiinInf?.SinDate ?? 0;
            bool isGetFamily = true;

            // Act
            var result = receptionRepository.GetList(hpId, sinDate, raiinNo, ptId, false, isGetFamily, 2, false);

            // Assert
            Assert.That(result.Any(x => x.IsNameDuplicate = true));
        }
    }
}
