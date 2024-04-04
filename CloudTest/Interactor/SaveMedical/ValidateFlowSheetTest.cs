﻿using Domain.Models.AuditLog;
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
using UseCase.FlowSheet.Upsert;

namespace CloudUnitTest.Interactor.SaveMedical
{
    public class ValidateFlowSheetTest : BaseUT
    {
        [Test]
        public void TC_001_SaveMedicalInteractor_TestValidateFlowSheet_TagNoNoValid()
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

            long raiinNo = 9999999;
            long ptId = 28032001;
            int sinDate = 20240328;
            int tagNo = -2;
            string cmt = "kaito";

            List<UpsertFlowSheetItemInputData> flowSheets = new List<UpsertFlowSheetItemInputData>()
            {
                new UpsertFlowSheetItemInputData(raiinNo, ptId, sinDate, tagNo, cmt)
            };

            // Act
            var result = saveMedicalInteractor.ValidateFlowSheet(flowSheets);

            // Assert
            Assert.That(result == UpsertFlowSheetStatus.TagNoNoValid);
        }

        [Test]
        public void TC_002_SaveMedicalInteractor_TestValidateFlowSheet_Valid()
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

            long raiinNo = 9999999;
            long ptId = 28032001;
            int sinDate = 20240328;
            int tagNo = 5;
            string cmt = "kaito";

            List<UpsertFlowSheetItemInputData> flowSheets = new List<UpsertFlowSheetItemInputData>()
            {
                new UpsertFlowSheetItemInputData(raiinNo, ptId, sinDate, tagNo, cmt)
            };

            // Act
            var result = saveMedicalInteractor.ValidateFlowSheet(flowSheets);

            // Assert
            Assert.That(result == UpsertFlowSheetStatus.Valid);
        }
    }
}
