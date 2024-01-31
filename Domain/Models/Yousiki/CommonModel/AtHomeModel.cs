using Domain.Models.Yousiki;
using Domain.Models.Yousiki.CommonModel.CommonOutputModel;

namespace Domain.Models.Yousiki.CommonModel;

public class AtHomeModel
{
    public AtHomeModel(List<Yousiki1InfDetailModel> yousiki1InfDetailList, List<StatusVisitModel> statusVisitList, List<StatusVisitModel> statusVisitNursingList, List<StatusEmergencyConsultationModel> statusEmergencyConsultationList, List<StatusShortTermAdmissionModel> statusShortTermAdmissionList, List<PatientSitutationModel> patientSitutationList, List<BarthelIndexModel> barthelIndexList, List<StatusNurtritionModel> statusNurtritionList, List<CommonForm1Model> hospitalizationStatusList, List<CommonForm1Model> statusHomeVisitList)
    {
        Yousiki1InfDetailList = yousiki1InfDetailList;
        StatusVisitList = statusVisitList;
        StatusVisitNursingList = statusVisitNursingList;
        StatusEmergencyConsultationList = statusEmergencyConsultationList;
        StatusShortTermAdmissionList = statusShortTermAdmissionList;
        PatientSitutationList = patientSitutationList;
        BarthelIndexList = barthelIndexList;
        StatusNurtritionList = statusNurtritionList;
        HospitalizationStatusList = hospitalizationStatusList;
        StatusHomeVisitList = statusHomeVisitList;
    }

    public List<Yousiki1InfDetailModel> Yousiki1InfDetailList { get; private set; }

    public List<StatusVisitModel> StatusVisitList { get; private set; }

    public List<StatusVisitModel> StatusVisitNursingList { get; private set; }

    public List<StatusEmergencyConsultationModel> StatusEmergencyConsultationList { get; private set; }

    public List<StatusShortTermAdmissionModel> StatusShortTermAdmissionList { get; private set; }

    public List<PatientSitutationModel> PatientSitutationList { get; private set; }

    public List<BarthelIndexModel> BarthelIndexList { get; private set; }

    public List<StatusNurtritionModel> StatusNurtritionList { get; private set; }

    public List<CommonForm1Model> HospitalizationStatusList { get; private set; }

    public List<CommonForm1Model> StatusHomeVisitList { get; private set; }
}
