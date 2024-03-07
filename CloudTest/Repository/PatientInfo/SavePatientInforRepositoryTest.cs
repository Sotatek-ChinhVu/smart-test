using Domain.Models.Diseases;
using Domain.Models.Insurance;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Domain.Models.SystemConf;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Interactor.PatientInfor;
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

            var savePatientInfo = new PatientInforRepository(TenantProvider, mockReceptionRepos.Object);
            // Arrange
            var patientInfoSaveModel = new PatientInforSaveModel(
                                            hpId: 1,
                                            ptId: 123456,
                                            ptNum: 0,
                                            kanaName: "Sample Kana Name",
                                            name: "Sample Name",
                                            sex: 1,
                                            birthday: 19900101,
                                            isDead: 0,
                                            deathDate: 0,
                                            mail: "sample@mail.com",
                                            homePost: "123-4567",
                                            homeAddress1: "Sample Home Address 1",
                                            homeAddress2: "Sample Home Address 2",
                                            tel1: "123-456-7890",
                                            tel2: "987-654-3210",
                                            setanusi: "Sample Setanusi",
                                            zokugara: "Sample Zokugara",
                                            job: "Sample Job",
                                            renrakuName: "Sample Renraku Name",
                                            renrakuPost: "987-6543",
                                            renrakuAddress1: "Sample Renraku Address 1",
                                            renrakuAddress2: "Sample Renraku Address 2",
                                            renrakuTel: "555-1234",
                                            renrakuMemo: "Sample Renraku Memo",
                                            officeName: "Sample Office Name",
                                            officePost: "543-2109",
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
                                            memo: "Sample Memo"
            );

            IEnumerable<InsuranceScanModel> insuranceScans = new List<InsuranceScanModel>
                                                            {
                                                                new InsuranceScanModel
                                                                (
                                                                    hpId: 1,
                                                                    ptId: 123456,
                                                                    seqNo: 789012,
                                                                    hokenGrp: 1,
                                                                    hokenId: 2,
                                                                    fileName: "SampleFile.pdf",
                                                                    file: GetSampleFileStream(),
                                                                    isDeleted: 0,
                                                                    updateTime: "2024-01-10T12:34:56"
                                                                )
                                                            };

            // Act
            savePatientInfo.CreatePatientInfo(patientInfoSaveModel, new(), new(), new(), new(), new(), new(), new(), insuranceScans, 9999);

            // Assert
            Assert.That(firstName, Is.EqualTo("太郎"));
            Assert.That(lastName, Is.EqualTo("山田"));
        }

        private static Stream GetSampleFileStream()
        {
            byte[] data = Encoding.UTF8.GetBytes("Sample file content.");
            return new MemoryStream(data);
        }
    }
}
