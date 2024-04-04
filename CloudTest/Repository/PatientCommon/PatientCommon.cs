using CloudUnitTest.SampleData;
using Domain.Models.Insurance;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.PatientCommon
{
    public class PatientCommon : BaseUT
    {
        [Test]
        public void TC_001_GetListGetById_ItemData_Null()
        {
            // Arrange
            var tenant = TenantProvider.GetNoTrackingDataContext();

            var ptMemmos = ReadPatientCommon.ReadPtMemo();
            tenant.PtMemos.AddRange(ptMemmos);
            var ptKyuseis = ReadPatientCommon.ReadPtKyusei();
            tenant.PtKyuseis.AddRange(ptKyuseis);
            var raiinInfs = ReadPatientCommon.ReadRainInf(1);
            var firstRaiin = raiinInfs.FirstOrDefault();
            tenant.RaiinInfs.AddRange(raiinInfs);
            var ptCmts = ReadPatientCommon.ReadPtCMT();
            tenant.PtCmtInfs.AddRange(ptCmts);


            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);
            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);

            try
            {
                tenant.SaveChanges();

                // Act
                var resultQuery = patientInforRepository.GetById(1, 999999999, 20250101, firstRaiin?.RaiinNo ?? 0, true, new());

                // Assert
                Assert.True(resultQuery?.HpId == 0 && resultQuery.PtId == 0);
            }
            finally
            {
                receiptRepository.ReleaseResource();
                patientInforRepository.ReleaseResource();
                tenant.PtMemos.RemoveRange(ptMemmos);
                tenant.PtKyuseis.RemoveRange(ptKyuseis);
                tenant.RaiinInfs.RemoveRange(raiinInfs);
                tenant.PtCmtInfs.RemoveRange(ptCmts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_002_GetListGetById_ItemData_NotNull()
        {
            // Arrange
            var tenant = TenantProvider.GetNoTrackingDataContext();
            var ptMemmos = ReadPatientCommon.ReadPtMemo();
            tenant.PtMemos.AddRange(ptMemmos);
            var ptKyuseis = ReadPatientCommon.ReadPtKyusei();
            tenant.PtKyuseis.AddRange(ptKyuseis);
            var raiinInfs = ReadPatientCommon.ReadRainInf(1);
            var firstRaiin = raiinInfs.FirstOrDefault();
            tenant.RaiinInfs.AddRange(raiinInfs);
            var ptCmts = ReadPatientCommon.ReadPtCMT();
            tenant.PtCmtInfs.AddRange(ptCmts);
            var ptInfs = ReadPatientCommon.ReadPtInf();
            tenant.PtInfs.AddRange(ptInfs);

            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);
            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);
            try
            {
                tenant.SaveChanges();

                // Act
                var resultQuery = patientInforRepository.GetById(1, 999999999, 20250101, (firstRaiin?.RaiinNo ?? 0) + 1, true, new() { 4, 5, 6, 7, 8, 9 });

                // Assert
                Assert.True(resultQuery?.HpId == 1 && resultQuery.PtId == 999999999);
            }
            finally
            {
                receiptRepository.ReleaseResource();
                patientInforRepository.ReleaseResource();
                tenant.PtMemos.RemoveRange(ptMemmos);
                tenant.PtKyuseis.RemoveRange(ptKyuseis);
                tenant.RaiinInfs.RemoveRange(raiinInfs);
                tenant.PtCmtInfs.RemoveRange(ptCmts);
                tenant.PtInfs.RemoveRange(ptInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_003_UpdateHokenCheck_AddNew()
        {
            // Arrange
            var tenant = TenantProvider.GetNoTrackingDataContext();
            var ptInfs = ReadPatientCommon.ReadPtInf();
            tenant.PtInfs.AddRange(ptInfs);
            var ptHokenChecks = ReadPatientCommon.ReadPtHokenCheck();
            tenant.PtHokenChecks.AddRange(ptHokenChecks);
            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);
            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);

            try
            {
                tenant.SaveChanges();
                // Act
                var confirmDates = new List<ConfirmDateModel> { new ConfirmDateModel(999999999, 1, 1, DateTime.UtcNow, 1, "Luu Check", 0) };
                patientInforRepository.UpdateHokenCheck(ptHokenChecks, confirmDates, 1, 999999999, 1, 1, true);
                tenant.SaveChanges();

                // Assert
                var hokenCheck = tenant.PtHokenChecks.FirstOrDefault(h => h.PtID == 999999999 && h.HpId == 1 && h.HokenGrp == 1 && h.HokenId == 1 && h.SeqNo == 3);
                Assert.True(hokenCheck != null);
            }
            finally
            {
                receiptRepository.ReleaseResource();
                patientInforRepository.ReleaseResource();
                tenant.PtHokenChecks.RemoveRange(ptHokenChecks);
                tenant.PtInfs.RemoveRange(ptInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_004_UpdateHokenCheck_Delete()
        {
            // Arrange
            var tenant = TenantProvider.GetNoTrackingDataContext();
            var ptInfs = ReadPatientCommon.ReadPtInf();
            tenant.PtInfs.AddRange(ptInfs);
            var ptHokenChecks = ReadPatientCommon.ReadPtHokenCheck();
            tenant.PtHokenChecks.AddRange(ptHokenChecks);
            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);
            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);

            try
            {
                // Act
                tenant.SaveChanges();
                var confirmDates = new List<ConfirmDateModel> { new ConfirmDateModel(999999999, 1, 1, DateTime.UtcNow, 1, "Luu Check", 0) };
                patientInforRepository.UpdateHokenCheck(ptHokenChecks, confirmDates, 1, 999999999, 1, 1, false);
                tenant.SaveChanges();

                //Assert
                Assert.True(ptHokenChecks.Any() && ptHokenChecks.FirstOrDefault()?.IsDeleted == 1);
            }
            finally
            {
                receiptRepository.ReleaseResource();
                patientInforRepository.ReleaseResource();
                tenant.PtHokenChecks.RemoveRange(ptHokenChecks);
                tenant.PtInfs.RemoveRange(ptInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_005_UpdateHokenCheck_Update()
        {
            // Arrange
            var tenant = TenantProvider.GetNoTrackingDataContext();
            var ptInfs = ReadPatientCommon.ReadPtInf();
            tenant.PtInfs.AddRange(ptInfs);
            var ptHokenChecks = ReadPatientCommon.ReadPtHokenCheck();
            tenant.PtHokenChecks.AddRange(ptHokenChecks);
            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);
            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);

            try
            {
                // Act
                tenant.SaveChanges();
                var confirmDates = new List<ConfirmDateModel> { new ConfirmDateModel(999999999, 1, 1, DateTime.UtcNow, 1, "Luu Check", 3) };
                patientInforRepository.UpdateHokenCheck(ptHokenChecks, confirmDates, 1, 999999999, 1, 1, false);
                tenant.SaveChanges();

                // Assert
                Assert.True(ptHokenChecks.Any() && ptHokenChecks.FirstOrDefault()?.CheckCmt == "Luu Check");
            }
            finally
            {
                receiptRepository.ReleaseResource();
                patientInforRepository.ReleaseResource();
                tenant.PtHokenChecks.RemoveRange(ptHokenChecks);
                tenant.PtInfs.RemoveRange(ptInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_006_CloneByomeiWithNewHokenId()
        {
            // Arrange
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = ReadPatientCommon.ReadPtInf();
            tenant.PtInfs.AddRange(ptInfs);
            var ptByomeis = ReadPatientCommon.ReadPtByomei();
            tenant.PtByomeis.AddRange(ptByomeis);
            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);
            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);

            try
            {
                // Act
                tenant.SaveChanges();
                var result = patientInforRepository.CloneByomeiWithNewHokenId(ptByomeis, 2, 2);

                // Assert
                Assert.True(result);
            }
            finally
            {
                receiptRepository.ReleaseResource();
                patientInforRepository.ReleaseResource();
                tenant.PtByomeis.RemoveRange(ptByomeis);
                tenant.PtInfs.RemoveRange(ptInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_007_FindSamePatient()
        {
            // Arrange
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = ReadPatientCommon.ReadPtInf();
            tenant.PtInfs.AddRange(ptInfs);
            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);
            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);
            try
            {
                // Act
                tenant.SaveChanges();
                var result = patientInforRepository.FindSamePatient(1, "quang anh", 1, 20020101);
                var result1 = patientInforRepository.FindSamePatient(1, "quang anh 12345", 1, 20000101);

                // Assert
                Assert.True(result.Any() && !result1.Any());
            }
            finally
            {
                receiptRepository.ReleaseResource();
                patientInforRepository.ReleaseResource();
                tenant.PtInfs.RemoveRange(ptInfs);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_008_GetPtByomeisByHokenId_ByomeiCd()
        {
            // Arrange
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = ReadPatientCommon.ReadPtInf();
            tenant.PtInfs.AddRange(ptInfs);
            var byomeiMsts = ReadPatientCommon.ReadByomeiMst("Luu0008");
            tenant.ByomeiMsts.AddRange(byomeiMsts);
            var ptByomeis = ReadPatientCommon.ReadPtByomei();
            tenant.PtByomeis.AddRange(ptByomeis);
            DiseaseRepository diseaseRepository = new DiseaseRepository(TenantProvider);

            try
            {
                // Act
                tenant.SaveChanges();
                var result = diseaseRepository.GetPtByomeisByHokenId(1, 999999999, 1);

                // Assert
                Assert.True(result.Any() && !string.IsNullOrEmpty(result.FirstOrDefault()?.Icd10) && !string.IsNullOrEmpty(result.FirstOrDefault()?.Icd1012013) && !string.IsNullOrEmpty(result.FirstOrDefault()?.Icd102013) && !string.IsNullOrEmpty(result.FirstOrDefault()?.Icd1022013));
            }
            finally
            {
                diseaseRepository.ReleaseResource();
                tenant.PtByomeis.RemoveRange(ptByomeis);
                tenant.PtInfs.RemoveRange(ptInfs);
                tenant.ByomeiMsts.RemoveRange(byomeiMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_009_GetPtByomeisByHokenId_FreeComment()
        {
            // Arrange
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            var ptInfs = ReadPatientCommon.ReadPtInf();
            tenant.PtInfs.AddRange(ptInfs);
            var byomeiMsts = ReadPatientCommon.ReadByomeiMst("0000999");
            var first = byomeiMsts.FirstOrDefault();
            var check = tenant.ByomeiMsts.FirstOrDefault(b => (first != null && b.HpId == first.HpId) && b.ByomeiCd == "0000999");
            if (check == null)
            {
                tenant.ByomeiMsts.AddRange(byomeiMsts);
            }
            var ptByomeis = ReadPatientCommon.ReadPtByomei("0000999");
            tenant.PtByomeis.AddRange(ptByomeis);
            DiseaseRepository diseaseRepository = new DiseaseRepository(TenantProvider);

            try
            {
                // Act
                tenant.SaveChanges();
                var result = diseaseRepository.GetPtByomeisByHokenId(1, 999999999, 1);

                // Assert
                Assert.True(result.Any() && ptByomeis.FirstOrDefault()?.Byomei == result.FirstOrDefault()?.Byomei);
            }
            finally
            {
                diseaseRepository.ReleaseResource();
                tenant.PtByomeis.RemoveRange(ptByomeis);
                tenant.PtInfs.RemoveRange(ptInfs);
                if (check == null) tenant.ByomeiMsts.RemoveRange(byomeiMsts);
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_010_GetAutoPtNum_SystemConf()
        {
            // Arrange
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            double tempValue = 0;
            long ptNum = 0;
            var systemConfs = ReadPatientCommon.ReadSystemConf();
            var first = systemConfs.FirstOrDefault();
            var check = tenant.SystemConfs.FirstOrDefault(s => (first != null && s.HpId == first.HpId) && s.GrpCd == 1014 && s.GrpEdaNo == 1);
            if (check == null)
            {
                ptNum = (long)(first?.Val ?? 0);
                tenant.SystemConfs.AddRange(systemConfs);
            }
            else
            {
                tempValue = check.Val;
                ptNum = 1000;
                check.Val = ptNum;
            }
            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);
            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);

            try
            {
                // Act
                tenant.SaveChanges();
                var result = patientInforRepository.GetAutoPtNum(1);

                // Assert
                Assert.True(result > 0);
            }
            finally
            {
                patientInforRepository.ReleaseResource();
                if (check == null) tenant.SystemConfs.RemoveRange(systemConfs);
                else check.Val = tempValue;
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_011_GetAutoPtNumAction_SystemConf_1001_1()
        {
            // Arrange
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            long ptNum = 0;
            double tempValue = 0, tempValue1001 = 0;
            var systemConfs = ReadPatientCommon.ReadSystemConf();
            var first = systemConfs.FirstOrDefault();
            var check = tenant.SystemConfs.FirstOrDefault(s => (first != null && s.HpId == first.HpId) && s.GrpCd == 1014 && s.GrpEdaNo == 0);
            if (check == null)
            {
                ptNum = (long)(first?.Val ?? 0);
                tenant.SystemConfs.Add(first ?? new());
            }
            else
            {
                tempValue = check.Val;
                ptNum = 1000;
                check.Val = ptNum;
            }
            var second = systemConfs.LastOrDefault();
            var checkSystem1001 = tenant.SystemConfs.FirstOrDefault(s => (first != null && s.HpId == first.HpId) && s.GrpCd == 1001 && s.GrpEdaNo == 0);
            if (checkSystem1001 == null)
            {
                tenant.SystemConfs.Add(second ?? new());
            }
            else
            {
                if (second != null)
                {
                    tempValue1001 = checkSystem1001.Val;
                    checkSystem1001.Val = 1;
                }
            }
            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);
            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);

            try
            {
                // Act
                tenant.SaveChanges();
                var result = patientInforRepository.GetAutoPtNum(1);

                // Assert
                Assert.True(result > 0);
            }
            finally
            {
                patientInforRepository.ReleaseResource();
                if (check == null) tenant.SystemConfs.Remove(first ?? new());
                else check.Val = tempValue;
                if (checkSystem1001 == null) tenant.SystemConfs.Remove(second ?? new());
                else checkSystem1001.Val = tempValue1001;
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_012_GetAutoPtNumAction_SystemConf_1001_0_Calculate_PtNumber()
        {
            // Arrange
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            long ptNum = 0;
            double tempValue = 0, tempValue1001 = 0;
            var systemConfs = ReadPatientCommon.ReadSystemConf();
            var first = systemConfs.FirstOrDefault();
            var check = tenant.SystemConfs.FirstOrDefault(s => (first != null && s.HpId == first.HpId) && s.GrpCd == 1014 && s.GrpEdaNo == 0);
            if (check == null)
            {
                ptNum = (long)(first?.Val ?? 0);
                tenant.SystemConfs.Add(first ?? new());
            }
            else
            {
                tempValue = check.Val;
                ptNum = 1000;
                check.Val = ptNum;
            }
            var second = systemConfs.LastOrDefault();
            var checkSystem1001 = tenant.SystemConfs.FirstOrDefault(s => (first != null && s.HpId == first.HpId) && s.GrpCd == 1001 && s.GrpEdaNo == 0);
            if (checkSystem1001 == null)
            {
                if (second != null)
                {
                    second.Val = 0;
                }
                tenant.SystemConfs.Add(second ?? new());
            }
            else
            {
                if (second != null)
                {
                    tempValue1001 = checkSystem1001.Val;
                    checkSystem1001.Val = 0;
                }
            }
            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);
            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);

            try
            {
                // Act
                tenant.SaveChanges();
                var result = patientInforRepository.GetAutoPtNum(1);

                // Assert
                Assert.True(result > 0);
            }
            finally
            {
                patientInforRepository.ReleaseResource();
                if (check == null) tenant.SystemConfs.Remove(first ?? new());
                else check.Val = tempValue;
                if (checkSystem1001 == null) tenant.SystemConfs.Remove(second ?? new());
                else checkSystem1001.Val = tempValue1001;
                tenant.SaveChanges();
            }
        }

        [Test]
        public void TC_013_GetAutoPtNumAction_SystemConf_1001_0_Not_Calculate_PtNumber()
        {
            // Arrange
            var tenant = TenantProvider.GetTrackingTenantDataContext();
            long ptNum = 1000000000;
            double tempValue = 0, tempValue1001 = 0, tempValue10141 = 0;
            var systemConfs = ReadPatientCommon.ReadSystemConf();
            var first = systemConfs.FirstOrDefault();
            var check = tenant.SystemConfs.FirstOrDefault(s => (first != null && s.HpId == first.HpId) && s.GrpCd == 1014 && s.GrpEdaNo == 0);
            if (check == null)
            {
                if (first != null)
                {
                    first.Val = 1;
                }
                tenant.SystemConfs.Add(first ?? new());
            }
            else
            {
                tempValue = check.Val;
                check.Val = 1;
            }
            var check10141 = tenant.SystemConfs.FirstOrDefault(s => (first != null && s.HpId == first.HpId) && s.GrpCd == 1014 && s.GrpEdaNo == 1);
            if (check10141 == null)
            {
                if (first != null)
                {
                    first.Val = ptNum;
                    first.GrpCd = 1;
                }
                tenant.SystemConfs.Add(first ?? new());
            }
            else
            {
                tempValue10141 = check10141.Val;
                check10141.Val = ptNum;
            }

            var second = systemConfs.LastOrDefault();
            var checkSystem1001 = tenant.SystemConfs.FirstOrDefault(s => (first != null && s.HpId == first.HpId) && s.GrpCd == 1001 && s.GrpEdaNo == 0);
            if (checkSystem1001 == null)
            {
                if (second != null)
                {
                    second.Val = 0;
                }
                tenant.SystemConfs.Add(second ?? new());
            }
            else
            {
                if (second != null)
                {
                    tempValue1001 = checkSystem1001.Val;
                    checkSystem1001.Val = 0;
                }
            }
            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);
            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);

            try
            {
                // Act
                tenant.SaveChanges();
                var result = patientInforRepository.GetAutoPtNum(1);

                // Assert
                Assert.True(result > 0);
            }
            finally
            {
                patientInforRepository.ReleaseResource();
                if (check10141 == null) tenant.SystemConfs.Remove(first ?? new());
                else check10141.Val = tempValue10141;
                if (check == null)
                {
                    if (first != null)
                    {
                        first.GrpCd = 0;
                        first.Val = 1;
                    }
                    tenant.SystemConfs.Remove(first ?? new());
                }
                else
                {
                    check.Val = tempValue;
                }
                if (checkSystem1001 == null) tenant.SystemConfs.Remove(second ?? new());
                else checkSystem1001.Val = tempValue1001;
                tenant.SaveChanges();
            }
        }

    }
}
