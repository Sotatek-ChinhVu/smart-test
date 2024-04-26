using Domain.Models.CalculationInf;
using Domain.Models.GroupInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.InsuranceMst;
using Domain.Models.MaxMoney;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Repositories;
using Moq;
using System.Text;

namespace CloudUnitTest.Repository.PatientInfo
{
    public class SavePatientInforRepositoryTest : BaseUT
    {
        [Test]
        public void TC_001_CreatePatientInfo_PtNum_Is_0()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 0,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            (bool resultSave, long ptId) createPtInf = new();
            PtInf? ptInf = null;
            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), new(), new(), new(), new(), new(), new(), insuranceScanModel, 9999);

                // Assert
                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                    tenantTracking.SaveChanges();
                }
            }
        }

        [Test]
        public void TC_002_CreatePatientInfo_PtNum_Is_Greater_Than_0()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            (bool resultSave, long ptId) createPtInf = new();
            PtInf? ptInf = null;
            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), new(), new(), new(), new(), new(), new(), insuranceScanModel, 9999);

                // Assert
                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(0));
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                    tenantTracking.SaveChanges();
                }
            }
        }

        [Test]
        public void TC_003_CreatePatientInfo_DeathDate_Is_Greater_Than_0()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            (bool resultSave, long ptId) createPtInf = new();
            PtInf? ptInf = null;
            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), new(), new(), new(), new(), new(), new(), insuranceScanModel, 9999);

                // Assert
                ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(0));
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                    tenantTracking.SaveChanges();
                }
            }
        }

        [Test]
        public void TC_004_CreatePatientInfo_Save_PtSanteis()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            (bool resultSave, long ptId) createPtInf = new();
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), new(), new(), insuranceScanModel, 9999);

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(0));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
            }
            finally
            {
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_005_CreatePatientInfo_Save_PtMemos()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Memo Test"
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            (bool resultSave, long ptId) createPtInf = new();
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            PtMemo? memo = null;
            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), new(), new(), insuranceScanModel, 9999);

                // Assert
                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                memo = tenantTracking.PtMemos.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(0));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(memo?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(memo?.HpId, Is.EqualTo(1));
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (memo != null)
                {
                    tenantTracking.PtMemos.Remove(memo);
                }
                tenantTracking.SaveChanges();

            }
        }

        [Test]
        public void TC_006_CreatePatientInfo_Save_PtGrps()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Memo Test"
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            var ptGrps = new List<GroupInfModel>()
            {
                new GroupInfModel(1, 98422233, 1, "1234", "1234"),
            };

            (bool resultSave, long ptId) createPtInf = new();
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            PtMemo? memo = null;
            PtGrpInf? ptGrp = null;
            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), ptGrps, new(), insuranceScanModel, 9999);

                // Assert
                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                memo = tenantTracking.PtMemos.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptGrp = tenantTracking.PtGrpInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(0));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(memo?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(memo?.HpId, Is.EqualTo(1));
                Assert.That(ptGrp?.HpId, Is.EqualTo(1));
                Assert.That(ptGrp?.PtId, Is.EqualTo(createPtInf.ptId));
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (memo != null)
                {
                    tenantTracking.PtMemos.Remove(memo);
                }
                if (ptGrp != null)
                {
                    tenantTracking.PtGrpInfs.Remove(ptGrp);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_007_CreatePatientInfo_Save_PtKyuseis()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Memo Test"
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            var ptGrps = new List<GroupInfModel>()
            {
                new GroupInfModel(1, 98422233, 1, "1234", "1234"),
            };

            var ptKyuseis = new List<PtKyuseiModel>()
            {
                new PtKyuseiModel(1, 98422233, 1, "Sample KanaName", "Name", 20230101)
            };

            (bool resultSave, long ptId) createPtInf = new();
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            PtMemo? memo = null;
            PtGrpInf? ptGrp = null;
            PtKyusei? ptKyusei = null;

            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, ptKyuseis, ptSantei, new(), new(), new(), ptGrps, new(), insuranceScanModel, 9999);

                // Assert
                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                memo = tenantTracking.PtMemos.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptGrp = tenantTracking.PtGrpInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptKyusei = tenantTracking.PtKyuseis.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(0));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(memo?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(memo?.HpId, Is.EqualTo(1));
                Assert.That(ptGrp?.HpId, Is.EqualTo(1));
                Assert.That(ptGrp?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKyusei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKyusei?.HpId, Is.EqualTo(1));
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (memo != null)
                {
                    tenantTracking.PtMemos.Remove(memo);
                }
                if (ptGrp != null)
                {
                    tenantTracking.PtGrpInfs.Remove(ptGrp);
                }
                if (ptKyusei != null)
                {
                    tenantTracking.PtKyuseis.Remove(ptKyusei);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_008_CreatePatientInfo_Save_PthokenPartterns()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Memo Test"
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            var ptGrps = new List<GroupInfModel>()
            {
                new GroupInfModel(1, 98422233, 1, "1234", "1234"),
            };

            var ptKyuseis = new List<PtKyuseiModel>()
            {
                new PtKyuseiModel(1, 98422233, 1, "Sample KanaName", "Name", 20230101)
            };

            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 9999, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };

            (bool resultSave, long ptId) createPtInf = new();
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            PtMemo? memo = null;
            PtGrpInf? ptGrp = null;
            PtKyusei? ptKyusei = null;
            PtHokenPattern? ptHokenPattern = null;

            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, ptKyuseis, ptSantei, insurances, new(), new(), ptGrps, new(), insuranceScanModel, 9999);

                // Assert
                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                memo = tenantTracking.PtMemos.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptGrp = tenantTracking.PtGrpInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptKyusei = tenantTracking.PtKyuseis.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptHokenPattern = tenantTracking.PtHokenPatterns.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(0));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(memo?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(memo?.HpId, Is.EqualTo(1));
                Assert.That(ptGrp?.HpId, Is.EqualTo(1));
                Assert.That(ptGrp?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKyusei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKyusei?.HpId, Is.EqualTo(1));
                Assert.That(ptHokenPattern?.HpId, Is.EqualTo(1));
                Assert.That(ptHokenPattern?.PtId, Is.EqualTo(createPtInf.ptId));
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (memo != null)
                {
                    tenantTracking.PtMemos.Remove(memo);
                }
                if (ptGrp != null)
                {
                    tenantTracking.PtGrpInfs.Remove(ptGrp);
                }
                if (ptKyusei != null)
                {
                    tenantTracking.PtKyuseis.Remove(ptKyusei);
                }
                if (ptHokenPattern != null)
                {
                    tenantTracking.PtHokenPatterns.Remove(ptHokenPattern);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_009_CreatePatientInfo_Save_PtHokenInfs()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Memo Test"
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            var ptGrps = new List<GroupInfModel>()
            {
                new GroupInfModel(1, 98422233, 1, "1234", "1234"),
            };

            var ptKyuseis = new List<PtKyuseiModel>()
            {
                new PtKyuseiModel(1, 98422233, 1, "Sample KanaName", "Name", 20230101)
            };

            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 9999, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "11",
                                  tokki2: "22",
                                  tokki3: "33",
                                  tokki4: "44",
                                  tokki5: "55",
                                  rousaiKofuNo: "RousaiKofu",
                                  rousaiRoudouCd: "R1",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "rk",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "rs",
                                  rousaiJigyosyoName: "rj",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "ABC",
                                  confirmDateList: new List<ConfirmDateModel>()
                                                      {
                                                        new ConfirmDateModel(123456, 1, 50, DateTime.UtcNow, 1, "Check Comment"),
                                                      },
                                  listRousaiTenki: new List<RousaiTenkiModel>()
                                                      {
                                                        new RousaiTenkiModel(1, 2, 20240101, 3, 99999),
                                                      },
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };

            (bool resultSave, long ptId) createPtInf = new();
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            PtMemo? memo = null;
            PtGrpInf? ptGrp = null;
            PtKyusei? ptKyusei = null;
            PtHokenPattern? ptHokenPattern = null;
            PtHokenInf? pthokenInf = null;
            PtRousaiTenki? ptRousaiTenkis = null;
            PtHokenCheck? ptHokenChecks = null;
            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, ptKyuseis, ptSantei, insurances, hokenInfs, new(), ptGrps, new(), insuranceScanModel, 9999);

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                memo = tenantTracking.PtMemos.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptGrp = tenantTracking.PtGrpInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptKyusei = tenantTracking.PtKyuseis.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptHokenPattern = tenantTracking.PtHokenPatterns.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                pthokenInf = tenantTracking.PtHokenInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptRousaiTenkis = tenantTracking.PtRousaiTenkis.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId && x.HokenId == 789012);
                ptHokenChecks = tenantTracking.PtHokenChecks.FirstOrDefault(x => x.HpId == 1 && x.HokenId == 789012 && x.PtID == createPtInf.ptId);

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(0));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(memo?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(memo?.HpId, Is.EqualTo(1));
                Assert.That(ptGrp?.HpId, Is.EqualTo(1));
                Assert.That(ptGrp?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKyusei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKyusei?.HpId, Is.EqualTo(1));
                Assert.That(ptHokenPattern?.HpId, Is.EqualTo(1));
                Assert.That(ptHokenPattern?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(pthokenInf?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(pthokenInf?.EndDate, Is.EqualTo(20221231));
                Assert.That(ptRousaiTenkis?.Tenki, Is.EqualTo(2));
                Assert.That(ptRousaiTenkis?.Sinkei, Is.EqualTo(1));
                Assert.That(ptRousaiTenkis?.HokenId, Is.EqualTo(789012));
                Assert.That(ptHokenChecks?.HokenId, Is.EqualTo(789012));
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (memo != null)
                {
                    tenantTracking.PtMemos.Remove(memo);
                }
                if (ptGrp != null)
                {
                    tenantTracking.PtGrpInfs.Remove(ptGrp);
                }
                if (ptKyusei != null)
                {
                    tenantTracking.PtKyuseis.Remove(ptKyusei);
                }
                if (ptHokenPattern != null)
                {
                    tenantTracking.PtHokenPatterns.Remove(ptHokenPattern);
                }
                if (pthokenInf != null)
                {
                    tenantTracking.PtHokenInfs.Remove(pthokenInf);
                }
                if (ptRousaiTenkis != null)
                {
                    tenantTracking.PtRousaiTenkis.Remove(ptRousaiTenkis);
                }
                if (ptHokenChecks != null)
                {
                    tenantTracking.PtHokenChecks.Remove(ptHokenChecks);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_010_CreatePatientInfo_Save_PtKohiInf()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Memo Test"
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            var ptGrps = new List<GroupInfModel>()
            {
                new GroupInfModel(1, 98422233, 1, "1234", "1234"),
            };

            var ptKyuseis = new List<PtKyuseiModel>()
            {
                new PtKyuseiModel(1, 98422233, 1, "Sample KanaName", "Name", 20230101)
            };

            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 9999, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "11",
                                  tokki2: "22",
                                  tokki3: "33",
                                  tokki4: "44",
                                  tokki5: "55",
                                  rousaiKofuNo: "RousaiKofu",
                                  rousaiRoudouCd: "R1",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "rk",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "rs",
                                  rousaiJigyosyoName: "rj",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "ABC",
                                  confirmDateList: new List<ConfirmDateModel>()
                                                      {
                                                        new ConfirmDateModel(123456, 1, 50, DateTime.UtcNow, 1, "Check Comment"),
                                                      },
                                  listRousaiTenki: new List<RousaiTenkiModel>()
                                                      {
                                                        new RousaiTenkiModel(1, 2, 20240101, 3, 99999),
                                                      },
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };

            var hokenKohis = new List<KohiInfModel>()
            {
                new KohiInfModel(futansyaNo: "123456",
                                 jyukyusyaNo:"234567",
                                 hokenId: 61,
                                 startDate: 20230101,
                                 endDate: 20240101,
                                 confirmDate: 20231212,
                                 rate: 3,
                                 gendoGaku: 5,
                                 sikakuDate: 20230506,
                                 kofuDate: 20230505,
                                 tokusyuNo: "33444",
                                 hokenSbtKbn: 55,
                                 houbetu: "123",
                                 hokenNo: 55,
                                 hokenEdaNo: 23,
                                 prefNo: 21,
                                 new(),
                                 sinDate: 20240101,
                                 confirmDateList: new List<ConfirmDateModel>()
                                                      {
                                                        new ConfirmDateModel(123456, 1, 50, DateTime.UtcNow, 1, "Check Comment"),
                                                      },
                                 true,
                                 0,
                                 isAddNew: true,
                                 seqNo:0
                                 ),
            };
            (bool resultSave, long ptId) createPtInf = new();
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            PtMemo? memo = null;
            PtGrpInf? ptGrp = null;
            PtKyusei? ptKyusei = null;
            PtHokenPattern? ptHokenPattern = null;
            PtHokenInf? pthokenInf = null;
            PtRousaiTenki? ptRousaiTenkis = null;
            PtHokenCheck? ptHokenChecks = null;
            PtHokenCheck? ptHokenCheckKohis = null;
            PtKohi? ptKohis = null;

            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, ptKyuseis, ptSantei, insurances, hokenInfs, hokenKohis, ptGrps, new(), insuranceScanModel, 9999);

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                memo = tenantTracking.PtMemos.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptGrp = tenantTracking.PtGrpInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptKyusei = tenantTracking.PtKyuseis.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptHokenPattern = tenantTracking.PtHokenPatterns.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                pthokenInf = tenantTracking.PtHokenInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptRousaiTenkis = tenantTracking.PtRousaiTenkis.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId && x.HokenId == 789012);
                ptHokenChecks = tenantTracking.PtHokenChecks.FirstOrDefault(x => x.HpId == 1 && x.HokenId == 789012 && x.PtID == createPtInf.ptId && x.HokenGrp == 1);
                ptHokenCheckKohis = tenantTracking.PtHokenChecks.FirstOrDefault(x => x.HpId == 1 && x.HokenId == 61 && x.PtID == createPtInf.ptId && x.HokenGrp == 2);
                ptKohis = tenantTracking.PtKohis.FirstOrDefault(x => x.HpId == 1 && x.HokenId == 61 && x.PtId == createPtInf.ptId);

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(0));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(memo?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(memo?.HpId, Is.EqualTo(1));
                Assert.That(ptGrp?.HpId, Is.EqualTo(1));
                Assert.That(ptGrp?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKyusei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKyusei?.HpId, Is.EqualTo(1));
                Assert.That(ptHokenPattern?.HpId, Is.EqualTo(1));
                Assert.That(ptHokenPattern?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(pthokenInf?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(pthokenInf?.EndDate, Is.EqualTo(20221231));
                Assert.That(ptRousaiTenkis?.Tenki, Is.EqualTo(2));
                Assert.That(ptRousaiTenkis?.Sinkei, Is.EqualTo(1));
                Assert.That(ptRousaiTenkis?.HokenId, Is.EqualTo(789012));
                Assert.That(ptHokenCheckKohis?.HokenId, Is.EqualTo(61));
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (memo != null)
                {
                    tenantTracking.PtMemos.Remove(memo);
                }
                if (ptGrp != null)
                {
                    tenantTracking.PtGrpInfs.Remove(ptGrp);
                }
                if (ptKyusei != null)
                {
                    tenantTracking.PtKyuseis.Remove(ptKyusei);
                }
                if (ptHokenPattern != null)
                {
                    tenantTracking.PtHokenPatterns.Remove(ptHokenPattern);
                }
                if (pthokenInf != null)
                {
                    tenantTracking.PtHokenInfs.Remove(pthokenInf);
                }
                if (ptRousaiTenkis != null)
                {
                    tenantTracking.PtRousaiTenkis.Remove(ptRousaiTenkis);
                }
                if (ptHokenChecks != null)
                {
                    tenantTracking.PtHokenChecks.Remove(ptHokenChecks);
                }
                if (ptHokenCheckKohis != null)
                {
                    tenantTracking.PtHokenChecks.Remove(ptHokenCheckKohis);
                }
                if (ptKohis != null)
                {
                    tenantTracking.PtKohis.Remove(ptKohis);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_011_CreatePatientInfo_Save_Maxmoney()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Memo Test"
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            var ptGrps = new List<GroupInfModel>()
            {
                new GroupInfModel(1, 98422233, 1, "1234", "1234"),
            };

            var ptKyuseis = new List<PtKyuseiModel>()
            {
                new PtKyuseiModel(1, 98422233, 1, "Sample KanaName", "Name", 20230101)
            };

            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 9999, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "11",
                                  tokki2: "22",
                                  tokki3: "33",
                                  tokki4: "44",
                                  tokki5: "55",
                                  rousaiKofuNo: "RousaiKofu",
                                  rousaiRoudouCd: "R1",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "rk",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "rs",
                                  rousaiJigyosyoName: "rj",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "ABC",
                                  confirmDateList: new List<ConfirmDateModel>()
                                                      {
                                                        new ConfirmDateModel(123456, 1, 50, DateTime.UtcNow, 1, "Check Comment"),
                                                      },
                                  listRousaiTenki: new List<RousaiTenkiModel>()
                                                      {
                                                        new RousaiTenkiModel(1, 2, 20240101, 3, 99999),
                                                      },
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };

            var hokenKohis = new List<KohiInfModel>()
            {
                new KohiInfModel(futansyaNo: "123456",
                                 jyukyusyaNo:"234567",
                                 hokenId: 61,
                                 startDate: 20230101,
                                 endDate: 20240101,
                                 confirmDate: 20231212,
                                 rate: 3,
                                 gendoGaku: 5,
                                 sikakuDate: 20230506,
                                 kofuDate: 20230505,
                                 tokusyuNo: "33444",
                                 hokenSbtKbn: 55,
                                 houbetu: "123",
                                 hokenNo: 55,
                                 hokenEdaNo: 23,
                                 prefNo: 21,
                                 new(),
                                 sinDate: 20240101,
                                 confirmDateList: new List<ConfirmDateModel>()
                                                      {
                                                        new ConfirmDateModel(123456, 1, 50, DateTime.UtcNow, 1, "Check Comment"),
                                                      },
                                 true,
                                 0,
                                 isAddNew: true,
                                 seqNo:0
                                 ),
            };

            var idMaxLimitList = tenantNoTracking.LimitListInfs.Max(x => x.Id);
            var maxMoneys = new List<LimitListModel>()
            {
                new LimitListModel(
                                    id: idMaxLimitList + 9999,
                                    kohiId: 30,
                                    sinDate: 20240303,
                                    hokenPid: 55,
                                    sortKey: "AM",
                                    raiinNo: 123456444,
                                    futanGaku: 9,
                                    totalGaku: 99,
                                    biko:"bi",
                                    isDeleted:0,
                                    seqNo: 0
                                   ),
            };

            (bool resultSave, long ptId) createPtInf = new();
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            PtMemo? memo = null;
            PtGrpInf? ptGrp = null;
            PtKyusei? ptKyusei = null;
            PtHokenPattern? ptHokenPattern = null;
            PtHokenInf? pthokenInf = null;
            PtRousaiTenki? ptRousaiTenkis = null;
            PtHokenCheck? ptHokenChecks = null;
            PtHokenCheck? ptHokenCheckKohis = null;
            PtKohi? ptKohis = null;
            List<LimitListInf> limitInfs = new();

            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, ptKyuseis, ptSantei, insurances, hokenInfs, hokenKohis, ptGrps, maxMoneys, insuranceScanModel, 9999);

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                memo = tenantTracking.PtMemos.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptGrp = tenantTracking.PtGrpInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptKyusei = tenantTracking.PtKyuseis.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptHokenPattern = tenantTracking.PtHokenPatterns.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                pthokenInf = tenantTracking.PtHokenInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptRousaiTenkis = tenantTracking.PtRousaiTenkis.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId && x.HokenId == 789012);
                ptHokenChecks = tenantTracking.PtHokenChecks.FirstOrDefault(x => x.HpId == 1 && x.HokenId == 789012 && x.PtID == createPtInf.ptId && x.HokenGrp == 1);
                ptHokenCheckKohis = tenantTracking.PtHokenChecks.FirstOrDefault(x => x.HpId == 1 && x.HokenId == 61 && x.PtID == createPtInf.ptId && x.HokenGrp == 2);
                ptKohis = tenantTracking.PtKohis.FirstOrDefault(x => x.HpId == 1 && x.HokenId == 61 && x.PtId == createPtInf.ptId);
                limitInfs = tenantTracking.LimitListInfs.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(0));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(memo?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(memo?.HpId, Is.EqualTo(1));
                Assert.That(ptGrp?.HpId, Is.EqualTo(1));
                Assert.That(ptGrp?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKyusei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKyusei?.HpId, Is.EqualTo(1));
                Assert.That(ptHokenPattern?.HpId, Is.EqualTo(1));
                Assert.That(ptHokenPattern?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(pthokenInf?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(pthokenInf?.EndDate, Is.EqualTo(20221231));
                Assert.That(ptRousaiTenkis?.Tenki, Is.EqualTo(2));
                Assert.That(ptRousaiTenkis?.Sinkei, Is.EqualTo(1));
                Assert.That(ptRousaiTenkis?.HokenId, Is.EqualTo(789012));
                Assert.That(ptHokenCheckKohis?.HokenId, Is.EqualTo(61));
                Assert.That(limitInfs.Any(), Is.EqualTo(true));
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (memo != null)
                {
                    tenantTracking.PtMemos.Remove(memo);
                }
                if (ptGrp != null)
                {
                    tenantTracking.PtGrpInfs.Remove(ptGrp);
                }
                if (ptKyusei != null)
                {
                    tenantTracking.PtKyuseis.Remove(ptKyusei);
                }
                if (ptHokenPattern != null)
                {
                    tenantTracking.PtHokenPatterns.Remove(ptHokenPattern);
                }
                if (pthokenInf != null)
                {
                    tenantTracking.PtHokenInfs.Remove(pthokenInf);
                }
                if (ptRousaiTenkis != null)
                {
                    tenantTracking.PtRousaiTenkis.Remove(ptRousaiTenkis);
                }
                if (ptHokenChecks != null)
                {
                    tenantTracking.PtHokenChecks.Remove(ptHokenChecks);
                }
                if (ptHokenCheckKohis != null)
                {
                    tenantTracking.PtHokenChecks.Remove(ptHokenCheckKohis);
                }
                if (ptKohis != null)
                {
                    tenantTracking.PtKohis.Remove(ptKohis);
                }
                if (limitInfs.Any())
                {
                    tenantTracking.LimitListInfs.RemoveRange(limitInfs);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_012_CreatePatientInfo_Save_InsurancesCan()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "Memo Test"
            );

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            var ptGrps = new List<GroupInfModel>()
            {
                new GroupInfModel(1, 98422233, 1, "1234", "1234"),
            };

            var ptKyuseis = new List<PtKyuseiModel>()
            {
                new PtKyuseiModel(1, 98422233, 1, "Sample KanaName", "Name", 20230101)
            };

            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 9999, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };

            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 123456,
                                  hokenId: 789012,
                                  seqNo: 1,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "11",
                                  tokki2: "22",
                                  tokki3: "33",
                                  tokki4: "44",
                                  tokki5: "55",
                                  rousaiKofuNo: "RousaiKofu",
                                  rousaiRoudouCd: "R1",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "rk",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "rs",
                                  rousaiJigyosyoName: "rj",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "ABC",
                                  confirmDateList: new List<ConfirmDateModel>()
                                                      {
                                                        new ConfirmDateModel(123456, 1, 50, DateTime.UtcNow, 1, "Check Comment"),
                                                      },
                                  listRousaiTenki: new List<RousaiTenkiModel>()
                                                      {
                                                        new RousaiTenkiModel(1, 2, 20240101, 3, 99999),
                                                      },
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };

            var hokenKohis = new List<KohiInfModel>()
            {
                new KohiInfModel(futansyaNo: "123456",
                                 jyukyusyaNo:"234567",
                                 hokenId: 61,
                                 startDate: 20230101,
                                 endDate: 20240101,
                                 confirmDate: 20231212,
                                 rate: 3,
                                 gendoGaku: 5,
                                 sikakuDate: 20230506,
                                 kofuDate: 20230505,
                                 tokusyuNo: "33444",
                                 hokenSbtKbn: 55,
                                 houbetu: "123",
                                 hokenNo: 55,
                                 hokenEdaNo: 23,
                                 prefNo: 21,
                                 new(),
                                 sinDate: 20240101,
                                 confirmDateList: new List<ConfirmDateModel>()
                                                      {
                                                        new ConfirmDateModel(123456, 1, 50, DateTime.UtcNow, 1, "Check Comment"),
                                                      },
                                 true,
                                 0,
                                 isAddNew: true,
                                 seqNo:0
                                 ),
            };

            var idMaxLimitList = tenantNoTracking.LimitListInfs.Max(x => x.Id);
            var maxMoneys = new List<LimitListModel>()
            {
                new LimitListModel(
                                    id: idMaxLimitList + 8888,
                                    kohiId: 30,
                                    sinDate: 20240303,
                                    hokenPid: 55,
                                    sortKey: "AM",
                                    raiinNo: 123456444,
                                    futanGaku: 9,
                                    totalGaku: 99,
                                    biko:"bi",
                                    isDeleted:0,
                                    seqNo: 0
                                   ),
            };

            var ptNumAuto = savePatientInfo.GetAutoPtNum(1);

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel =
               (hpId, ptId, seqNo) =>
               {

                   var insuranceScanModels = new List<InsuranceScanModel>()
                   {
                       new InsuranceScanModel(1, ptNumAuto, 0, 11, 40, "Unit-Test", GetSampleFileStream(), 0, "20230101"),
                   };
                   return insuranceScanModels;
               };

            (bool resultSave, long ptId) createPtInf = new();
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            PtMemo? memo = null;
            PtGrpInf? ptGrp = null;
            PtKyusei? ptKyusei = null;
            PtHokenPattern? ptHokenPattern = null;
            PtHokenInf? pthokenInf = null;
            PtRousaiTenki? ptRousaiTenkis = null;
            PtHokenCheck? ptHokenChecks = null;
            PtHokenCheck? ptHokenCheckKohis = null;
            PtKohi? ptKohis = null;
            List<LimitListInf> limitInfs = new();
            List<PtHokenScan> ptHokenScans = new();

            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, ptKyuseis, ptSantei, insurances, hokenInfs, hokenKohis, ptGrps, maxMoneys, insuranceScanModel, 9999);

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                memo = tenantTracking.PtMemos.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptGrp = tenantTracking.PtGrpInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptKyusei = tenantTracking.PtKyuseis.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptHokenPattern = tenantTracking.PtHokenPatterns.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                pthokenInf = tenantTracking.PtHokenInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptRousaiTenkis = tenantTracking.PtRousaiTenkis.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId && x.HokenId == 789012);
                ptHokenChecks = tenantTracking.PtHokenChecks.FirstOrDefault(x => x.HpId == 1 && x.HokenId == 789012 && x.PtID == createPtInf.ptId && x.HokenGrp == 1);
                ptHokenCheckKohis = tenantTracking.PtHokenChecks.FirstOrDefault(x => x.HpId == 1 && x.HokenId == 61 && x.PtID == createPtInf.ptId && x.HokenGrp == 2);
                ptKohis = tenantTracking.PtKohis.FirstOrDefault(x => x.HpId == 1 && x.HokenId == 61 && x.PtId == createPtInf.ptId);
                limitInfs = tenantTracking.LimitListInfs.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();
                ptHokenScans = tenantTracking.PtHokenScans.Where(x => x.HpId == 1 && x.PtId == ptNumAuto).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(0));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(memo?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(memo?.HpId, Is.EqualTo(1));
                Assert.That(ptGrp?.HpId, Is.EqualTo(1));
                Assert.That(ptGrp?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKyusei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKyusei?.HpId, Is.EqualTo(1));
                Assert.That(ptHokenPattern?.HpId, Is.EqualTo(1));
                Assert.That(ptHokenPattern?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(pthokenInf?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(pthokenInf?.EndDate, Is.EqualTo(20221231));
                Assert.That(ptRousaiTenkis?.Tenki, Is.EqualTo(2));
                Assert.That(ptRousaiTenkis?.Sinkei, Is.EqualTo(1));
                Assert.That(ptRousaiTenkis?.HokenId, Is.EqualTo(789012));
                Assert.That(ptHokenCheckKohis?.HokenId, Is.EqualTo(61));
                Assert.That(limitInfs.Any(), Is.EqualTo(true));
                Assert.That(ptHokenScans.Any(), Is.EqualTo(true));
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (memo != null)
                {
                    tenantTracking.PtMemos.Remove(memo);
                }
                if (ptGrp != null)
                {
                    tenantTracking.PtGrpInfs.Remove(ptGrp);
                }
                if (ptKyusei != null)
                {
                    tenantTracking.PtKyuseis.Remove(ptKyusei);
                }
                if (ptHokenPattern != null)
                {
                    tenantTracking.PtHokenPatterns.Remove(ptHokenPattern);
                }
                if (pthokenInf != null)
                {
                    tenantTracking.PtHokenInfs.Remove(pthokenInf);
                }
                if (ptRousaiTenkis != null)
                {
                    tenantTracking.PtRousaiTenkis.Remove(ptRousaiTenkis);
                }
                if (ptHokenChecks != null)
                {
                    tenantTracking.PtHokenChecks.Remove(ptHokenChecks);
                }
                if (ptHokenCheckKohis != null)
                {
                    tenantTracking.PtHokenChecks.Remove(ptHokenCheckKohis);
                }
                if (ptKohis != null)
                {
                    tenantTracking.PtKohis.Remove(ptKohis);
                }
                if (limitInfs.Any())
                {
                    tenantTracking.LimitListInfs.RemoveRange(limitInfs);
                }
                if (ptHokenScans.Any())
                {
                    tenantTracking.PtHokenScans.RemoveRange(ptHokenScans);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_013_UpdatePatientInfo_PtInf_Null()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            (bool resultSave, long ptId) createPtInf = new();
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;

            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), new(), new(), insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
                                        {
                                            new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
                                        };
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: 98422233,
                                                ptNum: 1,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 0,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );

                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new(), new(), new(), new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                // Assert
                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                Assert.That(updatePtInf.resultSave, Is.EqualTo(false));

            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_014_UpdatePatientInfo_Save_PtSanteis()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
                new CalculationInfModel(1, 98422233, 3, 1, 1, 20230101, 20240101, 1000),
            };

            PtInf? ptInf = null;
            List<PtSanteiConf> santeis = new();
            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), new(), new(), insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
                new CalculationInfModel(1, 98422233, 2, 1, 2, 20230101, 20240101, 0),
            };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 0,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );

                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSanteiUpdate, new(), new(), new(), new(), new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santeis = tenantTracking.PtSanteiConfs.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId && x.IsDeleted == DeleteTypes.None).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(0));
                Assert.That(santeis.Count() == 2);
                Assert.That(santeis.First().HpId, Is.EqualTo(1));
                Assert.That(santeis.First().PtId, Is.EqualTo(createPtInf.ptId));
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santeis.Any())
                {
                    tenantTracking.PtSanteiConfs.RemoveRange(santeis);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_015_UpdatePatientInfo_HaveByomei()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            PtByomei? ptByomei = null;
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtByomei> ptByomeiCheck = new();
            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), new(), new(), insuranceScanModel, 9999);
                ptByomei = new PtByomei { HpId = 1, PtId = createPtInf.ptId, ByomeiCd = "0000001", HokenPid = 1, TenkiKbn = 0 };
                tenantTracking.Add(ptByomei);
                tenantTracking.SaveChanges();
                var ptSanteiUpdate = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
            };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 0,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );
                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new List<HokenInfModel>() { new HokenInfModel(1, 0, 99999999) }, new(), new(), new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptByomeiCheck = tenantNoTracking.PtByomeis.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(0));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptByomeiCheck.Count() == 4);
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptByomei != null)
                {
                    tenantTracking.PtByomeis.RemoveRange(ptByomei);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_016_UpdatePatientInfo_IsDead()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            PtByomei? ptByomei = null;
            PtFamily? ptFamily = null;
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), new(), new(), insuranceScanModel, 9999);
                ptByomei = new PtByomei { HpId = 1, PtId = createPtInf.ptId, ByomeiCd = "0000001", HokenPid = 1, TenkiKbn = 0 };
                ptFamily = new PtFamily { FamilyPtId = createPtInf.ptId, HpId = 1 };
                tenantTracking.Add(ptByomei);
                tenantTracking.Add(ptFamily);
                tenantTracking.SaveChanges();
                var ptSanteiUpdate = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
            };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );
                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new List<HokenInfModel>() { new HokenInfModel(1, 0, 99999999) }, new(), new(), new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var ptByomeiCheck = tenantNoTracking.PtByomeis.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var ptFamilyCheck = tenantNoTracking.PtFamilys.FirstOrDefault(x => x.HpId == 1 && x.FamilyPtId == createPtInf.ptId);

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptByomeiCheck.Count() == 4);
                Assert.That(ptFamilyCheck?.IsDead == 1);
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptByomei != null)
                {
                    tenantTracking.PtByomeis.RemoveRange(ptByomei);
                }
                if (ptFamily != null)
                {
                    tenantTracking.PtFamilys.Remove(ptFamily);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_017_UpdatePatientInfo_Remove_Memo()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "abc"
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            (bool resultSave, long ptId) createPtInf = new();
            PtByomei? ptByomei = null;
            PtFamily? ptFamily = null;
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtByomei> ptByomeiCheck = new();

            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), new(), new(), insuranceScanModel, 9999);
                ptByomei = new PtByomei { HpId = 1, PtId = createPtInf.ptId, ByomeiCd = "0000001", HokenPid = 1, TenkiKbn = 0 };
                ptFamily = new PtFamily { FamilyPtId = createPtInf.ptId, HpId = 1 };
                tenantTracking.Add(ptByomei);
                tenantTracking.Add(ptFamily);
                tenantTracking.SaveChanges();
                var ptSanteiUpdate = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
            };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );
                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new List<HokenInfModel>() { new HokenInfModel(1, 0, 99999999) }, new(), new(), new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptByomeiCheck = tenantNoTracking.PtByomeis.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();
                var ptFamilyCheck = tenantNoTracking.PtFamilys.FirstOrDefault(x => x.HpId == 1 && x.FamilyPtId == createPtInf.ptId);
                var ptMemo = tenantTracking.PtMemos.FirstOrDefault(p => p.PtId == createPtInf.ptId && p.HpId == 1);

                // Assert
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptByomeiCheck.Count() == 4);
                Assert.That(ptFamilyCheck?.IsDead == 1);
                Assert.That(ptMemo?.IsDeleted == 1);
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptByomei != null)
                {
                    tenantTracking.PtByomeis.RemoveRange(ptByomei);
                }
                if (ptFamily != null)
                {
                    tenantTracking.PtFamilys.Remove(ptFamily);
                }
                if (ptByomeiCheck != null)
                {
                    tenantNoTracking.PtByomeis.RemoveRange(ptByomeiCheck);
                }
                if (tenantTracking.PtMemos.Where(p => p.PtId == createPtInf.ptId && p.HpId == 1).Any())
                {
                    tenantTracking.PtMemos.RemoveRange(tenantTracking.PtMemos.Where(p => p.PtId == createPtInf.ptId && p.HpId == 1));
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_018_UpdatePatientInfo_Add_Memo()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            (bool resultSave, long ptId) createPtInf = new();
            PtByomei? ptByomei = null;
            PtFamily? ptFamily = null;
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtByomei> ptByomeiCheck = new();

            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), new(), new(), insuranceScanModel, 9999);
                ptByomei = new PtByomei { HpId = 1, PtId = createPtInf.ptId, ByomeiCd = "0000001", HokenPid = 1, TenkiKbn = 0 };
                ptFamily = new PtFamily { FamilyPtId = createPtInf.ptId, HpId = 1 };
                tenantTracking.Add(ptByomei);
                tenantTracking.Add(ptFamily);
                tenantTracking.SaveChanges();
                var ptSanteiUpdate = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
            };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: "abc"
                );
                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new List<HokenInfModel>() { new HokenInfModel(1, 0, 99999999) }, new(), new(), new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptByomeiCheck = tenantNoTracking.PtByomeis.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();
                var ptFamilyCheck = tenantNoTracking.PtFamilys.FirstOrDefault(x => x.HpId == 1 && x.FamilyPtId == createPtInf.ptId);
                var ptMemo = tenantTracking.PtMemos.FirstOrDefault(p => p.PtId == createPtInf.ptId && p.HpId == 1);

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptByomeiCheck.Count() == 4);
                Assert.That(ptFamilyCheck?.IsDead == 1);
                Assert.That(ptMemo?.IsDeleted == 0);
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptByomei != null)
                {
                    tenantTracking.PtByomeis.RemoveRange(ptByomei);
                }
                if (ptFamily != null)
                {
                    tenantTracking.PtFamilys.Remove(ptFamily);
                }
                if (ptByomeiCheck != null)
                {
                    tenantNoTracking.PtByomeis.RemoveRange(ptByomeiCheck);
                }
                if (tenantTracking.PtMemos.Where(p => p.PtId == createPtInf.ptId && p.HpId == 1).Any())
                {
                    tenantTracking.PtMemos.RemoveRange(tenantTracking.PtMemos.Where(p => p.PtId == createPtInf.ptId && p.HpId == 1));
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_019_UpdatePatientInfo_Update_Memo()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: "memo"
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            (bool resultSave, long ptId) createPtInf = new();
            PtByomei? ptByomei = null;
            PtFamily? ptFamily = null;
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtByomei> ptByomeiCheck = new();

            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), new(), new(), insuranceScanModel, 9999);
                ptByomei = new PtByomei { HpId = 1, PtId = createPtInf.ptId, ByomeiCd = "0000001", HokenPid = 1, TenkiKbn = 0 };
                ptFamily = new PtFamily { FamilyPtId = createPtInf.ptId, HpId = 1 };
                tenantTracking.Add(ptByomei);
                tenantTracking.Add(ptFamily);
                tenantTracking.SaveChanges();
                var ptSanteiUpdate = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
            };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: "memo1"
                );
                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new List<HokenInfModel>() { new HokenInfModel(1, 0, 99999999) }, new(), new(), new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptByomeiCheck = tenantNoTracking.PtByomeis.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();
                var ptFamilyCheck = tenantNoTracking.PtFamilys.FirstOrDefault(x => x.HpId == 1 && x.FamilyPtId == createPtInf.ptId);
                var ptMemo = tenantTracking.PtMemos.FirstOrDefault(p => p.PtId == createPtInf.ptId && p.HpId == 1);

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptByomeiCheck.Count() == 4);
                Assert.That(ptFamilyCheck?.IsDead == 1);
                Assert.That(ptMemo?.IsDeleted == 0 && ptMemo?.Memo == "memo1");
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptByomei != null)
                {
                    tenantTracking.PtByomeis.RemoveRange(ptByomei);
                }
                if (ptFamily != null)
                {
                    tenantTracking.PtFamilys.Remove(ptFamily);
                }
                if (ptByomeiCheck != null)
                {
                    tenantNoTracking.PtByomeis.RemoveRange(ptByomeiCheck);
                }
                if (tenantTracking.PtMemos.Where(p => p.PtId == createPtInf.ptId && p.HpId == 1).Any())
                {
                    tenantTracking.PtMemos.RemoveRange(tenantTracking.PtMemos.Where(p => p.PtId == createPtInf.ptId && p.HpId == 1));
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_020_UpdatePatientInfo_Add_PtKyusei()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            (bool resultSave, long ptId) createPtInf = new();
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtByomei> ptByomeiCheck = new();
            PtKyusei? ptKyusei = null;

            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), new(), new(), insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
            };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );
                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new List<PtKyuseiModel> { new PtKyuseiModel(1, createPtInf.ptId, 0, "Kana Name", "Name", 99999999) }, ptSantei, new(), new List<HokenInfModel>() { new HokenInfModel(1, 0, 99999999) }, new(), new(), new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptKyusei = tenantTracking.PtKyuseis.FirstOrDefault(p => p.PtId == createPtInf.ptId && p.HpId == 1);

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKyusei?.KanaName == "Kana Name" && ptKyusei?.Name == "Name");
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptByomeiCheck != null)
                {
                    tenantNoTracking.PtByomeis.RemoveRange(ptByomeiCheck);
                }
                if (ptKyusei != null)
                {
                    tenantTracking.PtKyuseis.Remove(ptKyusei);
                }
                if (tenantTracking.PtMemos.Where(p => p.PtId == createPtInf.ptId && p.HpId == 1).Any())
                {
                    tenantTracking.PtMemos.RemoveRange(tenantTracking.PtMemos.Where(p => p.PtId == createPtInf.ptId && p.HpId == 1));
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_021_UpdatePatientInfo_Update_PtKyusei()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            (bool resultSave, long ptId) createPtInf = new();
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtByomei> ptByomeiCheck = new();
            PtKyusei? oldPtKyusei = null;
            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new List<PtKyuseiModel> { new PtKyuseiModel(1, 98422233, 0, "Kana Name", "Name", 99999999) }, ptSantei, new(), new(), new(), new(), new(), insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
            };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );
                oldPtKyusei = tenantTracking.PtKyuseis.FirstOrDefault(p => p.PtId == createPtInf.ptId && p.HpId == 1);
                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new List<PtKyuseiModel> { new PtKyuseiModel(1, oldPtKyusei?.PtId ?? 0, oldPtKyusei?.SeqNo ?? 0, "Kana Name 1", "Name 1", 99999999) }, ptSantei, new(), new List<HokenInfModel>() { new HokenInfModel(1, 0, 99999999) }, new(), new(), new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var ptKyusei = tenantNoTracking.PtKyuseis.FirstOrDefault(p => p.PtId == createPtInf.ptId && p.HpId == 1 && (oldPtKyusei != null && p.SeqNo == oldPtKyusei.SeqNo));

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKyusei?.KanaName == "Kana Name 1" && ptKyusei?.Name == "Name 1");
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (oldPtKyusei != null)
                {
                    tenantTracking.PtKyuseis.Remove(oldPtKyusei);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_022_UpdatePatientInfo_Remove_PtKyusei()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtByomei> ptByomeiCheck = new();
            PtKyusei? oldPtKyusei = null;

            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new List<PtKyuseiModel> { new PtKyuseiModel(1, 98422233, 0, "Kana Name", "Name", 99999999) }, ptSantei, new(), new(), new(), new(), new(), insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
                {
                    new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
                };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );
                oldPtKyusei = tenantTracking.PtKyuseis.FirstOrDefault(p => p.PtId == createPtInf.ptId && p.HpId == 1);
                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new List<HokenInfModel>() { new HokenInfModel(1, 0, 99999999) }, new(), new(), new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var ptKyusei = tenantNoTracking.PtKyuseis.FirstOrDefault(p => p.PtId == createPtInf.ptId && p.HpId == 1 && (oldPtKyusei != null && p.SeqNo == oldPtKyusei.SeqNo && p.IsDeleted == 0));

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKyusei == null);
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (oldPtKyusei != null)
                {
                    tenantTracking.PtKyuseis.Remove(oldPtKyusei);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_023_UpdatePatientInfo_Add_GrpInf()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtByomei> ptByomeiCheck = new();
            List<PtGrpInf> groupInf = new();
            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), new(), new(), insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
            };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );
                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new List<HokenInfModel>() { new HokenInfModel(1, 0, 99999999) }, new(), new List<GroupInfModel>() { new GroupInfModel(1, createPtInf.ptId, 1, "001", "Group Name") }, new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                groupInf = tenantNoTracking.PtGrpInfs.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf.Sex, Is.EqualTo(1));
                Assert.That(ptInf.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf.IsDead, Is.EqualTo(1));
                Assert.That(santei.HpId, Is.EqualTo(1));
                Assert.That(santei.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(groupInf.Count() == 1);
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (groupInf.Any())
                {
                    tenantTracking.RemoveRange(groupInf);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_024_UpdatePatientInfo_Update_GrpInf()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            (bool resultSave, long ptId) createPtInf = new();
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtByomei> ptByomeiCheck = new();
            List<PtGrpInf> groupInf = new();

            try
            {
                // Act
                createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 1, "001", "Group Name") }, new(), insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
            };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );
                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new List<HokenInfModel>() { new HokenInfModel(1, 0, 99999999) }, new(), new List<GroupInfModel>() { new GroupInfModel(1, createPtInf.ptId, 1, "001", "Group Name") }, new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                groupInf = tenantNoTracking.PtGrpInfs.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(groupInf.Count() == 2 && groupInf.Count(x => x.IsDeleted == 0) == 1 && groupInf.Count(x => x.IsDeleted == 1) == 1);
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (groupInf != null)
                {
                    tenantNoTracking.RemoveRange(groupInf);
                }
                tenantNoTracking.SaveChanges();
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_025_UpdatePatientInfo_Remove_GrpInf()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtByomei> ptByomeiCheck = new();
            List<PtGrpInf> groupInf = new();

            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 1, "001", "Group Name"), new GroupInfModel(1, 98422233, 2, "002", "Group Name") }, new(), insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
                {
                    new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
                };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );
                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new List<HokenInfModel>() { new HokenInfModel(1, 0, 99999999) }, new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 2, "", "Group Name") }, new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                groupInf = tenantNoTracking.PtGrpInfs.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId && x.IsDeleted == 1).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(groupInf.Count() == 2);
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (groupInf != null)
                {
                    tenantNoTracking.RemoveRange(groupInf);
                }
                tenantNoTracking.SaveChanges();
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_026_UpdatePatientInfo_Add_Insurance()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };

            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtByomei> ptByomeiCheck = new();
            List<PtGrpInf> groupInf = new();
            List<PtHokenPattern> ptHokenPattern = new();
            try
            {

                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 1, "001", "Group Name"), new GroupInfModel(1, 98422233, 2, "002", "Group Name") }, new(), insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
            };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );
                var insurances = new List<InsuranceModel>()
                {
                    new InsuranceModel(1, createPtInf.ptId, 19900101, 0, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
                };

                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, insurances, new List<HokenInfModel>() { new HokenInfModel(1, 0, 99999999) }, new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 2, "", "Group Name") }, new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptHokenPattern = tenantNoTracking.PtHokenPatterns.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptHokenPattern.Count() == 1);
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptHokenPattern.Any())
                {
                    tenantNoTracking.RemoveRange(ptHokenPattern);
                }
                tenantNoTracking.SaveChanges();
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_027_UpdatePatientInfo_Update_Insurance()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 0, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtHokenPattern> ptHokenPattern = new();

            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, insurances, new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 1, "001", "Group Name"), new GroupInfModel(1, 98422233, 2, "002", "Group Name") }, new(), insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
                {
                    new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
                };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );
                var oldPtHokenPattern = tenantNoTracking.PtHokenPatterns.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var updateInsurances = new List<InsuranceModel>()
                {
                    new InsuranceModel(1, createPtInf.ptId, 19900101, oldPtHokenPattern?.SeqNo ?? 0, 4, 4, 4, 20230505, "Sample Memo 1", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew: false),
                };

                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, updateInsurances, new List<HokenInfModel>() { new HokenInfModel(1, 0, 99999999) }, new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 2, "", "Group Name") }, new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptHokenPattern = tenantNoTracking.PtHokenPatterns.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptHokenPattern.Count() == 1 && ptHokenPattern.First().HokenMemo == "Sample Memo 1");
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptHokenPattern.Any())
                {
                    tenantNoTracking.RemoveRange(ptHokenPattern);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_028_UpdatePatientInfo_Remove_Insurance()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 0, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };

            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtHokenPattern> ptHokenPattern = new();
            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, insurances, new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 1, "001", "Group Name"), new GroupInfModel(1, 98422233, 2, "002", "Group Name") }, new(), insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
                {
                    new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
                };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );
                var oldPtHokenPattern = tenantNoTracking.PtHokenPatterns.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);

                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new List<HokenInfModel>() { new HokenInfModel(1, 0, 99999999) }, new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 2, "", "Group Name") }, new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptHokenPattern = tenantNoTracking.PtHokenPatterns.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId && x.IsDeleted == 1).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf.Sex, Is.EqualTo(1));
                Assert.That(ptInf.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf.IsDead, Is.EqualTo(1));
                Assert.That(santei.HpId, Is.EqualTo(1));
                Assert.That(santei.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptHokenPattern.Count() == 1);
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptHokenPattern.Any())
                {
                    tenantNoTracking.RemoveRange(ptHokenPattern);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_029_UpdatePatientInfo_Add_HokenInf()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 0, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };

            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtHokenPattern> ptHokenPattern = new();
            List<PtRousaiTenki> ptRousaiTenkis = new();
            List<PtHokenInf> ptHokenInfs = new();
            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, insurances, new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 1, "001", "Group Name"), new GroupInfModel(1, 98422233, 2, "002", "Group Name") }, new(), insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
                {
                    new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
                };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );
                var hokenInfs = new List<HokenInfModel>()
                {
                    new HokenInfModel(
                                      hpId: 1,
                                      ptId: createPtInf.ptId,
                                      hokenId: 789012,
                                      seqNo: 0,
                                      hokenNo: 456,
                                      hokenEdaNo: 2,
                                      hokenKbn: 3,
                                      hokensyaNo: "ABC1234",
                                      kigo: "K1234",
                                      bango: "B5678",
                                      edaNo: "E7",
                                      honkeKbn: 5,
                                      startDate: 20220101,
                                      endDate: 20221231,
                                      sikakuDate: 20220115,
                                      kofuDate: 20220201,
                                      confirmDate: 20220215,
                                      kogakuKbn: 1,
                                      tasukaiYm: 202203,
                                      tokureiYm1: 202204,
                                      tokureiYm2: 202205,
                                      genmenKbn: 1,
                                      genmenRate: 80,
                                      genmenGaku: 500000,
                                      syokumuKbn: 2,
                                      keizokuKbn: 1,
                                      tokki1: "11",
                                      tokki2: "22",
                                      tokki3: "33",
                                      tokki4: "44",
                                      tokki5: "55",
                                      rousaiKofuNo: "RousaiKofu",
                                      rousaiRoudouCd: "R1",
                                      rousaiSaigaiKbn: 0,
                                      rousaiKantokuCd: "rk",
                                      rousaiSyobyoDate: 20221001,
                                      ryoyoStartDate: 20221001,
                                      ryoyoEndDate: 20221231,
                                      rousaiSyobyoCd: "rs",
                                      rousaiJigyosyoName: "rj",
                                      rousaiPrefName: "Tokyo",
                                      rousaiCityName: "Shinjuku",
                                      rousaiReceCount: 3,
                                      hokensyaName: "HokensyaName",
                                      hokensyaAddress: "HokensyaAddress",
                                      hokensyaTel: "123-456-7890",
                                      sinDate: 20220101,
                                      jibaiHokenName: "JibaiHokenName",
                                      jibaiHokenTanto: "TantoName",
                                      jibaiHokenTel: "987-654-3210",
                                      jibaiJyusyouDate: 20220301,
                                      houbetu: "ABC",
                                      confirmDateList: new List<ConfirmDateModel>()
                                                          {
                                                            new ConfirmDateModel(createPtInf.ptId, 1, 50, DateTime.UtcNow, 1, "Check Comment"),
                                                          },
                                      listRousaiTenki: new List<RousaiTenkiModel>()
                                                          {
                                                            new RousaiTenkiModel(1, 2, 20240101, 3, 0),
                                                          },
                                      isReceKisaiOrNoHoken: true,
                                      isDeleted: 0,
                                      hokenMst: new HokenMstModel(),
                                      isAddNew: true,
                                      isAddHokenCheck: true,
                                      hokensyaMst: new HokensyaMstModel()
                                      ),
                };

                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), hokenInfs, new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 2, "", "Group Name") }, new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptHokenInfs = tenantNoTracking.PtHokenInfs.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();
                ptRousaiTenkis = tenantNoTracking.PtRousaiTenkis.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptHokenInfs.Count() == 1);
                Assert.That(ptRousaiTenkis.Count() == 1);
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptHokenInfs.Any())
                {
                    tenantTracking.RemoveRange(ptHokenInfs);
                }
                if (ptRousaiTenkis.Any())
                {
                    tenantTracking.RemoveRange(ptRousaiTenkis);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_030_UpdatePatientInfo_Update_HokenInf()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 0, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };
            var hokenInfs = new List<HokenInfModel>()
            {
                new HokenInfModel(
                                  hpId: 1,
                                  ptId: 98422233,
                                  hokenId: 789012,
                                  seqNo: 0,
                                  hokenNo: 456,
                                  hokenEdaNo: 2,
                                  hokenKbn: 3,
                                  hokensyaNo: "ABC1234",
                                  kigo: "K1234",
                                  bango: "B5678",
                                  edaNo: "E7",
                                  honkeKbn: 5,
                                  startDate: 20220101,
                                  endDate: 20221231,
                                  sikakuDate: 20220115,
                                  kofuDate: 20220201,
                                  confirmDate: 20220215,
                                  kogakuKbn: 1,
                                  tasukaiYm: 202203,
                                  tokureiYm1: 202204,
                                  tokureiYm2: 202205,
                                  genmenKbn: 1,
                                  genmenRate: 80,
                                  genmenGaku: 500000,
                                  syokumuKbn: 2,
                                  keizokuKbn: 1,
                                  tokki1: "11",
                                  tokki2: "22",
                                  tokki3: "33",
                                  tokki4: "44",
                                  tokki5: "55",
                                  rousaiKofuNo: "RousaiKofu",
                                  rousaiRoudouCd: "R1",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "rk",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "rs",
                                  rousaiJigyosyoName: "rj",
                                  rousaiPrefName: "Tokyo",
                                  rousaiCityName: "Shinjuku",
                                  rousaiReceCount: 3,
                                  hokensyaName: "HokensyaName",
                                  hokensyaAddress: "HokensyaAddress",
                                  hokensyaTel: "123-456-7890",
                                  sinDate: 20220101,
                                  jibaiHokenName: "JibaiHokenName",
                                  jibaiHokenTanto: "TantoName",
                                  jibaiHokenTel: "987-654-3210",
                                  jibaiJyusyouDate: 20220301,
                                  houbetu: "ABC",
                                  confirmDateList: new List<ConfirmDateModel>()
                                                      {
                                                        new ConfirmDateModel(98422233, 1, 50, DateTime.UtcNow, 1, "Check Comment"),
                                                      },
                                  listRousaiTenki: new List<RousaiTenkiModel>()
                                                      {
                                                        new RousaiTenkiModel(1, 2, 20240101, 3, 0),
                                                        new RousaiTenkiModel(1, 2, 20240101, 3, 0),
                                                      },
                                  isReceKisaiOrNoHoken: true,
                                  isDeleted: 0,
                                  hokenMst: new HokenMstModel(),
                                  isAddNew: true,
                                  isAddHokenCheck: true,
                                  hokensyaMst: new HokensyaMstModel()
                                  ),
            };

            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtHokenPattern> ptHokenPattern = new();
            List<PtRousaiTenki> ptRousaiTenkis = new();
            List<PtHokenInf> ptHokenInfs = new();

            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, insurances, hokenInfs, new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 1, "001", "Group Name"), new GroupInfModel(1, 98422233, 2, "002", "Group Name") }, new(), insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
                {
                    new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
                };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );
                var hokenInf = tenantNoTracking.PtHokenInfs.FirstOrDefault(h => h.PtId == createPtInf.ptId && h.HpId == 1);
                var rousaiTenki = tenantNoTracking.PtRousaiTenkis.FirstOrDefault(h => h.PtId == createPtInf.ptId && h.HpId == 1);
                var updateHokenInfs = new List<HokenInfModel>()
                {
                    new HokenInfModel(
                                      hpId: 1,
                                      ptId: createPtInf.ptId,
                                      hokenId: 789012,
                                      seqNo: hokenInf?.SeqNo ?? 0,
                                      hokenNo: 456,
                                      hokenEdaNo: 2,
                                      hokenKbn: 3,
                                      hokensyaNo: "ABC12345",
                                      kigo: "K12345",
                                      bango: "B56789",
                                      edaNo: "E7",
                                      honkeKbn: 5,
                                      startDate: 20220101,
                                      endDate: 20221231,
                                      sikakuDate: 20220115,
                                      kofuDate: 20220201,
                                      confirmDate: 20220215,
                                      kogakuKbn: 1,
                                      tasukaiYm: 202203,
                                      tokureiYm1: 202204,
                                      tokureiYm2: 202205,
                                      genmenKbn: 1,
                                      genmenRate: 80,
                                      genmenGaku: 500000,
                                      syokumuKbn: 2,
                                      keizokuKbn: 1,
                                      tokki1: "11",
                                      tokki2: "22",
                                      tokki3: "33",
                                      tokki4: "44",
                                      tokki5: "55",
                                      rousaiKofuNo: "RousaiKofu",
                                      rousaiRoudouCd: "R1",
                                      rousaiSaigaiKbn: 0,
                                      rousaiKantokuCd: "rk",
                                      rousaiSyobyoDate: 20221001,
                                      ryoyoStartDate: 20221001,
                                      ryoyoEndDate: 20221231,
                                      rousaiSyobyoCd: "rs",
                                      rousaiJigyosyoName: "rj",
                                      rousaiPrefName: "Tokyo",
                                      rousaiCityName: "Shinjuku",
                                      rousaiReceCount: 3,
                                      hokensyaName: "HokensyaName",
                                      hokensyaAddress: "HokensyaAddress",
                                      hokensyaTel: "123-456-7890",
                                      sinDate: 20220101,
                                      jibaiHokenName: "JibaiHokenName",
                                      jibaiHokenTanto: "TantoName",
                                      jibaiHokenTel: "987-654-3210",
                                      jibaiJyusyouDate: 20220301,
                                      houbetu: "ABC",
                                      confirmDateList: new List<ConfirmDateModel>()
                                                          {
                                                            new ConfirmDateModel(createPtInf.ptId, 1, 50, DateTime.UtcNow, 1, "Check Comment 2"),
                                                          },
                                      listRousaiTenki: new List<RousaiTenkiModel>()
                                                          {
                                                            new RousaiTenkiModel(2, 3, 99999999, 3, rousaiTenki?.SeqNo ?? 0),
                                                            new RousaiTenkiModel(1, 2, 20240101, 3, 0),
                                                          },
                                      isReceKisaiOrNoHoken: true,
                                      isDeleted: 0,
                                      hokenMst: new HokenMstModel(),
                                      isAddNew: false,
                                      isAddHokenCheck: true,
                                      hokensyaMst: new HokensyaMstModel()
                                      ),
                };

                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), updateHokenInfs, new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 2, "", "Group Name") }, new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptHokenInfs = tenantNoTracking.PtHokenInfs.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();
                ptRousaiTenkis = tenantNoTracking.PtRousaiTenkis.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptHokenInfs.Count() == 1 && ptHokenInfs.Any(h => h.HokensyaNo == "ABC12345" && h.Kigo == "K12345" && h.Bango == "B56789"));
                Assert.That(ptRousaiTenkis.Count() == 3 && ptRousaiTenkis.Any(h => h.IsDeleted == 1) && ptRousaiTenkis.Any(h => h.EndDate == 99999999));
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptHokenInfs.Any())
                {
                    tenantTracking.RemoveRange(ptHokenInfs);
                }
                if (ptRousaiTenkis.Any())
                {
                    tenantTracking.RemoveRange(ptRousaiTenkis);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_031_UpdatePatientInfo_Add_Kohi()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 0, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };

            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtHokenPattern> ptHokenPattern = new();
            List<PtKohi> ptKohiInfs = new();

            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, insurances, new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 1, "001", "Group Name"), new GroupInfModel(1, 98422233, 2, "002", "Group Name") }, new(), insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
                {
                    new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
                };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );
                var hokenKohis = new List<KohiInfModel>()
                {
                    new KohiInfModel(futansyaNo: "123456",
                                     jyukyusyaNo:"234567",
                                     hokenId: 61,
                                     startDate: 20230101,
                                     endDate: 20240101,
                                     confirmDate: 20231212,
                                     rate: 3,
                                     gendoGaku: 5,
                                     sikakuDate: 20230506,
                                     kofuDate: 20230505,
                                     tokusyuNo: "33444",
                                     hokenSbtKbn: 55,
                                     houbetu: "123",
                                     hokenNo: 55,
                                     hokenEdaNo: 23,
                                     prefNo: 21,
                                     new(),
                                     sinDate: 20240101,
                                     confirmDateList: new List<ConfirmDateModel>()
                                                          {
                                                            new ConfirmDateModel(123456, 1, 50, DateTime.UtcNow, 1, "Check Comment"),
                                                          },
                                     true,
                                     0,
                                     isAddNew: true,
                                     seqNo:0
                                     ),
                };

                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new(), hokenKohis, new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 2, "", "Group Name") }, new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptKohiInfs = tenantNoTracking.PtKohis.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKohiInfs.Count() == 1);
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptKohiInfs.Any())
                {
                    tenantTracking.RemoveRange(ptKohiInfs);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_032_UpdatePatientInfo_Update_Kohi()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 0, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };
            var hokenKohis = new List<KohiInfModel>()
            {
                new KohiInfModel(futansyaNo: "123456",
                                 jyukyusyaNo:"234567",
                                 hokenId: 61,
                                 startDate: 20230101,
                                 endDate: 20240101,
                                 confirmDate: 20231212,
                                 rate: 3,
                                 gendoGaku: 5,
                                 sikakuDate: 20230506,
                                 kofuDate: 20230505,
                                 tokusyuNo: "33444",
                                 hokenSbtKbn: 55,
                                 houbetu: "123",
                                 hokenNo: 55,
                                 hokenEdaNo: 23,
                                 prefNo: 21,
                                 new(),
                                 sinDate: 20240101,
                                 confirmDateList: new List<ConfirmDateModel>()
                                                      {
                                                        new ConfirmDateModel(123456, 1, 50, DateTime.UtcNow, 1, "Check Comment"),
                                                      },
                                 true,
                                 0,
                                 isAddNew: true,
                                 seqNo:0
                                 ),
            };

            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtHokenPattern> ptHokenPattern = new();
            List<PtKohi> ptKohiInfs = new();

            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, insurances, new(), hokenKohis, new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 1, "001", "Group Name"), new GroupInfModel(1, 98422233, 2, "002", "Group Name") }, new(), insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
                {
                    new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
                };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );

                var ptKohi = tenantNoTracking.PtKohis.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var updateHokenKohis = new List<KohiInfModel>()
                {
                    new KohiInfModel(futansyaNo: "12357",
                                     jyukyusyaNo:"2347",
                                     hokenId: 61,
                                     startDate: 20230101,
                                     endDate: 20240101,
                                     confirmDate: 20231212,
                                     rate: 3,
                                     gendoGaku: 5,
                                     sikakuDate: 20230506,
                                     kofuDate: 20230505,
                                     tokusyuNo: "3347",
                                     hokenSbtKbn: 55,
                                     houbetu: "123",
                                     hokenNo: 55,
                                     hokenEdaNo: 23,
                                     prefNo: 21,
                                     new(),
                                     sinDate: 20240101,
                                     confirmDateList: new List<ConfirmDateModel>()
                                                          {
                                                            new ConfirmDateModel(123456, 1, 50, DateTime.UtcNow, 1, "Check Comment"),
                                                          },
                                     true,
                                     0,
                                     isAddNew: false,
                                     seqNo:ptKohi?.SeqNo ?? 0
                                     ),
                };

                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new(), updateHokenKohis, new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 2, "", "Group Name") }, new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptKohiInfs = tenantNoTracking.PtKohis.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptKohiInfs.Any(k => k.FutansyaNo == "12357" && k.JyukyusyaNo == "2347" && k.TokusyuNo == "3347"));
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptKohiInfs.Any())
                {
                    tenantTracking.RemoveRange(ptKohiInfs);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_032_UpdatePatientInfo_Add_Limit()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 0, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };

            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<LimitListInf> ptLimits = new();

            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, insurances, new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 1, "001", "Group Name"), new GroupInfModel(1, 98422233, 2, "002", "Group Name") }, new(), insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
                {
                    new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
                };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );

                var maxMoneys = new List<LimitListModel>()
                {
                    new LimitListModel(
                                        id: 0,
                                        kohiId: 30,
                                        sinDate: 20240303,
                                        hokenPid: 55,
                                        sortKey: "AM",
                                        raiinNo: 123456444,
                                        futanGaku: 9,
                                        totalGaku: 99,
                                        biko:"bi",
                                        isDeleted:0,
                                        seqNo: 0
                                       ),
                };

                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 2, "", "Group Name") }, maxMoneys, insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptLimits = tenantNoTracking.LimitListInfs.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptLimits.Any());
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptLimits.Any())
                {
                    tenantTracking.RemoveRange(ptLimits);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_033_UpdatePatientInfo_Update_Limit()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 0, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };
            var maxMoneys = new List<LimitListModel>()
            {
                new LimitListModel(
                                    id: 0,
                                    kohiId: 30,
                                    sinDate: 20240303,
                                    hokenPid: 55,
                                    sortKey: "AM",
                                    raiinNo: 123456444,
                                    futanGaku: 9,
                                    totalGaku: 99,
                                    biko:"bi",
                                    isDeleted:0,
                                    seqNo: 0
                                   ),
            };


            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<LimitListInf> ptLimits = new();

            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, insurances, new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 1, "001", "Group Name"), new GroupInfModel(1, 98422233, 2, "002", "Group Name") }, maxMoneys, insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
                {
                    new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
                };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );

                var ptLimit = tenantNoTracking.LimitListInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var updateMaxMoneys = new List<LimitListModel>()
                {
                    new LimitListModel(
                                        id: ptLimit?.Id ?? 0,
                                        kohiId: 30,
                                        sinDate: 99999999,
                                        hokenPid: 55,
                                        sortKey: "AM1",
                                        raiinNo: 123456444,
                                        futanGaku: 9,
                                        totalGaku: 99,
                                        biko:"bi1",
                                        isDeleted:0,
                                        seqNo: ptLimit?.SeqNo ?? 0
                                       ),
                };

                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 2, "", "Group Name") }, updateMaxMoneys, insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptLimits = tenantNoTracking.LimitListInfs.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptLimits.Any(l => l.SortKey == "AM1" && l.Biko == "bi1" && l.SinDate == 99999999));
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptLimits.Any())
                {
                    tenantTracking.RemoveRange(ptLimits);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_034_UpdatePatientInfo_Delete_Limit()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 0, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };
            var maxMoneys = new List<LimitListModel>()
            {
                new LimitListModel(
                                    id: 0,
                                    kohiId: 30,
                                    sinDate: 20240303,
                                    hokenPid: 55,
                                    sortKey: "AM",
                                    raiinNo: 123456444,
                                    futanGaku: 9,
                                    totalGaku: 99,
                                    biko:"bi",
                                    isDeleted:0,
                                    seqNo: 0
                                   ),
            };
            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<LimitListInf> ptLimits = new();

            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, insurances, new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 1, "001", "Group Name"), new GroupInfModel(1, 98422233, 2, "002", "Group Name") }, maxMoneys, insuranceScanModel, 9999);
                var ptSanteiUpdate = new List<CalculationInfModel>()
                {
                    new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
                };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );


                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 2, "", "Group Name") }, new(), insuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptLimits = tenantNoTracking.LimitListInfs.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId && x.IsDeleted == 1).ToList();

                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptLimits.Any());

            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptLimits.Any())
                {
                    tenantTracking.RemoveRange(ptLimits);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_035_UpdatePatientInfo_Add_Scan()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 0, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };

            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtHokenScan> ptScans = new();

            try
            {

                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, insurances, new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 1, "001", "Group Name"), new GroupInfModel(1, 98422233, 2, "002", "Group Name") }, new(), insuranceScanModel, 9999);

                var ptSanteiUpdate = new List<CalculationInfModel>()
                {
                    new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
                };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );

                Func<int, long, long, IEnumerable<InsuranceScanModel>> updateInsuranceScanModel =
                (hpId, ptId, seqNo) =>
                {
                    var insuranceScanModels = new List<InsuranceScanModel>()
                    {
                                   new InsuranceScanModel(1, createPtInf.ptId, 0, 11, 40, "Unit-Test", GetSampleFileStream(), 0, "20230101"),
                    };
                    return insuranceScanModels;
                };
                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 2, "", "Group Name") }, new(), updateInsuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptScans = tenantNoTracking.PtHokenScans.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptScans.Any());
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptScans.Any())
                {
                    tenantTracking.RemoveRange(ptScans);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_036_UpdatePatientInfo_Update_Scan()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 0, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };

            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtHokenScan> ptScans = new();

            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, insurances, new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 1, "001", "Group Name"), new GroupInfModel(1, 98422233, 2, "002", "Group Name") }, new(), insuranceScanModel, 9999);

                tenantNoTracking.PtHokenScans.Add(new PtHokenScan
                {
                    PtId = createPtInf.ptId,
                    HpId = 1,
                    HokenGrp = 1,
                    HokenId = 1,
                    SeqNo = 0,
                    FileName = "Unit-Test"
                });
                tenantNoTracking.SaveChanges();

                var ptSanteiUpdate = new List<CalculationInfModel>()
                {
                    new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
                };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );

                var oldScan = tenantNoTracking.PtHokenScans.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                Func<int, long, long, IEnumerable<InsuranceScanModel>> updateInsuranceScanModel =
                (hpId, ptId, seqNo) =>
                {
                    var insuranceScanModels = new List<InsuranceScanModel>()
                    {
                                   new InsuranceScanModel(1, createPtInf.ptId, oldScan?.SeqNo ?? 0, 11, 40, "Unit-Test 1", GetSampleFileStream(), 0, "20230101"),
                    };
                    return insuranceScanModels;
                };
                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 2, "", "Group Name") }, new(), updateInsuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptScans = tenantNoTracking.PtHokenScans.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf?.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf?.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf?.Sex, Is.EqualTo(1));
                Assert.That(ptInf?.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf?.IsDead, Is.EqualTo(1));
                Assert.That(santei?.HpId, Is.EqualTo(1));
                Assert.That(santei?.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptScans.Any(s => s.FileName == "Unit-Test 1"));
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptScans.Any())
                {
                    tenantTracking.RemoveRange(ptScans);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_037_UpdatePatientInfo_Delete_Scan()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 98422233,
                                            ptNum: 1,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 20240312,
                                            mail: "sample@mail.com",
                                            homePost: "123-456",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-654",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-210",
                                            officeAddress1: "Sample Office Address 1",
                                            officeAddress2: "Sample Office Address 2",
                                            officeTel: "888-9999",
                                            officeMemo: "Sample Office Memo",
                                            isRyosyoDetail: 1,
                                            primaryDoctor: 2,
                                            isTester: 0,
                                            mainHokenPid: 3,
                                            referenceNo: 987654321,
                                            limitConsFlg: 1,
                                            memo: ""
            );
            var ptNumAuto = savePatientInfo.GetAutoPtNum(1);

            Func<int, long, long, IEnumerable<InsuranceScanModel>> insuranceScanModel = (param1, param2, param3) => Enumerable.Empty<InsuranceScanModel>();

            var ptSantei = new List<CalculationInfModel>()
            {
                new CalculationInfModel(1, 98422233, 1, 1, 1, 20230101, 20240101, 9999),
            };
            var insurances = new List<InsuranceModel>()
            {
                new InsuranceModel(1, 98422233, 19900101, 0, 4, 4, 4, 20230505, "Sample Memo", new(), new(), new(), new(), new(), 0, 20230606, 20240101, isAddNew:true),
            };

            PtInf? ptInf = null;
            PtSanteiConf? santei = null;
            List<PtHokenScan> ptScans = new();

            try
            {
                // Act
                var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, insurances, new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 1, "001", "Group Name"), new GroupInfModel(1, 98422233, 2, "002", "Group Name") }, new(), insuranceScanModel, 9999);

                tenantNoTracking.PtHokenScans.Add(new PtHokenScan
                {
                    PtId = createPtInf.ptId,
                    HpId = 1,
                    HokenGrp = 1,
                    HokenId = 1,
                    SeqNo = 0,
                    FileName = "Unit-Test"
                });
                tenantNoTracking.SaveChanges();

                var ptSanteiUpdate = new List<CalculationInfModel>()
                {
                    new CalculationInfModel(1, 98422233, 1, 1, 2, 20230101, 20240101, 9999),
                };
                var oldPtInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                var patientInfoSaveModelUpdate = new PatientInforSaveModel(
                                                hpId: 1,
                                                ptId: createPtInf.ptId,
                                                ptNum: oldPtInf.PtNum,
                                                kanaName: "Sample Kana Name 1",
                                                name: "Sample Name 1",
                                                sex: 1,
                                                birthday: 19900101,
                                                isDead: 1,
                                                deathDate: 20240312,
                                                mail: "sample@mail.com",
                                                homePost: "123-456",
                                                homeAddress1: "Sample Home Address 1",
                                                homeAddress2: "Sample Home Address 2",
                                                tel1: "123-456-7890",
                                                tel2: "987-654-3210",
                                                setanusi: "Sample Setanusi",
                                                zokugara: "Sample Zokugara",
                                                job: "Sample Job",
                                                renrakuName: "Sample Renraku Name",
                                                renrakuPost: "987-654",
                                                renrakuAddress1: "Sample Renraku Address 1",
                                                renrakuAddress2: "Sample Renraku Address 2",
                                                renrakuTel: "555-1234",
                                                renrakuMemo: "Sample Renraku Memo",
                                                officeName: "Sample Office Name",
                                                officePost: "543-210",
                                                officeAddress1: "Sample Office Address 1",
                                                officeAddress2: "Sample Office Address 2",
                                                officeTel: "888-9999",
                                                officeMemo: "Sample Office Memo",
                                                isRyosyoDetail: 1,
                                                primaryDoctor: 2,
                                                isTester: 0,
                                                mainHokenPid: 3,
                                                referenceNo: 987654321,
                                                limitConsFlg: 1,
                                                memo: ""
                );

                var oldScan = tenantNoTracking.PtHokenScans.FirstOrDefault(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                Func<int, long, long, IEnumerable<InsuranceScanModel>> updateInsuranceScanModel =
                (hpId, ptId, seqNo) =>
                {

                    var insuranceScanModels = new List<InsuranceScanModel>()
                    {
                                   new InsuranceScanModel(1, createPtInf.ptId, oldScan?.SeqNo ?? 0, 11, 40, "Unit-Test 1", GetSampleFileStream(), DeleteTypes.Deleted, "20230101"),
                    };
                    return insuranceScanModels;
                };
                var updatePtInf = savePatientInfo.UpdatePatientInfo(patientInfoSaveModelUpdate, new(), ptSantei, new(), new(), new(), new List<GroupInfModel>() { new GroupInfModel(1, 98422233, 2, "", "Group Name") }, new(), updateInsuranceScanModel, 9999, new List<int> { 1, 2, 3 });

                ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                santei = tenantTracking.PtSanteiConfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
                ptScans = tenantNoTracking.PtHokenScans.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId && x.IsDeleted == DeleteTypes.Deleted).ToList();

                // Assert
                Assert.That(createPtInf.resultSave, Is.EqualTo(true));
                Assert.That(updatePtInf.resultSave, Is.EqualTo(true));
                Assert.That(ptInf.KanaName, Is.EqualTo("Sample Kana Name 1"));
                Assert.That(ptInf.Name, Is.EqualTo("Sample Name 1"));
                Assert.That(ptInf.Sex, Is.EqualTo(1));
                Assert.That(ptInf.Birthday, Is.EqualTo(19900101));
                Assert.That(ptInf.IsDead, Is.EqualTo(1));
                Assert.That(santei.HpId, Is.EqualTo(1));
                Assert.That(santei.PtId, Is.EqualTo(createPtInf.ptId));
                Assert.That(ptScans.Any());
            }
            finally
            {
                if (ptInf != null)
                {
                    tenantTracking.PtInfs.Remove(ptInf);
                }
                if (santei != null)
                {
                    tenantTracking.PtSanteiConfs.Remove(santei);
                }
                if (ptScans.Any())
                {
                    tenantTracking.RemoveRange(ptScans);
                }
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_038_PatientInforRepository_PtKyuseiInfModels()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            int hpId = 99999999;
            long ptId = 28032001;
            bool isDeleted = true;

            PtKyusei ptKyusei = new PtKyusei()
            {
                HpId = hpId,
                PtId = ptId,
            };

            tenantNoTracking.Add(ptKyusei);

            try
            {
                // Act
                tenantNoTracking.SaveChanges();
                var result = savePatientInfo.PtKyuseiInfModels(hpId, ptId, isDeleted);

                // Assert
                Assert.That(result.Any(x => x.HpId == hpId && x.PtId == ptId) == true);
            }
            finally
            {
                savePatientInfo.ReleaseResource();
                tenantNoTracking.PtKyuseis.Remove(ptKyusei);
                tenantNoTracking.SaveChanges();
            }
        }

        #region SavePtKyusei

        [Test]
        public void TC_039_PatientInforRepository_SavePtKyusei_AddNew()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            int hpId = 99999999;
            int userId = 28032001;
            long ptId = 28032001;
            long seqNo = 0;
            string kanaName = "";
            string name = "";
            int endDate = 0;
            bool isDeleted = false;

            List<PtKyuseiModel> ptKyuseiModels = new List<PtKyuseiModel>()
            {
                new PtKyuseiModel(hpId, ptId, seqNo, kanaName, name, endDate, isDeleted)
            };

            List<PtKyusei> ptKyuseis = new List<PtKyusei>();

            try
            {
                // Act
                var result = savePatientInfo.SavePtKyusei(hpId, userId, ptKyuseiModels);
                ptKyuseis = tenantNoTracking.PtKyuseis.Where(x => x.HpId == hpId && x.PtId == ptId).ToList();

                // Assert
                Assert.That(result == true && ptKyuseis.Any(x => x.HpId == hpId && x.PtId == ptId) == true);
            }
            finally
            {
                savePatientInfo.ReleaseResource();
                tenantNoTracking.PtKyuseis.RemoveRange(ptKyuseis);
                tenantNoTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_040_PatientInforRepository_SavePtKyusei_Update()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            int hpId = 99999999;
            int userId = 28032001;
            long ptId = 28032001;
            long seqNo = 99999999;
            string kanaName = "Kaito";
            string name = "";
            int endDate = 0;
            bool isDeleted = false;

            List<PtKyuseiModel> ptKyuseiModels = new List<PtKyuseiModel>()
            {
                new PtKyuseiModel(hpId, ptId, seqNo, kanaName, name, endDate, isDeleted)
            };

            List<PtKyusei> ptKyuseis = new List<PtKyusei>();

            PtKyusei ptKyusei = new PtKyusei()
            {
                HpId = hpId,
                PtId = ptId,
                SeqNo = seqNo
            };

            tenantNoTracking.Add(ptKyusei);

            try
            {
                // Act
                tenantNoTracking.SaveChanges();
                var result = savePatientInfo.SavePtKyusei(hpId, userId, ptKyuseiModels);
                ptKyuseis = tenantNoTracking.PtKyuseis.Where(x => x.HpId == hpId && x.PtId == ptId && x.KanaName == kanaName).ToList();

                // Assert
                Assert.That(result == true && ptKyuseis.Any() == true);
            }
            finally
            {
                savePatientInfo.ReleaseResource();
                tenantNoTracking.PtKyuseis.RemoveRange(ptKyusei);
                tenantNoTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_041_PatientInforRepository_SavePtKyusei_Delete()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            int hpId = 99999999;
            int userId = 28032001;
            long ptId = 28032001;
            long seqNo = 99999999;
            string kanaName = "Kaito";
            string name = "";
            int endDate = 0;
            bool isDeleted = true;

            List<PtKyuseiModel> ptKyuseiModels = new List<PtKyuseiModel>()
            {
                new PtKyuseiModel(hpId, ptId, seqNo, kanaName, name, endDate, isDeleted)
            };

            List<PtKyusei> ptKyuseis = new List<PtKyusei>();

            PtKyusei ptKyusei = new PtKyusei()
            {
                HpId = hpId,
                PtId = ptId,
                SeqNo = seqNo
            };

            tenantNoTracking.Add(ptKyusei);

            try
            {
                // Act
                tenantNoTracking.SaveChanges();
                var result = savePatientInfo.SavePtKyusei(hpId, userId, ptKyuseiModels);
                ptKyuseis = tenantNoTracking.PtKyuseis.Where(x => x.HpId == hpId && x.PtId == ptId && x.KanaName == kanaName && x.IsDeleted == 1).ToList();

                // Assert
                Assert.That(result == true && ptKyuseis.Any() == true);
            }
            finally
            {
                savePatientInfo.ReleaseResource();
                tenantNoTracking.PtKyuseis.RemoveRange(ptKyusei);
                tenantNoTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_042_PatientInforRepository_SavePtKyusei_False()
        {
            // Arrange
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            int hpId = 99999999;
            int userId = 28032001;

            List<PtKyuseiModel> ptKyuseiModels = new List<PtKyuseiModel>();

            // Act
            var result = savePatientInfo.SavePtKyusei(hpId, userId, ptKyuseiModels);

            // Assert
            Assert.That(result == false);
        }
        #endregion

        private static Stream GetSampleFileStream()
        {
            byte[] data = Encoding.UTF8.GetBytes("Sample file content.");
            return new MemoryStream(data);
        }
    }
}
