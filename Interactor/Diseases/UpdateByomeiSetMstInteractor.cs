using Domain.Models.Diseases;
using Domain.Models.ListSetMst;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.ByomeiSetMst.UpdateByomeiSetMst;
using UseCase.ListSetMst.UpdateListSetMst;

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
