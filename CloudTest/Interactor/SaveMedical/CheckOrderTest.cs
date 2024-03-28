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
using Domain.Models.OrdInfDetails;
using Entity.Tenant;
using System.Linq.Dynamic.Core.Tokenizer;
using ZstdSharp.Unsafe;
using System.Drawing;
using System;
using Domain.Models.OrdInf;

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
            
            // Act
            var result = saveMedicalInteractor.CheckOrder(hpId, ptId, sinDate, inputDatas, inputDataList, status);

            // Assert
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

            //Act
            var result = saveMedicalInteractor.CheckOrder(hpId, ptId, sinDate, inputDatas, inputDataList, status);

            // Assert
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
            List<int> hokenPids = new List<int>();
            long ptId = 28032001;
            int hokenPid = 1;
            int sinDate = 20240304;
            byte status = 1;
            long raiinNo = 6739168;
            int id = 0;
            int sortNo = 2803;
            int seqNo = 1;
            bool isUpdateFile = true;
            int isDeleted = 0;

            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            PtHokenPattern ptHokenPattern = new PtHokenPattern()
            {
                HpId = hpId,
                PtId = ptId,
                HokenPid = hokenPid,
                SeqNo = seqNo
            };

            tenantTracking.Add(ptHokenPattern);
            tenantTracking.SaveChanges();
            hokenPids.Add(1);

            var ptHokenPatterns = tenantTracking.PtHokenPatterns.Where(h => h.HpId == hpId && hokenPids.Contains(h.HokenPid) && h.PtId == ptHokenPattern.PtId && h.IsDeleted == 0);
            var hokenInfModels = ptHokenPatterns.Select(r => new HokenInfModel(r.HokenPid, r.PtId, r.HpId, r.StartDate, r.EndDate)).ToList();
            mockIInsuranceRepository.Setup(finder => finder.GetCheckListHokenInf(hpId, ptHokenPattern.PtId, hokenPids))
            .Returns((int hpId, long ptId, List<int> hokenPids) => hokenInfModels);

            List<string> listFileItems = new List<string>()
            {
                "Kaito1",
                "Kaito2",
                "Kaito3"
            };

            FileItemInputItem fileItemInputItem = new FileItemInputItem(isUpdateFile, listFileItems);

            List<OdrInfItemInputData> inputDataList = new List<OdrInfItemInputData>()
            {
                new OdrInfItemInputData(hpId, raiinNo, 0, 0, ptId, sinDate, 1, 0, "", 0, 0, 0, 0, 0, 0, sortNo, id, new(), isDeleted)
            };

            SaveMedicalInputData inputDatas = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, 0, 0, 0, 0, 0, 0, "", "", "", status, inputDataList, new(), 999, true, true,
                                                                       fileItemInputItem, new(), new(), new(), new(), new(), new(), new());

            // Act
            var result = saveMedicalInteractor.CheckOrder(hpId, ptId, sinDate, inputDatas, inputDataList, status);

            // Assert
            try
            {
                Assert.That(result.Item1.Any() || result.Item2.Any());
            }
            finally
            {
                tenantTracking.PtHokenPatterns.Remove(ptHokenPattern);
                tenantTracking.SaveChanges();
            }
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
            List<int> hokenPids = new List<int>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var odrInfs = tenantTracking.OdrInfs.FirstOrDefault(x => x.HpId == hpId && x.OdrKouiKbn != 10 && x.IsDeleted == 0);

            int hokenPid = 1;
            long ptId = odrInfs?.PtId ?? 0;
            int sinDate = odrInfs?.SinDate ?? 0;
            byte status = 1;
            long raiinNo = odrInfs?.RaiinNo ?? 0;
            int id = 0;
            int sortNo = 2803;
            bool isUpdateFile = true;
            int seqNo = 1;
            int isDeleted = 0;

            PtHokenPattern ptHokenPattern = new PtHokenPattern()
            {
                HpId = hpId,
                PtId = ptId,
                HokenPid = hokenPid,
                SeqNo = seqNo
            };

            tenantTracking.Add(ptHokenPattern);
            tenantTracking.SaveChanges();
            hokenPids.Add(1);
            hokenPids.Add(0);

            var ptHokenPatterns = tenantTracking.PtHokenPatterns.Where(h => h.HpId == hpId && hokenPids.Contains(h.HokenPid) && h.PtId == ptHokenPattern.PtId && h.IsDeleted == 0);
            var hokenInfModels = ptHokenPatterns.Select(r => new HokenInfModel(r.HokenPid, r.PtId, r.HpId, r.StartDate, r.EndDate)).ToList();
            mockIInsuranceRepository.Setup(finder => finder.GetCheckListHokenInf(hpId, ptHokenPattern.PtId, hokenPids))
            .Returns((int hpId, long ptId, List<int> hokenPids) => hokenInfModels);

            List<long> raiinNos = new List<long>()
            {
                odrInfs.RaiinNo
            };

            List<OrdInfModel> ordInfModels = new List<OrdInfModel>()
            {

            };

            var allOdrInf = tenantTracking.OdrInfs.Where(odr => odr.PtId == ptId && odr.HpId == hpId && odr.OdrKouiKbn != 10 && raiinNos.Contains(odr.RaiinNo) && odr.IsDeleted == 0)?.ToList();
            var ordInfDetailModels = allOdrInf?.Select(o => ConvertToModel(o)) ?? new List<OrdInfModel>();
            mockIOrdInfRepository.Setup(finder => finder.GetListToCheckValidate(ptId, hpId, raiinNos))
            .Returns((long ptId, int hpId, List<long> raiinNos) => ordInfDetailModels);

            List<string> listFileItems = new List<string>()
            {
                "Kaito1",
                "Kaito2",
                "Kaito3"
            };

            FileItemInputItem fileItemInputItem = new FileItemInputItem(isUpdateFile, listFileItems);

            List<OdrInfItemInputData> inputDataList = new List<OdrInfItemInputData>()
            {
                new OdrInfItemInputData(1, raiinNo, 129005, 1, ptId, sinDate, 1, 0, "", 0, 0, 0, 0, 0, 0, sortNo, id + 1, new(), isDeleted),
                new OdrInfItemInputData(1, raiinNo, 129005, 1, ptId, sinDate, 0, 0, "", 0, 0, 0, 0, 0, 0, sortNo, id + 2, new(), isDeleted),
                new OdrInfItemInputData(1, raiinNo, 0, 0, ptId, sinDate, 0, 0, "", 0, 0, 0, 0, 0, 0, sortNo, id +3, new(), isDeleted)
            };

            SaveMedicalInputData inputDatas = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, 0, 0, 0, 0, 0, 0, "", "", "", status, inputDataList, new(), 999, true, true,
                                                                       fileItemInputItem, new(), new(), new(), new(), new(), new(), new());

            // Act
            var result = saveMedicalInteractor.CheckOrder(hpId, ptId, sinDate, inputDatas, inputDataList, status);

            // Assert
            try
            {
                Assert.That(result.Item1.Any() || result.Item2.Any());
            }
            finally
            {
                tenantTracking.PtHokenPatterns.Remove(ptHokenPattern);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_005_SaveMedicalInteractor_TestAddAuditTempSaveData()
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
            List<int> hokenPids = new List<int>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var odrInfs = tenantTracking.OdrInfs.FirstOrDefault(x => x.HpId == hpId && x.OdrKouiKbn != 10 && x.IsDeleted == 0);

            int hokenPid = 1;
            long ptId = odrInfs?.PtId ?? 0;
            int sinDate = odrInfs?.SinDate ?? 0;
            byte status = 1;
            long raiinNo = odrInfs?.RaiinNo ?? 0;
            int id = 0;
            int sortNo = 2803;
            bool isUpdateFile = true;
            int seqNo = 1;
            int isDeleted = 0;

            PtHokenPattern ptHokenPattern = new PtHokenPattern()
            {
                HpId = hpId,
                PtId = ptId,
                HokenPid = hokenPid,
                SeqNo = seqNo
            };

            tenantTracking.Add(ptHokenPattern);
            tenantTracking.SaveChanges();
            hokenPids.Add(0);
            hokenPids.Add(1);

            var ptHokenPatterns = tenantTracking.PtHokenPatterns.Where(h => h.HpId == hpId && hokenPids.Contains(h.HokenPid) && h.PtId == ptHokenPattern.PtId && h.IsDeleted == 0);
            var hokenInfModels = ptHokenPatterns.Select(r => new HokenInfModel(r.HokenPid, r.PtId, r.HpId, r.StartDate, r.EndDate)).ToList();
            mockIInsuranceRepository.Setup(finder => finder.GetCheckListHokenInf(hpId, ptHokenPattern.PtId, hokenPids))
            .Returns((int hpId, long ptId, List<int> hokenPids) => hokenInfModels);

            List<long> raiinNos = new List<long>()
            {
                odrInfs.RaiinNo
            };

            List<OrdInfModel> ordInfModels = new List<OrdInfModel>()
            {

            };

            var allOdrInf = tenantTracking.OdrInfs.Where(odr => odr.PtId == ptId && odr.HpId == hpId && odr.OdrKouiKbn != 10 && raiinNos.Contains(odr.RaiinNo) && odr.IsDeleted == 0)?.ToList();
            var ordInfDetailModels = allOdrInf?.Select(o => ConvertToModel(o)) ?? new List<OrdInfModel>();
            mockIOrdInfRepository.Setup(finder => finder.GetListToCheckValidate(ptId, hpId, raiinNos))
            .Returns((long ptId, int hpId, List<long> raiinNos) => ordInfDetailModels);

            List<string> listFileItems = new List<string>()
            {
                "Kaito1",
                "Kaito2",
                "Kaito3"
            };

            FileItemInputItem fileItemInputItem = new FileItemInputItem(isUpdateFile, listFileItems);

            List<OdrInfItemInputData> inputDataList = new List<OdrInfItemInputData>()
            {
                new OdrInfItemInputData(1, raiinNo, 129005, 1, ptId, sinDate, 0, 0, "", 0, 0, 0, 0, 0, 0, sortNo, id + 1, new(), isDeleted),
                new OdrInfItemInputData(1, raiinNo, 129005, 1, ptId, sinDate, 1, 0, "", 0, 0, 0, 0, 0, 0, sortNo, id + 2, new(), isDeleted),
                new OdrInfItemInputData(1, raiinNo, 0, 0, ptId, sinDate, 1, 0, "", 0, 0, 0, 0, 0, 0, sortNo, id +3, new(), isDeleted)
            };

            SaveMedicalInputData inputDatas = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, 0, 0, 0, 0, 0, 0, "", "", "", status, inputDataList, new(), 999, true, true,
                                                                       fileItemInputItem, new(), new(), new(), new(), new(), new(), new());

            // Act
            var result = saveMedicalInteractor.CheckOrder(hpId, ptId, sinDate, inputDatas, inputDataList, status);

            // Assert
            try
            {
                Assert.That(result.Item1.Any() || result.Item2.Any());
            }
            finally
            {
                tenantTracking.PtHokenPatterns.Remove(ptHokenPattern);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_006_SaveMedicalInteractor_TestAddAuditTempSaveData()
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

            Random random = new();
            int hpId = 1;
            List<int> hokenPids = new List<int>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var odrInfs = tenantTracking.OdrInfs.FirstOrDefault(x => x.HpId == hpId && x.OdrKouiKbn != 10 && x.IsDeleted == 0);

            int hokenPid = 1;
            long ptId = odrInfs?.PtId ?? 0;
            int sinDate = odrInfs?.SinDate ?? 0;
            byte status = 1;
            long raiinNo = odrInfs?.RaiinNo ?? 0;
            int id = 0;
            int sortNo = 2803;
            bool isUpdateFile = true;
            int seqNo = 1;
            int isDeleted = 0;
            string itemCd = "itemCd";
            string ipnNameCd = "ipnNameCd";
            string masterSbt = "masterSbt";
            int cmtCol1 = random.Next(999, 99999);
            int ten = random.Next(999, 99999);
            int startDate = 20220202;
            int endDate = 20220203;
            double yakka = random.Next(999, 99999);
            string ipnCd = "";
            string ipnName = "ipnName";

            PtHokenPattern ptHokenPattern = new PtHokenPattern()
            {
                HpId = hpId,
                PtId = ptId,
                HokenPid = hokenPid,
                SeqNo = seqNo
            };

            tenantTracking.Add(ptHokenPattern);
            tenantTracking.SaveChanges();
            hokenPids.Add(1);
            hokenPids.Add(0);

            List<TenItemModel> tenMstList = new() { new TenItemModel(hpId, itemCd, ipnNameCd, masterSbt, cmtCol1, ten) };
            var ipnNameMsts = new List<Tuple<string, string>>() { Tuple.Create(ipnNameCd, ipnName) };

            mockIOrdInfRepository.Setup(finder => finder.GetIpnMst(hpId, 20050307, 20050307, new() { ipnCd }))
            .Returns((int hpId, int sinDateMin, int sinDateMax, List<string> ipnCds) => ipnNameMsts);

            var ipnMinYakaMsts = new List<IpnMinYakkaMstModel> { new(random.Next(999, 99999), hpId, ipnNameCd, startDate, endDate, yakka, seqNo, 0, false) };
            mockIOrdInfRepository.Setup(finder => finder.GetCheckIpnMinYakkaMsts(hpId, sinDate, new() { ipnCd })).Returns((int hpId, int sinDate, List<string> ipnNameCds) => ipnMinYakaMsts);

            mockIMstItemRepository.Setup(finder => finder.GetCheckTenItemModels(hpId, sinDate, new() { itemCd })).Returns((int hpId, int sinDate, List<string> itemCds) => tenMstList);

            mockIOrdInfRepository.Setup(finder => finder.CheckIsGetYakkaPrices(hpId, tenMstList, sinDate)).Returns((int hpId, List<TenItemModel> tenMstList, int sinDate) => new List<Tuple<string, string, bool>>() { Tuple.Create(ipnNameCd, itemCd, false && false) });

            var ptHokenPatterns = tenantTracking.PtHokenPatterns.Where(h => h.HpId == hpId && hokenPids.Contains(h.HokenPid) && h.PtId == ptHokenPattern.PtId && h.IsDeleted == 0);
            var hokenInfModels = ptHokenPatterns.Select(r => new HokenInfModel(r.HokenPid, r.PtId, r.HpId, r.StartDate, r.EndDate)).ToList();
            
            mockIInsuranceRepository.Setup(finder => finder.GetCheckListHokenInf(hpId, ptHokenPattern.PtId, hokenPids)).Returns((int hpId, long ptId, List<int> hokenPids) => hokenInfModels);

            List<long> raiinNos = new List<long>()
            {
                odrInfs.RaiinNo
            };

            List<OrdInfModel> ordInfModels = new List<OrdInfModel>()
            {

            };

            var allOdrInf = tenantTracking.OdrInfs.Where(odr => odr.PtId == ptId && odr.HpId == hpId && odr.OdrKouiKbn != 10 && raiinNos.Contains(odr.RaiinNo) && odr.IsDeleted == 0)?.ToList();
            var ordInfDetailModels = allOdrInf?.Select(o => ConvertToModel(o)) ?? new List<OrdInfModel>();
            mockIOrdInfRepository.Setup(finder => finder.GetListToCheckValidate(ptId, hpId, raiinNos))
            .Returns((long ptId, int hpId, List<long> raiinNos) => ordInfDetailModels);

            List<string> listFileItems = new List<string>()
            {
                "Kaito1",
                "Kaito2",
                "Kaito3"
            };

            FileItemInputItem fileItemInputItem = new FileItemInputItem(isUpdateFile, listFileItems);

            List<OdrInfDetailItemInputData> odrDetails = new List<OdrInfDetailItemInputData>()
            {
                new OdrInfDetailItemInputData(2, "itemCd", 99999999)
            };

            List<OdrInfItemInputData> inputDataList = new List<OdrInfItemInputData>()
            {
                new OdrInfItemInputData(1, raiinNo, 129005, 1, ptId, sinDate, 1, 0, "", 0, 0, 0, 0, 0, 0, sortNo, id + 1, odrDetails, isDeleted),
                new OdrInfItemInputData(1, raiinNo, 129005, 1, ptId, sinDate, 0, 0, "", 0, 0, 0, 0, 0, 0, sortNo, id + 2, new(), isDeleted),
                new OdrInfItemInputData(1, raiinNo, 0, 0, ptId, sinDate, 0, 0, "", 0, 0, 0, 0, 0, 0, sortNo, id +3, new(), isDeleted)
            };

            SaveMedicalInputData inputDatas = new SaveMedicalInputData(hpId, ptId, raiinNo, sinDate, 0, 0, 0, 0, 0, 0, "", "", "", status, inputDataList, new(), 999, true, true,
                                                                       fileItemInputItem, new(), new(), new(), new(), new(), new(), new());

            // Act
            var result = saveMedicalInteractor.CheckOrder(hpId, ptId, sinDate, inputDatas, inputDataList, status);

            // Assert
            try
            {
                Assert.That(result.Item1.Any() || result.Item2.Any());
            }
            finally
            {
                tenantTracking.PtHokenPatterns.Remove(ptHokenPattern);
                tenantTracking.SaveChanges();
            }
        }

        private static OrdInfModel ConvertToModel(OdrInf ordInf, string createName = "", string updateName = "")
        {
            return new OrdInfModel(ordInf.HpId,
                        ordInf.RaiinNo,
                        ordInf.RpNo,
                        ordInf.RpEdaNo,
                        ordInf.PtId,
                        ordInf.SinDate,
                        ordInf.HokenPid,
                        ordInf.OdrKouiKbn,
                        ordInf.RpName ?? string.Empty,
                        ordInf.InoutKbn,
                        ordInf.SikyuKbn,
                        ordInf.SyohoSbt,
                        ordInf.SanteiKbn,
                        ordInf.TosekiKbn,
                        ordInf.DaysCnt,
                        ordInf.SortNo,
                        ordInf.IsDeleted,
                        ordInf.Id,
                        new List<OrdInfDetailModel>(),
                        ordInf.CreateDate,
                        ordInf.CreateId,
                        createName,
                        ordInf.UpdateDate,
                        ordInf.UpdateId,
                        updateName,
                        ordInf.CreateMachine ?? string.Empty,
                        ordInf.UpdateMachine ?? string.Empty
                   );
        }
    }
}
