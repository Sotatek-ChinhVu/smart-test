using Domain.Models.Yousiki;

namespace Domain.Models.Yousiki.CommonModel.CommonOutputModel;

public class StatusVisitModel
{
    public StatusVisitModel(Yousiki1InfDetailModel sinDate, Yousiki1InfDetailModel medicalInstitution, bool isDeleted)
    {
        SinDate = sinDate;
        MedicalInstitution = medicalInstitution;
        IsDeleted = isDeleted;
    }

    public StatusVisitModel(Yousiki1InfDetailModel sinDate, Yousiki1InfDetailModel medicalInstitution)
    {
        SinDate = sinDate;
        MedicalInstitution = medicalInstitution;
        IsDeleted = false;
    }

    public Yousiki1InfDetailModel SinDate { get; private set; }

    public Yousiki1InfDetailModel MedicalInstitution { get; private set; }

    public bool IsDeleted { get; private set; }
}
