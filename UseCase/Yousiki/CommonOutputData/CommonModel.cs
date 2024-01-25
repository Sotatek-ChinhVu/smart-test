using Domain.Models.Yousiki;
using UseCase.Yousiki.CommonOutputData.CommonOutputModel;

namespace UseCase.Yousiki.CommonOutputData;

public class CommonModel
{
    public CommonModel(List<Yousiki1InfDetailModel> yousiki1InfDetailList, List<CommonForm1Model> diagnosticInjuryList)
    {
        Yousiki1InfDetailList = yousiki1InfDetailList;
        DiagnosticInjuryList = diagnosticInjuryList;
    }

    public List<Yousiki1InfDetailModel> Yousiki1InfDetailList { get; private set; }

    public List<CommonForm1Model> DiagnosticInjuryList { get; private set; }
}
