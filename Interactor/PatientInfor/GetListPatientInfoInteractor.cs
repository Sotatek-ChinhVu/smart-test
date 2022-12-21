using Domain.Models.PatientInfor;
using Domain.Models.SpecialNote.PatientInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.PatientInfor.GetListPatient;

namespace Interactor.PatientInfor;

public class GetListPatientInfoInteractor : IGetPatientInfoInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;
    public GetListPatientInfoInteractor(IPatientInforRepository patientInfoRepository)
    {
        _patientInforRepository = patientInfoRepository;
    }
    public GetPatientInfoOutputData Handle(GetPatientInfoInputData input)
    {
        try
        {
            if(input.PtId <= 0)
            {
                return new GetPatientInfoOutputData(GetPatientInfoStatus.InvalidPtId);
            }

            var patientInf = _patientInforRepository.SearchPatient(input.HpId, input.PtId);
            return new GetPatientInfoOutputData(GetPatientInfoStatus.Success, patientInf);
        }
        catch
        {
            return new GetPatientInfoOutputData(GetPatientInfoStatus.Failed);
        }
    }
}
