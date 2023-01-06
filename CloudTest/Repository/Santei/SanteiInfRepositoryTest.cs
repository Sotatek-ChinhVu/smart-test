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
        #region DataExample
        int hpId = 1;
        long ptId = 1;
        int sinDate = 20221212;

        var listSanteiInfs = new List<SanteiInf>()
        {
            new SanteiInf()
            {
                HpId = hpId,
                PtId = ptId,
                Id = 1,
                ItemCd = "itemCd",
                SeqNo = 1,
                AlertDays = 1,
                AlertTerm = 2
            }
        };
        TenantProvider.GetNoTrackingDataContext().SanteiInfs.AddRange(listSanteiInfs);
        //mocTennantProvider.Setup(repo => repo.GetNoTrackingDataContext().SanteiInfs.AddRange(listSanteiInfs));

        #endregion

        var repository = new SanteiInfRepository(TenantProvider);
        var list = repository.GetListSanteiInf(hpId, ptId, sinDate);
    }


    #endregion
}
