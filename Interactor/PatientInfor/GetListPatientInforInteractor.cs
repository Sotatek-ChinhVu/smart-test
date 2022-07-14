using Domain.Models.PatientInfor;
using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.PatientInformation.GetList;

namespace Interactor.PatientInfor
{
    public class GetListPatientInforInteractor : IGetAllPateintInforInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        public GetListPatientInforInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public GetAllOutputData Handle(GetAllInputData inputData)
        {
            return new GetAllOutputData(_patientInforRepository.GetAll().ToList());
        }
    }
}
