using Domain.Models.MstItem;
using UseCase.MstItem.UpdateAdoptedByomei;

namespace Interactor.MstItem
{
    public class UpdateAdoptedByomeiInteractor : IUpdateAdoptedByomeiInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public UpdateAdoptedByomeiInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }
        public UpdateAdoptedByomeiOutputData Handle(UpdateAdoptedByomeiInputData inputData)
        {
            if (inputData.HpId < 0)
            {
                return new UpdateAdoptedByomeiOutputData(false, UpdateAdoptedByomeiStatus.InvalidHospitalId);
            }

            if (String.IsNullOrEmpty(inputData.ByomeiCd))
            {
                return new UpdateAdoptedByomeiOutputData(false, UpdateAdoptedByomeiStatus.InvalidByomeiCd);
            }
            try
            {
                var data = _mstItemRepository.UpdateAdoptedByomei(inputData.HpId, inputData.ByomeiCd, inputData.UserId);

                return new UpdateAdoptedByomeiOutputData(data, UpdateAdoptedByomeiStatus.Successed);
            }
            catch (Exception)
            {
                return new UpdateAdoptedByomeiOutputData(false, UpdateAdoptedByomeiStatus.Failed);
            }
        }
    }
}
