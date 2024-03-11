using CloudUnitTest.SampleData;
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

    }
}
