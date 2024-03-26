using Domain.Models.User;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using IDatabase = Microsoft.EntityFrameworkCore.Storage.IDatabase;

namespace CloudUnitTest.Interactor.SaveMedical
{
    public class GetCheckIpnCdsTest : BaseUT
    {
        [Test]
        public void TC_001_SaveMedicalInteractor_TestGetCheckIpnCdsSuccess()
        {
            //Setup Data Test
            var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();
            var tenant = TenantProvider.GetNoTrackingDataContext();
            var mstItemRepository = new MstItemRepository(TenantProvider, mockOptionsAccessor.Object);
            var ipnCds = tenant.IpnNameMsts.Take(30).Select(x => x.IpnNameCd).ToList();
            var getCheckIpnCds = mstItemRepository.GetCheckIpnCds(ipnCds);

            try
            {
                Assert.That(getCheckIpnCds.Count > 0);
            }
            finally
            {
                mstItemRepository.ReleaseResource();
            }
        }
    }
}
