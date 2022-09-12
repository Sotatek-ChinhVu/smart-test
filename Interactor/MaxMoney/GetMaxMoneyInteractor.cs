﻿using Domain.Models.MaxMoney;
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

            if (inputData.Rate == 0)
            {
                inputData.Rate = inputData.FutanRate;
            }

            if (inputData.GendoGaku == 0)
            {
                inputData.GendoGaku = inputData.LimitFutan;
            }

            int kohiId = inputData.HokenKohiId;
            int rate = inputData.Rate;
            int sinDateYM = inputData.SinYM;
            string displaySinDateYM = (inputData.SinYM / 100).AsString() + "/" + ((inputData.SinYM % 100 < 10) ? ("0" + (inputData.SinYM % 100).AsString()) : (inputData.SinYM % 100).AsString());
            bool isLimitMaxMoney = inputData.FutanKbn == 1 && inputData.MonthLimitFutan == 0;
            int gendoGaku = inputData.GendoGaku;
            string headerText = inputData.HoubetsuNumber + " " + inputData.HokenName;
            bool isToltalGakuDisplay = inputData.IsLimitListSum == 1;
            int remainGendoGaku = inputData.RemainGendoGaku;

            CalculateSort(listLimit);
            CalculateTotalMoney(listLimit, isLimitMaxMoney,);

            return new GetMaxMoneyOutputData(default, GetMaxMoneyStatus.Successed);
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

        public void CalculateTotalMoney(List<LimitListModel> listLimits, bool isLimitMaxMoney , ref int remainGendoGaku)
        {
            int total = 0;
            foreach (var model in listLimits.OrderBy(u => u.Sort))
            {
                total += model.FutanGaku;
                model.TotalMoney = total;
            }
            if (!isLimitMaxMoney)
            {
                remainGendoGaku = GendoGaku - MaxMoneys.GetFilteredDataCollection().Max(u => u.TotalMoney);
            }
        }
    }
}
