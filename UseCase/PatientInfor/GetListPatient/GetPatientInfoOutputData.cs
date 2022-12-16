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
    public GetPatientInfoOutputData(GetPatientInfoStatus status)
    {
        Status = status;
    }
    public GetPatientInfoOutputData(GetPatientInfoStatus staus, List<PatientInforModel> patientInfoList)
    {
        Status = staus;
        PatientInfoList = patientInfoList;
    }
    public GetPatientInfoStatus Status { get; private set; }
    public List<PatientInforModel> PatientInfoList { get; private set; } = new List<PatientInforModel>();
}
