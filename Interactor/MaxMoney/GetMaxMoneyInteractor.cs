using Domain.Models.MaxMoney;
using Helper.Extension;
using UseCase.MaxMoney.GetMaxMoney;

namespace Interactor.MaxMoney
{
    public class GetMaxMoneyInteractor : IGetMaxMoneyInputPort
    {
        private readonly IMaxmoneyReposiory _maxmoneyReposiory;

        public GetMaxMoneyInteractor(IMaxmoneyReposiory maxmoneyReposiory)
        {
            _maxmoneyReposiory = maxmoneyReposiory;
        }

        public GetMaxMoneyOutputData Handle(GetMaxMoneyInputData inputData)
        {
            if (inputData.HokenKohiId < 0)
                return new GetMaxMoneyOutputData(default, GetMaxMoneyStatus.InvalidKohiId);

            if (inputData.HpId < 0)
                return new GetMaxMoneyOutputData(default, GetMaxMoneyStatus.InvalidHpId);

            var listLimit = _maxmoneyReposiory.GetListLimitModel(inputData.PtId, inputData.HpId);
            if (listLimit != null)
                listLimit = listLimit.Where(x => x.KohiId == inputData.HokenKohiId
                                    && x.SinDateM == inputData.SinDateM
                                    && x.SinDateY == inputData.SinDateY).ToList();
            else
                listLimit = new List<LimitListModel>();

            var infoHoken = _maxmoneyReposiory.GetInfoHokenMoney(inputData.HpId, inputData.PtId, inputData.HokenKohiId, inputData.SinYM);

            if(infoHoken == null)
                return new GetMaxMoneyOutputData(default, GetMaxMoneyStatus.HokenKohiNotFound);

            if (infoHoken.MoneyLimitListFlag == 0)
                return new GetMaxMoneyOutputData(default, GetMaxMoneyStatus.HokenKohiNotValidToGet);

            int kohiId = infoHoken.HokenKohiId;
            int rate = infoHoken.Rate;
            int sinDateYM = infoHoken.SinYM;
            string displaySinDateYM = infoHoken.DisplaySinDateYM;
            bool isLimitMaxMoney = infoHoken.IsLimitMaxMoney;
            int gendoGaku = infoHoken.GendoGaku;
            bool isToltalGakuDisplay = infoHoken.IsToltalGakuDisplay;
            int remainGendoGaku = 0;

            if (rate == 0)
                rate = infoHoken.FutanRate;

            if (gendoGaku == 0)
                gendoGaku = infoHoken.LimitFutan;

            CalculateSort(listLimit);
            CalculateTotalMoney(listLimit, isLimitMaxMoney, gendoGaku,ref remainGendoGaku);

            MaxMoneyModel result = new MaxMoneyModel(kohiId, gendoGaku, remainGendoGaku, rate, infoHoken.Houbetsu
                , infoHoken.HokenName, sinDateYM, infoHoken.FutanKbn, infoHoken.MonthLimitFutan, infoHoken.IsLimitListSum,
                displaySinDateYM, listLimit);

            return new GetMaxMoneyOutputData(result, GetMaxMoneyStatus.Successed);
        }

        private void CalculateSort(List<LimitListModel> listLimits)
        {
            int total = 0;
            foreach (var model in listLimits.Where(u => u.SinDateD != 0).OrderBy(u => u.SortKey).ToList())
            {
                total += 1;
                model.Sort = total;
            }
        }

        public void CalculateTotalMoney(List<LimitListModel> listLimits, bool isLimitMaxMoney ,int gendoGaku, ref int remainGendoGaku)
        {
            int total = 0;
            foreach (var model in listLimits.OrderBy(u => u.Sort))
            {
                total += model.FutanGaku;
                model.TotalMoney = total;
            }
            if (!isLimitMaxMoney && listLimits.Count != 0)
            {
                remainGendoGaku = gendoGaku - listLimits.Max(u => u.TotalMoney);
            }
        }
    }
}
