using Amazon.Runtime.Internal.Util;
using CloudUnitTest.SampleData.AccountingRepository;
using Domain.Models.AccountDue;
using Entity.SuperAdmin;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Repositories;
using Interactor.Accounting;
using Microsoft.Extensions.Configuration;
using Moq;
using PostgreDataContext;
using UseCase.Accounting.CheckAccountingStatus;

namespace CloudUnitTest.Interactor.Accounting
{
    public class CheckAccountingStatusInteractorTest : BaseUT
    {
        private readonly string billUpdate = "今回請求額が更新されています。\n窓口精算画面を開きなおしてください。";
        private readonly string verifyDate = "請求金額が変更されているため、以下の請求日の領収証を印刷できません";
        private readonly string validAmount = "入金額が正しくありません。・入金額を確認し、再実行してください。";
        private readonly string mbOk = "mbOk";
        private readonly string mbClose = "mbClose";
        private readonly string yesNoCancel = "YesNoCancel";

        [Test]
        public void CheckAccountingStatusInteractorTest_001_Default()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            int hpId = 998; long ptId = 12345; int sinDate = 0; long raiinNo = 1234321; int debitBalance = 0; int sumAdjust = 0; int credit = 0; int wari = 0; bool isDisCharge = true; bool isSaveAccounting = false;
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>();
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>();
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            //Act
            var result = checkAccountingStatusInteractor.Handle(inputData);
            //Assert
            Assert.True(result.ErrorType == string.Empty && result.Message == string.Empty && result.Status == CheckAccountingStatus.Successed);
        }

        //[Test]
        //public void CheckAccountingStatusInteractorTest_002_IsDisCharge_Check_SyunoSeikyusChecking()
        //{
        //    // Arrange 
        //    SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor,out StackExchange.Redis.IDatabase cache);
        //    int hpId = 998; long ptId = 12345; int sinDate = 0; long raiinNo = 1234321; int debitBalance = 0; int sumAdjust = 0; int credit = 0; int wari = 0; bool isDisCharge = true; bool isSaveAccounting = false;
        //    List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>();
        //    List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>();
        //    var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
        //    //Act
        //    var result = checkAccountingStatusInteractor.Handle(inputData);
        //    //Assert
        //    Assert.True(result.ErrorType == mbOk && result.Message == billUpdate && result.Status == CheckAccountingStatus.BillUpdated);
        //}

        [Test]
        public void CheckAccountingStatusInteractorTest_003_IsDisCharge_Check_SyunoChanged()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = 0; int sumAdjust = 0; int credit = 0; int wari = 0; bool isDisCharge = true; bool isSaveAccounting = false;
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>();
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>();
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == mbOk && result.Message == billUpdate && result.Status == CheckAccountingStatus.BillUpdated);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        [Test]
        public void CheckAccountingStatusInteractorTest_004_IsDisCharge_SyunoDifferent_CheckSeikyuNew()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = 0; int sumAdjust = 0; int credit = 0; int wari = 0; bool isDisCharge = true; bool isSaveAccounting = false;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,0,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, 1234322, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>();
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == mbOk && result.Message == billUpdate && result.Status == CheckAccountingStatus.BillUpdated);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void CheckAccountingStatusInteractorTest_005_IsDisCharge_SyunoDifferent_CheckNyukinKbn()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = 0; int sumAdjust = 0; int credit = 0; int wari = 0; bool isDisCharge = true; bool isSaveAccounting = false;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,0,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, raiinNo, 0,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>();
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == mbOk && result.Message == billUpdate && result.Status == CheckAccountingStatus.BillUpdated);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        [Test]
        public void CheckAccountingStatusInteractorTest_006_IsDisCharge_SyunoDifferent_CheckSum()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = 0; int sumAdjust = 0; int credit = 0; int wari = 0; bool isDisCharge = true; bool isSaveAccounting = false;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,20,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, raiinNo, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>();
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == mbOk && result.Message == billUpdate && result.Status == CheckAccountingStatus.BillUpdated);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        [Test]
        public void CheckAccountingStatusInteractorTest_007_IsDisCharge_SyunoDifferent_CheckSum()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = 0; int sumAdjust = 0; int credit = 0; int wari = 0; bool isDisCharge = true; bool isSaveAccounting = false;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,0,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, raiinNo, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>();
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                UpdateValueSystemConf(cache, hpId, tenant);
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == string.Empty && result.Message == string.Empty && result.Status == CheckAccountingStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        /// <summary>
        /// accDue<0 && accDue == credit
        /// </summary>
        [Test]
        public void CheckAccountingStatusInteractorTest_008_IsDisCharge_CheckCredit()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = -1; int sumAdjust = 0; int credit = -1; int wari = 0; bool isDisCharge = true; bool isSaveAccounting = false;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,0,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, raiinNo, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>();
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                UpdateValueSystemConf(cache, hpId, tenant);
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == string.Empty && result.Message == string.Empty && result.Status == CheckAccountingStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        /// <summary>
        /// credit >= 0 && credit <= accDue
        /// </summary>
        [Test]
        public void CheckAccountingStatusInteractorTest_009_IsDisCharge_CheckCredit()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = 1; int sumAdjust = 0; int credit = 1; int wari = 0; bool isDisCharge = true; bool isSaveAccounting = false;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,0,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, raiinNo, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>();
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                UpdateValueSystemConf(cache, hpId, tenant);
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == string.Empty && result.Message == string.Empty && result.Status == CheckAccountingStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        [Test]
        public void CheckAccountingStatusInteractorTest_010_IsDisCharge_CheckCredit_Faild()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = 1; int sumAdjust = 0; int credit = 2; int wari = 0; bool isDisCharge = true; bool isSaveAccounting = false;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,0,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, raiinNo, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>();
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                UpdateValueSystemConf(cache, hpId, tenant);
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == mbClose && result.Message == validAmount && result.Status == CheckAccountingStatus.ValidPaymentAmount);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        [Test]
        public void CheckAccountingStatusInteractorTest_011_CheckSyunoDifferent_True()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = 1; int sumAdjust = 0; int credit = 1; int wari = 0; bool isDisCharge = true; bool isSaveAccounting = false;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,0,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, raiinNo, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, raiinNo, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                UpdateValueSystemConf(cache, hpId, tenant);
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == mbOk && result.Message == billUpdate && result.Status == CheckAccountingStatus.BillUpdated);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        [Test]
        public void CheckAccountingStatusInteractorTest_012_dateNotVerify_Empty()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = 1; int sumAdjust = 0; int credit = 1; int wari = 0; bool isDisCharge = true; bool isSaveAccounting = false;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,0,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, raiinNo, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, 12345321, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoSeikyu2s = AccountingRepositoryData.ReadSyunoSeikyu();
            syunoSeikyu2s[0].RaiinNo = 12345321;
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[1].OyaRaiinNo = 801265567;
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoSeikyu2s);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                UpdateValueSystemConf(cache, hpId, tenant);
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == string.Empty && result.Message == string.Empty && result.Status == CheckAccountingStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoSeikyu2s);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        [Test]
        public void CheckAccountingStatusInteractorTest_013_VerifyCredit_DateNotVerify_Empty()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = 1; int sumAdjust = 0; int credit = 1; int wari = 0; bool isDisCharge = true; bool isSaveAccounting = false;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,0,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, raiinNo, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, 12345321, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoSeikyu2s = AccountingRepositoryData.ReadSyunoSeikyu();
            syunoSeikyu2s[0].RaiinNo = 12345321;
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[1].OyaRaiinNo = 801265567;
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoSeikyu2s);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                UpdateValueSystemConf(cache, hpId, tenant);
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == string.Empty && result.Message == string.Empty && result.Status == CheckAccountingStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoSeikyu2s);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        [Test]
        public void CheckAccountingStatusInteractorTest_014_VerifyCredit_DateNotVerify_NotEmpty()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = 1; int sumAdjust = 0; int credit = 1; int wari = 0; bool isDisCharge = true; bool isSaveAccounting = false;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,0,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, raiinNo, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, 12345321, 1,0,0,0,"",0,0,1,"",syunoNyukinModels)
            };
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoSeikyu2s = AccountingRepositoryData.ReadSyunoSeikyu();
            syunoSeikyu2s[0].RaiinNo = 12345321;
            syunoSeikyu2s[0].NewSeikyuGaku = 1;
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[1].OyaRaiinNo = 801265567;
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoSeikyu2s);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                UpdateValueSystemConf(cache, hpId, tenant);
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == yesNoCancel && result.Status == CheckAccountingStatus.DateNotVerify);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoSeikyu2s);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        [Test]
        public void CheckAccountingStatusInteractorTest_015_IsSaveAccounting_CheckSyunoChanged_Faild()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = 1; int sumAdjust = 0; int credit = 1; int wari = 0; bool isDisCharge = false; bool isSaveAccounting = true;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,0,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, 1234322, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, 12345321, 1,0,0,0,"",0,0,1,"",syunoNyukinModels)
            };
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoSeikyu2s = AccountingRepositoryData.ReadSyunoSeikyu();
            syunoSeikyu2s[0].RaiinNo = 12345321;
            syunoSeikyu2s[0].NewSeikyuGaku = 1;
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[1].OyaRaiinNo = 801265567;
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoSeikyu2s);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                UpdateValueSystemConf(cache, hpId, tenant);
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == mbOk && result.Message == billUpdate && result.Status == CheckAccountingStatus.BillUpdated);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoSeikyu2s);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        [Test]
        public void CheckAccountingStatusInteractorTest_016_IsSaveAccounting_CheckCredit_Faild()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = 1; int sumAdjust = 0; int credit = 1; int wari = 0; bool isDisCharge = false; bool isSaveAccounting = true;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,0,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, raiinNo, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, 12345321, 1,0,0,0,"",0,0,1,"",syunoNyukinModels)
            };
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoSeikyu2s = AccountingRepositoryData.ReadSyunoSeikyu();
            syunoSeikyu2s[0].RaiinNo = 12345321;
            syunoSeikyu2s[0].NewSeikyuGaku = 1;
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[1].OyaRaiinNo = 801265567;
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoSeikyu2s);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                UpdateValueSystemConf(cache, hpId, tenant);
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == mbClose && result.Message == validAmount && result.Status == CheckAccountingStatus.ValidPaymentAmount);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoSeikyu2s);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        /// <summary>
        /// credit + wari <= sumAdjust
        /// </summary>
        [Test]
        public void CheckAccountingStatusInteractorTest_016_IsSaveAccounting_CheckCredit_True()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = 0; int sumAdjust = 1; int credit = 1; int wari = 0; bool isDisCharge = false; bool isSaveAccounting = true;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,0,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, raiinNo, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, 12345321, 1,0,0,0,"",0,0,1,"",syunoNyukinModels)
            };
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoSeikyu2s = AccountingRepositoryData.ReadSyunoSeikyu();
            syunoSeikyu2s[0].RaiinNo = 12345321;
            syunoSeikyu2s[0].NewSeikyuGaku = 1;
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[1].OyaRaiinNo = 801265567;
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoSeikyu2s);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                UpdateValueSystemConf(cache, hpId, tenant);
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == string.Empty && result.Message == string.Empty && result.Status == CheckAccountingStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoSeikyu2s);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        /// <summary>
        /// accDue < 0 && sumCredit == 0
        /// </summary>
        [Test]
        public void CheckAccountingStatusInteractorTest_017_IsSaveAccounting_CheckCredit_True()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = -1; int sumAdjust = 1; int credit = 0; int wari = 0; bool isDisCharge = false; bool isSaveAccounting = true;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,0,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, raiinNo, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, 12345321, 1,0,0,0,"",0,0,1,"",syunoNyukinModels)
            };
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoSeikyu2s = AccountingRepositoryData.ReadSyunoSeikyu();
            syunoSeikyu2s[0].RaiinNo = 12345321;
            syunoSeikyu2s[0].NewSeikyuGaku = 1;
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[1].OyaRaiinNo = 801265567;
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoSeikyu2s);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                UpdateValueSystemConf(cache, hpId, tenant);
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == string.Empty && result.Message == string.Empty && result.Status == CheckAccountingStatus.Successed);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoSeikyu2s);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        /// <summary>
        /// accDue < 0 sumCredit <= sumAdjust
        /// </summary>
        [Test]
        public void CheckAccountingStatusInteractorTest_018_IsSaveAccounting_CheckCredit_True()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = -1; int sumAdjust = 1; int credit = 1; int wari = 0; bool isDisCharge = false; bool isSaveAccounting = true;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,0,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, raiinNo, 1,0,0,0,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, 12345321, 1,0,0,0,"",0,0,1,"",syunoNyukinModels)
            };
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            var syunoSeikyu2s = AccountingRepositoryData.ReadSyunoSeikyu();
            syunoSeikyu2s[0].RaiinNo = 12345321;
            syunoSeikyu2s[0].NewSeikyuGaku = 1;
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[1].OyaRaiinNo = 801265567;
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoSeikyu2s);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                UpdateValueSystemConf(cache, hpId, tenant);
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == yesNoCancel && result.Status == CheckAccountingStatus.DateNotVerify);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoSeikyu2s);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        [Test]
        public void CheckAccountingStatusInteractorTest_019_IsSaveAccounting_ParseValueUpdate()
        {
            // Arrange 
            SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; int debitBalance = -1; int sumAdjust = 1; int credit = 1; int wari = 0; bool isDisCharge = false; bool isSaveAccounting = true;
            var syunoNyukinModels = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, sinDate,raiinNo,0,0,0,0,0,0,0,"",0,0,"")
            };
            List<SyunoSeikyuDto> syunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, raiinNo, 1,0,0,3,"",0,0,0,"",syunoNyukinModels)
            };
            List<SyunoSeikyuDto> allSyunoSeikyuDtos = new List<SyunoSeikyuDto>()
            {
                new SyunoSeikyuDto(hpId,ptId, sinDate, 12345321, 1,0,0,0,"",0,0,1,"",syunoNyukinModels)
            };
            var inputData = new CheckAccountingStatusInputData(hpId, ptId, sinDate, raiinNo, debitBalance, sumAdjust, credit, wari, isDisCharge, isSaveAccounting, syunoSeikyuDtos, allSyunoSeikyuDtos);
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            syunoSeikyus[0].SeikyuGaku = 3;
            var syunoSeikyu2s = AccountingRepositoryData.ReadSyunoSeikyu();
            syunoSeikyu2s[0].RaiinNo = 12345321;
            syunoSeikyu2s[0].NewSeikyuGaku = 1;
            var syunoNyukins = AccountingRepositoryData.ReadSyunoNyukin();
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            raiinInfs[1].OyaRaiinNo = 801265567;
            var hokenPatterns = AccountingRepositoryData.ReadPtHokenPattern();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            tenant.AddRange(syunoSeikyus);
            tenant.AddRange(syunoSeikyu2s);
            tenant.AddRange(syunoNyukins);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(hokenPatterns);
            tenant.AddRange(kaikeiInfs);
            try
            {
                UpdateValueSystemConf(cache, hpId, tenant);
                tenant.SaveChanges();
                //Act
                var result = checkAccountingStatusInteractor.Handle(inputData);
                //Assert
                Assert.True(result.ErrorType == yesNoCancel && result.Status == CheckAccountingStatus.DateNotVerify);
            }
            finally
            {
                tenant.RemoveRange(syunoSeikyus);
                tenant.RemoveRange(syunoSeikyu2s);
                tenant.RemoveRange(syunoNyukins);
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(hokenPatterns);
                tenant.RemoveRange(kaikeiInfs);
                tenant.SaveChanges();
            }

        }

        private void SetupTestEnvironment(out CheckAccountingStatusInteractor checkAccountingStatusInteractor, out StackExchange.Redis.IDatabase cache)
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            var accountingRepository = new AccountingRepository(TenantProvider, mockConfiguration.Object);
            var systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
            checkAccountingStatusInteractor = new CheckAccountingStatusInteractor(accountingRepository, systemConfRepository);
            cache = systemConfRepository.GetCache();
        }

        private void UpdateValueSystemConf(StackExchange.Redis.IDatabase cache, int hpId, TenantDataContext tenant)
        {
            var keySystemConfig = TenantProvider.GetDomainName() + CacheKeyConstant.GetListSystemConf + "_" + hpId;
            if (cache.KeyExists(keySystemConfig))
            {
                cache.KeyDelete(keySystemConfig);
            }
            var systemConf = tenant.SystemConfs.FirstOrDefault(p => p.HpId == hpId && p.GrpCd == 3020 && p.GrpEdaNo == 0);
            var temp = systemConf?.Val ?? 0;
            if (systemConf != null) systemConf.Val = 0;
            else
            {
                systemConf = new SystemConf
                {
                    HpId = hpId,
                    GrpCd = 3020,
                    GrpEdaNo = 0,
                    CreateDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    CreateId = 1,
                    UpdateId = 1,
                    Val = 0
                };
                tenant.SystemConfs.Add(systemConf);
            }
        }
    }
}
