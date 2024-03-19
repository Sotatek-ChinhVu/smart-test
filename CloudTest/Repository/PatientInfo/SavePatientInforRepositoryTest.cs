using Domain.Constant;
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
            //Mock
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);
            // Arrange
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

            // Act
            var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), new(), new(), new(), new(), new(), new(), insuranceScanModel, 9999);

            var ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            Assert.That(createPtInf.resultSave, Is.EqualTo(true));
            Assert.That(ptInf.KanaName, Is.EqualTo("Sample Kana Name"));
            Assert.That(ptInf.Name, Is.EqualTo("Sample Name"));
            Assert.That(ptInf.Sex, Is.EqualTo(1));
            Assert.That(ptInf.Birthday, Is.EqualTo(19900101));

            if (createPtInf.resultSave)
            {
                tenantTracking.PtInfs.Remove(ptInf);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_002_CreatePatientInfo_PtNum_Is_Greater_Than_0()
        {
            //Mock
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);
            // Arrange
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

            // Act
            var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), new(), new(), new(), new(), new(), new(), insuranceScanModel, 9999);

            var ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            Assert.That(createPtInf.resultSave, Is.EqualTo(true));
            Assert.That(ptInf.KanaName, Is.EqualTo("Sample Kana Name"));
            Assert.That(ptInf.Name, Is.EqualTo("Sample Name"));
            Assert.That(ptInf.Sex, Is.EqualTo(1));
            Assert.That(ptInf.Birthday, Is.EqualTo(19900101));
            Assert.That(ptInf.IsDead, Is.EqualTo(0));

            if (createPtInf.resultSave)
            {
                tenantTracking.PtInfs.Remove(ptInf);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_003_CreatePatientInfo_DeathDate_Is_Greater_Than_0()
        {
            //Mock
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);
            // Arrange
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

            // Act
            var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), new(), new(), new(), new(), new(), new(), insuranceScanModel, 9999);

            var ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            Assert.That(createPtInf.resultSave, Is.EqualTo(true));
            Assert.That(ptInf.KanaName, Is.EqualTo("Sample Kana Name"));
            Assert.That(ptInf.Name, Is.EqualTo("Sample Name"));
            Assert.That(ptInf.Sex, Is.EqualTo(1));
            Assert.That(ptInf.Birthday, Is.EqualTo(19900101));
            Assert.That(ptInf.IsDead, Is.EqualTo(1));

            if (createPtInf.resultSave)
            {
                tenantTracking.PtInfs.Remove(ptInf);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_004_CreatePatientInfo_Save_PtSanteis()
        {
            //Mock
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);
            // Arrange
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

            // Act
            var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), new(), new(), insuranceScanModel, 9999);

            var ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var santei = tenantTracking.PtSanteiConfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            Assert.That(createPtInf.resultSave, Is.EqualTo(true));
            Assert.That(ptInf.KanaName, Is.EqualTo("Sample Kana Name"));
            Assert.That(ptInf.Name, Is.EqualTo("Sample Name"));
            Assert.That(ptInf.Sex, Is.EqualTo(1));
            Assert.That(ptInf.Birthday, Is.EqualTo(19900101));
            Assert.That(ptInf.IsDead, Is.EqualTo(1));
            Assert.That(santei.HpId, Is.EqualTo(1));
            Assert.That(santei.PtId, Is.EqualTo(createPtInf.ptId));

            if (createPtInf.resultSave)
            {
                tenantTracking.PtInfs.Remove(ptInf);
                tenantTracking.PtSanteiConfs.Remove(santei);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_005_CreatePatientInfo_Save_PtMemos()
        {
            //Mock
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);
            // Arrange
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

            // Act
            var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), new(), new(), insuranceScanModel, 9999);

            var ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var santei = tenantTracking.PtSanteiConfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var memo = tenantTracking.PtMemos.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            Assert.That(createPtInf.resultSave, Is.EqualTo(true));
            Assert.That(ptInf.KanaName, Is.EqualTo("Sample Kana Name"));
            Assert.That(ptInf.Name, Is.EqualTo("Sample Name"));
            Assert.That(ptInf.Sex, Is.EqualTo(1));
            Assert.That(ptInf.Birthday, Is.EqualTo(19900101));
            Assert.That(ptInf.IsDead, Is.EqualTo(1));
            Assert.That(santei.HpId, Is.EqualTo(1));
            Assert.That(santei.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(memo.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(memo.HpId, Is.EqualTo(1));

            if (createPtInf.resultSave)
            {
                tenantTracking.PtInfs.Remove(ptInf);
                tenantTracking.PtSanteiConfs.Remove(santei);
                tenantTracking.PtMemos.Remove(memo);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_006_CreatePatientInfo_Save_PtGrps()
        {
            //Mock
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);
            // Arrange
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

            // Act
            var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), ptSantei, new(), new(), new(), ptGrps, new(), insuranceScanModel, 9999);

            var ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var santei = tenantTracking.PtSanteiConfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var memo = tenantTracking.PtMemos.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptGrp = tenantTracking.PtGrpInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            Assert.That(createPtInf.resultSave, Is.EqualTo(true));
            Assert.That(ptInf.KanaName, Is.EqualTo("Sample Kana Name"));
            Assert.That(ptInf.Name, Is.EqualTo("Sample Name"));
            Assert.That(ptInf.Sex, Is.EqualTo(1));
            Assert.That(ptInf.Birthday, Is.EqualTo(19900101));
            Assert.That(ptInf.IsDead, Is.EqualTo(1));
            Assert.That(santei.HpId, Is.EqualTo(1));
            Assert.That(santei.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(memo.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(memo.HpId, Is.EqualTo(1));
            Assert.That(ptGrp.HpId, Is.EqualTo(1));
            Assert.That(ptGrp.PtId, Is.EqualTo(createPtInf.ptId));

            if (createPtInf.resultSave)
            {
                tenantTracking.PtInfs.Remove(ptInf);
                tenantTracking.PtSanteiConfs.Remove(santei);
                tenantTracking.PtMemos.Remove(memo);
                tenantTracking.PtGrpInfs.Remove(ptGrp);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_007_CreatePatientInfo_Save_PtKyuseis()
        {
            //Mock
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);
            // Arrange
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

            // Act
            var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, ptKyuseis, ptSantei, new(), new(), new(), ptGrps, new(), insuranceScanModel, 9999);

            var ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var santei = tenantTracking.PtSanteiConfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var memo = tenantTracking.PtMemos.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptGrp = tenantTracking.PtGrpInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptKyusei = tenantTracking.PtKyuseis.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            Assert.That(createPtInf.resultSave, Is.EqualTo(true));
            Assert.That(ptInf.KanaName, Is.EqualTo("Sample Kana Name"));
            Assert.That(ptInf.Name, Is.EqualTo("Sample Name"));
            Assert.That(ptInf.Sex, Is.EqualTo(1));
            Assert.That(ptInf.Birthday, Is.EqualTo(19900101));
            Assert.That(ptInf.IsDead, Is.EqualTo(1));
            Assert.That(santei.HpId, Is.EqualTo(1));
            Assert.That(santei.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(memo.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(memo.HpId, Is.EqualTo(1));
            Assert.That(ptGrp.HpId, Is.EqualTo(1));
            Assert.That(ptGrp.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(ptKyusei.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(ptKyusei.HpId, Is.EqualTo(1));

            if (createPtInf.resultSave)
            {
                tenantTracking.PtInfs.Remove(ptInf);
                tenantTracking.PtSanteiConfs.Remove(santei);
                tenantTracking.PtMemos.Remove(memo);
                tenantTracking.PtGrpInfs.Remove(ptGrp);
                tenantTracking.PtKyuseis.Remove(ptKyusei);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_008_CreatePatientInfo_Save_PthokenPartterns()
        {
            //Mock
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);
            // Arrange
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

            // Act
            var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, ptKyuseis, ptSantei, insurances, new(), new(), ptGrps, new(), insuranceScanModel, 9999);

            var ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var santei = tenantTracking.PtSanteiConfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var memo = tenantTracking.PtMemos.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptGrp = tenantTracking.PtGrpInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptKyusei = tenantTracking.PtKyuseis.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptHokenPattern = tenantTracking.PtHokenPatterns.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            Assert.That(createPtInf.resultSave, Is.EqualTo(true));
            Assert.That(ptInf.KanaName, Is.EqualTo("Sample Kana Name"));
            Assert.That(ptInf.Name, Is.EqualTo("Sample Name"));
            Assert.That(ptInf.Sex, Is.EqualTo(1));
            Assert.That(ptInf.Birthday, Is.EqualTo(19900101));
            Assert.That(ptInf.IsDead, Is.EqualTo(1));
            Assert.That(santei.HpId, Is.EqualTo(1));
            Assert.That(santei.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(memo.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(memo.HpId, Is.EqualTo(1));
            Assert.That(ptGrp.HpId, Is.EqualTo(1));
            Assert.That(ptGrp.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(ptKyusei.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(ptKyusei.HpId, Is.EqualTo(1));
            Assert.That(ptHokenPattern.HpId, Is.EqualTo(1));
            Assert.That(ptHokenPattern.PtId, Is.EqualTo(createPtInf.ptId));

            if (createPtInf.resultSave)
            {
                tenantTracking.PtInfs.Remove(ptInf);
                tenantTracking.PtSanteiConfs.Remove(santei);
                tenantTracking.PtMemos.Remove(memo);
                tenantTracking.PtGrpInfs.Remove(ptGrp);
                tenantTracking.PtKyuseis.Remove(ptKyusei);
                tenantTracking.PtHokenPatterns.Remove(ptHokenPattern);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_009_CreatePatientInfo_Save_PtHokenInfs()
        {
            //Mock
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);
            // Arrange
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

            // Act
            var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, ptKyuseis, ptSantei, insurances, hokenInfs, new(), ptGrps, new(), insuranceScanModel, 9999);

            var ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var santei = tenantTracking.PtSanteiConfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var memo = tenantTracking.PtMemos.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptGrp = tenantTracking.PtGrpInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptKyusei = tenantTracking.PtKyuseis.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptHokenPattern = tenantTracking.PtHokenPatterns.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var pthokenInf = tenantTracking.PtHokenInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptRousaiTenkis = tenantTracking.PtRousaiTenkis.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId && x.HokenId == 789012);
            var ptHokenChecks = tenantTracking.PtHokenChecks.First(x => x.HpId == 1 && x.HokenId == 789012 && x.PtID == createPtInf.ptId);
            Assert.That(createPtInf.resultSave, Is.EqualTo(true));
            Assert.That(ptInf.KanaName, Is.EqualTo("Sample Kana Name"));
            Assert.That(ptInf.Name, Is.EqualTo("Sample Name"));
            Assert.That(ptInf.Sex, Is.EqualTo(1));
            Assert.That(ptInf.Birthday, Is.EqualTo(19900101));
            Assert.That(ptInf.IsDead, Is.EqualTo(1));
            Assert.That(santei.HpId, Is.EqualTo(1));
            Assert.That(santei.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(memo.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(memo.HpId, Is.EqualTo(1));
            Assert.That(ptGrp.HpId, Is.EqualTo(1));
            Assert.That(ptGrp.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(ptKyusei.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(ptKyusei.HpId, Is.EqualTo(1));
            Assert.That(ptHokenPattern.HpId, Is.EqualTo(1));
            Assert.That(ptHokenPattern.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(pthokenInf.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(pthokenInf.EndDate, Is.EqualTo(20221231));
            Assert.That(ptRousaiTenkis.Tenki, Is.EqualTo(2));
            Assert.That(ptRousaiTenkis.Sinkei, Is.EqualTo(1));
            Assert.That(ptRousaiTenkis.HokenId, Is.EqualTo(789012));
            Assert.That(ptHokenChecks.HokenId, Is.EqualTo(789012));

            if (createPtInf.resultSave)
            {
                tenantTracking.PtInfs.Remove(ptInf);
                tenantTracking.PtSanteiConfs.Remove(santei);
                tenantTracking.PtMemos.Remove(memo);
                tenantTracking.PtGrpInfs.Remove(ptGrp);
                tenantTracking.PtKyuseis.Remove(ptKyusei);
                tenantTracking.PtHokenPatterns.Remove(ptHokenPattern);
                tenantTracking.PtHokenInfs.Remove(pthokenInf);
                tenantTracking.PtRousaiTenkis.Remove(ptRousaiTenkis);
                tenantTracking.PtHokenChecks.Remove(ptHokenChecks);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_010_CreatePatientInfo_Save_PtKohiInf()
        {
            //Mock
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);
            // Arrange
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

            // Act
            var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, ptKyuseis, ptSantei, insurances, hokenInfs, hokenKohis, ptGrps, new(), insuranceScanModel, 9999);

            var ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var santei = tenantTracking.PtSanteiConfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var memo = tenantTracking.PtMemos.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptGrp = tenantTracking.PtGrpInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptKyusei = tenantTracking.PtKyuseis.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptHokenPattern = tenantTracking.PtHokenPatterns.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var pthokenInf = tenantTracking.PtHokenInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptRousaiTenkis = tenantTracking.PtRousaiTenkis.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId && x.HokenId == 789012);
            var ptHokenChecks = tenantTracking.PtHokenChecks.First(x => x.HpId == 1 && x.HokenId == 789012 && x.PtID == createPtInf.ptId && x.HokenGrp == 1);
            var ptHokenCheckKohis = tenantTracking.PtHokenChecks.First(x => x.HpId == 1 && x.HokenId == 61 && x.PtID == createPtInf.ptId && x.HokenGrp == 2);
            var ptKohis = tenantTracking.PtKohis.First(x => x.HpId == 1 && x.HokenId == 61 && x.PtId == createPtInf.ptId);
            Assert.That(createPtInf.resultSave, Is.EqualTo(true));
            Assert.That(ptInf.KanaName, Is.EqualTo("Sample Kana Name"));
            Assert.That(ptInf.Name, Is.EqualTo("Sample Name"));
            Assert.That(ptInf.Sex, Is.EqualTo(1));
            Assert.That(ptInf.Birthday, Is.EqualTo(19900101));
            Assert.That(ptInf.IsDead, Is.EqualTo(1));
            Assert.That(santei.HpId, Is.EqualTo(1));
            Assert.That(santei.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(memo.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(memo.HpId, Is.EqualTo(1));
            Assert.That(ptGrp.HpId, Is.EqualTo(1));
            Assert.That(ptGrp.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(ptKyusei.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(ptKyusei.HpId, Is.EqualTo(1));
            Assert.That(ptHokenPattern.HpId, Is.EqualTo(1));
            Assert.That(ptHokenPattern.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(pthokenInf.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(pthokenInf.EndDate, Is.EqualTo(20221231));
            Assert.That(ptRousaiTenkis.Tenki, Is.EqualTo(2));
            Assert.That(ptRousaiTenkis.Sinkei, Is.EqualTo(1));
            Assert.That(ptRousaiTenkis.HokenId, Is.EqualTo(789012));
            Assert.That(ptHokenCheckKohis.HokenId, Is.EqualTo(61));

            if (createPtInf.resultSave)
            {
                tenantTracking.PtInfs.Remove(ptInf);
                tenantTracking.PtSanteiConfs.Remove(santei);
                tenantTracking.PtMemos.Remove(memo);
                tenantTracking.PtGrpInfs.Remove(ptGrp);
                tenantTracking.PtKyuseis.Remove(ptKyusei);
                tenantTracking.PtHokenPatterns.Remove(ptHokenPattern);
                tenantTracking.PtHokenInfs.Remove(pthokenInf);
                tenantTracking.PtRousaiTenkis.Remove(ptRousaiTenkis);
                tenantTracking.PtHokenChecks.Remove(ptHokenChecks);
                tenantTracking.PtHokenChecks.Remove(ptHokenCheckKohis);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_011_CreatePatientInfo_Save_Maxmoney()
        {
            //Mock
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);
            // Arrange
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

            // Act
            var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, ptKyuseis, ptSantei, insurances, hokenInfs, hokenKohis, ptGrps, maxMoneys, insuranceScanModel, 9999);

            var ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var santei = tenantTracking.PtSanteiConfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var memo = tenantTracking.PtMemos.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptGrp = tenantTracking.PtGrpInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptKyusei = tenantTracking.PtKyuseis.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptHokenPattern = tenantTracking.PtHokenPatterns.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var pthokenInf = tenantTracking.PtHokenInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptRousaiTenkis = tenantTracking.PtRousaiTenkis.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId && x.HokenId == 789012);
            var ptHokenChecks = tenantTracking.PtHokenChecks.First(x => x.HpId == 1 && x.HokenId == 789012 && x.PtID == createPtInf.ptId && x.HokenGrp == 1);
            var ptHokenCheckKohis = tenantTracking.PtHokenChecks.First(x => x.HpId == 1 && x.HokenId == 61 && x.PtID == createPtInf.ptId && x.HokenGrp == 2);
            var ptKohis = tenantTracking.PtKohis.First(x => x.HpId == 1 && x.HokenId == 61 && x.PtId == createPtInf.ptId);
            var limitInfs = tenantTracking.LimitListInfs.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();

            Assert.That(createPtInf.resultSave, Is.EqualTo(true));
            Assert.That(ptInf.KanaName, Is.EqualTo("Sample Kana Name"));
            Assert.That(ptInf.Name, Is.EqualTo("Sample Name"));
            Assert.That(ptInf.Sex, Is.EqualTo(1));
            Assert.That(ptInf.Birthday, Is.EqualTo(19900101));
            Assert.That(ptInf.IsDead, Is.EqualTo(1));
            Assert.That(santei.HpId, Is.EqualTo(1));
            Assert.That(santei.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(memo.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(memo.HpId, Is.EqualTo(1));
            Assert.That(ptGrp.HpId, Is.EqualTo(1));
            Assert.That(ptGrp.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(ptKyusei.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(ptKyusei.HpId, Is.EqualTo(1));
            Assert.That(ptHokenPattern.HpId, Is.EqualTo(1));
            Assert.That(ptHokenPattern.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(pthokenInf.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(pthokenInf.EndDate, Is.EqualTo(20221231));
            Assert.That(ptRousaiTenkis.Tenki, Is.EqualTo(2));
            Assert.That(ptRousaiTenkis.Sinkei, Is.EqualTo(1));
            Assert.That(ptRousaiTenkis.HokenId, Is.EqualTo(789012));
            Assert.That(ptHokenCheckKohis.HokenId, Is.EqualTo(61));
            Assert.That(limitInfs.Any(), Is.EqualTo(true));

            if (createPtInf.resultSave)
            {
                tenantTracking.PtInfs.Remove(ptInf);
                tenantTracking.PtSanteiConfs.Remove(santei);
                tenantTracking.PtMemos.Remove(memo);
                tenantTracking.PtGrpInfs.Remove(ptGrp);
                tenantTracking.PtKyuseis.Remove(ptKyusei);
                tenantTracking.PtHokenPatterns.Remove(ptHokenPattern);
                tenantTracking.PtHokenInfs.Remove(pthokenInf);
                tenantTracking.PtRousaiTenkis.Remove(ptRousaiTenkis);
                tenantTracking.PtHokenChecks.Remove(ptHokenChecks);
                tenantTracking.PtHokenChecks.Remove(ptHokenCheckKohis);
                tenantTracking.LimitListInfs.RemoveRange(limitInfs);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_012_CreatePatientInfo_Save_InsurancesCan()
        {
            //Mock
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);
            // Arrange
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
                   // Implementation to return IEnumerable<InsuranceScanModel> based on the parameters provided
                   var insuranceScanModels = new List<InsuranceScanModel>()
                   {
                       new InsuranceScanModel(1, ptNumAuto, 0, 11, 40, "Unit-Test", GetSampleFileStream(), 0, "20230101"),
                   };
                   return insuranceScanModels;
               };
            // Act
            var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, ptKyuseis, ptSantei, insurances, hokenInfs, hokenKohis, ptGrps, maxMoneys, insuranceScanModel, 9999);

            var ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var santei = tenantTracking.PtSanteiConfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var memo = tenantTracking.PtMemos.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptGrp = tenantTracking.PtGrpInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptKyusei = tenantTracking.PtKyuseis.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptHokenPattern = tenantTracking.PtHokenPatterns.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var pthokenInf = tenantTracking.PtHokenInfs.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
            var ptRousaiTenkis = tenantTracking.PtRousaiTenkis.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId && x.HokenId == 789012);
            var ptHokenChecks = tenantTracking.PtHokenChecks.First(x => x.HpId == 1 && x.HokenId == 789012 && x.PtID == createPtInf.ptId && x.HokenGrp == 1);
            var ptHokenCheckKohis = tenantTracking.PtHokenChecks.First(x => x.HpId == 1 && x.HokenId == 61 && x.PtID == createPtInf.ptId && x.HokenGrp == 2);
            var ptKohis = tenantTracking.PtKohis.First(x => x.HpId == 1 && x.HokenId == 61 && x.PtId == createPtInf.ptId);
            var limitInfs = tenantTracking.LimitListInfs.Where(x => x.HpId == 1 && x.PtId == createPtInf.ptId).ToList();
            var ptHokenScans = tenantTracking.PtHokenScans.Where(x => x.HpId == 1 && x.PtId == ptNumAuto).ToList();

            Assert.That(createPtInf.resultSave, Is.EqualTo(true));
            Assert.That(ptInf.KanaName, Is.EqualTo("Sample Kana Name"));
            Assert.That(ptInf.Name, Is.EqualTo("Sample Name"));
            Assert.That(ptInf.Sex, Is.EqualTo(1));
            Assert.That(ptInf.Birthday, Is.EqualTo(19900101));
            Assert.That(ptInf.IsDead, Is.EqualTo(1));
            Assert.That(santei.HpId, Is.EqualTo(1));
            Assert.That(santei.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(memo.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(memo.HpId, Is.EqualTo(1));
            Assert.That(ptGrp.HpId, Is.EqualTo(1));
            Assert.That(ptGrp.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(ptKyusei.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(ptKyusei.HpId, Is.EqualTo(1));
            Assert.That(ptHokenPattern.HpId, Is.EqualTo(1));
            Assert.That(ptHokenPattern.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(pthokenInf.PtId, Is.EqualTo(createPtInf.ptId));
            Assert.That(pthokenInf.EndDate, Is.EqualTo(20221231));
            Assert.That(ptRousaiTenkis.Tenki, Is.EqualTo(2));
            Assert.That(ptRousaiTenkis.Sinkei, Is.EqualTo(1));
            Assert.That(ptRousaiTenkis.HokenId, Is.EqualTo(789012));
            Assert.That(ptHokenCheckKohis.HokenId, Is.EqualTo(61));
            Assert.That(limitInfs.Any(), Is.EqualTo(true));
            Assert.That(ptHokenScans.Any(), Is.EqualTo(true));

            if (createPtInf.resultSave)
            {
                tenantTracking.PtInfs.Remove(ptInf);
                tenantTracking.PtSanteiConfs.Remove(santei);
                tenantTracking.PtMemos.Remove(memo);
                tenantTracking.PtGrpInfs.Remove(ptGrp);
                tenantTracking.PtKyuseis.Remove(ptKyusei);
                tenantTracking.PtHokenPatterns.Remove(ptHokenPattern);
                tenantTracking.PtHokenInfs.Remove(pthokenInf);
                tenantTracking.PtRousaiTenkis.Remove(ptRousaiTenkis);
                tenantTracking.PtHokenChecks.Remove(ptHokenChecks);
                tenantTracking.PtHokenChecks.Remove(ptHokenCheckKohis);
                tenantTracking.LimitListInfs.RemoveRange(limitInfs);
                tenantTracking.PtHokenScans.RemoveRange(ptHokenScans);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_013_UpdatePatientInfo_CloneByomeiWithNewHokenId()
        {
            //Mock
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);

            // Arrange
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
                   // Implementation to return IEnumerable<InsuranceScanModel> based on the parameters provided
                   var insuranceScanModels = new List<InsuranceScanModel>()
                   {
                       new InsuranceScanModel(1, ptNumAuto, 0, 11, 40, "Unit-Test", GetSampleFileStream(), 0, "20230101"),
                   };
                   return insuranceScanModels;
               };

            var hokenIds = new List<int>() { 99 };

            var createPatient = CreatePatientData();

            //Add PtByomeis
            var ptByomei = new PtByomei()
            {
                HpId = 1,
                PtId = createPatient.ptId,
                ByomeiCd = "5738002",
                HokenPid = 789012,
                TenkiKbn = TenkiKbnConst.Continued,
                IsDeleted = DeleteTypes.None
            };

            tenantTracking.PtByomeis.Add(ptByomei);

            var updatePattient = savePatientInfo.UpdatePatientInfo(patientInfoSaveModel, ptKyuseis, ptSantei, insurances, hokenInfs, hokenKohis, ptGrps, maxMoneys, insuranceScanModel, 9999, hokenIds);

            var updateByomei = tenantTracking.PtByomeis.Any(x => x.PtId == createPatient.ptId && x.HokenPid == 99);
            try
            {
                Assert.That(updateByomei, Is.EqualTo(true));
            }
            finally
            {
                tenantTracking.Remove(ptByomei);
                DeletePatientData(createPatient.ptNumAuto, createPatient.ptId);

                tenantTracking.SaveChanges();
            }
            
        }

        #region Setup
        private (long ptNumAuto, long ptId) CreatePatientData()
        {
            //Mock
            var mockReceptionRepos = new Mock<IReceptionRepository>();
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);
            // Arrange
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
                   // Implementation to return IEnumerable<InsuranceScanModel> based on the parameters provided
                   var insuranceScanModels = new List<InsuranceScanModel>()
                   {
                       new InsuranceScanModel(1, ptNumAuto, 0, 11, 40, "Unit-Test", GetSampleFileStream(), 0, "20230101"),
                   };
                   return insuranceScanModels;
               };

            // Act
            var createPtInf = savePatientInfo.CreatePatientInfo(patientInfoSaveModel, ptKyuseis, ptSantei, insurances, hokenInfs, hokenKohis, ptGrps, maxMoneys, insuranceScanModel, 9999);

            return (ptNumAuto, createPtInf.ptId);
        }

        private bool DeletePatientData(long ptNumAuto, long ptId)
        {
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var tenantNoTracking = TenantProvider.GetNoTrackingDataContext();
            var ptInf = tenantNoTracking.PtInfs.First(x => x.HpId == 1 && x.PtId == ptId);
            var santei = tenantNoTracking.PtSanteiConfs.First(x => x.HpId == 1 && x.PtId == ptId);
            var memo = tenantNoTracking.PtMemos.First(x => x.HpId == 1 && x.PtId == ptId);
            var ptGrp = tenantNoTracking.PtGrpInfs.First(x => x.HpId == 1 && x.PtId == ptId);
            var ptKyusei = tenantNoTracking.PtKyuseis.First(x => x.HpId == 1 && x.PtId == ptId);
            var ptHokenPattern = tenantNoTracking.PtHokenPatterns.First(x => x.HpId == 1 && x.PtId == ptId);
            var pthokenInf = tenantNoTracking.PtHokenInfs.First(x => x.HpId == 1 && x.PtId == ptId);
            var ptRousaiTenkis = tenantNoTracking.PtRousaiTenkis.First(x => x.HpId == 1 && x.PtId == ptId && x.HokenId == 789012);
            var ptHokenChecks = tenantNoTracking.PtHokenChecks.First(x => x.HpId == 1 && x.HokenId == 789012 && x.PtID == ptId && x.HokenGrp == 1);
            var ptHokenCheckKohis = tenantNoTracking.PtHokenChecks.First(x => x.HpId == 1 && x.HokenId == 61 && x.PtID == ptId && x.HokenGrp == 2);
            var ptKohis = tenantNoTracking.PtKohis.First(x => x.HpId == 1 && x.HokenId == 61 && x.PtId == ptId);
            var limitInfs = tenantNoTracking.LimitListInfs.Where(x => x.HpId == 1 && x.PtId == ptId).ToList();
            var ptHokenScans = tenantNoTracking.PtHokenScans.Where(x => x.HpId == 1 && x.PtId == ptNumAuto).ToList();

            tenantTracking.PtInfs.Remove(ptInf);
            tenantTracking.PtSanteiConfs.Remove(santei);
            tenantTracking.PtMemos.Remove(memo);
            tenantTracking.PtGrpInfs.Remove(ptGrp);
            tenantTracking.PtKyuseis.Remove(ptKyusei);
            tenantTracking.PtHokenPatterns.Remove(ptHokenPattern);
            tenantTracking.PtHokenInfs.Remove(pthokenInf);
            tenantTracking.PtRousaiTenkis.Remove(ptRousaiTenkis);
            tenantTracking.PtHokenChecks.Remove(ptHokenChecks);
            tenantTracking.PtHokenChecks.Remove(ptHokenCheckKohis);
            tenantTracking.LimitListInfs.RemoveRange(limitInfs);
            tenantTracking.PtHokenScans.RemoveRange(ptHokenScans);
            return tenantTracking.SaveChanges() > 0;
        }

        private static Stream GetSampleFileStream()
        {
            byte[] data = Encoding.UTF8.GetBytes("Sample file content.");
            return new MemoryStream(data);
        }

        #endregion
    }
}
