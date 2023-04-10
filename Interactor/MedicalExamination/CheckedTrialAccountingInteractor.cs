using CommonCheckers.OrderRealtimeChecker.DB;
using Domain.Models.MstItem;
using Domain.Models.User;
using Domain.Models.UserConf;
using Helper.Constants;
using UseCase.MedicalExamination.CheckedTrialAccounting;
using UseCase.MedicalExamination.GetCheckedOrder;

namespace Interactor.MedicalExamination
{
    public class CheckedTrialAccountingInteractor : ICheckedTrialAccountingInputPort
    {
        private readonly IUserRepository _userRepository;
        private readonly IMstItemRepository _mstItemRepository;
        private readonly IUserConfRepository _userConfRepository;

        public CheckedTrialAccountingInteractor(IUserRepository userRepository, IMstItemRepository mstItemRepository, IUserConfRepository userConfRepository)
        {
            _userRepository = userRepository;
            _mstItemRepository = mstItemRepository;
            _userConfRepository = userConfRepository;
        }

        public CheckedTrialAccountingOutputData Handle(CheckedTrialAccountingInputData inputData)
        {
            var tanToDict = GetTantoDict(inputData.HpId, inputData.SinDate);
            var kaDict = _mstItemRepository.GetKaDict(inputData.HpId);
            var isValidHeader = IsValidHeader(tanToDict, inputData.TantoId, kaDict, inputData.KaId);
            if (!string.IsNullOrEmpty(isValidHeader)) return new CheckedTrialAccountingOutputData(isValidHeader, CheckedTrialAccountingStatus.Failed);
            var commentCheckSaveParam = _userConfRepository.GetSettingParam(inputData.HpId, inputData.UserId, 921, 0);
            var InputCheckSaveParam = _userConfRepository.GetSettingParam(inputData.HpId, inputData.UserId, 921, 2);
            var SanteiCheckSaveParam = _userConfRepository.GetSettingParam(inputData.HpId, inputData.UserId, 921, 1);
            var userConfig = new UserConfModel(commentCheckSaveParam, InputCheckSaveParam, SanteiCheckSaveParam);
        }

        private string IsValidHeader(Dictionary<int, string> tanToDict, int tantoId, Dictionary<int, string> kaDict, int kaId)
        {
            if (!tanToDict.ContainsKey(tantoId) || tantoId <= 0)
            {
                return "担当医";
            }

            if (!kaDict.ContainsKey(kaId))
            {
                return "診療科";
            }
            return string.Empty;
        }

        private Dictionary<int, string> GetTantoDict(int hpId, int sinday)
        {
            Dictionary<int, string> tantoDict = new Dictionary<int, string>
            {
                { 0, "" }
            };
            var userMsts = _userRepository.GetListDoctorBySinDate(hpId, sinday);
            if (userMsts != null)
            {
                foreach (var userMst in userMsts)
                {
                    tantoDict.Add(userMst.UserId, userMst.Sname);
                }
            }
            return tantoDict;
        }

        public bool CheckHokenPatternSelect(List<OdrInfItem> OdrInfItems)
        {
            string functionName = nameof(CheckHokenPatternSelect);
            try
            {
                if (SelectedHokenPattern == null || SelectedHokenPattern.PtId <= 0 || SelectedHokenPattern.HokenPid <= 0)
                {
                    return false;
                }

                var listOdrInfHokenPid = OdrInfItems.Where(u => u.IsDeleted == 0).Select(u => u.HokenPid).ToList();
                listOdrInfHokenPid.Add(SelectedHokenPattern.HokenPid);
                using (var dbService = new DBContextFactory())
                {
                    IMasterFinder masterFinder = new MasterFinder(dbService);
                    var listHokenPattern = masterFinder.FindPtHokenPatternList(PtId, SinDay);
                    var listHokenPids = listHokenPattern.Select(u => u.HokenPid).ToList();
                    var hokenPidInValid = listOdrInfHokenPid.FirstOrDefault(i => !listHokenPids.Any(h => h == i));
                    if (listHokenPids.Count == 0 || hokenPidInValid > 0)
                    {
                        var hokenPattern = HokenPatternModels.FirstOrDefault(p => p.HokenPid == hokenPidInValid);
                        string message = $"削除された保険組合せ{hokenPattern?.HokenPatternName ?? string.Empty}が使用されています。" + Environment.NewLine +
                                          "保険組合せを変更してください。";

                        new EmrDialogMessage(EmrMessageType.mFree00030, message, EmrMessageButtons.mbClose, 0).Send();
                        return false;
                    }

                    return true;
                }

            }
            catch (Exception ex)
            {
                Log.WriteLogError(ModuleNameConst.EmrCommonView, this, functionName, ex);
                return false;
            }
        }
    }
}
