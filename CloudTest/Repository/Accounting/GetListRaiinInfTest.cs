﻿using CloudUnitTest.SampleData.AccountingRepository;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;

namespace CloudUnitTest.Repository.Accounting
{
    public class GetListRaiinInfTest : BaseUT
    {
        [Test]
        public void GetListRaiinInf_001_getAll_Faild()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            try
            {
                int hpId = 1; long ptId = 1; int sinDate = 20230303; long raiinNo = -1; bool isGetHeader = false; bool getAll = false;
                var result = accountingRepository.GetListRaiinInf(hpId, ptId, sinDate, raiinNo, isGetHeader, getAll);
                Assert.True(result.Count == 0);
            }
            finally
            {
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void GetListRaiinInf_002_getAll_True()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            try
            {
                int hpId = 1; long ptId = 1; int sinDate = 20230303; long raiinNo = -1; bool isGetHeader = false; bool getAll = true;
                var result = accountingRepository.GetListRaiinInf(hpId, ptId, sinDate, raiinNo, isGetHeader, getAll);
                Assert.True(result.Count == 0);
            }
            finally
            {
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void GetListRaiinInf_003_isGetHeader_faild()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; bool isGetHeader = false; bool getAll = true;
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            tenant.AddRange(raiinInfs);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.GetListRaiinInf(hpId, ptId, sinDate, raiinNo, isGetHeader, getAll);
                Assert.True(result.Count == 2);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void GetListRaiinInf_004_isGetHeader()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; bool isGetHeader = true; bool getAll = true;
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            tenant.AddRange(raiinInfs);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.GetListRaiinInf(hpId, ptId, sinDate, raiinNo, isGetHeader, getAll);
                Assert.True(result.Count == 2);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void GetListRaiinInf_005_count_listRaiinInf()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; bool isGetHeader = true; bool getAll = true;
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            tenant.AddRange(raiinInfs);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.GetListRaiinInf(hpId, ptId, sinDate, raiinNo, isGetHeader, getAll);
                Assert.True(result.Count == 2 && result.Select(i => i.PersonNumber).Contains(0));
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void GetListRaiinInf_006_count_listRaiinInf()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; bool isGetHeader = true; bool getAll = true;
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var kaikeiInfs = AccountingRepositoryData.ReadKaikeiInf();
            raiinInfs.RemoveAt(1);
            tenant.AddRange(raiinInfs);
            tenant.AddRange(kaikeiInfs);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.GetListRaiinInf(hpId, ptId, sinDate, raiinNo, isGetHeader, getAll);
                Assert.True(result.Count == 1 && result.Select(i => i.PersonNumber).Contains(0));
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.RemoveRange(kaikeiInfs);
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
