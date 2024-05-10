using Entity.SuperAdmin;
using Entity.Tenant;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.ReceSeikyuRepo
{
    public class UpdateSeikyuYmReceipSeikyuIfExistTest : BaseUT
    {
        [Test]
        public void TC_001_UpdateSeikyuYmReceipSeikyuIfExistTest_NotAny()
        {
            //Arrange
            var receSeikyuRepository = new ReceSeikyuRepository(TenantProvider);
            var tenant = TenantProvider.GetNoTrackingDataContext();

            int hpId = 99999999;
            int sinYm = 202404;
            long ptId = 28032001;
            int hokenId = 0;
            int userId = 999;
            int seikyuYm = 5;

            // Act
            var result = receSeikyuRepository.UpdateSeikyuYmReceipSeikyuIfExist(ptId, sinYm, hokenId, seikyuYm, userId, hpId);
            var receSeikyus = tenant.ReceSeikyus.Where(x => x.HpId == hpId && x.SinYm == sinYm).ToList();

            // Assert
            Assert.That(result && !receSeikyus.Any());
        }

        [Test]
        public void TC_002_UpdateSeikyuYmReceipSeikyuIfExistTest_Any()
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
            int seikyuYm = 999999;

            ReceSeikyu receSeikyu = new ReceSeikyu()
            {
                HpId = hpId,
                SinYm = sinYm,
                SeqNo = seqNo,
                PtId = ptId,
                HokenId = hokenId,
                SeikyuYm = seikyuYm
            };

            tenant.Add(receSeikyu);

            try
            {
                // Act
                tenant.SaveChanges();
                var result = receSeikyuRepository.UpdateSeikyuYmReceipSeikyuIfExist(ptId, sinYm, hokenId, seikyuYm + 1, userId, hpId);
                var receSeikyus = tenant.ReceSeikyus.Where(x => x.HpId == hpId && x.SinYm == sinYm && x.SeikyuYm == seikyuYm + 1).ToList();

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
