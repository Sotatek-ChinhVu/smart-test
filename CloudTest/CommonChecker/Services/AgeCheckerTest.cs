using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using CommonCheckers.OrderRealtimeChecker.Services;
using Domain.Models.Diseases;
using Domain.Models.Family;
using Moq;
using UseCase.Family;
using UseCase.MedicalExamination.SaveMedical;
using SpecialNoteFull = Domain.Models.SpecialNote.SpecialNoteModel;

namespace CloudUnitTest.CommonChecker.Services
{
    public class AgeCheckerTest : BaseUT
    {
        [Test]
        public void HandleCheckOrderList_InvalidSettingLevel_ReturnsOriginalResult()
        {
            var odrInf = new List<OrdInfoModel>();
            int sinDay = 20230603;
            int ptId = 1231;
            bool isDataOfDb = false;
            // Arrange
            var unitCheckerForOrderListResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(RealtimeCheckerType.Age, odrInf, sinDay, 1231, new(new(), new(), new()), new(), new(), isDataOfDb);
            unitCheckerForOrderListResult.ChangeCheckingOrderList(new());

            // Assert
            Assert.AreEqual(unitCheckerForOrderListResult, new());
        }

        [Test]
        public void CheckAge_ReturnsExpectedResult()
        {
            // Arrange
            var unitChecker = new AgeChecker<OrdInfoModel, OrdInfoDetailModel>();
            var checkingOrderList = new List<OrdInfoModel>();
            var specialNoteItem = new SpecialNoteItem();
            var ptDiseaseModels = new List<PtDiseaseModel>();
            var familyItems = new List<FamilyItem>();
            var isDataOfDb = true;

            var odrInf = new List<OrdInfoModel>();
            int sinDay = 20230603;
            int ptId = 1231;

            var mockAgeChecker = new Mock<AgeChecker<OrdInfoModel, OrdInfoDetailModel>>();
            var expectedResult = new UnitCheckerForOrderListResult<OrdInfoModel, OrdInfoDetailModel>(RealtimeCheckerType.Age, odrInf, sinDay, 1231, new(new(), new(), new()), new(), new(), isDataOfDb);
            mockAgeChecker.Setup(c => c.CheckOrderList(
                It.IsAny<List<OrdInfoModel>>(),
                It.IsAny<SpecialNoteFull>(),
                It.IsAny<List<PtDiseaseModel>>(),
                It.IsAny<List<FamilyModel>>(),
                It.IsAny<bool>()
            )).Returns(expectedResult);
            unitChecker.AgeChecker = mockAgeChecker.Object;

            // Act
            var result = unitChecker.CheckAge(checkingOrderList, specialNoteItem, ptDiseaseModels, familyItems, isDataOfDb);

            // Assert
            Assert.AreSame(expectedResult, result);
        }

    }
}
