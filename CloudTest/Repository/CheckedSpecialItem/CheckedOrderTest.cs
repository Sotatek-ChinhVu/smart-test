using Domain.Models.Diseases;
using Domain.Models.OrdInfDetails;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.CheckedSpecialItem;

public class CheckedOrderTest : BaseUT
{
    /// <summary>
    /// Check IgakuItem if it exist then return null
    /// </summary>
    [Test]
    public void IgakuTokusitu_Exist_IgakuItem()
    {
        // Arrange
        int hpId = 1, sinDate = 20221111, hokenId = 10, syosaisinKbn = 1;
        bool isJouhou = true;
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                1,
                10,
                1,
                1,
                1
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "113034010",
                10
            ),
             new OrdInfDetailModel(
                "113001810",
                10
            )
        };

        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider);
        // Act
        var iagkutokusitu = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou);
        // Assert
        Assert.True(iagkutokusitu.Count == 0);
    }

    /// <summary>
    /// Check IsJouHou if it is true then check sinDate and tenMst, if it is false then only check tenMst
    /// </summary>
    [Test]
    public void IgakuTokusitu_IsJouhou()
    {
        // Arrange
        int hpId = 1, sinDate1 = 20221111, sinDate2 = 20210101, hokenId = 10, syosaisinKbn = 1;
        bool isJouhou1 = true, isJouhou2 = false;
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                1,
                10,
                1,
                1,
                1
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "113034010",
                10
            ), new OrdInfDetailModel(
                "113001810",
                10
            )
        };

        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider);
        // Act
        var iagkutokusitu1 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate1, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou1);
        var iagkutokusitu2 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate2, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou1);
        var iagkutokusitu3 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate2, hokenId, syosaisinKbn, byomeiModelList, ordInfDetailModels, isJouhou2);
        // Assert
        Assert.True(iagkutokusitu1.Count == 0 && iagkutokusitu2.Count == 0 && iagkutokusitu3.Count == 0);
    }

    /// <summary>
    /// Check Syosai
    /// </summary>
    [Test]
    public void IgakuTokusitu_Syosai()
    {
        // Arrange
        int hpId = 1, sinDate1 = 20221111, sinDate2 = 20210101, hokenId = 10, syosaisinKbn1 = 1, syosaisinKbn2 = 6, syosaisinKbn3 = 2, syosaisinKbn4 = 4, syosaisinKbn5 = 8;
        bool isJouhou1 = true, isJouhou2 = false;
        var byomeiModelList = new List<PtDiseaseModel>()
        {
            new PtDiseaseModel(
                1,
                10,
                1,
                1,
                1
            )
        };
        var ordInfDetailModels = new List<OrdInfDetailModel>()
        {
            new OrdInfDetailModel(
                "11303401012",
                10
            ), new OrdInfDetailModel(
                "11300181013",
                10
            )
        };

        MedicalExaminationRepository medicalExaminationRepository = new MedicalExaminationRepository(TenantProvider);
        // Act
        var iagkutokusitu1 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate1, hokenId, syosaisinKbn1, byomeiModelList, ordInfDetailModels, isJouhou1);
        var iagkutokusitu2 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate2, hokenId, syosaisinKbn2, byomeiModelList, ordInfDetailModels, isJouhou1);
        var iagkutokusitu3 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate2, hokenId, syosaisinKbn3, byomeiModelList, ordInfDetailModels, isJouhou2);
        var iagkutokusitu4 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate2, hokenId, syosaisinKbn4, byomeiModelList, ordInfDetailModels, isJouhou2);
        var iagkutokusitu5 = medicalExaminationRepository.IgakuTokusitu(hpId, sinDate2, hokenId, syosaisinKbn5, byomeiModelList, ordInfDetailModels, isJouhou2);
        // Assert
        Assert.True(iagkutokusitu1.Count == 0 && iagkutokusitu2.Count == 0 && iagkutokusitu3.Count == 0 && iagkutokusitu4.Count == 0 && iagkutokusitu5.Count == 0);
    }
}