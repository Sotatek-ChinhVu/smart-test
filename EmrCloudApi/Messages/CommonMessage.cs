using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using System.Text.Json.Serialization;
using UseCase.SetMst.GetList;
using UseCase.Todo.GetTodoInfFinder;

namespace EmrCloudApi.Messages;

public class SuperSetMessage
{
    [JsonPropertyName("reorderSetMstModels")]
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

public class TodoInfMessage
{
    public TodoInfMessage(List<GetListTodoInfFinderOutputItem> todoInfOutputItem)
    {
        TodoInfOutputItem = todoInfOutputItem;
    }

    public List<GetListTodoInfFinderOutputItem> TodoInfOutputItem { get; set; }
}