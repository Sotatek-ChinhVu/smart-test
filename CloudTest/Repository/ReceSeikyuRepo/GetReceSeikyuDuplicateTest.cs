using Entity.Tenant;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.ReceSeikyuRepo
{
    public class GetReceSeikyuDuplicateTest : BaseUT
    {
        [Test]
        public void TC_001_GetReceSeikyuDuplicateTest_Any()
        {
            //Arrange
            var receSeikyuRepository = new ReceSeikyuRepository(TenantProvider);
            var tennal = TenantProvider.GetNoTrackingDataContext();

            int hpId = 99999999;
            int sinYm = 202404;
            int seqNo = 0;
            long ptId = 28032001;
            int hokenId = 0;

            ReceSeikyu receSeikyu = new ReceSeikyu()
            {
                HpId = hpId,
                SinYm = sinYm,
                SeqNo = seqNo,
                PtId = ptId,
                HokenId = hokenId
            };

            tennal.Add(receSeikyu);

            try
            {
                // Act
                tennal.SaveChanges();
                var result = receSeikyuRepository.GetReceSeikyuDuplicate(hpId, ptId, sinYm, hokenId);

                // Assert
                Assert.That(result.HpId == hpId && result.SinYm == sinYm);
            }
            finally
            {
                receSeikyuRepository.ReleaseResource();
                tennal.Remove(receSeikyu);
                tennal.SaveChanges();
            }
        }

        [Test]
        public void TC_002_GetReceSeikyuDuplicateTest_NotAny()
        {
            //Arrange
            var receSeikyuRepository = new ReceSeikyuRepository(TenantProvider);

            int hpId = 99999999;
            int sinYm = 202404;
            long ptId = 28032001;
            int hokenId = 0;

            try
            {
                // Act
                var result = receSeikyuRepository.GetReceSeikyuDuplicate(hpId, ptId, sinYm, hokenId);

                // Assert
                Assert.False(result.HpId == hpId && result.SinYm == sinYm);
            }
            finally
            {
                receSeikyuRepository.ReleaseResource();
            }
        }
    }
}
