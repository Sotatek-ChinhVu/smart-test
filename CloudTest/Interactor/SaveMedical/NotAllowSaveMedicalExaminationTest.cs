using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Moq;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using IDatabase = Microsoft.EntityFrameworkCore.Storage.IDatabase;

namespace CloudUnitTest.Interactor.SaveMedical
{
    public class NotAllowSaveMedicalExaminationTest : BaseUT
    {
        [Test]
        public void TC_001_SaveMedicalInteractor_TestNotAllowSaveMedicalExaminationSuccess()
        {
            //Setup Data Test
            var mockIUserInfoService = new Mock<IUserInfoService>();
            var mockIConfiguration = new Mock<IConfiguration>();
            var mock_cache = new Mock<IDatabase>();
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockIConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");

            //Arrange
            var userRepository = new UserRepository(TenantProvider, mockIConfiguration.Object, mockIUserInfoService.Object);

            //Mock data
            int hpId = 1;
            long ptId = 28032001;
            long raiinNo = 28032001;
            int sinDate = 28032001;
            int userId = 28032001;

            var result = userRepository.NotAllowSaveMedicalExamination(hpId, ptId, raiinNo, sinDate, userId);

            try
            {
                Assert.That(!result);
            }
            finally
            {
                userRepository.ReleaseResource();
            }
        }
    }
}
