using Domain.Models.PatientComment;
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
        private readonly IPatientCommentRepository _patientCommentRepository;

        public GetPatientCommentInteractor(IPatientCommentRepository patientCommentRepository)
        {
            _patientCommentRepository = patientCommentRepository;
        }

        public GetPatientCommentOutputData Handle(GetPatientCommentInputData inputData)
        {
            if (inputData.HpId <= 0 || inputData.PdId <= 0)
            {
                return new GetPatientCommentOutputData(new List<PatientCommentModel>(), GetPatientCommentStatus.InvalidData);
            }

            var listData = _patientCommentRepository.patientCommentModels(inputData.HpId, inputData.PdId);

            if (listData == null || listData.Count == 0)
            {
                return new GetPatientCommentOutputData(new(), GetPatientCommentStatus.NoData);
            }

            return new GetPatientCommentOutputData(listData, GetPatientCommentStatus.Success);
        }
    }
}
