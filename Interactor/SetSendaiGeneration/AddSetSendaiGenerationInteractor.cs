using Domain.Models.SetGenerationMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.SetSendaiGeneration.Add;
using UseCase.SetSendaiGeneration.Delete;

namespace Interactor.SetSendaiGeneration
{
    public class AddSetSendaiGenerationInteractor : IAddSetSendaiGenerationInputPort
    {
        private readonly ISetGenerationMstRepository _inputItemRepository;

        public AddSetSendaiGenerationInteractor(ISetGenerationMstRepository inputItemRepository)
        {
            _inputItemRepository = inputItemRepository;
        }

        public AddSetSendaiGenerationOutputData Handle(AddSetSendaiGenerationInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new AddSetSendaiGenerationOutputData(false, AddSetSendaiGenerationStatus.InvalidHpId);
                }

                if (inputData.StartDate <= 0)
                {
                    return new AddSetSendaiGenerationOutputData(false, AddSetSendaiGenerationStatus.InvalidStartDate);
                }

                if (inputData.UserId <= 0)
                {
                    return new AddSetSendaiGenerationOutputData(false, AddSetSendaiGenerationStatus.InvalidUserId);
                }

                var result = _inputItemRepository.AddSetSendaiGeneration(inputData.UserId, inputData.HpId, inputData.StartDate);
                if (result)
                {
                    return new AddSetSendaiGenerationOutputData(result, AddSetSendaiGenerationStatus.Success);
                }
                return new AddSetSendaiGenerationOutputData(false, AddSetSendaiGenerationStatus.Faild);
            }
            finally
            {
                _inputItemRepository.ReleaseResource();
            }
        }
    }
}