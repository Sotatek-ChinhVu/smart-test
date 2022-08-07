using Domain.Models.PatientGroupMst;
using UseCase.PatientGroupMst.GetList;

namespace Interactor.PatientGroupMst
{
    public class GetListPatientGroupMstInteractor : IGetListPatientGroupMstInputPort
    {
        private readonly IPatientGroupMstRepository _patientGroupMstRepository;
        public GetListPatientGroupMstInteractor(IPatientGroupMstRepository patientGroupMstRepository)
        {
            _patientGroupMstRepository = patientGroupMstRepository;
        }

        public GetListPatientGroupMstOutputData Handle(GetListPatientGroupMstInputData inputData)
        {
            return new GetListPatientGroupMstOutputData(_patientGroupMstRepository.GetAll());
        }
    }
}
