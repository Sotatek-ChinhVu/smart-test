using Domain.Models.Yousiki;

namespace Domain.Models.Yousiki.CommonModel.CommonOutputModel;

public class OutpatientConsultationInfModel
{
    public OutpatientConsultationInfModel(Yousiki1InfDetailModel consultationDate, Yousiki1InfDetailModel firstVisit, Yousiki1InfDetailModel appearanceReferral, Yousiki1InfDetailModel departmentCode, bool isDeleted)
    {
        ConsultationDate = consultationDate;
        FirstVisit = firstVisit;
        AppearanceReferral = appearanceReferral;
        DepartmentCode = departmentCode;
        IsDeleted = isDeleted;
    }

    public OutpatientConsultationInfModel(Yousiki1InfDetailModel consultationDate, Yousiki1InfDetailModel firstVisit, Yousiki1InfDetailModel appearanceReferral, Yousiki1InfDetailModel departmentCode)
    {
        ConsultationDate = consultationDate;
        FirstVisit = firstVisit;
        AppearanceReferral = appearanceReferral;
        DepartmentCode = departmentCode;
        IsDeleted = false;
    }

    public Yousiki1InfDetailModel ConsultationDate { get; private set; }

    public Yousiki1InfDetailModel FirstVisit { get; private set; }

    public Yousiki1InfDetailModel AppearanceReferral { get; private set; }

    public Yousiki1InfDetailModel DepartmentCode { get; private set; }

    public bool IsDeleted { get; private set; }

    public bool IsEnableReferral { get => FirstVisit.Value == "1"; }
}
