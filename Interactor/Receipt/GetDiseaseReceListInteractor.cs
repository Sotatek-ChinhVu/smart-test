using Domain.Constant;
using Domain.Enum;
using Domain.Models.Diseases;
using Domain.Models.UserConf;
using Helper.Common;
using System.Text;
using UseCase.Receipt.GetDiseaseReceList;

namespace Interactor.Receipt;

public class GetDiseaseReceListInteractor : IGetDiseaseReceListInputPort
{
    private readonly IPtDiseaseRepository _ptDiseaseRepository;
    private readonly IUserConfRepository _userConfRepository;

    public GetDiseaseReceListInteractor(IPtDiseaseRepository ptDiseaseRepository, IUserConfRepository userConfRepository)
    {
        _ptDiseaseRepository = ptDiseaseRepository;
        _userConfRepository = userConfRepository;
    }

    public GetDiseaseReceListOutputData Handle(GetDiseaseReceListInputData inputData)
    {
        try
        {
            int year = inputData.SinYm / 100;
            int month = inputData.SinYm % 100;
            int firstDay = 1;
            int lastDay = DateTime.DaysInMonth(year, month);
            int firstDate = year * 10000 + month * 100 + firstDay;
            int lastDate = year * 10000 + month * 100 + lastDay;

            var patientDiseaseList = _ptDiseaseRepository.GetPatientDiseaseList(inputData.HpId, inputData.PtId, lastDate, inputData.HokenId, DiseaseViewType.FromReceiptCheck, false, false);
            patientDiseaseList = patientDiseaseList.Where(item => item.IsDeleted == 0
                                                                  && item.StartDate <= lastDate
                                                                  && item.IsNodspRece == 0                            // Rece Kisai (レセ記載)
                                                                  && (item.TenkiKbn == TenkiKbnConst.Continued        // Continuous disease (継続病名)
                                                                      || item.TenkiDate >= firstDate))                // Tenki disease (転帰した病名)
                                                   .ToList();

            if (_userConfRepository.GetSettingValue(inputData.HpId, inputData.UserId, 94001, 0) == 0)
            {
                patientDiseaseList = patientDiseaseList.OrderBy(p => p.StartDate)
                                                       .ThenBy(p => p.TenkiDate)
                                                       .ThenBy(p => p.ReceTenkiKbn)
                                                       .ThenByDescending(p => p.SyubyoKbn)
                                                       .ThenBy(p => p.SortNo)
                                                       .ToList();
            }
            else
            {
                patientDiseaseList = patientDiseaseList.OrderByDescending(p => p.SyubyoKbn)
                                                       .ThenBy(p => p.StartDate)
                                                       .ThenBy(p => p.TenkiDate)
                                                       .ThenBy(p => p.ReceTenkiKbn)
                                                       .ThenBy(p => p.SortNo)
                                                       .ToList();
            }

            var displayByomeiDateType = _userConfRepository.GetSettingValue(inputData.HpId, inputData.UserId, 100001);
            List<DiseaseReceOutputItem> result = patientDiseaseList.Select(item => ConvertToDiseaseReceItem(item, displayByomeiDateType)).ToList();
            return new GetDiseaseReceListOutputData(result, GetDiseaseReceListStatus.Successed);
        }
        finally
        {
            _ptDiseaseRepository.ReleaseResource();
            _userConfRepository.ReleaseResource();
        }
    }

    private DiseaseReceOutputItem ConvertToDiseaseReceItem(PtDiseaseModel model, int displayByomeiDateType)
    {
        StringBuilder fullByomei = new();
        StringBuilder byomei = new();
        StringBuilder freFixString = new();
        StringBuilder sufFixString = new();
        foreach (var item in model.PrefixSuffixList)
        {
            if (item.Code.StartsWith("8"))
            {
                sufFixString.Append(item.Name);
                continue;
            }
            freFixString.Append(item.Name);
        }
        byomei = freFixString.Append(model.Byomei).Append(sufFixString);

        if (model.SyubyoKbn == 1)
        {
            fullByomei.Append("(主)");
        }

        switch (model.SikkanKbn)
        {
            case 3:
                fullByomei.Append("(皮1)");
                break;
            case 4:
                fullByomei.Append("(皮2)");
                break;
            case 5:
                fullByomei.Append("(特)");
                break;
            case 7:
                fullByomei.Append("(て)");
                break;
            case 8:
                fullByomei.Append("(特て)");
                break;
        }

        if (model.NanbyoCd == 9)
        {
            fullByomei.Append("(難)");
        }

        fullByomei.Append(byomei);

        if (!string.IsNullOrWhiteSpace(model.HosokuCmt))
        {
            fullByomei.Append("(").Append(model.HosokuCmt).Append(")");
        }

        return new DiseaseReceOutputItem(
                    fullByomei.ToString(),
                    displayByomeiDateType == 0 ? CIUtil.SDateToShowSDate(model.StartDate) : CIUtil.SDateToShowWDate(model.StartDate),
                    model.TenkiKbn == 0 ? string.Empty : TenkiKbnConst.DisplayedTenkiKbnDict[model.TenkiKbn],
                    displayByomeiDateType == 0 ? CIUtil.SDateToShowSDate(model.TenkiDate) : CIUtil.SDateToShowWDate(model.TenkiDate)
                );
    }
}
