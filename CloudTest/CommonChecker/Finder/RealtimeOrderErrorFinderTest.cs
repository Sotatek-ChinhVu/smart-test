using CommonChecker.DB;
using Entity.Tenant;

namespace CloudUnitTest.CommonChecker.Finder
{
    public class RealtimeOrderErrorFinderTest : BaseUT
    {
        [Test]
        public void TC_001_FindAgeComment_Test_AgeCommentInfo_Is_Not_Null()
        {

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var testCmt1 = "UT6666";
            var testCmt2 = "UT7777";
            var ageCommentInfo = new List<M14CmtCode>()
            {
                new M14CmtCode()
                {
                    AttentionCmt = null,
                    AttentionCmtCd = testCmt1,
                },
                new M14CmtCode()
                {
                    AttentionCmt = "COMMENT-Test",
                    AttentionCmtCd = testCmt2,
                },
            };

            tenantTracking.M14CmtCode.AddRange(ageCommentInfo);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result1 = realtimcheckerfinder.FindAgeComment(testCmt1);
                var result2 = realtimcheckerfinder.FindAgeComment(testCmt2);

                // Assert
                Assert.AreEqual(string.Empty, result1);
                Assert.AreEqual("COMMENT-Test", result2);
            }
            finally
            {
                tenantTracking.M14CmtCode.RemoveRange(ageCommentInfo);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_002_FindAgeComment_Test_AgeCommentInfo_Is_Null()
        {

            //Setup Data Test
            var testCmt1 = "UT6666";
            var testCmt2 = "UT7777";

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            // Act
            var result1 = realtimcheckerfinder.FindAgeComment(testCmt1);
            var result2 = realtimcheckerfinder.FindAgeComment(testCmt2);

            // Assert
            Assert.AreEqual(string.Empty, result1);
            Assert.AreEqual(string.Empty, result2);
        }

        [Test]
        public void TC_003_FindAnalogueName_Test_AnalogueInfo_Is_Not_Null()
        {

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var testAnalogueCd1 = "UT7777";
            var testAnalogueCd2 = "UT8888";
            var m56AnalogueCds = new List<M56AnalogueCd>()
            {
                new M56AnalogueCd()
                {
                    AnalogueCd = testAnalogueCd1,
                    AnalogueName = null,
                },
                new M56AnalogueCd()
                {
                    AnalogueCd = testAnalogueCd2,
                    AnalogueName = "Analogue-Name-Test",
                },
            };

            tenantTracking.M56AnalogueCd.AddRange(m56AnalogueCds);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result1 = realtimcheckerfinder.FindAnalogueName(testAnalogueCd1);
                var result2 = realtimcheckerfinder.FindAnalogueName(testAnalogueCd2);

                // Assert
                Assert.AreEqual(string.Empty, result1);
                Assert.AreEqual("Analogue-Name-Test", result2);
            }
            finally
            {
                tenantTracking.M56AnalogueCd.RemoveRange(m56AnalogueCds);
                tenantTracking.SaveChanges();
            }

        }
    }
}
