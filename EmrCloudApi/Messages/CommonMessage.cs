using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Helper.Constants;
using UseCase.SetMst.GetList;

namespace EmrCloudApi.Messages;

public class CommonMessage
{
    public int SinDate { get; set; }
    public long RaiinNo { get; set; } = CommonConstants.InvalidId;
    public long PtId { get; set; } = CommonConstants.InvalidId;
}

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