using Domain.Models.Insurance;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
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
            }
        }

        private static Stream GetSampleFileStream()
        {
            byte[] data = Encoding.UTF8.GetBytes("Sample file content.");
            return new MemoryStream(data);
        }
    }
}
