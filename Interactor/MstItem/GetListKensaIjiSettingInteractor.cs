using Domain.Models.MstItem;
using UseCase.MstItem.GetListKensaIjiSetting;

namespace Interactor.MstItem
{
    public class GetListKensaIjiSettingInteractor : IGetListKensaIjiSettingInputPort
    {
        private readonly IMstItemRepository _mstItemRepository;
        public GetListKensaIjiSettingInteractor(IMstItemRepository mstItemRepository)
        {
            _mstItemRepository = mstItemRepository;
        }

        public GetListKensaIjiSettingOutputData Handle(GetListKensaIjiSettingInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new GetListKensaIjiSettingOutputData(new List<KensaIjiSettingModel>(), GetListKensaIjiSettingStatus.InvalidHpId);
                var data = _mstItemRepository.GetListKensaIjiSettingModel(inputData.HpId, inputData.KeyWords, inputData.IsValid, inputData.IsExpired, default);
                if (!data.Any())
                {
                    return new GetListKensaIjiSettingOutputData(new List<KensaIjiSettingModel>(), GetListKensaIjiSettingStatus.NoData);
                }
                return new GetListKensaIjiSettingOutputData(data, GetListKensaIjiSettingStatus.Successful);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
