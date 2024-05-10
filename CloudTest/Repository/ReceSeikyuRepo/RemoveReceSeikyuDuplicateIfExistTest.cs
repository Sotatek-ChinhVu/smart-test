using Entity.SuperAdmin;
using Entity.Tenant;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.ReceSeikyuRepo
{
    public class RemoveReceSeikyuDuplicateIfExistTest : BaseUT
    {
        [Test]
        public void TC_001_RemoveReceSeikyuDuplicateIfExistTes_NotAny()
        {
            //Arrange
            var receSeikyuRepository = new ReceSeikyuRepository(TenantProvider);

            int hpId = 99999999;
            int sinYm = 202404;
            long ptId = 28032001;
            int hokenId = 0;
            int userId = 999;

            // Act
            var result = receSeikyuRepository.RemoveReceSeikyuDuplicateIfExist(ptId, sinYm, hokenId, userId, hpId);

            // Assert
            Assert.That(result);
        }

        [Test]
        public void TC_002_RemoveReceSeikyuDuplicateIfExistTest_Any()
        {
            //Arrange
            var receSeikyuRepository = new ReceSeikyuRepository(TenantProvider);
            var tenant = TenantProvider.GetNoTrackingDataContext();

            int hpId = 99999999;
            int sinYm = 202404;
            int seqNo = 0;
            long ptId = 28032001;
            int hokenId = 0;
            int userId = 999;

            ReceSeikyu receSeikyu = new ReceSeikyu()
            {
                HpId = hpId,
                SinYm = sinYm,
                SeqNo = seqNo,
                PtId = ptId,
                HokenId = hokenId
            };

            tenant.Add(receSeikyu);

            try
            {
                // Act
                tenant.SaveChanges();
                var result = receSeikyuRepository.RemoveReceSeikyuDuplicateIfExist(ptId, sinYm, hokenId, userId, hpId);
                var receSeikyus = tenant.ReceSeikyus.Where(x => x.HpId == hpId && x.SinYm == sinYm && x.IsDeleted == 1).ToList();

                // Assert
                Assert.That(result && receSeikyus.Any());
            }
            finally
            {
                receSeikyuRepository.ReleaseResource();
                tenant.Remove(receSeikyu);
                tenant.SaveChanges();
            }
        }
    }
}
