using Domain.Models.NextOrder;
using Domain.Models.PatientInfor;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Repositories;
using System.Linq;
using System.Text;
using UseCase.NextOrder.Get;
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
            if (input.HpId <= 0)
            {
                return new GetPatientInfoOutputData(GetPatientInfoStatus.InvalidHpId, new());
            }
            if(input.PtId <= 0)
            {
                return new GetPatientInfoOutputData(GetPatientInfoStatus.InvalidPtId, new());
            }
            var listPatientInfs = GetListPatientInfos(input.HpId, input.PtId).Select(item => new GetListPatientInfoInputItem(
                                                                                     item.HpId, 
                                                                                     item.PtId, 
                                                                                     item.PtNum, 
                                                                                     item.KanaName, 
                                                                                     item.Name, 
                                                                                     item.Birthday, 
                                                                                     item.LastVisitDate)).ToList();

            return new GetPatientInfoOutputData(GetPatientInfoStatus.Success, listPatientInfs);
        }
        finally
        {
            _patientInforRepository.ReleaseResource();
        }
    }

    private List<PatientInforModel> GetListPatientInfos(int hpId, long ptId)
    {
        List<PatientInforModel> result = new(_patientInforRepository.SearchPatient(hpId, ptId).Select(x => new PatientInforModel(
                                                    x.HpId,
                                                    x.PtId,
                                                    x.PtNum,
                                                    x.KanaName,
                                                    x.Name,
                                                    x.Birthday,
                                                    x.LastVisitDate)));
        return result;
    }
}