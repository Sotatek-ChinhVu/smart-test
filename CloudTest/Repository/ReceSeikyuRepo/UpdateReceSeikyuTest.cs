using Domain.Models.ReceSeikyu;
using Entity.Tenant;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.ReceSeikyuRepo
{
    public class UpdateReceSeikyuTest : BaseUT
    {
        [Test]
        public void TC_001_UpdateReceSeikyuTest_Update()
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
            long ptNum = 999;
            int seikyuKbn = 0;
            string cmt = "Comment";
            int preHokenId = 5;

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

                seqNo = tenant.ReceSeikyus.FirstOrDefault(x => x.HpId == hpId && x.SinYm == sinYm)!.SeqNo;

                List<ReceSeikyuModel> receSeikyuList = new List<ReceSeikyuModel>()
                {
                    new ReceSeikyuModel(ptId, sinYm, hokenId, ptNum, seikyuKbn).UpdateReceSeikyuModel(seqNo, 0, preHokenId, cmt)
                };

                var result = receSeikyuRepository.UpdateReceSeikyu(receSeikyuList, userId, hpId);

                var receSeikyus = tenant.ReceSeikyus.Where(x => x.HpId == hpId && x.SinYm == sinYm && x.SeqNo == seqNo).ToList();

                // Assert
                Assert.That(result && receSeikyus.Any(x => x.HpId == hpId && x.SinYm == sinYm && x.SeqNo == seqNo && x.PreHokenId == preHokenId && x.Cmt == cmt));
            }
            finally
            {
                receSeikyuRepository.ReleaseResource();
                tenant.Remove(receSeikyu);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_002_UpdateReceSeikyuTest_Insert()
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
            long ptNum = 999;
            int seikyuKbn = 0;
            string cmt = "Comment";
            int preHokenId = 5;
            List<ReceSeikyu> receSeikyus = new List<ReceSeikyu>();

            try
            {
                // Act
                tenant.SaveChanges();

                List<ReceSeikyuModel> receSeikyuList = new List<ReceSeikyuModel>()
                {
                    new ReceSeikyuModel(ptId, sinYm, hokenId, ptNum, seikyuKbn).UpdateReceSeikyuModel(seqNo, 0, preHokenId, cmt)
                };

                var result = receSeikyuRepository.UpdateReceSeikyu(receSeikyuList, userId, hpId);
                receSeikyus = tenant.ReceSeikyus.Where(x => x.HpId == hpId && x.SinYm == sinYm).ToList();

                // Assert
                Assert.That(result && receSeikyus.Any(x => x.HpId == hpId && x.SinYm == sinYm && x.PreHokenId == preHokenId && x.Cmt == cmt));
            }
            finally
            {
                receSeikyuRepository.ReleaseResource();
                tenant.RemoveRange(receSeikyus);
                tenant.SaveChanges();
            }
        }
    }
}
