using CloudUnitTest.SampleData;
using Domain.Models.Santei;
using Entity.Tenant;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using PostgreDataContext;

namespace CloudUnitTest.Repository.Santei;

public class SanteiInfRepositoryTest : BaseUT
{
    #region GetListSanteiInf
    [Test]
    public void GetListSanteiInf_TestSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        var sampleData = ReadDataSanteiInf.ReadSanteiInf();
        tenant.SanteiInfs.AddRange(sampleData);
        tenant.SaveChanges();
        // Arrange
        SanteiInfRepository santeiInfRepository = new SanteiInfRepository(TenantProvider);
        // Act
        var value = santeiInfRepository.GetListSanteiInf(1, 883, 20221212);
        // Assert
        //Assert.True(value.Any(item => item.Id == 999999));
        tenant.SanteiInfs.RemoveRange(sampleData);
        tenant.SaveChanges();
    }


    #endregion
}
