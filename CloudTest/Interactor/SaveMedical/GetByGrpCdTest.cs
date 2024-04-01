using Infrastructure.Repositories;
using Moq;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace CloudUnitTest.Interactor.SaveMedical
{
    public class GetByGrpCdTest : BaseUT
    {
        [Test]
        public void TC_001_SaveMedicalInteractor_TestGetByGrpCd_return_new()
        {
            //Setup Data Test
            var mockIConfiguration = new Mock<IConfiguration>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            var systemConfRepository = new SystemConfRepository(TenantProvider, mockIConfiguration.Object);

            //Mock data
            int hpId = 1;
            int grpCd = 1;
            int grpEdaNo = 1;
            var output = systemConfRepository.GetByGrpCd(hpId, grpCd, grpEdaNo);

            Assert.That(output.HpId == 0 && output.GrpCd == 0 && output.GrpEdaNo == 0);
        }

        [Test]
        public void TC_002_SaveMedicalInteractor_TestGetByGrpCd()
        {
            //Setup Data Test
            var mockIConfiguration = new Mock<IConfiguration>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            var systemConfRepository = new SystemConfRepository(TenantProvider, mockIConfiguration.Object);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            var systemConf = tenant.SystemConfs.FirstOrDefault();

            //Mock data
            int hpId = systemConf.HpId;
            int grpCd = systemConf.GrpCd;
            int grpEdaNo = systemConf.GrpEdaNo;
            var output = systemConfRepository.GetByGrpCd(hpId, grpCd, grpEdaNo);

            Assert.That(output.HpId != 0 || output.GrpCd != 0 || output.GrpEdaNo != 0);
        }
    }
}
