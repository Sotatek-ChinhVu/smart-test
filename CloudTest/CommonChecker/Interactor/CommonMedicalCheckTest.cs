using CommonChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using Interactor.CommonChecker.CommonMedicalCheck;
using Moq;

namespace CloudUnitTest.CommonChecker.Interactor
{
    public class CommonMedicalCheckTest : BaseUT
    {
        [Test]
        public void TC_001_GetItemCdError_DrugAllergy()
        {
            //Setup Data Test
            var listDrugAllergyResult = new List<DrugAllergyResultModel>()
            {
                new DrugAllergyResultModel()
                {
                    Level = 1,
                    ItemCd = "UT1234",
                    YjCd = "YJ123456789",
                    SeibunCd = "UT7777",
                    AllergySeibunCd = "UT8888",
                    AllergyYjCd = "Al1234567890",
                    Tag = "TagTest"
                }
            };

            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.DrugAllergy,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = listDrugAllergyResult
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object);

            mock.Setup(finder => finder.FindComponentNameDic(It.IsAny<List<string>>()))
            .Returns((List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindItemNameByItemCodeDic(It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((List<string> inputList, int sinday) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindAnalogueNameDic(It.IsAny<List<string>>()))
           .Returns((List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindDrvalrgyNameDic(It.IsAny<List<string>>()))
           .Returns((List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._componentNameDictionary["UT7777"], Is.EqualTo("MockedValueFor_UT7777"));
            Assert.That(commonMedicalCheck._componentNameDictionary["UT8888"], Is.EqualTo("MockedValueFor_UT8888"));
            Assert.That(commonMedicalCheck._itemNameByItemCodeDictionary["UT1234"], Is.EqualTo("MockedValueFor_UT1234"));
            Assert.That(commonMedicalCheck._itemNameDictionary["YJ123456789"], Is.EqualTo("MockedValueFor_YJ123456789"));
            Assert.That(commonMedicalCheck._itemNameDictionary["Al1234567890"], Is.EqualTo("MockedValueFor_Al1234567890"));
            Assert.That(commonMedicalCheck._analogueNameDictionary["TagTest"], Is.EqualTo("MockedValueFor_TagTest"));
            Assert.That(commonMedicalCheck._drvalrgyNameDictionary["TagTest"], Is.EqualTo("MockedValueFor_TagTest"));
        }

        [Test]
        public void TC_002_GetItemCdError_DrugAllergy_DrugAllergyInfo_IsNull()
        {
            //Setup Data Test

            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.DrugAllergy,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = string.Empty
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object);

            mock.Setup(finder => finder.FindComponentNameDic(It.IsAny<List<string>>()))
            .Returns((List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindItemNameByItemCodeDic(It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((List<string> inputList, int sinday) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindAnalogueNameDic(It.IsAny<List<string>>()))
           .Returns((List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindDrvalrgyNameDic(It.IsAny<List<string>>()))
           .Returns((List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._componentNameDictionary.Count == 0);
            Assert.That(commonMedicalCheck._componentNameDictionary.Count == 0);
            Assert.That(commonMedicalCheck._itemNameByItemCodeDictionary.Count == 0);
            Assert.That(commonMedicalCheck._itemNameDictionary.Count == 0);
            Assert.That(commonMedicalCheck._itemNameDictionary.Count == 0);
            Assert.That(commonMedicalCheck._analogueNameDictionary.Count == 0);
            Assert.That(commonMedicalCheck._drvalrgyNameDictionary.Count == 0);
        }

        [Test]
        public void TC_003_GetItemCdError_FoodAllergy()
        {
            //Setup Data Test
            var foodAllergyInfo = new List<FoodAllergyResultModel>()
            {
                new FoodAllergyResultModel()
                {
                    PtId = 999999999,
                    AlrgyKbn = "9",
                    ItemCd = "UT88888",
                    YjCd = "YJ999777",
                    TenpuLevel = "6",
                    AttentionCmt = "Test Comment",
                    WorkingMechanism = "Sotatek"
                }
            };

            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.FoodAllergy,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = foodAllergyInfo
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object);

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindFoodNameDic(It.IsAny<List<string>>()))
           .Returns((List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));
            // Act
            commonMedicalCheck.GetItemCdError(listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary["YJ999777"], Is.EqualTo("MockedValueFor_YJ999777"));
            Assert.That(commonMedicalCheck._foodNameDictionary["9"], Is.EqualTo("MockedValueFor_9"));
        }

        [Test]
        public void TC_004_GetItemCdError_FoodAllergy_FoodAllergyInfo_IsNull()
        {
            //Setup Data Test

            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.FoodAllergy,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = string.Empty
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object);

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindFoodNameDic(It.IsAny<List<string>>()))
           .Returns((List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));
            // Act
            commonMedicalCheck.GetItemCdError(listErrorInfo);

            // Assert
            Assert.True(commonMedicalCheck._itemNameDictionary.Count == 0);
            Assert.True(commonMedicalCheck._itemNameDictionary.Count == 0);
        }

        [Test]
        public void TC_005_GetItemCdError_Age()
        {
            //Setup Data Test
            var ageResults = new List<AgeResultModel>()
            {
                new AgeResultModel()
                {
                    ItemCd = "UT88888",
                    YjCd = "YJ999777",
                    TenpuLevel = "6",
                    WorkingMechanism = "Sotatek"
                }
            };

            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.Age,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = ageResults
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object);

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary["YJ999777"], Is.EqualTo("MockedValueFor_YJ999777"));
        }

        [Test]
        public void TC_006_GetItemCdError_Age_AgeErrorInfo_IsNull()
        {
            //Setup Data Test

            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.Age,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = string.Empty
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object);

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary.Count == 0);
        }

        [Test]
        public void TC_007_GetItemCdError_Disease()
        {
            //Setup Data Test
            var diseaseResults = new List<DiseaseResultModel>()
            {
                new DiseaseResultModel()
                {
                    ItemCd = "UT88888",
                    YjCd = "YJ999777",
                    ByotaiCd = "Byo99999"
                }
            };

            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.Disease,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = diseaseResults
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object);

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindDiseaseNameDic(It.IsAny<List<string>>()))
            .Returns((List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary["YJ999777"], Is.EqualTo("MockedValueFor_YJ999777"));
            Assert.That(commonMedicalCheck._diseaseNameDictionary["Byo99999"], Is.EqualTo("MockedValueFor_Byo99999"));
        }

        [Test]
        public void TC_008_GetItemCdError_Disease_DiseaseErrorInfo_IsNull()
        {
            //Setup Data Test

            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.Disease,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = string.Empty
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object);

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindDiseaseNameDic(It.IsAny<List<string>>()))
            .Returns((List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary.Count == 0);
            Assert.That(commonMedicalCheck._diseaseNameDictionary.Count == 0);
        }
    }
}