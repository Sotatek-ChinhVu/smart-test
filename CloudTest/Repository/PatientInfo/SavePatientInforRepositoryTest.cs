using Domain.Models.CalculationInf;
using Domain.Models.GroupInf;
using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.InsuranceMst;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Infrastructure.Repositories;
using Moq;

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
                                  edaNo: "E7890",
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
                                  tokki1: "Tokki1",
                                  tokki2: "Tokki2",
                                  tokki3: "Tokki3",
                                  tokki4: "Tokki4",
                                  tokki5: "Tokki5",
                                  rousaiKofuNo: "RousaiKofu123",
                                  rousaiRoudouCd: "R123",
                                  rousaiSaigaiKbn: 0,
                                  rousaiKantokuCd: "K123",
                                  rousaiSyobyoDate: 20221001,
                                  ryoyoStartDate: 20221001,
                                  ryoyoEndDate: 20221231,
                                  rousaiSyobyoCd: "RSC123",
                                  rousaiJigyosyoName: "RousaiJigyosyo",
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
                                  houbetu: "Houbetu123",
                                  confirmDateList: new List<ConfirmDateModel>()
                                                      {
                                                        new ConfirmDateModel(123456, 1, 50, DateTime.UtcNow, 1, "Check Comment"),
                                                      },
                                  listRousaiTenki: new List<RousaiTenkiModel>()
                                                      {
                                                        new RousaiTenkiModel(1, 1, 20240101, 1, 99999),
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
            var ptRousaiTenkis = tenantTracking.PtRousaiTenkis.First(x => x.HpId == 1 && x.PtId == createPtInf.ptId);
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
            Assert.That(pthokenInf.PtRousaiTenkis, Is.EqualTo(20221231));

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
    }
}
