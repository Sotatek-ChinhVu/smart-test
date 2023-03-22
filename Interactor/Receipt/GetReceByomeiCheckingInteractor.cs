using Domain.Models.Diseases;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.Receipt;
using Helper.Constants;
using Helper.Extension;
using UseCase.Receipt.GetReceByomeiChecking;

namespace Interactor.Receipt;

public class GetReceByomeiCheckingInteractor : IGetReceByomeiCheckingInputPort
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IOrdInfRepository _ordInfRepository;
    private readonly IPtDiseaseRepository _ptDiseaseRepository;
    private readonly IMstItemRepository _mstItemRepository;
    private const string _suspectFlag = "の疑い";

    public GetReceByomeiCheckingInteractor(IReceiptRepository receiptRepository, IOrdInfRepository ordInfRepository, IPtDiseaseRepository ptDiseaseRepository, IMstItemRepository mstItemRepository)
    {
        _receiptRepository = receiptRepository;
        _ordInfRepository = ordInfRepository;
        _ptDiseaseRepository = ptDiseaseRepository;
        _mstItemRepository = mstItemRepository;
    }

    public GetReceByomeiCheckingOutputData Handle(GetReceByomeiCheckingInputData inputData)
    {
        try
        {
            var resultGetData = GetOrderInfDetailModel(inputData);
            var orderInfDetailList = resultGetData.orderInfDetailList;
            var ptDiseaseModelList = resultGetData.ptDiseaseModelList;
            var tenMstItemList = resultGetData.tenMstItemList;
            Dictionary<OrdInfDetailModel, List<PtDiseaseModel>> result = new();
            foreach (var orderInf in orderInfDetailList)
            {
                string santeiItemCd = tenMstItemList.FirstOrDefault(item => item.ItemCd == orderInf.ItemCd)?.SanteiItemCd ?? string.Empty;
                var byomeiItemList = ptDiseaseModelList.Where(item => item.ItemCd == orderInf.ItemCd
                                                                      || item.ItemCd == santeiItemCd)
                                                       .OrderByDescending(item => item.IsAdopted)
                                                       .ThenBy(item => item.Byomei)
                                                       .ToList();
                result.Add(orderInf, byomeiItemList);
            }
            return new GetReceByomeiCheckingOutputData(GetReceByomeiCheckingStatus.Successed, result);
        }
        finally
        {
            _receiptRepository.ReleaseResource();
        }
    }

    private (List<OrdInfDetailModel> orderInfDetailList, List<PtDiseaseModel> ptDiseaseModelList, List<TenItemModel> tenMstItemList) GetOrderInfDetailModel(GetReceByomeiCheckingInputData inputData)
    {
        List<OrdInfDetailModel> orderInfDetailList = new();
        List<PtDiseaseModel> ptDiseaseModelList = new();

        int sinYm = inputData.SinDate / 100;
        var todayOrderList = _ordInfRepository.GetOdrInfsBySinDate(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.HokenId);
        var todayByomeiList = _ptDiseaseRepository.GetByomeiInThisMonth(inputData.HpId, sinYm, inputData.PtId, inputData.HokenId);
        var itemCdList = todayOrderList.Select(item => item.ItemCd).Distinct().ToList();

        var tenMstItemList = _mstItemRepository.FindTenMst(inputData.HpId, itemCdList, inputData.SinDate, inputData.SinDate);

        var santeiItemCdList = tenMstItemList.Select(item => item?.SanteiItemCd ?? string.Empty).Distinct().ToList();
        itemCdList.AddRange(santeiItemCdList);
        var allByomeisByOdr = _ptDiseaseRepository.GetTekiouByomeiByOrder(inputData.HpId, itemCdList);

        foreach (var odrDetail in todayOrderList)
        {
            string itemCd = odrDetail.ItemCd;
            if (string.IsNullOrEmpty(itemCd) ||
                itemCd == ItemCdConst.Con_TouyakuOrSiBunkatu ||
                itemCd == ItemCdConst.Con_Refill)
            {
                continue;
            }

            if (!tenMstItemList.Any(item => item.ItemCd == itemCd))
            {
                continue;
            }

            string santeiItemCd = tenMstItemList.FirstOrDefault(item => item.ItemCd == itemCd)?.SanteiItemCd ?? string.Empty;

            var byomeisByOdr = allByomeisByOdr.Where(item => item.ItemCd == itemCd || item.ItemCd == santeiItemCd).ToList();
            if (byomeisByOdr.Count == 0)
            {
                continue;
            }

            // No.6510 future byomei check
            var byomeiCds = byomeisByOdr.Select(item => item.ByomeiCd).Distinct().ToList();
            if (!orderInfDetailList.Exists(item => item.ItemCd == odrDetail.ItemCd)
                && !todayByomeiList.Where(item => item.IsNodspRece == 0
                                                  && item.HokenPid == 0
                                                  && item.StartDate <= inputData.SinDate
                                                  && (!item.IsTenki || item.TenkiDate >= inputData.SinDate)
                                                  && (!odrDetail.IsDrug || !item.FullByomei.AsString().Contains(_suspectFlag)))
                                   .Any(item => byomeiCds.Contains(item.ByomeiCd)))
            {
                orderInfDetailList.Add(odrDetail);
                ptDiseaseModelList.AddRange(byomeisByOdr);
            }
        }

        return (orderInfDetailList, ptDiseaseModelList, tenMstItemList);
    }
}