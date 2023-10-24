using Domain.Models.PatientInfor;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetVisitTimesManagementModels;

public class GetVisitTimesManagementModelsOutputData : IOutputData
{
    public GetVisitTimesManagementModelsOutputData(List<VisitTimesManagementModel> visitTimesManagementList, GetVisitTimesManagementModelsStatus status)
    {
        VisitTimesManagementList = visitTimesManagementList;
        Status = status;
    }

    public List<VisitTimesManagementModel> VisitTimesManagementList { get; private set; }

    public GetVisitTimesManagementModelsStatus Status { get; private set; }
}
