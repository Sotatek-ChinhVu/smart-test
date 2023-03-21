using Domain.Models.HistoryOrder;
using Helper.Constants;
using Interactor.MedicalExamination.HistoryCommon;
using UseCase.MedicalExamination.GetDataPrintKarte2;
using UseCase.MedicalExamination.GetHistory;

namespace Interactor.MedicalExamination;

public class GetDataPrintKarte2Interactor : IGetDataPrintKarte2InputPort
{
    private readonly IHistoryCommon _historyCommon;
    private readonly IHistoryOrderRepository _historyOrderRepository;

    public GetDataPrintKarte2Interactor(IHistoryCommon historyCommon, IHistoryOrderRepository historyOrderRepository)
    {
        _historyCommon = historyCommon;
        _historyOrderRepository = historyOrderRepository;
    }

    public GetMedicalExaminationHistoryOutputData Handle(GetDataPrintKarte2InputData inputData)
    {
        try
        {
            (int, List<HistoryOrderModel>) historyList = _historyOrderRepository.GetList(inputData.HpId,
                                                                                         inputData.PtId,
                                                                                         inputData.SinDate,
                                                                                         inputData.StartDate,
                                                                                         inputData.EndDate,
                                                                                         1);
            var result = _historyCommon.GetHistoryOutput(inputData.HpId, inputData.PtId, inputData.SinDate, historyList);
            List<HistoryKarteOdrRaiinItem> historyKarteOdrRaiinList = result.RaiinfList;
            FilterData(ref historyKarteOdrRaiinList, inputData);
            return result;
        }
        finally
        {
            _historyCommon.ReleaseResources();
        }
    }

    private void FilterData(ref List<HistoryKarteOdrRaiinItem> historyKarteOdrRaiinItems, GetDataPrintKarte2InputData inputData)
    {
        List<OrderHokenType> GetListAcceptedHokenType()
        {
            List<OrderHokenType> result = new();
            if (inputData.IsCheckedHoken)
            {
                result.Add(OrderHokenType.Hoken);
            }
            if (inputData.IsCheckedJihi)
            {
                result.Add(OrderHokenType.Jihi);
            }
            if (inputData.IsCheckedHokenJihi)
            {
                result.Add(OrderHokenType.HokenJihi);
            }
            if (inputData.IsCheckedJihiRece)
            {
                result.Add(OrderHokenType.JihiRece);
            }
            if (inputData.IsCheckedHokenRousai)
            {
                result.Add(OrderHokenType.Rousai);
            }
            if (inputData.IsCheckedHokenJibai)
            {
                result.Add(OrderHokenType.Jibai);
            }
            return result;
        }

        if (!inputData.IsIncludeTempSave)
        {
            historyKarteOdrRaiinItems = historyKarteOdrRaiinItems.Where(k => k.Status != 3).ToList();
        }

        List<OrderHokenType> listAcceptedHokenType = GetListAcceptedHokenType();

        //Filter raiin as hoken setting
        List<HistoryKarteOdrRaiinItem> filteredKaruteList = new();
        foreach (var history in historyKarteOdrRaiinItems)
        {
            if (history.HokenGroups == null || !history.HokenGroups.Any())
            {
                continue;
            }

            if (listAcceptedHokenType.Contains((OrderHokenType)history.HokenType))
            {
                filteredKaruteList.Add(history);
                continue;
            }

            if (inputData.DeletedOdrVisibilitySetting == 0)
            {
                foreach (var hokenGroup in history.HokenGroups)
                {
                    bool isDataExisted = false;
                    foreach (var group in hokenGroup.GroupOdrItems)
                    {
                        isDataExisted = group.OdrInfs.Any(o => o.IsDeleted == 0);
                        if (isDataExisted)
                        {
                            break;
                        }
                    }

                    if (isDataExisted && listAcceptedHokenType.Contains((OrderHokenType)history.HokenType))
                    {
                        filteredKaruteList.Add(history);
                        break;
                    }
                }
            }
            else if (inputData.DeletedOdrVisibilitySetting == 2)
            {
                foreach (var hokenGroup in history.HokenGroups)
                {
                    bool isDataExisted = false;
                    foreach (var group in hokenGroup.GroupOdrItems)
                    {
                        isDataExisted = group.OdrInfs.Any(o => o.IsDeleted != 2);
                        if (isDataExisted)
                        {
                            break;
                        }
                    }

                    if (isDataExisted && listAcceptedHokenType.Contains((OrderHokenType)history.HokenType))
                    {
                        filteredKaruteList.Add(history);
                        break;
                    }
                }
            }
        }

        historyKarteOdrRaiinItems = filteredKaruteList;

        //Filter karte and order empty
        historyKarteOdrRaiinItems = historyKarteOdrRaiinItems.Where(k => k.HokenGroups != null && k.HokenGroups.Any() && k.KarteHistories != null && k.KarteHistories.Any()).ToList();
    }
}
