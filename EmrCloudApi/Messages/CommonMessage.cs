using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using UseCase.SetMst.GetList;

namespace EmrCloudApi.Messages;

public class SuperSetMessage
{
    public List<GetSetMstListOutputItem> ReorderSetMstModels { get; set; } = new();
}

public class ReceptionChangedMessage
{
    public ReceptionChangedMessage(List<ReceptionRowModel> receptionInfos, List<SameVisitModel> sameVisitList)
    {
        ReceptionInfos = receptionInfos;
        SameVisitList = sameVisitList;
    }

    public List<ReceptionRowModel> ReceptionInfos { get; set; }

    public List<SameVisitModel> SameVisitList { get; set; }
}

public class PatientInforMessage
{
    public PatientInforMessage(PatientInforModel patientInforModel)
    {
        PatientInforModel = patientInforModel;
    }

    public PatientInforModel PatientInforModel { get; set; }
}

public class ReceptionRevertMessage
{
    public ReceptionRevertMessage(ReceptionModel receptionModel)
    {
        ReceptionModel = receptionModel;
    }

    public ReceptionModel ReceptionModel { get; private set; }
}