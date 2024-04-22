using Domain.Models.ReceSeikyu;
using Entity.Tenant;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.ReceSeikyuRepo
{
    public class SaveReceSeiKyuTest : BaseUT
    {
        [Test]
        public void TC_001_SaveReceSeiKyuTest_AddNew()
        {
            //Arrange
            var receSeikyuRepository = new ReceSeikyuRepository(TenantProvider);
            var tenant = TenantProvider.GetNoTrackingDataContext();

            int hpId = 0;
            int sinYm = 202404;
            int seqNo = 0;
            long ptId = 28032001;
            int hokenId = 0;
            int userId = 999;
            long ptNum = 999;
            int seikyuKbn = 0;
            string cmt = "返戻ファイルより登録";
            int preHokenId = 5;

            List<ReceSeikyuModel> data = new List<ReceSeikyuModel>()
            {
                new ReceSeikyuModel(ptId, sinYm, hokenId, ptNum, seikyuKbn).UpdateReceSeikyuModel(seqNo, 0, preHokenId, cmt)
            };

            List<ReceSeikyu> receSeikyus = new List<ReceSeikyu>();

            try
            {
                // Act
                var result = receSeikyuRepository.SaveReceSeiKyu(hpId, userId, data);

                receSeikyus = tenant.ReceSeikyus.Where(x => x.HpId == hpId && x.SinYm == sinYm && x.PreHokenId == preHokenId && x.Cmt == cmt).ToList();

                // Assert
                Assert.That(result && receSeikyus.Any());
            }
            finally
            {
                receSeikyuRepository.ReleaseResource();
                tenant.RemoveRange(receSeikyus);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_002_SaveReceSeiKyuTest_UpDate()
        {
            //Arrange
            var receSeikyuRepository = new ReceSeikyuRepository(TenantProvider);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            var tenanTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 1;
            int sinYm = 202404;
            int seqNo = 0;
            long ptId = 28032001;
            int hokenId = 0;
            int userId = 999;
            long ptNum = 999;
            int seikyuKbn = 0;
            string cmt = "返戻ファイルより登録";
            int preHokenId = 5;
            int sinDate = 0, receListSinYm = 0, seikyuYm = 0, hokenKbn = 0, hokenStartDate = 0, hokenEndDate = 0, originSeikyuYm = 0, originSinYm = 202404, isDeleted = 0;
            string ptName = "", hokensyaNo = "", houbetu = "";
            bool isModified = false, isAddNew = false, isChecked = false;
            List<RecedenHenJiyuuModel> listRecedenHenJiyuuModel = new();

            ReceSeikyu receSeikyu = new ReceSeikyu()
            {
                HpId = hpId,
                SinYm = sinYm,
                SeqNo = seqNo,
                PtId = ptId,
                HokenId = hokenId
            };

            tenant.Add(receSeikyu);

            List<ReceSeikyu> receSeikyus = new List<ReceSeikyu>();

            try
            {
                // Act
                tenant.SaveChanges();
                seqNo = tenant.ReceSeikyus.FirstOrDefault(x => x.HpId == hpId && x.SinYm == sinYm)!.SeqNo;

                List<ReceSeikyuModel> data = new List<ReceSeikyuModel>()
                {
                    new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn,
                                        houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm, originSinYm, isAddNew, isDeleted, isChecked, listRecedenHenJiyuuModel).UpdateReceSeikyuModel(seqNo, 0, preHokenId, cmt)
                };

                var result = receSeikyuRepository.SaveReceSeiKyu(hpId, userId, data);

                receSeikyus = tenant.ReceSeikyus.Where(x => x.HpId == hpId && x.SinYm == sinYm && x.SeqNo == seqNo && x.PreHokenId == preHokenId && x.Cmt == cmt).ToList();

                // Assert
                Assert.That(result && receSeikyus.Any());
            }
            finally
            {
                receSeikyuRepository.ReleaseResource();
                tenanTracking.RemoveRange(receSeikyus);
                tenanTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_003_SaveReceSeiKyuTest_Deleted()
        {
            //Arrange
            var receSeikyuRepository = new ReceSeikyuRepository(TenantProvider);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            var tenanTracking = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 1;
            int sinYm = 202404;
            int seqNo = 0;
            long ptId = 28032001;
            int hokenId = 0;
            int userId = 999;
            long ptNum = 999;
            int seikyuKbn = 0;
            string cmt = "返戻ファイルより登録";
            int preHokenId = 5;
            int sinDate = 0, receListSinYm = 0, seikyuYm = 0, hokenKbn = 0, hokenStartDate = 0, hokenEndDate = 0, originSeikyuYm = 0, originSinYm = 202404, isDeleted = 0;
            string ptName = "", hokensyaNo = "", houbetu = "";
            bool isModified = false, isAddNew = false, isChecked = false;
            List<RecedenHenJiyuuModel> listRecedenHenJiyuuModel = new();

            ReceSeikyu receSeikyu = new ReceSeikyu()
            {
                HpId = hpId,
                SinYm = sinYm,
                SeqNo = seqNo,
                PtId = ptId,
                HokenId = hokenId
            };

            tenant.Add(receSeikyu);

            List<ReceSeikyu> receSeikyus = new List<ReceSeikyu>();

            try
            {
                // Act
                tenant.SaveChanges();
                seqNo = tenant.ReceSeikyus.FirstOrDefault(x => x.HpId == hpId && x.SinYm == sinYm)!.SeqNo;

                List<ReceSeikyuModel> data = new List<ReceSeikyuModel>()
                {
                    new ReceSeikyuModel(sinDate, hpId, ptId, ptName, sinYm, receListSinYm, hokenId, hokensyaNo, seqNo, seikyuYm, seikyuKbn, preHokenId, cmt, ptNum, hokenKbn,
                                        houbetu, hokenStartDate, hokenEndDate, isModified, originSeikyuYm, originSinYm, isAddNew, isDeleted, isChecked, listRecedenHenJiyuuModel).UpdateReceSeikyuModel(seqNo, 1, preHokenId, cmt)
                };

                var result = receSeikyuRepository.SaveReceSeiKyu(hpId, userId, data);

                receSeikyus = tenant.ReceSeikyus.Where(x => x.HpId == hpId && x.SinYm == sinYm && x.IsDeleted == 1).ToList();

                // Assert
                Assert.That(result && receSeikyus.Any());
            }
            finally
            {
                receSeikyuRepository.ReleaseResource();
                tenanTracking.RemoveRange(receSeikyus);
                tenanTracking.SaveChanges();
            }
        }
    }
}
