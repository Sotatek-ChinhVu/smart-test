using Entity.Tenant;
using Infrastructure.Repositories;
using System;

namespace CloudUnitTest.Repository.AccountDueRepo
{
    public class AccountDueRepositoryTest : BaseUT
    {
        [Test]
        public void TC_001_AccountDueRepository_GetUketsukeSbt()
        {
            //Arrange
            AccountDueRepository accountDueRepository = new AccountDueRepository(TenantProvider);
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            Random random = new Random();
            int hpId = random.Next(999, 99999);
            int kbnId = random.Next(999, 99999);

            UketukeSbtMst uketukeSbtMst = new UketukeSbtMst()
            {
                HpId = hpId,
                KbnId = kbnId,
                IsDeleted = 0
            };

            tenantTracking.Add(uketukeSbtMst);

            try
            {
                // Act
                tenantTracking.SaveChanges();
                var result = accountDueRepository.GetUketsukeSbt(hpId);

                // Assert
                Assert.That(result.Count() > 0 && result.Any(x => x.Key == kbnId));
            }
            finally
            {
                accountDueRepository.ReleaseResource();
                tenantTracking.Remove(uketukeSbtMst);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_002_AccountDueRepository_GetAccountDueList()
        {
            //Arrange
            AccountDueRepository accountDueRepository = new AccountDueRepository(TenantProvider);
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            Random random = new Random();

            int hpId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(999, 99999);
            long raiinNo = random.Next(999, 99999);
            long seqNo = random.Next(999, 99999);
            int status = random.Next(999, 99999);
            long id = random.Next(999, 99999);
            bool isUnpaidChecked = true;

            SyunoSeikyu syunoSeikyu = new SyunoSeikyu()
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = sinDate,
                RaiinNo = raiinNo
            };

            SyunoNyukin syunoNyukin = new SyunoNyukin()
            {
                HpId = hpId,
                PtId = ptId,
                RaiinNo = raiinNo,
                SeqNo = seqNo
            };

            RaiinInf raiinInf = new RaiinInf()
            {
                HpId = hpId,
                RaiinNo = raiinNo,
                PtId = ptId,
                Status = status
            };

            KaMst kaMst = new KaMst()
            {
                HpId = hpId,
                Id = id
            };

            tenantTracking.Add(syunoSeikyu);
            tenantTracking.Add(syunoNyukin);
            tenantTracking.Add(raiinInf);
            tenantTracking.Add(kaMst);

            try
            {
                // Act
                tenantTracking.SaveChanges();
                var result = accountDueRepository.GetAccountDueList(hpId, ptId, sinDate, isUnpaidChecked);

                // Assert
                Assert.That(result.Count() > 0);
            }
            finally
            {
                accountDueRepository.ReleaseResource();
                tenantTracking.SyunoSeikyus.Remove(syunoSeikyu);
                tenantTracking.SyunoNyukin.Remove(syunoNyukin);
                tenantTracking.RaiinInfs.Remove(raiinInf);
                tenantTracking.KaMsts.Remove(kaMst);
                tenantTracking.SaveChanges();
            }

        }
    }
}