using Entity.Tenant;
using Infrastructure.Repositories;
using System;

namespace CloudUnitTest.Repository.AccountDueRepo
{
    public class GetUketsukeSbtTest : BaseUT
    {
        [Test]
        public void TC_001_NextOrderRepository_GetUketsukeSbt()
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
    }
}