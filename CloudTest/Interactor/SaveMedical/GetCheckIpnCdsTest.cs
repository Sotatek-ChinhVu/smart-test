using Infrastructure.Options;
using Infrastructure.Repositories;
using Microsoft.Extensions.Options;
using Moq;

namespace CloudUnitTest.Interactor.SaveMedical
{
    public class GetCheckIpnCdsTest : BaseUT
    {
        [Test]
        public void TC_001_SaveMedicalInteractor_TestGetCheckIpnCdsSuccess()
        {
            // Arrange
            var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();
            var tenant = TenantProvider.GetNoTrackingDataContext();
            var mstItemRepository = new MstItemRepository(TenantProvider, mockOptionsAccessor.Object);
            var ipnCds = tenant.IpnNameMsts.Take(30).Select(x => x.IpnNameCd).ToList();

            // Act
            var getCheckIpnCds = mstItemRepository.GetCheckIpnCds(ipnCds);

            try
            {
                // Assert
                Assert.That(getCheckIpnCds.Count > 0);
            }
            finally
            {
                mstItemRepository.ReleaseResource();
            }
        }
    }
}
