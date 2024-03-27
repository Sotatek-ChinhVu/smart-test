using Domain.Models.AuditLog;
using Domain.Models.HpInf;
using Domain.Models.Insurance;
using Domain.Models.Ka;
using Domain.Models.KarteInfs;
using Domain.Models.Medical;
using Domain.Models.MstItem;
using Domain.Models.OrdInfs;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.SpecialNote.SummaryInf;
using Domain.Models.SystemConf;
using Domain.Models.SystemGenerationConf;
using Domain.Models.TodayOdr;
using Domain.Models.User;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Interactor.CalculateService;
using Interactor.Family.ValidateFamilyList;
using Interactor.MedicalExamination.KensaIraiCommon;
using Interactor.MedicalExamination;
using Microsoft.Extensions.Options;
using Moq;
using UseCase.MedicalExamination.SaveMedical;
using UseCase.Diseases.Upsert;
using Domain.Models.Diseases;
using UseCase.MedicalExamination.UpsertTodayOrd;

namespace CloudUnitTest.Interactor.SaveMedical
{
    public class CheckOrderTest : BaseUT
    {
        [Test]
        public void TC_001_SaveMedicalInteractor_TestAddAuditTempSaveData()
        {
            //Arrange
            var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockIKaRepository = new Mock<IKaRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();
            var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
            var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
            var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
            var mockICalculateService = new Mock<ICalculateService>();
            var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
            var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
            var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
            var mockISystemConfRepository = new Mock<ISystemConfRepository>();
            var mockIAuditLogRepository = new Mock<IAuditLogRepository>();

            var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, TenantProvider, mockIOrdInfRepository.Object,
                                                                  mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object,
                                                                  mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                  mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object,
                                                                  mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object,
                                                                  mockISystemConfRepository.Object, mockIAuditLogRepository.Object);
            int hpId = 1;
            long ptId = 28032001;
            int sinDate = 20240304;
            byte status = 1;
            long raiinNo = 6739168;
            int id = 1;
            int sortNo = 2803;
            bool isUpdateFile = true;
            int isDeleted = 0;
            List<string> listFileItems = new List<string>()
            {
                "Kaito1",
                "Kaito2",
                "Kaito3"
            };

            FileItemInputItem fileItemInputItem = new FileItemInputItem(isUpdateFile, listFileItems);

            List<OdrInfItemInputData> inputDataList = new List<OdrInfItemInputData>()
            {
                new OdrInfItemInputData(hpId, raiinNo, 0, 0, ptId, sinDate, 0, 0, "", 0, 0, 0, 0, 0, 0, sortNo, id, new(), isDeleted)
            };

            SaveMedicalInputData inputDatas = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, 0, 0, 0, 0, 0, 0, "", "", "", status, inputDataList, new(), 999, true, true, 
                                                                       fileItemInputItem, new(), new(), new(), new(), new(), new(), new());
            
            var result = saveMedicalInteractor.CheckOrder(hpId, ptId, sinDate, inputDatas, inputDataList, status);

            Assert.That(result.Item1.Any() || result.Item2.Any());
        }

        [Test]
        public void TC_002_SaveMedicalInteractor_TestAddAuditTempSaveData()
        {
            //Arrange
            var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockIKaRepository = new Mock<IKaRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();
            var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
            var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
            var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
            var mockICalculateService = new Mock<ICalculateService>();
            var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
            var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
            var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
            var mockISystemConfRepository = new Mock<ISystemConfRepository>();
            var mockIAuditLogRepository = new Mock<IAuditLogRepository>();

            var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, TenantProvider, mockIOrdInfRepository.Object,
                                                                  mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object,
                                                                  mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                  mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object,
                                                                  mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object,
                                                                  mockISystemConfRepository.Object, mockIAuditLogRepository.Object);
            int hpId = 1;
            long ptId = 28032001;
            int sinDate = 20240304;
            byte status = 1;
            long raiinNo = 6739168;
            int id = 1;
            int sortNo = 2803;
            bool isUpdateFile = true;
            int isDeleted = 1;

            List<string> listFileItems = new List<string>()
            {
                "Kaito1",
                "Kaito2",
                "Kaito3"
            };

            FileItemInputItem fileItemInputItem = new FileItemInputItem(isUpdateFile, listFileItems);

            List<OdrInfItemInputData> inputDataList = new List<OdrInfItemInputData>()
            {
                new OdrInfItemInputData(hpId, raiinNo, 0, 0, ptId, sinDate, 0, 0, "", 0, 0, 0, 0, 0, 0, sortNo, id, new(), isDeleted)
            };

            SaveMedicalInputData inputDatas = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, 0, 0, 0, 0, 0, 0, "", "", "", status, inputDataList, new(), 999, true, true,
                                                                       fileItemInputItem, new(), new(), new(), new(), new(), new(), new());

            var result = saveMedicalInteractor.CheckOrder(hpId, ptId, sinDate, inputDatas, inputDataList, status);

            Assert.That(result.Item1.Any() || result.Item2.Any());
        }

        [Test]
        public void TC_003_SaveMedicalInteractor_TestAddAuditTempSaveData()
        {
            //Arrange
            var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockIKaRepository = new Mock<IKaRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();
            var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
            var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
            var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
            var mockICalculateService = new Mock<ICalculateService>();
            var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
            var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
            var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
            var mockISystemConfRepository = new Mock<ISystemConfRepository>();
            var mockIAuditLogRepository = new Mock<IAuditLogRepository>();

            var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, TenantProvider, mockIOrdInfRepository.Object,
                                                                  mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object,
                                                                  mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                  mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object,
                                                                  mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object,
                                                                  mockISystemConfRepository.Object, mockIAuditLogRepository.Object);
            int hpId = 1;
            long ptId = 28032001;
            int sinDate = 20240304;
            byte status = 1;
            long raiinNo = 6739168;
            int id = 0;
            int sortNo = 2803;
            bool isUpdateFile = true;
            int isDeleted = 0;

            List<string> listFileItems = new List<string>()
            {
                "Kaito1",
                "Kaito2",
                "Kaito3"
            };

            FileItemInputItem fileItemInputItem = new FileItemInputItem(isUpdateFile, listFileItems);

            List<OdrInfItemInputData> inputDataList = new List<OdrInfItemInputData>()
            {
                new OdrInfItemInputData(hpId, raiinNo, 0, 0, ptId, sinDate, 0, 0, "", 0, 0, 0, 0, 0, 0, sortNo, id, new(), isDeleted)
            };

            SaveMedicalInputData inputDatas = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, 0, 0, 0, 0, 0, 0, "", "", "", status, inputDataList, new(), 999, true, true,
                                                                       fileItemInputItem, new(), new(), new(), new(), new(), new(), new());

            var result = saveMedicalInteractor.CheckOrder(hpId, ptId, sinDate, inputDatas, inputDataList, status);

            Assert.That(result.Item1.Any() || result.Item2.Any());
        }

        [Test]
        public void TC_004_SaveMedicalInteractor_TestAddAuditTempSaveData()
        {
            //Arrange
            var mockOptionsAccessor = new Mock<IOptions<AmazonS3Options>>();
            var mockIAmazonS3Service = new Mock<IAmazonS3Service>();
            var mockIOrdInfRepository = new Mock<IOrdInfRepository>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockIKaRepository = new Mock<IKaRepository>();
            var mockIMstItemRepository = new Mock<IMstItemRepository>();
            var mockISystemGenerationConfRepository = new Mock<ISystemGenerationConfRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();
            var mockIUserRepository = new Mock<IUserRepository>();
            var mockIHpInfRepository = new Mock<IHpInfRepository>();
            var mockISaveMedicalRepository = new Mock<ISaveMedicalRepository>();
            var mockITodayOdrRepository = new Mock<ITodayOdrRepository>();
            var mockIKarteInfRepository = new Mock<IKarteInfRepository>();
            var mockICalculateService = new Mock<ICalculateService>();
            var mockIValidateFamilyList = new Mock<IValidateFamilyList>();
            var mockISummaryInfRepository = new Mock<ISummaryInfRepository>();
            var mockIKensaIraiCommon = new Mock<IKensaIraiCommon>();
            var mockISystemConfRepository = new Mock<ISystemConfRepository>();
            var mockIAuditLogRepository = new Mock<IAuditLogRepository>();

            var saveMedicalInteractor = new SaveMedicalInteractor(mockOptionsAccessor.Object, mockIAmazonS3Service.Object, TenantProvider, mockIOrdInfRepository.Object,
                                                                  mockIReceptionRepository.Object, mockIKaRepository.Object, mockIMstItemRepository.Object, mockISystemGenerationConfRepository.Object,
                                                                  mockIPatientInforRepository.Object, mockIInsuranceRepository.Object, mockIUserRepository.Object, mockIHpInfRepository.Object,
                                                                  mockISaveMedicalRepository.Object, mockITodayOdrRepository.Object, mockIKarteInfRepository.Object,
                                                                  mockICalculateService.Object, mockIValidateFamilyList.Object, mockISummaryInfRepository.Object, mockIKensaIraiCommon.Object,
                                                                  mockISystemConfRepository.Object, mockIAuditLogRepository.Object);

            int hpId = 1;
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var odrInfs = tenantTracking.OdrInfs.FirstOrDefault(x => x.HpId == hpId && x.OdrKouiKbn != 10 && x.IsDeleted == 0);

            long ptId = odrInfs?.PtId ?? 0;
            int sinDate = odrInfs?.SinDate ?? 0;
            byte status = 1;
            long raiinNo = odrInfs?.RaiinNo ?? 0;
            int id = 0;
            int sortNo = 2803;
            bool isUpdateFile = true;
            int isDeleted = 0;
            List<long> raiinNos = new List<long>()
            {
                odrInfs.RaiinNo
            };

            List<OrdInfModel> ordInfModels = new List<OrdInfModel>()
            {

            };
            /*mockIOrdInfRepository.Setup(finder => finder.GetListToCheckValidate(ptId, hpId, raiinNos))
            .Returns((int hpId, List<TenItemModel> tenMsts, int sinDate) => );*/

            List<string> listFileItems = new List<string>()
            {
                "Kaito1",
                "Kaito2",
                "Kaito3"
            };

            FileItemInputItem fileItemInputItem = new FileItemInputItem(isUpdateFile, listFileItems);

            List<OdrInfItemInputData> inputDataList = new List<OdrInfItemInputData>()
            {
                new OdrInfItemInputData(-1, raiinNo, 0, 0, ptId, sinDate, 0, 0, "", 0, 0, 0, 0, 0, 0, sortNo, id + 1, new(), isDeleted),
                new OdrInfItemInputData(-1, raiinNo, 0, 0, ptId, sinDate, 0, 0, "", 0, 0, 0, 0, 0, 0, sortNo, id + 2, new(), isDeleted),
                new OdrInfItemInputData(-1, raiinNo, 0, 0, ptId, sinDate, 0, 0, "", 0, 0, 0, 0, 0, 0, sortNo, id +3, new(), isDeleted)
            };

            SaveMedicalInputData inputDatas = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, 0, 0, 0, 0, 0, 0, "", "", "", status, inputDataList, new(), 999, true, true,
                                                                       fileItemInputItem, new(), new(), new(), new(), new(), new(), new());

            var result = saveMedicalInteractor.CheckOrder(hpId, ptId, sinDate, inputDatas, inputDataList, status);

            Assert.That(result.Item1.Any() || result.Item2.Any());
        }
    }
}
