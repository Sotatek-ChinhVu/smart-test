using Domain.Models.MstItem;
using Domain.Models.SetGenerationMst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.MstItem.GetDiseaseList;
using UseCase.SetSendaiGeneration.GetList;

namespace Interactor.SetSendaiGeneration
{
    public class SetSendaiGenerationGetListInteractor : ISetSendaiGenerationInputPort
    {
        private readonly ISetGenerationMstRepository _inputItemRepository;

        public SetSendaiGenerationGetListInteractor(ISetGenerationMstRepository inputItemRepository)
        {
            _inputItemRepository = inputItemRepository;
        }

        public SetSendaiGenerationOutputData Handle(SetSendaiGenerationInputData inputData)
        {
            try
            {
                if(inputData.HpId == 0)
                {
                    return new SetSendaiGenerationOutputData(new List<SetSendaiGenerationModel>(), SetSendaiGenerationStatus.InvalidHpId);
                }
                var result = _inputItemRepository.GetListSendaiGeneration(inputData.HpId);
                return new SetSendaiGenerationOutputData(result, SetSendaiGenerationStatus.Success);
            }
            finally
            {
                _inputItemRepository.ReleaseResource();
            }
        }
    }
}
