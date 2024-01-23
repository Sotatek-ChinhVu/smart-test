using Domain.Models.MstItem;
using Domain.Models.Santei;
using Helper.Common;
using UseCase.Santei.GetListSanteiInf;

namespace Interactor.Santei;

public class GetListSanteiInfInteractor : IGetListSanteiInfInputPort
{
    private readonly ISanteiInfRepository _santeiInfRepository;
    private readonly IMstItemRepository _mstItemRepository;

    public GetListSanteiInfInteractor(ISanteiInfRepository santeiInfRepository, IMstItemRepository mstItemRepository)
    {
        _santeiInfRepository = santeiInfRepository;
        _mstItemRepository = mstItemRepository;
    }

    public GetListSanteiInfOutputData Handle(GetListSanteiInfInputData input)
    {
        try
        {
            var listByomeis = _mstItemRepository.GetListSanteiByomeis(input.HpId, input.PtId, input.SinDate, input.HokenPid);
            var listSanteiInf = _santeiInfRepository.GetListSanteiInf(input.HpId, input.PtId, input.SinDate);

            var result = ConvertToResult(input.SinDate, listSanteiInf, listByomeis);
            var santeiResultList = result.santeiResultList.OrderBy(item => item.SeqNo).ToList();
            return new GetListSanteiInfOutputData(GetListSanteiInfStatus.Successed, santeiResultList, DicAlertTermCombobox(), DicKisanKbnCombobox(), result.byomeiList);
        }
        finally
        {
            _santeiInfRepository.ReleaseResource();
            _mstItemRepository.ReleaseResource();
        }
    }

    public Dictionary<int, string> DicAlertTermCombobox()
    {
        return new Dictionary<int, string>()
            {
                {2, "日" },
                {3, "暦週" },
                {4, "暦月" },
                {5, "週" },
                {6, "月" },
            };
    }

    public Dictionary<int, string> DicKisanKbnCombobox()
    {
        return new Dictionary<int, string>()
            {
                {0, string.Empty },
                {1, "初回算定" },
                {2, "発症" },
                {3, "急性憎悪" },
                {4, "治療開始" },
                {5, "手術" },
                {6, "初回診断" }
            };
    }

    public (List<SanteiInfOutputItem> santeiResultList, List<string> byomeiList) ConvertToResult(int sinDate, List<SanteiInfModel> listSanteiInfs, List<string> byomeiList)
    {
        List<SanteiInfOutputItem> santeiResultList = new();
        foreach (var santeiInf in listSanteiInfs)
        {
            int kisanDate = 0;
            int dayCount = 0;

            // kisanDate
            var santeiInfDetailList = santeiInf.SanteiInfDetailList.Select(item => ConvertToSanteiInfDetailOutputItem(item))
                                                                   .OrderBy(item => item.EndDate)
                                                                   .ThenBy(item => item.Id)
                                                                   .ToList();

            var kisanSbt = 0;
            foreach (var item in santeiInfDetailList)
            {
                // Add byomei to byomei List
                if (byomeiList.FirstOrDefault(byomei => byomei.Equals(item.Byomei)) == null && !string.IsNullOrEmpty(item.Byomei))
                {
                    byomeiList.Add(item.Byomei);
                }
                if (sinDate > item.EndDate) continue;
                if (item.KisanDate > kisanDate)
                {
                    kisanSbt = item.KisanSbt;
                    kisanDate = item.KisanDate;
                }
            }
            //Type 初回算定 =>  If 前回日 already exists, 起算日 will not be displayed
            if (santeiInf.LastOdrDate > 0 && kisanSbt == 1)
            {
                kisanDate = 0;
            }
            // dayCount
            int targetDateInt = kisanDate != 0 ? kisanDate : santeiInf.LastOdrDate;
            dayCount = CIUtil.GetSanteInfDayCount(sinDate, targetDateInt, santeiInf.AlertTerm);
            santeiResultList.Add(new SanteiInfOutputItem(
                                     santeiInf.Id,
                                     santeiInf.PtId,
                                     santeiInf.ItemCd,
                                     santeiInf.SeqNo,
                                     santeiInf.AlertDays,
                                     santeiInf.AlertTerm,
                                     santeiInf.ItemName,
                                     santeiInf.LastOdrDate,
                                     kisanDate,
                                     dayCount,
                                     santeiInf.SanteiItemCount,
                                     santeiInf.SanteiItemSum,
                                     santeiInf.CurrentMonthSanteiItemCount,
                                     santeiInf.CurrentMonthSanteiItemSum,
                                     santeiInfDetailList
                                ));
        }
        return (santeiResultList, byomeiList);
    }

    private SanteiInfDetailOutputItem ConvertToSanteiInfDetailOutputItem(SanteiInfDetailModel modelDetail)
    {
        return new SanteiInfDetailOutputItem(
                   modelDetail.Id,
                   modelDetail.PtId,
                   modelDetail.ItemCd,
                   modelDetail.EndDate,
                   modelDetail.KisanSbt,
                   modelDetail.KisanDate,
                   modelDetail.Byomei,
                   modelDetail.HosokuComment,
                   modelDetail.Comment);
    }
}
