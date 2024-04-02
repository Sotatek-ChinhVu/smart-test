using Domain.Models.Yousiki.CommonModel.CommonOutputModel;

namespace Domain.Models.Yousiki.CommonModel;

public class CommonModel
{
    public CommonModel(List<Yousiki1InfDetailModel> yousiki1InfDetailList, List<CommonForm1Model> diagnosticInjuryList, InputByomeiCommonModel hospitalizationStatusInf, InputByomeiCommonModel finalExaminationInf)
    {
        Yousiki1InfDetailList = yousiki1InfDetailList;
        DiagnosticInjuryList = diagnosticInjuryList;
        HospitalizationStatusInf = hospitalizationStatusInf;
        FinalExaminationInf = finalExaminationInf;
    }

    public List<Yousiki1InfDetailModel> Yousiki1InfDetailList { get; private set; }

    public List<CommonForm1Model> DiagnosticInjuryList { get; private set; }

    public InputByomeiCommonModel HospitalizationStatusInf { get; private set; }

    public InputByomeiCommonModel FinalExaminationInf { get; private set; }
}
