using CloudUnitTest.SampleData.AccountingRepository;
using Domain.Models.AccountDue;
using Domain.Models.Accounting;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CloudUnitTest.Repository.Accounting
{
    public class SaveAccountingTest : BaseUT
    {
        [Test]
        public void SaveAccountingTest_001_Faild()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            List<SyunoSeikyuModel> listAllSyunoSeikyu = new List<SyunoSeikyuModel>();
            List<SyunoSeikyuModel> syunoSeikyuModels = new List<SyunoSeikyuModel>();
            int hpId = 998; long ptId = 12345; int userId = 1; int accDue = 0; int sumAdjust = 0; int thisWari = 0; int thisCredit = 0;
            int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            try
            {
                var result = accountingRepository.SaveAccounting(listAllSyunoSeikyu, syunoSeikyuModels, hpId, ptId, userId, accDue, sumAdjust, thisWari,
                                                                thisCredit, payType, comment, isDisCharged, kaikeiTime, out List<long> listRaiinNoPrint);
                Assert.True(!result);
            }
            finally
            {
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void SaveAccountingTest_002_thisCredit()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 1; int accDue = 0; int sumAdjust = 0; int thisWari = 0; int thisCredit = 0;
            int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            List<SyunoSeikyuModel> listAllSyunoSeikyu = new List<SyunoSeikyuModel>()
            {
                new SyunoSeikyuModel(hpId, ptId,0,1234321,0,0,0,0,"",0,0,0,"")
            };
            List<SyunoSeikyuModel> syunoSeikyuModels = new List<SyunoSeikyuModel>()
            {
                 new SyunoSeikyuModel(hpId, ptId,0,1234321,0,0,0,0,"",0,0,0,"")
            };
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(syunoSeikyus);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.SaveAccounting(listAllSyunoSeikyu, syunoSeikyuModels, hpId, ptId, userId, accDue, sumAdjust, thisWari,
                                                                thisCredit, payType, comment, isDisCharged, kaikeiTime, out List<long> listRaiinNoPrint);
                Assert.True(result && listRaiinNoPrint.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void SaveAccountingTest_003_thisCredit_02()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 1; int accDue = 1; int sumAdjust = 0; int thisWari = 0; int thisCredit = 0;
            int payType = 0; string comment = ""; bool isDisCharged = true; string kaikeiTime = "";
            List<SyunoSeikyuModel> listAllSyunoSeikyu = new List<SyunoSeikyuModel>()
            {
                new SyunoSeikyuModel(hpId, ptId,0,1234321,0,0,0,0,"",0,0,0,"")
            };
            List<SyunoSeikyuModel> syunoSeikyuModels = new List<SyunoSeikyuModel>()
            {
                 new SyunoSeikyuModel(hpId, ptId,0,1234321,0,0,0,0,"",0,0,0,"")
            };
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(syunoSeikyus);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.SaveAccounting(listAllSyunoSeikyu, syunoSeikyuModels, hpId, ptId, userId, accDue, sumAdjust, thisWari,
                                                                thisCredit, payType, comment, isDisCharged, kaikeiTime, out List<long> listRaiinNoPrint);
                Assert.True(result && listRaiinNoPrint.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void SaveAccountingTest_004_isDisCharged()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 1; int accDue = 0; int sumAdjust = 0; int thisWari = 0; int thisCredit = 0;
            int payType = 0; string comment = ""; bool isDisCharged = true; string kaikeiTime = "";
            List<SyunoSeikyuModel> listAllSyunoSeikyu = new List<SyunoSeikyuModel>()
            {
                new SyunoSeikyuModel(hpId, ptId,0,1234321,0,0,0,0,"",0,0,0,"")
            };
            List<SyunoSeikyuModel> syunoSeikyuModels = new List<SyunoSeikyuModel>()
            {
                 new SyunoSeikyuModel(hpId, ptId,0,1234321,0,0,0,0,"",0,0,0,"")
            };
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(syunoSeikyus);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.SaveAccounting(listAllSyunoSeikyu, syunoSeikyuModels, hpId, ptId, userId, accDue, sumAdjust, thisWari,
                                                                thisCredit, payType, comment, isDisCharged, kaikeiTime, out List<long> listRaiinNoPrint);
                Assert.True(result && listRaiinNoPrint.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void SaveAccountingTest_005_thisSeikyuGaku()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 1; int accDue = 0; int sumAdjust = 0; int thisWari = 0; int thisCredit = 0;
            int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var syunoNyukinModel = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, 0, 1234321,1,0,0,2,0,0,0,"",0,0,"")
            };
            var kaikeiInfModel = new List<KaikeiInfModel>()
            {
                new KaikeiInfModel()
            };
            var syunoRaiinInfModel = new SyunoRaiinInfModel();
            List<SyunoSeikyuModel> listAllSyunoSeikyu = new List<SyunoSeikyuModel>()
            {
                new SyunoSeikyuModel(hpId, ptId,0,1234321,0,0,0,0,"",0,0,0,"")
            };
            List<SyunoSeikyuModel> syunoSeikyuModels = new List<SyunoSeikyuModel>()
            {
                 new SyunoSeikyuModel(hpId, ptId,0,1234321,0,0,1,2,"",0,0,0,"",syunoRaiinInfModel,syunoNyukinModel,kaikeiInfModel,10)
            };
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(syunoSeikyus);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.SaveAccounting(listAllSyunoSeikyu, syunoSeikyuModels, hpId, ptId, userId, accDue, sumAdjust, thisWari,
                                                                thisCredit, payType, comment, isDisCharged, kaikeiTime, out List<long> listRaiinNoPrint);
                Assert.True(result && listRaiinNoPrint.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void SaveAccountingTest_006()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 1; int accDue = 0; int sumAdjust = 0; int thisWari = 0; int thisCredit = 0;
            int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var syunoNyukinModel = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, 0, 1234321,1,0,0,0,0,0,0,"",0,0,"")
            };
            var kaikeiInfModel = new List<KaikeiInfModel>()
            {
                new KaikeiInfModel()
            };
            var syunoRaiinInfModel = new SyunoRaiinInfModel();
            List<SyunoSeikyuModel> listAllSyunoSeikyu = new List<SyunoSeikyuModel>()
            {
                new SyunoSeikyuModel(hpId, ptId,0,1234321,0,0,0,0,"",0,0,0,"")
            };
            List<SyunoSeikyuModel> syunoSeikyuModels = new List<SyunoSeikyuModel>()
            {
                 new SyunoSeikyuModel(hpId, ptId,0,1234321,0,0,1,0,"",0,0,0,"",syunoRaiinInfModel,syunoNyukinModel,kaikeiInfModel,10)
            };
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(syunoSeikyus);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.SaveAccounting(listAllSyunoSeikyu, syunoSeikyuModels, hpId, ptId, userId, accDue, sumAdjust, thisWari,
                                                                thisCredit, payType, comment, isDisCharged, kaikeiTime, out List<long> listRaiinNoPrint);
                Assert.True(result && listRaiinNoPrint.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        /// <summary>
        /// Check isLastRecord
        /// </summary>
        [Test]
        public void SaveAccountingTest_007_ParseValueUpdate()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 1; int accDue = 0; int sumAdjust = 1; int thisWari = 0; int thisCredit = 0;
            int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var syunoNyukinModel = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, 0, 1234321,1,0,0,0,0,0,0,"",0,0,"")
            };
            var kaikeiInfModel = new List<KaikeiInfModel>()
            {
                new KaikeiInfModel()
            };
            var syunoRaiinInfModel = new SyunoRaiinInfModel();
            List<SyunoSeikyuModel> listAllSyunoSeikyu = new List<SyunoSeikyuModel>()
            {
                new SyunoSeikyuModel(hpId, ptId,0,1234321,0,0,0,0,"",0,0,0,"")
            };
            List<SyunoSeikyuModel> syunoSeikyuModels = new List<SyunoSeikyuModel>()
            {
                 new SyunoSeikyuModel(hpId, ptId,0,1234321,0,0,1,0,"",0,0,0,"",syunoRaiinInfModel,syunoNyukinModel,kaikeiInfModel,10)
            };
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(syunoSeikyus);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.SaveAccounting(listAllSyunoSeikyu, syunoSeikyuModels, hpId, ptId, userId, accDue, sumAdjust, thisWari,
                                                                thisCredit, payType, comment, isDisCharged, kaikeiTime, out List<long> listRaiinNoPrint);
                Assert.True(result && listRaiinNoPrint.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        /// <summary>
        /// CHeck adjustFutan >= thisSeikyuGaku
        /// </summary>
        [Test]
        public void SaveAccountingTest_008_ParseValueUpdate_02()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 1; int accDue = 0; int sumAdjust = 2; int thisWari = 2; int thisCredit = 0;
            int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var syunoNyukinModel = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, 0, 1234321,1,0,0,0,0,0,0,"",0,0,"")
            };
            var kaikeiInfModel = new List<KaikeiInfModel>()
            {
                new KaikeiInfModel()
            };
            var syunoRaiinInfModel = new SyunoRaiinInfModel();
            List<SyunoSeikyuModel> listAllSyunoSeikyu = new List<SyunoSeikyuModel>()
            {
                new SyunoSeikyuModel(hpId, ptId,0,1234323,0,0,0,0,"",0,0,0,"")
            };
            List<SyunoSeikyuModel> syunoSeikyuModels = new List<SyunoSeikyuModel>()
            {
                 new SyunoSeikyuModel(hpId, ptId,0,1234321,0,0,1,0,"",0,0,0,"",syunoRaiinInfModel,syunoNyukinModel,kaikeiInfModel,10),
                 new SyunoSeikyuModel(hpId, ptId,0,1234322,0,0,1,0,"",0,0,0,"",syunoRaiinInfModel,syunoNyukinModel,kaikeiInfModel,10)
            };
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(syunoSeikyus);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.SaveAccounting(listAllSyunoSeikyu, syunoSeikyuModels, hpId, ptId, userId, accDue, sumAdjust, thisWari,
                                                                thisCredit, payType, comment, isDisCharged, kaikeiTime, out List<long> listRaiinNoPrint);
                Assert.True(result && listRaiinNoPrint.Count == 3);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        /// <summary>
        /// CHeck adjustFutan >= thisSeikyuGaku
        /// </summary>
        [Test]
        public void SaveAccountingTest_009_ParseValueUpdate_03()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 998; long ptId = 12345; int userId = 1; int accDue = 0; int sumAdjust = 2; int thisWari = 1; int thisCredit = 1;
            int payType = 0; string comment = ""; bool isDisCharged = false; string kaikeiTime = "";
            var syunoNyukinModel = new List<SyunoNyukinModel>()
            {
                new SyunoNyukinModel(hpId, ptId, 0, 1234321,1,0,0,0,0,0,0,"",0,0,"")
            };
            var kaikeiInfModel = new List<KaikeiInfModel>()
            {
                new KaikeiInfModel()
            };
            var syunoRaiinInfModel = new SyunoRaiinInfModel();
            List<SyunoSeikyuModel> listAllSyunoSeikyu = new List<SyunoSeikyuModel>()
            {
                new SyunoSeikyuModel(hpId, ptId,0,1234323,0,0,0,0,"",0,0,0,"")
            };
            List<SyunoSeikyuModel> syunoSeikyuModels = new List<SyunoSeikyuModel>()
            {
                 new SyunoSeikyuModel(hpId, ptId,0,1234321,0,0,1,5,"",0,0,0,"",syunoRaiinInfModel,syunoNyukinModel,kaikeiInfModel,10),
                 new SyunoSeikyuModel(hpId, ptId,0,1234322,0,0,1,0,"",0,0,0,"",syunoRaiinInfModel,syunoNyukinModel,kaikeiInfModel,10)
            };
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var syunoSeikyus = AccountingRepositoryData.ReadSyunoSeikyu();
            tenant.AddRange(raiinInfs);
            tenant.AddRange(syunoSeikyus);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.SaveAccounting(listAllSyunoSeikyu, syunoSeikyuModels, hpId, ptId, userId, accDue, sumAdjust, thisWari,
                                                                thisCredit, payType, comment, isDisCharged, kaikeiTime, out List<long> listRaiinNoPrint);
                Assert.True(result && listRaiinNoPrint.Count == 3);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(syunoSeikyus);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        private void SetupTestEnvironment(out AccountingRepository accountingRepository)
        {
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
            mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
            accountingRepository = new AccountingRepository(TenantProvider, mockConfiguration.Object);
        }
        private void CleanupResources(AccountingRepository accountingRepository)
        {
            accountingRepository.ReleaseResource();
        }
    }
}
