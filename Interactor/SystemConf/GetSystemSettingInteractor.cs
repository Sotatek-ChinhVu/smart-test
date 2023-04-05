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
                var systemSetting = new List<SystemConfMenuModel>();

                //tab IryoKikanJoho
                var roudouMst = _systemConfRepository.GetRoudouMst();
                var hpInfs = _systemConfRepository.GetListHpInf(inputData.HpId);
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenuWithGeneration(inputData.HpId, 2000));

                //tab KihonSetting
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenuWithGeneration(inputData.HpId, 2001));
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1000));

                //SanteiShien
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1001));
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1002));

                //JidoSantei
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenuOnly(inputData.HpId, 3000));
                var autoSanteis = _santeiInfRepository.GetListAutoSanteiMst(inputData.HpId);

                //Uketsuke
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1003));

                //Shinsatsu
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1004));

                //Kaikei
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1005));

                //Bunsho
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1006));

                //KensaKanren
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1013));
                var kensaCenterMstList = _mstItemRepository.GetListKensaCenterMst(inputData.HpId);
                var centerCds = _systemConfRepository.GetListCenterCd(inputData.HpId); //check lai cai nay

                //SystemOther
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1007));

                //FormUketsuke
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1008));

                //FormShinsatsu
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1009));

                // FormKaikei
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1010));

                //Reseputo
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1011));

                //FormOther
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1012));

                //GaibuRenkei
                systemSetting.AddRange(_systemConfRepository.GetListSystemConfMenu(inputData.HpId, 1014));

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
