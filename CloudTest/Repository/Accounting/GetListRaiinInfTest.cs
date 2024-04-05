using CloudUnitTest.SampleData.AccountingRepository;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudUnitTest.Repository.Accounting
{
    public class GetListRaiinInfTest : BaseUT
    {
        [Test]
        public void GetListRaiinInf_001()
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
        public void GetListRaiinInf_002()
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
                Assert.True(result.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void GetListRaiinInf_003()
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
                Assert.True(result.Count == 1);
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
                tenant.SaveChanges();
                CleanupResources(accountingRepository);
            }
        }

        [Test]
        public void GetListRaiinInf_004()
        {
            SetupTestEnvironment(out AccountingRepository accountingRepository);
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 998; long ptId = 12345; int sinDate = 20180807; long raiinNo = 1234321; bool isGetHeader = true; bool getAll = true;
            var raiinInfs = AccountingRepositoryData.ReadRaiinInf();
            var raiinInfs2 = AccountingRepositoryData.ReadRaiinInf();
            tenant.AddRange(raiinInfs);
            try
            {
                tenant.SaveChanges();
                var result = accountingRepository.GetListRaiinInf(hpId, ptId, sinDate, raiinNo, isGetHeader, getAll);
                Assert.True(result.Count == 2 && result.Select(i=>i.PersonNumber).Contains(0));
            }
            finally
            {
                tenant.RemoveRange(raiinInfs);
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
