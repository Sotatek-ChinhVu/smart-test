using Domain.Models.PatientInfor;
using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.PatientInformation.GetById;

namespace Interactor.PatientInfor
{
    public class GetPatientInforByIdInteractor : IGetPatientInforByIdInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        public GetPatientInforByIdInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public GetPatientInforByIdOutputData Handle(GetPatientInforByIdInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new GetPatientInforByIdOutputData(null, GetPatientInforByIdStatus.InvalidPtId);
            }

            if (inputData.PtId < 0)
            {
                return new GetPatientInforByIdOutputData(null, GetPatientInforByIdStatus.InvalidPtId);
            }

            var data = _patientInforRepository.GetById(inputData.HpId, inputData.PtId);
            if (data == null)
                return new GetPatientInforByIdOutputData(null, GetPatientInforByIdStatus.DataNotExist);
            return new GetPatientInforByIdOutputData(data, GetPatientInforByIdStatus.Successed);
        }
    }
}