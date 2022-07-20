using Domain.Models.PatientCommentInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.PatientInformation.GetById;

namespace Interactor.PatientCommentInfo
{
    public class GetPatientCommentInfoInteractor : IGetPatientCommentInfoByPtIdInputPort
    {
        private readonly IPatientCommentInfoRepository _patientCommentInfoRepository;
        public GetPatientCommentInfoInteractor(IPatientCommentInfoRepository patientCommentInfoRepository)
        {
            _patientCommentInfoRepository = patientCommentInfoRepository;
        }
   
        public GetPatientCommentInfoByPtIdOutputData Handle(GetPatientCommentInfoByPtIdInputData inputData)
        {
            var data = _patientCommentInfoRepository.GetPatientCmtInf(inputData.PtId);
            return new GetPatientCommentInfoByPtIdOutputData(data);
        }
    }
}
