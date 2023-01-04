using Domain.Common;
using Domain.Models.Diseases;
using Domain.Models.OrdInfDetails;

namespace Domain.Models.MedicalExamination
{
    public interface IMedicalExaminationRepository : IRepositoryBase
    {
        List<CheckedOrderModel> IgakuTokusitu(int sinDate, List<PtDiseaseModel> ByomeiModelList, List<OrdInfDetailModel> allOdrInfDetail, bool isJouhou);
    }   
}
