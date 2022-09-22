using Domain.Models.PatientInfor;
using Domain.Models.PatientInfor.Domain.Models.PatientInfor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.VisitingList.PatientComment;

namespace Interactor.VisitingList
{
    public class GetPatientCommentInteractor : IGetPatientCommentInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;

        public GetPatientCommentInteractor(IPatientInforRepository patientInforRepository)
        {
            _patientInforRepository = patientInforRepository;
        }

        public GetPatientCommentOutputData Handle(GetPatientCommentInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0 || inputData.PdId <= 0)
                {
                    return new GetPatientCommentOutputData(GetPatientCommentStatus.InvalidData);
                }

                var listData = _patientInforRepository.PatientCommentModels(inputData.HpId, inputData.PdId);

                return new GetPatientCommentOutputData(listData, GetPatientCommentStatus.Success);
            }
            catch (Exception)
            {
                return new GetPatientCommentOutputData(GetPatientCommentStatus.Failed);
            }
        }
    }
}
