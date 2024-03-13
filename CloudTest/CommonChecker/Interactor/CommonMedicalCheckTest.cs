using CloudUnitTest.SampleData;
using CommonChecker.DB;
using CommonChecker.Models;
using CommonChecker.Models.OrdInf;
using CommonChecker.Models.OrdInfDetailModel;
using CommonCheckers.OrderRealtimeChecker.DB;
using CommonCheckers.OrderRealtimeChecker.Enums;
using CommonCheckers.OrderRealtimeChecker.Models;
using Domain.Models.Family;
using Interactor.CommonChecker.CommonMedicalCheck;
using Moq;
using UseCase.Family;
using UseCase.MedicalExamination.SaveMedical;
using UseCase.SpecialNote.Save;

namespace CloudUnitTest.CommonChecker.Interactor
{
    public class CommonMedicalCheckTest : BaseUT
    {
        [Test]
        public void TC_001_GetItemCdError_DrugAllergy()
        {
            int hpId = 1;
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
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindComponentNameDic(It.IsAny<int>(), It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindAnalogueNameDic(It.IsAny<int>(), It.IsAny<List<string>>()))
           .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindDrvalrgyNameDic(It.IsAny<int>(), It.IsAny<List<string>>()))
           .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindItemNameByItemCodeDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

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
            int hpId = 1;
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
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

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
            int hpId = 1;
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
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindFoodNameDic(hpId, It.IsAny<List<string>>()))
           .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));
            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary["YJ999777"], Is.EqualTo("MockedValueFor_YJ999777"));
            Assert.That(commonMedicalCheck._foodNameDictionary["9"], Is.EqualTo("MockedValueFor_9"));
        }

        [Test]
        public void TC_004_GetItemCdError_FoodAllergy_FoodAllergyInfo_IsNull()
        {
            //Setup Data Test
            int hpId = 1;
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
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindFoodNameDic(hpId, It.IsAny<List<string>>()))
           .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));
            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.True(commonMedicalCheck._itemNameDictionary.Count == 0);
            Assert.True(commonMedicalCheck._itemNameDictionary.Count == 0);
        }

        [Test]
        public void TC_005_GetItemCdError_Age()
        {
            int hpId = 1;
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
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary["YJ999777"], Is.EqualTo("MockedValueFor_YJ999777"));
        }

        [Test]
        public void TC_006_GetItemCdError_Age_AgeErrorInfo_IsNull()
        {
            int hpId = 1;
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
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary.Count == 0);
        }

        [Test]
        public void TC_007_GetItemCdError_Disease()
        {
            int hpId = 1;
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
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindDiseaseNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary["YJ999777"], Is.EqualTo("MockedValueFor_YJ999777"));
            Assert.That(commonMedicalCheck._diseaseNameDictionary["Byo99999"], Is.EqualTo("MockedValueFor_Byo99999"));
        }

        [Test]
        public void TC_008_GetItemCdError_Disease_DiseaseErrorInfo_IsNull()
        {
            //Setup Data Test
            int hpId = 1;
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
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindDiseaseNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary.Count == 0);
            Assert.That(commonMedicalCheck._diseaseNameDictionary.Count == 0);
        }

        [Test]
        public void TC_009_GetItemCdError_Kinki()
        {
            int hpId = 1;

            //Setup Data Test
            var kinkiResults = new List<KinkiResultModel>()
            {
                new KinkiResultModel()
                {
                    AYjCd = "A12345",
                    BYjCd = "B56789",
                    CommentCode = "KinkiComment",
                    SayokijyoCode = "KijiyoComment",
                    SeibunCd = "Sebun7755",
                }
            };

            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.Kinki,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = kinkiResults
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKinkiCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKijyoCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindOTCItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetOTCComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetSupplementComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindSuppleItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary["A12345"], Is.EqualTo("MockedValueFor_A12345"));
            Assert.That(commonMedicalCheck._itemNameDictionary["B56789"], Is.EqualTo("MockedValueFor_B56789"));
            Assert.That(commonMedicalCheck._kinkiCommentDictionary["KinkiComment"], Is.EqualTo("MockedValueFor_KinkiComment"));
            Assert.That(commonMedicalCheck._kijyoCommentDictionary["KijiyoComment"], Is.EqualTo("MockedValueFor_KijiyoComment"));
            Assert.That(commonMedicalCheck._oTCItemNameDictionary["B56789"], Is.EqualTo("MockedValueFor_B56789"));
            Assert.That(commonMedicalCheck._oTCComponentInfoDictionary["Sebun7755"], Is.EqualTo("MockedValueFor_Sebun7755"));
            Assert.That(commonMedicalCheck._supplementComponentInfoDictionary["Sebun7755"], Is.EqualTo("MockedValueFor_Sebun7755"));
            Assert.That(commonMedicalCheck._suppleItemNameDictionary["Sebun7755"], Is.EqualTo("MockedValueFor_Sebun7755"));
        }

        [Test]
        public void TC_010_GetItemCdError_Kinki_KinkiErrorInfo_IsNull()
        {
            //Setup Data Test
            int hpId = 1;
            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.Kinki,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = string.Empty
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKinkiCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKijyoCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindOTCItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetOTCComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetSupplementComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindSuppleItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.True(commonMedicalCheck._itemNameDictionary.Count == 0);
            Assert.True(commonMedicalCheck._itemNameDictionary.Count == 0);
            Assert.True(commonMedicalCheck._kinkiCommentDictionary.Count == 0);
            Assert.True(commonMedicalCheck._kijyoCommentDictionary.Count == 0);
            Assert.True(commonMedicalCheck._oTCItemNameDictionary.Count == 0);
            Assert.True(commonMedicalCheck._oTCComponentInfoDictionary.Count == 0);
            Assert.True(commonMedicalCheck._supplementComponentInfoDictionary.Count == 0);
            Assert.True(commonMedicalCheck._suppleItemNameDictionary.Count == 0);
        }

        [Test]
        public void TC_011_GetItemCdError_KinkiTain()
        {
            int hpId = 1;
            //Setup Data Test
            var kinkiResults = new List<KinkiResultModel>()
            {
                new KinkiResultModel()
                {
                    AYjCd = "A12345",
                    BYjCd = "B56789",
                    CommentCode = "KinkiComment",
                    SayokijyoCode = "KijiyoComment",
                    SeibunCd = "Sebun7755",
                }
            };

            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.KinkiTain,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = kinkiResults
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKinkiCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKijyoCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindOTCItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetOTCComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetSupplementComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindSuppleItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary["A12345"], Is.EqualTo("MockedValueFor_A12345"));
            Assert.That(commonMedicalCheck._itemNameDictionary["B56789"], Is.EqualTo("MockedValueFor_B56789"));
            Assert.That(commonMedicalCheck._kinkiCommentDictionary["KinkiComment"], Is.EqualTo("MockedValueFor_KinkiComment"));
            Assert.That(commonMedicalCheck._kijyoCommentDictionary["KijiyoComment"], Is.EqualTo("MockedValueFor_KijiyoComment"));
            Assert.That(commonMedicalCheck._oTCItemNameDictionary["B56789"], Is.EqualTo("MockedValueFor_B56789"));
            Assert.That(commonMedicalCheck._oTCComponentInfoDictionary["Sebun7755"], Is.EqualTo("MockedValueFor_Sebun7755"));
            Assert.That(commonMedicalCheck._supplementComponentInfoDictionary["Sebun7755"], Is.EqualTo("MockedValueFor_Sebun7755"));
            Assert.That(commonMedicalCheck._suppleItemNameDictionary["Sebun7755"], Is.EqualTo("MockedValueFor_Sebun7755"));
        }

        [Test]
        public void TC_012_GetItemCdError_KinkiTain_KinkiErrorInfo_IsNull()
        {
            //Setup Data Test
            int hpId = 1;
            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.KinkiTain,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = string.Empty
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKinkiCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKijyoCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindOTCItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetOTCComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetSupplementComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindSuppleItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.True(commonMedicalCheck._itemNameDictionary.Count == 0);
            Assert.True(commonMedicalCheck._itemNameDictionary.Count == 0);
            Assert.True(commonMedicalCheck._kinkiCommentDictionary.Count == 0);
            Assert.True(commonMedicalCheck._kijyoCommentDictionary.Count == 0);
            Assert.True(commonMedicalCheck._oTCItemNameDictionary.Count == 0);
            Assert.True(commonMedicalCheck._oTCComponentInfoDictionary.Count == 0);
            Assert.True(commonMedicalCheck._supplementComponentInfoDictionary.Count == 0);
            Assert.True(commonMedicalCheck._suppleItemNameDictionary.Count == 0);
        }

        [Test]
        public void TC_013_GetItemCdError_KinkiOTC()
        {
            int hpId = 1;
            //Setup Data Test
            var kinkiResults = new List<KinkiResultModel>()
            {
                new KinkiResultModel()
                {
                    AYjCd = "A12345",
                    BYjCd = "B56789",
                    CommentCode = "KinkiComment",
                    SayokijyoCode = "KijiyoComment",
                    SeibunCd = "Sebun7755",
                }
            };

            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.KinkiOTC,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = kinkiResults
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKinkiCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKijyoCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindOTCItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetOTCComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetSupplementComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindSuppleItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary["A12345"], Is.EqualTo("MockedValueFor_A12345"));
            Assert.That(commonMedicalCheck._itemNameDictionary["B56789"], Is.EqualTo("MockedValueFor_B56789"));
            Assert.That(commonMedicalCheck._kinkiCommentDictionary["KinkiComment"], Is.EqualTo("MockedValueFor_KinkiComment"));
            Assert.That(commonMedicalCheck._kijyoCommentDictionary["KijiyoComment"], Is.EqualTo("MockedValueFor_KijiyoComment"));
            Assert.That(commonMedicalCheck._oTCItemNameDictionary["B56789"], Is.EqualTo("MockedValueFor_B56789"));
            Assert.That(commonMedicalCheck._oTCComponentInfoDictionary["Sebun7755"], Is.EqualTo("MockedValueFor_Sebun7755"));
            Assert.That(commonMedicalCheck._supplementComponentInfoDictionary["Sebun7755"], Is.EqualTo("MockedValueFor_Sebun7755"));
            Assert.That(commonMedicalCheck._suppleItemNameDictionary["Sebun7755"], Is.EqualTo("MockedValueFor_Sebun7755"));
        }

        [Test]
        public void TC_014_GetItemCdError_KinkiOTC_KinkiErrorInfo_IsNull()
        {
            //Setup Data Test
            int hpId = 1;
            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.KinkiOTC,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = string.Empty
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>(); var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKinkiCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKijyoCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindOTCItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetOTCComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetSupplementComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindSuppleItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.True(commonMedicalCheck._itemNameDictionary.Count == 0);
            Assert.True(commonMedicalCheck._itemNameDictionary.Count == 0);
            Assert.True(commonMedicalCheck._kinkiCommentDictionary.Count == 0);
            Assert.True(commonMedicalCheck._kijyoCommentDictionary.Count == 0);
            Assert.True(commonMedicalCheck._oTCItemNameDictionary.Count == 0);
            Assert.True(commonMedicalCheck._oTCComponentInfoDictionary.Count == 0);
            Assert.True(commonMedicalCheck._supplementComponentInfoDictionary.Count == 0);
            Assert.True(commonMedicalCheck._suppleItemNameDictionary.Count == 0);
        }

        [Test]
        public void TC_015_GetItemCdError_KinkiSupplement()
        {
            int hpId = 1;
            //Setup Data Test
            var kinkiResults = new List<KinkiResultModel>()
            {
                new KinkiResultModel()
                {
                    AYjCd = "A12345",
                    BYjCd = "B56789",
                    CommentCode = "KinkiComment",
                    SayokijyoCode = "KijiyoComment",
                    SeibunCd = "Sebun7755",
                }
            };

            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.KinkiSupplement,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = kinkiResults
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>(); var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKinkiCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKijyoCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindOTCItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetOTCComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetSupplementComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindSuppleItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary["A12345"], Is.EqualTo("MockedValueFor_A12345"));
            Assert.That(commonMedicalCheck._itemNameDictionary["B56789"], Is.EqualTo("MockedValueFor_B56789"));
            Assert.That(commonMedicalCheck._kinkiCommentDictionary["KinkiComment"], Is.EqualTo("MockedValueFor_KinkiComment"));
            Assert.That(commonMedicalCheck._kijyoCommentDictionary["KijiyoComment"], Is.EqualTo("MockedValueFor_KijiyoComment"));
            Assert.That(commonMedicalCheck._oTCItemNameDictionary["B56789"], Is.EqualTo("MockedValueFor_B56789"));
            Assert.That(commonMedicalCheck._oTCComponentInfoDictionary["Sebun7755"], Is.EqualTo("MockedValueFor_Sebun7755"));
            Assert.That(commonMedicalCheck._supplementComponentInfoDictionary["Sebun7755"], Is.EqualTo("MockedValueFor_Sebun7755"));
            Assert.That(commonMedicalCheck._suppleItemNameDictionary["Sebun7755"], Is.EqualTo("MockedValueFor_Sebun7755"));
        }

        [Test]
        public void TC_016_GetItemCdError_KinkiSupplement_KinkiErrorInfo_IsNull()
        {
            //Setup Data Test
            int hpId = 1;
            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.KinkiSupplement,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = string.Empty
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKinkiCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKijyoCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindOTCItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetOTCComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetSupplementComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindSuppleItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.True(commonMedicalCheck._itemNameDictionary.Count == 0);
            Assert.True(commonMedicalCheck._itemNameDictionary.Count == 0);
            Assert.True(commonMedicalCheck._kinkiCommentDictionary.Count == 0);
            Assert.True(commonMedicalCheck._kijyoCommentDictionary.Count == 0);
            Assert.True(commonMedicalCheck._oTCItemNameDictionary.Count == 0);
            Assert.True(commonMedicalCheck._oTCComponentInfoDictionary.Count == 0);
            Assert.True(commonMedicalCheck._supplementComponentInfoDictionary.Count == 0);
            Assert.True(commonMedicalCheck._suppleItemNameDictionary.Count == 0);
        }

        [Test]
        public void TC_017_GetItemCdError_KinkiUser()
        {
            int hpId = 1;
            //Setup Data Test
            var kinkiResults = new List<KinkiResultModel>()
            {
                new KinkiResultModel()
                {
                    AYjCd = "YJ888889",
                    BYjCd = "YJ999777",
                }
            };

            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.KinkiUser,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = kinkiResults
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary["YJ888889"], Is.EqualTo("MockedValueFor_YJ888889"));
            Assert.That(commonMedicalCheck._itemNameDictionary["YJ999777"], Is.EqualTo("MockedValueFor_YJ999777"));
        }

        [Test]
        public void TC_018_GetItemCdError_KinkiUser_KinkiUserErrorInfo_IsNull()
        {
            //Setup Data Test
            int hpId = 1;
            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.KinkiUser,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = string.Empty
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.True(commonMedicalCheck._itemNameDictionary.Count == 0);
            Assert.True(commonMedicalCheck._diseaseNameDictionary.Count == 0);
        }

        [Test]
        public void TC_019_GetItemCdError_Day()
        {
            int hpId = 1;
            //Setup Data Test
            var kinkiResults = new List<DayLimitResultModel>()
            {
                new DayLimitResultModel()
                {
                    YjCd = "YJ888889"
                }
            };

            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.Days,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = kinkiResults
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary["YJ888889"], Is.EqualTo("MockedValueFor_YJ888889"));
        }

        [Test]
        public void TC_020_GetItemCdError_Day_DayLimitErrorInfo_IsNull()
        {
            //Setup Data Test
            var hpId = 1;
            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.Dosage,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = string.Empty
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.True(commonMedicalCheck._itemNameDictionary.Count == 0);
        }

        [Test]
        public void TC_021_GetItemCdError_Dosage()
        {
            int hpId = 1;
            //Setup Data Test
            var dosageResults = new List<DosageResultModel>()
            {
                new DosageResultModel()
                {
                    YjCd = "YJ888889"
                }
            };

            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.Dosage,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = dosageResults
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetUsageDosageDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary["YJ888889"], Is.EqualTo("MockedValueFor_YJ888889"));
            Assert.That(commonMedicalCheck._usageDosageDictionary["YJ888889"], Is.EqualTo("MockedValueFor_YJ888889"));
        }

        [Test]
        public void TC_022_GetItemCdError_Dosage_DosageErrorInfo_IsNull()
        {
            //Setup Data Test
            int hpId = 1;
            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.Dosage,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = string.Empty
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.GetUsageDosageDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameDictionary.Count == 0);
            Assert.That(commonMedicalCheck._usageDosageDictionary.Count == 0);
        }

        [Test]
        public void TC_023_GetItemCdError_Duplication()
        {
            int hpId = 1;
            //Setup Data Test
            var duplicationResults = new List<DuplicationResultModel>()
            {
                new DuplicationResultModel()
                {
                    ItemCd = "8310000001",
                    DuplicatedItemCd = "52100000001"
                }
            };

            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.Duplication,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = duplicationResults
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameByItemCodeDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameByItemCodeDictionary["8310000001"], Is.EqualTo("MockedValueFor_8310000001"));
            Assert.That(commonMedicalCheck._itemNameByItemCodeDictionary["52100000001"], Is.EqualTo("MockedValueFor_52100000001"));
        }

        [Test]
        public void TC_024_GetItemCdError_Duplication_DuplicationErrorInfo_IsNull()
        {
            //Setup Data Test
            int hpId = 1;
            var listErrorInfo = new List<UnitCheckInfoModel>()
            {
                new UnitCheckInfoModel()
                {
                    CheckerType = RealtimeCheckerType.Duplication,
                    Sinday = 20230101,
                    PtId = 999,
                    IsError = true,
                    ErrorInfo = string.Empty
                }
            };

            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameByItemCodeDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            commonMedicalCheck.GetItemCdError(hpId, listErrorInfo);

            // Assert
            Assert.That(commonMedicalCheck._itemNameByItemCodeDictionary.Count == 0);
            Assert.That(commonMedicalCheck._itemNameByItemCodeDictionary.Count == 0);
        }

        [Test]
        public void TC_025_ProcessDataForDrugAllergyWithNoMasterData()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var allergyInfo = new List<DrugAllergyResultModel>
            {
                new DrugAllergyResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  Level = 1
                }
            };

            commonMedicalCheck._itemNameByItemCodeDictionary = new Dictionary<string, string> { { "UT1234", "Item Name Test" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDrugAllergyWithNoMasterData(allergyInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DrugAllergyChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("アレルギー"));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo("Item Name Test"));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo("Item Name Test"));
            Assert.That(errorInfo.ListLevelInfo.First().FirstItemName, Is.EqualTo("Item Name Test"));
            Assert.That(errorInfo.ListLevelInfo.First().SecondItemName, Is.EqualTo("Item Name Test"));
            Assert.That(errorInfo.ListLevelInfo.First().Level, Is.EqualTo(1));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("※アレルギー登録薬です。"));
        }

        [Test]
        public void TC_026_ProcessDataForKinkiUser()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var kinkiResults = new List<KinkiResultModel>
            {
                new KinkiResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  AYjCd = "A1234567",
                  BYjCd = "B7654321"
                }
            };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string> { { "A1234567", "Item Name Test 1" }, { "B7654321", "Item Name Test 2" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForKinkiUser(kinkiResults);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.KinkiChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("相互作用"));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo("Item Name Test 1"));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo("Item Name Test 2"));
            Assert.That(errorInfo.ListLevelInfo.First().FirstItemName, Is.EqualTo("Item Name Test 1"));
            Assert.That(errorInfo.ListLevelInfo.First().SecondItemName, Is.EqualTo("Item Name Test 2"));
            Assert.That(errorInfo.ListLevelInfo.First().Level, Is.EqualTo(1));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("ユーザー設定"));
        }

        [Test]
        public void TC_027_ProcessDataForDayLimit()
        {
            int hpId = 1;
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var kinkiResults = new List<DayLimitResultModel>
            {
                new DayLimitResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  UsingDay = 99,
                  LimitDay = 101,
                  YjCd = "YJ1234"
                }
            };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string> { { "A1234567", "Item Name Test 1" }, { "B7654321", "Item Name Test 2" } };

            mock.Setup(finder => finder.FindItemName(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
            .Returns((int hpId, string stringInput, int sinday) => $"Item Name Mock");

            // Act
            var result = commonMedicalCheck.ProcessDataForDayLimit(hpId, kinkiResults);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DayLimitChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("投与日数"));
            Assert.That(errorInfo.SecondCellContent, Is.EqualTo("ー"));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo("Item Name Mock"));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo("99日"));
            Assert.That(errorInfo.SuggestedContent, Is.EqualTo("／101日"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#f12c47"));
            Assert.That(errorInfo.ListLevelInfo.First().FirstItemName, Is.EqualTo("Item Name Mock"));
            Assert.That(errorInfo.ListLevelInfo.First().SecondItemName, Is.EqualTo(""));
            Assert.That(errorInfo.ListLevelInfo.First().Level, Is.EqualTo(0));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("投与日数制限（101日）を超えています。"));
        }

        [Test]
        public void TC_028_ProcessDataForDrugAllergy_Level_1()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var allergyInfo = new List<DrugAllergyResultModel>
            {
                new DrugAllergyResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "YJ1234",
                  AllergyYjCd = "Al67890",
                  Level = 1,
                  SeibunCd = "66666"
                },
                new DrugAllergyResultModel()
                {
                  Id = "2",
                  ItemCd = "UT12345",
                  YjCd = "YJ12345",
                  AllergyYjCd = "Al67899",
                  Level = 1,
                  SeibunCd = "77777"
                }
            };

            mock.Setup(finder => finder.IsNoMasterData(hpId))
                .Returns(false);

            commonMedicalCheck._componentNameDictionary = new Dictionary<string, string> { { "77777", "Name Test 1" } };
            // Act
            var result = commonMedicalCheck.ProcessDataForDrugAllergy(hpId, allergyInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DrugAllergyChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("アレルギー"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(errorInfo.ListLevelInfo.Count, Is.EqualTo(1));
            Assert.That(errorInfo.ListLevelInfo.First().Level, Is.EqualTo(1));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo("※アレルギー登録薬「」と成分（Name Test 1）が同じです。\r\n\r\n"));
        }

        [Test]
        public void TC_029_ProcessDataForDrugAllergy_Level_2()
        {
            int hpId = 1;
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var allergyInfo = new List<DrugAllergyResultModel>
            {
                new DrugAllergyResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "YJ1234",
                  AllergyYjCd = "Al67890",
                  Level = 2,
                  SeibunCd = "66666"
                },
                new DrugAllergyResultModel()
                {
                  Id = "2",
                  ItemCd = "UT12345",
                  YjCd = "YJ12345",
                  AllergyYjCd = "Al67899",
                  Level = 2,
                  SeibunCd = "77777"
                }
            };

            mock.Setup(finder => finder.IsNoMasterData(hpId))
                .Returns(false);

            mock.Setup(finder => finder.FindComponentName(hpId, It.IsAny<string>()))
            .Returns((int inputHpId, string stringInput) => $"MockedComponentName");

            commonMedicalCheck._componentNameDictionary = new Dictionary<string, string> { { "77777", "Name Test 1" } };
            // Act
            var result = commonMedicalCheck.ProcessDataForDrugAllergy(hpId, allergyInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DrugAllergyChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("アレルギー"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(errorInfo.ListLevelInfo.Count, Is.EqualTo(1));
            Assert.That(errorInfo.ListLevelInfo.First().Level, Is.EqualTo(2));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo("※「」の成分（Name Test 1）はアレルギー登録薬「」の成分（MockedComponentName）と活性体成分（Name Test 1）が同じです。\r\n\r\n"));
        }

        [Test]
        public void TC_030_ProcessDataForDrugAllergy_Level_3()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var allergyInfo = new List<DrugAllergyResultModel>
            {
                new DrugAllergyResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "YJ1234",
                  AllergyYjCd = "Al67890",
                  Level = 3,
                  SeibunCd = "66666"
                },
                new DrugAllergyResultModel()
                {
                  Id = "2",
                  ItemCd = "UT12345",
                  YjCd = "YJ12345",
                  AllergyYjCd = "Al67899",
                  Level = 3,
                  SeibunCd = "77777",
                  AllergySeibunCd = "888888",
                  Tag = "Tag9999"
                }
            };

            mock.Setup(finder => finder.IsNoMasterData(hpId))
                .Returns(false);

            mock.Setup(finder => finder.FindComponentName(hpId, It.IsAny<string>()))
            .Returns((int inputHpId, string stringInput) => $"MockedComponentName");

            commonMedicalCheck._componentNameDictionary = new Dictionary<string, string> { { "77777", "Name Test 1" }, { "888888", "Component_Mocked_Test_2" } };
            commonMedicalCheck._analogueNameDictionary = new Dictionary<string, string> { { "Tag9999", "Mocked_Tag_Name_1" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDrugAllergy(hpId, allergyInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DrugAllergyChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("アレルギー"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(errorInfo.ListLevelInfo.Count, Is.EqualTo(1));
            Assert.That(errorInfo.ListLevelInfo.First().Level, Is.EqualTo(3));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo("※「」の成分（Name Test 1）はアレルギー登録薬「」の成分（Component_Mocked_Test_2）の類似成分（Mocked_Tag_Name_1）です。\r\n\r\n"));
        }

        [Test]
        public void TC_031_ProcessDataForDrugAllergy_Level_4()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var allergyInfo = new List<DrugAllergyResultModel>
            {
                new DrugAllergyResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "YJ1234",
                  AllergyYjCd = "Al67890",
                  Level = 4,
                  SeibunCd = "66666"
                },
                new DrugAllergyResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  YjCd = "YJ1234",
                  AllergyYjCd = "Al67890",
                  Level = 4,
                  SeibunCd = "66667"
                },
                new DrugAllergyResultModel()
                {
                  Id = "2",
                  ItemCd = "UT12345",
                  YjCd = "YJ12345",
                  AllergyYjCd = "Al67899",
                  Level = 4,
                  SeibunCd = "77777",
                  AllergySeibunCd = "888888",
                  Tag = "Tag9999"
                }
            };

            mock.Setup(finder => finder.IsNoMasterData(hpId))
                .Returns(false);

            mock.Setup(finder => finder.FindComponentName(hpId, It.IsAny<string>()))
            .Returns((int inputHpId, string stringInput) => $"MockedComponentName");

            commonMedicalCheck._drvalrgyNameDictionary = new Dictionary<string, string> { { "Tag9999", "Mocked_Tag_Name_1" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDrugAllergy(hpId, allergyInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(3));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DrugAllergyChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("アレルギー"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(errorInfo.ListLevelInfo.Count, Is.EqualTo(1));
            Assert.That(errorInfo.ListLevelInfo.First().Level, Is.EqualTo(4));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo("※「」はアレルギー登録薬「」と同じ系統（Mocked_Tag_Name_1）の成分を含みます。\r\n\r\n"));
        }

        /// <summary>
        /// Level < 0  and Level > 4
        /// </summary>
        [Test]
        public void TC_032_ProcessDataForDrugAllergy_YjCd_Equal_AllergyYjCd()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var allergyInfo = new List<DrugAllergyResultModel>
            {
                new DrugAllergyResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "YJ1234",
                  AllergyYjCd = "YJ1234",
                  Level = 4,
                  SeibunCd = "66666"
                }
            };

            mock.Setup(finder => finder.IsNoMasterData(hpId))
                .Returns(false);

            mock.Setup(finder => finder.FindComponentName(hpId, It.IsAny<string>()))
            .Returns((int inputHpId, string stringInput) => $"MockedComponentName");

            commonMedicalCheck._drvalrgyNameDictionary = new Dictionary<string, string> { { "Tag9999", "Mocked_Tag_Name_1" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDrugAllergy(hpId, allergyInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DrugAllergyChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("アレルギー"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(errorInfo.ListLevelInfo.Count, Is.EqualTo(1));
            Assert.That(errorInfo.ListLevelInfo.First().Level, Is.EqualTo(4));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("※アレルギー登録薬です。\r\n\r\n"));
        }

        [Test]
        public void TC_033_ProcessDataForDrugAllergy_Level_OutOfRange_0_To_4()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var allergyInfo = new List<DrugAllergyResultModel>
            {
                new DrugAllergyResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "YJ1234",
                  AllergyYjCd = "Al67890",
                  Level = -1,
                  SeibunCd = "66666"
                },
                new DrugAllergyResultModel()
                {
                  Id = "2",
                  ItemCd = "UT12345",
                  YjCd = "YJ12345",
                  AllergyYjCd = "Al67899",
                  Level = 5,
                  SeibunCd = "77777",
                  AllergySeibunCd = "888888",
                  Tag = "Tag9999"
                }
            };

            mock.Setup(finder => finder.IsNoMasterData(hpId))
                .Returns(false);

            mock.Setup(finder => finder.FindComponentName(hpId, It.IsAny<string>()))
            .Returns((int inputHpId, string stringInput) => $"MockedComponentName");

            commonMedicalCheck._drvalrgyNameDictionary = new Dictionary<string, string> { { "Tag9999", "Mocked_Tag_Name_1" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDrugAllergy(hpId, allergyInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.First().ListLevelInfo.First().Comment, Is.EqualTo(string.Empty));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// IsNoMasterData() Is True
        /// </summary>
        [Test]
        public void TC_034_ProcessDataForDrugAllergy_IsNoMasterData_IsTrue()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var allergyInfo = new List<DrugAllergyResultModel>
            {
                new DrugAllergyResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  Level = 1
                }
            };

            mock.Setup(finder => finder.IsNoMasterData(hpId))
                .Returns(true);

            mock.Setup(finder => finder.FindComponentName(hpId, It.IsAny<string>()))
            .Returns((int inputHpId, string stringInput) => $"MockedComponentName");

            commonMedicalCheck._drvalrgyNameDictionary = new Dictionary<string, string> { { "Tag9999", "Mocked_Tag_Name_1" } };

            commonMedicalCheck._itemNameByItemCodeDictionary = new Dictionary<string, string> { { "UT1234", "Item Name Test" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDrugAllergy(hpId, allergyInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DrugAllergyChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("アレルギー"));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo("Item Name Test"));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo("Item Name Test"));
            Assert.That(errorInfo.ListLevelInfo.First().FirstItemName, Is.EqualTo("Item Name Test"));
            Assert.That(errorInfo.ListLevelInfo.First().SecondItemName, Is.EqualTo("Item Name Test"));
            Assert.That(errorInfo.ListLevelInfo.First().Level, Is.EqualTo(1));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("※アレルギー登録薬です。"));
        }

        [Test]
        public void TC_035_ProcessDataForFoodAllergy_Test_AllergyInfo_IsEmpty_List()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var allergyInfo = new List<FoodAllergyResultModel>();

            mock.Setup(finder => finder.IsNoMasterData(hpId))
                .Returns(false);

            mock.Setup(finder => finder.FindComponentName(hpId, It.IsAny<string>()))
            .Returns((int inputHpId, string stringInput) => $"MockedComponentName");

            commonMedicalCheck._drvalrgyNameDictionary = new Dictionary<string, string> { { "Tag9999", "Mocked_Tag_Name_1" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForFoodAllergy(allergyInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        /// <summary>
        /// 1 <= level && level <= 3
        /// </summary>
        [Test]
        public void TC_036_ProcessDataForFoodAllergy()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var allergyInfo = new List<FoodAllergyResultModel>
            {
                new FoodAllergyResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "Yj888888",
                  AlrgyKbn = "9988",
                  AttentionCmt = "Attention Comment Test 1 ",
                  WorkingMechanism = "WorkingMechanism Test 1",
                  TenpuLevel = "2"
                },
                new FoodAllergyResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "Yj888888",
                  AlrgyKbn = "9988",
                  AttentionCmt = "Attention Comment Test 2",
                  WorkingMechanism = "WorkingMechanism Test 2",
                  TenpuLevel = "2"
                }
            };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string> { { "Yj888888", "Mocked_YjCd_Name_1" } };

            commonMedicalCheck._foodNameDictionary = new Dictionary<string, string> { { "9988", "Mocked_AlrgyKbn_Name_1" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForFoodAllergy(allergyInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.FoodAllergyChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("アレルギー"));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo("Mocked_YjCd_Name_1"));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo("Mocked_AlrgyKbn_Name_1"));
            Assert.That(errorInfo.ListLevelInfo.Count, Is.EqualTo(1));
            Assert.That(errorInfo.ListLevelInfo.First().Caption, Is.EqualTo("Mocked_YjCd_Name_1 × Mocked_AlrgyKbn_Name_1"));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("Attention Comment Test 1 \r\nWorkingMechanism Test 1\r\n\r\nAttention Comment Test 2\r\nWorkingMechanism Test 2\r\n\r\n"));
        }

        /// <summary>
        /// Test level = 0 & level = 4
        /// </summary>
        [Test]
        public void TC_037_ProcessDataForFoodAllergy()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var allergyInfo = new List<FoodAllergyResultModel>
            {
                new FoodAllergyResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "Yj888888",
                  AlrgyKbn = "9988",
                  AttentionCmt = "Attention Comment Test 1 ",
                  WorkingMechanism = "WorkingMechanism Test 1",
                  TenpuLevel = "0"
                },
                new FoodAllergyResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "Yj888888",
                  AlrgyKbn = "9988",
                  AttentionCmt = "Attention Comment Test 2",
                  WorkingMechanism = "WorkingMechanism Test 2",
                  TenpuLevel = "4"
                }
            };

            // Act
            var result = commonMedicalCheck.ProcessDataForFoodAllergy(allergyInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.FoodAllergyChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("アレルギー"));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo(""));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo(""));
            Assert.That(errorInfo.ListLevelInfo.Count, Is.EqualTo(2));
            Assert.That(errorInfo.ListLevelInfo.First().Caption, Is.EqualTo(""));
            Assert.That(errorInfo.ListLevelInfo.First().BackgroundCode, Is.EqualTo("#FFFFFF"));
            Assert.That(errorInfo.ListLevelInfo.First().BorderBrushCode, Is.EqualTo("#999999"));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo(""));
        }

        [Test]
        public void TC_038_ProcessDataForAge_()
        {
            int hpId = 1;
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var ages = new List<AgeResultModel>
            {
                new AgeResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "Yj888888",
                  WorkingMechanism = "WorkingMechanism Test 1",
                  TenpuLevel = "0"
                },
            };

            // Act
            var result = commonMedicalCheck.ProcessDataForAge(hpId, ages);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(1));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.AgeChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("投与年齢"));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo(""));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo("ー"));
            Assert.That(errorInfo.ListLevelInfo.Count, Is.EqualTo(1));
            Assert.That(errorInfo.ListLevelInfo.First().Caption, Is.EqualTo(""));
            Assert.That(errorInfo.ListLevelInfo.First().BackgroundCode, Is.EqualTo("#d8e4bc"));
            Assert.That(errorInfo.ListLevelInfo.First().BorderBrushCode, Is.EqualTo("#c8c8c8"));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("情報なし"));
        }

        [Test]
        public void TC_039_ProcessDataForAge_LevelInfo_IsNotNull()
        {
            int hpId = 1;
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var ages = new List<AgeResultModel>
            {
                new AgeResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "Yj888888",
                  WorkingMechanism = "WorkingMechanism Test 1",
                  TenpuLevel = "4"
                },
                new AgeResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  YjCd = "Yj888888",
                  WorkingMechanism = "WorkingMechanism Test 2",
                  TenpuLevel = "4"
                },
            };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string> { { "Yj888888", "Item Name Test 1" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForAge(hpId, ages);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.AgeChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("投与年齢"));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo("Item Name Test 1"));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo("ー"));
            Assert.That(errorInfo.ListLevelInfo.Count, Is.EqualTo(1));
            Assert.That(errorInfo.ListLevelInfo.First().Caption, Is.EqualTo("Item Name Test 1"));
            Assert.That(errorInfo.ListLevelInfo.First().BackgroundCode, Is.EqualTo("#ff9999"));
            Assert.That(errorInfo.ListLevelInfo.First().BorderBrushCode, Is.EqualTo("#ff5454"));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("原則禁忌が望ましい"));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("\r\nWorkingMechanism Test 1\r\n\r\n\r\nWorkingMechanism Test 2\r\n\r\n"));
        }

        [Test]
        public void TC_040_ProcessDataForDisease_LevelInfo_IsNotNull()
        {
            int hpId = 1;
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var diseaseInfo = new List<DiseaseResultModel>
            {
                new DiseaseResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "Yj888888",
                  DiseaseType = 1,
                  TenpuLevel = 2
                },
                new DiseaseResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  YjCd = "Yj888888",
                  ByotaiCd = "Byo999999",
                  DiseaseType = 2,
                  TenpuLevel = 2
                },
                new DiseaseResultModel()
                {
                  Id = "3",
                  ItemCd = "UT1236",
                  DiseaseType = 2,
                  YjCd = "Yj888888",
                  ByotaiCd = "Byo999999",
                  TenpuLevel = 2
                },
                new DiseaseResultModel()
                {
                  Id = "4",
                  ItemCd = "UT1236",
                  DiseaseType = 3,
                  YjCd = "Yj9999",
                  ByotaiCd = "Byo999999",
                  TenpuLevel = 3
                },
            };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string> { { "Yj888888", "Item Name Test 1" } };
            commonMedicalCheck._diseaseNameDictionary = new Dictionary<string, string> { { "Byo999999", "Byotai Name Test 1" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDisease(hpId, diseaseInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(4));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DiseaseChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("既往歴"));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo("Item Name Test 1"));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo(""));
            Assert.That(errorInfo.ListLevelInfo.Count, Is.EqualTo(1));
            Assert.That(errorInfo.ListLevelInfo.First().Caption, Is.EqualTo("Item Name Test 1"));
            Assert.That(errorInfo.ListLevelInfo.First().BackgroundCode, Is.EqualTo("#ff9999"));
            Assert.That(errorInfo.ListLevelInfo.First().BorderBrushCode, Is.EqualTo("#ff5454"));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("原則禁忌"));
            Assert.That(result[1].FirstCellContent, Is.EqualTo("家族歴"));
            Assert.That(result[2].FirstCellContent, Is.EqualTo("家族歴"));
            Assert.That(result[3].FirstCellContent, Is.EqualTo("現疾患"));
        }

        [Test]
        public void TC_041_ProcessDataForDosage_LabelChecking_OneMin()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var listDosageError = new List<DosageResultModel>
            {
                new DosageResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "Yj888888",
                  LabelChecking = DosageLabelChecking.OneMin,
                  IsFromUserDefined = true
                },
                new DosageResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  YjCd = "Yj7777777",
                  LabelChecking = DosageLabelChecking.OneMin,
                  IsFromUserDefined = false
                },
            };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string> { { "Yj888888", "Item Name Test 1" } };
            commonMedicalCheck._diseaseNameDictionary = new Dictionary<string, string> { { "Byo999999", "Byotai Name Test 1" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDosage(listDosageError);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DosageChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("ユーザー設定"));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("一回量／最小値"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#0000ff"));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo(""));
        }

        [Test]
        public void TC_042_ProcessDataForDosage_LabelChecking_OneMax()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var listDosageError = new List<DosageResultModel>
            {
                new DosageResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "Yj888888",
                  LabelChecking = DosageLabelChecking.OneMax,
                  IsFromUserDefined = true
                },
                new DosageResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  YjCd = "Yj7777777",
                  LabelChecking = DosageLabelChecking.OneMax,
                  IsFromUserDefined = false
                },
            };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string> { { "Yj888888", "Item Name Test 1" } };
            commonMedicalCheck._diseaseNameDictionary = new Dictionary<string, string> { { "Byo999999", "Byotai Name Test 1" } };
            commonMedicalCheck._usageDosageDictionary = new Dictionary<string, string> { { "Yj7777777", "Usage Name Test" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDosage(listDosageError);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DosageChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("ユーザー設定"));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("一回量／最大値"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#f12c47"));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo("Usage Name Test"));
        }

        [Test]
        public void TC_043_ProcessDataForDosage_LabelChecking_OneLimit()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var listDosageError = new List<DosageResultModel>
            {
                new DosageResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "Yj888888",
                  LabelChecking = DosageLabelChecking.OneLimit,
                  IsFromUserDefined = true
                },
                new DosageResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  YjCd = "Yj7777777",
                  LabelChecking = DosageLabelChecking.OneLimit,
                  IsFromUserDefined = false
                },
            };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string> { { "Yj888888", "Item Name Test 1" } };
            commonMedicalCheck._diseaseNameDictionary = new Dictionary<string, string> { { "Byo999999", "Byotai Name Test 1" } };
            commonMedicalCheck._usageDosageDictionary = new Dictionary<string, string> { { "Yj7777777", "Usage Name Test" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDosage(listDosageError);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DosageChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("ユーザー設定"));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("一回量／上限値"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#f12c47"));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo("Usage Name Test"));
        }

        [Test]
        public void TC_044_ProcessDataForDosage_LabelChecking_DayMin()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var listDosageError = new List<DosageResultModel>
            {
                new DosageResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "Yj888888",
                  LabelChecking = DosageLabelChecking.DayMin,
                  IsFromUserDefined = true
                },
                new DosageResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  YjCd = "Yj7777777",
                  LabelChecking = DosageLabelChecking.DayMin,
                  IsFromUserDefined = false
                },
            };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string> { { "Yj888888", "Item Name Test 1" } };
            commonMedicalCheck._diseaseNameDictionary = new Dictionary<string, string> { { "Byo999999", "Byotai Name Test 1" } };
            commonMedicalCheck._usageDosageDictionary = new Dictionary<string, string> { { "Yj7777777", "Usage Name Test" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDosage(listDosageError);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DosageChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("ユーザー設定"));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("一日量／最小値"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#0000ff"));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo("Usage Name Test"));
        }

        [Test]
        public void TC_045_ProcessDataForDosage_LabelChecking_DayMax()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var listDosageError = new List<DosageResultModel>
            {
                new DosageResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "Yj888888",
                  LabelChecking = DosageLabelChecking.DayMax,
                  IsFromUserDefined = true
                },
                new DosageResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  YjCd = "Yj7777777",
                  LabelChecking = DosageLabelChecking.DayMax,
                  IsFromUserDefined = false
                },
            };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string> { { "Yj888888", "Item Name Test 1" } };
            commonMedicalCheck._diseaseNameDictionary = new Dictionary<string, string> { { "Byo999999", "Byotai Name Test 1" } };
            commonMedicalCheck._usageDosageDictionary = new Dictionary<string, string> { { "Yj7777777", "Usage Name Test" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDosage(listDosageError);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DosageChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("ユーザー設定"));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("一日量／最大値"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#f12c47"));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo("Usage Name Test"));
        }

        [Test]
        public void TC_046_ProcessDataForDosage_LabelChecking_DayMax()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var listDosageError = new List<DosageResultModel>
            {
                new DosageResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "Yj888888",
                  LabelChecking = DosageLabelChecking.DayMax,
                  IsFromUserDefined = true
                },
                new DosageResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  YjCd = "Yj7777777",
                  LabelChecking = DosageLabelChecking.DayMax,
                  IsFromUserDefined = false
                },
            };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string> { { "Yj888888", "Item Name Test 1" } };
            commonMedicalCheck._diseaseNameDictionary = new Dictionary<string, string> { { "Byo999999", "Byotai Name Test 1" } };
            commonMedicalCheck._usageDosageDictionary = new Dictionary<string, string> { { "Yj7777777", "Usage Name Test" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDosage(listDosageError);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DosageChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("ユーザー設定"));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("一日量／最大値"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#f12c47"));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo("Usage Name Test"));
        }

        [Test]
        public void TC_047_ProcessDataForDosage_LabelChecking_DayLimit()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var listDosageError = new List<DosageResultModel>
            {
                new DosageResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "Yj888888",
                  LabelChecking = DosageLabelChecking.DayLimit,
                  IsFromUserDefined = true
                },
                new DosageResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  YjCd = "Yj7777777",
                  LabelChecking = DosageLabelChecking.DayLimit,
                  IsFromUserDefined = false
                },
            };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string> { { "Yj888888", "Item Name Test 1" } };
            commonMedicalCheck._diseaseNameDictionary = new Dictionary<string, string> { { "Byo999999", "Byotai Name Test 1" } };
            commonMedicalCheck._usageDosageDictionary = new Dictionary<string, string> { { "Yj7777777", "Usage Name Test" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDosage(listDosageError);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DosageChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("ユーザー設定"));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("一日量／上限値"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#f12c47"));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo("Usage Name Test"));
        }

        [Test]
        public void TC_048_ProcessDataForDosage_LabelChecking_TermLimit()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var listDosageError = new List<DosageResultModel>
            {
                new DosageResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  YjCd = "Yj888888",
                  LabelChecking = DosageLabelChecking.TermLimit,
                  IsFromUserDefined = true
                },
                new DosageResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  YjCd = "Yj7777777",
                  LabelChecking = DosageLabelChecking.TermLimit,
                  IsFromUserDefined = false
                },
            };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string> { { "Yj888888", "Item Name Test 1" } };
            commonMedicalCheck._diseaseNameDictionary = new Dictionary<string, string> { { "Byo999999", "Byotai Name Test 1" } };
            commonMedicalCheck._usageDosageDictionary = new Dictionary<string, string> { { "Yj7777777", "Usage Name Test" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDosage(listDosageError);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DosageChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("ユーザー設定"));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("期間上限"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#f12c47"));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo("Usage Name Test"));
        }

        [Test]
        public void TC_049_ProcessDataForDuplication_IsIppanCdDuplicated_IsFalse_IsComponentDuplicated_IsFalse()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var listDuplicationError = new List<DuplicationResultModel>
            {
                new DuplicationResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  IsComponentDuplicated = false,
                  IsIppanCdDuplicated = false,
                },
                new DuplicationResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  DuplicatedItemCd = "UT1234",
                  IsComponentDuplicated = false,
                  IsIppanCdDuplicated = false,
                },
            };

            commonMedicalCheck._itemNameByItemCodeDictionary = new Dictionary<string, string> { { "UT1234", "Item Name_By_Code Test 1" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDuplication(hpId, listDuplicationError);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DuplicationChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("同一薬剤"));
            Assert.That(errorInfo.SecondCellContent, Is.EqualTo("ー"));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo("Item Name_By_Code Test 1"));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo("ー"));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("同一薬剤"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo("同一薬剤（）が処方されています。"));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("同一薬剤（Item Name_By_Code Test 1）が処方されています。"));
        }

        [Test]
        public void TC_050_ProcessDataForDuplication_IsComponentDuplicated_IsFalse_IsIppanCdDuplicated_IsTrue()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var listDuplicationError = new List<DuplicationResultModel>
            {
                new DuplicationResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  IsComponentDuplicated = false,
                  IsIppanCdDuplicated = true,
                },
                new DuplicationResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  DuplicatedItemCd = "UT1234",
                  IsComponentDuplicated = false,
                  IsIppanCdDuplicated = true,
                },
            };

            commonMedicalCheck._itemNameByItemCodeDictionary = new Dictionary<string, string> { { "UT1234", "Item Name_By_Code Test 1" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDuplication(hpId, listDuplicationError);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DuplicationChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("同一薬剤"));
            Assert.That(errorInfo.SecondCellContent, Is.EqualTo("ー"));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo("Item Name_By_Code Test 1"));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo(string.Empty));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("同一薬剤"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo("「」と「Item Name_By_Code Test 1」は一般名（）が同じです。"));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("「Item Name_By_Code Test 1」と「」は一般名（）が同じです。"));
        }

        /// <summary>
        /// duplicationError Level = 0
        /// </summary>
        [Test]
        public void TC_051_ProcessDataForDuplication_IsComponentDuplicated_IsTrue_IsIppanCdDuplicated_IsFalse_Level_0()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var listDuplicationError = new List<DuplicationResultModel>
            {
                new DuplicationResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  IsComponentDuplicated = true,
                  IsIppanCdDuplicated = false,
                  Level = 0,
                },
                new DuplicationResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  DuplicatedItemCd = "UT1234",
                  IsComponentDuplicated = true,
                  IsIppanCdDuplicated = false,
                  Level = 0,
                },
            };

            commonMedicalCheck._itemNameByItemCodeDictionary = new Dictionary<string, string> { { "UT1234", "Item Name_By_Code Test 1" } };

            // Act
            var result = commonMedicalCheck.ProcessDataForDuplication(hpId, listDuplicationError);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DuplicationChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("成分重複"));
            Assert.That(errorInfo.SecondCellContent, Is.EqualTo("ー"));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo("Item Name_By_Code Test 1"));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo(string.Empty));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("同一薬剤"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo(string.Empty));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// duplicationError Level = 1
        /// </summary>
        [Test]
        public void TC_052_ProcessDataForDuplication_IsComponentDuplicated_IsTrue_IsIppanCdDuplicated_IsFalse_Level_1()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var listDuplicationError = new List<DuplicationResultModel>
            {
                new DuplicationResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  IsComponentDuplicated = true,
                  IsIppanCdDuplicated = false,
                  Level = 1,
                },
                new DuplicationResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  DuplicatedItemCd = "UT1234",
                  IsComponentDuplicated = true,
                  IsIppanCdDuplicated = false,
                  Level = 1,
                },
            };

            commonMedicalCheck._itemNameByItemCodeDictionary = new Dictionary<string, string> { { "UT1234", "Item Name_By_Code Test 1" } };

            mock.Setup(finder => finder.FindComponentName(hpId, It.IsAny<string>()))
           .Returns((int inputHpId, string stringInput) => "ComponentName1_Mocked_Test");

            // Act
            var result = commonMedicalCheck.ProcessDataForDuplication(hpId, listDuplicationError);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DuplicationChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("成分重複"));
            Assert.That(errorInfo.SecondCellContent, Is.EqualTo("ー"));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo("Item Name_By_Code Test 1"));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo(string.Empty));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("同一成分"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo("※「」 と「Item Name_By_Code Test 1」 は成分（ComponentName1_Mocked_Test）が重複しています。\r\n\r\n"));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("※「Item Name_By_Code Test 1」 と「」 は成分（ComponentName1_Mocked_Test）が重複しています。\r\n\r\n"));
        }

        /// <summary>
        /// duplicationError Level = 2
        /// </summary>
        [Test]
        public void TC_053_ProcessDataForDuplication_IsComponentDuplicated_IsTrue_IsIppanCdDuplicated_IsFalse_Level_2()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var listDuplicationError = new List<DuplicationResultModel>
            {
                new DuplicationResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  IsComponentDuplicated = true,
                  IsIppanCdDuplicated = false,
                  Level = 2,
                  SeibunCd = "S1234",
                  AllergySeibunCd = "A1234",
                },
                new DuplicationResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  DuplicatedItemCd = "UT1234",
                  IsComponentDuplicated = true,
                  IsIppanCdDuplicated = false,
                  Level = 2,
                  SeibunCd = "S1234",
                  AllergySeibunCd = "A1234",
                },
            };

            commonMedicalCheck._itemNameByItemCodeDictionary = new Dictionary<string, string> { { "UT1234", "Item Name_By_Code Test 1" } };

            mock.Setup(finder => finder.FindComponentName(hpId, "S1234"))
           .Returns((int inputHpId, string stringInput) => "ComponentName2_Mocked_Test");

            mock.Setup(finder => finder.FindComponentName(hpId, "A1234"))
           .Returns((int inputHpId, string stringInput) => "AllergyComponentName2_Mocked_Test");
            // Act
            var result = commonMedicalCheck.ProcessDataForDuplication(hpId, listDuplicationError);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DuplicationChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("成分重複"));
            Assert.That(errorInfo.SecondCellContent, Is.EqualTo("ー"));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo("Item Name_By_Code Test 1"));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo(string.Empty));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("同一成分"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(result.Last().ListLevelInfo.First().Comment, Is.EqualTo("※「」 の成分（ComponentName2_Mocked_Test）と「Item Name_By_Code Test 1」 の成分（AllergyComponentName2_Mocked_Test）は活性対成分（ComponentName2_Mocked_Test）が同じです。\r\n\r\n"));
            Assert.That(errorInfo.ListLevelInfo.First().Comment, Is.EqualTo("※「Item Name_By_Code Test 1」 の成分（ComponentName2_Mocked_Test）と「」 の成分（AllergyComponentName2_Mocked_Test）は活性対成分（ComponentName2_Mocked_Test）が同じです。\r\n\r\n"));
        }

        /// <summary>
        /// duplicationError Level = 3
        /// </summary>
        [Test]
        public void TC_054_ProcessDataForDuplication_IsComponentDuplicated_IsTrue_IsIppanCdDuplicated_IsFalse_Level_3()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var listDuplicationError = new List<DuplicationResultModel>
            {
                new DuplicationResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  IsComponentDuplicated = true,
                  IsIppanCdDuplicated = false,
                  Level = 3,
                  SeibunCd = "S1234",
                  AllergySeibunCd = "A1234",
                },
                new DuplicationResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  DuplicatedItemCd = "UT1234",
                  IsComponentDuplicated = true,
                  IsIppanCdDuplicated = false,
                  Level = 3,
                  SeibunCd = "S1234",
                  AllergySeibunCd = "A1234",
                  Tag = "T1234",
                },
            };

            commonMedicalCheck._itemNameByItemCodeDictionary = new Dictionary<string, string> { { "UT1234", "Item Name_By_Code Test 1" } };

            mock.Setup(finder => finder.FindComponentName(hpId, "S1234"))
           .Returns((int inputHpId, string stringInput) => "ComponentName2_Mocked_Test");

            mock.Setup(finder => finder.FindComponentName(hpId, "A1234"))
           .Returns((int inputHpId, string stringInput) => "AllergyComponentName2_Mocked_Test");

            mock.Setup(finder => finder.FindAnalogueName(hpId, "T1234"))
           .Returns((int inputHpId, string stringInput) => "AnalogueName_Mocked_Test");

            // Act
            var result = commonMedicalCheck.ProcessDataForDuplication(hpId, listDuplicationError);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DuplicationChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("成分重複"));
            Assert.That(errorInfo.SecondCellContent, Is.EqualTo("ー"));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo("Item Name_By_Code Test 1"));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo(string.Empty));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("類似成分"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(result.Last().ListLevelInfo.First().Comment,
                Is.EqualTo("※「」の成分（ComponentName2_Mocked_Test）と「Item Name_By_Code Test 1」 の成分（AllergyComponentName2_Mocked_Test）は類似成分（AnalogueName_Mocked_Test）です。\r\n\r\n"));
            Assert.That(result.First().ListLevelInfo.First().Comment,
                Is.EqualTo("※「Item Name_By_Code Test 1」の成分（ComponentName2_Mocked_Test）と「」 の成分（AllergyComponentName2_Mocked_Test）は類似成分（）です。\r\n\r\n"));
        }

        /// <summary>
        /// duplicationError Level = 4
        /// </summary>
        [Test]
        public void TC_055_ProcessDataForDuplication_IsComponentDuplicated_IsTrue_IsIppanCdDuplicated_IsFalse_Level_3()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var listDuplicationError = new List<DuplicationResultModel>
            {
                new DuplicationResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  IsComponentDuplicated = true,
                  IsIppanCdDuplicated = false,
                  Level = 4,
                  SeibunCd = "S1234",
                  AllergySeibunCd = "A1234",
                },
                new DuplicationResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  DuplicatedItemCd = "UT1234",
                  IsComponentDuplicated = true,
                  IsIppanCdDuplicated = false,
                  Level = 4,
                  SeibunCd = "S1234",
                  AllergySeibunCd = "A1234",
                  Tag = "T1234",
                },
            };

            commonMedicalCheck._itemNameByItemCodeDictionary = new Dictionary<string, string> { { "UT1234", "Item Name_By_Code Test 1" } };

            mock.Setup(finder => finder.FindComponentName(hpId, "S1234"))
           .Returns((int inputHpId, string stringInput) => "ComponentName2_Mocked_Test");

            mock.Setup(finder => finder.FindComponentName(hpId, "A1234"))
           .Returns((int inputHpId, string stringInput) => "AllergyComponentName2_Mocked_Test");

            mock.Setup(finder => finder.FindAnalogueName(hpId, "T1234"))
           .Returns((int inputHpId, string stringInput) => "AnalogueName_Mocked_Test");

            mock.Setup(finder => finder.FindClassName(hpId, "T1234"))
           .Returns((int inputHpId, string stringInput) => "ClassName_Mocked_Test");

            // Act
            var result = commonMedicalCheck.ProcessDataForDuplication(hpId, listDuplicationError);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.DuplicationChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("成分重複"));
            Assert.That(errorInfo.SecondCellContent, Is.EqualTo("ー"));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo("Item Name_By_Code Test 1"));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo(string.Empty));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("同一系統"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(result.Last().ListLevelInfo.First().Comment,
                Is.EqualTo("※「」 と「Item Name_By_Code Test 1」 は同じ系統（ClassName_Mocked_Test）の成分を含みます。\r\n\r\n"));
            Assert.That(result.First().ListLevelInfo.First().Comment,
                Is.EqualTo("※「Item Name_By_Code Test 1」 と「」 は同じ系統（）の成分を含みます。\r\n\r\n"));
        }

        [Test]
        public void TC_056_RemoveDuplicatedErrorInfo()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var originList = new List<KinkiResultModel>
            {
                new KinkiResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  AYjCd = "AYj1234",
                  BYjCd = "BYj1234",
                  CommentCode = "C1234",
                  SayokijyoCode = "Sa1234",
                  Kyodo = "Ky1234",
                  IsNeedToReplace = true,
                  IndexWord = "A"
                },
                new KinkiResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  AYjCd = "AYj1234",
                  BYjCd = "BYj1234",
                  CommentCode = "C1234",
                  SayokijyoCode = "Sa1234",
                  Kyodo = "Ky1234",
                  IsNeedToReplace = true,
                  IndexWord = "A"
                },
                new KinkiResultModel()
                {
                  Id = "3",
                  ItemCd = "UT1235",
                  AYjCd = "AYj1235",
                  BYjCd = "BYj1234",
                  CommentCode = "C1234",
                  SayokijyoCode = "Sa1234",
                  Kyodo = "Ky1234",
                  IsNeedToReplace = true,
                  IndexWord = "A"
                },
                new KinkiResultModel()
                {
                  Id = "3",
                  ItemCd = "UT1235",
                  AYjCd = "AYj1234",
                  BYjCd = "BYj1235",
                  CommentCode = "C1234",
                  SayokijyoCode = "Sa1234",
                  Kyodo = "Ky1234",
                  IsNeedToReplace = true,
                  IndexWord = "A"
                },
                new KinkiResultModel()
                {
                  Id = "3",
                  ItemCd = "UT1235",
                  AYjCd = "AYj1234",
                  BYjCd = "BYj1234",
                  CommentCode = "C1235",
                  SayokijyoCode = "Sa1234",
                  Kyodo = "Ky1234",
                  IsNeedToReplace = true,
                  IndexWord = "A"
                },
                new KinkiResultModel()
                {
                  Id = "3",
                  ItemCd = "UT1235",
                  AYjCd = "AYj1234",
                  BYjCd = "BYj1234",
                  CommentCode = "C1234",
                  SayokijyoCode = "Sa1235",
                  Kyodo = "Ky1234",
                  IsNeedToReplace = true,
                  IndexWord = "A"
                },
                new KinkiResultModel()
                {
                  Id = "3",
                  ItemCd = "UT1235",
                  AYjCd = "AYj1234",
                  BYjCd = "BYj1234",
                  CommentCode = "C1234",
                  SayokijyoCode = "Sa1234",
                  Kyodo = "Ky1235",
                  IsNeedToReplace = true,
                  IndexWord = "A"
                },
                new KinkiResultModel()
                {
                  Id = "3",
                  ItemCd = "UT1235",
                  AYjCd = "AYj1234",
                  BYjCd = "BYj1234",
                  CommentCode = "C1234",
                  SayokijyoCode = "Sa1234",
                  Kyodo = "Ky1234",
                  IsNeedToReplace = false,
                  IndexWord = "A"
                },
                new KinkiResultModel()
                {
                  Id = "3",
                  ItemCd = "UT1235",
                  AYjCd = "AYj1234",
                  BYjCd = "BYj1234",
                  CommentCode = "C1234",
                  SayokijyoCode = "Sa1234",
                  Kyodo = "Ky1234",
                  IsNeedToReplace = true,
                  IndexWord = "B"
                },
            };

            // Act
            var result = commonMedicalCheck.RemoveDuplicatedErrorInfo(originList);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(8));
        }

        /// <summary>
        /// CheckingType = KinkiSupplement
        /// </summary>
        [Test]
        public void TC_057_ProcessDataForKinki_KinkiSupplement()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var kinkiErrorInfo = new List<KinkiResultModel>
            {
                new KinkiResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  SeibunCd = "S1234",
                  AYjCd = "A1234",
                  BYjCd = "B1234",
                  KinkiItemCd = "K1234",
                  Kyodo = "1"
                },
                new KinkiResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  SeibunCd = "S1235",
                  AYjCd = "A1235",
                  BYjCd = "B1235",
                  KinkiItemCd = "K1235",
                  Kyodo = "1"
                },
            };

            commonMedicalCheck._suppleItemNameDictionary = new Dictionary<string, string> {
                                                                                            { "S1234", "SeibunName_Mocked_Test_1" },
                                                                                            { "S1235", "SeibunName_Mocked_Test_2" },
                                                                                          };

            mock.Setup(finder => finder.FindClassName(hpId, "T1234"))
           .Returns((int inputHpId, string stringInput) => "ClassName_Mocked_Test");

            var checkingType = RealtimeCheckerType.KinkiSupplement;
            // Act
            var result = commonMedicalCheck.ProcessDataForKinki(checkingType, kinkiErrorInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.KinkiChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("相互作用(サプリ)"));
            Assert.That(errorInfo.SecondCellContent, Is.EqualTo(""));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo(string.Empty));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo("（SeibunName_Mocked_Test_1）"));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("禁忌"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(errorInfo.ListLevelInfo.First().Caption, Is.EqualTo("Ａ.  × Ｂ. （SeibunName_Mocked_Test_1）"));
            Assert.That(result.Last().ListLevelInfo.First().Comment,
                Is.EqualTo("\r\n※\r\n\r\n"));
            Assert.That(result.First().ListLevelInfo.First().Comment,
                Is.EqualTo("\r\n※\r\n\r\n"));
        }

        /// <summary>
        /// CheckingType = KinkiOTC
        /// </summary>
        [Test]
        public void TC_058_ProcessDataForKinki_KinkiOTC()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var kinkiErrorInfo = new List<KinkiResultModel>
            {
                new KinkiResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  SeibunCd = "S1234",
                  AYjCd = "A1234",
                  BYjCd = "B1234",
                  KinkiItemCd = "K1234",
                  Kyodo = "1"
                },
                new KinkiResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  SeibunCd = "S1235",
                  AYjCd = "A1235",
                  BYjCd = "B1235",
                  KinkiItemCd = "K1235",
                  Kyodo = "1"
                },
            };

            commonMedicalCheck._suppleItemNameDictionary = new Dictionary<string, string> {
                                                                                            { "S1234", "SeibunName_Mocked_Test_1" },
                                                                                            { "S1235", "SeibunName_Mocked_Test_2" },
                                                                                          };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string>
            {
                {"B1235", "BName_Mocked_Test_1"},
            };

            commonMedicalCheck._oTCItemNameDictionary = new Dictionary<string, string>
            {
                {"B1235", "BName_Mocked_Test_2" }
            };

            var checkingType = RealtimeCheckerType.KinkiOTC;
            // Act
            var result = commonMedicalCheck.ProcessDataForKinki(checkingType, kinkiErrorInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.KinkiChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("相互作用(OTC)"));
            Assert.That(errorInfo.SecondCellContent, Is.EqualTo(""));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo(string.Empty));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo(string.Empty));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("禁忌"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(errorInfo.ListLevelInfo.First().Caption, Is.EqualTo("Ａ.  × Ｂ. "));
            Assert.That(result.Last().ListLevelInfo.First().Comment,
                Is.EqualTo("\r\n※\r\n\r\n"));
            Assert.That(result.First().ListLevelInfo.First().Comment,
                Is.EqualTo("\r\n※\r\n\r\n"));
        }

        /// <summary>
        /// CheckingType = KinkiOTC
        /// </summary>
        [Test]
        public void TC_059_ProcessDataForKinki_KinkiOTC()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var kinkiErrorInfo = new List<KinkiResultModel>
            {
                new KinkiResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  SeibunCd = "S1234",
                  AYjCd = "A1234",
                  BYjCd = "B1234",
                  KinkiItemCd = "K1234",
                  Kyodo = "1",
                  CommentCode = "C1234",
                  SayokijyoCode = "Sa1234"
                },
                new KinkiResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  SeibunCd = "S1235",
                  AYjCd = "A1235",
                  BYjCd = "B1235",
                  KinkiItemCd = "K1235",
                  Kyodo = "1",
                  CommentCode = "C1235",
                  SayokijyoCode = "Sa1235"
                },
            };

            commonMedicalCheck._suppleItemNameDictionary = new Dictionary<string, string> {
                                                                                            { "S1234", "SeibunName_Mocked_Test_1" },
                                                                                            { "S1235", "SeibunName_Mocked_Test_2" },
                                                                                          };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string>
            {
                {"B1235", "BName_Mocked_Test_1"},
            };

            commonMedicalCheck._oTCItemNameDictionary = new Dictionary<string, string>
            {
                {"B1235", "BName_Mocked_Test_2" }
            };

            commonMedicalCheck._kinkiCommentDictionary = new Dictionary<string, string>
            {
                {"C1234", "CommentContent_Mocked_Test_1" },
                {"C1235", "CommentContent_Mocked_Test_2" },
            };

            commonMedicalCheck._kijyoCommentDictionary = new Dictionary<string, string>
            {
                {"Sa1234", "SayokijyoContent_Test_1" },
                {"Sa1235", "SayokijyoContent_Test_2" },
            };

            mock.Setup(finder => finder.FindClassName(hpId, "T1234"))
           .Returns((int inputHpId, string stringInput) => "ClassName_Mocked_Test");

            var checkingType = RealtimeCheckerType.KinkiTain;
            // Act
            var result = commonMedicalCheck.ProcessDataForKinki(checkingType, kinkiErrorInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.KinkiChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("相互作用(他院)"));
            Assert.That(errorInfo.SecondCellContent, Is.EqualTo(""));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo(string.Empty));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo(""));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("禁忌"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(errorInfo.ListLevelInfo.First().Caption, Is.EqualTo("Ａ.  × Ｂ. "));
            Assert.That(result.Last().ListLevelInfo.First().Comment,
                Is.EqualTo("CommentContent_Mocked_Test_2\r\n※SayokijyoContent_Test_2\r\n\r\n"));
            Assert.That(result.First().ListLevelInfo.First().Comment,
                Is.EqualTo("CommentContent_Mocked_Test_1\r\n※SayokijyoContent_Test_1\r\n\r\n"));
        }

        /// <summary>
        /// CheckingType = Default
        /// </summary>
        [Test]
        public void TC_060_ProcessDataForKinki_CheckingType_Default()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var kinkiErrorInfo = new List<KinkiResultModel>
            {
                new KinkiResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  SeibunCd = "S1234",
                  AYjCd = "A1234",
                  BYjCd = "B1234",
                  KinkiItemCd = "K1234",
                  Kyodo = "1",
                  CommentCode = "C1234",
                  SayokijyoCode = "Sa1234"
                },
                new KinkiResultModel()
                {
                  Id = "2",
                  ItemCd = "UT1235",
                  SeibunCd = "S1235",
                  AYjCd = "A1235",
                  BYjCd = "B1235",
                  KinkiItemCd = "K1235",
                  Kyodo = "1",
                  CommentCode = "C1235",
                  SayokijyoCode = "Sa1235"
                },
            };

            commonMedicalCheck._suppleItemNameDictionary = new Dictionary<string, string> {
                                                                                            { "S1234", "SeibunName_Mocked_Test_1" },
                                                                                            { "S1235", "SeibunName_Mocked_Test_2" },
                                                                                          };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string>
            {
                {"B1235", "BName_Mocked_Test_1"},
            };

            commonMedicalCheck._oTCItemNameDictionary = new Dictionary<string, string>
            {
                {"B1235", "BName_Mocked_Test_2" }
            };

            commonMedicalCheck._kinkiCommentDictionary = new Dictionary<string, string>
            {
                {"C1234", "CommentContent_Mocked_Test_1" },
                {"C1235", "CommentContent_Mocked_Test_2" },
            };

            commonMedicalCheck._kijyoCommentDictionary = new Dictionary<string, string>
            {
                {"Sa1234", "SayokijyoContent_Test_1" },
                {"Sa1235", "SayokijyoContent_Test_2" },
            };

            mock.Setup(finder => finder.FindClassName(hpId, "T1234"))
           .Returns((int inputHpId, string stringInput) => "ClassName_Mocked_Test");

            var checkingType = RealtimeCheckerType.KinkiUser;
            // Act
            var result = commonMedicalCheck.ProcessDataForKinki(checkingType, kinkiErrorInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.KinkiChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("相互作用"));
            Assert.That(errorInfo.SecondCellContent, Is.EqualTo(""));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo(string.Empty));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo(""));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("禁忌"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(errorInfo.ListLevelInfo.First().Caption, Is.EqualTo("Ａ.  × Ｂ. "));
            Assert.That(result.Last().ListLevelInfo.First().Comment,
                Is.EqualTo("CommentContent_Mocked_Test_2\r\n※SayokijyoContent_Test_2\r\n\r\n"));
            Assert.That(result.First().ListLevelInfo.First().Comment,
                Is.EqualTo("CommentContent_Mocked_Test_1\r\n※SayokijyoContent_Test_1\r\n\r\n"));
        }

        [Test]
        public void TC_061_ProcessDataForKinki_CheckingType_IndexWord_Equal_SeibunName()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var kinkiErrorInfo = new List<KinkiResultModel>
            {
                new KinkiResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  SeibunCd = "S1234",
                  AYjCd = "A1234",
                  BYjCd = "B1234",
                  KinkiItemCd = "K1234",
                  Kyodo = "1",
                  CommentCode = "C1234",
                  SayokijyoCode = "Sa1234",
                  IndexWord = "SeibunName_Mocked_Test_1",
                },
                new KinkiResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1235",
                  SeibunCd = "S1235",
                  AYjCd = "A1235",
                  BYjCd = "B1235",
                  KinkiItemCd = "K1235",
                  Kyodo = "1",
                  CommentCode = "C1235",
                  SayokijyoCode = "Sa1235",
                  IndexWord = "SeibunName_Mocked_Test_2",
                },
            };

            commonMedicalCheck._suppleItemNameDictionary = new Dictionary<string, string> {
                                                                                            { "S1234", "SeibunName_Mocked_Test_1" },
                                                                                            { "S1235", "SeibunName_Mocked_Test_2" },
                                                                                          };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string>
            {
                {"B1235", "BName_Mocked_Test_1"},
            };

            commonMedicalCheck._oTCItemNameDictionary = new Dictionary<string, string>
            {
                {"B1235", "BName_Mocked_Test_2" }
            };

            commonMedicalCheck._kinkiCommentDictionary = new Dictionary<string, string>
            {
                {"C1234", "CommentContent_Mocked_Test_1" },
                {"C1235", "CommentContent_Mocked_Test_2" },
            };

            commonMedicalCheck._kijyoCommentDictionary = new Dictionary<string, string>
            {
                {"Sa1234", "SayokijyoContent_Test_1" },
                {"Sa1235", "SayokijyoContent_Test_2" },
            };

            mock.Setup(finder => finder.FindClassName(hpId, "T1234"))
           .Returns((int inputHpId, string stringInput) => "ClassName_Mocked_Test");

            var checkingType = RealtimeCheckerType.KinkiUser;
            // Act
            var result = commonMedicalCheck.ProcessDataForKinki(checkingType, kinkiErrorInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.KinkiChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("相互作用"));
            Assert.That(errorInfo.SecondCellContent, Is.EqualTo(""));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo(string.Empty));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo(""));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("禁忌"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(errorInfo.ListLevelInfo.First().Caption, Is.EqualTo("Ａ.  × Ｂ. "));
            Assert.That(result.Last().ListLevelInfo.First().Comment,
                Is.EqualTo("CommentContent_Mocked_Test_2\r\n※SayokijyoContent_Test_2\r\n\r\n"));
            Assert.That(result.First().ListLevelInfo.First().Comment,
                Is.EqualTo("CommentContent_Mocked_Test_1\r\n※SayokijyoContent_Test_1\r\n\r\n"));
        }

        /// <summary>
        /// CheckingType = KinkiOTC
        /// </summary>
        [Test]
        public void TC_062_ProcessDataForKinki_CheckingType_IsNeedToReplace_True()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var kinkiErrorInfo = new List<KinkiResultModel>
            {
                new KinkiResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  SeibunCd = "S1234",
                  AYjCd = "A1234",
                  BYjCd = "B1234",
                  KinkiItemCd = "K1234",
                  Kyodo = "1",
                  CommentCode = "C1234",
                  SayokijyoCode = "Sa1234",
                  IndexWord = "SeibunName_Mocked_Test_1",
                  IsNeedToReplace = true
                },
                new KinkiResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1235",
                  SeibunCd = "S1235",
                  AYjCd = "A1235",
                  BYjCd = "B1235",
                  KinkiItemCd = "K1235",
                  Kyodo = "1",
                  CommentCode = "C1235",
                  SayokijyoCode = "Sa1235",
                  IndexWord = "SeibunName_Mocked_Test_2",
                  IsNeedToReplace = true
                },
            };

            commonMedicalCheck._suppleItemNameDictionary = new Dictionary<string, string> {
                                                                                            { "S1234", "SeibunName_Mocked_Test_1" },
                                                                                            { "S1235", "SeibunName_Mocked_Test_2" },
                                                                                          };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string>
            {
                {"B1235", "BName_Mocked_Test_1"},
            };

            commonMedicalCheck._oTCItemNameDictionary = new Dictionary<string, string>
            {
                {"B1235", "BName_Mocked_Test_2" }
            };

            commonMedicalCheck._kinkiCommentDictionary = new Dictionary<string, string>
            {
                {"C1234", "CommentContent_Mocked_Test_1" },
                {"C1235", "CommentContent_Mocked_Test_2" },
            };

            commonMedicalCheck._kijyoCommentDictionary = new Dictionary<string, string>
            {
                {"Sa1234", "SayokijyoContent_Test_1" },
                {"Sa1235", "SayokijyoContent_Test_2" },
            };

            commonMedicalCheck._oTCComponentInfoDictionary = new Dictionary<string, string>
            {
                {"S1234",  "OtcComponentInfo_Mocked_Test_1"},
                {"S1235",  "OtcComponentInfo_Mocked_Test_2"},
            };
            mock.Setup(finder => finder.FindClassName(hpId, "T1234"))
           .Returns((int inputHpId, string stringInput) => "ClassName_Mocked_Test");

            var checkingType = RealtimeCheckerType.KinkiOTC;
            // Act
            var result = commonMedicalCheck.ProcessDataForKinki(checkingType, kinkiErrorInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(2));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.KinkiChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("相互作用(OTC)"));
            Assert.That(errorInfo.SecondCellContent, Is.EqualTo(""));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo(string.Empty));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo(""));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("禁忌"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(errorInfo.ListLevelInfo.First().Caption, Is.EqualTo("Ａ.  × Ｂ. "));
            Assert.That(result.Last().ListLevelInfo.First().Comment,
                Is.EqualTo("CommentContent_Mocked_Test_2\r\n※SayokijyoContent_Test_2\r\n\r\n"));
            Assert.That(result.First().ListLevelInfo.First().Comment,
                Is.EqualTo("CommentContent_Mocked_Test_1\r\n※SayokijyoContent_Test_1\r\n\r\n"));
        }

        [Test]
        public void TC_063_ProcessDataForKinki_CheckingType_IsNeedToReplace_Test_CommentContent()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 1;

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var kinkiErrorInfo = new List<KinkiResultModel>
            {
                new KinkiResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  SeibunCd = "S1234",
                  AYjCd = "A1234",
                  BYjCd = "B1234",
                  KinkiItemCd = "K1234",
                  Kyodo = "1",
                  CommentCode = "C1234",
                  SayokijyoCode = "Sa1234",
                  IndexWord = "SeibunName_Mocked_Test_1",
                  IsNeedToReplace = true,
                },
                new KinkiResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1235",
                  SeibunCd = "S1235",
                  AYjCd = "A1234",
                  BYjCd = "B1234",
                  KinkiItemCd = "K1235",
                  Kyodo = "1",
                  CommentCode = "C1234",
                  SayokijyoCode = "Sa1234",
                  IndexWord = "SeibunName_Mocked_Test_2",
                  IsNeedToReplace = true
                },
                new KinkiResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1236",
                  SeibunCd = "S1235",
                  AYjCd = "A1236",
                  BYjCd = "B1236",
                  KinkiItemCd = "K1236",
                  Kyodo = "3",
                  CommentCode = "C1236",
                  SayokijyoCode = "Sa1236",
                  IndexWord = "SeibunName_Mocked_Test_3",
                  IsNeedToReplace = true
                },
            };

            commonMedicalCheck._suppleItemNameDictionary = new Dictionary<string, string> {
                                                                                            { "S1234", "SeibunName_Mocked_Test_1" },
                                                                                            { "S1235", "SeibunName_Mocked_Test_2" },
                                                                                            { "S1236", "SeibunName_Mocked_Test_2" },
                                                                                          };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string>
            {
                {"B1235", "BName_Mocked_Test_1"},
                {"B1236", "BName_Mocked_Test_2"},
            };

            commonMedicalCheck._oTCItemNameDictionary = new Dictionary<string, string>
            {
                {"B1235", "BName_Mocked_Test_2" },
                {"B1236", "BName_Mocked_Test_2" }
            };

            commonMedicalCheck._kinkiCommentDictionary = new Dictionary<string, string>
            {
                {"C1234", "CommentContent_Mocked_Test_1" },
                {"C1235", "CommentContent_Mocked_Test_2" },
                {"C1236", "CommentContent_Mocked_Test_2" },
            };

            commonMedicalCheck._kijyoCommentDictionary = new Dictionary<string, string>
            {
                {"Sa1234", "SayokijyoContent_Test_1" },
                {"Sa1235", "SayokijyoContent_Test_2" },
            };

            commonMedicalCheck._supplementComponentInfoDictionary = new Dictionary<string, string>
            {
                {"S1234",  "OtcComponentInfo_Mocked_Test_1"},
                {"S1235",  "OtcComponentInfo_Mocked_Test_2"},
                {"S1236",  "OtcComponentInfo_Mocked_Test_2"},
            };
            mock.Setup(finder => finder.FindClassName(hpId, "T1234"))
           .Returns((int inputHpId, string stringInput) => "ClassName_Mocked_Test");

            var checkingType = RealtimeCheckerType.KinkiSupplement;
            // Act
            var result = commonMedicalCheck.ProcessDataForKinki(checkingType, kinkiErrorInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(3));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.KinkiChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("相互作用(サプリ)"));
            Assert.That(errorInfo.SecondCellContent, Is.EqualTo(""));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo(string.Empty));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo("SeibunName_Mocked_Test_1"));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("禁忌"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(errorInfo.ListLevelInfo.First().Caption, Is.EqualTo("Ａ.  × Ｂ. SeibunName_Mocked_Test_1"));
            Assert.That(result.Last().ListLevelInfo.First().Comment,
                Is.EqualTo("CommentContent_Mocked_Test_2\r\n※\r\n\r\n"));
            Assert.That(result.First().ListLevelInfo.First().Comment,
                Is.EqualTo("CommentContent_Mocked_Test_1\r\n※SayokijyoContent_Test_1\r\n※SayokijyoContent_Test_1\r\n\r\n"));
            Assert.That(result[1].ListLevelInfo.First().Comment,
                Is.EqualTo("CommentContent_Mocked_Test_1\r\n※SayokijyoContent_Test_1\r\n※SayokijyoContent_Test_1\r\n\r\n")
                );
        }

        [Test]
        public void TC_064_ProcessDataForKinki_CheckingType_Test_SayokijyoContent()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var kinkiErrorInfo = new List<KinkiResultModel>
            {
                new KinkiResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1234",
                  SeibunCd = "S1234",
                  AYjCd = "A1234",
                  BYjCd = "B1234",
                  KinkiItemCd = "K1234",
                  Kyodo = "1",
                  CommentCode = "C1234",
                  SayokijyoCode = "Sa1234",
                  IndexWord = "SeibunName_Mocked_Test_1",
                  IsNeedToReplace = true,
                },
                new KinkiResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1235",
                  SeibunCd = "S1235",
                  AYjCd = "A1234",
                  BYjCd = "B1234",
                  KinkiItemCd = "K1235",
                  Kyodo = "1",
                  CommentCode = "C1235",
                  SayokijyoCode = "Sa1234",
                  IndexWord = "SeibunName_Mocked_Test_2",
                  IsNeedToReplace = true
                },
                new KinkiResultModel()
                {
                  Id = "1",
                  ItemCd = "UT1236",
                  SeibunCd = "S1235",
                  AYjCd = "A1236",
                  BYjCd = "B1236",
                  KinkiItemCd = "K1236",
                  Kyodo = "3",
                  CommentCode = "C1236",
                  SayokijyoCode = "Sa1236",
                  IndexWord = "SeibunName_Mocked_Test_3",
                  IsNeedToReplace = true
                },
            };

            commonMedicalCheck._itemNameDictionary = new Dictionary<string, string>
            {
                {"A1234", "ItemAName_Mocked_Test_1"},
                {"A1235", "ItemAName_Mocked_Test_2"},
            };

            commonMedicalCheck._oTCItemNameDictionary = new Dictionary<string, string>
            {
                {"B1235", "BName_Mocked_Test_2" },
                {"B1236", "BName_Mocked_Test_2" }
            };

            commonMedicalCheck._kinkiCommentDictionary = new Dictionary<string, string>
            {
                {"C1234", "CommentContent_Mocked_Test_1" },
                {"C1235", "CommentContent_Mocked_Test_2" },
                {"C1236", "CommentContent_Mocked_Test_2" },
            };

            commonMedicalCheck._kijyoCommentDictionary = new Dictionary<string, string>
            {
                {"Sa1234", "SayokijyoContent_Test_1" },
                {"Sa1235", "SayokijyoContent_Test_2" },
            };

            commonMedicalCheck._supplementComponentInfoDictionary = new Dictionary<string, string>
            {
                {"S1234",  "OtcComponentInfo_Mocked_Test_1"},
                {"S1235",  "OtcComponentInfo_Mocked_Test_2"},
                {"S1236",  "OtcComponentInfo_Mocked_Test_2"},
            };

            var checkingType = RealtimeCheckerType.KinkiSupplement;
            // Act
            var result = commonMedicalCheck.ProcessDataForKinki(checkingType, kinkiErrorInfo);

            // Assert
            Assert.NotNull(result);
            Assert.That(result.Count, Is.EqualTo(3));

            var errorInfo = result.First();
            Assert.That(errorInfo.ErrorType, Is.EqualTo(CommonCheckerType.KinkiChecker));
            Assert.That(errorInfo.Id, Is.EqualTo("1"));
            Assert.That(errorInfo.FirstCellContent, Is.EqualTo("相互作用(サプリ)"));
            Assert.That(errorInfo.SecondCellContent, Is.EqualTo(""));
            Assert.That(errorInfo.ThridCellContent, Is.EqualTo("ItemAName_Mocked_Test_1"));
            Assert.That(errorInfo.FourthCellContent, Is.EqualTo("SeibunName_Mocked_Test_1（）"));
            Assert.That(errorInfo.ListLevelInfo.First().Title, Is.EqualTo("禁忌"));
            Assert.That(errorInfo.HighlightColorCode, Is.EqualTo("#000000"));
            Assert.That(errorInfo.ListLevelInfo.First().Caption, Is.EqualTo("Ａ. ItemAName_Mocked_Test_1 × Ｂ. SeibunName_Mocked_Test_1（）"));
            Assert.That(result.Last().ListLevelInfo.First().Comment,
                Is.EqualTo("CommentContent_Mocked_Test_2\r\n※\r\n\r\n"));
            Assert.That(result.First().ListLevelInfo.First().Comment,
                Is.EqualTo("CommentContent_Mocked_Test_1\r\nCommentContent_Mocked_Test_2\r\n※SayokijyoContent_Test_1\r\n\r\n"));
            Assert.That(result[1].ListLevelInfo.First().Comment,
                Is.EqualTo("CommentContent_Mocked_Test_1\r\nCommentContent_Mocked_Test_2\r\n※SayokijyoContent_Test_1\r\n\r\n")
                );
        }

        [Test]
        public void TC_065_ConvertToFamilyModel()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            //Setup
            var familyRekis = new List<FamilyRekiItem>()
            {
                new FamilyRekiItem(1, "BYOMEICD", "Sick", "So Sleepy", 2, true),
            };


            var familyItem = new FamilyItem(
                                        familyId: 111,
                                        ptId: 1234,
                                        zokugaraCd: "zokugara",
                                        familyPtId: 1231,
                                        name: "smartkarte",
                                        kanaName: "A",
                                        sex: 1,
                                        birthday: 19900101,
                                        isDead: 1,
                                        isSeparated: 1,
                                        biko: "B",
                                        sortNo: 2,
                                        isDeleted: true,
                                        familyRekis,
                                        isRevertItem: true
                );

            //Act
            var result = commonMedicalCheck.ConvertToFamilyModel(familyItem);

            var expectedFamilyRekis = new List<PtFamilyRekiModel>()
            {
                new PtFamilyRekiModel (1, "BYOMEICD", "Sick", "So Sleepy", 2, true),
            };

            var expectedResult = new FamilyModel(111, 1234, 0, "zokugara", 1231, 0, "smartkarte", "A", 1, 19900101, 0, 1, 1, "B", 2, expectedFamilyRekis, string.Empty);
            //Assert
            Assert.That(expectedResult.FamilyId, Is.EqualTo(result.FamilyId));
            Assert.That(expectedResult.PtId, Is.EqualTo(result.PtId));
            Assert.That(expectedResult.ZokugaraCd, Is.EqualTo(result.ZokugaraCd));
            Assert.That(expectedResult.FamilyPtId, Is.EqualTo(result.FamilyPtId));
            Assert.That(expectedResult.SeqNo, Is.EqualTo(result.SeqNo));
            Assert.That(expectedResult.FamilyPtNum, Is.EqualTo(result.FamilyPtNum));
            Assert.That(expectedResult.Name, Is.EqualTo(result.Name));
            Assert.That(expectedResult.KanaName, Is.EqualTo(result.KanaName));
            Assert.That(expectedResult.Sex, Is.EqualTo(result.Sex));
            Assert.That(expectedResult.Birthday, Is.EqualTo(result.Birthday));
            Assert.That(expectedResult.Age, Is.EqualTo(result.Age));
            Assert.That(expectedResult.IsDead, Is.EqualTo(result.IsDead));
            Assert.That(expectedResult.IsSeparated, Is.EqualTo(result.IsSeparated));
            Assert.That(expectedResult.Biko, Is.EqualTo(result.Biko));
            Assert.That(expectedResult.SortNo, Is.EqualTo(result.SortNo));
            Assert.That(expectedResult.DiseaseName, Is.EqualTo(result.DiseaseName));
            Assert.That(expectedResult.IsDeleted, Is.EqualTo(result.IsDeleted));
            Assert.That(expectedResult.ListPtFamilyRekis.First().Id, Is.EqualTo(result.ListPtFamilyRekis.First().Id));
            Assert.That(expectedResult.ListPtFamilyRekis.First().ByomeiCd, Is.EqualTo(result.ListPtFamilyRekis.First().ByomeiCd));
            Assert.That(expectedResult.ListPtFamilyRekis.First().Byomei, Is.EqualTo(result.ListPtFamilyRekis.First().Byomei));
            Assert.That(expectedResult.ListPtFamilyRekis.First().Cmt, Is.EqualTo(result.ListPtFamilyRekis.First().Cmt));
            Assert.That(expectedResult.ListPtFamilyRekis.First().SortNo, Is.EqualTo(result.ListPtFamilyRekis.First().SortNo));
            Assert.That(expectedResult.ListPtFamilyRekis.First().IsDeleted, Is.EqualTo(result.ListPtFamilyRekis.First().IsDeleted));
        }

        [Test]
        public void TC_066_ConvertToSpecialNoteModel()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();

            // Arrange
            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            var specialNoteItem = new SpecialNoteItem(
                new SummaryInfItem(id: 2, 99, 1234, 5, "Summary Text", "R Summary Text"),
                new(),
                new PatientInfoItem(
                    new List<PtPregnancyItem>()
                    {
                        new PtPregnancyItem(id: 77, hpId:777, ptId:2345, seqNo:7, startDate: 20230101, endDate:20230102, periodDate:20230103, periodDueDate:20230104, ovulationDate:20230105, ovulationDueDate:20230106, isDeleted:1, sinDate:20230107)
                    },
                    new(),
                    new(),
                    new List<KensaInfDetailItem>()
                    {
                        new KensaInfDetailItem(hpId: 666, ptId:3456, iraiCd: 8, seqNo: 9, iraiDate:20230201,raiinNo:5555555, kensaItemCd:"V0001", resultVal: "VAL TEST 1", resultType:"TYPE TEST 1", abnormalKbn:"KBN TEST", isDeleted:3, cmtCd1:"COMMENT 1", cmtCd2: "COMMENT 2")
                    }
                    )
                );

            var result = commonMedicalCheck.ConvertToSpecialNoteModel(specialNoteItem);

            Assert.That(result.ImportantNoteModel.AlrgyFoodItems.Count, Is.EqualTo(0));
            Assert.That(result.ImportantNoteModel.AlrgyElseItems.Count, Is.EqualTo(0));
            Assert.That(result.ImportantNoteModel.AlrgyDrugItems.Count, Is.EqualTo(0));
            Assert.That(result.ImportantNoteModel.KioRekiItems.Count, Is.EqualTo(0));
            Assert.That(result.ImportantNoteModel.InfectionsItems.Count, Is.EqualTo(0));
            Assert.That(result.ImportantNoteModel.OtherDrugItems.Count, Is.EqualTo(0));
            Assert.That(result.ImportantNoteModel.OtcDrugItems.Count, Is.EqualTo(0));
            Assert.That(result.ImportantNoteModel.SuppleItems.Count, Is.EqualTo(0));
            Assert.That(result.PatientInfoModel.PregnancyItems.First().Id, Is.EqualTo(77));
            Assert.That(result.PatientInfoModel.PregnancyItems.First().HpId, Is.EqualTo(777));
            Assert.That(result.PatientInfoModel.PregnancyItems.First().PtId, Is.EqualTo(2345));
            Assert.That(result.PatientInfoModel.PregnancyItems.First().SeqNo, Is.EqualTo(7));
            Assert.That(result.PatientInfoModel.PregnancyItems.First().StartDate, Is.EqualTo(20230101));
            Assert.That(result.PatientInfoModel.PregnancyItems.First().EndDate, Is.EqualTo(20230102));
            Assert.That(result.PatientInfoModel.PregnancyItems.First().PeriodDate, Is.EqualTo(20230103));
            Assert.That(result.PatientInfoModel.PregnancyItems.First().PeriodDueDate, Is.EqualTo(20230104));
            Assert.That(result.PatientInfoModel.PregnancyItems.First().OvulationDate, Is.EqualTo(20230105));
            Assert.That(result.PatientInfoModel.PregnancyItems.First().OvulationDueDate, Is.EqualTo(20230106));
            Assert.That(result.PatientInfoModel.PregnancyItems.First().IsDeleted, Is.EqualTo(1));
            Assert.That(result.PatientInfoModel.PregnancyItems.First().SinDate, Is.EqualTo(20230107));
        }

        [Test]
        public void TC_067_CheckListOrder_NotExistErrorOrderList()
        {
            //Setup
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            var tenMsts = CommonCheckerData.ReadTenMst("day004", "day004");
            var drugDayLimits = CommonCheckerData.ReadDrugDayLimit("day004");
            var m10DayLimits = CommonCheckerData.ReadM10DayLimit("day004");
            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.DrugDayLimits.AddRange(drugDayLimits);
            tenantTracking.M10DayLimit.AddRange(m10DayLimits);
            tenantTracking.SaveChanges();

            int hpId = 999;
            long ptId = 1212;
            int sinDay = 20230101;

            var ordInfDetails = new List<OrdInfoDetailModel>()
        {
            new OrdInfoDetailModel("id1", 20, "61" + "day004", "・ｼ・・ｽ・・ｽ・・・ｻ・・ｫ・・ｷ・・ｳ・・", 1, "・・", 0, 2, 0, 1, 0, "1124017F4", "", "Y", 0),
            new OrdInfoDetailModel("id2", 21, "Y101" + "day004", "・・・・ｼ・・・ｵｷ・ｺ・・・・", 2, "・・･・・・", 0, 0, 0, 0, 1, "", "", "", 1),
        };

            var checkingListOdr = new List<OrdInfoModel>()
        {
            new OrdInfoModel(21, 0, ordInfDetails),
        };
            var realtimeCheckerCondition = new RealTimeCheckerCondition(
                                                isCheckingDuplication: false,
                                                isCheckingKinki: false,
                                                isCheckingAllergy: false,
                                                isCheckingDosage: false,
                                                isCheckingDays: true,
                                                isCheckingAge: false,
                                                isCheckingDisease: false,
                                                isCheckingInvalidData: false,
                                                isCheckingAutoCheck: false);

            try
            {
                var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);
                var listErrorInfo = commonMedicalCheck.CheckListOrder(hpId, ptId, sinDay, checkingListOdr, realtimeCheckerCondition, new(), new(), new(), true);
                var result = commonMedicalCheck.GetErrorDetails(hpId, ptId, sinDay, listErrorInfo, new());

                // Assert
                Assert.True(result.errors.Any());
            }
            finally
            {
                //Clear Data test
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.DrugDayLimits.RemoveRange(drugDayLimits);
                tenantTracking.M10DayLimit.RemoveRange(m10DayLimits);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_068_GetErrorDetails_DrugAllergy()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 999;
            long ptId = 1212;
            int sinDay = 20230101;

            var listErrorInfo = new List<UnitCheckInfoModel>()
                {
                    new UnitCheckInfoModel()
                    {
                        CheckerType = RealtimeCheckerType.DrugAllergy,
                        ErrorInfo = new List<DrugAllergyResultModel>()
                        {
                            new DrugAllergyResultModel()
                            {
                                Level = 1,
                                ItemCd = "1234",
                                YjCd = "93112345",
                                SeibunCd = "123",
                            },
                            new DrugAllergyResultModel()
                            {
                                Level = 2,
                                ItemCd = "1234",
                                YjCd = "93112346",
                                SeibunCd = "124",
                            },
                        }
                    },
                };

            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => new Dictionary<string, string> {
                                                                                              { "93112345", "Item Mock 1" },
                                                                                              { "93112346", "Item Mock 2" }
                                                                                             });

            mock.Setup(finder => finder.FindComponentNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => new Dictionary<string, string> {
                                                                                   { "123", "Sebun Mock 1" },
                                                                                   { "124", "Sebun Mock 2" }
                                                                                 });

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            var result = commonMedicalCheck.GetErrorDetails(hpId, ptId, sinDay, listErrorInfo, new());

            // Assert
            Assert.True(result.errors.Count == 2);
        }

        [Test]
        public void TC_069_GetErrorDetails_FoodAllergy()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 999;
            long ptId = 1212;
            int sinDay = 20230101;

            var listErrorInfo = new List<UnitCheckInfoModel>()
                {
                    new UnitCheckInfoModel()
                    {
                        CheckerType = RealtimeCheckerType.FoodAllergy,
                        ErrorInfo = new List<FoodAllergyResultModel>()
                        {
                            new FoodAllergyResultModel()
                            {
                                Id = "1",
                                ItemCd = "1234",
                                YjCd = "93112345",
                                AlrgyKbn = "3",
                                TenpuLevel = "A",
                            },
                            new FoodAllergyResultModel()
                            {
                                Id = "2",
                                ItemCd = "1234",
                                YjCd = "93112346",
                                AlrgyKbn = "4",
                                TenpuLevel = "1",
                            },
                        }
                    },
                };

            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => new Dictionary<string, string> {
                                                                                              { "93112345", "Item Mock 1" },
                                                                                              { "93112346", "Item Mock 2" }
                                                                                             });

            mock.Setup(finder => finder.FindFoodNameDic(It.IsAny<int>(), It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => new Dictionary<string, string> {
                                                                                   { "3", "Food Name Mock 1" },
                                                                                   { "4", "Food Name Mock 2" }
                                                                                 });

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            var result = commonMedicalCheck.GetErrorDetails(hpId, ptId, sinDay, listErrorInfo, new());

            // Assert
            Assert.That(result.errors.Count, Is.EqualTo(2));
        }

        [Test]
        public void TC_070_GetErrorDetails_Age()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 999;
            long ptId = 1212;
            int sinDay = 20230101;

            var listErrorInfo = new List<UnitCheckInfoModel>()
                {
                    new UnitCheckInfoModel()
                    {
                        CheckerType = RealtimeCheckerType.Age,
                        ErrorInfo = new List<AgeResultModel>()
                        {
                            new AgeResultModel()
                            {
                                Id = "1",
                                ItemCd = "1234",
                                YjCd = "93112345",
                                TenpuLevel = "A",
                            },
                            new AgeResultModel()
                            {
                                Id = "2",
                                ItemCd = "1234",
                                YjCd = "93112346",
                                TenpuLevel = "1",
                            },
                        }
                    },
                };

            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => new Dictionary<string, string> {
                                                                                              { "93112345", "Item Mock 1" },
                                                                                              { "93112346", "Item Mock 2" }
                                                                                             });

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            var result = commonMedicalCheck.GetErrorDetails(hpId, ptId, sinDay, listErrorInfo, new());

            // Assert
            Assert.That(result.errors.Count, Is.EqualTo(2));
        }

        [Test]
        public void TC_071_GetErrorDetails_Disease()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 999;
            long ptId = 1212;
            int sinDay = 20230101;

            var listErrorInfo = new List<UnitCheckInfoModel>()
                {
                    new UnitCheckInfoModel()
                    {
                        CheckerType = RealtimeCheckerType.Disease,
                        ErrorInfo = new List<DiseaseResultModel>()
                        {
                            new DiseaseResultModel()
                            {
                                Id = "1",
                                ItemCd = "1234",
                                YjCd = "93112345",
                                ByotaiCd = "33333",
                                DiseaseType = 1,
                                TenpuLevel = 2,
                            },
                            new DiseaseResultModel()
                            {
                                Id = "2",
                                ItemCd = "1234",
                                YjCd = "93112346",
                                ByotaiCd = "44444",
                                DiseaseType = 2,
                                TenpuLevel = 3,
                            },
                        }
                    },
                };

            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) => new Dictionary<string, string> {
                                                                                              { "93112345", "Item Mock 1" },
                                                                                              { "93112346", "Item Mock 2" }
                                                                                             });

            mock.Setup(finder => finder.FindDiseaseNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> inputList) => new Dictionary<string, string> {
                                                                                              { "33333", "Byotai Mock 1" },
                                                                                              { "44444", "Byotai Mock 2" }
                                                                                             });

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            var result = commonMedicalCheck.GetErrorDetails(hpId, ptId, sinDay, listErrorInfo, new());

            // Assert
            Assert.That(result.errors.Count, Is.EqualTo(2));
        }

        [Test]
        public void TC_072_GetErrorDetails_Kinki()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 999;
            long ptId = 1212;
            int sinDay = 20230101;

            var listErrorInfo = new List<UnitCheckInfoModel>()
                {
                    new UnitCheckInfoModel()
                    {
                        CheckerType = RealtimeCheckerType.KinkiSupplement,
                        ErrorInfo = new List<KinkiResultModel>()
                        {
                            new KinkiResultModel()
                            {
                                Id = "1",
                                ItemCd = "1234",
                                KinkiItemCd = "23456",
                                SeibunCd = "33333",
                                CommentCode = "C1234",
                                SayokijyoCode = "S1234",
                                Kyodo = "2",
                            },
                            new KinkiResultModel()
                            {
                                Id = "2",
                                ItemCd = "1235",
                                SeibunCd = "44444",
                                CommentCode = "C1235",
                                SayokijyoCode = "S1235",
                                Kyodo = "3",
                            },
                        }
                    },
                };

            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            commonMedicalCheck._suppleItemNameDictionary = new Dictionary<string, string> {
                                                                                            { "S1234", "SeibunName_Mocked_Test_1" },
                                                                                            { "S1235", "SeibunName_Mocked_Test_2" },
                                                                                          };

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> input, int input2) => new Dictionary<string, string> {
                                                                        { "1234", "ItemName Mock 1" },
                                                                        { "1235", "ItemName Mock 2" }
                                                                      });

            mock.Setup(finder => finder.FindClassName(hpId, It.IsAny<string>()))
            .Returns((int inputHpId, string input) => ("ClassName_Mocked_Test"));

            mock.Setup(finder => finder.FindSuppleItemNameDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> input) => new Dictionary<string, string>
                                                                       {
                                                                        { "33333", "Byotai Mock 1" },
                                                                        { "44444", "Byotai Mock 2" }
                                                                       });

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindKinkiCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> input) => new Dictionary<string, string>
                                                                       {
                                                                        { "C1234", "Comment Mock 1" },
                                                                        { "C1235", "Comment Mock 2" }
                                                                       });

            mock.Setup(finder => finder.FindKijyoCommentDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> input) => new Dictionary<string, string>
                                                                       {
                                                                        { "S1234", "Kijyo Mock 1" },
                                                                        { "S1235", "Kijyo Mock 2" }
                                                                       });

            mock.Setup(finder => finder.GetSupplementComponentInfoDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> input) => new Dictionary<string, string>
                                                                       {
                                                                        { "33333", "Supplement Mock 1" },
                                                                        { "44444", "Supplement Mock 2" }
                                                                       });

            // Act

            var result = commonMedicalCheck.GetErrorDetails(hpId, ptId, sinDay, listErrorInfo, new());

            // Assert
            Assert.That(result.errors.Count, Is.EqualTo(2));
        }

        [Test]
        public void TC_073_GetErrorDetails_KinkiUser()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 999;
            long ptId = 1212;
            int sinDay = 20230101;

            var listErrorInfo = new List<UnitCheckInfoModel>()
                {
                    new UnitCheckInfoModel()
                    {
                        CheckerType = RealtimeCheckerType.KinkiUser,
                        ErrorInfo = new List<KinkiResultModel>()
                        {
                            new KinkiResultModel()
                            {
                                Id = "1",
                                ItemCd = "1234",
                                KinkiItemCd = "23456",
                                SeibunCd = "33333",
                                CommentCode = "C1234",
                                SayokijyoCode = "S1234",
                                Kyodo = "2",
                                AYjCd = "A1234",
                                BYjCd = "B1234",
                            },
                            new KinkiResultModel()
                            {
                                Id = "2",
                                ItemCd = "1235",
                                SeibunCd = "44444",
                                CommentCode = "C1235",
                                SayokijyoCode = "S1235",
                                Kyodo = "3",
                                AYjCd = "A1235",
                                BYjCd = "B1235",
                            },
                        }
                    },
                };

            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> input, int input2) => new Dictionary<string, string>
                                                                       {
                                                                        { "A1234", "Item Mock 1" },
                                                                        { "B1234", "Item Mock 2" },
                                                                        { "A1235", "Item Mock 1" },
                                                                        { "B1235", "Item Mock 2" },
                                                                       });

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act

            var result = commonMedicalCheck.GetErrorDetails(hpId, ptId, sinDay, listErrorInfo, new());

            // Assert
            Assert.That(result.errors.Count, Is.EqualTo(2));
        }

        [Test]
        public void TC_074_GetErrorDetails_Days()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 999;
            long ptId = 1212;
            int sinDay = 20230101;

            var listErrorInfo = new List<UnitCheckInfoModel>()
                {
                    new UnitCheckInfoModel()
                    {
                        CheckerType = RealtimeCheckerType.Days,
                        ErrorInfo = new List<DayLimitResultModel>()
                        {
                            new DayLimitResultModel()
                            {
                                Id = "1",
                                ItemCd = "1234",
                            },
                            new DayLimitResultModel()
                            {
                                Id = "2",
                                ItemCd = "1235",
                            },
                        }
                    },
                };

            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemName(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
            .Returns((int hpId, string stringInput, int sinday) => "Item Name Mock");

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act

            var result = commonMedicalCheck.GetErrorDetails(hpId, ptId, sinDay, listErrorInfo, new());

            // Assert
            Assert.That(result.errors.Count, Is.EqualTo(2));
        }

        [Test]
        public void TC_075_GetErrorDetails_Dosage()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 999;
            long ptId = 1212;
            int sinDay = 20230101;

            var listErrorInfo = new List<UnitCheckInfoModel>()
                {
                    new UnitCheckInfoModel()
                    {
                        CheckerType = RealtimeCheckerType.Dosage,
                        ErrorInfo = new List<DosageResultModel>()
                        {
                            new DosageResultModel()
                            {
                                Id = "1",
                                ItemCd = "1234",
                                YjCd = "333444",
                            },
                            new DosageResultModel()
                            {
                                Id = "2",
                                ItemCd = "1235",
                                YjCd = "444555",
                            },
                        }
                    },
                };

            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(hpId, It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> input, int input2) => new Dictionary<string, string> {
                                                                        { "1234", "ItemName Mock 1" },
                                                                        { "1235", "ItemName Mock 2" }
                                                                      });

            mock.Setup(finder => finder.GetUsageDosageDic(hpId, It.IsAny<List<string>>()))
            .Returns((int inputHpId, List<string> input) => new Dictionary<string, string> {
                                                                        { "333444", "ItemName Mock 1" },
                                                                        { "444555", "ItemName Mock 2" }
                                                                      });

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            var result = commonMedicalCheck.GetErrorDetails(hpId, ptId, sinDay, listErrorInfo, new());

            // Assert
            Assert.That(result.errors.Count, Is.EqualTo(2));
        }

        [Test]
        public void TC_076_GetErrorDetails_Duplication()
        {
            var mock = new Mock<IRealtimeOrderErrorFinder>();
            var mockRealtimeCheckerFinder = new Mock<IRealtimeCheckerFinder>();
            int hpId = 999;
            long ptId = 1212;
            int sinDay = 20230101;

            var listErrorInfo = new List<UnitCheckInfoModel>()
                {
                    new UnitCheckInfoModel()
                    {
                        CheckerType = RealtimeCheckerType.Duplication,
                        ErrorInfo = new List<DuplicationResultModel>()
                        {
                            new DuplicationResultModel()
                            {
                                Id = "1",
                                ItemCd = "1234",
                            },
                            new DuplicationResultModel()
                            {
                                Id = "2",
                                ItemCd = "1235",
                            },
                        }
                    },
                };

            var commonMedicalCheck = new CommonMedicalCheck(TenantProvider, mock.Object, mockRealtimeCheckerFinder.Object);

            mock.Setup(finder => finder.FindItemNameDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> input, int input2) => new Dictionary<string, string> {
                                                                        { "1234", "ItemName Mock 1" },
                                                                        { "1235", "ItemName Mock 2" }
                                                                      });

            mock.Setup(finder => finder.FindItemNameByItemCodeDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>()))
            .Returns((int hpId, List<string> inputList, int sinday) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            mock.Setup(finder => finder.FindLastItemNameByYjCdDic(It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<int>(), false))
            .Returns((int hpId, List<string> inputList, int sinday, bool dontCheckIsDeleted) =>
            inputList.ToDictionary(item => item, item => $"MockedValueFor_{item}"));

            // Act
            var result = commonMedicalCheck.GetErrorDetails(hpId, ptId, sinDay, listErrorInfo, new());

            // Assert
            Assert.That(result.errors.Count, Is.EqualTo(2));
        }
    }
}