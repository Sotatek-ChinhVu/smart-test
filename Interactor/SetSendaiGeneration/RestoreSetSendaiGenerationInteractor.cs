using Domain.Models.SetGenerationMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.SetSendaiGeneration.Restore;
using UseCase.SetSendaiGeneration.Delete;

namespace Interactor.SetSendaiGeneration
{
    public class RestoreSetSendaiGenerationInteractor : IRestoreSetSendaiGenerationInputPort
    {
        private readonly ISetGenerationMstRepository _inputItemRepository;

        public RestoreSetSendaiGenerationInteractor(ISetGenerationMstRepository inputItemRepository)
        {
            _inputItemRepository = inputItemRepository;
        }

        public RestoreSetSendaiGenerationOutputData Handle(RestoreSetSendaiGenerationInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new RestoreSetSendaiGenerationOutputData(false, RestoreSetSendaiGenerationStatus.InvalidHpId);
                }

                if (inputData.UserId <= 0)
                {
                    return new RestoreSetSendaiGenerationOutputData(false, RestoreSetSendaiGenerationStatus.InvalidUserId);
                }

                if (inputData.RestoreGenerationId <= 0)
                {
                    return new RestoreSetSendaiGenerationOutputData(false, RestoreSetSendaiGenerationStatus.InvalidRestoreGenerationId);
                }

                var result = _inputItemRepository.RestoreSetSendaiGeneration(inputData.RestoreGenerationId, inputData.HpId, inputData.UserId);
                if (result)
                {
                    return new RestoreSetSendaiGenerationOutputData(result, RestoreSetSendaiGenerationStatus.Success);
                }
                return new RestoreSetSendaiGenerationOutputData(false, RestoreSetSendaiGenerationStatus.Faild);
            }
            finally
            {
                _inputItemRepository.ReleaseResource();
            }
        }
    }
}