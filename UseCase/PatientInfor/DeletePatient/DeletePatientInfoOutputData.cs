using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.DeletePatient;

public class DeletePatientInfoOutputData : IOutputData
{
    public DeletePatientInfoStatus Status { get; private set; }

    public List<ReceptionRowModel> ReceptionInfos { get; private set; }

    public List<SameVisitModel> SameVisitList { get; private set; }

    public DeletePatientInfoOutputData(DeletePatientInfoStatus status)
    {
        Status = status;
        ReceptionInfos = new();
        SameVisitList = new();
    }

    public DeletePatientInfoOutputData(DeletePatientInfoStatus status, List<ReceptionRowModel> receptionInfos, List<SameVisitModel> sameVisitList)
    {
        Status = status;
        ReceptionInfos = receptionInfos;
        SameVisitList = sameVisitList;
    }
}
