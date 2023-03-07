﻿using Domain.Models.Accounting;
using Domain.Models.CalculateModel;
using Domain.Models.MstItem;
using Helper.Constants;
using Helper.Enum;
using Helper.Extension;
using Interactor.CalculateService;
using UseCase.Accounting.GetMeiHoGai;
using UseCase.Accounting.GetSinMei;

namespace Interactor.Accounting
{
    public class GetMeiHoGaiInteractor : IGetMeiHoGaiInputPort
    {
        private readonly IAccountingRepository _accountingRepository;
        private readonly ICalculateService _calculateService;

        public GetMeiHoGaiInteractor(IAccountingRepository accountingRepository, ICalculateService calculateService)
        {
            _accountingRepository = accountingRepository;
            _calculateService = calculateService;
        }

        public GetMeiHoGaiOutputData Handle(GetMeiHoGaiInputData inputData)
        {
            try
            {
                var raiinNos = _accountingRepository.GetRaiinNos(inputData.HpId, inputData.PtId, inputData.RaiinNo);

                if (!raiinNos.Any()) { return new GetMeiHoGaiOutputData(new(), new(), new(), GetMeiHoGaiStatus.NoData); }

                var sinMeiInputData = new GetSinMeiDtoInputData(raiinNos, inputData.PtId, inputData.SinDate, inputData.HpId);

                var sinMei = GetSinMei(sinMeiInputData);

                var sinHo = GetSinHo(sinMei);

                var sinGai = GetSinGai(inputData.HpId, inputData.PtId, raiinNos, sinMei);

                return new GetMeiHoGaiOutputData(sinMei, sinHo, sinGai, GetMeiHoGaiStatus.Successed);

            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }

        private List<SinMeiModel> GetSinMei(GetSinMeiDtoInputData sinMeiInputData)
        {
            Task<string> task = _calculateService.CallCalculate("SinMei/GetSinMeiList", sinMeiInputData);

            var result = task.Result;

            SinMeiDataModelDto sinMeiViewModelDto = Newtonsoft.Json.JsonConvert.DeserializeObject<SinMeiDataModelDto>(result);

            if (sinMeiViewModelDto == null) return new();

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
            if (listSinMei == null || listSinMei.Count <= 0) return new List<SinMeiModel>();

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

        private List<SinHoModel> GetSinHo(List<SinMeiModel> sinMeiModels)
        {
            var sinHo = new List<SinHoModel>();
            var codeCount = Enum.GetNames(typeof(CodeHoEnum)).Length;

            for (int i = 0; i < codeCount - 4; i++)
            {
                var codeHo = (CodeHoEnum)i;
                var sinMeiCode = sinMeiModels.Where(item => item.CdKbn == codeHo.AsString());
                sinHo.Add(new SinHoModel(
                                        codeHo.AsString(),
                                        sinMeiCode.Sum(item => item.SinHoTotalTen).AsDouble(),
                                        sinMeiCode.Where(item => item.FutanK1 == 1).Sum(item => item.SinHoTotalTen).AsDouble(),
                                        sinMeiCode.Where(item => item.FutanK2 == 1).Sum(item => item.SinHoTotalTen).AsDouble(),
                                        sinMeiCode.Where(item => item.FutanK3 == 1).Sum(item => item.SinHoTotalTen).AsDouble(),
                                        sinMeiCode.Where(item => item.FutanK4 == 1).Sum(item => item.SinHoTotalTen).AsDouble(),
                                        false));
            }

            var hoAll = new SinHoModel(
                                        CodeHoEnum.ALL.AsString(),
                                        sinHo.Sum(item => item.Point).AsDouble(),
                                        sinHo.Sum(item => item.PointKohi1).AsDouble(),
                                        sinHo.Sum(item => item.PointKohi2).AsDouble(),
                                        sinHo.Sum(item => item.PointKohi3).AsDouble(),
                                        sinHo.Sum(item => item.PointKohi4).AsDouble(),
                                        true);
            sinHo.Add(hoAll);

            return sinHo;
        }

        private List<SinGaiModel> GetSinGai(int hpId, long ptId, List<long> raiinNos, List<SinMeiModel> sinMeiModels)
        {
            var sinGai = new List<SinGaiModel>();
            var jihis = GetListJihiSbtMst(hpId);

            if (jihis != null && jihis.Count > 0)
            {
                foreach (var jihi in jihis)
                {
                    sinGai.Add(new SinGaiModel(jihi.Name,
                                               sinMeiModels.Where(item => item.JihiSbt == jihi.JihiSbt)
                                               .Sum(item => item.TotalKingaku).AsInteger(),
                                               false));
                }
            }

            sinGai.Add(new SinGaiModel("自費",
                                        sinMeiModels.Where(item => item.JihiSbt == 0 && item.SanteiKbn == 2).Sum(item => item.TotalKingaku).AsInteger(),
                                        false));

            sinGai.Add(new SinGaiModel("合計金額",
                                        sinMeiModels.Where(item => item.JihiSbt > 0 || (item.JihiSbt == 0 && item.SanteiKbn == 2)).Sum(item => item.TotalKingaku)
                                            .AsInteger(),
                                        true));

            int point = _accountingRepository.GetJihiOuttaxPoint(hpId, ptId, raiinNos);

            sinGai.Add(new SinGaiModel(SinHoConstant.CodeHoDic[nameof(CodeHoEnum.SZ)],
                                       point,
                                       true));

            return sinGai;
        }

        private List<JihiSbtMstModel> GetListJihiSbtMst(int hpId)
        {
            return _accountingRepository.GetListJihiSbtMst(hpId);
        }

        //public async Task<string> GetSinMei(string apiUrl, GetSinMeiDtoInputData sinMei)
        //{
        //    using (var httpClient = new HttpClient())
        //    {
        //        var jsonContent = JsonSerializer.Serialize(sinMei);
        //        var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        //        var response = await httpClient.PostAsync(apiUrl, content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseContent = await response.Content.ReadAsStringAsync();
        //            return responseContent;
        //        }
        //        else
        //        {
        //            throw new Exception("Failed: " + response.StatusCode);
        //        }
        //    }
        //}


    }
}
