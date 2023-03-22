using Domain.Models.Accounting;
using Domain.Models.Diseases;
using Domain.Models.MstItem;
using Helper.Constants;
using Helper.Extension;
using Interactor.CalculateService;
using System.Globalization;
using UseCase.Accounting.GetMeiHoGai;
using UseCase.Receipt.MedicalDetail;

namespace Interactor.Receipt
{
    public class GetMedicalDetailsInteractor : IGetMedicalDetailsInputPort
    {
        private readonly ICalculateService _calculateService;
        private readonly IMstItemRepository _mstItemRepository;

        public GetMedicalDetailsInteractor(ICalculateService calculateService, IMstItemRepository mstItemRepository)
        {
            _calculateService = calculateService;
            _mstItemRepository = mstItemRepository;
        }

        public GetMedicalDetailsOutputData Handle(GetMedicalDetailsInputData inputData)
        {
            try
            {
                var sinMeiInputData = new GetSinMeiDtoInputData(inputData.PtId, inputData.HpId, inputData.SinYm, inputData.HokenId, SinMeiModeConst.AccountingCard);
                var sinMei = GetSinMei(sinMeiInputData);
                var holiday = GetHolidayMst(inputData.HpId, inputData.SinYm);

                return new GetMedicalDetailsOutputData(sinMei, holiday, GetMedicalDetailsStatus.Successed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }

        private List<SinMeiModel> GetSinMei(GetSinMeiDtoInputData sinMeiInputData)
        {
            var sinMeiViewModelDto = _calculateService.GetSinMeiList(sinMeiInputData);

            if (!sinMeiViewModelDto.sinMeiList.Any()) return new();

            var sinMei = sinMeiViewModelDto.sinMeiList.Select(item => new SinMeiModel(
            item.SinId,
            string.Empty,
            item.ItemName,
            item.Suryo,
            item.UnitName,
            item.TenKai,
            item.TotalTen,
            item.TotalKingaku,
            item.Kingaku,
            item.FutanS,
            item.FutanK1,
            item.FutanK2,
            item.FutanK3,
            item.FutanK4,
            item.CdKbn,
            item.JihiSbt,
            item.EnTenKbn,
            item.SanteiKbn,
            item.InOutKbn,
            false,
            days: new List<int> { item.Day1, item.Day2, item.Day3, item.Day4, item.Day5, item.Day6, item.Day7, item.Day8, item.Day9, item.Day10,
                                  item.Day11, item.Day12, item.Day13, item.Day14, item.Day15, item.Day16, item.Day17, item.Day18, item.Day19, item.Day20,
                                  item.Day21, item.Day22, item.Day23, item.Day24, item.Day25, item.Day26, item.Day27, item.Day28, item.Day29, item.Day30,
                                  item.Day31 }
            )).ToList();
            return EditSinMei(sinMei);
        }
        private List<SinMeiModel> EditSinMei(List<SinMeiModel> listSinMei)
        {
            if (listSinMei == null || listSinMei.Count <= 0) return new();
            int oldSinId = 0;
            var result = new List<SinMeiModel>();
            foreach (SinMeiModel sinMei in listSinMei)
            {
                if (sinMei.SinId != 0 && sinMei.SinId != oldSinId)
                {
                    oldSinId = sinMei.SinId;
                    sinMei.SinIdBinding = oldSinId.AsString();
                }
                result.Add(sinMei);
                if (sinMei == listSinMei.Last()) continue;
                SinMeiModel nextSinMei = listSinMei[listSinMei.IndexOf(sinMei) + 1];
                if (nextSinMei.SinId != 0 && nextSinMei.SinId != oldSinId)
                {
                    result.Add(new SinMeiModel(sinMei.SinId,
                    sinMei.SinIdBinding,
                    sinMei.ItemName,
                    sinMei.Suryo,
                    sinMei.UnitName,
                    sinMei.TenKai,
                    sinMei.TotalTen,
                    sinMei.TotalKingaku,
                    sinMei.Kingaku,
                    sinMei.FutanS,
                    sinMei.FutanK1,
                    sinMei.FutanK2,
                    sinMei.FutanK3,
                    sinMei.FutanK4,
                    sinMei.CdKbn,
                    sinMei.JihiSbt,
                    sinMei.EnTenKbn,
                    sinMei.SanteiKbn,
                    sinMei.InOutKbn,
                    true,
                    sinMei.Days
                    ));
                }
            }
            return result;
        }
        private Dictionary<int, string> GetHolidayMst(int hpId, int SinYm)
        {
            var holidayOfMonth = new Dictionary<int, string>();
            var listHolidayMst = _mstItemRepository.FindHolidayMstList(hpId, SinYm * 100 + 1, SinYm * 100 + 99);
            var listHolidayMstHashSet = new HashSet<int>(listHolidayMst.Select(item => item.SinDate));
            DateTime.TryParseExact(SinYm.AsString(), "yyyyMM",
            CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime currentTime);
            int countDay = DateTime.DaysInMonth(currentTime.Year, currentTime.Month);
            for (int i = 0; i < countDay; i++)
            {
                var date = currentTime.AddDays(i);
                if (date.DayOfWeek == DayOfWeek.Sunday || listHolidayMstHashSet.Contains(SinYm * 100 + i + 1))
                {
                    holidayOfMonth.Add(i + 1, "red");
                }
                else if (date.DayOfWeek == DayOfWeek.Saturday)
                {
                    holidayOfMonth.Add(i + 1, "blue");
                }
            }
            return holidayOfMonth;
        }

    }
}
