using CommonChecker.Models;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Models;
using Domain.Models.SpecialNote.PatientInfo;
using Moq;
using PostgreDataContext;

namespace CloudUnitTest.CommonChecker.Services
{
    public class AgeCheckerTest : BaseUT
    {
        public TenantNoTrackingDataContext NoTrackingDataContext { get; private set; }
        public AgeCheckerTest(TenantNoTrackingDataContext noTrackingDataContext)
        {
            NoTrackingDataContext = noTrackingDataContext;
        }

        [Test]
        public void CheckAge_001_ReturnsEmptyList_WhenPatientInfoIsNull()
        {
            // Arrange
            var ageChecker = new List<AgeResultModel>();
            var realtimcheckerfinder = new RealtimeCheckerFinder(NoTrackingDataContext);

            //Setup
            int hpId = 1;
            long ptId = 1231;
            int sinDay = 20230605;
            int level = 0;
            int ageTypeCheckSetting = 1;
            var listItemCode = new List<ItemCodeModel>();
            var kensaInfDetailModels = new List<KensaInfDetailModel>();
            bool isDataOfDb = true;

            // Act
            var result = realtimcheckerfinder.CheckAge(hpId, ptId, sinDay, level, ageTypeCheckSetting, listItemCode, kensaInfDetailModels, isDataOfDb);

            // Assert
            Assert.IsNull(result);
        }

    }
}
