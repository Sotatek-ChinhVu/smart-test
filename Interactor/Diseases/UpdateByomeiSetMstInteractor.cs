using Domain.Models.Diseases;
using UseCase.ByomeiSetMst.UpdateByomeiSetMst;

namespace Interactor.Diseases
{
    public class UpdateByomeiSetMstInteractor : IUpdateByomeiSetMstInputPort
    {
        private readonly IPtDiseaseRepository _ptDiseaseRepository;
        public UpdateByomeiSetMstInteractor(IPtDiseaseRepository ptDiseaseRepository)
        {
            _ptDiseaseRepository = ptDiseaseRepository;
        }

        public UpdateByomeiSetMstOutputData Handle(UpdateByomeiSetMstInputData inputData)
        {

            if (inputData.HpId < 0)
            {
                return new UpdateByomeiSetMstOutputData(false, UpdateByomeiSetMstStatus.InValidHpId);
            }

            if (inputData.UserId < 0)
            {
                return new UpdateByomeiSetMstOutputData(false, UpdateByomeiSetMstStatus.InValidUserId);
            }

            if (inputData.ByomeiSetMstUpdates.Count <= 0)
            {
                return new UpdateByomeiSetMstOutputData(false, UpdateByomeiSetMstStatus.InvalidDataUpdate);
            }
            try
            {
                var data = _ptDiseaseRepository.UpdateByomeiSetMst(inputData.UserId, inputData.HpId, inputData.ByomeiSetMstUpdates);
                return new UpdateByomeiSetMstOutputData(data, UpdateByomeiSetMstStatus.Successed);
            }
            finally
            {
                _ptDiseaseRepository.ReleaseResource();
            }
        }
    }
}
