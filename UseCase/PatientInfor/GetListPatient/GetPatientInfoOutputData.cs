using Domain.Models.PatientInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Core.Sync.Core;

namespace UseCase.PatientInfor.GetListPatient;

public class GetPatientInfoOutputData : IOutputData
{
    public GetPatientInfoOutputData(GetPatientInfoStatus status, List<GetListPatientInfoInputItem> patientInfoLists)
    {
        Status = status;
        PatientInfoLists = patientInfoLists;
    }

    public GetPatientInfoStatus Status { get; private set; }
    public List<GetListPatientInfoInputItem> PatientInfoLists { get; private set; } = new List<GetListPatientInfoInputItem>();
}
