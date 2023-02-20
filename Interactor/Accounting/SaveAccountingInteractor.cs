using Domain.Models.Accounting;
using Domain.Models.SystemConf;
using UseCase.Accounting.SaveAccounting;

namespace Interactor.Accounting
{
    public class SaveAccountingInteractor : ISaveAccountingInputPort
    {
        private readonly IAccountingRepository _accountingRepository;
        private readonly ISystemConfRepository _systemConfRepository;

        public SaveAccountingInteractor(IAccountingRepository accountingRepository, ISystemConfRepository systemConfRepository)
        {
            _accountingRepository = accountingRepository;
            _systemConfRepository = systemConfRepository;
        }

        public SaveAccountingOutputData Handle(SaveAccountingInputData inputData)
        {
            try
            {
                var listRaiinInf = _accountingRepository.GetListRaiinInf(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo);

                var listRaiinNo = listRaiinInf.Select(r => r.RaiinNo).ToList();

                var listSyunoSeikyu = _accountingRepository.GetListSyunoSeikyu(inputData.HpId, inputData.PtId, inputData.SinDate, listRaiinNo);

                var syunoSeikyu = listSyunoSeikyu.FirstOrDefault(x => x.RaiinNo == inputData.RaiinNo);

                if (syunoSeikyu == null)
                {
                    return new SaveAccountingOutputData(SaveAccountingStatus.Failed);
                }
                else if (syunoSeikyu.NyukinKbn == 0)
                {
                    listSyunoSeikyu = listSyunoSeikyu.Where(item => item.NyukinKbn == 0).ToList();
                }
                else
                {
                    listSyunoSeikyu = listSyunoSeikyu.Where(item => item.NyukinKbn != 0).ToList();
                }

                var listAllSyunoSeikyu = _accountingRepository.GetListSyunoSeikyu(inputData.HpId, inputData.PtId, inputData.SinDate, listRaiinNo, true);

                var debitBalance = listAllSyunoSeikyu.Sum(item => item.SeikyuGaku -
                                                  item.SyunoNyukinModels.Sum(itemNyukin =>
                                                      itemNyukin.NyukinGaku + itemNyukin.AdjustFutan));
                var accDue = (int)_systemConfRepository.GetSettingValue(3020, 0, 0); 

                if(accDue == 0)
                {
                    accDue = debitBalance;
                }

                var save = _accountingRepository.SaveAccounting(listAllSyunoSeikyu, listSyunoSeikyu, inputData.HpId, inputData.PtId, inputData.UserId, accDue, inputData.ThisWari, inputData.ThisCredit,
                                                                inputData.PayType, inputData.Comment);
                if (save)
                {
                    return new SaveAccountingOutputData(SaveAccountingStatus.Success);
                }
                return new SaveAccountingOutputData(SaveAccountingStatus.Failed);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }

    }
}
