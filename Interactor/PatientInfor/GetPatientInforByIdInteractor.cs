using Domain.Models.PatientInfor;
using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.PatientInformation.GetById;
using UseCase.PatientInformation.GetList;

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
            var data = _patientInforRepository.GetById(inputData.PtId);
            return new GetPatientInforByIdOutputData(data);
        }
    }
}
