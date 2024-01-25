using Domain.Models.Yousiki;

namespace UseCase.Yousiki.CommonOutputData.CommonOutputModel;

public class StatusShortTermAdmissionModel
{
    public StatusShortTermAdmissionModel(Yousiki1InfDetailModel admissionDate, Yousiki1InfDetailModel dischargeDate, Yousiki1InfDetailModel service, bool isDeleted)
    {
        AdmissionDate = admissionDate;
        DischargeDate = dischargeDate;
        Service = service;
        IsDeleted = isDeleted;
    }

    public StatusShortTermAdmissionModel(Yousiki1InfDetailModel admissionDate, Yousiki1InfDetailModel dischargeDate, Yousiki1InfDetailModel service)
    {
        AdmissionDate = admissionDate;
        DischargeDate = dischargeDate;
        Service = service;
        IsDeleted = false;
    }

    public Yousiki1InfDetailModel AdmissionDate { get; private set; }

    public Yousiki1InfDetailModel DischargeDate { get; private set; }

    public Yousiki1InfDetailModel Service { get; private set; }

    public bool IsDeleted { get; private set; }
}
