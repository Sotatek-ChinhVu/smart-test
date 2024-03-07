using Domain.Models.HpInf;
using Domain.Models.MstItem;
using Domain.Models.Santei;
using Domain.Models.SystemConf;
using UseCase.SystemConf.SystemSetting;

namespace Interactor.SystemConf
{
    public class GetSystemSettingInteractor : IGetSystemSettingInputPort
    {
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly ISanteiInfRepository _santeiInfRepository;
        private readonly IMstItemRepository _mstItemRepository;
        private readonly IHpInfRepository _hpInfRepository;

        public GetSystemSettingInteractor(ISystemConfRepository systemConfRepository, ISanteiInfRepository santeiInfRepository, IMstItemRepository mstItemRepository, IHpInfRepository hpInfRepository)
        {
            _systemConfRepository = systemConfRepository;
            _santeiInfRepository = santeiInfRepository;
            _mstItemRepository = mstItemRepository;
            _hpInfRepository = hpInfRepository;
        }

        public GetSystemSettingOutputData Handle(GetSystemSettingInputData inputData)
        {
            try
            {
                var systemSetting = new List<SystemConfMenuModel>();

                //tab IryoKikanJoho
                var roudouMst = _systemConfRepository.GetRoudouMst(inputData.HpId);
                var hpInfs = _hpInfRepository.GetListHpInf(inputData.HpId);
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenuWithGeneration(inputData.HpId, new List<int> { 2000, 2001 }));
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenu(inputData.HpId, new List<int> { 1000, 1001, 1002, 1003, 1004, 1005, 1006, 1007, 1008, 1009, 1010, 1011, 1012, 1013, 1014 }));
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenuOnly(inputData.HpId, 3000));
                var autoSanteis = _santeiInfRepository.GetListAutoSanteiMst(inputData.HpId);
                var kensaCenterMstList = _mstItemRepository.GetListKensaCenterMst(inputData.HpId);
                var centerCds = _systemConfRepository.GetListCenterCd(inputData.HpId);

                return new GetSystemSettingOutputData(roudouMst, hpInfs, autoSanteis, kensaCenterMstList, centerCds, systemSetting, GetSystemSettingStatus.Successed);
            }
            finally
            {
                _systemConfRepository.ReleaseResource();
                _santeiInfRepository.ReleaseResource();
                _mstItemRepository.ReleaseResource();
                _hpInfRepository.ReleaseResource();
            }
        }
    }
}
