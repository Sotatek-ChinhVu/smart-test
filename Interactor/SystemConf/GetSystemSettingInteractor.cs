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

        public GetSystemSettingInteractor(ISystemConfRepository systemConfRepository, ISanteiInfRepository santeiInfRepository, IMstItemRepository mstItemRepository)
        {
            _systemConfRepository = systemConfRepository;
            _santeiInfRepository = santeiInfRepository;
            _mstItemRepository = mstItemRepository;
        }

        public GetSystemSettingOutputData Handle(GetSystemSettingInputData inputData)
        {
            try
            {
                //tab IryoKikanJoho
                var roudouMst = _systemConfRepository.GetRoudouMst();
                var hpInfs = _systemConfRepository.GetListHpInf(inputData.HpId);
                var iryoKihanMenus = _systemConfRepository.GetListSystemConfMenuWithGeneration(inputData.HpId, 2000);

                //tab KihonSetting
                var kihonMenus = _systemConfRepository.GetListSystemConfMenuWithGeneration(inputData.HpId, 2001);
                var systemConfMenusList = _systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1000);

                //SanteiShien
                var systemConfMenusSupport = _systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1001);
                var systemConfMenusCheck = _systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1002);

                //JidoSantei
                var source = _systemConfRepository.GetListSystemConfMenuOnly(inputData.HpId, 3000);
                var autoSanteis = _santeiInfRepository.GetListAutoSanteiMst(inputData.HpId);

                //Uketsuke
                var uketsukeConfMenus = _systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1003);

                //Shinsatsu
                var shinsatsuConfMenus = _systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1004);

                //Kaikei
                var kaikeiConfMenus = _systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1005);

                //Bunsho
                var bunshoConfMenus = _systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1006);

                //KensaKanren
                var kensaKanrenConfMenus = _systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1013);
                var kensaCenterMstList = _mstItemRepository.GetListKensaCenterMst(inputData.HpId);
                var centerCds = _systemConfRepository.GetListCenterCd(inputData.HpId); //check lai cai nay

                //SystemOther
                var systemOtherConfMenus = _systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1007);

                //FormUketsuke
                var uketsukeConfForm = _systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1008);

                //FormShinsatsu
                var shinsatsuConfForm = _systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1009);

                // FormKaikei
                var kaikeiConfForm = _systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1010);

                //Reseputo
                var reseputoConfMenus = _systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1011);

                //FormOther
                var otherConfForm = _systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1012);

                //GaibuRenkei
                var gaibuRenkeiConfMenus = _systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1014);

                //KanrishaSettei chua lam

                return new GetSystemSettingOutputData(roudouMst, hpInfs, systemSetting, GetSystemSettingStatus.Successed);
            }
            finally
            {
                _systemConfRepository.ReleaseResource();
                _santeiInfRepository.ReleaseResource();
                _mstItemRepository.ReleaseResource();
            }
        }
    }
}
