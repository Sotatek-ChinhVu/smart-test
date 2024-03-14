using Domain.Models.Family;
using Entity.Tenant;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.SaveMedical;

public class SaveFamilyListTest : BaseUT
{
    #region SaveFamilyList
    [Test]
    public void TC_001_SaveFamilyList_TestCreateFamilySuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        FamilyModel familyModel = new FamilyModel(0, random.Next(999, 99999999), "zokugaraCd", random.Next(999, 99999), "nameFamily", "kanaNameFamily", 1, 20000202, 0, 0, "bikoFamily", random.Next(999, 99999), false, new());
        PtFamily? ptFamily = null;

        FamilyRepository familyRepository = new FamilyRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = familyRepository.SaveFamilyList(hpId, userId, new() { familyModel });

            // Assert
            ptFamily = tenant.PtFamilys.FirstOrDefault(item => item.HpId == hpId
                                                               && item.PtId == familyModel.PtId
                                                               && item.ZokugaraCd == familyModel.ZokugaraCd
                                                               && item.SortNo == familyModel.SortNo
                                                               && item.FamilyPtId == familyModel.FamilyPtId
                                                               && item.KanaName == familyModel.KanaName
                                                               && item.Name == familyModel.Name
                                                               && item.Sex == familyModel.Sex
                                                               && item.Birthday == familyModel.Birthday
                                                               && item.IsDead == familyModel.IsDead
                                                               && item.IsSeparated == familyModel.IsSeparated
                                                               && item.Biko == familyModel.Biko
                                                               && item.IsDeleted == 0);

            result = result && ptFamily != null;

            Assert.True(result);
        }
        finally
        {
            familyRepository.ReleaseResource();

            #region Remove Data Fetch
            if (ptFamily != null)
            {
                tenant.PtFamilys.Remove(ptFamily);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_002_SaveFamilyList_TestUpdateFamilySuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        FamilyModel familyModel = new FamilyModel(random.Next(999, 99999999), random.Next(999, 99999999), "zokugaraCd", random.Next(999, 99999), "nameFamily", "kanaNameFamily", 1, 20000202, 0, 0, "bikoFamily", random.Next(999, 99999), false, new());
        PtFamily? ptFamily = new PtFamily()
        {
            FamilyId = familyModel.FamilyId,
            HpId = hpId,
            PtId = familyModel.PtId,
        };

        tenant.PtFamilys.Add(ptFamily);
        FamilyRepository familyRepository = new FamilyRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = familyRepository.SaveFamilyList(hpId, userId, new() { familyModel });

            // Assert
            var ptFamilyAfter = tenant.PtFamilys.FirstOrDefault(item => item.HpId == hpId
                                                                && item.FamilyId == familyModel.FamilyId
                                                                && item.PtId == familyModel.PtId
                                                                && item.ZokugaraCd == familyModel.ZokugaraCd
                                                                && item.SortNo == familyModel.SortNo
                                                                && item.FamilyPtId == familyModel.FamilyPtId
                                                                && item.KanaName == familyModel.KanaName
                                                                && item.Name == familyModel.Name
                                                                && item.Sex == familyModel.Sex
                                                                && item.Birthday == familyModel.Birthday
                                                                && item.IsDead == familyModel.IsDead
                                                                && item.IsSeparated == familyModel.IsSeparated
                                                                && item.Biko == familyModel.Biko
                                                                && item.IsDeleted == 0);

            result = result && ptFamilyAfter != null;

            Assert.True(result);
        }
        finally
        {
            familyRepository.ReleaseResource();

            #region Remove Data Fetch
            if (ptFamily != null)
            {
                tenant.PtFamilys.Remove(ptFamily);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_003_SaveFamilyList_TestDeleteFamilySuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        FamilyModel familyModel = new FamilyModel(random.Next(999, 99999999), random.Next(999, 99999999), "zokugaraCd", random.Next(999, 99999), "nameFamily", "kanaNameFamily", 1, 20000202, 0, 0, "bikoFamily", random.Next(999, 99999), true, new());
        PtFamily? ptFamily = new PtFamily()
        {
            FamilyId = familyModel.FamilyId,
            HpId = hpId,
            PtId = familyModel.PtId,
        };

        tenant.PtFamilys.Add(ptFamily);
        FamilyRepository familyRepository = new FamilyRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = familyRepository.SaveFamilyList(hpId, userId, new() { familyModel });

            // Assert
            var ptFamilyAfter = tenant.PtFamilys.FirstOrDefault(item => item.HpId == hpId
                                                                        && item.FamilyId == familyModel.FamilyId
                                                                        && item.PtId == familyModel.PtId
                                                                        && item.IsDeleted == 1);

            result = result && ptFamilyAfter != null;

            Assert.True(result);
        }
        finally
        {
            familyRepository.ReleaseResource();

            #region Remove Data Fetch
            if (ptFamily != null)
            {
                tenant.PtFamilys.Remove(ptFamily);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_004_SaveFamilyList_TestUpdateFamilyFalse()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        FamilyModel familyModel = new FamilyModel(random.Next(999, 99999999), random.Next(999, 99999999), "zokugaraCd", random.Next(999, 99999), "nameFamily", "kanaNameFamily", 1, 20000202, 0, 0, "bikoFamily", random.Next(999, 99999), true, new());
        PtFamily? ptFamily = new PtFamily()
        {
            FamilyId = random.Next(999, 99999999),
            HpId = hpId,
            PtId = familyModel.PtId,
        };

        tenant.PtFamilys.Add(ptFamily);
        FamilyRepository familyRepository = new FamilyRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = familyRepository.SaveFamilyList(hpId, userId, new() { familyModel });

            // Assert
            var ptFamilyAfter = tenant.PtFamilys.FirstOrDefault(item => item.HpId == hpId
                                                                        && item.FamilyId == familyModel.FamilyId
                                                                        && item.PtId == familyModel.PtId);

            result = result && ptFamilyAfter == null;

            Assert.True(result);
        }
        finally
        {
            familyRepository.ReleaseResource();

            #region Remove Data Fetch
            if (ptFamily != null)
            {
                tenant.PtFamilys.Remove(ptFamily);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_005_SaveFamilyList_TestSaveFamilyRekiListSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        PtFamilyRekiModel ptFamilyRekiModel = new PtFamilyRekiModel(0, "byoCdUT", "byomeiPtFamilyReki", "cmtPtFamilyReki", random.Next(999, 99999999), false);
        FamilyModel familyModel = new FamilyModel(random.Next(999, 99999999), random.Next(999, 99999999), "zokugaraCd", random.Next(999, 99999), "nameFamily", "kanaNameFamily", 1, 20000202, 0, 0, "bikoFamily", random.Next(999, 99999), false, new() { ptFamilyRekiModel });
        PtFamily? ptFamily = new PtFamily()
        {
            FamilyId = familyModel.FamilyId,
            HpId = hpId,
            PtId = familyModel.PtId,
        };
        PtFamilyReki? ptFamilyReki = null;

        tenant.PtFamilys.Add(ptFamily);
        FamilyRepository familyRepository = new FamilyRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = familyRepository.SaveFamilyList(hpId, userId, new() { familyModel });

            // Assert
            var ptFamilyAfter = tenant.PtFamilys.FirstOrDefault(item => item.HpId == hpId
                                                                && item.FamilyId == familyModel.FamilyId
                                                                && item.PtId == familyModel.PtId
                                                                && item.ZokugaraCd == familyModel.ZokugaraCd
                                                                && item.SortNo == familyModel.SortNo
                                                                && item.FamilyPtId == familyModel.FamilyPtId
                                                                && item.KanaName == familyModel.KanaName
                                                                && item.Name == familyModel.Name
                                                                && item.Sex == familyModel.Sex
                                                                && item.Birthday == familyModel.Birthday
                                                                && item.IsDead == familyModel.IsDead
                                                                && item.IsSeparated == familyModel.IsSeparated
                                                                && item.Biko == familyModel.Biko
                                                                && item.IsDeleted == 0);

            if (ptFamilyAfter == null)
            {
                Assert.True(false);
            }
            ptFamilyReki = tenant.PtFamilyRekis.FirstOrDefault(item => item.HpId == hpId
                                                                       && item.PtId == ptFamilyAfter!.FamilyPtId
                                                                       && item.FamilyId == ptFamilyAfter.FamilyId
                                                                       && item.SortNo == ptFamilyRekiModel.SortNo
                                                                       && item.ByomeiCd == ptFamilyRekiModel.ByomeiCd
                                                                       && item.Byomei == ptFamilyRekiModel.Byomei
                                                                       && item.Cmt == ptFamilyRekiModel.Cmt
                                                                       && item.IsDeleted == 0);

            result = result && ptFamilyAfter != null && ptFamilyReki != null;

            Assert.True(result);
        }
        finally
        {
            familyRepository.ReleaseResource();

            #region Remove Data Fetch
            if (ptFamily != null)
            {
                tenant.PtFamilys.Remove(ptFamily);
            }
            if (ptFamilyReki != null)
            {
                tenant.PtFamilyRekis.Remove(ptFamilyReki);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_006_SaveFamilyList_TestUpdateFamilyRekiListSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        PtFamilyRekiModel ptFamilyRekiModel = new PtFamilyRekiModel(random.Next(999, 99999999), "byoCdUT", "byomeiPtFamilyReki", "cmtPtFamilyReki", random.Next(999, 99999999), false);
        FamilyModel familyModel = new FamilyModel(random.Next(999, 99999999), random.Next(999, 99999999), "zokugaraCd", random.Next(999, 99999), "nameFamily", "kanaNameFamily", 1, 20000202, 0, 0, "bikoFamily", random.Next(999, 99999), false, new() { ptFamilyRekiModel });
        PtFamily? ptFamily = new()
        {
            FamilyId = familyModel.FamilyId,
            HpId = hpId,
            PtId = familyModel.PtId,
        };
        PtFamilyReki? ptFamilyReki = new()
        {
            HpId = hpId,
            Id = ptFamilyRekiModel.Id,
            FamilyId = ptFamily.FamilyId
        };

        tenant.PtFamilys.Add(ptFamily);
        tenant.PtFamilyRekis.Add(ptFamilyReki);
        FamilyRepository familyRepository = new FamilyRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = familyRepository.SaveFamilyList(hpId, userId, new() { familyModel });

            // Assert
            var ptFamilyAfter = tenant.PtFamilys.FirstOrDefault(item => item.HpId == hpId
                                                                && item.FamilyId == familyModel.FamilyId
                                                                && item.PtId == familyModel.PtId
                                                                && item.ZokugaraCd == familyModel.ZokugaraCd
                                                                && item.SortNo == familyModel.SortNo
                                                                && item.FamilyPtId == familyModel.FamilyPtId
                                                                && item.KanaName == familyModel.KanaName
                                                                && item.Name == familyModel.Name
                                                                && item.Sex == familyModel.Sex
                                                                && item.Birthday == familyModel.Birthday
                                                                && item.IsDead == familyModel.IsDead
                                                                && item.IsSeparated == familyModel.IsSeparated
                                                                && item.Biko == familyModel.Biko
                                                                && item.IsDeleted == 0);

            if (ptFamilyAfter == null)
            {
                Assert.True(false);
            }
            var ptFamilyRekiAfter = tenant.PtFamilyRekis.FirstOrDefault(item => item.HpId == hpId
                                                                        && item.FamilyId == ptFamilyAfter!.FamilyId
                                                                        && item.Id == ptFamilyRekiModel.Id
                                                                        && item.SortNo == ptFamilyRekiModel.SortNo
                                                                        && item.ByomeiCd == ptFamilyRekiModel.ByomeiCd
                                                                        && item.Byomei == ptFamilyRekiModel.Byomei
                                                                        && item.Cmt == ptFamilyRekiModel.Cmt
                                                                        && item.IsDeleted == 0);

            result = result && ptFamilyAfter != null && ptFamilyRekiAfter != null;

            Assert.True(result);
        }
        finally
        {
            familyRepository.ReleaseResource();

            #region Remove Data Fetch
            if (ptFamily != null)
            {
                tenant.PtFamilys.Remove(ptFamily);
            }
            if (ptFamilyReki != null)
            {
                tenant.PtFamilyRekis.Remove(ptFamilyReki);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_007_SaveFamilyList_TestDeleteFamilyRekiListSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        PtFamilyRekiModel ptFamilyRekiModel = new PtFamilyRekiModel(random.Next(999, 99999999), "byoCdUT", "byomeiPtFamilyReki", "cmtPtFamilyReki", random.Next(999, 99999999), true);
        FamilyModel familyModel = new FamilyModel(random.Next(999, 99999999), random.Next(999, 99999999), "zokugaraCd", random.Next(999, 99999), "nameFamily", "kanaNameFamily", 1, 20000202, 0, 0, "bikoFamily", random.Next(999, 99999), false, new() { ptFamilyRekiModel });
        PtFamily? ptFamily = new()
        {
            FamilyId = familyModel.FamilyId,
            HpId = hpId,
            PtId = familyModel.PtId,
        };
        PtFamilyReki? ptFamilyReki = new()
        {
            HpId = hpId,
            Id = ptFamilyRekiModel.Id,
            FamilyId = ptFamily.FamilyId
        };

        tenant.PtFamilys.Add(ptFamily);
        tenant.PtFamilyRekis.Add(ptFamilyReki);
        FamilyRepository familyRepository = new FamilyRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = familyRepository.SaveFamilyList(hpId, userId, new() { familyModel });

            // Assert
            var ptFamilyAfter = tenant.PtFamilys.FirstOrDefault(item => item.HpId == hpId
                                                                && item.FamilyId == familyModel.FamilyId
                                                                && item.PtId == familyModel.PtId
                                                                && item.ZokugaraCd == familyModel.ZokugaraCd
                                                                && item.SortNo == familyModel.SortNo
                                                                && item.FamilyPtId == familyModel.FamilyPtId
                                                                && item.KanaName == familyModel.KanaName
                                                                && item.Name == familyModel.Name
                                                                && item.Sex == familyModel.Sex
                                                                && item.Birthday == familyModel.Birthday
                                                                && item.IsDead == familyModel.IsDead
                                                                && item.IsSeparated == familyModel.IsSeparated
                                                                && item.Biko == familyModel.Biko
                                                                && item.IsDeleted == 0);

            if (ptFamilyAfter == null)
            {
                Assert.True(false);
            }
            var ptFamilyRekiAfter = tenant.PtFamilyRekis.FirstOrDefault(item => item.HpId == hpId
                                                                                && item.FamilyId == ptFamilyAfter!.FamilyId
                                                                                && item.Id == ptFamilyRekiModel.Id
                                                                                && item.IsDeleted == 1);

            result = result && ptFamilyAfter != null && ptFamilyRekiAfter != null;

            Assert.True(result);
        }
        finally
        {
            familyRepository.ReleaseResource();

            #region Remove Data Fetch
            if (ptFamily != null)
            {
                tenant.PtFamilys.Remove(ptFamily);
            }
            if (ptFamilyReki != null)
            {
                tenant.PtFamilyRekis.Remove(ptFamilyReki);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_008_SaveFamilyList_TestSaveFamilyRekiListFalse()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        PtFamilyRekiModel ptFamilyRekiModel = new PtFamilyRekiModel(random.Next(999, 99999999), "byoCdUT", "byomeiPtFamilyReki", "cmtPtFamilyReki", random.Next(999, 99999999), true);
        FamilyModel familyModel = new FamilyModel(random.Next(999, 99999999), random.Next(999, 99999999), "zokugaraCd", random.Next(999, 99999), "nameFamily", "kanaNameFamily", 1, 20000202, 0, 0, "bikoFamily", random.Next(999, 99999), false, new() { ptFamilyRekiModel });
        PtFamily? ptFamily = new()
        {
            FamilyId = familyModel.FamilyId,
            HpId = hpId,
            PtId = familyModel.PtId,
        };
        PtFamilyReki? ptFamilyReki = new()
        {
            HpId = hpId,
            Id = random.Next(999, 99999999),
            FamilyId = ptFamily.FamilyId
        };

        tenant.PtFamilys.Add(ptFamily);
        tenant.PtFamilyRekis.Add(ptFamilyReki);
        FamilyRepository familyRepository = new FamilyRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = familyRepository.SaveFamilyList(hpId, userId, new() { familyModel });

            // Assert
            var ptFamilyAfter = tenant.PtFamilys.FirstOrDefault(item => item.HpId == hpId
                                                                && item.FamilyId == familyModel.FamilyId
                                                                && item.PtId == familyModel.PtId
                                                                && item.ZokugaraCd == familyModel.ZokugaraCd
                                                                && item.SortNo == familyModel.SortNo
                                                                && item.FamilyPtId == familyModel.FamilyPtId
                                                                && item.KanaName == familyModel.KanaName
                                                                && item.Name == familyModel.Name
                                                                && item.Sex == familyModel.Sex
                                                                && item.Birthday == familyModel.Birthday
                                                                && item.IsDead == familyModel.IsDead
                                                                && item.IsSeparated == familyModel.IsSeparated
                                                                && item.Biko == familyModel.Biko
                                                                && item.IsDeleted == 0);

            if (ptFamilyAfter == null)
            {
                Assert.True(false);
            }
            var ptFamilyRekiAfter = tenant.PtFamilyRekis.FirstOrDefault(item => item.HpId == hpId
                                                                                && item.FamilyId == ptFamilyAfter!.FamilyId
                                                                                && item.Id == ptFamilyRekiModel.Id);

            result = result && ptFamilyAfter != null && ptFamilyRekiAfter == null;

            Assert.True(result);
        }
        finally
        {
            familyRepository.ReleaseResource();

            #region Remove Data Fetch
            if (ptFamily != null)
            {
                tenant.PtFamilys.Remove(ptFamily);
            }
            if (ptFamilyReki != null)
            {
                tenant.PtFamilyRekis.Remove(ptFamilyReki);
            }
            tenant.SaveChanges();
            #endregion
        }
    }

    [Test]
    public void TC_009_SaveFamilyList_TestUpdatePtInfSuccess()
    {
        var tenant = TenantProvider.GetNoTrackingDataContext();
        Random random = new();
        int hpId = random.Next(999, 99999);
        int userId = random.Next(999, 99999);

        FamilyModel familyModel = new FamilyModel(random.Next(999, 99999999), random.Next(999, 99999999), "zokugaraCd", random.Next(999, 99999), "nameFamily", "kanaNameFamily", 1, 20000202, 1, 0, "bikoFamily", random.Next(999, 99999), false, new() { });
        PtFamily? ptFamily = new PtFamily()
        {
            FamilyId = familyModel.FamilyId,
            HpId = hpId,
            PtId = familyModel.PtId,
        };
        PtInf ptInf = new PtInf()
        {
            HpId = hpId,
            PtId = familyModel.FamilyPtId,
            IsDead = 0
        };
        tenant.PtFamilys.Add(ptFamily);
        tenant.PtInfs.Add(ptInf);
        FamilyRepository familyRepository = new FamilyRepository(TenantProvider);
        try
        {
            tenant.SaveChanges();

            // Act
            bool result = familyRepository.SaveFamilyList(hpId, userId, new() { familyModel });

            // Assert
            var ptFamilyAfter = tenant.PtFamilys.FirstOrDefault(item => item.HpId == hpId
                                                                && item.FamilyId == familyModel.FamilyId
                                                                && item.PtId == familyModel.PtId
                                                                && item.ZokugaraCd == familyModel.ZokugaraCd
                                                                && item.SortNo == familyModel.SortNo
                                                                && item.FamilyPtId == familyModel.FamilyPtId
                                                                && item.KanaName == familyModel.KanaName
                                                                && item.Name == familyModel.Name
                                                                && item.Sex == familyModel.Sex
                                                                && item.Birthday == familyModel.Birthday
                                                                && item.IsDead == familyModel.IsDead
                                                                && item.IsSeparated == familyModel.IsSeparated
                                                                && item.Biko == familyModel.Biko
                                                                && item.IsDeleted == 0);
            if (ptFamilyAfter == null)
            {
                Assert.True(false);
            }

            var ptInfAfter = tenant.PtInfs.FirstOrDefault(item => item.HpId == hpId
                                                                  && item.PtId == ptFamilyAfter!.FamilyPtId
                                                                  && item.IsDead == ptFamilyAfter.IsDead);

            result = result && ptFamilyAfter != null && ptInfAfter != null;

            Assert.True(result);
        }
        finally
        {
            familyRepository.ReleaseResource();

            #region Remove Data Fetch
            if (ptFamily != null)
            {
                tenant.PtFamilys.Remove(ptFamily);
            }
            if (ptInf != null)
            {
                tenant.PtInfs.Remove(ptInf);
            }
            tenant.SaveChanges();
            #endregion
        }
    }
    #endregion SavePregnancyItems
}
