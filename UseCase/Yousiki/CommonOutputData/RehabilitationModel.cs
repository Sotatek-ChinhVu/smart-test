using Domain.Models.Yousiki;
using UseCase.Yousiki.CommonOutputData.CommonOutputModel;

namespace UseCase.Yousiki.CommonOutputData;

public class RehabilitationModel
{
    public RehabilitationModel(List<Yousiki1InfDetailModel> yousiki1InfDetailList, List<OutpatientConsultationModel> outpatientConsultationList, List<CommonForm1Model> byomeiRehabilitationList, List<PatientStatusModel> barthelIndexList, List<PatientStatusModel> fIMList)
    {
        Yousiki1InfDetailList = yousiki1InfDetailList;
        OutpatientConsultationList = outpatientConsultationList;
        ByomeiRehabilitationList = byomeiRehabilitationList;
        BarthelIndexList = barthelIndexList;
        FIMList = fIMList;
    }

    public List<Yousiki1InfDetailModel> Yousiki1InfDetailList { get; private set; }

    public List<OutpatientConsultationModel> OutpatientConsultationList { get; private set; }

    public List<CommonForm1Model> ByomeiRehabilitationList { get; private set; }

    public List<PatientStatusModel> BarthelIndexList { get; private set; }

    public List<PatientStatusModel> FIMList { get; private set; }
}
