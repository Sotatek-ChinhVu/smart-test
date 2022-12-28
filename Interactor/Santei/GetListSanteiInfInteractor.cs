using Domain.Enum;
using Domain.Models.Diseases;
using Domain.Models.Santei;
using Helper.Common;
using UseCase.Santei.GetListSanteiInf;

namespace Interactor.Santei;

public class GetListSanteiInfInteractor : IGetListSanteiInfInputPort
{
    private readonly ISanteiInfRepository _santeiInfRepository;
    private readonly IPtDiseaseRepository _ptDiseaseRepository;

    public GetListSanteiInfInteractor(ISanteiInfRepository santeiInfRepository, IPtDiseaseRepository ptDiseaseRepository)
    {
        _santeiInfRepository = santeiInfRepository;
        _ptDiseaseRepository = ptDiseaseRepository;
    }

    public GetListSanteiInfOutputData Handle(GetListSanteiInfInputData input)
    {
        try
        {
            var listByomeis = _santeiInfRepository.GetListSanteiByomeis(input.HpId, input.PtId, input.SinDate, input.HokenPid);
            var listSanteiInf = _santeiInfRepository.GetListSanteiInfModel(input.HpId, input.PtId, input.SinDate);
            var listSanteiInfDetail = _santeiInfRepository.GetListSanteiInfDetailModel(input.HpId, input.PtId, input.SinDate);

            var result = ConvertToResult(input.SinDate, listSanteiInf, listSanteiInfDetail, listByomeis);

            return new GetListSanteiInfOutputData(GetListSanteiInfStatus.Successed, result.Item1, DicAlertTermCombobox(), DicKisanKbnCombobox(), result.Item2);
        }
        catch (Exception)
        {
            return new GetListSanteiInfOutputData(GetListSanteiInfStatus.Failed);
        }
        finally
        {
            _santeiInfRepository.ReleaseResource();
        }
    }

    private Dictionary<int, string> DicAlertTermCombobox()
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

    private Dictionary<int, string> DicKisanKbnCombobox()
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

    private Tuple<List<SanteiInfOutputItem>, List<string>> ConvertToResult(int sinDate, List<SanteiInfModel> listSanteiInf, List<SanteiInfDetailModel> listSanteiInfDetail, List<string> listByomeis)
    {
        List<SanteiInfOutputItem> listSanteiInfResult = new();
        foreach (var santeiInf in listSanteiInf)
        {
            int kisanDate = 0;
            int dayCount = 0;

            // kisanDate
            var listSanteiInfDetails = listSanteiInfDetail.Where(item => item.ItemCd == santeiInf.ItemCd)
                                                          .Select(item => ConvertToSanteiInfDetailOutputItem(item))
                                                          .OrderBy(item => item.EndDate)
                                                          .ThenBy(item => item.SeqNo)
                                                          .ToList();

            foreach (var item in listSanteiInfDetails)
            {
                // Add byomei to byomei List
                if (listByomeis.FirstOrDefault(byomei => byomei.Equals(item.Byomei)) == null && !string.IsNullOrEmpty(item.Byomei))
                {
                    listByomeis.Add(item.Byomei);
                }

                if (sinDate > item.EndDate) continue;
                if (item.KisanDate > kisanDate)
                {
                    kisanDate = item.KisanDate;
                }
            }

            // dayCount
            int targetDateInt = kisanDate != 0 ? kisanDate : santeiInf.LastOdrDate;
            dayCount = CIUtil.GetSanteInfDayCount(sinDate, targetDateInt, santeiInf.AlertTerm);
            listSanteiInfResult.Add(
                                    new SanteiInfOutputItem(
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
                                        listSanteiInfDetails
                                    ));
        }
        return Tuple.Create(listSanteiInfResult, listByomeis);
    }

    private SanteiInfDetailOutputItem ConvertToSanteiInfDetailOutputItem(SanteiInfDetailModel modelDetail)
    {
        return new SanteiInfDetailOutputItem(
                                                modelDetail.PtId,
                                                modelDetail.ItemCd,
                                                modelDetail.SeqNo,
                                                modelDetail.EndDate,
                                                modelDetail.KisanSbt,
                                                modelDetail.KisanDate,
                                                modelDetail.Byomei,
                                                modelDetail.HosokuComment,
                                                modelDetail.Comment
                                            );
    }
}
