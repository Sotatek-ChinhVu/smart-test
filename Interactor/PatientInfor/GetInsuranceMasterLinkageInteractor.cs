using Domain.Models.PatientInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Insurance.GetList;
using UseCase.PatientInfor.GetInsuranceMasterLinkage;

namespace Interactor.PatientInfor
{
    public class GetInsuranceMasterLinkageInteractor: IGetInsuranceMasterLinkageInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;

        public GetInsuranceMasterLinkageInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public GetInsuranceMasterLinkageOutputData Handle(GetInsuranceMasterLinkageInputData inputData)
        {
            try
            {
                if(inputData.HpId <= 0)
                    return new GetInsuranceMasterLinkageOutputData(new List<DefHokenNoModel>(), GetInsuranceMasterLinkageStatus.InvalidHpId);

                if(string.IsNullOrWhiteSpace(inputData.FutansyaNo))
                    return new GetInsuranceMasterLinkageOutputData(new List<DefHokenNoModel>(), GetInsuranceMasterLinkageStatus.In);
            }
            catch (Exception)
            {
                return new GetInsuranceMasterLinkageOutputData(new List<DefHokenNoModel>(), GetInsuranceMasterLinkageStatus.Failed);
            }
        }
    }
}
