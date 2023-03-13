using Domain.Models.Accounting;
using Domain.Models.MstItem;
using Helper.Extension;
using Interactor.CalculateService;
using System.Globalization;
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
                var sinMeiInputData = new GetSinMeiAccountingCardDtoInputData(inputData.HpId, inputData.PtId, inputData.SinYm, inputData.HokenId);
                var sinMei = GetSinMei(sinMeiInputData);
                var holiday = GetHolidayMst(inputData.HpId, inputData.SinYm);

                return new GetMedicalDetailsOutputData(sinMei, holiday, GetMedicalDetailsStatus.Successed);
            }
            finally
            {
                _mstItemRepository.ReleaseResource();
            }
        }

        private List<SinMeiModel> GetSinMei(GetSinMeiAccountingCardDtoInputData sinMeiInputData)
        {
            var sinMeiViewModelDto = _calculateService.GetSinMeiAccountingCard(sinMeiInputData);

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
                                                                        false
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
                                                true));
                }
            }

            return result;
        }

        private Dictionary<int, string> GetHolidayMst(int hpId, int SinYm)
        {
            var holidayOfMonth = new Dictionary<int, string>();

            var listHolidayMst = _mstItemRepository.FindHolidayMstList(hpId, SinYm * 100 + 1, SinYm * 100 + 99);

            DateTime.TryParseExact(SinYm.AsString(), "yyyyMM",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime currentTime);

            int countDay = DateTime.DaysInMonth(currentTime.Year, currentTime.Month);

            for (int i = 0; i < countDay; i++)
            {
                var date = currentTime.AddDays(i);
                if (date.DayOfWeek == DayOfWeek.Sunday || listHolidayMst.Any(item => item.SinDate == (SinYm * 100 + i + 1)))
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
