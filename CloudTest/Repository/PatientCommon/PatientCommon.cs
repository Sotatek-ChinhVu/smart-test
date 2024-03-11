using CloudUnitTest.SampleData;
using Domain.Models.Insurance;
using Entity.Tenant;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudUnitTest.Repository.PatientCommon
{
    public class PatientCommon : BaseUT
    {
        [Test]
        public void GetListGetById_ItemData_Null_001()
        {
            #region Fetch data
            var tenant = TenantProvider.GetNoTrackingDataContext();

            // PtMemo
            var ptMemmos = ReadPatientCommon.ReadPtMemo();
            tenant.PtMemos.AddRange(ptMemmos);

            // PtKyusei
            var ptKyuseis = ReadPatientCommon.ReadPtKyusei();
            tenant.PtKyuseis.AddRange(ptKyuseis);

            // RaiinInf
            var raiinInfs = ReadPatientCommon.ReadRainInf(1);
            var firstRaiin = raiinInfs.FirstOrDefault();
            tenant.RaiinInfs.AddRange(raiinInfs);

            // PtCmt
            var ptCmts = ReadPatientCommon.ReadPtCMT();
            tenant.PtCmtInfs.AddRange(ptCmts);

            #endregion

            // Arrange
            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);

            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);
            // Assert
            try
            {
                tenant.SaveChanges();

             
                var resultQuery = patientInforRepository.GetById(1, 999999999, 20250101, firstRaiin?.RaiinNo ?? 0, true, new ());

                Assert.True(resultQuery.HpId == 0 && resultQuery.PtId == 0);
            }
            finally
            {
                #region Remove Data Fetch
                receiptRepository.ReleaseResource();
                patientInforRepository.ReleaseResource();
                tenant.PtMemos.RemoveRange(ptMemmos);
                tenant.PtKyuseis.RemoveRange(ptKyuseis);
                tenant.RaiinInfs.RemoveRange(raiinInfs);
                tenant.PtCmtInfs.RemoveRange(ptCmts);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void GetListGetById_ItemData_NotNull_002()
        {
            #region Fetch data
            var tenant = TenantProvider.GetNoTrackingDataContext();

            // PtMemo
            var ptMemmos = ReadPatientCommon.ReadPtMemo();
            tenant.PtMemos.AddRange(ptMemmos);

            // PtKyusei
            var ptKyuseis = ReadPatientCommon.ReadPtKyusei();
            tenant.PtKyuseis.AddRange(ptKyuseis);

            // RaiinInf
            var raiinInfs = ReadPatientCommon.ReadRainInf(1);
            var firstRaiin = raiinInfs.FirstOrDefault();
            tenant.RaiinInfs.AddRange(raiinInfs);

            // PtCmt
            var ptCmts = ReadPatientCommon.ReadPtCMT();
            tenant.PtCmtInfs.AddRange(ptCmts);

            var ptInfs = ReadPatientCommon.ReadPtInf();
            tenant.PtInfs.AddRange(ptInfs);

            #endregion

            // Arrange
            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);

            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);
            // Assert
            try
            {
                tenant.SaveChanges();


                var resultQuery = patientInforRepository.GetById(1, 999999999, 20250101, (firstRaiin?.RaiinNo ?? 0) + 1, true, new() { 4, 5, 6, 7, 8, 9});

                Assert.True(resultQuery.HpId == 1 && resultQuery.PtId == 999999999);
            }
            finally
            {
                #region Remove Data Fetch
                receiptRepository.ReleaseResource();
                patientInforRepository.ReleaseResource();
                tenant.PtMemos.RemoveRange(ptMemmos);
                tenant.PtKyuseis.RemoveRange(ptKyuseis);
                tenant.RaiinInfs.RemoveRange(raiinInfs);
                tenant.PtCmtInfs.RemoveRange(ptCmts);
                tenant.PtInfs.RemoveRange(ptInfs);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void UpdateHokenCheck_AddNew_003()
        {
            #region Fetch data
            var tenant = TenantProvider.GetNoTrackingDataContext();

            //PtInf
            var ptInfs = ReadPatientCommon.ReadPtInf();
            tenant.PtInfs.AddRange(ptInfs);

            // PtHokenCheck
            var ptHokenChecks = ReadPatientCommon.ReadPtHokenCheck();
            tenant.PtHokenChecks.AddRange(ptHokenChecks);

            #endregion

            // Arrange
            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);

            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);

            // Assert
            try
            {
                tenant.SaveChanges();
                var confirmDates = new List < ConfirmDateModel> { new ConfirmDateModel(999999999, 1, 1, DateTime.UtcNow, 1, "Luu Check", 0) };
                patientInforRepository.UpdateHokenCheck(ptHokenChecks, confirmDates, 1, 999999999, 1, 1, true);
                tenant.SaveChanges();
                var hokenCheck = tenant.PtHokenChecks.FirstOrDefault(h => h.PtID == 999999999 && h.HpId == 1 && h.HokenGrp == 1 && h.HokenId == 1 && h.SeqNo == 3);
                Assert.True(hokenCheck != null);
            }
            finally
            {
                #region Remove Data Fetch
                receiptRepository.ReleaseResource();
                patientInforRepository.ReleaseResource();
                tenant.PtHokenChecks.RemoveRange(ptHokenChecks);
                tenant.PtInfs.RemoveRange(ptInfs);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void UpdateHokenCheck_Delete_004()
        {
            #region Fetch data
            var tenant = TenantProvider.GetNoTrackingDataContext();

            //PtInf
            var ptInfs = ReadPatientCommon.ReadPtInf();
            tenant.PtInfs.AddRange(ptInfs);

            // PtHokenCheck
            var ptHokenChecks = ReadPatientCommon.ReadPtHokenCheck();
            tenant.PtHokenChecks.AddRange(ptHokenChecks);

            #endregion

            // Arrange
            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);

            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);

            // Assert
            try
            {
                tenant.SaveChanges();
                var confirmDates = new List<ConfirmDateModel> { new ConfirmDateModel(999999999, 1, 1, DateTime.UtcNow, 1, "Luu Check", 0) };
                patientInforRepository.UpdateHokenCheck(ptHokenChecks, confirmDates, 1, 999999999, 1, 1, false);
                tenant.SaveChanges();
                Assert.True(ptHokenChecks.Any() && ptHokenChecks.FirstOrDefault()?.IsDeleted == 1);
            }
            finally
            {
                #region Remove Data Fetch
                receiptRepository.ReleaseResource();
                patientInforRepository.ReleaseResource();
                tenant.PtHokenChecks.RemoveRange(ptHokenChecks);
                tenant.PtInfs.RemoveRange(ptInfs);
                tenant.SaveChanges();
                #endregion
            }
        }


        [Test]
        public void UpdateHokenCheck_Update_005()
        {
            #region Fetch data
            var tenant = TenantProvider.GetNoTrackingDataContext();

            //PtInf
            var ptInfs = ReadPatientCommon.ReadPtInf();
            tenant.PtInfs.AddRange(ptInfs);

            // PtHokenCheck
            var ptHokenChecks = ReadPatientCommon.ReadPtHokenCheck();
            tenant.PtHokenChecks.AddRange(ptHokenChecks);

            #endregion

            // Arrange
            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);

            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);

            // Assert
            try
            {
                tenant.SaveChanges();
                var confirmDates = new List<ConfirmDateModel> { new ConfirmDateModel(999999999, 1, 1, DateTime.UtcNow, 1, "Luu Check", 3) };
                patientInforRepository.UpdateHokenCheck(ptHokenChecks, confirmDates, 1, 999999999, 1, 1, false);
                tenant.SaveChanges();
                Assert.True(ptHokenChecks.Any() && ptHokenChecks.FirstOrDefault()?.CheckCmt == "Luu Check");
            }
            finally
            {
                #region Remove Data Fetch
                receiptRepository.ReleaseResource();
                patientInforRepository.ReleaseResource();
                tenant.PtHokenChecks.RemoveRange(ptHokenChecks);
                tenant.PtInfs.RemoveRange(ptInfs);
                tenant.SaveChanges();
                #endregion
            }
        }
        
        [Test]
        public void CloneByomeiWithNewHokenId_006()
        {
            #region Fetch data
            var tenant = TenantProvider.GetTrackingTenantDataContext();

            //PtInf
            var ptInfs = ReadPatientCommon.ReadPtInf();
            tenant.PtInfs.AddRange(ptInfs);

            // PtByomei
            var ptByomeis = ReadPatientCommon.ReadPtByomei();
            tenant.PtByomeis.AddRange(ptByomeis);

            #endregion

            // Arrange
            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);

            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);

            // Assert
            try
            {
                tenant.SaveChanges();
               
                var result = patientInforRepository.CloneByomeiWithNewHokenId(ptByomeis,2, 2);

                Assert.True(result);
            }
            finally
            {
                #region Remove Data Fetch
                receiptRepository.ReleaseResource();
                patientInforRepository.ReleaseResource();
                tenant.PtByomeis.RemoveRange(ptByomeis);
                tenant.PtInfs.RemoveRange(ptInfs);
                tenant.SaveChanges();
                #endregion
            }
        } 
        
        [Test]
        public void FindSamePatient_007()
        {
            #region Fetch data
            var tenant = TenantProvider.GetTrackingTenantDataContext();

            //PtInf
            var ptInfs = ReadPatientCommon.ReadPtInf();
            tenant.PtInfs.AddRange(ptInfs);

            #endregion

            // Arrange
            ReceptionRepository receiptRepository = new ReceptionRepository(TenantProvider);

            PatientInforRepository patientInforRepository = new PatientInforRepository(TenantProvider, receiptRepository);

            // Assert
            try
            {
                tenant.SaveChanges();
               
                var result = patientInforRepository.FindSamePatient(1, "quang anh", 1, 20020101);
                var result1 = patientInforRepository.FindSamePatient(1, "quang anh 12345", 1, 20000101);

                Assert.True(result.Any() && !result1.Any());
            }
            finally
            {
                #region Remove Data Fetch
                receiptRepository.ReleaseResource();
                patientInforRepository.ReleaseResource();
                tenant.PtInfs.RemoveRange(ptInfs);
                tenant.SaveChanges();
                #endregion
            }
        }

    }
}
