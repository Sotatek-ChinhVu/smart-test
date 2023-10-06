using Domain.Models.MstItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.MstItem.UpdateAdoptedByomei;
using UseCase.MstItem.UpdateByomeiMst;

namespace Interactor.MstItem
{
    public class UpdateByomeiMstInteractor : IUpdateByomeiMstInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public UpdateByomeiMstInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public UpdateByomeiMstOutputData Handle(UpdateByomeiMstInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new UpdateByomeiMstOutputData(false, UpdateByomeiMstStatus.InValidHpId);
            }

            if (inputData.UserId < 0)
            {
                return new UpdateByomeiMstOutputData(false, UpdateByomeiMstStatus.InValidUserId);
            }

            if (inputData.ListData.Count <= 0)
            {
                return new UpdateByomeiMstOutputData(false, UpdateByomeiMstStatus.InvalidDataUpdate);
            }
            try
            {
                var data = _mstItemRepository.UpdateByomeiMst(inputData.HpId, inputData.UserId, inputData.ListData);
                return new UpdateByomeiMstOutputData(data, UpdateByomeiMstStatus.Successed);
            }
            catch (Exception)
            {
                return new UpdateByomeiMstOutputData(false, UpdateByomeiMstStatus.Failed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
