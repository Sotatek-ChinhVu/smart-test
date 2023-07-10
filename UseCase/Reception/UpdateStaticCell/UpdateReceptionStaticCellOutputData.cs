using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using UseCase.Core.Sync.Core;

namespace UseCase.Reception.UpdateStaticCell;

public class UpdateReceptionStaticCellOutputData : IOutputData
{
    public UpdateReceptionStaticCellOutputData(UpdateReceptionStaticCellStatus status)
    {
        Status = status;
        ReceptionInfos = new();
        SameVisitList = new();
        PatientInforModel = new();
    }

    public UpdateReceptionStaticCellOutputData(UpdateReceptionStaticCellStatus status, List<ReceptionRowModel> receptionInfos, List<SameVisitModel> sameVisitList, PatientInforModel patientInforModel)
    {
        Status = status;
        ReceptionInfos = receptionInfos;
        SameVisitList = sameVisitList;
        PatientInforModel = patientInforModel;
    }

    public UpdateReceptionStaticCellStatus Status { get; private set; }

    public List<ReceptionRowModel> ReceptionInfos { get; private set; }

    public List<SameVisitModel> SameVisitList { get; private set; }

    public PatientInforModel PatientInforModel { get; private set; }
}
