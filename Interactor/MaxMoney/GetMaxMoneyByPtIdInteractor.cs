using Domain.Models.MaxMoney;
using UseCase.MaxMoney.GetMaxMoneyByPtId;

namespace Interactor.MaxMoney
{
    public class GetMaxMoneyByPtIdInteractor : IGetMaxMoneyByPtIdInputPort
    {
        private readonly IMaxmoneyReposiory _maxmoneyReposiory;

        public GetMaxMoneyByPtIdInteractor(IMaxmoneyReposiory maxmoneyReposiory) => _maxmoneyReposiory = maxmoneyReposiory;

        public GetMaxMoneyByPtIdOutputData Handle(GetMaxMoneyByPtIdInputData inputData)
        {
            var result = new List<MaxMoneyModel>();
            try
            {
                if (inputData.PtId < 0)
                    return new GetMaxMoneyByPtIdOutputData(result, GetMaxMoneyByPtIdStatus.InvalidPtId);

                if (inputData.HpId < 0)
                    return new GetMaxMoneyByPtIdOutputData(result, GetMaxMoneyByPtIdStatus.InvalidHpId);

                if (inputData.SinDate <= 0)
                    return new GetMaxMoneyByPtIdOutputData(result, GetMaxMoneyByPtIdStatus.InvalidSinDate);

                var datas = _maxmoneyReposiory.GetListLimitModel(inputData.PtId, inputData.HpId);

                if (datas != null && datas.Any()){
                    datas = datas.Where(x => x.SinDateM == inputData.SinDateM && x.SinDateY == inputData.SinDateY).ToList();}
                else
                    datas = new List<LimitListModel>();

                var kohiIdList = datas.GroupBy(x => x.KohiId).Select(x => x.Key);

                var infoHokens = _maxmoneyReposiory.GetListInfoHokenMoneys(inputData.HpId, inputData.PtId, kohiIdList, inputData.SinYM);

                foreach (int kohiId in kohiIdList)
                {
                    var listLimit = datas.Where(x => x.KohiId == kohiId);
                    var infoHoken = infoHokens.FirstOrDefault(x => x.HokenKohiId == kohiId) ?? new MaxMoneyInfoHokenModel(0, 0, 0, 0, 0, 0, string.Empty, string.Empty, 0, 0, 0, 0);

                    int rate = infoHoken.Rate;
                    int sinDateYM = infoHoken.SinYM;
                    string displaySinDateYM = infoHoken.DisplaySinDateYM;
                    bool isLimitMaxMoney = infoHoken.IsLimitMaxMoney;
                    int gendoGaku = infoHoken.GendoGaku;
                    int remainGendoGaku = 0;

                    if (rate == 0)
                        rate = infoHoken.FutanRate;

                    if (gendoGaku == 0)
                        gendoGaku = infoHoken.LimitFutan;

                    CalculateSort(listLimit);
                    CalculateTotalMoney(listLimit, isLimitMaxMoney, gendoGaku, ref remainGendoGaku);

                    result.Add(new MaxMoneyModel(kohiId, gendoGaku, remainGendoGaku, rate, infoHoken.Houbetsu
                        , infoHoken.HokenName, sinDateYM, infoHoken.FutanKbn, infoHoken.MonthLimitFutan, infoHoken.IsLimitListSum,
                        displaySinDateYM, listLimit));
                }

                if(result.Any())
                    return new GetMaxMoneyByPtIdOutputData(result, GetMaxMoneyByPtIdStatus.Successed);
                else
                    return new GetMaxMoneyByPtIdOutputData(result, GetMaxMoneyByPtIdStatus.DataNotFound);

            }
            finally
            {
                _maxmoneyReposiory.ReleaseResource();
            }
        }

        private void CalculateSort(IEnumerable<LimitListModel> listLimits)
        {
            int total = 0;
            foreach (var model in listLimits.Where(u => u.SinDateD != 0).OrderBy(u => u.SortKey).ToList())
            {
                total += 1;
                model.Sort = total;
            }
        }

        private void CalculateTotalMoney(IEnumerable<LimitListModel> listLimits, bool isLimitMaxMoney, int gendoGaku, ref int remainGendoGaku)
        {
            int total = 0;
            foreach (var model in listLimits.OrderBy(u => u.Sort))
            {
                total += model.FutanGaku;
                model.TotalMoney = total;
            }
            if (!isLimitMaxMoney && listLimits.Count() != 0)
            {
                remainGendoGaku = gendoGaku - listLimits.Max(u => u.TotalMoney);
            }
        }
    }
}
