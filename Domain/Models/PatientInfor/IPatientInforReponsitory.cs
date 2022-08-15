using Domain.Models.PatientInfor.Domain.Models.PatientInfor;

namespace Domain.Models.PatientInfor
{
    public interface IPatientInforRepository
    {
        PatientInforModel? GetById(int hpId, long ptId, bool isDeleted = true);
        List<PatientInforModel> GetById(long ptId);

        List<PatientInforModel> SearchSimple(string keyword, bool isContainMode);
    }
}